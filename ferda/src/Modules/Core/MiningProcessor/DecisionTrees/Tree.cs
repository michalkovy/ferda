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
                bool hasCategories = false;

                foreach (string category in rootNode.SubCategories)
                {
                    if (rootNode.CategoryFrequency(category) > minimalNodeFrequency)
                    {
                        hasCategories = true;
                        break;
                    }
                }

                if (hasCategories)
                {
                    nodesWithMoreThanMinimalFrequency.Add(rootNode);
                }
                return hasCategories;
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
        public void InitNodeClassification(IBitString[] classificationBitStrings,
            string[] classificationCategories)
        {
            rootNode.InitNodeClassification(classificationBitStrings,
                classificationCategories);
        }

        /// <summary>
        /// Returns the confusion matrix (matrix of positive and
        /// negative classification vs. the true or false of
        /// the examples for given classification
        /// </summary>
        /// <param name="classificationBitStrings">Bit strings of the classification 
        /// attribute</param>
        /// <param name="classificationCategories">Categories names of the
        /// classification attribute.</param>
        /// <returns>Confusion matrix</returns>
        public double[][] ConfusionMatrix(IBitString[] classificationBitStrings,
            string[] classificationCategories)
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
                truePositive +=
                    classificationBitStrings[i].And(
                    rootNode.ClassifiedCategoryBitString(classificationCategories[i])).Sum;
                trueNegative +=
                    classificationBitStrings[i].And(
                    rootNode.ClassifiedCategoryBitString(classificationCategories[i]).Not()).Sum;
                falsePositive +=
                    classificationBitStrings[i].Not().And(
                    rootNode.ClassifiedCategoryBitString(classificationCategories[i])).Sum;
                falseNegative +=
                    classificationBitStrings[i].Not().And(
                    rootNode.ClassifiedCategoryBitString(classificationCategories[i]).Not()).Sum;
            }

            result[0][0] = (double)truePositive;
            result[0][1] = (double)trueNegative;
            result[1][0] = (double)falsePositive;
            result[1][1] = (double)falseNegative;

            return result;
        }

        #endregion
    }
}
