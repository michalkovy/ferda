// SerializableResultInfo.cs - information about run of a GUHA procedure
// - serializable form
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
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace Ferda.Guha.MiningProcessor.Results
{
    /// <summary>
    /// Serializable class which holds information about 
    /// task progress and result.
    /// </summary>
    [Serializable()]
    public class SerializableResultInfo
    {
        /// <summary>
        /// Other string info ... for some other purposes (like performance testing).
        /// </summary>
        public string OtherInfo;
        
        /// <summary>
        /// Total number of relevant questions specified by
        /// setting all boolean/categorial attributes.
        /// </summary>
        public double TotalNumberOfRelevantQuestions;
        
        /// <summary>
        /// Number of executed verifications.
        /// </summary>
        public long NumberOfVerifications;
        
        /// <summary>
        /// Number of relevant questions satisfactory all quantifiers.
        /// </summary>
        public long NumberOfHypotheses;
        
        /// <summary>
        /// Time of generation and verification start.
        /// </summary>
        public DateTime StartTime;
        
        /// <summary>
        /// Time of generation and verification end.
        /// </summary>
        public DateTime EndTime;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:SerializableResultInfo"/> class.
        /// </summary>
        public SerializableResultInfo()
        {
        }

        /// <summary>
        /// Serializes the specified input.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static string Serialize(SerializableResultInfo input)
        {
            if (input == null)
                return null;
            XmlSerializer serializer = new XmlSerializer(typeof(SerializableResultInfo));
            StringBuilder sb = new StringBuilder();
            using (
            StringWriter writer = new StringWriter(sb)
            )
            {
                serializer.Serialize(writer, input);
                return sb.ToString();
            }
        }

        /// <summary>
        /// Deserializes the specified input.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static SerializableResultInfo Deserialize(string input)
        {
            if (String.IsNullOrEmpty(input))
                return null;
            using (
            StringReader reader = new StringReader(input)
            )
            {
                XmlSerializer deserealizer = new XmlSerializer(typeof(SerializableResultInfo));
                object deserealized = deserealizer.Deserialize(reader);
                return (SerializableResultInfo)deserealized;
            }
        }
    }
}
