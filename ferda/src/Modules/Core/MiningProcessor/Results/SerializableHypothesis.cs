using System;
using System.Collections.Generic;
using System.Text;

namespace Ferda.Guha.MiningProcessor.Results
{
    /// <summary>
    /// Serializable form of <see cref="T:Ferda.Guha.MiningProcessor.Results.Hypothesis"/>.
    /// (Design Pattern <c>Builder</c> is used).
    /// </summary>
    [Serializable()]
    public class SerializableHypothesis
    {
        /// <summary>
        /// The contingecy table (for first set in SD tasks).
        /// </summary>
        public double[][] ContingencyTableA;
        
        /// <summary>
        /// The contingency table for second set in SD tasks.
        /// </summary>
        public double[][] ContingencyTableB;
        
        /// <summary>
        /// Serialized formulas specifiing used boolean/categorial attributes.
        /// </summary>
        public SerializableFormula[] Formulas = new SerializableFormula[Hypothesis.CountOfFormulas];

        /// <summary>
        /// Builds the specified input.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static Hypothesis Build(SerializableHypothesis input)
        {
            Hypothesis result = new Hypothesis();
            result.ContingencyTableA = input.ContingencyTableA;
            result.ContingencyTableB = input.ContingencyTableB;
            for (int i = 0; i < Hypothesis.CountOfFormulas; i++)
            {
                result._formulas[i] = SerializableFormula.Build(input.Formulas[i]);
            }
            return result;
        }

        /// <summary>
        /// Builds the specified input.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static SerializableHypothesis Build(Hypothesis input)
        {
            SerializableHypothesis result = new SerializableHypothesis();
            result.ContingencyTableA = input.ContingencyTableA;
            result.ContingencyTableB = input.ContingencyTableB;
            for (int i = 0; i < Hypothesis.CountOfFormulas; i++)
            {
                result.Formulas[i] = SerializableFormula.Build(input._formulas[i]);
            }
            return result;
        }
    }
}
