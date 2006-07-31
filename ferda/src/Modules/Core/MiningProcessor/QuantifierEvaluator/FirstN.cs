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

        //TODO tady pujde bufferovat zadani a volat Valid na pole CT
        public bool VerifyIsComplete(ContingencyTableHelper contingencyTable, Hypothesis hypothesis)
        {
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
        }

        //TODO tohle bude validovat zbytek bufferu
        public void Flush()
        {
            ;
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