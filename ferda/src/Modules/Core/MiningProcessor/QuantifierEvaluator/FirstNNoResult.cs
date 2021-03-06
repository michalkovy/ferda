// FirstNNoResult.cs - evaluator for virtual attribute
//
// Author:  Alexander Kuzmin <alexander.kuzmin@gmail.com>
//
// Copyright (c) 2007 Alexander Kuzmin
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
using System.Diagnostics;
using Ferda.Guha.MiningProcessor.Miners;
using Ferda.Guha.MiningProcessor.Results;

namespace Ferda.Guha.MiningProcessor.QuantifierEvaluator
{
    /// <summary>
    /// Evaluator for relational DM - works with result differently
    /// no hypotheses are added to the result
    /// bool vector is constructed instead
    /// </summary>
    class FirstNNoResult : EvaluatorBase
    {
        /// <summary>
        /// Class for buffering contingency tables to count quantifiers on
        /// </summary>
        private class bufferItem
        {
            public bufferItem(ContingencyTableHelper contingencyTable, double [] firstSetValues)
            {
                ContingencyTable = contingencyTable;
                FirstSetValues = firstSetValues;
            }
            public ContingencyTableHelper ContingencyTable;
            public double [] FirstSetValues;
        }

        #region Private fields

        /// <summary>
        /// The contingency tables buffer
        /// </summary>
        private bufferItem[] _buffer = new bufferItem[_bufferSize];

        /// <summary>
        /// Counted evaluation vector
        /// </summary>
        private List<bool> evalVector = new List<bool>();

        #endregion

        #region Constructor

        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="miningProcessor">Mining processor</param>
        public FirstNNoResult(MiningProcessorBase miningProcessor)
            : base(miningProcessor)
        {
        }

        #endregion

        #region IEvaluator Members

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
        public override void VerifyIsComplete(ContingencyTableHelper contingencyTable, Hypothesis hypothesis, System.Threading.WaitCallback setFinished)
        {
            lock(this)
			{
				if (_actBufferUsed < _bufferMaxUsedSize)
				{
					_buffer[_actBufferUsed] = new bufferItem(contingencyTable, new double [0]);
					_actBufferUsed++;
				}
				if (_actBufferUsed == _bufferMaxUsedSize)
                //  return flushIsComplete();
					setFinished(null);
			}
        }

        /// <summary>
        /// Flushes the evaluator, cleans the buffer
        /// </summary>
        public override void Flush()
        {
            _actBufferUsed = 0;
            evalVector.Clear();
            _buffer = new bufferItem[_bufferSize];
        }

        /// <summary>
        /// Verifies a relevant question against the quantifiers for the SD task
        /// and also checks if the process of relevant questions
        /// verification is complete. 
        /// </summary>
        /// <param name="contingencyTable">The contingency table</param>
        /// <param name="hypothesis">Corresponding hypothesis</param>
        /// <param name="sDFirstSetValues">Table of first set values</param>
        /// <returns>If the verification process is complete.</returns>
        public override bool VerifyIsCompleteSDSecondSet(ContingencyTableHelper contingencyTable, double[] sDFirstSetValues, Hypothesis hypothesis)
        {

            if (_actBufferUsed < _bufferMaxUsedSize)
            {
                _buffer[_actBufferUsed] = new bufferItem(contingencyTable, sDFirstSetValues);
                _actBufferUsed++;
            }
            if (_actBufferUsed == _bufferMaxUsedSize)
                //  return flushIsComplete();
                return true;

            return false;
        }

        /// <summary>
        /// Returns boolean vector of verification results on contingency tables. 
        /// The method is used for computation of the virtual hypotheses attributes,
        /// it is not used in the "normal" mining. 
        /// </summary>
        /// <returns>Boolean vector</returns>
        public override bool[] GetEvaluationVector()
        {
            countEvaluationVector();
            return evalVector.ToArray();
        }

        /// <summary>
        /// Returns boolean vector of verification results on contingency tables for SD miners
        /// The method is used for computation of the virtual hypotheses attributes,
        /// it is not used in the "normal" mining.
        /// </summary>
        /// <returns>Boolean vector</returns>
        public override bool[] GetEvaluationVectorSD()
        {
            countEvaluationVectorSD();
            return evalVector.ToArray();
        }

        #endregion

        /// <summary>
        /// Determines the progress of the computation
        /// (value from 0 to 1, -1 for unknown)
        /// </summary>
        /// <returns>Progress value</returns>
        private float progressValue()
        {
            return System.Math.Max(
                (float)(_rInfo.NumberOfHypotheses) / (float)_n,
                (float)(_rInfo.NumberOfVerifications) / (float)(_miningProcessor.TotalCount)
                );
        }

        /// <summary>
        /// Sets the progress message
        /// </summary>
        /// <returns>Progress message</returns>
        private string progressMessage()
        {
            return string.Format("# of relevant questions: {0}",
                                 _rInfo.TotalNumberOfRelevantQuestions);
        }

        /// <summary>
        /// Counts evaluation bool vector - contingency tables are taken form the buffer
        /// </summary>
        /// <returns></returns>
        private bool countEvaluationVector()
        {
            if (_actBufferUsed == 0)
                return false;

            ContingencyTableHelper[] tables = new ContingencyTableHelper[_actBufferUsed];

            for (int i = 0; i < _actBufferUsed; i++)
            {
                tables[i] = _buffer[i].ContingencyTable;
            }
            _rInfo.NumberOfVerifications += _actBufferUsed;
            _miningProcessor.ProgressSetValue(
                progressValue(), "Counting quantifiers..."
                );
            evalVector.AddRange(_quantifiers.Valid(tables));
            bool finalResult = false;
            bool shouldStop = !_miningProcessor.ProgressSetValue(
                progressValue(), "Counting quantifiers...done, \n" +
                progressMessage()
                );

            _actBufferUsed = 0;
            return finalResult || shouldStop;
        }

        /// <summary>
        /// Counts evaluation bool vector for SD miners - contingency tables are taken form the buffer
        /// </summary>
        /// <returns></returns>
        private bool countEvaluationVectorSD()
        {
            if (_actBufferUsed == 0)
                return false;

            _miningProcessor.ProgressSetValue(
               progressValue(), "Counting quantifiers..."
               );

            for (int j = 0; j < _actBufferUsed; j++)
            {
                ContingencyTableHelper table =
                    _buffer[j].ContingencyTable;
                double[] sDSecondSetValues = _quantifiers.Values(table);
                Debug.Assert(_buffer[j].FirstSetValues.Length == sDSecondSetValues.Length);
                for (int i = 0; i < _buffer[j].FirstSetValues.Length; i++)
                {
                    if (
                    _quantifiers.VerifySdDifferenceOfQuantifierValues(
                        _buffer[j].FirstSetValues,
                        sDSecondSetValues)
                        )
                    {
                        evalVector.Add(true);
                    }
                    else
                    {
                        evalVector.Add(false);
                    }
                }
                _rInfo.NumberOfVerifications += 1;//_buffer[j].FirstSetValues.Length;
            }

            bool finalResult = false;
            bool shouldStop = !_miningProcessor.ProgressSetValue(
                progressValue(), "Counting quantifiers...done, \n" +
                progressMessage()
                );

            _actBufferUsed = 0;
            return finalResult || shouldStop;
        }
    }
}
