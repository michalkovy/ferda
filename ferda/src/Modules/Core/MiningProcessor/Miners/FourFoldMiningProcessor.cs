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
            booleanCedents = new IEntityEnumerator[] {_antecedent, _succedent, _condition};
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
            _succedent = CreateBooleanAttributeTrace(MarkEnum.Succedent, _booleanAttributes, false);

            if (!ProgressSetValue(-1, "Preparing Antecedent trace"))
                return;
            _antecedent = CreateBooleanAttributeTrace(MarkEnum.Antecedent, _booleanAttributes, true);

            if (!ProgressSetValue(-1, "Preparing Condition trace"))
                return;
            _condition = CreateBooleanAttributeTrace(MarkEnum.Condition, _booleanAttributes, true);
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

            long allObjectsCount = Int64.MinValue;

            MissingInformation missingInformation = new MissingInformation();
            IBitString xS;
            IBitString nS;
            IBitString xA;
            IBitString nA;
            IBitString xC;
            nineFoldTableOfBitStrings nineFT = new nineFoldTableOfBitStrings();

            foreach (IBitString pS in _succedent)
            {
                if (pS is IEmptyBitString)
                    continue;
                GetNegationAndMissings(pS, out xS, out nS, _succedent.UsedAttributes, missingInformation);
                if (allObjectsCount < 0)
                    allObjectsCount = pS.Length;
                if (allObjectsCount < 0)
                    throw new ApplicationException("Unable to determine \"all objects count\".");
                foreach (IBitString pA in _antecedent)
                {
                    GetNegationAndMissings(pA, out xA, out nA, _antecedent.UsedAttributes, missingInformation);

                    nineFT.pSpA = pS.And(pA);
                    nineFT.pSxA = pS.And(xA);
                    nineFT.pSnA = pS.And(nA);

                    nineFT.xSpA = xS.And(pA);
                    nineFT.xSxA = xS.And(xA);
                    nineFT.xSnA = xS.And(nA);

                    nineFT.nSpA = nS.And(pA);
                    nineFT.nSxA = nS.And(xA);
                    nineFT.nSnA = nS.And(nA);

                    foreach (IBitString pC in _condition)
                    {
                        NineFoldContingencyTablePair fft = new NineFoldContingencyTablePair();
                        if (pC is FalseBitString)
                        {
                            // empty contingency table (zeros)
                        }
                        GetMissings(pC, out xC, _condition.UsedAttributes, missingInformation);
                        if (pC is EmptyBitString || xC.Sum == 0)
                        {
                            fft.f111 = nineFT.pSpA.Sum;
                            fft.f1x1 = nineFT.pSxA.Sum;
                            fft.f101 = nineFT.pSnA.Sum;

                            fft.fx11 = nineFT.xSpA.Sum;
                            fft.fxx1 = nineFT.xSxA.Sum;
                            fft.fx01 = nineFT.xSnA.Sum;

                            fft.f011 = nineFT.nSpA.Sum;
                            fft.f0x1 = nineFT.nSxA.Sum;
                            fft.f001 = nineFT.nSnA.Sum;
                        }
                        else
                        {
                            fft.f111 = nineFT.pSpA.And(pC).Sum;
                            fft.f1x1 = nineFT.pSxA.And(pC).Sum;
                            fft.f101 = nineFT.pSnA.And(pC).Sum;

                            fft.fx11 = nineFT.xSpA.And(pC).Sum;
                            fft.fxx1 = nineFT.xSxA.And(pC).Sum;
                            fft.fx01 = nineFT.xSnA.And(pC).Sum;

                            fft.f011 = nineFT.nSpA.And(pC).Sum;
                            fft.f0x1 = nineFT.nSxA.And(pC).Sum;
                            fft.f001 = nineFT.nSnA.And(pC).Sum;

                            fft.f11x = nineFT.pSpA.And(xC).Sum;
                            fft.f1xx = nineFT.pSxA.And(xC).Sum;
                            fft.f10x = nineFT.pSnA.And(xC).Sum;

                            fft.fx1x = nineFT.xSpA.And(xC).Sum;
                            fft.fxxx = nineFT.xSxA.And(xC).Sum;
                            fft.fx0x = nineFT.xSnA.And(xC).Sum;

                            fft.f01x = nineFT.nSpA.And(xC).Sum;
                            fft.f0xx = nineFT.nSxA.And(xC).Sum;
                            fft.f00x = nineFT.nSnA.And(xC).Sum;
                        }

                        ContingencyTableHelper contingencyTable = new ContingencyTableHelper(
                            fft.ContingencyTable,
                            allObjectsCount);

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
            resultFinish(allObjectsCount);
        }
    }
}