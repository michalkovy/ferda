using System;
using System.Collections.Generic;
using Ferda.Guha.Math.Quantifiers;
using Ferda.Guha.MiningProcessor.BitStrings;
using Ferda.Guha.MiningProcessor.Generation;
using Ferda.Guha.MiningProcessor.QuantifierEvaluator;
using Ferda.Guha.MiningProcessor.Results;
using Ferda.ModulesManager;

namespace Ferda.Guha.MiningProcessor.Miners
{
    public class CFMiningProcessor : MiningProcessorBase
    {
        protected override void getCedents(out ICollection<IEntityEnumerator> booleanCedents,
                                           out ICollection<CategorialAttributeTrace[]> categorialCedents)
        {
            booleanCedents = new IEntityEnumerator[] { _condition};
            categorialCedents = new CategorialAttributeTrace[][] { _attribute};
        }

        public override TaskTypeEnum TaskType
        {
            get { return TaskTypeEnum.CF; }
        }

        protected override CategorialAttributeTrace[] attributesWhichShouldSupportNumericValues()
        {
            return _attribute;
        }

        protected override List<CategorialAttributeTrace[]> attributesWhichRequestsSomeCardinality()
        {
            List<CategorialAttributeTrace[]> result = new List<CategorialAttributeTrace[]>();
            result.Add(_attribute);
            return result;
        }

        private CategorialAttributeTrace[] _attribute;
        private IEntityEnumerator _condition;

        protected override void prepareAttributeTraces()
        {
            if (!ProgressSetValue(-1, "Preparing Attribute trace"))
                return;
            _attribute = CreateCategorialAttributeTrace(MarkEnum.Attribute, _categorialAttributes, false);

            if (!ProgressSetValue(-1, "Preparing Condition trace"))
                return;
            _condition = CreateBooleanAttributeTrace(MarkEnum.Condition, _booleanAttributes, true);
        }

        public CFMiningProcessor(
            BooleanAttribute[] booleanAttributes,
            CategorialAttribute[] categorialAttributes,
            QuantifierBaseFunctionsPrx[] quantifiers,
            TaskRunParams taskParams,
            BitStringGeneratorProviderPrx taskFuncPrx,
            ProgressTaskListener progressListener,
            ProgressBarPrx progressBarPrx
            )
            : base(
                booleanAttributes, categorialAttributes, quantifiers, taskFuncPrx, taskParams, progressListener,
                progressBarPrx)
        {
            afterConstruct();
        }

        public override void Trace()
        {
            if (!ProgressSetValue(-1, "Begining of attributes trace."))
                return;
            resultInit();

            IEvaluator evaluator;
            if (TaskParams.evaluationType == TaskEvaluationTypeEnum.FirstN)
                evaluator = new FirstN(this);
            else
                throw new NotImplementedException();

            long allObjectsCount = Int64.MinValue;

            ContingencyTableHelper contingencyTable;
            double[][] cT;
            
            foreach (CategorialAttributeTrace trace in _attribute)
            {
                if (allObjectsCount < 0)
                    allObjectsCount = trace.BitStrings[0].Length;
                if (allObjectsCount < 0)
                    throw new ApplicationException("Unable to determine \"all objects count\".");
                
                foreach (IBitString cS in _condition)
                {
                    cT = new double[1][];
                    cT[0] = BitStringsArraySums.Sum(
                            BitStringsArrayAnd.Operation(trace.BitStrings, cS)
                        );
                    contingencyTable = new ContingencyTableHelper(
                        cT,
                        allObjectsCount,
                        trace.Identifier.AttributeGuid
                        );
                    Hypothesis hypothesis = new Hypothesis();
                    hypothesis.SetFormula(MarkEnum.Attribute, trace.Identifier);
                    hypothesis.SetFormula(MarkEnum.Condition, cS.Identifier);
                    hypothesis.ContingencyTableA = contingencyTable.ContingencyTable;

                    if (evaluator.VerifyIsComplete(contingencyTable, hypothesis))
                        goto finish;
                }
            }
          
            finish:
            ProgressSetValue(1, "Completing result.");
            evaluator.Flush();
            resultFinish(allObjectsCount);
        }
    }
}