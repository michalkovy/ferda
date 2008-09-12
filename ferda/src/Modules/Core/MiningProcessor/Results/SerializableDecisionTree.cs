// SerializableDecisionTree.cs - serializable version of a decision tree
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
using System.Xml.Serialization;
using System.IO;

namespace Ferda.Guha.MiningProcessor.Results
{
    /// <summary>
    /// Serializable version of the decision tree. It contains the IF-representation
    /// and the contingency table for the quantifiers to be recomputed. 
    /// </summary>
    [Serializable()]
    public class SerializableDecisionTree
    {
        /// <summary>
        /// The If representation of the tree
        /// </summary>
        public string IfRepresentation;

        /// <summary>
        /// Confusion matrix of the decision tree.
        /// (matrix of positive and
        /// negative classification vs. the true or false of
        /// the examples for given classification).
        /// It is used similarily to the contingency tables
        /// of the other GUHA miners.
        /// </summary>
        public double[][] ConfusionMatrix;

        /// <summary>
        /// Depth of the tree
        /// </summary>
        public int TreeDepth;

        /// <summary>
        /// The root node of the tree
        /// </summary>
        public SerializableNode RootNode;

        /// <summary>
        /// Serializes the specified input.
        /// </summary>
        /// <param name="input">The decision tree to be serialized</param>
        /// <returns>Serialized version of the decision tree</returns>
        public static string Serialize(SerializableDecisionTree input)
        {
            if (input == null)
                return null;
            XmlSerializer serializer = new XmlSerializer(typeof(SerializableDecisionTree));
            StringBuilder sb = new StringBuilder();
            using (StringWriter writer = new StringWriter(sb))
            {
                serializer.Serialize(writer, input);
                return sb.ToString();
            }
        }

        /// <summary>
        /// Deserializes the specified input (string representing a
        /// <see cref="T:DecisionTreeResult"/>)
        /// </summary>
        /// <param name="input">The input string</param>
        /// <returns>Decision tree result</returns>
        public static SerializableDecisionTree Deserialize(string input)
        {
            if (String.IsNullOrEmpty(input))
                return null;
            using (
            StringReader reader = new StringReader(input)
            )
            {
                XmlSerializer deserealizer = new XmlSerializer(typeof(SerializableDecisionTree));
                object deserealized = deserealizer.Deserialize(reader);
                return (SerializableDecisionTree)deserealized;
            }
        }
    }
}
