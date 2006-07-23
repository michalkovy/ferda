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
    public class SDFourFoldMiningProcessor : MiningProcessorBase
    {
        protected override void getCedents(out ICollection<IEntityEnumerator> booleanCedents,
                                           out ICollection<CategorialAttributeTrace[]> categorialCedents)
        {
            booleanCedents = new IEntityEnumerator[] { _antecedent, _succedent, _condition, _firstSet, _secondSet };
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
        private IEntityEnumerator _firstSet;
        private IEntityEnumerator _secondSet;

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

            if (!ProgressSetValue(-1, "Preparing First set trace"))
                return;
            _firstSet = CreateBooleanAttributeTrace(MarkEnum.FirstSet, _booleanAttributes, false);

            if (!ProgressSetValue(-1, "Preparing Second set trace"))
                return;
            _secondSet = CreateBooleanAttributeTrace(MarkEnum.SecondSet, _booleanAttributes, false);
        }

        public SDFourFoldMiningProcessor(
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
            if (!ProgressSetValue(-1, "Quantifiers preparation."))
                return;
            Quantifiers.UsedSDQuantifiersOpeartionModesClass quantifiers = _quantifiers.UsedSDQuantifiersOpeartionModes;
            bool skipSecondSet = false;
            bool alwaysSkipSecondSet = (
                                           quantifiers.SecondSetFrequencies
                                           || quantifiers.DifferenceOfFrequencies
                                           || quantifiers.DifferenceOfQuantifierValues
                                       );

            if (!ProgressSetValue(-1, "Begining of attributes trace."))
                return;
            resultInit();

            IEvaluator evaluator;
            if (TaskParams.evaluationType == TaskEvaluationTypeEnum.FirstN)
                evaluator = new FirstN(this);
            else
                throw new NotImplementedException();

            long allObjectsCount = Int64.MinValue;

            //MissingInformation missingInformation = new MissingInformation();
            //IBitString xS;
            IBitString nS;
            //IBitString xA;
            IBitString nA;
            //IBitString xC;
            IBitString fSF; // first set
            IBitString sSF; // second set
            fourFoldTableOfBitStrings fourFT = new fourFoldTableOfBitStrings();

            foreach (IBitString pS in _succedent)
            {
                if (pS is EmptyBitString)
                    continue;
                GetNegation(pS, out nS);
                if (allObjectsCount < 0)
                    allObjectsCount = pS.Length;
                if (allObjectsCount < 0)
                    throw new ApplicationException("Unable to determine \"all objects count\".");
                foreach (IBitString pA in _antecedent)
                {
                    GetNegation(pA, out nA);

                    fourFT.pSpA = pS.AndCloned(pA);
                    fourFT.pSnA = pS.AndCloned(nA);

                    fourFT.nSpA = nS.AndCloned(pA);
                    fourFT.nSnA = nS.AndCloned(nA);

                    foreach (IBitString pC in _condition)
                    {
                        foreach (IBitString fS in _firstSet)
                        {
                            fSF = fS.AndCloned(pC);
                            FourFoldContingencyTable fourFoldCT = new FourFoldContingencyTable();

                            fourFoldCT.a = fourFT.pSpA.AndCloned(fSF).Sum;
                            fourFoldCT.c = fourFT.pSnA.AndCloned(fSF).Sum;

                            fourFoldCT.b = fourFT.nSpA.AndCloned(fSF).Sum;
                            fourFoldCT.d = fourFT.nSnA.AndCloned(fSF).Sum;

                            skipSecondSet = false;
                            if (quantifiers.FirstSetFrequencies)
                            {
                                foreach (Quantifier q in quantifiers.QFirstSetFrequencies)
                                {
                                    if (!q.Valid(
                                             new ContingencyTableHelper(
                                                 fourFoldCT.ContingencyTable,
                                                 allObjectsCount
                                                 )
                                             )
                                        )
                                    {
                                        skipSecondSet = true;
                                        break;
                                    }
                                }
                            }

                            if (!alwaysSkipSecondSet && !skipSecondSet)
                                foreach (IBitString sS in _secondSet)
                                {
                                    switch (TaskParams.sdWorkingWithSecondSetMode)
                                    {
                                        case WorkingWithSecondSetModeEnum.Cedent1AndCedent2:
                                            sSF = sS.AndCloned(fS).AndCloned(pC);
                                            break;
                                        case WorkingWithSecondSetModeEnum.Cedent2:
                                            sSF = sS.AndCloned(pC);
                                            break;
                                        case WorkingWithSecondSetModeEnum.None:
                                        default:
                                            throw new NotImplementedException();
                                    }

                                    FourFoldContingencyTable fourFoldCT2 = new FourFoldContingencyTable();

                                    fourFoldCT2.a = fourFT.pSpA.AndCloned(sSF).Sum;
                                    fourFoldCT2.c = fourFT.pSnA.AndCloned(sSF).Sum;

                                    fourFoldCT2.b = fourFT.nSpA.AndCloned(sSF).Sum;
                                    fourFoldCT2.d = fourFT.nSnA.AndCloned(sSF).Sum;
                                }
                        }

                        //ContingencyTableHelper contingencyTable = new ContingencyTableHelper(
                        //    fft.ContingencyTable,
                        //    allObjectsCount);

                        //Hypothesis hypothesis = new Hypothesis();
                        //hypothesis.SetFormula(MarkEnum.Succedent, pS.Identifier);
                        //hypothesis.SetFormula(MarkEnum.Attribute, pA.Identifier);
                        //hypothesis.SetFormula(MarkEnum.Condition, pC.Identifier);
                        //hypothesis.ContingencyTableA = contingencyTable.ContingencyTable;
                        ////h.NumericValuesAttributeGuid = contingencyTable.NumericValuesAttributeGuid;

                        //if (evaluator.VerifyIsComplete(contingencyTable, hypothesis))
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