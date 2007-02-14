using System;
using System.Collections.Generic;
using System.Diagnostics;
using Ferda.Guha.MiningProcessor.Miners;
using Ferda.Guha.MiningProcessor.Results;

namespace Ferda.Guha.MiningProcessor.QuantifierEvaluator
{
    class FirstNNoResult : IEvaluator
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

        private const int _bufferSize = 10000;
        private bufferItem[] _buffer = new bufferItem[_bufferSize];
        private int _actBufferUsed = 0;
        private readonly MiningProcessorBase _miningProcessor;
        private readonly Quantifiers _quantifiers;
      //  private readonly Result _result;
        private readonly SerializableResultInfo _rInfo;
        private readonly long _n;

        /// <summary>
        /// Counted evaluation vector
        /// </summary>
        private List<bool> evalVector = new List<bool>();

        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="miningProcessor">Mining processor</param>
        public FirstNNoResult(MiningProcessorBase miningProcessor)
        {
            _miningProcessor = miningProcessor;
            _quantifiers = miningProcessor.Quantifiers;
           // _result = miningProcessor.Result;
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
        private readonly int _bufferMaxUsedSize;

        #region IEvaluator Members


        /// <summary>
        /// Checks whether one more contingency table can be added to the buffer
        /// </summary>
        /// <param name="contingencyTable">Contingency table to add</param>
        /// <param name="hypothesis"></param>
        /// <returns>True if added succesfully</returns>
        public bool VerifyIsComplete(ContingencyTableHelper contingencyTable, Hypothesis hypothesis)
        {
            
            if (_actBufferUsed < _bufferMaxUsedSize)
            {
                _buffer[_actBufferUsed] = new bufferItem(contingencyTable, new double [0]);
                _actBufferUsed++;
            }
            if (_actBufferUsed == _bufferMaxUsedSize)
                //  return flushIsComplete();
                return true;

            return false;
        }

        /// <summary>
        /// Flushes the evaluator, cleans the buffer
        /// </summary>
        public void Flush()
        {
            _actBufferUsed = 0;
            evalVector.Clear();
        }

        /// <summary>
        /// Counts evaluation bool vector - contingency tables are taken form the buffer
        /// </summary>
        /// <returns></returns>
        private bool countEvaluationVector()
        {
            if (_actBufferUsed == 0)
                return false;

            ContingencyTableHelper [] tables = new ContingencyTableHelper[_actBufferUsed];

            for (int i = 0; i < _actBufferUsed; i++)
            {
                tables[i] =_buffer[i].ContingencyTable;
            }
            _rInfo.NumberOfVerifications += _actBufferUsed;
            _miningProcessor.ProgressSetValue(
                progress(), "Counting quantifiers..."
                );
            evalVector.AddRange(_quantifiers.Valid(tables));
            bool finalResult = false;
            bool shouldStop = !_miningProcessor.ProgressSetValue(
                progress(), "Counting quantifiers...done, \n" + 
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
               progress(), "Counting quantifiers..."
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
                _rInfo.NumberOfVerifications += _buffer[j].FirstSetValues.Length;
            }
           
            bool finalResult = false;
            bool shouldStop = !_miningProcessor.ProgressSetValue(
                progress(), "Counting quantifiers...done, \n" +
                progressMessage()
                );

            _actBufferUsed = 0;
            return finalResult || shouldStop;
        }

        /// <summary>
        /// Counts quantifier values for first contingency table for SD miners
        /// </summary>
        /// <param name="contingencyTable"></param>
        /// <returns></returns>
        public double[] SDFirstSetValues(ContingencyTableHelper contingencyTable)
        {
            return _quantifiers.Values(contingencyTable);
        }

        /// <summary>
        /// Checks whether one more contingency table can be added to the buffer - SD miners version
        /// </summary>
        /// <param name="contingencyTable">Contingency table to add</param>
        /// <param name="sDFirstSetValues">First contingency table</param>
        /// <param name="hypothesis"></param>
        /// <returns>True if added succesfully</returns>
        public bool VerifyIsCompleteSDSecondSet(ContingencyTableHelper contingencyTable, double[] sDFirstSetValues, Hypothesis hypothesis)
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
        /// Gets the evaluation vector for the contingency tables in the buffer
        /// </summary>
        /// <returns>Bool evaluation vector</returns>
        public bool[] GetEvaluationVector()
        {
            countEvaluationVector();
            return evalVector.ToArray();
        }

        /// <summary>
        /// Gets the evaluation vector for the contingency tables in the buffer for SD miners
        /// </summary>
        /// <returns></returns>
        public bool[] GetEvaluationVectorSD()
        {
            countEvaluationVectorSD();
            return evalVector.ToArray();
        }

        #endregion

        /// <summary>
        /// Sets miner progress bar
        /// </summary>
        /// <returns></returns>
        private float progress()
        {
            return System.Math.Max(
                (float)(_rInfo.NumberOfHypotheses) / (float)_n,
                (float)(_rInfo.NumberOfVerifications) / (float)(_miningProcessor.TotalCount)
                );
        }

        /// <summary>
        /// Sets progress bar message
        /// </summary>
        /// <returns></returns>
        private string progressMessage()
        {
            return string.Format("# of relevant questions: {0}",
                                 _rInfo.TotalNumberOfRelevantQuestions);
        }
    }
}
