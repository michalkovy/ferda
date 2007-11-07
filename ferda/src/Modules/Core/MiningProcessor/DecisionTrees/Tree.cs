// Tree.cs - representation of a GUHA decision tree
//
// Authors: Martin Ralbovsk� <martin.ralbovsky@gmail.com>
//
// Copyright (c) 2007 Martin Ralbovsk� 
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
        /// Attributes that were so far used in this tree. The
        /// attributes are represented by their formulas.
        /// </summary>
        private CategorialAttributeTrace[] usedAttributes;

        /// <summary>
        /// Attributes that were so far not used in this tree.
        /// The attributes are represented by their formulas.
        /// </summary>
        private CategorialAttributeTrace[] unusedAttributes;

        /// <summary>
        /// Depth of the tree
        /// </summary>
        private int depth;

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
        /// Attributes that were so far used in this tree. The
        /// attributes are represented by their formulas.
        /// </summary>
        public CategorialAttributeTrace[] UsedAttributes
        {
            get { return usedAttributes; }
            set { usedAttributes = value; }
        }

        /// <summary>
        /// Attributes that were so far not used in this tree.
        /// The attributes are represented by their formulas.
        /// </summary>
        public CategorialAttributeTrace[] UnusedAttributes
        {
            get { return unusedAttributes; }
            set { unusedAttributes = value; }
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
                return rootNode.IfRepresentation;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Determines if the tree contains at least one leaf, that
        /// has node frequency (number of items classified by the node)
        /// bigger then minimal node frequency from the parameter.
        /// Here, we do not count individual categories of nodes, but
        /// rather frequencies of whole nodes. 
        /// </summary>
        /// <param name="minimalNodeFrequency">Minimal node frequency</param>
        /// <param name="nodesWithMoreThanMinimalFrequency">
        /// In this parameter, the nodes that have bigger frequency than
        /// minimal are stored.
        /// </param>
        /// <returns>Iff the tree contains a node with frequency bigger
        /// than minimal</returns>
        public bool ContainsMoreThanMinimalFrequencyNodes(int minimalNodeFrequency,
            out List<Node> nodesWithMoreThanMinimalFrequency)
        {
            nodesWithMoreThanMinimalFrequency = new List<Node>();

            if (rootNode.Leaf)
            {
                if (rootNode.Frequency > minimalNodeFrequency)
                {
                    nodesWithMoreThanMinimalFrequency.Add(rootNode);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                foreach (Node node in rootNode.GetLeaves())
                {
                    if (node.Frequency > minimalNodeFrequency)
                    {
                        nodesWithMoreThanMinimalFrequency.Add(node);
                    }
                }

                return (nodesWithMoreThanMinimalFrequency.Count > 0);
            }
        }

        /// <summary>
        /// Determines if the tree has a node which fullfils the minimal node
        /// impurity criterion, that is that there exists a category in the node,
        /// which has number of items greater than the parameter.
        /// </summary>
        /// <param name="minimalImpurity">Mininmal node impurity parameter</param>
        /// <returns>True if there exist a node fullfilling the minimal node
        /// impurity criterion</returns>
        public bool HasMinimalImpurity(int minimalImpurity)
        {
            return rootNode.HasMinimalImpurity(minimalImpurity);
        }

        /// <summary>
        /// Creates a new object that is a copy of the current instance. 
        /// </summary>
        /// <returns>A new object that is a copy of this instance. </returns>
        public object Clone()
        {
            Tree tree = new Tree();
            tree.depth = depth;
            tree.usedAttributes = (CategorialAttributeTrace[]) usedAttributes.Clone();
            tree.unusedAttributes = (CategorialAttributeTrace[])unusedAttributes.Clone();
            tree.rootNode = (Node)rootNode.Clone();

            return tree;
        }

        /// <summary>
        /// Finds and returns node with specified identification in the tree
        /// </summary>
        /// <param name="attributeId">Identification of the attribute</param>
        /// <returns>Node with given attribute</returns>
        public Node FindNode(string attributeId)
        {
            return rootNode.FindNode(attributeId);
        }

        #endregion
    }
}
