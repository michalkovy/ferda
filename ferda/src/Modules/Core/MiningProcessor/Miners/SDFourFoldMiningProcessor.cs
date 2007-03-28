// SDFourFoldMiningProcessor.cs - core functionality of SD4ft miner
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

        public override IEnumerable<KeyValuePair<string, BitStringIce>> TraceBoolean(int[] countVector, Ferda.Modules.GuidStruct attributeGuid, int skipFirstN)
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

            int step = 0;

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

                            fSF = fS.And(pC);
                            //cycle through countvector-based masks
                            for (int i = 0; i < CountVector.Length; i++)
                            {
                                #region SD first set contingency table                                
                                fourFoldCT1 = new FourFoldContingencyTable();

                                //if countvector contains zero, it means that there is a record
                                //in the master table which has no corresponding record in the detail table
                                //in order to keep the correct length of the yielded bitstring
                                //for every missing record from the MT we add an empty contingency table
                                if (CountVector[i] > 0)
                                {
                                    fourFoldCT1.a = fourFT.pSpA.And(fSF.And(Masks[i])).Sum;
                                    fourFoldCT1.c = fourFT.pSnA.And(fSF.And(Masks[i])).Sum;

                                    fourFoldCT1.b = fourFT.nSpA.And(fSF.And(Masks[i])).Sum;
                                    fourFoldCT1.d = fourFT.nSnA.And(fSF.And(Masks[i])).Sum;
                                }
                                contingencyTable1 = new ContingencyTableHelper(
                                    fourFoldCT1.ContingencyTable,
                                    _result.AllObjectsCount
                                    );
                                #endregion

                                double[] sDFirstSetValues
                                    = evaluator.SDFirstSetValues(contingencyTable1);

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

                                    //if countvector contains zero, it means that there is a record
                                    //in the master table which has no corresponding record in the detail table
                                    //in order to keep the correct length of the yielded bitstring
                                    //for every missing record from the MT we add an empty contingency table
                                    if (CountVector[i] > 0)
                                    {
                                        fourFoldCT2.a = fourFT.pSpA.And(sSF.And(Masks[i])).Sum;
                                        fourFoldCT2.c = fourFT.pSnA.And(sSF.And(Masks[i])).Sum;

                                        fourFoldCT2.b = fourFT.nSpA.And(sSF.And(Masks[i])).Sum;
                                        fourFoldCT2.d = fourFT.nSnA.And(sSF.And(Masks[i])).Sum;
                                    }

                                    contingencyTable2 = new ContingencyTableHelper(
                                        fourFoldCT2.ContingencyTable,
                                        _result.AllObjectsCount
                                        );
                                    #endregion

                                    //VerifyIsComplete means no buffer is left.
                                    //If not all relevant questions have been
                                    //generated and verified, will stop yielding bitstrings
                                    if (evaluator.VerifyIsCompleteSDSecondSet(
                                        contingencyTable2, sDFirstSetValues, new Hypothesis()))
                                        break;
                                }
                                //vector to be yielded as bitstring
                                bool[] evalVector = evaluator.GetEvaluationVectorSD();

                                int _arraySize = (CountVector.Length + _blockSize - 1) / _blockSize;

                                long[] _tmpString = new long[_arraySize];
                                _tmpString.Initialize();

                                //here we create virtual attribute name
                                //based on relevant question parameters

                                #region Compose virtual attribute name

                                string _yieldStringName = String.Empty;

                                if (!(pA.Identifier is IEmptyBitString) && !(String.IsNullOrEmpty(pA.Identifier.ToString())))
                                {
                                    _yieldStringName =
                                        //MarkEnum.Antecedent.ToString() +
                                        "[ant]" +
                                    ": " + pA.Identifier;
                                }

                                if (!(pS.Identifier is IEmptyBitString) && !(String.IsNullOrEmpty(pS.Identifier.ToString())))
                                {
                                    if (!String.IsNullOrEmpty(_yieldStringName))
                                    {
                                        _yieldStringName = _yieldStringName + ", ";
                                    }
                                    _yieldStringName = _yieldStringName +
                                        //    MarkEnum.Succedent.ToString() +
                                    " *** [succ]" +
                                    ": " + pS.Identifier;
                                }

                                if (!(pC.Identifier is IEmptyBitString) && !(String.IsNullOrEmpty(pC.Identifier.ToString())))
                                {
                                    if (!String.IsNullOrEmpty(_yieldStringName))
                                    {
                                        _yieldStringName = _yieldStringName + ", ";
                                    }
                                    _yieldStringName = _yieldStringName +
                                        //MarkEnum.Condition.ToString() +
                                        " / [cond]" +
                                    ": " + pC.Identifier;
                                }

                                //first set identifier
                                if (!(fS is EmptyBitString) && !(String.IsNullOrEmpty(fS.Identifier.ToString())))
                                {
                                    if (!String.IsNullOrEmpty(_yieldStringName))
                                    {
                                        _yieldStringName = _yieldStringName + ", ";
                                    }
                                    _yieldStringName = _yieldStringName +
                                        " ::: " +
                                        fS.Identifier;
                                }

                                //second set identifier
                              /*  if (!(sS is EmptyBitString) && !(String.IsNullOrEmpty(sS.Identifier.ToString())))
                                {
                                    if (!String.IsNullOrEmpty(_yieldStringName))
                                    {
                                        _yieldStringName = _yieldStringName + " \u00D7 ";
                                    }
                                    _yieldStringName = _yieldStringName +
                                        sS.Identifier;
                                }*/

                                #endregion

                                for (int k = 0; k < evalVector.Length; k++)
                                {
                                    if (evalVector[k])
                                        setTrueBit(k, _tmpString);
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
            //       finish:
            //          evaluator.Flush();
            //          resultFinish();
        }
    }
}