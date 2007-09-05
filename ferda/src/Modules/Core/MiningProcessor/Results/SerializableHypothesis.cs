// SerializableHypothesis.cs - a GUHA hypothesis (serializable version)
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
