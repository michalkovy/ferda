// ETreeMiningProcessor.cs - mining processor for the ETree procedure
//
// Authors: Martin Ralbovský <martin.ralbovsky@gmail.com>
//
// Copyright (c) 2007 Martin Ralbovský 
//
// This program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA

using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.IO;
using Ferda.Guha.MiningProcessor;
using Ferda.Guha.Math.Quantifiers;
using Ferda.ModulesManager;
using Ferda.Guha.MiningProcessor.DecisionTrees;
using Ferda.Guha.MiningProcessor.Generation;
using Ferda.Guha.MiningProcessor.Formulas;
using Ferda.Guha.MiningProcessor.BitStrings;
using Ferda.Guha.MiningProcessor.QuantifierEvaluator;
using Ferda.Guha.MiningProcessor.Results;

namespace Ferda.Guha.MiningProcessor.Miners
{
    /// <summary>
    /// When selecting attributes for further branching, we need to compute the
    /// values of chi square for individual attributes and then sort the values
    /// and pick best n. There can be a problem when two attributes have identically
    /// same chi-square values (one of the attributes would not be recognized).
    /// Therefore this structure is used.
    /// </summary>
    internal class IdValuePair : IComparer<IdValuePair> 
    {
        /// <summary>
        /// Identifier of the attribute
        /// </summary>
        public int Id;
        /// <summary>
        /// The attribute itself
        /// </summary>
        public double chiSquared;

        /// <summary>
        /// Compares two objects and returns a value indicating
        /// whether one is less than, equal to, or greater than the other. 
        /// </summary>
        /// <param name="x">The first object to compare. </param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>Minus if first is smaller, 0 if equal, plus if second is smaller</returns>
        public int Compare(IdValuePair x, IdValuePair y)
        {
            double result = x.chiSquared - y.chiSquared;
            if (result < 0)
            {
                return -1;
            }
            if (result > 0)
            {
                return 1;
            }
            return 0;
        }
    }

    /// <summary>
    /// The ETree procedure mining processor
    /// </summary>
    public class ETreeMiningProcessor : ProgressBarHandler
    {
        #region Private fields

        /// <summary>
        /// Minimal node impurity (algorithm parameter). Minimal node impurity is
        /// a condition for stopping growth of a tree. When sufficient amount 
        /// (determined by this parameter) of cases (items) belongs to one classification
        /// class in one node, the three is returned in output and stops growing. 
        /// </summary>
        private int minimalNodeImpurity;

        /// <summary>
        /// Minimal node frequency (algorithm parameter). Minimal node frequency is
        /// a condition for stopping growth of a tree. When a node does not contain
        /// minimal number of items (determined by this parameter), the three is returned
        /// in output and stops growing. 
        /// </summary>
        private int minimalNodeFrequency;

        /// <summary>
        /// Maximal tree depth (algorithm parameter). The total depth of the tree
        /// cannot exceed this value. 
        /// </summary>
        private int maximalTreeDepth;

        /// <summary>
        /// Number of attributes for branching (algorithm parameter). When determining
        /// the most suitable for tree branching, the remaining attributes are sorted
        /// by a criterion (here, chi squared) and the best N(determined by this 
        /// parameter) are used for branching.
        /// </summary>
        private int noAttributesForBranching;

        /// <summary>
        /// Maximal number of hypotheses to be generated by the miner. This parameter
        /// is present mainly because of the fact, that total number of relevant questions
        /// is not a good sign of progress of the task (in present way of approximating the
        /// number, it can easily reach infinity). 
        /// </summary>
        private long maxNumberOfHypotheses;

        /// <summary>
        /// Branching attributes (algorithm parameter)
        /// </summary>
        private CategorialAttributeTrace[] branchingAttributes;

        /// <summary>
        /// Target classification attribute (algorithm parameter)
        /// </summary>
        private CategorialAttributeTrace targetClassificationAttribute;

        /// <summary>
        /// Quatifiers to evaluate the tree quality 
        /// (algorithm parameter)
        /// </summary>
        private QuantifierBaseFunctionsPrx[] quantifiers;

        /// <summary>
        /// If output should contain only trees of desired
        /// length, or also shorter subtrees.
        /// </summary>
        private bool onlyFullTree;

        /// <summary>
        /// Count of all objects in the data matrix
        /// </summary>
        protected long allObjectsCount = -1;

        /// <summary>
        /// Structure holding information about the running of the
        /// task
        /// </summary>
        protected SerializableResultInfo resultInfo = null;

        /// <summary>
        /// The result of a run of this procedure
        /// </summary>
        private DecisionTreeResult result = null;

        private List<Tree> hypotheses = new List<Tree>();

        #endregion

        #region Properties

        /// <summary>
        /// Structure holding information about the running of the
        /// task
        /// </summary>        
        public SerializableResultInfo ResultInfo
        {
            get { return resultInfo; }
        }

        /// <summary>
        /// The result of a run of this procedure
        /// </summary>
        public DecisionTreeResult Result
        {
            get { return result; }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor of the class.
        /// </summary>
        /// <param name="branchingAttributes">
        /// Branching attributes (algorithm parameter)
        /// </param>
        /// <param name="targetClassificationAttribute">
        /// Target classification attribute (algorithm parameter)
        /// </param>
        /// <param name="quantifiers">
        /// Quatifiers to evaluate the tree quality 
        /// (algorithm parameter)
        /// </param>
        /// <param name="minimalNodeImpurity">
        /// Minimal node impurity (algorithm parameter). Minimal node impurity is
        /// a condition for stopping growth of a tree. When sufficient amount 
        /// (determined by this parameter) of cases (items) belongs to one classification
        /// class in one node, the three is returned in output and stops growing. 
        /// </param>
        /// <param name="minimalNodeFrequency">
        /// Minimal node frequency (algorithm parameter). Minimal node frequency is
        /// a condition for stopping growth of a tree. When a node does not contain
        /// minimal number of items (determined by this parameter), the three is returned
        /// in output and stops growing. 
        /// </param>
        /// <param name="maximalTreeDepth">
        /// Maximal tree depth (algorithm parameter). The total depth of the tree
        /// cannot exceed this value. 
        /// </param>
        /// <param name="noAttributesForBranching">
        /// Number of attributes for branching (algorithm parameter). When determining
        /// the most suitable for tree branching, the remaining attributes are sorted
        /// by a criterion (here, chi squared) and the best N(determined by this 
        /// parameter) are used for branching.
        /// </param>
        /// <param name="maxNumberOfHypotheses">
        /// Maximal number of hypotheses - decision trees. The parameter here is present
        /// mainly because we need to show progress in the progress bar (this cannot be 
        /// computed from the number of total relevant questions, because this number
        /// is often so high that is computed as infinity).
        /// </param>
        /// <param name="progressListener">The progress listener.</param>
        /// <param name="onlyFullTree">
        /// If output should contain only trees of desired
        /// length, or also shorter subtrees.
        /// </param>
        /// <param name="progressBarPrx">The progress bar PRX.</param>
        public ETreeMiningProcessor(
            CategorialAttribute[] branchingAttributes,
            CategorialAttribute targetClassificationAttribute,
            QuantifierBaseFunctionsPrx[] quantifiers,
            int minimalNodeImpurity,
            int minimalNodeFrequency,
            int maximalTreeDepth,
            int noAttributesForBranching,
            long maxNumberOfHypotheses,
            bool onlyFullTree,
            ProgressTaskListener progressListener,
            ProgressBarPrx progressBarPrx) : base(progressListener, progressBarPrx)
        {
            //getting the attribute traces
            this.branchingAttributes =
                MiningProcessorBase.CreateCategorialAttributeTrace(MarkEnum.BranchingAttributes,
                branchingAttributes, false);
            this.targetClassificationAttribute =
                MiningProcessorBase.CreateCategorialAttributeTrace(MarkEnum.TargetClassificationAttribute,
                targetClassificationAttribute, false);
            this.quantifiers = quantifiers;
            this.onlyFullTree = onlyFullTree;

            //checking corectness of the input parameters
            if (minimalNodeFrequency < 1)
            {
                throw Exceptions.NotMoreThanZeroException("MinimalNodeFrequency");
            }
            else
            {
                this.minimalNodeFrequency = minimalNodeFrequency;
            }

            if (minimalNodeImpurity < 1)
            {
                throw Exceptions.NotMoreThanZeroException("MinimalNodeImpurity");
            }
            else
            {
                this.minimalNodeImpurity = minimalNodeImpurity;

            }

            if (maximalTreeDepth < 1)
            {
                throw Exceptions.NotMoreThanZeroException("MaximalTreeDepth");
            }
            else
            {
                this.maximalTreeDepth = maximalTreeDepth;
            }

            if (noAttributesForBranching < 1)
            {
                throw Exceptions.NotMoreThanZeroException("MaximalTreeDepth");
            }
            else
            {
                this.noAttributesForBranching = noAttributesForBranching;
            }
            this.maxNumberOfHypotheses = maxNumberOfHypotheses;
        }

        #endregion

        /// <summary>
        /// The main procedure for generating hypotheses
        /// </summary>
        public void Trace()
        {
            //initialization of the resultinfo
            resultInfo = new SerializableResultInfo();
            resultInfo.StartTime = DateTime.Now;

            double relevantQuestionsCount = 0;
            long noOfVerifications = 0;
            long noOfHypotheses = 0;

            //Counting number of relevant questions
            //Setting the progres bar (if result is false, user stopped the task
            if (!ProgressSetValue(-1, "Counting number of relevant questions"))
            {
                return;
            }
            relevantQuestionsCount = CountRelevantQuestions();
            ComputeAllObjectsCount();


            //Intializing a new LIFO stack and planting a seed tree (no attribute tree)
            //inside;
            Stack<Tree> stack = new Stack<Tree>();
            Tree seed = MakeSeed();
            stack.Push(seed);

            Tree processTree;

            //basic algorithm for construction GUHA decision trees
            while (stack.Count > 0)
            {
                noOfVerifications++;
                processTree = stack.Pop();
                string ifr = processTree.IfRepresentation;

                if (QualityTree(processTree))
                {
                    noOfHypotheses++;
                    PutToOutPut(processTree);
                    //filling the progress bar with new values

                    if (noOfHypotheses == maxNumberOfHypotheses)
                    {
                        ResultFinish(relevantQuestionsCount, noOfVerifications,
                            noOfHypotheses);
                        return;
                    }
                }

                stack = Process(processTree, stack);

                if (!ProgressSetValue((float)noOfHypotheses / maxNumberOfHypotheses,
                    string.Format("Number of Verifications: {0}, Number of hypotheses: {1}, Stack size: {2}",
                             noOfVerifications,
                             noOfHypotheses,
                             stack.Count)))
                {
                    ResultFinish(relevantQuestionsCount, noOfVerifications,
                        noOfHypotheses);
                    return;
                }
            }

            ResultFinish(relevantQuestionsCount, noOfVerifications, noOfHypotheses);
        }

        #region Private methods

        /// <summary>
        /// Finishes filling of the result info structure according
        /// to the up-to-date values.
        /// </summary>
        /// <param name="relevantQuestionsCount">Number of relevant questions</param>
        /// <param name="noOfVerifications">Number of verifications carried out</param>
        /// <param name="noOfHypotheses">Number of hypotheses found</param>
        private void ResultFinish(double relevantQuestionsCount, long noOfVerifications,
            long noOfHypotheses)
        {
            resultInfo.EndTime = DateTime.Now;
            resultInfo.TotalNumberOfRelevantQuestions =
                relevantQuestionsCount;
            resultInfo.NumberOfVerifications = noOfVerifications;
            resultInfo.NumberOfHypotheses = noOfHypotheses;

            result = new DecisionTreeResult();
            result.TaskTypeEnum = TaskTypeEnum.ETree;
            result.AllObjectsCount = allObjectsCount;
            
            List<SerializableDecisionTree> hyps = 
                new List<SerializableDecisionTree>(hypotheses.Count);
            foreach (Tree tree in hypotheses)
            {
                SerializableDecisionTree ser = new SerializableDecisionTree();
                ser.IfRepresentation = tree.IfRepresentation;
                ser.ConfusionMatrix = tree.ConfusionMatrix(
                    targetClassificationAttribute.BitStrings, 
                    targetClassificationAttribute.CategoriesIds);
                hyps.Add(ser);
            }
            result.decisionTrees = hyps.ToArray();

        }

        /// <summary>
        /// Stores the tree in the output of the miner.
        /// </summary>
        /// <param name="processTree">The tree to be put to output</param>
        private void PutToOutPut(Tree processTree)
        {
            hypotheses.Add(processTree);
        }

        /// <summary>
        /// Determines, if the tree is good enought to be put to output.
        /// The tree must pass the quality test defined by all 4FT quantifiers
        /// connected to the task.
        /// </summary>
        /// <param name="processTree">The examined tree</param>
        /// <returns>If the tree souhld be put to output</returns>
        private bool QualityTree(Tree processTree)
        {
            bool rightLentgh;

            //checking the length of the tree
            if (onlyFullTree)
            {
                if (processTree.Depth == maximalTreeDepth)
                {
                    rightLentgh = true;
                }
                else
                {
                    rightLentgh = false;
                }
            }
            else
            {
                rightLentgh = true;
            }
            if (!rightLentgh)
            {
                return false;
            }

            //cannot determine the root node
            if (processTree.RootNode == null)
            {
                return false;
            }

            processTree.InitNodeClassification(
                targetClassificationAttribute.BitStrings,
                targetClassificationAttribute.CategoriesIds);

            QuantifierEvaluateSetting setting = new QuantifierEvaluateSetting();
            setting.contingencyTable = processTree.ConfusionMatrix(
                targetClassificationAttribute.BitStrings,
                targetClassificationAttribute.CategoriesIds);
            setting.denominator = 1;

            foreach (QuantifierBaseFunctionsPrx quant in quantifiers)
            {
                if (!quant.Compute(setting))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Processes one tree from the parameter. If there are
        /// any possilities for further branching of the tree, this
        /// procedure does so. At the and it puts all the newly created
        /// trees to the queue. 
        /// </summary>
        /// <param name="processTree">Tree to be processed</param>
        /// <param name="lifo">Structure holding temporary results</param>
        /// <returns>The stack where new trees were added.</returns>
        private Stack<Tree> Process(Tree processTree, Stack<Tree> lifo)
        {
            List<Node> nodesForBranching;

            //1. rule for further branching - if the three is already of the 
            //maximal length, do no further branching of the tree
            if (processTree.Depth == maximalTreeDepth)
            {
                return lifo;
            }

            //the seed tree is treated differently.
            if (processTree.RootNode == null)
            {
                CategorialAttributeTrace[] branching =
                    SelectAttributesForBranching(branchingAttributes, TrueBitString.GetInstance());
                
                //at this point, the fifo should always be empty.
                Debug.Assert(lifo.Count > 0);

                return AddAttributesFromSeed(branching);
            }

            //2. rule for further branching - if the tree contains node that
            //fulfills the minimal node impurity criterion (there exists a category
            //where number of items for that category is larger than given parameter)
            //then do no further branching of the tree
            if (processTree.HasMinimalImpurity(minimalNodeImpurity))
            {
                return lifo;
            }

            //3. rule for further branching - if the tree does not contain nodes
            //that fulfill minimal node frequency criterion (number of items in 
            //their nodes is smaller that the MinimalNodeFrequency parameter),
            //then do no further branching of the tree
            if (!processTree.ContainsMoreThanMinimalFrequencyNodes(
                minimalNodeFrequency, out nodesForBranching))
            {
                return lifo;
            }

            //for each node adding a new tree 
            foreach (Node node in nodesForBranching)
            {
                lifo = ProlongTree(processTree, node, lifo);
            }

            return lifo;
        }

        /// <summary>
        /// Prolongs the tree (in parameter <paramref name="processTree"/> in node
        /// (in parameter <paramref name="node"/>) and adds the tree into the
        /// <paramref name="lifo"/> queue, which is returned. From previous code, the
        /// node (not its categories) need to fullfill the minimal node frequency 
        /// criterion. In this procedure, only the categories that also fulfill the
        /// minimal node frequency criterion are bases of new nodes.
        /// </summary>
        /// <param name="processTree">Tree to be prolonged</param>
        /// <param name="node">Where to prolong the tree</param>
        /// <param name="lifo">Where to add newly created trees</param>
        /// <returns>The stack is with newly created trees is returned.</returns>
        private Stack<Tree> ProlongTree(Tree processTree, Node node, Stack<Tree> lifo)
        {
            //we are creating one subset of categories, that contains all the
            //categories which have higher frequency than minimal node frequency.
            List<string> rightCats = new List<string>();

            foreach (string category in node.SubCategories)
            {
                if (node.CategoryFrequency(category) >= minimalNodeFrequency)
                {
                    rightCats.Add(category);
                }            
            }
            rightCats = DeleteOneClassificationCategoryOnly(node, rightCats);
            
            if (rightCats.Count == 0)
            {
                return lifo;
            }

            //by anding the bit strings of individual categories, we
            //improve the accuracy for the attribute selection
            IBitString newBitString = FalseBitString.GetInstance();
            foreach (string category in rightCats)
            {
                newBitString = newBitString.Or(node.CategoryBitString(category));
            }

            //selecting which attributes to branch
            CategorialAttributeTrace[] branchingAttributes =
                SelectAttributesForBranching(node.UnusedAttributes,
                newBitString);

            //creating the individual subtrees
            foreach (CategorialAttributeTrace attribute in branchingAttributes)
            {
                Tree t = CreateTree(processTree, node, rightCats.ToArray(), attribute);
                lifo.Push(t);
            }

            return lifo;
        }

        /// <summary>
        /// From the categories of the node for branching, the procedure removes the
        /// categories (of the node) that contain only one classification category.
        /// </summary>
        /// <param name="node">Node where the category is present</param>
        /// <param name="catsForBranching">Categories for branching</param>
        /// <returns>New list with removed categories</returns>
        private List<string> DeleteOneClassificationCategoryOnly(Node node,
            List<string> catsForBranching)
        {
            List<string> tmp = new List<string>(catsForBranching);

            foreach (string category in tmp)
            {
                bool hasOneOnly = false;

                for (int i = 0; i < targetClassificationAttribute.BitStrings.Length; i++)
                {
                    long andSum = targetClassificationAttribute.BitStrings[i].And(
                        node.CategoryBitString(category)).Sum;
                    if (andSum == node.CategoryBitString(category).Sum)
                    {
                        hasOneOnly = true;
                        break;
                    }
                }

                if (hasOneOnly)
                {
                    catsForBranching.Remove(category);
                }
            }

            return catsForBranching;
        }

        /// <summary>
        /// Creates a new cloned tree out of the tree in <paramref name="processTree"/>
        /// and for given node <paramref name="node"/> makes a new node out of
        /// categories <paramref name="categories"/> using attribute
        /// <paramref name="attribute"/>.
        /// </summary>
        /// <param name="processTree">Tree to be cloned.</param>
        /// <param name="node">Node where noew node is to be created.</param>
        /// <param name="categories">Category which is to be replaced by a new leaf.</param>
        /// <param name="attribute">Attribute that will be used for creation of a new leaf</param>
        /// <returns>Newly created tree.</returns>
        private Tree CreateTree(Tree processTree, Node node, string[] categories, 
            CategorialAttributeTrace attribute)
        {
            Tree result = (Tree) processTree.Clone();
            result.Depth = processTree.Depth + 1;
            //finding the right node in the new tree
            Node n = result.FindNode(node);

            if (n == null)
            {
                throw new Exception("WRONGGGGGGGRRRRR");
            }

            //it is no longer a leaf
            n.Leaf = false;

            //removal of the category from tree categories
            List<string> newCats = new List<string>(n.SubCategories);
            foreach (string category in categories)
            {
                newCats.Remove(category);
            }
            n.SubCategories = newCats.ToArray();

            //creating a new nodes
            Dictionary<string,Node> newNodes = new Dictionary<string,Node>();
            foreach (string category in categories)
            {
                Node newNode = new Node(true);
                newNode.Attribute = attribute;
                newNode.BaseBitString = n.CategoryBitString(category).And(n.BaseBitString);
                newNode.Frequency = newNode.BaseBitString.Sum;
                newNode.SubCategories = attribute.CategoriesIds;

                //changing the unused attributes of the new node
                List<CategorialAttributeTrace> attr = 
                    new List<CategorialAttributeTrace>(node.UnusedAttributes);
                attr.Remove(attribute);
                newNode.UnusedAttributes = attr.ToArray();

                newNodes.Add(category,newNode);
            }

            n.SubNodes = newNodes;
            return result;
        }

        /// <summary>
        /// Makes new trees out of the seed tree. 
        /// </summary>
        /// <param name="attributes">The best attributes (judging by chi-squared kriterion)
        /// for construction of trees.
        /// </param>
        /// <returns>Processed queue of decision trees</returns>
        private Stack<Tree> AddAttributesFromSeed(CategorialAttributeTrace[] attributes)
        {
            Stack<Tree> fifo = new Stack<Tree>();
            Tree t;
            Node n;
            
            foreach (CategorialAttributeTrace attribute in attributes)
            {
                //constructing a node
                n = new Node(true);
                n.Attribute = attribute;
                n.BaseBitString = TrueBitString.GetInstance();
                n.Frequency = allObjectsCount;
                n.SubCategories = attribute.CategoriesIds;

                //setting the used and unused attributes
                List<CategorialAttributeTrace> attr = 
                    new List<CategorialAttributeTrace>(branchingAttributes);
                attr.Remove(attribute);
                n.UnusedAttributes = attr.ToArray();

                //construction of the tree
                t = new Tree();
                t.Depth = 1;
                t.RootNode = n;

                fifo.Push(t);
            }

            return fifo;
        }

        /// <summary>
        /// Makes a seed tree (a tree without any node, but with
        /// full attributes), a starting point of the algorithm
        /// </summary>
        /// <returns>Tree representing a seed</returns>
        private Tree MakeSeed()
        {
            Tree seed = new Tree();

            return seed;
        }

        /// <summary>
        /// <para>
        /// Counts number of relevant questions for this task setting. The formula
        /// can be upper approximated with variables <c>k</c>, <c>l</c> and <c>v</c> as
        /// <c>k*PRODUCT(i=1 to l)(k^v)</c>.
        /// </para>
        /// <para>
        /// where <c>k</c> stands for maximal number attributes for branching,
        /// <c>l</c> stands for maximal tree depth and <c>v</c> for maximal 
        /// number of categories from branching attributes.
        /// </para>
        /// </summary>
        /// <returns>Number of relevant questions</returns>
        private double CountRelevantQuestions()
        {
            //OLD VERSION
            int l;
            if (branchingAttributes.Length < maximalTreeDepth)
            {
                l = branchingAttributes.Length;
            }
            else
            {
                l = maximalTreeDepth;
            }

            //getting the categorial attribute with most categories
            int v = 1;
            foreach (CategorialAttributeTrace atrTrace in branchingAttributes)
            {
                if (atrTrace.NoOfCategories > v)
                {
                    v = atrTrace.NoOfCategories;
                }
            }

            return Ferda.Guha.Math.DecisionTrees.CountRelevantQuestions(noAttributesForBranching, l, v);
            
            //NEW VERSION
            //int EfNoAttr;
            //if (noAttributesForBranching > branchingAttributes.Length)
            //{
            //    EfNoAttr = branchingAttributes.Length;
            //}
            //else
            //{
            //    EfNoAttr = noAttributesForBranching;
            //}
            //int EfTD;
            //if (noAttributesForBranching > maximalTreeDepth)
            //{
            //    EfTD = maximalTreeDepth;
            //}
            //else
            //{
            //    EfTD = noAttributesForBranching;
            //}

            //return Ferda.Guha.Math.DecisionTrees.CountRelevantQuestionsNew(EfNoAttr, EfTD);
        }

        /// <summary>
        /// Select the best attributes for branching depending on the number of 
        /// attributes that can be used for branching (
        /// <see cref="noAttributesForBranching"/>),
        /// possible attributes for branching and base bit string from the node
        /// that is beeing branched.
        /// </summary>
        /// <param name="possibleAttributes">Array of possible attributes for branching</param>
        /// <param name="baseBitString">
        /// The base bit string of the node that is being branched. The 1's in 
        /// the bit string represent items
        /// from the data table, that are true for this node. The 0's are items
        /// true for some other nodes of the tree.
        /// </param>
        /// <returns>Attributes for branching</returns>
        private CategorialAttributeTrace[] SelectAttributesForBranching(
            CategorialAttributeTrace[] possibleAttributes,
            IBitString baseBitString)
        {
            //simple but effective optimizing
            if (noAttributesForBranching >= possibleAttributes.Length)
            {
                return possibleAttributes;
            }
            if (possibleAttributes.Length == 0)
            {
                return new CategorialAttributeTrace[0];
            }
            if (possibleAttributes.Length == 1)
            {
                return possibleAttributes;
            }

            //The r_{i} array
            int[] r;
            //The s_{j} array and array of (classificationAttribute AND baseBitString)
            //bit strings
            int[] s;
            IBitString[] sBitStrings;
            //The a_{i,j} array
            int[,] a;
            double chiSq;

            Dictionary<IdValuePair, CategorialAttributeTrace> dict = 
                new Dictionary<IdValuePair,CategorialAttributeTrace>();
            List<IdValuePair> values = new List<IdValuePair>();
            //for identfying IdValuePairs
            int id = 0;

            foreach (CategorialAttributeTrace attribute in possibleAttributes)
            {
                //computing the s_{j} array
                s = new int[targetClassificationAttribute.NoOfCategories];
                sBitStrings = new IBitString[targetClassificationAttribute.NoOfCategories];
                for (int i = 0; i < s.Length; i++)
                {
                    sBitStrings[i] = 
                        targetClassificationAttribute.BitStrings[i].And(baseBitString);
                    s[i] = sBitStrings[i].Sum;
                }

                FillChiSqData(attribute, sBitStrings, baseBitString, out r, out a);
                chiSq = Math.DecisionTrees.ChiSquared(r, s, a);
                
                //constructing a new IdValuePair
                IdValuePair idValue = new IdValuePair();
                idValue.Id = id;
                idValue.chiSquared = chiSq;

                dict.Add(idValue, attribute);
                values.Add(idValue);

                id++;
            }

            CategorialAttributeTrace[] result = new CategorialAttributeTrace[noAttributesForBranching];
            IdValuePair comparer = new IdValuePair();
            values.Sort(comparer);

            for (int i = 0; i < noAttributesForBranching; i++)
            {
                //adding in reverse order
                result[i] = dict[values[values.Count - 1 - i]];
            }

            return result;
        }

        /// <summary>
        /// Fills data structures needed for computation of chi-squared criterion
        /// of the attribute in parameter and classification attribute on the base
        /// bit string defined by parameter.
        /// The data are three integer arrays and are returned in parameters.
        /// </summary>
        /// <param name="attribute">Attribute for which the data are filled</param>
        /// <param name="classificationBitStrings">
        /// The bit strings of classification
        /// attribute. The are created as AND of the base bit string for the 
        /// examined node and the bit strings of classification attribute.
        /// They are passed as a parameter because of optimizing their computation.
        /// </param>
        /// <param name="baseBitString">
        /// We need the base bit string of the considered node, because otherwise the
        /// sum of r's and sum of s's would not fit. The bit strings of 
        /// <paramref name="attribute"/>need to be reduced by the base bit string of the
        /// node.
        /// </param>
        /// <param name="r">The <c>r_{i}</c> array corresponding to numbers of items of
        /// individual categories of the attribute.</param>
        /// <param name="a">The <c>a_{i,j}</c> array. Item on indes <c>(i,j)</c> is the
        /// number of items that are present in given node (determined by the
        /// base bit string) for classification category <c>j</c> and attribute
        /// category <c>i</c></param>
        private void FillChiSqData(CategorialAttributeTrace attribute,
            IBitString[] classificationBitStrings,
            IBitString baseBitString,
            out int[] r,
            out int[,] a)
        {
            r = new int[attribute.NoOfCategories];
            a = new int[attribute.NoOfCategories, classificationBitStrings.Length];

            for (int i = 0; i < r.Length; i++)
            {
                IBitString reducedAttrBS =
                    attribute.BitStrings[i].And(baseBitString);
                r[i] = reducedAttrBS.Sum;
            }

            for (int i = 0; i < r.Length; i++)
            {
                for (int j = 0; j < classificationBitStrings.Length; j++)
                {
                    a[i, j] = attribute.BitStrings[i].And(classificationBitStrings[j]).Sum;
                }
            }
        }

        /// <summary>
        /// Computes the count of all objects in the data matrix
        /// </summary>
        private void ComputeAllObjectsCount()
        {
            if (allObjectsCount > 0)
                return;

            foreach (IBitString s in targetClassificationAttribute.BitStrings)
            {
                allObjectsCount = s.Length;
                return;
            }
        }

        #endregion
    }
}