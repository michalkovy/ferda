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
    public class FourFoldMiningProcessor : MiningProcessorBase
    {
        protected override void getCedents(out ICollection<IEntityEnumerator> booleanCedents,
                                           out ICollection<CategorialAttributeTrace[]> categorialCedents)
        {
            booleanCedents = new IEntityEnumerator[] { _antecedent, _succedent, _condition };
            categorialCedents = null;
        }

        public override TaskTypeEnum TaskType
        {
            get { return TaskTypeEnum.FourFold; }
        }

        protected override CategorialAttributeTrace[] attributesWhichShouldSupportNumericValues()
        {
            return null;
        }

        protected override List<CategorialAttributeTrace[]> attributesWhichRequestsSomeCardinality()
        {
            return null;
        }

        private IEntityEnumerator _antecedent;
        private IEntityEnumerator _succedent;
        private IEntityEnumerator _condition;

        protected override void prepareAttributeTraces()
        {
            if (!ProgressSetValue(-1, "Preparing Succedent trace"))
                return;
            _succedent = CreateBooleanAttributeTrace(MarkEnum.Succedent, _booleanAttributes, false, this);

            if (!ProgressSetValue(-1, "Preparing Antecedent trace"))
                return;
            _antecedent = CreateBooleanAttributeTrace(MarkEnum.Antecedent, _booleanAttributes, true, this);

            if (!ProgressSetValue(-1, "Preparing Condition trace"))
                return;
            _condition = CreateBooleanAttributeTrace(MarkEnum.Condition, _booleanAttributes, true, this);
        }

        public FourFoldMiningProcessor(
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

            MissingInformation missingInformation = new MissingInformation();
            IBitString xS;
            IBitString nS;
            IBitString xA;
            IBitString nA;
            IBitString xC;
            nineFoldTableOfBitStrings nineFT = new nineFoldTableOfBitStrings();

            foreach (IBitString pC in _condition)
            {
                //if (pC is FalseBitString)
                //{
                //    // empty contingency table (zeros)
                //    continue;
                //}
                GetMissings(pC, out xC, _condition.UsedAttributes, missingInformation);

                if (pC is IEmptyBitString)
                    ActConditionCountOfObjects = (int)_result.AllObjectsCount;
                else
                    ActConditionCountOfObjects = pC.Sum;

                foreach (IBitString pS in _succedent)
                {
                    if (pS is IEmptyBitString)
                        continue;
                    GetNegationAndMissings(pS, out xS, out nS, _succedent.UsedAttributes, missingInformation);

                    // A is condition, B is succedent
                    nineFT.pApB = pC.And(pS);
                    nineFT.pAxB = pC.And(xS);
                    nineFT.pAnB = pC.And(nS);

                    nineFT.xApB = xC.And(pS);
                    nineFT.xAxB = xC.And(xS);
                    nineFT.xAnB = xC.And(nS);

                    foreach (IBitString pA in _antecedent)
                    {
                        GetNegationAndMissings(pA, out xA, out nA, _antecedent.UsedAttributes, missingInformation);

                        NineFoldContingencyTablePair fft = new NineFoldContingencyTablePair();
                        fft.f111 = nineFT.pApB.And(pA).Sum;
                        fft.f1x1 = nineFT.pAxB.And(pA).Sum;
                        fft.f101 = nineFT.pAnB.And(pA).Sum;

                        fft.fx11 = nineFT.pApB.And(xA).Sum;
                        fft.fxx1 = nineFT.pAxB.And(xA).Sum;
                        fft.fx01 = nineFT.pAnB.And(xA).Sum;

                        fft.f011 = nineFT.pApB.And(nA).Sum;
                        fft.f0x1 = nineFT.pAxB.And(nA).Sum;
                        fft.f001 = nineFT.pAnB.And(nA).Sum;

                        fft.f11x = nineFT.xApB.And(pA).Sum;
                        fft.f1xx = nineFT.xAxB.And(pA).Sum;
                        fft.f10x = nineFT.xAnB.And(pA).Sum;

                        fft.fx1x = nineFT.xApB.And(xA).Sum;
                        fft.fxxx = nineFT.xAxB.And(xA).Sum;
                        fft.fx0x = nineFT.xAnB.And(xA).Sum;

                        fft.f01x = nineFT.xApB.And(nA).Sum;
                        fft.f0xx = nineFT.xAxB.And(nA).Sum;
                        fft.f00x = nineFT.xAnB.And(nA).Sum;

                        ContingencyTableHelper contingencyTable = new ContingencyTableHelper(
                            fft.ContingencyTable,
                            _result.AllObjectsCount
                            );

                        Hypothesis hypothesis = new Hypothesis();
                        hypothesis.SetFormula(MarkEnum.Succedent, pS.Identifier);
                        hypothesis.SetFormula(MarkEnum.Antecedent, pA.Identifier);
                        hypothesis.SetFormula(MarkEnum.Condition, pC.Identifier);
                        hypothesis.ContingencyTableA = contingencyTable.ContingencyTable;
                        //h.NumericValuesAttributeGuid = contingencyTable.NumericValuesAttributeGuid;

                        if (evaluator.VerifyIsComplete(contingencyTable, hypothesis))
                            goto finish;
                    }
                }
            }

        finish:
            ProgressSetValue(1, "Completing result.");
            evaluator.Flush();
            resultFinish();
        }
    }
}