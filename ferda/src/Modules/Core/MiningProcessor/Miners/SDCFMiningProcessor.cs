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
    public class SDCFMiningProcessor : MiningProcessorBase
    {
        protected override void getCedents(out ICollection<IEntityEnumerator> booleanCedents,
                                           out ICollection<CategorialAttributeTrace[]> categorialCedents)
        {
            booleanCedents = new IEntityEnumerator[] { _condition, _firstSet, _secondSet };
            categorialCedents = new CategorialAttributeTrace[][] { _attribute };
        }

        public override TaskTypeEnum TaskType
        {
            get { return TaskTypeEnum.SDCF; }
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
        private IEntityEnumerator _firstSet;
        private IEntityEnumerator _secondSet;

        protected override void prepareAttributeTraces()
        {
            if (!ProgressSetValue(-1, "Preparing Attribute trace"))
                return;
            _attribute = CreateCategorialAttributeTrace(MarkEnum.Attribute, _categorialAttributes, false, this);

            if (!ProgressSetValue(-1, "Preparing Condition trace"))
                return;
            _condition = CreateBooleanAttributeTrace(MarkEnum.Condition, _booleanAttributes, true, this);

            if (!ProgressSetValue(-1, "Preparing First set trace"))
                return;
            _firstSet = CreateBooleanAttributeTrace(MarkEnum.FirstSet, _booleanAttributes, false, this);

            if (!ProgressSetValue(-1, "Preparing Second set trace"))
                return;
            _secondSet = CreateBooleanAttributeTrace(MarkEnum.SecondSet, _booleanAttributes, false, this);
        }

        public SDCFMiningProcessor(
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

            IBitString fSF; // first set
            IBitString sSF; // second set

            ContingencyTableHelper contingencyTable1;
            ContingencyTableHelper contingencyTable2;
            double[][] cT1;
            double[][] cT2;
                
            foreach (CategorialAttributeTrace trace in _attribute)
            {
                foreach (IBitString cS in _condition)
                {
                    foreach (IBitString fS in _firstSet)
                    {
                        #region SD first set contingency table
                        fSF = fS.And(cS);
                        
                        cT1 = new double[1][];
                        if (fSF is IEmptyBitString)
                            cT1[0] = BitStringsArraySums.Sum(trace.BitStrings);
                        else
                            cT1[0] = BitStringsArraySums.Sum(
                                    BitStringsArrayAnd.Operation(trace.BitStrings, fSF)
                                );

                        contingencyTable1 = new ContingencyTableHelper(
                            cT1,
                            _result.AllObjectsCount,
                            trace.Identifier.AttributeGuid
                            );
                        #endregion

                        double[] sDFirstSetValues = evaluator.SDFirstSetValues(contingencyTable1);

                        foreach (IBitString sS in _secondSet)
                        {
                            #region SD second set contingency table
                            switch (TaskParams.sdWorkingWithSecondSetMode)
                            {
                                case WorkingWithSecondSetModeEnum.Cedent1AndCedent2:
                                    sSF = sS.And(fS).And(cS);
                                    break;
                                case WorkingWithSecondSetModeEnum.Cedent2:
                                    sSF = sS.And(cS);
                                    break;
                                case WorkingWithSecondSetModeEnum.None:
                                default:
                                    throw new NotImplementedException();
                            }

                            cT2 = new double[1][];
                            if (sSF is IEmptyBitString)
                                cT2[0] = BitStringsArraySums.Sum(trace.BitStrings);
                            else
                                cT2[0] = BitStringsArraySums.Sum(
                                        BitStringsArrayAnd.Operation(trace.BitStrings, sSF)
                                    );

                            contingencyTable2 = new ContingencyTableHelper(
                                cT2,
                                _result.AllObjectsCount,
                                trace.Identifier.AttributeGuid
                                );
                            #endregion

                            Hypothesis hypothesis = new Hypothesis();
                            hypothesis.SetFormula(MarkEnum.Attribute, trace.Identifier);
                            hypothesis.SetFormula(MarkEnum.Condition, cS.Identifier);
                            hypothesis.SetFormula(MarkEnum.FirstSet, fS.Identifier);
                            hypothesis.SetFormula(MarkEnum.SecondSet, sS.Identifier);
                            hypothesis.ContingencyTableA = contingencyTable1.ContingencyTable;
                            hypothesis.ContingencyTableB = contingencyTable2.ContingencyTable;

                            if (evaluator.VerifyIsCompleteSDSecondSet(contingencyTable2, sDFirstSetValues, hypothesis))
                                goto finish;
                        }
                    }
                }
            }
        finish:
            evaluator.Flush();
            resultFinish();
        }

        public override IEnumerable<KeyValuePair<string, BitStringIce>> TraceBoolean(int[] CountVector, Ferda.Modules.GuidStruct attributeGuid, int skipFirstN)
        {
            throw new Exception("The method or operation is not implemented.");
        }
    }
}
