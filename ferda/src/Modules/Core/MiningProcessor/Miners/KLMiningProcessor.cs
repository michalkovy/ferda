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
    public class KLMiningProcessor : MiningProcessorBase
    {
        protected override void getCedents(out ICollection<IEntityEnumerator> booleanCedents,
                                           out ICollection<CategorialAttributeTrace[]> categorialCedents)
        {
            booleanCedents = new IEntityEnumerator[] { _condition };
            categorialCedents = new CategorialAttributeTrace[][] { _rowAttribute, _columnAttribute };
        }

        public override TaskTypeEnum TaskType
        {
            get { return TaskTypeEnum.KL; }
        }

        protected override CategorialAttributeTrace[] attributesWhichShouldSupportNumericValues()
        {
            return null;
        }

        protected override List<CategorialAttributeTrace[]> attributesWhichRequestsSomeCardinality()
        {
            List<CategorialAttributeTrace[]> result = new List<CategorialAttributeTrace[]>();
            result.Add(_rowAttribute);
            result.Add(_columnAttribute);
            return result;
        }

        private CategorialAttributeTrace[] _rowAttribute;
        private CategorialAttributeTrace[] _columnAttribute;
        private IEntityEnumerator _condition;

        protected override void prepareAttributeTraces()
        {
            if (!ProgressSetValue(-1, "Preparing Row Attribute trace"))
                return;
            _rowAttribute = CreateCategorialAttributeTrace(MarkEnum.RowAttribute, _categorialAttributes, false);

            if (!ProgressSetValue(-1, "Preparing Column Attribute trace"))
                return;
            _columnAttribute = CreateCategorialAttributeTrace(MarkEnum.ColumnAttribute, _categorialAttributes, false);

            if (!ProgressSetValue(-1, "Preparing Condition trace"))
                return;
            _condition = CreateBooleanAttributeTrace(MarkEnum.Condition, _booleanAttributes, true);
        }

        public KLMiningProcessor(
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
            IBitString[][] bSCT;
            double[][] cT;

            foreach (CategorialAttributeTrace rowTrace in _rowAttribute)
            {
                if (allObjectsCount < 0)
                    allObjectsCount = rowTrace.BitStrings[0].Length;
                if (allObjectsCount < 0)
                    throw new ApplicationException("Unable to determine \"all objects count\".");

                foreach (CategorialAttributeTrace columnTrace in _columnAttribute)
                {
                    bSCT = BitStringsArrayAnd.Operation(rowTrace.BitStrings, columnTrace.BitStrings);
                    
                    foreach (IBitString cS in _condition)
                    {
                        if (cS is IEmptyBitString)
                            cT = BitStringsArraySums.Sum(bSCT);
                        else
                            cT = BitStringsArraySums.Sum(
                                BitStringsArrayAnd.Operation(bSCT, cS)
                            );
                        
                        contingencyTable = new ContingencyTableHelper(
                            cT,
                            allObjectsCount
                            );
                        Hypothesis hypothesis = new Hypothesis();
                        hypothesis.SetFormula(MarkEnum.RowAttribute, rowTrace.Identifier);
                        hypothesis.SetFormula(MarkEnum.ColumnAttribute, columnTrace.Identifier);
                        hypothesis.SetFormula(MarkEnum.Condition, cS.Identifier);
                        hypothesis.ContingencyTableA = contingencyTable.ContingencyTable;

                        if (evaluator.VerifyIsComplete(contingencyTable, hypothesis))
                            goto finish;
                    }
                }
            }

        finish:
            ProgressSetValue(1, "Completing result.");
            evaluator.Flush();
            resultFinish(allObjectsCount);
        }
    }
}