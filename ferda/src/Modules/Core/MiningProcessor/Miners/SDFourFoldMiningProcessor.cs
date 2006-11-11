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
            get { return TaskTypeEnum.SDFourFold; }
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
            _succedent = CreateBooleanAttributeTrace(MarkEnum.Succedent, _booleanAttributes, false, this);

            if (!ProgressSetValue(-1, "Preparing Antecedent trace"))
                return;
            _antecedent = CreateBooleanAttributeTrace(MarkEnum.Antecedent, _booleanAttributes, true, this);

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
            if (!ProgressSetValue(-1, "Begining of attributes trace."))
                return;
            resultInit();

            IEvaluator evaluator;
            if (TaskParams.evaluationType == TaskEvaluationTypeEnum.FirstN)
                evaluator = new FirstN(this);
            else
                throw new NotImplementedException();

            //MissingInformation missingInformation = new MissingInformation();
            //IBitString xS;
            IBitString nS;
            //IBitString xA;
            IBitString nA;
            //IBitString xC;
            IBitString fSF; // first set
            IBitString sSF; // second set
            
            fourFoldTableOfBitStrings fourFT = new fourFoldTableOfBitStrings();
            FourFoldContingencyTable fourFoldCT1;
            FourFoldContingencyTable fourFoldCT2;
            ContingencyTableHelper contingencyTable1;
            ContingencyTableHelper contingencyTable2;

            foreach (IBitString pS in _succedent)
            {
                if (pS is EmptyBitString)
                    continue;
                GetNegation(pS, out nS);
                foreach (IBitString pA in _antecedent)
                {
                    GetNegation(pA, out nA);

                    fourFT.pSpA = pS.And(pA);
                    fourFT.pSnA = pS.And(nA);

                    fourFT.nSpA = nS.And(pA);
                    fourFT.nSnA = nS.And(nA);

                    foreach (IBitString pC in _condition)
                    {
                        foreach (IBitString fS in _firstSet)
                        {
                            #region SD first set contingency table
                            fSF = fS.And(pC);
                            fourFoldCT1 = new FourFoldContingencyTable();

                            fourFoldCT1.a = fourFT.pSpA.And(fSF).Sum;
                            fourFoldCT1.c = fourFT.pSnA.And(fSF).Sum;

                            fourFoldCT1.b = fourFT.nSpA.And(fSF).Sum;
                            fourFoldCT1.d = fourFT.nSnA.And(fSF).Sum;

                            contingencyTable1 = new ContingencyTableHelper(
                                fourFoldCT1.ContingencyTable,
                                _result.AllObjectsCount
                                ); 
                            #endregion

                            double[] sDFirstSetValues = evaluator.SDFirstSetValues(contingencyTable1);

                            foreach (IBitString sS in _secondSet)
                            {
                                #region SD second set contingency table
                                switch (TaskParams.sdWorkingWithSecondSetMode)
                                {
                                    case WorkingWithSecondSetModeEnum.Cedent1AndCedent2:
                                        sSF = sS.And(fS).And(pC);
                                        break;
                                    case WorkingWithSecondSetModeEnum.Cedent2:
                                        sSF = sS.And(pC);
                                        break;
                                    case WorkingWithSecondSetModeEnum.None:
                                    default:
                                        throw new NotImplementedException();
                                }

                                fourFoldCT2 = new FourFoldContingencyTable();

                                fourFoldCT2.a = fourFT.pSpA.And(sSF).Sum;
                                fourFoldCT2.c = fourFT.pSnA.And(sSF).Sum;

                                fourFoldCT2.b = fourFT.nSpA.And(sSF).Sum;
                                fourFoldCT2.d = fourFT.nSnA.And(sSF).Sum;

                                contingencyTable2 = new ContingencyTableHelper(
                                    fourFoldCT2.ContingencyTable,
                                    _result.AllObjectsCount
                                    );
                                #endregion

                                Hypothesis hypothesis = new Hypothesis();
                                hypothesis.SetFormula(MarkEnum.Succedent, pS.Identifier);
                                hypothesis.SetFormula(MarkEnum.Antecedent, pA.Identifier);
                                hypothesis.SetFormula(MarkEnum.Condition, pC.Identifier);
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
            }
        finish:
            evaluator.Flush();
            resultFinish();
        }

        public override IEnumerable<IBitString> TraceBoolean(int[] CountVector, Ferda.Modules.GuidStruct attributeGuid)
        {
            throw new Exception("The method or operation is not implemented.");
        }
    }
}