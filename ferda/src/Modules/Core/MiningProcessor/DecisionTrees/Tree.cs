// Tree.cs - representation of a GUHA decision tree
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
using Ferda.Guha.MiningProcessor.Generation;
using Ferda.Guha.MiningProcessor.BitStrings;

namespace Ferda.Guha.MiningProcessor.DecisionTrees
{
    /// <summary>
    /// Representation of a GUHA decision tree.
    /// </summary>
    public class Tree : ICloneable
    {
        #region Private fields

        /// <summary>
        /// The root node
        /// </summary>
        private Node rootNode = null;

        /// <summary>
        /// Depth of the tree
        /// </summary>
        private int depth;

        /// <summary>
        /// Simple cache for computation of confusion matrix
        /// (in order to be computed only once).
        /// </summary>
        double[][] confusionMatrixCache = null;

        #endregion

        #region Properties

        /// <summary>
        /// The root node
        /// </summary>
        public Node RootNode
        {
            get { return rootNode; }
            set { rootNode = value; }
        }

        /// <summary>
        /// Depth of the tree
        /// </summary>
        public int Depth
        {
            get { return depth; }
            set { depth = value; }
        }

        /// <summary>
        /// Returns the IF representation of the tree. The IF representation is a way
        /// to express form of decicison tree in textual forms. The decisions are
        /// written into IF THEN rules.
        /// </summary>
        public string IfRepresentation
        {
            get
            {
                if (rootNode == null)
                {
                    return "Seed tree";
                }
                return rootNode.IfRepresentation;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Selects the nodes for further branching (they have at least
        /// one category, which has frequency biggre that minimal node frequency
        /// </summary>
        /// <param name="minimalNodeFrequency">Minimal node frequency</param>
        /// <param name="maximalTreeDepth">Maximal tree depth of the tree. If this 
        /// parameter is equal to -1, then all the leaves of the tree are considered
        /// for possible branching, if the parameter is greater then -1, only the
        /// leaves with shorter depth than the parameter are considered for
        /// possible branching.
        /// </param>
        /// <returns>Nodes for branching</returns>
        public async Task<List<Node>> NodesForBranchingAsync(int minimalNodeFrequency, int maximalTreeDepth)
        {
            List<Node> nodesWithMoreThanMinimalFrequency = new List<Node>();

            if (rootNode.Leaf)
            {
                //if the maximal tree depth is 1 and the root node is a leaf
                //(it has depth of 1, no other leafs should be branched
                if (maximalTreeDepth == 1)
                {
                    return new List<Node>();
                }

                bool hasCategories = false;

                foreach (string category in rootNode.SubCategories)
                {
                    if (await rootNode.CategoryFrequencyAsync(category).ConfigureAwait(false) > minimalNodeFrequency)
                    {
                        hasCategories = true;
                        break;
                    }
                }

                if (hasCategories)
                {
                    nodesWithMoreThanMinimalFrequency.Add(rootNode);
                }
                return nodesWithMoreThanMinimalFrequency;
            }
            else
            {
                Node[] potentialBranching;
                if (maximalTreeDepth == -1)
                {
                    potentialBranching = rootNode.GetLeaves();
                }
                else
                {
                    potentialBranching =
                        LeavesOfNotMaximalLength(maximalTreeDepth).ToArray();
                }

                foreach (Node node in potentialBranching)
                {
                    if (node.Frequency > minimalNodeFrequency)
                    {
                        nodesWithMoreThanMinimalFrequency.Add(node);
                    }
                }

                return nodesWithMoreThanMinimalFrequency;
            }
        }

        /// <summary>
        /// Returns leaves of the tree, that are in shorter depth than the maximal 
        /// tree depth
        /// </summary>
        /// <param name="maximalTreeDepth">Maximal tree depth</param>
        /// <returns>Leaves whose depth is shorter then maximal tree depth</returns>
        public List<Node> LeavesOfNotMaximalLength(int maximalTreeDepth)
        {
            Dictionary<Node, int> LandD = rootNode.GetLeavesAndDepth();
            List<Node> result = new List<Node>();

            foreach (Node n in LandD.Keys)
            {
                if (LandD[n] < maximalTreeDepth)
                {
                    result.Add(n);
                }
            }

            return result;
        }

        /// <summary>
        /// Determines, if the tree has leaf of given depth.
        /// </summary>
        /// <param name="givenDepth">Given depth</param>
        /// <returns>True iff the tree has leaf of given depth</returns>
        public bool HasLeafOfGivenDepth(int givenDepth)
        {
            if (rootNode == null)
            {
                return false;
            }

            Dictionary<Node, int> LandD = rootNode.GetLeavesAndDepth();

            foreach (Node n in LandD.Keys)
            {
                if (LandD[n] == givenDepth)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Creates a new object that is a copy of the current instance. 
        /// </summary>
        /// <returns>A new object that is a copy of this instance. </returns>
        public object Clone()
        {
            Tree tree = new Tree();
            tree.depth = depth;
            tree.rootNode = (Node)rootNode.Clone();

            return tree;
        }

        /// <summary>
        /// Finds and returns node with specified identification in the tree.
        /// </summary>
        /// <param name="node">The node</param>
        /// <returns>Node with given attribute</returns>
        public Node FindNode(Node node)
        {
            return rootNode.FindNode(node);
        }

        /// <summary>
        /// This procedure initializes the node classification. Beforehand,
        /// the <see cref="Ferda.Guha.MiningProcessor.DecisionTrees.Node.classifiedCategories"/>
        /// dictionary is null. Procedure fills the dictionary and
        /// afterwards each dictionary value contains node classification structure with
        /// the most present classification category. 
        /// </summary>
        /// <param name="classificationBitStrings">Bit strings of the classification 
        /// attribute</param>
        /// <param name="classificationCategories">Categories names of the
        /// classification attribute.</param>
        public Task InitNodeClassificationAsync(IBitString[] classificationBitStrings,
            string[] classificationCategories)
        {
            return rootNode.InitNodeClassificationAsync(classificationBitStrings,
                classificationCategories);
        }

        /// <summary>
        /// Returns the confusion matrix (matrix of positive and
        /// negative classification vs. the true or false of
        /// the examples for given classification)
        /// </summary>
        /// <param name="classificationBitStrings">Bit strings of the classification 
        /// attribute</param>
        /// <param name="classificationCategories">Categories names of the
        /// classification attribute.</param>
        /// <returns>Confusion matrix</returns>
        public double[][] ConfusionMatrix(IBitString[] classificationBitStrings,
            string[] classificationCategories)
        {
            if (confusionMatrixCache == null)
            {
                double[][] result = new double[2][];
                result[0] = new double[2];
                result[1] = new double[2];

                long truePositive = 0;
                long trueNegative = 0;
                long falsePositive = 0;
                long falseNegative = 0;

                for (int i = 0; i < classificationCategories.Length; i++)
                {
                    IBitString classifiedBitString = 
                        rootNode.ClassifiedCategoryBitString(classificationCategories[i]);
                    
                    //when classified bit string is empty bit string (which is that
                    //no row was classified by this category), the true positive
                    //field of the confusion matrix should be 0
                    if (!(classifiedBitString is EmptyBitString))
                    {
                        truePositive +=
                            classificationBitStrings[i].And(classifiedBitString).NonZeroBitsCount;
                    }
                    
                    falseNegative +=
                        classificationBitStrings[i].And(classifiedBitString.Not()).NonZeroBitsCount;

                    //when classified bit string is empty bit string (which is that
                    //no row was classified by this category), the false positive
                    //field of the confusion matrix should be 0
                    if (!(classifiedBitString is EmptyBitString))
                    {
                        falsePositive +=
                            classificationBitStrings[i].Not().And(classifiedBitString).NonZeroBitsCount;
                    }

                    trueNegative +=
                        classificationBitStrings[i].Not().And(classifiedBitString.Not()).NonZeroBitsCount;
                }

                result[0][0] = (double)truePositive;
                result[0][1] = (double)falsePositive;
                result[1][0] = (double)falseNegative;
                result[1][1] = (double)trueNegative;

                confusionMatrixCache = result;
            }

            return confusionMatrixCache;
        }

        #endregion
    }
}
