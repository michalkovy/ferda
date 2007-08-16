// IEvaluator.cs - interface for quantifier evaluation
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

using Ferda.Guha.MiningProcessor.Results;

namespace Ferda.Guha.MiningProcessor.QuantifierEvaluator
{
    /// <summary>
    /// Interface for quantifier evaluation
    /// </summary>
    public interface IEvaluator
    {		
        /// <summary>
        /// Returns boolean vector of verification results on contingency tables. 
        /// The method is used for computation of the virtual hypotheses attributes,
        /// it is not used in the "normal" mining. 
        /// </summary>
        /// <returns>Boolean vector</returns>
        bool[] GetEvaluationVector();

        /// <summary>
        /// Returns boolean vector of verification results on contingency tables for SD miners
        /// The method is used for computation of the virtual hypotheses attributes,
        /// it is not used in the "normal" mining.
        /// </summary>
        /// <returns>Boolean vector</returns>
        bool[] GetEvaluationVectorSD();

        /// <summary>
        /// Flushes the evaluator, cleans the buffer. 
        /// </summary>
        void Flush();

        /// <summary>
        /// Verifies a relevant question against the quantifiers
        /// and also checks if the process of relevant questions
        /// verification is complete. 
        /// </summary>
        /// <remarks>
        /// In the future, we plan to buffer the settings and execute
        /// <c>Valid</c> on array of contingency tables.
        /// </remarks>
        /// <param name="contingencyTable">The contingency table</param>
        /// <param name="hypothesis">Corresponding hypothesis</param>
        /// <param name="setFinished">The fininshed callback,
        /// method to be executed when the verification process
        /// is complete.</param>
        void VerifyIsComplete(ContingencyTableHelper contingencyTable, 
            Hypothesis hypothesis, System.Threading.WaitCallback setFinished);

        /// <summary>
        /// Counts quantifier values for first contingency table for SD miners
        /// </summary>
        /// <param name="contingencyTable">Contingency table</param>
        /// <returns>Counted values of quantifiers</returns>
        double[] SDFirstSetValues(ContingencyTableHelper contingencyTable);

        /// <summary>
        /// Verifies a relevant question against the quantifiers for the SD task
        /// and also checks if the process of relevant questions
        /// verification is complete. 
        /// </summary>
        /// <param name="contingencyTable">The contingency table</param>
        /// <param name="hypothesis">Corresponding hypothesis</param>
        /// <param name="sDFirstSetValues">Table of first set values</param>
        /// <returns>If the verification process is complete.</returns>
        bool VerifyIsCompleteSDSecondSet(ContingencyTableHelper contingencyTable, double[] sDFirstSetValues, Hypothesis hypothesis);
    }
}
