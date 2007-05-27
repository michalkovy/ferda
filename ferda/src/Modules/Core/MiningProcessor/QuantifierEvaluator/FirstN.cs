#define BATCH

using System;
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

        private const int _bufferSize = 300;
        private bufferItem[] _buffer = new bufferItem[_bufferSize];
        private int _actBufferUsed = 0;
		private static readonly bool _useThreads = System.Environment.ProcessorCount > 1;
		
		private class FlushIsCompleteHelper
		{
			public FlushIsCompleteHelper(bufferItem[] bufferCopy, int bufferUsed, System.Threading.WaitCallback setFinished)
			{
				this.bufferCopy = bufferCopy;
				this.bufferUsed = bufferUsed;
				this.setFinished = setFinished;
			}
			
			public bufferItem[] bufferCopy;
			public int bufferUsed;
			public System.Threading.WaitCallback setFinished;
		}

        public void VerifyIsComplete(ContingencyTableHelper contingencyTable, Hypothesis hypothesis, System.Threading.WaitCallback setFinished)
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

        public void Flush()
        {
			lock(this)
			{
				flushIsComplete(new FlushIsCompleteHelper(_buffer, _actBufferUsed, null));
				_buffer = new bufferItem[_bufferSize];
				_actBufferUsed = 0;
			}
        }

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
			lock(_result)
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
					progress(), progressMessage()
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

        #region IEvaluator Members


        public double[] SDFirstSetValues(ContingencyTableHelper contingencyTable)
        {
            return _quantifiers.Values(contingencyTable);
        }

        public bool VerifyIsCompleteSDSecondSet(ContingencyTableHelper contingencyTable, double[] sDFirstSetValues, Hypothesis hypothesis)
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
                progress(), progressMessage()
                );
            return false || shouldStop;
        }

        public bool[] GetEvaluationVector()
        {
            return new bool[0];
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
            return string.Format("Number of Verifications: {0}, Number of hypotheses: {1}",
                                 _rInfo.NumberOfVerifications,
                                 _rInfo.NumberOfHypotheses);
        }

        #region IEvaluator Members


        public bool[] GetEvaluationVectorSD()
        {
            return new bool[0];
        }

        #endregion
    }
}
