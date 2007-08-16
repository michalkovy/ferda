// EvaluatorBase.cs - basic abstract class for quantifier evaluation
//
// Authors: Martin Ralbovský <martin.ralbovsky@gmail.com>      
//
// Copyright (c) 2006 Martin Ralbovský
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
using Ferda.Guha.MiningProcessor.Results;
using Ferda.Guha.MiningProcessor.Miners;

namespace Ferda.Guha.MiningProcessor.QuantifierEvaluator
{
    /// <summary>
    /// Abstract common class for all evaluators
    /// </summary>
    public abstract class EvaluatorBase : IEvaluator
    {
        #region Protected fields

        /// <summary>
        /// The mining processor
        /// </summary>
        protected readonly MiningProcessorBase _miningProcessor;
        /// <summary>
        /// List of quantifiers
        /// </summary>
        protected readonly Quantifiers _quantifiers;
        /// <summary>
        /// Result of the mining processor run
        /// </summary>
        protected readonly Result _result;
        /// <summary>
        /// Information about the mining processor run
        /// </summary>
        protected readonly SerializableResultInfo _rInfo;
        /// <summary>
        /// Maximal number of hypotheses to be generated
        /// </summary>
        protected readonly long _n;

        /// <summary>
        /// Maximal used size of the buffer
        /// </summary>
        protected readonly int _bufferMaxUsedSize;
        /// <summary>
        /// Initial size of the buffer
        /// </summary>
        protected const int _bufferSize = 10000;

        /// <summary>
        /// Actual size of the buffer
        /// </summary>
        protected int _actBufferUsed = 0;

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor of the class. Initializes
        /// the protected fields.
        /// </summary>
        /// <param name="miningProcessor">Mining processor</param>
        protected EvaluatorBase(MiningProcessorBase miningProcessor)
        {
            _miningProcessor = miningProcessor;
            _quantifiers = miningProcessor.Quantifiers;
            _result = miningProcessor.Result;
            _rInfo = miningProcessor.ResultInfo;
            _n = miningProcessor.TaskParams.maxSizeOfResult;

            switch (_miningProcessor.TaskType)
            {
                case TaskTypeEnum.FourFold:
                case TaskTypeEnum.SDFourFold:
                    _bufferMaxUsedSize = _bufferSize;
                    break;
                case TaskTypeEnum.KL:
                case TaskTypeEnum.SDKL:
                    _bufferMaxUsedSize = _bufferSize / 100;
                    break;
                case TaskTypeEnum.CF:
                case TaskTypeEnum.SDCF:
                    _bufferMaxUsedSize = _bufferSize / 10;
                    break;
                default:
                    throw new NotImplementedException();
            }
            if (_bufferMaxUsedSize <= 0)
                throw new ApplicationException();
        }

        #endregion

        #region Abstract members

        /// <summary>
        /// Returns boolean vector of verification results on contingency tables. 
        /// The method is used for computation of the virtual hypotheses attributes,
        /// it is not used in the "normal" mining. 
        /// </summary>
        /// <returns>Boolean vector</returns>
        public abstract bool[] GetEvaluationVector();

        /// <summary>
        /// Returns boolean vector of verification results on contingency tables for SD miners
        /// The method is used for computation of the virtual hypotheses attributes,
        /// it is not used in the "normal" mining.
        /// </summary>
        /// <returns>Boolean vector</returns>
        public abstract bool[] GetEvaluationVectorSD();

        /// <summary>
        /// Flushes the evaluator, cleans the buffer. 
        /// </summary>
        public abstract void Flush();

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
        public abstract void VerifyIsComplete(ContingencyTableHelper contingencyTable,
            Hypothesis hypothesis, System.Threading.WaitCallback setFinished);

        /// <summary>
        /// Verifies a relevant question against the quantifiers for the SD task
        /// and also checks if the process of relevant questions
        /// verification is complete. 
        /// </summary>
        /// <param name="contingencyTable">The contingency table</param>
        /// <param name="hypothesis">Corresponding hypothesis</param>
        /// <param name="sDFirstSetValues">Table of first set values</param>
        /// <returns>If the verification process is complete.</returns>
        public abstract bool VerifyIsCompleteSDSecondSet(ContingencyTableHelper contingencyTable, 
            double[] sDFirstSetValues, Hypothesis hypothesis);

        #endregion

        /// <summary>
        /// Counts quantifier values for first contingency table for SD miners
        /// </summary>
        /// <param name="contingencyTable">Contingency table</param>
        /// <returns>Counted values of quantifiers</returns>
        public double[] SDFirstSetValues(ContingencyTableHelper contingencyTable)
        {
            return _quantifiers.Values(contingencyTable);
        }
    }
}
