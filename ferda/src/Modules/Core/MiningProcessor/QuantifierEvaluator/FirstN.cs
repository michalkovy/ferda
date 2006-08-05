#define BATCH

using System.Collections.Generic;
using System.Diagnostics;
using Ferda.Guha.MiningProcessor.Miners;
using Ferda.Guha.MiningProcessor.Results;

namespace Ferda.Guha.MiningProcessor.QuantifierEvaluator
{
    public class FirstN : IEvaluator
    {
        private readonly MiningProcessorBase _miningProcessor;
        private readonly Quantifiers _quantifiers;
        private readonly Result _result;
        private readonly SerializableResultInfo _rInfo;
        private readonly long _n;

        public FirstN(MiningProcessorBase miningProcessor)
        {
            _miningProcessor = miningProcessor;
            _quantifiers = miningProcessor.Quantifiers;
            _result = miningProcessor.Result;
            _rInfo = miningProcessor.ResultInfo;
            _n = miningProcessor.TaskParams.maxSizeOfResult;
        }

        private class bufferItem
        {
            public bufferItem(ContingencyTableHelper contingencyTable, Hypothesis hypothesis)
            {
                ContingencyTable = contingencyTable;
                Hypothesis = hypothesis;
            }
            public ContingencyTableHelper ContingencyTable;
            public Hypothesis Hypothesis;
        }

        private const int _bufferSize = 30;
        private bufferItem[] _buffer = new bufferItem[_bufferSize];
        private int _actBufferUsed = 0;

        public bool VerifyIsComplete(ContingencyTableHelper contingencyTable, Hypothesis hypothesis)
        {
#if BATCH //Valid ComputeBatch(setting[] ...)
            if (_actBufferUsed < _bufferSize)
            {
                _buffer[_actBufferUsed] = new bufferItem(contingencyTable, hypothesis);
                _actBufferUsed++;
            }
            if (_actBufferUsed == _bufferSize)
                return flushIsComplete();

            return false;
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

        public void Flush()
        {
            flushIsComplete();
        }
                
        private bool flushIsComplete()
        {
            if (_actBufferUsed == 0)
                return false;

            List<ContingencyTableHelper> tables = new List<ContingencyTableHelper>(_actBufferUsed);
            for (int i = 0; i < _actBufferUsed; i++)
            {
                tables.Insert(i, _buffer[i].ContingencyTable);
            }
            _rInfo.NumberOfVerifications += _actBufferUsed;
            List<bool> result = _quantifiers.Valid(tables);
            bool finalResult = false;

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
            }

            _miningProcessor.ProgressSetValue(
                _rInfo.NumberOfHypotheses / _n,
                string.Format("Number of Verifications: {0}, Number of hypotheses: {1}",
                              _rInfo.NumberOfVerifications,
                              _rInfo.NumberOfHypotheses)
                );
            
            _actBufferUsed = 0;
            return finalResult;
        }

        #region IEvaluator Members


        public double[] SDFirstSetValues(ContingencyTableHelper contingencyTable)
        {
            return _quantifiers.Values(contingencyTable);
        }

        public bool VerifyIsCompleteSDSecondSet(ContingencyTableHelper contingencyTable, double[] sDFirstSetValues, Hypothesis hypothesis)
        {
            _rInfo.NumberOfVerifications++;
            _miningProcessor.ProgressSetValue(
                _rInfo.NumberOfHypotheses / _n,
                string.Format("Number of Verifications: {0}, Number of hypotheses: {1}",
                              _rInfo.NumberOfVerifications,
                              _rInfo.NumberOfHypotheses)
                );
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
            return false;
        }

        #endregion
    }
}