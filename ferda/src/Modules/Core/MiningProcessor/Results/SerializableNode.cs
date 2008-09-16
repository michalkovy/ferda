// SerializableNode.cs - serializable version of a decision tree node
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
using Ferda.Guha.MiningProcessor.DecisionTrees;
using System.IO;

namespace Ferda.Guha.MiningProcessor.Results
{
    /// <summary>
    /// Serializable version of the decision tree node. It contains the name of
    /// the attribute used for this node, information if node is a leaf,
    /// list of subcategories (categories that provide direct classification)
    /// and list of subnodes (categories that lead to other nodes). 
    /// </summary>
    [Serializable()]
    public class SerializableNode
    {
        /// <summary>
        /// Name of the attribute, which is represented by this node. Note, that
        /// this variable is filled with two different values. When staying in 
        /// the Mining processor, the name from the
        /// <see cref="Ferda.Guha.MiningProcessor.GuidAttributeNamePair"/> is used.
        /// When entering the ETree box, this is changed to select expression
        /// of the column for classification purposes. 
        /// </summary>
        public string AttributeLaterColumnName;

        /// <summary>
        /// Determines wheather this node is a leaf or it has other descendants
        /// </summary>
        public bool Leaf;

        /// <summary>
        /// List of categories that provide direct classification of the
        /// examples - do not lead to other nodes. 
        /// </summary>
        public string[] NodeCategories;

        /// <summary>
        /// Results of classification of <see cref="NodeCategories"/>. The order of the
        /// classification categories remains the same as in the <see cref="NodeCategories"/>.
        /// </summary>
        public string[] ClassificationCategories;

        /// <summary>
        /// Categories of the attribute, that lead to further nodes. The list
        /// is ordered so that it corresponds to <see cref="SubNodes"/> list.
        /// </summary>
        public string[] SubNodeCategories;

        /// <summary>
        /// The subnodes of this node leading to further classification.
        /// The list is ordered so that it corresponds to the 
        /// <see cref="SubNodeCategories"/> list. 
        /// </summary>
        public SerializableNode[] SubNodes;

        /// <summary>
        /// Parametr less constructor - needed for serialization.
        /// </summary>
        public SerializableNode()
        {
        }

        /// <summary>
        /// This constructor should be always used when constructing the class.
        /// </summary>
        /// <param name="node">Node from which the serialized form
        /// is created</param>
        public SerializableNode(Node node)
        {
            int i;
            Leaf = node.Leaf;
            NodeCategories = node.SubCategories;
            AttributeLaterColumnName = node.Attribute.Identifier.ToString();

            ClassificationCategories = new string[NodeCategories.Length];
            for (i = 0; i < NodeCategories.Length; i++)
            {
                ClassificationCategories[i] =
                    node.ClassificationOfCategory(NodeCategories[i]);
            }

            if (!Leaf)
            {
                SubNodeCategories = new string[node.SubNodes.Count];
                SubNodes = new SerializableNode[node.SubNodes.Count];
                i = 0;
                foreach (string catName in node.SubNodes.Keys)
                {
                    SubNodeCategories[i] = catName;
                    SerializableNode tmp =
                        new SerializableNode(node.SubNodes[catName]);
                    SubNodes[i] = tmp;
                    i++;
                }
            }
        }

        /// <summary>
        /// Serializes the specified input.
        /// </summary>
        /// <param name="input">The decision tree node to be serialized</param>
        /// <returns>Serialized version of the decision tree node</returns>
        public static string Serialize(SerializableNode input)
        {
            if (input == null)
                return null;
            XmlSerializer serializer = new XmlSerializer(typeof(SerializableNode));
            StringBuilder sb = new StringBuilder();
            using (StringWriter writer = new StringWriter(sb))
            {
                serializer.Serialize(writer, input);
                return sb.ToString();
            }
        }

        /// <summary>
        /// Deserializes the specified input (string representing a 
        /// decision tree node).
        /// </summary>
        /// <param name="input">The input string</param>
        /// <returns>Decision tree node result</returns>
        public static SerializableNode Deserialize(string input)
        {
            if (String.IsNullOrEmpty(input))
                return null;
            using (
            StringReader reader = new StringReader(input)
            )
            {
                XmlSerializer deserealizer = new XmlSerializer(typeof(SerializableNode));
                object deserealized = deserealizer.Deserialize(reader);
                return (SerializableNode)deserealized;
            }
        }

        /// <summary>
        /// Changes the contents of the field <see cref="AttributeLaterColumnName"/> 
        /// from name of the attribute (property "Name in Boolean attributes") to
        /// select expression of the column.
        /// </summary>
        /// <param name="node">Node to be changed.</param>
        /// <param name="conversion">Conversion dictionary, key key is attribute name,
        /// value is select expression
        /// </param>
        /// <returns>Changed node</returns>
        public void ChangeAttributeToColumnNames(Dictionary<string, string> conversion)
        {
            AttributeLaterColumnName = conversion[AttributeLaterColumnName];

            if (!Leaf)
            {
                foreach (SerializableNode subnode in SubNodes)
                {
                    subnode.ChangeAttributeToColumnNames(conversion);
                }
            }
        }
    }
}
