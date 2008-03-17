// SerializableOntologyMapping.cs - serializable version of ontology mapping
//
// Authors: Martin Zeman <martinzeman@email.cz>
//
// Copyright (c) 2008 Martin Zeman
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
    public struct MappedPair {
        /// <summary>
        /// Data Table Name
        /// </summary>
        public string DataTableName;
        
        /// <summary>
        /// Column Name
        /// </summary>
        public string DataTableColumnName;

        /// <summary>
        /// Ontology Entity Name
        /// </summary>
        public string OntologyEntityName;
    }

    /// <summary>
    /// Serializable version of ontology mapping. It consists of   
    /// </summary>
    [Serializable()]
    public class SerializableOntologyMapping
    {
        /// <summary>
        /// Mapped pairs array
        /// </summary>
        public MappedPair[] FerdaOntologyMapping;

        /// <summary>
        /// Serializes the specified input.
        /// </summary>
        /// <param name="input">The decision tree to be serialized</param>
        /// <returns>Serialized version of the decision tree</returns>
        public static string Serialize(SerializableOntologyMapping input)
        {
            if (input == null)
                return null;
            XmlSerializer serializer = new XmlSerializer(typeof(SerializableOntologyMapping));
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
        public static SerializableOntologyMapping Deserialize(string input)
        {
            if (String.IsNullOrEmpty(input))
                return null;
            using (
            StringReader reader = new StringReader(input)
            )
            {
                XmlSerializer deserealizer = new XmlSerializer(typeof(SerializableOntologyMapping));
                object deserealized = deserealizer.Deserialize(reader);
                return (SerializableOntologyMapping)deserealized;
            }
        }
    }
}
