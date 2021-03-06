// Node.cs - representation of a node for GUHA decision trees
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
using Ferda.Guha.MiningProcessor.Generation;
using Ferda.Guha.MiningProcessor.BitStrings;

namespace Ferda.Guha.MiningProcessor.DecisionTrees
{
    /// <summary>
    /// Representation of a node for GUHA decision trees
    /// </summary>
    public class Node : ICloneable
    {
        #region Private fields

        /// <summary>
        /// Iff the node is a leaf. When the node is a leaf, it contains
        /// categories of an attribute and for each category there is a corresponding
        /// classification category (category of the classification attribute). When
        /// the node is not a leaf, for each category there is an attached node that
        /// represents further branching of the tree.
        /// </summary>
        private bool leaf = true;

        /// <summary>
        /// The node represents a division by an attribute. This property determines
        /// the attribute and its formula.
        /// </summary>
        private CategorialAttributeTrace attribute;

        /// <summary>
        /// List of categories that are not subnodes in the parent
        /// node. These categories will be used for classification,
        /// the subnodes will be used for further distribution of the
        /// example to subnodes. When a subnode is created, the 
        /// corresponding category is removed form this list.
        /// </summary>
        private string[] subCategories;

        /// <summary>
        /// The dictionary is used when the tree is not a leaf. For each category of the
        /// attribute, there the dictionary returns a node (subtree) that represents
        /// further branching of the tree. 
        /// </summary>
        private Dictionary<string, Node> subNodes;

        /// <summary>
        /// A dictionary giving information about how the categories of 
        /// the node are classified. The key is name of category of this
        /// node (from the subCategories list), the value is node classification
        /// structure that classifies this node.
        /// </summary>
        private Dictionary<string, NodeClassification> classifiedCategories = null;

        /// <summary>
        /// Number representing how many items from the original database
        /// belong to this node, that is how many examples from the database
        /// was sorted out by the decision tree to this node.
        /// </summary>
        private long frequency;

        /// <summary>
        /// The base bit string of the current node. The 1's in the bit string represent items
        /// from the data table, that are true for this node. The 0's are items
        /// true for some other nodes of the tree.
        /// </summary>
        private IBitString baseBitString = new FalseBitString();

        /// <summary>
        /// Unique identifier of this node and clones of the node
        /// </summary>
        private double id;

        /// <summary>
        /// Attributes still unused in this node (that means in this path from
        /// the root of the tree to this node).
        /// </summary>
        private CategorialAttributeTrace[] unusedAttributes;

        #endregion

        #region Properties

        /// <summary>
        /// Iff the node is a leaf. When the node is a leaf, it contains
        /// categories of an attribute and for each category there is a corresponding
        /// classification category (category of the classification attribute). When
        /// the node is not a leaf, for each category there is an attached node that
        /// represents further branching of the tree.
        /// </summary>
        public bool Leaf
        {
            get { return leaf; }
            set { leaf = value; }
        }

        /// <summary>
        /// The node represents a division by an attribute. This property determines
        /// the attribute and its formula.
        /// </summary>
        public CategorialAttributeTrace Attribute
        {
            get { return attribute; }
            set { attribute = value; }
        }

        /// <summary>
        /// Attributes still unused in this node (that means in this path from
        /// the root of the tree to this node).
        /// </summary>
        public CategorialAttributeTrace[] UnusedAttributes
        {
            get { return unusedAttributes; }
            set { unusedAttributes = value; }
        }

        /// <summary>
        /// List of categories that are not subnodes in the parent
        /// node. These categories will be used for classification,
        /// the subnodes will be used for further distribution of the
        /// example to subnodes. When a subnode is created, the 
        /// corresponding category is removed form this list.
        /// </summary>
        public string[] SubCategories
        {
            get { return subCategories; }
            set { subCategories = value; }
        }

        /// <summary>
        /// The dictionary is used when the tree is not a leaf. For each category of the
        /// attribute, there the dictionary returns a node (subtree) that represents
        /// further branching of the tree. 
        /// </summary>
        public Dictionary<string, Node> SubNodes
        {
            get { return subNodes; }
            set { subNodes = value; }
        }

        /// <summary>
        /// Returns the IF representation of the node. The IF representation is a way
        /// to express form of decicison tree in textual forms. The decisions are
        /// written into IF THEN rules.
        /// </summary>
        public string IfRepresentation
        {
            get
            {
                string attributeName = 
                    attribute.Identifier.ToString();

                StringBuilder sb = new StringBuilder();

                foreach (string cat in subCategories)
                {
                    sb.Append(attributeName);
                    sb.Append(" = ");
                    sb.Append(cat);
                    if (classifiedCategories != null)
                    {
                        sb.Append(" : ");
                        sb.Append(classifiedCategories[cat].classificationCategory);
                        sb.Append(" (");
                        sb.Append(classifiedCategories[cat].noItemsInCategory);
                        sb.Append(',');
                        sb.Append(classifiedCategories[cat].noErrors);
                        sb.Append(")");
                    }
                    sb.Append('\n');
                }

                if (SubNodes != null)
                {
                    foreach (string cat in SubNodes.Keys)
                    {
                        sb.Append(attributeName);
                        sb.Append(" = ");
                        sb.Append(cat);
                        sb.Append('\n');

                        sb.Append(Tabulate(SubNodes[cat].IfRepresentation));
                    }
                }

                return sb.ToString();
            }
        }

        /// <summary>
        /// Number representing how many items from the original database
        /// belong to this node, that is how many examples from the database
        /// was sorted out by the decision tree to this node.
        /// </summary>
        public long Frequency
        {
            get { return frequency; }
            set { frequency = value; }
        }

        /// <summary>
        /// The base bit string of the current node. The 1's in the bit string represent items
        /// from the data table, that are true for this node. The 0's are items
        /// true for some other nodes of the tree.
        /// </summary>
        public IBitString BaseBitString
        {
            get { return baseBitString; }
            set { baseBitString = value; }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor of the class
        /// </summary>
        /// <param name="isLeaf">If the node is a leaf or not</param>
        public Node(bool isLeaf)
        {
            this.leaf = isLeaf;
            this.id = NodeCounter.GetNodeNumber();
        }

        #endregion

        #region Protected methods

        /// <summary>
        /// The method is used for the IF representation of the
        /// tree. It returns the IF representation of the sub
        /// categories.
        /// </summary>
        /// <returns>If representation of subcategories</returns>
        protected string SubCategoriesToIfString()
        {
            return string.Empty;
        }

        /// <summary>
        /// The method is used for the IF representation of
        /// the tree. It returns the IF representation of the
        /// sub nodes.
        /// </summary>
        /// <returns>If representation of the subnodes</returns>
        protected string SubNodesToIfString()
        {
            return string.Empty;
        }

        /// <summary>
        /// Appends each line of the <paramref name="source"/>
        /// string with '\t'. It is used for construction of
        /// if representation of the trees.
        /// </summary>
        /// <param name="source">Source string</param>
        /// <returns>Tabulated string</returns>
        protected static string Tabulate(string source)
        {
            string result = string.Empty;

            while (source.IndexOf('\n') != -1)
            {
                string tmp = '\t' + source.Substring(0, source.IndexOf('\n')+1);
                source = source.Remove(0, source.IndexOf('\n')+1);
                result += tmp;
            }

            return result;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Returns classification class of a category.
        /// </summary>
        /// <param name="category">Category</param>
        /// <returns>Classification of the category</returns>
        public string ClassificationOfCategory(string category)
        {
            return classifiedCategories[category].classificationCategory;
        }

        /// <summary>
        /// Returns leaf nodes, that are located in subnodes of this node.
        /// </summary>
        /// <returns>Leaf nodes located in subnodes</returns>
        public Node[] GetLeaves()
        {
            if (Leaf)
            {
                return new Node[] { this };
            }
            else
            {
                List<Node> result = new List<Node>();
                foreach (Node node in subNodes.Values)
                {
                    result.AddRange(node.GetLeaves());
                }
                return result.ToArray();
            }
        }

        /// <summary>
        /// Returns leaves of the tree and for each leaf, there
        /// is a numbes stating depth of the leaf in the tree.
        /// </summary>
        /// <returns>Leaves and their depth in the tree</returns>
        public Dictionary<Node, int> GetLeavesAndDepth()
        {
            Dictionary<Node, int> result = new Dictionary<Node, int>();

            if (Leaf)
            {
                result.Add(this, 1);
            }
            else
            {
                foreach (Node node in SubNodes.Values)
                {
                    Dictionary<Node, int> tmp = node.GetLeavesAndDepth();
                    foreach (Node n in tmp.Keys)
                    {
                        result.Add(n, tmp[n] + 1);
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Gets a bit string of one category. This string is created
        /// by ANDing the base bit string of the node and bit string of the
        /// category from the bit string generator.
        /// </summary>
        /// <param name="category">Category</param>
        /// <returns>Category bit string</returns>
        public async Task<IBitString> CategoryBitStringAsync(string category)
        {
            IBitString categoryOnlyBitString = await attribute.CategoryBitStringAsync(category).ConfigureAwait(false);
            return baseBitString.And(categoryOnlyBitString);
        }

        /// <summary>
        /// Gets the bit string that shows, how this node classified the
        /// classification category determined by
        /// <paramref name="classificationCategory"/> parameter. 
        /// </summary>
        /// <param name="classificationCategory">Classification category</param>
        /// <returns>Bit string showing how the node classified this
        /// category.</returns>
        public IBitString ClassifiedCategoryBitString(string classificationCategory)
        {
            IBitString result = EmptyBitString.GetInstance();
            //FalseBitString cannot be used as startup string for the result.
            //The reason is that there the category in parameter may have an empty
            //intersection with the classification, therefore empty bit string
            //would be returned (there would be a problem with negations of the bit
            //string and so on)
            bool first = true;

            //results from the sub categories of the node
            //classified categories at this point should not be null
            foreach (NodeClassification clas in classifiedCategories.Values)
            {
                if (clas.classificationCategory == classificationCategory)
                {
                    if (first)
                    {
                        result = clas.classificationBitString;
                        first = false;
                    }
                    else
                    {
                        result = result.Or(clas.classificationBitString);
                    }
                }
            }

            //adding the results from the subcategories.
            if (subNodes != null)
            {
                foreach (Node node in subNodes.Values)
                {
                    if (first)
                    {
                        result = node.ClassifiedCategoryBitString(classificationCategory);
                        first = false;
                    }
                    else
                    {
                        result =
                            result.Or(node.ClassifiedCategoryBitString(classificationCategory));
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Gets frequency of one category. The frequency is computed
        /// as sum of the category bit string from the bit string generator
        /// and base bit string of the node.
        /// The NonZeroBitsCount function is used instead of Sum, because the 
        /// number determines the count of examples classified by this category
        /// - which is an integer number.
        /// </summary>
        /// <param name="category">Category name</param>
        /// <returns>Category frequency</returns>
        public async Task<long> CategoryFrequencyAsync(string category)
        {
            return (await CategoryBitStringAsync(category)).NonZeroBitsCount;
        }

        /// <summary>
        /// Creates a new object that is a copy of the current instance. 
        /// </summary>
        /// <returns>A new object that is a copy of this instance. </returns>
        public object Clone()
        {
            Node node = new Node(leaf);
            node.attribute = attribute;
            node.subCategories = (string[]) subCategories.Clone();
            node.baseBitString = baseBitString;
            node.frequency = frequency;
            node.id = id;
            node.unusedAttributes = (CategorialAttributeTrace[])unusedAttributes.Clone();

            //the leaf does not contain any subnodes
            if (leaf)
            {
                return node;
            }

            //copying subNodes
            node.subNodes = new Dictionary<string, Node>(subNodes.Count);
            foreach (string category in subNodes.Keys)
            {
                node.subNodes.Add(category, (Node)subNodes[category].Clone());
            }

            return node;
        }

        /// <summary>
        /// Finds and returns node with specified identification in the node
        /// or its subNodes;
        /// </summary>
        /// <param name="node">The node to be found</param>
        /// <returns>Node with given attribute</returns>
        public Node FindNode(Node node)
        {
            if (this.id == node.id)
            {
                return this;
            }

            if (subNodes != null)
            {
                foreach (Node n in subNodes.Values)
                {
                    Node tmp = n.FindNode(node);
                    if (tmp != null)
                    {
                        return tmp;
                    }
                }
            }

            return null;
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
        public async Task InitNodeClassificationAsync(IBitString[] classificationBitStrings,
            string[] classificationCategories)
        {
            //if (classifiedCategories != null)
            //{
            //    throw new Exception("More times tree classification");
            //}
            classifiedCategories = new Dictionary<string, NodeClassification>();

            foreach (string category in subCategories)
            {
                //determining the classification category which bit string
                //has maximal interference with the selected category

                //The sum function is used instead of the NonZeroBitsCount function.
                //The goal is to determine which category suits the best for one
                //classification category. Therefore when con sidering fuzzy cases, 
                //it is a difference between membership degree 0.1 and 0.9
                float max = -1;
                int index = -1;
                IBitString categoryBitString = await CategoryBitStringAsync(category).ConfigureAwait(false);

                for (int i = 0; i < classificationCategories.Length; i++)
                {
                    float sum = categoryBitString.And(classificationBitStrings[i]).Sum;

                    if (sum > max)
                    {
                        max = sum;
                        index = i;
                    }
                }

                //we have the maximal category at index (index)
                //now creating the node classification structure
                NodeClassification nc = new NodeClassification();
                nc.classificationCategory = classificationCategories[index];
                nc.classificationBitString = categoryBitString;
                    //categoryBitString.And(classificationBitStrings[index]);
                nc.noItemsInCategory = categoryBitString.NonZeroBitsCount;
                nc.noErrors = 
                    categoryBitString.And(classificationBitStrings[index].Not()).NonZeroBitsCount;

                classifiedCategories.Add(category, nc);
            }

            //initializes also the subnodes
            if (subNodes != null)
            {
                foreach (Node n in subNodes.Values)
                {
                    await n.InitNodeClassificationAsync(classificationBitStrings,
                        classificationCategories).ConfigureAwait(false);
                }
            }
        }

        /// <summary>
        /// Determines the purity of a category in parameter
        /// (No of right classifications/no of items in da category)
        /// </summary>
        /// <param name="category">Category</param>
        /// <returns>Purity of a category</returns>
        public float CategoryPurity(string category)
        {
            NodeClassification classif = classifiedCategories[category];
            float result = (float)(classif.noItemsInCategory - classif.noErrors) /
                (float)classif.noItemsInCategory;
            return result;
        }

        #endregion
    }
}
