// FourFoldMiningProcessor.cs - core functionality of 4ft miner
//
// Author:  Tomáš Kuchař <tomas.kuchar@gmail.com>
//          Alexander Kuzmin <alexander.kuzmin@gmail.com> (Virtual attribute functionality)
//          Michal Kováč <michal.kovac.develop@centrum.cz> (AndSum functions)
//
// Copyright (c) 2007 Tomáš Kuchař, Alexander Kuzmin, Michal Kováč
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
using Ferda.Modules.Helpers.Common;

namespace Ferda.Guha.MiningProcessor.Miners
{
    public class FourFoldMiningProcessor : MiningProcessorBase
    {
		private struct MiningSetting
		{
			IBitString pA, pS, pC;
			nineFoldTableOfBitStrings nineFT;
			IEvaluator evaluator;
            Set<String> usedAttributes;
            int allObjectsCount;
			
			///<summary>
			/// Constructor
			/// </summary>
			/// <param name="pA">An IBitString</param>
			/// <param name="pS">An IBitString</param>
			/// <param name="pC">An IBitString</param>
			/// <param name="nineFT">A  nineFoldTableOfBitStrings</param>
			/// <param name="evaluator">An IEvaluator</param>
            public MiningSetting(IBitString pA, IBitString pS, IBitString pC, nineFoldTableOfBitStrings nineFT, IEvaluator evaluator, Set<String> usedAttributes, int allObjectsCount)
			{
				this.pA = pA;
				this.pS = pS;
				this.pC = pC;
				this.nineFT = nineFT;
				this.evaluator = evaluator;
                this.usedAttributes = (new Set<string>()).Join(usedAttributes);
                this.allObjectsCount = allObjectsCount;
			}
			
			public IBitString PA
			{
				set {
					pA = value;
				}
				
				get {
					return pA;
				}
			}
			
			public IBitString PS
			{
				set {
					pS = value;
				}
				
				get {
					return pS;
				}
			}
			
			public IBitString PC
			{
				set {
					pC = value;
				}
				
				get {
					return pC;
				}
			}
			
			public nineFoldTableOfBitStrings NineFT
			{
				set {
					nineFT = value;
				}
				
				get {
					return nineFT;
				}
			}
			
			public IEvaluator Evaluator
			{
				set {
					evaluator = value;
				}
				
				get {
					return evaluator;
				}
			}

            public Set<String> UsedAttributes
            {
                set
                {
                    usedAttributes = value;
                }

                get
                {
                    return usedAttributes;
                }
            }

            public int AllObjectsCount
            {
                set
                {
                    allObjectsCount = value;
                }

                get
                {
                    return allObjectsCount;
                }
            }
		}
		
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
		
		private bool finishThreads = false;
		
		private bool finished()
		{
			lock(this)
			{
				return finishThreads;
			}
		}
		
		private void mine(MiningSetting miningSetting)
		{
			IBitString pA = miningSetting.PA;
			IBitString pS = miningSetting.PS;
			IBitString pC = miningSetting.PC;
			nineFoldTableOfBitStrings nineFT = miningSetting.NineFT;
			IEvaluator evaluator = miningSetting.Evaluator;
            Set<String> usedAttributes = miningSetting.UsedAttributes;
            int allObjectsCount = miningSetting.AllObjectsCount;
			
			MissingInformation missingInformation = MissingInformation.GetInstance();
			IBitString xA;
			
			GetMissings(pA, out xA, usedAttributes, missingInformation);
			int f111, fx11, f1x1, fxx1;
			int f11x, fx1x, f1xx, fxxx;
			
			BitString.CrossAndSum(pA, xA, nineFT.pApB, nineFT.pAxB, pA.Sum, xA.Sum, nineFT.pApB.Sum, nineFT.pAxB.Sum, out f111, out fx11, out f1x1, out fxx1);
			BitString.CrossAndSum(pA, xA, nineFT.xApB, nineFT.xAxB, pA.Sum, xA.Sum, nineFT.xApB.Sum, nineFT.xAxB.Sum, out f11x, out fx1x, out f1xx, out fxxx);
			NineFoldContingencyTablePair fft = new NineFoldContingencyTablePair();
			
			fft.f111 = f111;
			fft.f1x1 = f1x1;
			fft.f101 = pA.Sum - f111 - f1x1;
			
			fft.fx11 = fx11;
			fft.fxx1 = fxx1;
			fft.fx01 = xA.Sum - fx11 - fxx1;
			
			fft.f011 = nineFT.pApB.Sum - fft.f111 - fft.fx11;
			fft.f0x1 = nineFT.pAxB.Sum - fft.f1x1 - fft.fxx1;
			fft.f001 = nineFT.pAnB.Sum - fft.f101 - fft.fx01;
			
			fft.f11x = f11x;
			fft.f1xx = f1xx;
			fft.f10x = pA.Sum - f11x - f1xx;
			
			fft.fx1x = fx1x;
			fft.fxxx = fxxx;
			fft.fx0x = xA.Sum - fx1x - fxxx;
			
			fft.f01x = nineFT.xApB.Sum - fft.f111 - fft.fx11;
			fft.f0xx = nineFT.xAxB.Sum - fft.f1x1 - fft.fxx1;
			fft.f00x = nineFT.xAnB.Sum - fft.f101 - fft.fx01;
			
			ContingencyTableHelper contingencyTable = new ContingencyTableHelper(
				fft.ContingencyTable,
				allObjectsCount
			);
			
			Hypothesis hypothesis = new Hypothesis();
			hypothesis.SetFormula(MarkEnum.Succedent, pS.Identifier);
			hypothesis.SetFormula(MarkEnum.Antecedent, pA.Identifier);
			hypothesis.SetFormula(MarkEnum.Condition, pC.Identifier);
			hypothesis.ContingencyTableA = contingencyTable.ContingencyTable;
			//h.NumericValuesAttributeGuid = contingencyTable.NumericValuesAttributeGuid;

            lock (this)
            {
                if (!finishThreads)
                {		
					if (evaluator.VerifyIsComplete(contingencyTable, hypothesis))
						finishThreads = true;
				}
			}
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
			
            MissingInformation missingInformation = MissingInformation.GetInstance();
            IBitString xS;
            IBitString nS;
            IBitString xC;
            nineFoldTableOfBitStrings nineFT = new nineFoldTableOfBitStrings();
			
			MiningSettingCollectionWithMiningThreads<MiningSetting> miningThreads = new MiningSettingCollectionWithMiningThreads<FourFoldMiningProcessor.MiningSetting>(mine, finished);
			
            foreach (IBitString pC in _condition)
            {
                //if (pC is FalseBitString)
                //{
                //    // empty contingency table (zeros)
                //    continue;
                //}
                GetMissings(pC, out xC, _condition.UsedAttributes, missingInformation);
				int pCSum = (pC is IEmptyBitString) ? (int)_result.AllObjectsCount : pC.Sum;
				
				ActConditionCountOfObjects = pCSum;
				
                foreach (IBitString pS in _succedent)
                {
                    if (pS is IEmptyBitString)
                        continue;
                    GetNegationAndMissings(pS, out xS, out nS, _succedent.UsedAttributes, missingInformation);
					
                    // A is condition, B is succedent
                    nineFT.pApB = pC.And(pS);
                    nineFT.pAxB = pC.And(xS);
                    nineFT.pAnB = pC.And(nS);
					nineFT.pAnB.Sum = pCSum - nineFT.pApB.Sum - nineFT.pAxB.Sum;
					
                    // should be simillar
                    nineFT.xApB = xC.And(pS);
                    nineFT.xAxB = xC.And(xS);
                    nineFT.xAnB = xC.And(nS);
					nineFT.xAnB.Sum = xC.Sum - nineFT.xApB.Sum - nineFT.xAxB.Sum;
					
                    foreach (IBitString pA in _antecedent)
                    {
                        MiningSetting miningSetting = new FourFoldMiningProcessor.MiningSetting(pA, pS, pC, nineFT, evaluator, _succedent.UsedAttributes, (int)_result.AllObjectsCount);
						
						miningThreads.AddSetting(miningSetting);
						if(finished())
							goto finish;
                    }
                }
            }
			
			finish:
            evaluator.Flush();
            resultFinish();
			miningThreads.Finish();
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
		
        /// <summary>
        /// Method that yields bitstrings for virtual 4ft attribute
        /// </summary>
        /// <param name="countVector">CountVector</param>
        /// <param name="attributeGuid">Guid of the virtual attribute box</param>
        /// <param name="skipFirstN">Number of virtual columns to skip</param>
        /// <returns>Enumerable collection</returns>
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
			
            MissingInformation missingInformation = MissingInformation.GetInstance();
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
