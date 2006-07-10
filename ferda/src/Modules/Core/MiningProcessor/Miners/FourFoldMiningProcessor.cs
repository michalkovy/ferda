using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Ferda.Guha.Data;
using Ferda.Guha.Math.Quantifiers;
using Ferda.Guha.MiningProcessor.BitStrings;
using Ferda.Guha.MiningProcessor.QuantifierEvaluator;
using Ferda.Modules;

namespace Ferda.Guha.MiningProcessor.Miners
{
    public class FourFoldMiningProcessor : MiningProcessorBase
    {
        #region Fields

        private readonly IEntityEnumerator _antecedent;
        private readonly IEntityEnumerator _succedent;
        private readonly IEntityEnumerator _condition;

        #endregion

        public FourFoldMiningProcessor(
            BooleanAttribute[] booleanAttributes,
            CategorialAttribute[] categorialAttributes,
            QuantifierBaseFunctionsPrx[] quantifiers,
            TaskRunParams taskParams,
            BitStringGeneratorProviderPrx taskFuncPrx
            )
            : base(quantifiers, taskFuncPrx, taskParams)
        {
            // Validate quantifiers
            bool notOnlyFirstSetOperationMode;
            bool needsNumericValues; // ignore
            bool notOnlyDeletingMissingInformation;
            // ignore allways study ... for that purpose if user join new quantifier to task box after run to see its values in Result Browser
            CardinalityEnum maximalRequestedCardinality; // UNDONE ignored

            _quantifiers.ValidRequests(
                out notOnlyFirstSetOperationMode,
                out needsNumericValues,
                out notOnlyDeletingMissingInformation,
                out maximalRequestedCardinality
                );

            if (notOnlyFirstSetOperationMode)
                throw Modules.Exceptions.BadParamsError(null, null,
                                                        "Property \"Operation mode\" is not set to \"FirstSetOperationMode\" in some quantifier.",
                                                        restrictionTypeEnum.OtherReason);

            // Create cedent traces
            _antecedent = CreateBooleanAttributeTrace(MarkEnum.Antecedent, booleanAttributes, true);
            _succedent = CreateBooleanAttributeTrace(MarkEnum.Succedent, booleanAttributes, false);
            _condition = CreateBooleanAttributeTrace(MarkEnum.Condition, booleanAttributes, true);
        }

        private class NineFoldTableOfBitStrings
        {
            public IBitString pSpA;
            public IBitString pSxA;
            public IBitString pSnA;

            public IBitString xSpA;
            public IBitString xSxA;
            public IBitString xSnA;

            public IBitString nSpA;
            public IBitString nSxA;
            public IBitString nSnA;
        }

        public override Result Trace(out SerializableResultInfo rInfo)
        {
            Debug.Assert(TaskParams.taskType == TaskTypeEnum.FourFold);
            Result result = new Result();
            result.TaskTypeEnum = TaskParams.taskType;

            rInfo = new SerializableResultInfo();
            rInfo.StartTime = DateTime.Now;
            rInfo.TotalNumberOfRelevantQuestions = TotalCount;

            IEvaluator evaluator;
            if (TaskParams.evaluationType == TaskEvaluationTypeEnum.FirstN)
                evaluator = new FirstN(_quantifiers, result, rInfo, TaskParams);
            else
                throw new NotImplementedException();

            long allObjectsCount = Int64.MinValue;

            MissingInformation missingInformation = new MissingInformation();
            IBitString xS;
            IBitString nS;
            IBitString xA;
            IBitString nA;
            IBitString xC;
            NineFoldTableOfBitStrings nineFT = new NineFoldTableOfBitStrings();

            foreach (IBitString pS in _succedent)
            {
                GetNegationAndMissings(pS, out xS, out nS, _succedent.UsedAttributes, missingInformation);
                if (allObjectsCount < 0)
                    allObjectsCount = pS.Length;
                if (allObjectsCount < 0)
                    throw new ApplicationException("Unknown all objects count.");
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
                        rInfo.NumberOfVerifications++;
                        NineFoldContingencyTablePair fft = new NineFoldContingencyTablePair();
                        if (pC is FalseBitString)
                        {
                            // empty contingency table (zeros)
                        }
                        if (pC is EmptyBitString)
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
                            GetMissings(pC, out xC, _condition.UsedAttributes, missingInformation);
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
                        hypothesis.SetFormula(MarkEnum.Attribute, pA.Identifier);
                        hypothesis.SetFormula(MarkEnum.Condition, pC.Identifier);
                        hypothesis.ContingencyTableA = contingencyTable.ContingencyTable;
                        //h.NumericValuesAttributeGuid = contingencyTable.NumericValuesAttributeGuid;

                        if (evaluator.VerifyIsComplete(contingencyTable, hypothesis))
                            goto finish;
                    }
                }
            }
        finish:
            evaluator.Flush();
            rInfo.EndTime = DateTime.Now;
            result.AllObjectsCount = allObjectsCount;
            return result;
        }

        protected override void getCedents(out ICollection<IEntityEnumerator> booleanCedents,
                                           out ICollection<CategorialAttributeTrace[]> categorialCedents)
        {
            booleanCedents = new IEntityEnumerator[] { _antecedent, _succedent, _condition };
            categorialCedents = null;
        }
    }
}
