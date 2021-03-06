// FirstN.cs - the FirstN quantifier evaluation
//
// Authors: Tomáš Kuchař <tomas.kuchar@gmail.com>      
// Commented by: Martin Ralbovský <martin.ralbovsky@gmail.com>
//
// Copyright (c) 2006 Tomáš Kuchař
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

#define BATCH

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Ferda.Guha.MiningProcessor.Miners;
using Ferda.Guha.MiningProcessor.Results;

namespace Ferda.Guha.MiningProcessor.QuantifierEvaluator
{
    /// <summary>
    /// The FirstN quantifier evaluator
    /// </summary>
    public class FirstN : EvaluatorBase
    {
        /// <summary>
        /// An item on a buffer, consisting of a contingency
        /// table and a hypothesis. The item is to be
        /// evaluated by the quantifiers. 
        /// </summary>
        private class bufferItem
        {
            /// <summary>
            /// Default consturctor of the class
            /// </summary>
            /// <param name="contingencyTable">The contingency table</param>
            /// <param name="hypothesis">The hypothesis</param>
            public bufferItem(ContingencyTableHelper contingencyTable, Hypothesis hypothesis)
            {
                ContingencyTable = contingencyTable;
                Hypothesis = hypothesis;
            }

            /// <summary>
            /// A contingency table
            /// </summary>
            public ContingencyTableHelper ContingencyTable;
            /// <summary>
            /// The hypothesis
            /// </summary>
            public Hypothesis Hypothesis;
        }

        /// <summary>
        /// Class that helps with flushing the buffer.
        /// Contains copy of the buffer, information about how many 
        /// items there are in the buffer used and a method to be 
        /// executed when the buffer flushes.
        /// </summary>
        private class FlushIsCompleteHelper
        {
            /// <summary>
            /// Default constructor of the class
            /// </summary>
            /// <param name="bufferCopy">Copy of the buffer</param>
            /// <param name="bufferUsed">How mucho of the buffer is used</param>
            /// <param name="setFinished">The callback on finishing the buffer</param>
            public FlushIsCompleteHelper(bufferItem[] bufferCopy, int bufferUsed, System.Threading.WaitCallback setFinished)
            {
                this.bufferCopy = bufferCopy;
                this.bufferUsed = bufferUsed;
                this.setFinished = setFinished;
            }

            /// <summary>
            /// Copy of the buffer
            /// </summary>
            public bufferItem[] bufferCopy;
            /// <summary>
            /// How much of the buffer is used
            /// </summary>
            public int bufferUsed;
            /// <summary>
            /// Wait callback method to finish the buffer
            /// </summary>
            public System.Threading.WaitCallback setFinished;
        }

        #region Private fields

        /// <summary>
        /// The contingency tables buffer
        /// </summary>
        private bufferItem[] _buffer = new bufferItem[_bufferSize];

        /// <summary>
        /// If the computation uses more threads
        /// </summary>
        private static readonly bool _useThreads = System.Environment.ProcessorCount > 1;

        #endregion

        #region Constructor

        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="miningProcessor">Mining processor</param>
        public FirstN(MiningProcessorBase miningProcessor)
            :base(miningProcessor)
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
#if BATCH //Valid ComputeBatch(setting[] ...)
			lock(this)
			{
				if (_actBufferUsed < _bufferMaxUsedSize)
				{
					_buffer[_actBufferUsed] = new bufferItem(contingencyTable, hypothesis);
					_actBufferUsed++;
				}
				if (_actBufferUsed == _bufferMaxUsedSize)
				{
					if(_useThreads)
					{
						System.Threading.ThreadPool.QueueUserWorkItem(flushIsComplete, new FlushIsCompleteHelper(_buffer, _actBufferUsed, setFinished));
					}
					else
					{
						flushIsComplete(new FlushIsCompleteHelper(_buffer, _actBufferUsed, setFinished));
					}
					_buffer = new bufferItem[_bufferSize];
					_actBufferUsed = 0;
				}
			}

            return;
#else
            _rInfo.NumberOfVerifications++;
            _miningProcessor.ProgressSetValue(
                _rInfo.NumberOfHypotheses / _n,
                string.Format("Number of Verifications: {0}, Number of hypotheses: {1}",
                              _rInfo.NumberOfVerifications,
                              _rInfo.NumberOfHypotheses)
                );
            if (_quantifiers.Valid(contingencyTable))
            {
                _rInfo.NumberOfHypotheses++;
                _result.Hypotheses.Add(hypothesis);
                if (_result.Hypotheses.Count >= _n)
                    return true;
            }
            return false;
#endif
        }

        /// <summary>
        /// Flushes the evaluator, cleans the buffer
        /// </summary>
        public override void Flush()
        {
			lock(this)
			{
				flushIsComplete(new FlushIsCompleteHelper(_buffer, _actBufferUsed, null));
				_buffer = new bufferItem[_bufferSize];
				_actBufferUsed = 0;
			}
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
            double[] sDSecondSetValues = _quantifiers.Values(contingencyTable);
            Debug.Assert(sDFirstSetValues.Length == sDSecondSetValues.Length);
            for (int i = 0; i < sDFirstSetValues.Length; i++)
            {
                if (
                _quantifiers.VerifySdDifferenceOfQuantifierValues(
                    sDFirstSetValues,
                    sDSecondSetValues)
                    )
                {
                    _rInfo.NumberOfHypotheses++;
                    _result.Hypotheses.Add(hypothesis);
                    if (_result.Hypotheses.Count >= _n)
                        return true;
                }
            }
            _rInfo.NumberOfVerifications += sDFirstSetValues.Length;
            bool shouldStop = !_miningProcessor.ProgressSetValue(
                progressValue(), progressMessage()
                );
            return false || shouldStop;
        }

        /// <summary>
        /// Returns boolean vector of verification results on contingency tables. 
        /// The method is used for computation of the virtual hypotheses attributes,
        /// it is not used in the "normal" mining. 
        /// </summary>
        /// <returns>Boolean vector</returns>
        public override bool[] GetEvaluationVector()
        {
            return new bool[0];
        }

        /// <summary>
        /// Returns boolean vector of verification results on contingency tables for SD miners
        /// The method is used for computation of the virtual hypotheses attributes,
        /// it is not used in the "normal" mining.
        /// </summary>
        /// <returns>Boolean vector</returns>
        public override bool[] GetEvaluationVectorSD()
        {
            return new bool[0];
        }

        #endregion

        /// <summary>
        /// When the buffer is full, this function adds verifies all the 
        /// relevant questions in the buffer and adds the valid
        /// hypotheses to the result.
        /// </summary>
        /// <param name="flushIsCompleteHelperAsObject">The 
        /// <see cref="FlushIsCompleteHelper"/> as an object.</param>
        private void flushIsComplete(Object flushIsCompleteHelperAsObject)
        {
            FlushIsCompleteHelper flushIsCompleteHelper = flushIsCompleteHelperAsObject as FlushIsCompleteHelper;
            if (flushIsCompleteHelper == null || _result == null || _rInfo == null || _quantifiers == null || _miningProcessor == null)
                return;
            System.Threading.WaitCallback setFinished = flushIsCompleteHelper.setFinished;
            bufferItem[] bufferCopy = flushIsCompleteHelper.bufferCopy;
            int actBufferUsed = flushIsCompleteHelper.bufferUsed;
            if (actBufferUsed == 0)
                return;

            List<ContingencyTableHelper> tables = new List<ContingencyTableHelper>(actBufferUsed);
            for (int i = 0; i < actBufferUsed; i++)
            {
                tables.Insert(i, bufferCopy[i].ContingencyTable);
            }

            bool finalResult = false;
            bool shouldStop = false;
            lock (_result)
            {
                _rInfo.NumberOfVerifications += actBufferUsed;

                List<bool> result = _quantifiers.Valid(tables);


                for (int i = 0; i < result.Count; i++)
                {
                    if (result[i])
                    {
                        _rInfo.NumberOfHypotheses++;
                        _result.Hypotheses.Add(bufferCopy[i].Hypothesis);
                        if (_result.Hypotheses.Count >= _n)
                        {
                            finalResult = true;
                            break;
                        }
                    }
                }

                shouldStop = !_miningProcessor.ProgressSetValue(
                    progressValue(), progressMessage()
                );
            }

            if (finalResult || shouldStop)
            {
                if (setFinished != null)
                {
                    setFinished(null);
                }
            }
        }

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
            return string.Format("Number of Verifications: {0}, Number of hypotheses: {1}",
                                 _rInfo.NumberOfVerifications,
                                 _rInfo.NumberOfHypotheses);
        }
    }
}
