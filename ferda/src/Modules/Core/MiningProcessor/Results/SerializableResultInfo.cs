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
