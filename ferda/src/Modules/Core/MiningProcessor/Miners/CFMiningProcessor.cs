// CFMiningProcessor.cs - mining processor for the CF procedure
//
// Authors: Tomáš Kuchař <tomas.kuchar@gmail.com>      
// Commented by: Martin Ralbovský <martin.ralbovsky@gmail.com>
//
// Copyright (c) 2006 Tomáš Kuchař
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
    /// <summary>
    /// The CF procedure mining processor
    /// </summary>
    public class CFMiningProcessor : MiningProcessorBase
    {
        #region Private fields

        /// <summary>
        /// Traces of categorial attributes that are connected to the task
        /// </summary>
        private CategorialAttributeTrace[] _attribute;
        /// <summary>
        /// Condition Boolean attribute entity enumerator
        /// </summary>
        private IEntityEnumerator _condition;

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
        public CFMiningProcessor(
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
            afterConstructAsync().Wait(); //TODO: get rid of Wait
        }

        #endregion

        #region MiningProcessorBase overrides

        /// <summary>
        /// Returns all cedents of the particular miner, for this class only the condition.
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
            booleanCedents = new IEntityEnumerator[] { _condition };
            categorialCedents = new CategorialAttributeTrace[][] { _attribute };
        }

        /// <summary>
        /// Type of the task of the particular miner (CF)
        /// </summary>
        public override TaskTypeEnum TaskType
        {
            get { return TaskTypeEnum.CF; }
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
            return _attribute;
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
            List<CategorialAttributeTrace[]> result = new List<CategorialAttributeTrace[]>();
            result.Add(_attribute);
            return result;
        }

        /// <summary>
        /// Prepares traces (entity enumerators) for a given miner
        /// </summary>
        protected override async Task prepareAttributeTracesAsync()
        {
            if (!ProgressSetValue(-1, "Preparing Attribute trace"))
                return;
            _attribute = await CreateCategorialAttributeTraceAsync(MarkEnum.Attribute, _categorialAttributes, false, this);

            if (!ProgressSetValue(-1, "Preparing Condition trace"))
                return;
            _condition = CreateBooleanAttributeTrace(MarkEnum.Condition, _booleanAttributes, true, this);
        }

        /// <summary>
        /// Algoritm for tracing the relevant questions and verifying them against
        /// the quantifier. The algoritm computes valid hypotheses.
        /// </summary>
        public override async Task TraceAsync()
        {
            if (!ProgressSetValue(-1, "Begining of attributes trace."))
                return;
            resultInit();

            IEvaluator evaluator;
            if (TaskParams.evaluationType == TaskEvaluationTypeEnum.FirstN)
                evaluator = new FirstN(this);
            else
                throw new NotImplementedException();

            ContingencyTableHelper contingencyTable;
            double[][] cT;

            foreach (CategorialAttributeTrace trace in _attribute)
            {
                await foreach (IBitString cS in _condition)
                {
                    cT = new double[1][];
                    if (cS is IEmptyBitString)
                        cT[0] = BitStringsArraySums.Sum(await trace.GetBitStringsAsync().ConfigureAwait(false));
                    else
                        cT[0] = BitStringsArraySums.Sum(
                                BitStringsArrayAnd.Operation(await trace.GetBitStringsAsync().ConfigureAwait(false), cS)
                            );
                    contingencyTable = new ContingencyTableHelper(
                        cT,
                        _result.AllObjectsCount,
                        trace.Identifier.AttributeGuid
                        );
                    Hypothesis hypothesis = new Hypothesis();
                    hypothesis.SetFormula(MarkEnum.Attribute, trace.Identifier);
                    hypothesis.SetFormula(MarkEnum.Condition, cS.Identifier);
                    hypothesis.ContingencyTableA = contingencyTable.ContingencyTable;

                    if (!finished())
                    {
                        evaluator.VerifyIsComplete(contingencyTable, hypothesis, setFinished);
                    }
                    if (finished())
                        goto finish;
                }
            }


        finish:
            evaluator.Flush();
            resultFinish();
        }

        /// <summary>
        /// Algoritm for computing all the relevant questions and veryfiing them
        /// agaist the quantifier. It is used in the virtual attributes.
        /// In contrary to the <see cref="MiningProcessorBase.Trace()"/>
        /// method, this method returns not only valid hypotheses, but all the relevant
        /// questions and states with 0 or 1 iff the relevant question is valid for
        /// given record of the master data table.
        /// </summary>
        /// <param name="CountVector">
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
        /// <remarks>
        /// The algortihm is not implemented for the CF procedure.
        /// </remarks>
        public override IAsyncEnumerable<KeyValuePair<string, BitStringIce>> TraceBoolean(int[] CountVector, Ferda.Modules.GuidStruct attributeGuid, int skipFirstN)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion

        #region Thread handling

        /// <summary>
        /// If threads of this miner are finished
        /// </summary>
        private bool finishThreads = false;
		
        /// <summary>
        /// Function returns if procedure is fininshed.
        /// Thread safe.
        /// </summary>
        /// <returns>If the computing of the procedure is finished</returns>
		private bool finished()
		{
			lock(this)
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
			lock(this)
			{
				finishThreads = true;
			}
        }

        #endregion
    }
}
