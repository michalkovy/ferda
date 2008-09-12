// DecisionTreeeResult.cs - a result of a ETree procedure run
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
using Ferda.Guha.MiningProcessor.DecisionTrees;
using System.Xml.Serialization;
using System.IO;

namespace Ferda.Guha.MiningProcessor.Results
{
    /// <summary>
    /// The class represents a serializable result of one ETree procedure run.
    /// Contains information about the task and decision trees.
    /// </summary>
    [Serializable()]
    public class DecisionTreeResult
    {
        /// <summary>
        /// Count of all objects in analyzed data table.
        /// </summary>
        public long AllObjectsCount;

        /// <summary>
        /// Type of the task. In effective form of this class
        /// (see <see cref="T:Ferda.Guha.MiningProcessor.Results.Result"/>)
        /// the type of the task indicates 
        /// list of used boolean/categorial attributes (method <code>GetSemanticMarks()</code>)
        /// and usage of one or two contingecy tables (field <code>TwoContingencyTables</code>).
        /// </summary>
        public TaskTypeEnum TaskTypeEnum;

        /// <summary>
        /// Decision trees (GUHA hypotheses) that were created by this
        /// task in form of IF rules.
        /// </summary>
        public SerializableDecisionTree[] decisionTrees;

        /// <summary>
        /// Default constructor of the class.
        /// </summary>
        public DecisionTreeResult()
        {
        }

        /// <summary>
        /// Serializes the specified result
        /// </summary>
        /// <param name="result">The result</param>
        /// <returns>Result in serialized (string) form</returns>
        public static string Serialize(DecisionTreeResult result)
        {
            if (result == null)
                return null;
            XmlSerializer serializer = new XmlSerializer(typeof(DecisionTreeResult));
            StringBuilder sb = new StringBuilder();
            using (StringWriter writer = new StringWriter(sb))
            {
                serializer.Serialize(writer, result);
                return sb.ToString();
            }
        }

        /// <summary>
        /// Deserializes the specified input (string representing a
        /// <see cref="T:DecisionTreeResult"/>)
        /// </summary>
        /// <param name="input">The input string</param>
        /// <returns>Decision tree result</returns>
        public static DecisionTreeResult Deserialize(string input)
        {
            if (String.IsNullOrEmpty(input))
                return null;
            using (
            StringReader reader = new StringReader(input)
            )
            {
                XmlSerializer deserealizer = new XmlSerializer(typeof(DecisionTreeResult));
                object deserealized = deserealizer.Deserialize(reader);
                return (DecisionTreeResult)deserealized;
            }
        }
    }
}
