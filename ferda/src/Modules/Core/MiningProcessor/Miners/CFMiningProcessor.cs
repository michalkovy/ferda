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
            booleanCedents = new IEntityEnumerator[] { _condition };
            categorialCedents = new CategorialAttributeTrace[][] { _attribute };
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
            _attribute = CreateCategorialAttributeTrace(MarkEnum.Attribute, _categorialAttributes, false, this);

            if (!ProgressSetValue(-1, "Preparing Condition trace"))
                return;
            _condition = CreateBooleanAttributeTrace(MarkEnum.Condition, _booleanAttributes, true, this);
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

            ContingencyTableHelper contingencyTable;
            double[][] cT;

            foreach (CategorialAttributeTrace trace in _attribute)
            {
                foreach (IBitString cS in _condition)
                {
                    cT = new double[1][];
                    if (cS is IEmptyBitString)
                        cT[0] = BitStringsArraySums.Sum(trace.BitStrings);
                    else
                        cT[0] = BitStringsArraySums.Sum(
                                BitStringsArrayAnd.Operation(trace.BitStrings, cS)
                            );
                    contingencyTable = new ContingencyTableHelper(
                        cT,
                        _result.AllObjectsCount,
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
            evaluator.Flush();
            resultFinish();
        }

        public override IEnumerable<KeyValuePair<string, BitStringIce>> TraceBoolean(int[] CountVector, Ferda.Modules.GuidStruct attributeGuid)
        {
            throw new Exception("The method or operation is not implemented.");
        }
    }
}