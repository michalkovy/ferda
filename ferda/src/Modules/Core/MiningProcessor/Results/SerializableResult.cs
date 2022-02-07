// SerializableResult.cs - a result of a GUHA procedure run - serializable form
//
// Authors: Tomáš Kuchaø <tomas.kuchar@gmail.com>      
// Commented by: Martin Ralbovský <martin.ralbovsky@gmail.com>
//
// Copyright (c) 2006 Tomáš Kuchaø
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
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace Ferda.Guha.MiningProcessor.Results
{
    /// <summary>
    /// Represents serializable form of <see cref="T:Ferda.Guha.MiningProcessor.Results.Result"/>.
    /// (De)Serialization methods builds to/from the effective
    /// form of result (<see cref="T:Ferda.Guha.MiningProcessor.Results.Result"/>)
    /// (Design Pattern <c>Builder</c> is used).
    /// </summary>
    [Serializable()]
    public class SerializableResult
    {
        /// <summary>
        /// Count of all objects in analyzed data table.
        /// </summary>
        public long AllObjectsCount;

        /// <summary>
        /// Collection of serializabled hypotheses.
        /// </summary>
        public SerializableHypothesis[] Hypotheses;

        /// <summary>
        /// Type of the task. In effective form of this class
        /// (see <see cref="T:Ferda.Guha.MiningProcessor.Results.Result"/>)
        /// the type of the task indicates 
        /// list of used boolean/categorial attributes (method <code>GetSemanticMarks()</code>)
        /// and usage of one or two contingecy tables (field <code>TwoContingencyTables</code>).
        /// </summary>
        public TaskTypeEnum TaskTypeEnum;

        private static XmlSerializer _serializer = new XmlSerializer(typeof(SerializableResult), typeof(SerializableResult).GetNestedTypes());

        /// <summary>
        /// Initializes a new instance of the <see cref="T:SerializableResult"/> class.
        /// </summary>
        public SerializableResult()
        {
        }

        /// <summary>
        /// Builds the effective form of specified serializable input.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        private static Result build(SerializableResult input)
        {
            Result result = new Result();
            result.AllObjectsCount = input.AllObjectsCount;
            result.TaskTypeEnum = input.TaskTypeEnum;
            if (input.Hypotheses == null)
                result.Hypotheses = new List<Hypothesis>();
            else
            {
                result.Hypotheses = new List<Hypothesis>(input.Hypotheses.Length);
                for (int i = 0; i < input.Hypotheses.Length; i++)
                {
                    result.Hypotheses.Add(SerializableHypothesis.Build(input.Hypotheses[i]));
                }
            }
            return result;
        }

        /// <summary>
        /// Builds the serializable form of specified input.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        private static SerializableResult build(Result input)
        {
            SerializableResult result = new SerializableResult();
            result.AllObjectsCount = input.AllObjectsCount;
            result.TaskTypeEnum = input.TaskTypeEnum;
            if (input.Hypotheses == null)
                result.Hypotheses = new SerializableHypothesis[0];
            else
            {
                result.Hypotheses = new SerializableHypothesis[input.Hypotheses.Count];
                for (int i = 0; i < input.Hypotheses.Count; i++)
                {
                    result.Hypotheses[i] = SerializableHypothesis.Build(input.Hypotheses[i]);
                }
            }
            return result;
        }

        /// <summary>
        /// Serializes the specified input.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static string Serialize(Result input)
        {
            if (input == null)
                return null;
            SerializableResult serializable = SerializableResult.build(input);
            StringBuilder sb = new StringBuilder();
            using (
            StringWriter writer = new StringWriter(sb)
            )
            {
                
                _serializer.Serialize(writer, serializable);
                return sb.ToString();
            }
        }

        /// <summary>
        /// Deserializes the specified input.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static Result Deserialize(string input)
        {
            if (String.IsNullOrEmpty(input))
                return null;
            using (
            StringReader reader = new StringReader(input)
            )
            {
                object deserealized = _serializer.Deserialize(reader);
                SerializableResult serializable = (SerializableResult)deserealized;
                return SerializableResult.build(serializable);
            }
        }
    }
}
