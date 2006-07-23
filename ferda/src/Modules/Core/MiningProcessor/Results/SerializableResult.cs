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
            XmlSerializer serializer = new XmlSerializer(typeof(SerializableResult));
            StringBuilder sb = new StringBuilder();
            using (
            StringWriter writer = new StringWriter(sb)
            )
            {
                serializer.Serialize(writer, serializable);
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
                XmlSerializer deserealizer = new XmlSerializer(typeof(SerializableResult));
                object deserealized = deserealizer.Deserialize(reader);
                SerializableResult serializable = (SerializableResult)deserealized;
                return SerializableResult.build(serializable);
            }
        }
    }
}
