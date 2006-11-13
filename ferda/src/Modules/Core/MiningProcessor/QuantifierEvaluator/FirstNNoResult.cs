using System;
using System.Collections.Generic;
using System.Diagnostics;
using Ferda.Guha.MiningProcessor.Miners;
using Ferda.Guha.MiningProcessor.Results;

namespace Ferda.Guha.MiningProcessor.QuantifierEvaluator
{
    class FirstNNoResult : IEvaluator
    {
        private class bufferItem
        {
            public bufferItem(ContingencyTableHelper contingencyTable)
            {
                ContingencyTable = contingencyTable;
            }
            public ContingencyTableHelper ContingencyTable;
        }

        private const int _bufferSize = 300;
        private bufferItem[] _buffer = new bufferItem[_bufferSize];
        private int _actBufferUsed = 0;
        private readonly MiningProcessorBase _miningProcessor;
        private readonly Quantifiers _quantifiers;
      //  private readonly Result _result;
        private readonly SerializableResultInfo _rInfo;
        private readonly long _n;

        private List<bool> evalVector = new List<bool>();

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

        public bool VerifyIsComplete(ContingencyTableHelper contingencyTable, Hypothesis hypothesis)
        {
            
            if (_actBufferUsed < _bufferMaxUsedSize)
            {
                _buffer[_actBufferUsed] = new bufferItem(contingencyTable);
                _actBufferUsed++;
            }
            if (_actBufferUsed == _bufferMaxUsedSize)
                //  return flushIsComplete();
                return true;

            return false;
        }

        public void Flush()
        {
            _actBufferUsed = 0;
            evalVector.Clear();
        }

        private bool countEvaluationVector()
        {
            if (_actBufferUsed == 0)
                return false;

            List<ContingencyTableHelper> tables = new List<ContingencyTableHelper>(_actBufferUsed);
            for (int i = 0; i < _actBufferUsed; i++)
            {
                tables.Insert(i, _buffer[i].ContingencyTable);
            }
            _rInfo.NumberOfVerifications += _actBufferUsed;
            evalVector.AddRange(_quantifiers.Valid(tables));
            bool finalResult = false;

            /*
            for (int i = 0; i < result.Count; i++)
            {
                if (result[i])
                {
                    _rInfo.NumberOfHypotheses++;
                    _result.Hypotheses.Add(_buffer[i].Hypothesis);
                    if (_result.Hypotheses.Count >= _n)
                    {
                        finalResult = true;
                        break;
                    }
                }
            }*/

            bool shouldStop = !_miningProcessor.ProgressSetValue(
                progress(), progressMessage()
                );

            _actBufferUsed = 0;
            return finalResult || shouldStop;
        }

        public double[] SDFirstSetValues(ContingencyTableHelper contingencyTable)
        {
            return _quantifiers.Values(contingencyTable);
        }

        public bool VerifyIsCompleteSDSecondSet(ContingencyTableHelper contingencyTable, double[] sDFirstSetValues, Hypothesis hypothesis)
        {
            double[] sDSecondSetValues = _quantifiers.Values(contingencyTable);
            Debug.Assert(sDFirstSetValues.Length == sDSecondSetValues.Length);
           /* for (int i = 0; i < sDFirstSetValues.Length; i++)
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
            }*/
            _rInfo.NumberOfVerifications += sDFirstSetValues.Length;
            bool shouldStop = !_miningProcessor.ProgressSetValue(
                progress(), progressMessage()
                );
            return false || shouldStop;
        }

        public bool[] GetEvaluationVector()
        {
            countEvaluationVector();
            return evalVector.ToArray();
        }

        #endregion

        private float progress()
        {
            return System.Math.Max(
                (float)(_rInfo.NumberOfHypotheses) / (float)_n,
                (float)(_rInfo.NumberOfVerifications) / (float)(_miningProcessor.TotalCount)
                );
        }

        private string progressMessage()
        {
            return string.Format("Number of Relevant Questions: {0}",
                                 _rInfo.TotalNumberOfRelevantQuestions);
        }
    }
}
