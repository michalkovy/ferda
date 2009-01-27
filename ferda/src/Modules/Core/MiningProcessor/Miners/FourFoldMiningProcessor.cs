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
    /// <summary>
    /// The 4FT procedure mining processor
    /// </summary>
    public class FourFoldMiningProcessor : MiningProcessorBase
    {
        /// <summary>
        /// Class representing a mining setting. It has all
        /// the information needed for verification of one relevant question.
        /// </summary>
		private class MiningSetting
		{
            //positive antecedent, positive succedent, positive condition
			IBitString pA, pS, pC;
            //nine fold table of condition and succedent
			nineFoldTableOfBitStrings nineFT;
            //quantifier evaluator
			IEvaluator evaluator;
            //set of used attributes
            Set<String> usedAttributes;
            //count of all objects that is mined upon
            int allObjectsCount;
			
			///<summary>
			/// Default constructor of the class.
			/// </summary>
			/// <param name="pA">Positive antecedent bit string</param>
			/// <param name="pS">Positive succedent bit string</param>
			/// <param name="pC">Positive condition bit string</param>
            /// <param name="nineFT">Nine fold table of condition and succedent</param>
			/// <param name="evaluator">Quantifier evaluator</param>
            /// <param name="allObjectsCount">
            /// Count of all objects that is mined upon
            /// (considering condition)
            /// </param>
            /// <param name="usedAttributes">Set of used attributes</param>
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
			
            /// <summary>
            /// Positive antecedent bit string
            /// </summary>
			public IBitString PA
			{
				set {
					pA = value;
				}
				
				get {
					return pA;
				}
			}
			
            /// <summary>
            /// Positive succedent bit string
            /// </summary>
			public IBitString PS
			{
				set {
					pS = value;
				}
				
				get {
					return pS;
				}
			}
			
            /// <summary>
            /// Positive condition bit string
            /// </summary>
			public IBitString PC
			{
				set {
					pC = value;
				}
				
				get {
					return pC;
				}
			}
			
            /// <summary>
            /// Nine fold table of condition and succedent
            /// </summary>
			public nineFoldTableOfBitStrings NineFT
			{
				set {
					nineFT = value;
				}
				
				get {
					return nineFT;
				}
			}
			
            /// <summary>
            /// Quantifier evaluator
            /// </summary>
			public IEvaluator Evaluator
			{
				set {
					evaluator = value;
				}
				
				get {
					return evaluator;
				}
			}

            /// <summary>
            /// Set of used attributes
            /// </summary>
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

            /// <summary>
            /// Count of all objects that is mined upon
            /// (considering condition)
            /// </summary>
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

        #region Private fields

        /// <summary>
        /// Antecedent Boolean attribute entity enumerator
        /// </summary>
        private IEntityEnumerator _antecedent;

        /// <summary>
        /// Succedent Boolean attribute entity enumerator
        /// </summary>
        private IEntityEnumerator _succedent;

        /// <summary>
        /// Condition Boolean attribute entity enumerator
        /// </summary>
        private IEntityEnumerator _condition;

        /// <summary>
        /// Count of relevant questions so far returned
        /// by the virtual 4FT attribute
        /// </summary>
        private long _relevantQuestionsCount = 0;

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor of the class.
        /// </summary>
        /// <param name="booleanAttributes">Boolean attributes connected to the task</param>
        /// <param name="categorialAttributes">Categorial attributes connected to the task</param>
        /// <param name="quantifiers">Quantifiers connected to the task</param>
        /// <param name="taskFuncPrx">Proxy of the task functions</param>
        /// <param name="taskParams">Task parameters</param>
        /// <param name="progressListener">The progress listener.</param>
        /// <param name="progressBarPrx">The progress bar PRX.</param>
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

        #endregion

        #region MiningProcessorBase overrides

        /// <summary>
        /// Returns all cedents of the particular miner, for this class
        /// the antecedent, succedent and condition.
        /// </summary>
        /// <example>
        /// The 4FT procedure returns <c>Antecedent</c>, <c>Succedent</c> and <c>Condition</c>
        /// as Boolean cedents and <c>null</c> as categorial cedents.
        /// </example>
        /// <param name="booleanCedents">List of Boolean cedents to be returned</param>
        /// <param name="categorialCedents">List of categorial cedents to be returned</param>
        protected override void getCedents(out ICollection<IEntityEnumerator> booleanCedents,
                                           out ICollection<CategorialAttributeTrace[]> categorialCedents)
        {
            booleanCedents = new IEntityEnumerator[] { _antecedent, _succedent, _condition };
            categorialCedents = null;
        }

        /// <summary>
        /// Type of the task of the particular miner (4FT)
        /// </summary>
        public override TaskTypeEnum TaskType
        {
            get { return TaskTypeEnum.FourFold; }
        }

        /// <summary>
        /// Returns array of categorial attributes (in form of
        /// <see cref="T:Ferda.Guha.MiningProcessor.Generation.CategorialAttributeTrace"/>
        /// that should support numeric values
        /// (for purposes of mining).
        /// </summary>
        /// <returns>Array of categorial attributes</returns>
        protected override CategorialAttributeTrace[] attributesWhichShouldSupportNumericValues()
        {
            return null;
        }

        /// <summary>
        /// Returns array of categorial attributes (in form of
        /// <see cref="T:Ferda.Guha.MiningProcessor.Generation.CategorialAttributeTrace"/>
        /// that request some form of cardinality
        /// (for purposes of mining).
        /// </summary>
        /// <example>
        /// The CF procedure mines upon one categorial attribute. The procedure needs for this
        /// attribute to be cardinal, because it needs to order the attributes to count the 
        /// quantifiers.
        /// </example>
        /// <returns>Array of categorial attributes</returns>
        protected override List<CategorialAttributeTrace[]> attributesWhichRequestsSomeCardinality()
        {
            return null;
        }

        /// <summary>
        /// Prepares traces (entity enumerators) for a given miner
        /// </summary>
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

        /// <summary>
        /// Algoritm for tracing the relevant questions and verifying them against
        /// the quantifier. The algoritm computes valid hypotheses.
        /// </summary>
        public override void Trace()
        {
            //code common to trace and trace boolean methods
            InitializeTrace();
            IEvaluator evaluator = CreateEvaluator(TaskParams.evaluationType, false);

            MissingInformation missingInformation = MissingInformation.GetInstance();
            //missing succedent
            IBitString xS;
            //negative succedent
            IBitString nS;
            //missing condition
            IBitString xC;
            nineFoldTableOfBitStrings nineFT;

            //MiningSettingCollectionWithMiningThreads<MiningSetting> miningThreads = new MiningSettingCollectionWithMiningThreads<FourFoldMiningProcessor.MiningSetting>(mine, finished);
            //System.Threading.ThreadPool threadPool = new System.Threading.ThreadPool();

            _mineRuns = 0;
            int mineToRun = 0;
            foreach (IBitString pC in _condition)
            {
                //Gets missing value for the condition
                GetMissings(pC, out xC, _condition.UsedAttributes, missingInformation);

                //set actual count of objects with respect to condition
                long actCount = SetActConditionCountOfObjects(pC);

                foreach (IBitString pS in _succedent)
                {
                    if (pS is IEmptyBitString)
                        continue;
                    GetNegationAndMissings(pS, out xS, out nS, _succedent.UsedAttributes, missingInformation);

                    //Fills the nine fold table (A is condition, B is succedent)
                    nineFT = FillNineFoldConditionSuccedent(pS, nS, xS, pC, xC, actCount);

                    foreach (IBitString pA in _antecedent)
                    {
                        MiningSetting miningSetting =
                            new MiningSetting(pA, pS, pC, nineFT, evaluator, _succedent.UsedAttributes, 
                            (int)_result.AllObjectsCount);

                        //miningThreads.AddSetting(miningSetting);
                        mineToRun++;
                        if (_useThreads)
                        {
                            System.Threading.ThreadPool.UnsafeQueueUserWorkItem(mine, miningSetting);
                        }
                        else
                        {
                            mine(miningSetting);
                        }
                        if (finished())
                            goto finish;
                    }
                }
            }

            //waiting for all the threads still computing to 
            //finish
            while (mineToRun != _mineRuns && (!finished()))
            {
                System.Threading.Thread.Sleep(50);
            }
        finish:
            evaluator.Flush();
            resultFinish();
            //miningThreads.Finish();
        }

        /// <summary>
        /// Algoritm for computing all the relevant questions and veryfiing them
        /// agaist the quantifier. It is used in the virtual attributes.
        /// In contrary to the <see cref="MiningProcessorBase.Trace"/>
        /// method, this method returns not only valid hypotheses, but all the relevant
        /// questions and states with 0 or 1 iff the relevant question is valid for
        /// given record of the master data table.
        /// </summary>
        /// <param name="countVector">
        /// The count vector is an array of integers 
        /// representing for each item in the master data table how many records are
        /// in the detail data table corresponding to the item. 
        /// More information can
        /// be found in <c>svnroot/publications/diplomky/Kuzmos/diplomka.pdf</c> or
        /// in <c>svnroot/publications/Icde08/ICDE.pdf</c>.
        /// </param>
        /// <param name="attributeGuid">Identification of newly created attribute</param>
        /// <param name="skipFirstN">Skip first N steps of the computation</param>
        /// <returns>A key/value pair: the key is identification of the virtual hypothesis attribute,
        /// the value is bit string corresponding to this attribute.</returns>
        public override IEnumerable<KeyValuePair<string, BitStringIce>> TraceBoolean(int[] countVector, GuidStruct attributeGuid, int skipFirstN)
        {
            //initialization of multirelational stuff
            if (skipFirstN >= this.TaskParams.maxSizeOfResult)
            {
                ProgressSetValue(1, "Reading " + skipFirstN.ToString() + " bitstrings from cache");
                yield break;
            }
            CountVector = countVector;

            //code common to trace and trace boolean methods
            InitializeTrace();
            IEvaluator evaluator = CreateEvaluator(TaskParams.evaluationType, true);

            MissingInformation missingInformation = MissingInformation.GetInstance();
            //missing succedent
            IBitString xS;
            //negative succedent
            IBitString nS;
            //missing antecedent
            IBitString xA;
            //negative antecedent
            IBitString nA;
            //missing condition
            IBitString xC;
            nineFoldTableOfBitStrings nineFT;

            int step = 0;

            foreach (IBitString pC in _condition)
            {
                //Gets missing value for the condition
                GetMissings(pC, out xC, _condition.UsedAttributes, missingInformation);

                //set actual count of objects with respect to condition
                SetActConditionCountOfObjects(pC);

                foreach (IBitString pS in _succedent)
                {
                    if (pS is IEmptyBitString)
                        continue;
                    GetNegationAndMissings(pS, out xS, out nS, _succedent.UsedAttributes, missingInformation);

                    //Fills the nine fold table (A is condition, B is succedent)
                    nineFT = FillNineFoldConditionSuccedent(pS, nS, xS, pC, xC, Int32.MinValue);

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
                            if (!finished())
                            {
                                evaluator.VerifyIsComplete(contingencyTable, new Hypothesis(), setFinished);
                            }
                            if (finished())
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

        #endregion

        #region Private methods

        /// <summary>
        /// Returns true, if the bit string is everything else except fuzzy bit 
        /// string
        /// </summary>
        /// <param name="source">The source bit string</param>
        /// <returns>
        /// Returns true, if the bit string is everything else except fuzzy bit 
        /// string
        /// </returns>
        private bool IsNotFuzzyBitString(IBitString source)
        {
            if (source is FuzzyBitString)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Executes one verification.
        /// </summary>
        /// <param name="ob">Mining settings</param>
        private void mine(Object ob)
		{
			MiningSetting miningSetting = ob as MiningSetting;
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

            NineFoldContingencyTablePair fft = new NineFoldContingencyTablePair();

            if (IsNotFuzzyBitString(pA) && IsNotFuzzyBitString(pS) && 
                IsNotFuzzyBitString(pC) && IsNotFuzzyBitString(xA))
            {
                int pASum = Convert.ToInt32(pA.Sum);
                int xASum = Convert.ToInt32(xA.Sum);
                int nineFTpApBSum = Convert.ToInt32(nineFT.pApB.Sum);
                int nineFTpAxBSum = Convert.ToInt32(nineFT.pAxB.Sum);
                int nineFTxApBSum = Convert.ToInt32(nineFT.xApB.Sum);
                int nineFTxAxBSum = Convert.ToInt32(nineFT.xAxB.Sum);
                int nineFTpAnBSum = Convert.ToInt32(nineFT.pAnB.Sum);
                int nineFTxAnBSum = Convert.ToInt32(nineFT.xAnB.Sum);

                int f111, fx11, f1x1, fxx1;
                int f11x, fx1x, f1xx, fxxx;

                BitString.CrossAndSum(pA, xA, nineFT.pApB, nineFT.pAxB,
                    pASum, xASum, nineFTpApBSum, nineFTpAxBSum,
                    out f111, out fx11, out f1x1, out fxx1);
                BitString.CrossAndSum(pA, xA, nineFT.xApB, nineFT.xAxB,
                    pASum, xASum, nineFTxApBSum, nineFTxAxBSum,
                    out f11x, out fx1x, out f1xx, out fxxx);

                fft.f111 = f111;
                fft.f1x1 = f1x1;
                fft.f101 = pASum - f111 - f1x1;

                fft.fx11 = fx11;
                fft.fxx1 = fxx1;
                fft.fx01 = xASum - fx11 - fxx1;

                fft.f011 = nineFTpApBSum - fft.f111 - fft.fx11;
                fft.f0x1 = nineFTpApBSum - fft.f1x1 - fft.fxx1;
                fft.f001 = nineFTpAnBSum - fft.f101 - fft.fx01;

                fft.f11x = f11x;
                fft.f1xx = f1xx;
                fft.f10x = pASum - f11x - f1xx;

                fft.fx1x = fx1x;
                fft.fxxx = fxxx;
                fft.fx0x = xASum - fx1x - fxxx;

                fft.f01x = nineFTxApBSum - fft.f111 - fft.fx11;
                fft.f0xx = nineFTxAxBSum - fft.f1x1 - fft.fxx1;
                fft.f00x = nineFTxAnBSum - fft.f101 - fft.fx01;
            }
            else
            {
                
            }
			
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

			if (!finished())
			{
				evaluator.VerifyIsComplete(contingencyTable, hypothesis, setFinished);
			}

			_mineRuns++;
		}

        /// <summary>
        /// Method is used for retrieving virtual attributes. Iff
        /// the method returns true, the generation must stop
        /// (the method generated more virtual attributes than
        /// maximal desired amount).
        /// Otherwise, it increases the count of returned
        /// relevatant questions.
        /// </summary>
        /// <returns>Iff generation of virutal attributes must stop</returns>
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

        /// <summary>
        /// Initializes trace of the procedure. 
        /// This code is common
        /// to <see cref="FourFoldMiningProcessor.Trace"/> and
        /// <see cref="FourFoldMiningProcessor.TraceBoolean"/>
        /// methods.
        /// </summary>
        private void InitializeTrace()
        {
            if (!ProgressSetValue(-1, "Beginning of attributes trace."))
                return;
            resultInit();
        }

        /// <summary>
        /// Gets count of all object with respect to a condition.
        /// When there is a non empty condition, the count of all
        /// objects equals to sum of the positive condition bit string.
        /// </summary>
        /// <param name="positiveCondition">The positive condition
        /// bit string</param>
        /// <returns>Count of all objects with respect
        /// to condition.</returns>
        private long SetActConditionCountOfObjects(IBitString positiveCondition)
        {
            long result;

            if (positiveCondition is IEmptyBitString)
                result = _result.AllObjectsCount;
            else
                result = positiveCondition.NonZeroBitsCount;

            ActConditionCountOfObjects = result;
            return result;
        }

        /// <summary>
        /// Fills the nine-fold contingency table with values of condition and
        /// succedent (A is condition, B is succedent) from provided bit strings.
        /// The <paramref name="allObjectCount"/> serves for computing
        /// optimalization. It should not be <c>Int32.MinValue</c> in the 
        /// <see cref="FourFoldMiningProcessor.Trace"/> method and
        /// should be <c>Int32.MinValue</c> in the 
        /// <see cref="FourFoldMiningProcessor.TraceBoolean"/> method.
        /// </summary>
        /// <param name="pS">Positive succedent bit string</param>
        /// <param name="nS">Negative succedent bit string</param>
        /// <param name="xS">Missing succedent bit string</param>
        /// <param name="pC">Positive condition bit string</param>
        /// <param name="xC">Missing condition bit string</param>
        /// <param name="allObjectsCount">Count of all objects
        /// that are mined in this relevant question. Server
        /// for computing optimalization. It should not be <c>null</c> in the 
        /// <see cref="FourFoldMiningProcessor.Trace"/> method and
        /// should be null in the 
        /// <see cref="FourFoldMiningProcessor.TraceBoolean"/> method.
        /// </param>
        /// <returns>Nine fold contingency table of condition
        /// and succedent.</returns>
        private nineFoldTableOfBitStrings FillNineFoldConditionSuccedent(
            IBitString pS, IBitString nS, IBitString xS,
            IBitString pC, IBitString xC, long allObjectsCount)
        {
            nineFoldTableOfBitStrings result = new nineFoldTableOfBitStrings();

            result.pApB = pC.And(pS);
            result.pAxB = pC.And(xS);
            result.pAnB = pC.And(nS);

            result.xApB = xC.And(pS);
            result.xAxB = xC.And(xS);
            result.xAnB = xC.And(nS);

            if (allObjectsCount == long.MinValue)
            {
                result.pAnB.Sum = allObjectsCount - result.pApB.Sum - result.pAxB.Sum;
                result.xAnB.Sum = xC.Sum - result.xApB.Sum - result.xAxB.Sum;
            }

            return result;
        }


        #endregion

        #region Thread handling

        /// <summary>
        /// If threads of this miner are finished
        /// </summary>
        private bool finishThreads = false;

        /// <summary>
        /// If more than one thread should be used
        /// (makes sense form more then 1 core processors
        /// </summary>
        private static readonly bool _useThreads = System.Environment.ProcessorCount > 1;

        /// <summary>
        /// How many times did the threads acutally computed contingency
        /// tables
        /// </summary>
        private int _mineRuns = 0;

        /// <summary>
        /// Function returns if procedure is fininshed.
        /// Thread safe.
        /// </summary>
        /// <returns>If the computing of the procedure is finished</returns>
        private bool finished()
        {
            lock (this)
            {
                return finishThreads;
            }
        }

        /// <summary>
        /// Wait callback delegate to the thread pool. Writes the finishing
        /// information. Thread safe.
        /// </summary>
        /// <param name="o">An object</param>
        private void setFinished(object o)
        {
            lock (this)
            {
                finishThreads = true;
            }
        }

        #endregion

    }
}
