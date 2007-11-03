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
using Ferda.Guha.MiningProcessor.Generation;

namespace Ferda.Guha.MiningProcessor.DecisionTrees
{
    /// <summary>
    /// Representation of a node for GUHA decision trees
    /// </summary>
    public class Node
    {
        #region Private fields

        /// <summary>
        /// Iff the node is a leaf. When the node is a leaf, it contains
        /// categories of an attribute and for each category there is a corresponding
        /// classification category (category of the classification attribute). When
        /// the node is not a leaf, for each category there is an attached node that
        /// represents further branching of the tree.
        /// </summary>
        private bool leaf;

        /// <summary>
        /// The node represents a division by an attribute. This property determines
        /// the attribute and its formula.
        /// </summary>
        private CategorialAttributeTrace attribute;

        /// <summary>
        /// The dictionary is used when the tree is a leaf. For each category of the
        /// attribute, there the dictionary returns the classification category of 
        /// the classfication attribute.
        /// </summary>
        private Dictionary<string, string> subCategories;

        /// <summary>
        /// The dictionary is used when the tree is not a leaf. For each category of the
        /// attribute, there the dictionary returns a node (subtree) that represents
        /// further branching of the tree. 
        /// </summary>
        private Dictionary<string, Node> subNodes;

        /// <summary>
        /// Number representing how many items from the original database
        /// belong to this node, that is how many examples from the database
        /// was sorted out by the decision tree to this node.
        /// </summary>
        private long frequency;

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
        /// The dictionary is used when the tree is a leaf. For each category of the
        /// attribute, there the dictionary returns the classification category of 
        /// the classfication attribute.
        /// </summary>
        public Dictionary<string, string> SubCategories
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
                if (leaf)
                {
                    return SubCategoriesToIfString();
                }
                else
                {
                    return SubNodesToIfString();
                }
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
            if (!leaf)
            {
                throw Exceptions.WrongNodeTypeException();
            }
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
            if (leaf)
            {
                throw Exceptions.WrongNodeTypeException();
            }
            return string.Empty;
        }

        #endregion

        #region Public methods

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
        /// Determines if the node (or one of its subnodes) fullfils the minimal node
        /// impurity criterion, that is that there exists a category in the node,
        /// which has number of items greater than the parameter.
        /// </summary>
        /// <param name="minimalImpurity">Mininmal node impurity parameter</param>
        /// <returns>True if there exist a node fullfilling the minimal node
        /// impurity criterion</returns>
        public bool HasMinimalImpurity(int minimalImpurity)
        {
            return true;
        }

        #endregion
    }
}
