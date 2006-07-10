namespace Ferda.Guha.MiningProcessor.QuantifierEvaluator
{
    public class FirstN : IEvaluator
    {
        private Quantifiers _quantifiers;
        private Result _result;
        private SerializableResultInfo _rInfo;
        private TaskRunParams _taskParams;

        public FirstN(Quantifiers quantifiers, Result result, SerializableResultInfo rInfo, TaskRunParams taskParams)
        {
            _quantifiers = quantifiers;
            _result = result;
            _rInfo = rInfo;
            _taskParams = taskParams;
        }

        //TODO tady pujde bufferovat zadani a volat Valid na pole CT
        public bool VerifyIsComplete(ContingencyTableHelper contingencyTable, Hypothesis hypothesis)
        {
            if (_quantifiers.Valid(contingencyTable))
            {
                _rInfo.NumberOfHypotheses++;
                _result.Hypotheses.Add(hypothesis);
                if (_result.Hypotheses.Count >= _taskParams.maxSizeOfResult)
                    return true;
            }
            return false;
        }

        //TODO tohle bude validovat zbytek bufferu
        public void Flush()
        {
            ;
        }
    }
}