// FourFoldMiningProcessor.cs - core functionality of 4ft miner
//
// Author:  Tomáš Kuchaø <tomas.kuchar@gmail.com>
//          Alexander Kuzmin <alexander.kuzmin@gmail.com> (Virtual attribute functionality)
//
// Copyright (c) 2007 Tomáš Kuchaø, Alexander Kuzmin
//
// This program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA

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

        



        #region Testing
        private long _relevantQuestionsCount = 0;

        private bool MustStop()
        {
            if (_relevantQuestionsCount > this.TaskParams.maxSizeOfResult)
            {
                return true;
            }
            else
            {
                _relevantQuestionsCount++;
                return false;
            }
        }
        #endregion

        public override IEnumerable<KeyValuePair<string, BitStringIce>> TraceBoolean(int[] countVector, GuidStruct attributeGuid, int skipFirstN)
        {
            if (skipFirstN >= this.TaskParams.maxSizeOfResult)
            {
                ProgressSetValue(100, "Reading " + skipFirstN.ToString() + " bitstrings from cache");
                yield break;
            }
            ProgressSetValue(-1, "Beginning of attributes trace.");
            //       return false;
            resultInit();
            CountVector = countVector;

            
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

            int step = 0;

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
                        if (MustStop())
                        {
                            yield break;
                        }

                        if (step < skipFirstN)
                        {
                            step++;
                            continue;
                        }
                        if (skipFirstN > 0)
                        {
                            if (this.TaskParams.maxSizeOfResult > 0)
                            {
                                ProgressSetValue((float)step / (float)this.TaskParams.maxSizeOfResult,
                                    "Skipped " + step.ToString() + " steps, using cache");
                            }
                        }
                        step++;
                        GetNegationAndMissings(pA, out xA, out nA, _antecedent.UsedAttributes, missingInformation);

                        //cycle through countvector-based masks
                        for (int i = 0; i < CountVector.Length; i++)
                        {
                            NineFoldContingencyTablePair fft = new NineFoldContingencyTablePair();

                            //if countvector contains zero, it means that there is a record
                            //in the master table which has no corresponding record in the detail table
                            //in order to keep the correct length of the yielded bitstring
                            //for every missing record from the MT we add an empty contingency table
                            if (CountVector[i] > 0)
                            {
                                IBitString mask = Masks[i];

                                fft.f111 = nineFT.pApB.And(pA.And(Masks[i])).Sum;
                                fft.f1x1 = nineFT.pAxB.And(pA.And(Masks[i])).Sum;
                                fft.f101 = nineFT.pAnB.And(pA.And(Masks[i])).Sum;

                                fft.fx11 = nineFT.pApB.And(xA.And(Masks[i])).Sum;
                                fft.fxx1 = nineFT.pAxB.And(xA.And(Masks[i])).Sum;
                                fft.fx01 = nineFT.pAnB.And(xA.And(Masks[i])).Sum;

                                fft.f011 = nineFT.pApB.And(nA.And(Masks[i])).Sum;
                                fft.f0x1 = nineFT.pAxB.And(nA.And(Masks[i])).Sum;
                                fft.f001 = nineFT.pAnB.And(nA.And(Masks[i])).Sum;

                                fft.f11x = nineFT.xApB.And(pA.And(Masks[i])).Sum;
                                fft.f1xx = nineFT.xAxB.And(pA.And(Masks[i])).Sum;
                                fft.f10x = nineFT.xAnB.And(pA.And(Masks[i])).Sum;

                                fft.fx1x = nineFT.xApB.And(xA.And(Masks[i])).Sum;
                                fft.fxxx = nineFT.xAxB.And(xA.And(Masks[i])).Sum;
                                fft.fx0x = nineFT.xAnB.And(xA.And(Masks[i])).Sum;

                                fft.f01x = nineFT.xApB.And(nA.And(Masks[i])).Sum;
                                fft.f0xx = nineFT.xAxB.And(nA.And(Masks[i])).Sum;
                                fft.f00x = nineFT.xAnB.And(nA.And(Masks[i])).Sum;                                
                            }
                            ContingencyTableHelper contingencyTable = new ContingencyTableHelper(
                                    fft.ContingencyTable,
                                    _result.AllObjectsCount
                                    );

                            //VerifyIsComplete means no buffer is left.
                            //If not all relevant questions have been
                            //generated and verified, will stop yielding bitstrings
                            if (evaluator.VerifyIsComplete(contingencyTable, new Hypothesis()))
                                break;
                        }                       

                        //vector to be yielded as bitstring
                        bool[] evalVector = evaluator.GetEvaluationVector();

                        int _arraySize = (CountVector.Length + _blockSize - 1) / _blockSize;

                        long[] _tmpString = new long[_arraySize];
                        _tmpString.Initialize();

                        //here we create virtual attribute name
                        //based on relevant question parameters

                        #region Compose virtual attribute name

                        string _yieldStringName = String.Empty;

                        if (!(pA.Identifier is IEmptyBitString))
                        {
                            _yieldStringName = 
                                MarkEnum.Antecedent.ToString() +
                            ": " + pA.Identifier;
                        }

                        if (!(pS.Identifier is IEmptyBitString))
                        {
                            if (!String.IsNullOrEmpty(_yieldStringName))
                            {
                                _yieldStringName = _yieldStringName + ", ";
                            }
                                _yieldStringName = _yieldStringName +
                                    MarkEnum.Succedent.ToString() +
                                ": " + pS.Identifier;
                        }

                        if (!(pC.Identifier is IEmptyBitString))
                        {
                            if (!String.IsNullOrEmpty(_yieldStringName))
                            {
                                _yieldStringName = _yieldStringName + ", ";
                            }
                            _yieldStringName = _yieldStringName +
                                MarkEnum.Condition.ToString() +
                            ": " + pC.Identifier;
                        }

                        #endregion

                        for (int i = 0; i < evalVector.Length; i++)
                        {
                            if (evalVector[i])
                                setTrueBit(i, _tmpString);
                        }

                        yield return new KeyValuePair<string, BitStringIce>(
                        _yieldStringName,
                        new BitStringIce(_tmpString, CountVector.Length));
                        evaluator.Flush();
                    }
                }
            }
        }
    }
}