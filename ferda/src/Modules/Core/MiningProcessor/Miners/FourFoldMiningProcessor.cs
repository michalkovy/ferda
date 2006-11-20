using System;
using System.Collections.Generic;
using Ferda.Guha.Math.Quantifiers;
using Ferda.Guha.MiningProcessor.BitStrings;
using Ferda.Guha.MiningProcessor.Generation;
using Ferda.Guha.MiningProcessor.QuantifierEvaluator;
using Ferda.Guha.MiningProcessor.Results;
using Ferda.ModulesManager;
using Ferda.Modules;
using System.Text;

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
            if (!ProgressSetValue(-1, "Beginning of attributes trace."))
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
            evaluator.Flush();
            resultFinish();
        }

        // private const string categoryNamePrefix = "VA 4ft: ";
        /*
                public override IEnumerable<IBitString> GetBooleanTraceEnumerator()
                {
                    GuidStruct attributeGuid = this.b
                    //return TraceBoolean(this.
                }*/

        private const int _blockSize = 64;
        private const long _one = 1;
        private void setTrueBit(int index, long[] array)
        {
            array[index / _blockSize] |= _one << (index % _blockSize);
        }

        public override IEnumerable<KeyValuePair<string, BitStringIce>> TraceBoolean(int[] CountVector, GuidStruct attributeGuid)
        {
            //  if (!ProgressSetValue(-1, "Beginning of attributes trace."))
            //       return false;
            resultInit();

            
            IEvaluator evaluator;
            if (TaskParams.evaluationType == TaskEvaluationTypeEnum.FirstN)
                evaluator = new FirstNNoResult(this);
            else
                throw new NotImplementedException();

            MissingInformation missingInformation = new MissingInformation();
            IBitString xS;
            IBitString nS;
            IBitString xA;
            IBitString nA;
            IBitString xC;
            nineFoldTableOfBitStrings nineFT = new nineFoldTableOfBitStrings();

            //produce mask bitstrings from countvector
            BitString[] masks = new BitString[CountVector.Length];
            int marker = 0;
            int length = 0;           
           
            for (int i = 0; i < CountVector.Length; i++)
            {
                length += CountVector[i];
            }

            int arraySize = (length + _blockSize - 1) / _blockSize;

            for (int i = 0; i < masks.Length; i++)
            {
                long[] tmpString = new long[arraySize];
                tmpString.Initialize();
                masks[i] = new BitString(new BitStringIdentifier(attributeGuid.value, i.ToString()),
                    length, tmpString);
            }

            for (int i = 0; i < masks.Length; i++)
            {
               /* for (int k = 0; k < marker; k++)
                {
                    tmpString[k] = 0;
                }*/

                for (int k = marker; k < marker + CountVector[i]; k++)
                {
                    masks[i].SetBit(k, true);
                }

               /* for (int k = marker + CountVector[i]; k < bitStringLength; k++)
                {
                    tmpString[k] = 0;
                }*/
                marker += CountVector[i];
            }

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

                        //cycle through countvector-based masks
                        for (int i = 0; i < masks.Length; i++)
                        {
                            NineFoldContingencyTablePair fft = new NineFoldContingencyTablePair();
                            fft.f111 = nineFT.pApB.And(pA.And(masks[i])).Sum;
                            fft.f1x1 = nineFT.pAxB.And(pA.And(masks[i])).Sum;
                            fft.f101 = nineFT.pAnB.And(pA.And(masks[i])).Sum;

                            fft.fx11 = nineFT.pApB.And(xA.And(masks[i])).Sum;
                            fft.fxx1 = nineFT.pAxB.And(xA.And(masks[i])).Sum;
                            fft.fx01 = nineFT.pAnB.And(xA.And(masks[i])).Sum;

                            fft.f011 = nineFT.pApB.And(nA.And(masks[i])).Sum;
                            fft.f0x1 = nineFT.pAxB.And(nA.And(masks[i])).Sum;
                            fft.f001 = nineFT.pAnB.And(nA.And(masks[i])).Sum;

                            fft.f11x = nineFT.xApB.And(pA.And(masks[i])).Sum;
                            fft.f1xx = nineFT.xAxB.And(pA.And(masks[i])).Sum;
                            fft.f10x = nineFT.xAnB.And(pA.And(masks[i])).Sum;

                            fft.fx1x = nineFT.xApB.And(xA.And(masks[i])).Sum;
                            fft.fxxx = nineFT.xAxB.And(xA.And(masks[i])).Sum;
                            fft.fx0x = nineFT.xAnB.And(xA.And(masks[i])).Sum;

                            fft.f01x = nineFT.xApB.And(nA.And(masks[i])).Sum;
                            fft.f0xx = nineFT.xAxB.And(nA.And(masks[i])).Sum;
                            fft.f00x = nineFT.xAnB.And(nA.And(masks[i])).Sum;

                            ContingencyTableHelper contingencyTable = new ContingencyTableHelper(
                                fft.ContingencyTable,
                                _result.AllObjectsCount
                                );

                            //VerifyIsComplete means no buffer is left.
                            //If not all relevant questions have been
                            //generated and verified, will stop yielding bitstrings
                            if (evaluator.VerifyIsComplete(contingencyTable, new Hypothesis()))
                                //goto finish;
                                //throw (new ArgumentOutOfRangeException());
                                break;

                        }

                        //here we create virtual attribute name
                        //based on relevant question parameters
                        //  Hypothesis hypothesis = new Hypothesis();
                        //   hypothesis.SetFormula(MarkEnum.Succedent, pS.Identifier);
                        //  hypothesis.SetFormula(MarkEnum.Antecedent, pA.Identifier);
                        //  hypothesis.SetFormula(MarkEnum.Condition, pC.Identifier);
                        //  hypothesis.ContingencyTableA = contingencyTable.ContingencyTable;
                        bool[] evalVector = evaluator.GetEvaluationVector();
                      //  long[] evalVectorLong = new long[evalVector.Length];

                        int _arraySize = (CountVector.Length + _blockSize - 1) / _blockSize;

                        long[] _tmpString = new long[_arraySize];
                        _tmpString.Initialize();
                        string _yieldStringName = MarkEnum.Antecedent.ToString() +
                                ": " + pA.Identifier + ", " +
                                MarkEnum.Succedent.ToString() +
                                ": " + pS.Identifier + ", " +
                                MarkEnum.Condition.ToString() +
                                ": " + pC.Identifier;

                        for (int i = 0; i < evalVector.Length; i++)
                        {
                            if (evalVector[i])
                                setTrueBit(i, _tmpString);
                               // _yieldString.SetBit(i, true);
                           // j = i;
                        }


                        yield return new KeyValuePair<string, BitStringIce>(
                        _yieldStringName,
                        new BitStringIce(_tmpString, CountVector.Length));
                        evaluator.Flush();
                    }
                }
            }
            //throw new Exception("The method or operation is not implemented.");
        }
    }
}