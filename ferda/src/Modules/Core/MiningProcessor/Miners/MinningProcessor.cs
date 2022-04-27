// MinningProcessor.cs - interface and abstract class for mining processor
//
// Author:  Tomáš Kuchař <tomas.kuchar@gmail.com>
//          Alexander Kuzmin <alexander.kuzmin@gmail.com> (Virtual attribute functionality)
//
// Copyright (c) 2007 Tomáš Kuchař, Alexander Kuzmin
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
using System.Diagnostics;
using Ferda.Guha.Data;
using Ferda.Guha.Math.Quantifiers;
using Ferda.Guha.MiningProcessor.BitStrings;
using Ferda.Guha.MiningProcessor.Generation;
using Ferda.Guha.MiningProcessor.QuantifierEvaluator;
using Ferda.Guha.MiningProcessor.Results;
using Ferda.Modules;
using Ferda.Modules.Helpers.Common;
using Ferda.ModulesManager;

namespace Ferda.Guha.MiningProcessor.Miners
{
    /// <summary>
    /// Interface giving information about the count of all objects 
    /// that are mined upon (all records of the mined data matrix).
    /// </summary>
    public interface IComputeAllObjectsCount
    {
        /// <summary>
        /// Computes all objects from list of categorial attributes
        /// </summary>
        /// <param name="attributes">Categorial attributes</param>
        Task ComputeAllObjectsCountAsync(CategorialAttributeTrace[] attributes);
        /// <summary>
        /// Computes all objects from list of Boolean attributes
        /// (represented by 
        /// <see cref="T:Ferda.Guha.MiningProcessor.Generation.IEntityEnumerator"/>).
        /// </summary>
        /// <param name="attribute">Boolean attributes</param>
        Task ComputeAllObjectsCountAsync(IEntityEnumerator attribute);
    }

    /// <summary>
    /// Base abstract class providing functionality for all GUHA procedures
    /// (in Ferda terminology mining processors).
    /// </summary>
    public abstract class MiningProcessorBase : ProgressBarHandler, IComputeAllObjectsCount, ISkipOptimalization
    {
        #region Abstract members

        /// <summary>
        /// Returns all cedents of the particular miner
        /// </summary>
        /// <example>
        /// The 4FT procedure returns <c>Antecedent</c>, <c>Succedent</c> and <c>Condition</c>
        /// as Boolean cedents and <c>null</c> as categorial cedents.
        /// </example>
        /// <param name="booleanCedents">List of Boolean cedents to be returned</param>
        /// <param name="categorialCedents">List of categorial cedents to be returned</param>
        protected abstract void getCedents(out ICollection<IEntityEnumerator> booleanCedents,
                           out ICollection<CategorialAttributeTrace[]> categorialCedents);

        /// <summary>
        /// Type of the task of the particular miner (4FT, KL, CF...)
        /// </summary>
        public abstract TaskTypeEnum TaskType { get;}

        /// <summary>
        /// Returns array of categorial attributes (in form of
        /// <see cref="T:Ferda.Guha.MiningProcessor.Generation.CategorialAttributeTrace"/>
        /// that should support numeric values
        /// (for purposes of mining).
        /// </summary>
        /// <returns>Array of categorial attributes</returns>
        protected abstract CategorialAttributeTrace[] attributesWhichShouldSupportNumericValues();

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
        protected abstract List<CategorialAttributeTrace[]> attributesWhichRequestsSomeCardinality();

        /// <summary>
        /// Algoritm for tracing the relevant questions and verifying them against
        /// the quantifier. The algoritm computes valid hypotheses.
        /// </summary>
        public abstract Task TraceAsync();

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
        public abstract IAsyncEnumerable<KeyValuePair<string, BitStringIce>> TraceBoolean(int[] CountVector, GuidStruct attributeGuid, int skipFirstN);

        /// <summary>
        /// Prepares traces (entity enumerators) for a given miner
        /// </summary>
        protected abstract Task prepareAttributeTracesAsync();

        #endregion

        #region Validate quantifiers and quantifiers x attributes

        /// <summary>
        /// Tests, if there is a quantifier where the 
        /// <c>Operation mode</c> property is not set to 
        /// <c>FirstSetOperationMode</c> in some of the connected
        /// quantifiers. Throws an error, if not.
        /// </summary>
        /// <param name="actual">Actual state</param>
        /// <param name="supported">Supported state</param>
        private static void testNotOnlyFirstSetOperationMode(bool actual, bool supported)
        {
            if (!supported && actual)
                throw Modules.Exceptions.BadParamsError(
                    null,
                    null,
                    "Property \"Operation mode\" is not set to \"FirstSetOperationMode\" in some quantifier.",
                    restrictionTypeEnum.OtherReason);
        }

        /// <summary>
        /// Tests, if some quantifier need numeric values of a cardinal attribute, but
        /// the task does not support this operation. Throws an error, if not. 
        /// </summary>
        /// <param name="actual">Actual state</param>
        /// <param name="supported">Supported state</param>
        private static void testNeedsNumericValues(bool actual, bool supported)
        {
            if (actual && !supported)
                throw Modules.Exceptions.BadParamsError(
                    null,
                    null,
                    "Not implemented: some quantifier needs numeric values of cardinal attribute, but the task does not support this operations.",
                    restrictionTypeEnum.OtherReason);
        }

        /// <summary>
        /// Tests, if there is a quantifier where the 
        /// <c>Missing information handling</c> property is not set to 
        /// <c>Deleting</c>. Throws an error if not. 
        /// </summary>
        /// <param name="actual">Actual state</param>
        /// <param name="supported">Supported state</param>
        private static void testNotOnlyDeletingMissingInformation(bool actual, bool supported)
        {
            if (actual && !supported)
                throw Modules.Exceptions.BadParamsError(
                    null,
                    null,
                    "Property \"Missing information handling\" is not set to \"Deleting\" in some quantifier.",
                    restrictionTypeEnum.OtherReason);
        }

        /// <summary>
        /// Returns minimal cardinality of all the categorial attributes
        /// in the parameter <paramref name="categorialAttributes"/>.
        /// </summary>
        /// <param name="categorialAttributes">List of categorial attributes</param>
        /// <returns>Minimal cardinality</returns>
        private static CardinalityEnum minimalCardinality(List<CategorialAttributeTrace[]> categorialAttributes)
        {
            CardinalityEnum result = CardinalityEnum.Cardinal;
            if (categorialAttributes != null && categorialAttributes.Count > 0)
                foreach (CategorialAttributeTrace[] categorialAttribute in categorialAttributes)
                {
                    if (categorialAttribute != null && categorialAttribute.Length > 0)
                        foreach (CategorialAttributeTrace attributeTrace in categorialAttribute)
                        {
                            if (attributeTrace != null)
                                result = Common.LesserCardinalityEnums(result, attributeTrace.AttributeCardinality);
                        }
                }
            return result;
        }

        /// <summary>
        /// Returns true iff all the quantifiers in the parameter 
        /// <paramref name="categorialAttributes"/> support numeric values. 
        /// </summary>
        /// <param name="categorialAttributes">Array of categorial attributes</param>
        /// <returns>Numeric values are supported in all the quantifiers</returns>
        public static bool supportsNumericValues(CategorialAttributeTrace[] categorialAttributes)
        {
            if (categorialAttributes != null && categorialAttributes.Length > 0)
                foreach (CategorialAttributeTrace categorialAttribute in categorialAttributes)
                {
                    if (!categorialAttribute.SupportsNumericValues)
                        return false;
                }
            return true;
        }

        /// <summary>
        /// The method test setting of quantifiers against the attributes. It is tested,
        /// if the attributes support numeric values and have desired minimal cardinality.
        /// </summary>
        public void TestOfQuantifiersXAttributes()
        {
            StaticTestOfQuantifiers(_quantifiers, TaskType);

            bool dummy;
            bool needsNumericValues;
            CardinalityEnum maximalRequestedCardinality;

            _quantifiers.ValidRequests(
                out dummy,
                out needsNumericValues,
                out dummy,
                out maximalRequestedCardinality
                );

            if (needsNumericValues)
                if (!supportsNumericValues(attributesWhichShouldSupportNumericValues()))
                    throw Modules.Exceptions.BadParamsError(
                        null,
                        null,
                        "Not all cardinal attributes provides numeric values as requested by quantifier(s).",
                        restrictionTypeEnum.OtherReason);

            if (Common.CompareCardinalityEnums(maximalRequestedCardinality, CardinalityEnum.Nominal) > 0)
                if (Common.CompareCardinalityEnums(maximalRequestedCardinality, minimalCardinality(attributesWhichRequestsSomeCardinality())) > 0)
                    throw Modules.Exceptions.BadParamsError(
                        null,
                        null,
                        "Not all used cardinal attributes provides such semantic (cardinality) as requested by quantifier(s).",
                        restrictionTypeEnum.OtherReason);
        }

        /// <summary>
        /// Validates the quantifiers of a task. Don't forget validate quantifiers
        /// in Result Browser, for purpose if user join 
        /// new quantifier to task box after run to see its values.
        /// </summary>
        /// <param name="quantifiers">The quantifiers.</param>
        /// <param name="taskType">Type of the task.</param>
        public static void StaticTestOfQuantifiers(Quantifiers quantifiers, TaskTypeEnum taskType)
        {
            bool notOnlyFirstSetOperationMode;
            bool needsNumericValues;
            bool notOnlyDeletingMissingInformation;
            CardinalityEnum dummy;

            bool actSupportsTwoContingencyTables = Result.SupportsTwoContingencyTables(taskType);
            bool actSupportsNumericValues = (taskType == TaskTypeEnum.CF || taskType == TaskTypeEnum.SDCF);
            bool actSupportsNotOnlyDeletingMissingInformationHandling = (taskType == TaskTypeEnum.FourFold);

            quantifiers.ValidRequests(
                out notOnlyFirstSetOperationMode,
                out needsNumericValues,
                out notOnlyDeletingMissingInformation,
                out dummy
                );

            testNotOnlyFirstSetOperationMode(
                notOnlyFirstSetOperationMode,
                actSupportsTwoContingencyTables
                );

            testNeedsNumericValues(
                needsNumericValues,
                actSupportsNumericValues
                );

            testNotOnlyDeletingMissingInformation(
                notOnlyDeletingMissingInformation,
                actSupportsNotOnlyDeletingMissingInformationHandling
                );
        }

        #endregion

        #region Total count of relevant questions

        /// <summary>
        /// Private variable for computing total number of relevant questions.
        /// It also serves as a cache.
        /// </summary>
        private long _totalCount = Int64.MinValue;

        /// <summary>
        /// Cached value of total number of relevant questions.
        /// </summary>
        public long TotalCount
        {
            get
            {
                if (_totalCount == Int64.MinValue)
                {
                    ICollection<IEntityEnumerator> booleanCedents;
                    ICollection<CategorialAttributeTrace[]> categorialCedents;
                    getCedents(out booleanCedents, out categorialCedents);
                    _totalCount = totalCount(booleanCedents, categorialCedents);
                }
                return _totalCount;
            }
        }

        /// <summary>
        /// Method for computing total number of relevant questions from list of attributes
        /// The method is unchecked - does not perform overflow checking.
        /// </summary>
        /// <param name="booleanAttributes">List of Boolean Attributes enumerators</param>
        /// <param name="categorialAttributes">List of Categorial Attributes enumerators</param>
        /// <returns>Total number of relevant questions</returns>
        protected static long totalCount(ICollection<IEntityEnumerator> booleanAttributes,
                                 ICollection<CategorialAttributeTrace[]> categorialAttributes)
        {
            unchecked
            {
                long result = 1;
                if (booleanAttributes != null)
                    foreach (IEntityEnumerator cedent in booleanAttributes)
                    {
                        if (cedent != null)
                            result *= cedent.TotalCount;
                    }
                if (categorialAttributes != null)
                    foreach (CategorialAttributeTrace[] cedent in categorialAttributes)
                    {
                        if (cedent == null || cedent.Length == 0)
                            continue;
                        else
                            result *= cedent.Length;
                    }
                if (result == 1)
                    return 0;
                return result;
            }
        }

        #endregion

        #region Fields and Properties

        /// <summary>
        /// Boolean attributes connected to this task
        /// </summary>
        protected readonly BooleanAttribute[] _booleanAttributes;

        /// <summary>
        /// Categorial attributes connected to this task
        /// </summary>
        protected readonly CategorialAttribute[] _categorialAttributes;

        /// <summary>
        /// Quantifiers connected to this task
        /// </summary>
        protected readonly Quantifiers _quantifiers;

        /// <summary>
        /// Quantifiers connected to this task
        /// </summary>        
        public Quantifiers Quantifiers
        {
            get { return _quantifiers; }
        }

        /// <summary>
        /// Setting of the Base quantifier
        /// </summary>
        private readonly QuantifierSetting _baseQuantifierSetting;
        
        /// <summary>
        /// Run parameters of the task
        /// </summary>
        private readonly TaskRunParams _taskParams;

        /// <summary>
        /// The bit string generator provider of the task
        /// </summary>
        private readonly BitStringGeneratorProviderPrx _taskFuncPrx;

        /// <summary>
        /// Run parameters of the task
        /// </summary>
        public TaskRunParams TaskParams
        {
            get { return _taskParams; }
        }

        #endregion

        #region Result

        /// <summary>
        /// Structure holding information about result of the task
        /// </summary>
        protected Result _result = null;

        /// <summary>
        /// Structure holding information about result of the task
        /// </summary>
        public Result Result
        {
            get { return _result; }
        }

        /// <summary>
        /// Structure holding information about the running of the
        /// task
        /// </summary>
        protected SerializableResultInfo _resultInfo = null;

        /// <summary>
        /// Structure holding information about the running of the
        /// task
        /// </summary>        
        public SerializableResultInfo ResultInfo
        {
            get { return _resultInfo; }
        }

        /// <summary>
        /// Initiation of the result structures
        /// </summary>
        protected void resultInit()
        {
            _result = new Result();
            Debug.Assert(TaskParams.taskType == TaskType);
            _result.TaskTypeEnum = TaskType;
            _result.AllObjectsCount = _allObjectsCount;

            _resultInfo = new SerializableResultInfo();
            _resultInfo.StartTime = DateTime.Now;
            _resultInfo.TotalNumberOfRelevantQuestions = TotalCount;
        }

        /// <summary>
        /// Method used to finins writing to result structures when
        /// the task is done.
        /// </summary>
        protected void resultFinish()
        {
            _resultInfo.EndTime = DateTime.Now;
        }

        #endregion

        #region RelMining

        protected const int _blockSize = 64;
        private const long _one = 1;

        /// <summary>
        /// Method sets a true bit on 
        /// <paramref name="index"/> position
        /// in a <paramref name="array"/> bit string.
        /// </summary>
        /// <param name="index">Index of the postition</param>
        /// <param name="array">Bit string where to set the bit</param>
        protected void setTrueBit(int index, long[] array)
        {
            array[index / _blockSize] |= _one << (index % _blockSize);
        }

        /// <summary>
        /// The count vector is an array of integers 
        /// representing for each item in the master data table how many records are
        /// in the detail data table corresponding to the item. 
        /// More information can
        /// be found in <c>svnroot/publications/diplomky/Kuzmos/diplomka.pdf</c> or
        /// in <c>svnroot/publications/Icde08/ICDE.pdf</c>.
        /// </summary>
        private int[] _countVector = null;

        /// <summary>
        /// The count vector is an array of integers 
        /// representing for each item in the master data table how many records are
        /// in the detail data table corresponding to the item. 
        /// More information can
        /// be found in <c>svnroot/publications/diplomky/Kuzmos/diplomka.pdf</c> or
        /// in <c>svnroot/publications/Icde08/ICDE.pdf</c>.
        /// </summary>
        protected int[] CountVector
        {
            get
            {
                return _countVector;
            }

            set
            {
                if (value != null)
                {
                    _countVector = value;
                }
            }
        }

        /// <summary>
        /// Bit string masks of objects for multirelational data mining. The construction
        /// of comes from the <c>CountVector</c>. There are objects in the detail data
        /// table that correspond to one object of the master data table. The mask bit strings
        /// show us which objects from the detail data table correspond to one object
        /// in the master data table. These objects have 1 in the bit string. 
        /// More information can
        /// be found in <c>svnroot/publications/diplomky/Kuzmos/diplomka.pdf</c> or
        /// in <c>svnroot/publications/Icde08/ICDE.pdf</c>.        
        /// </summary>
        private BitString[] _masks = null;

        /// <summary>
        /// Bit string masks of objects for multirelational data mining. The construction
        /// of comes from the <c>CountVector</c>. There are objects in the detail data
        /// table that correspond to one object of the master data table. The mask bit strings
        /// show us which objects from the detail data table correspond to one object
        /// in the master data table. These objects have 1 in the bit string. 
        /// More information can
        /// be found in <c>svnroot/publications/diplomky/Kuzmos/diplomka.pdf</c> or
        /// in <c>svnroot/publications/Icde08/ICDE.pdf</c>.        
        /// </summary>
        protected IBitString[] Masks
        {
            get
            {
                if (_masks == null)
                {
                    //produce mask bitstrings from countvector
                    _masks = new BitString[CountVector.Length];
                    int marker = 0;
                    int length = 0;

                    //counting length of the bitstring
                    //zero value on i-th position of the countvector means
                    //that no record in the detail table corresponds
                    //to one in the master table
                    for (int i = 0; i < CountVector.Length; i++)
                    {
                      //  length += System.Math.Max(1, CountVector[i]);
                        length += CountVector[i];
                    }

                    int arraySize = (length + _blockSize - 1) / _blockSize;

                    for (int i = 0; i < _masks.Length; i++)
                    {
                        long[] tmpString = new long[arraySize];
                        tmpString.Initialize();
                        _masks[i] = new BitString(new BitStringIdentifier(
                            "123", i.ToString()),
                            length, tmpString);

                        for (int k = marker; k < marker + CountVector[i]; k++)
                        {
                            _masks[i].SetBit(k, 1f);
                        }
                        marker += CountVector[i];
                    }
                }
                return _masks;
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="T:MiningProcessorBase"/> class,
        /// default constructor of the class.
        /// </summary>
        /// <param name="booleanAttributes">Boolean attributes connected to the task</param>
        /// <param name="categorialAttributes">Categorial attributes connected to the task</param>
        /// <param name="quantifiers">Quantifiers connected to the task</param>
        /// <param name="taskFuncPrx">Proxy of the task functions</param>
        /// <param name="taskParams">Task parameters</param>
        /// <param name="progressListener">The progress listener.</param>
        /// <param name="progressBarPrx">The progress bar PRX.</param>
        protected MiningProcessorBase(
            BooleanAttribute[] booleanAttributes,
            CategorialAttribute[] categorialAttributes,
            QuantifierBaseFunctionsPrx[] quantifiers,
            BitStringGeneratorProviderPrx taskFuncPrx,
            TaskRunParams taskParams,
            ProgressTaskListener progressListener,
            ProgressBarPrx progressBarPrx
            ) : base(progressListener, progressBarPrx)
        {
            _taskParams = taskParams;
            _booleanAttributes = booleanAttributes;
            _categorialAttributes = categorialAttributes;
            _taskFuncPrx = taskFuncPrx;

            ProgressSetValue(-1, "Preparing quantifiers");

            _quantifiers = new Quantifiers(quantifiers, taskFuncPrx);
            _baseQuantifierSetting = _quantifiers.GetBaseQuantifierSetting();
        }

        /// <summary>
        /// Validation done after construction.
        /// </summary>
        protected async Task afterConstructAsync()
        {
            await prepareAttributeTracesAsync().ConfigureAwait(false);
            ProgressSetValue(-1, "Validating quantifiers and attributes.");
            TestOfQuantifiersXAttributes();
        }

        #endregion

        #region IComputeAllObjectsCount members

        /// <summary>
        /// Count of all objects in the data matrix
        /// </summary>
        protected long _allObjectsCount = -1;

        /// <summary>
        /// Computes count of all objects in the data matrix according to 
        /// categorial attribute
        /// </summary>
        /// <param name="attributes">The categorial attribute</param>
        public async Task ComputeAllObjectsCountAsync(CategorialAttributeTrace[] attributes)
        {
            if (_allObjectsCount > 0)
                return;
            if (attributes == null || attributes.Length == 0)
                return;
            foreach (CategorialAttributeTrace trace in attributes)
            {
                if ((await trace.GetBitStringsAsync().ConfigureAwait(false)).Length == 0)
                {
                    Debug.Assert(false);
                    continue;
                }
                _allObjectsCount = (await trace.GetBitStringsAsync().ConfigureAwait(false))[0].Length;
                return;
            }
        }

        /// <summary>
        /// Computes count of all objects in the data matrix according to 
        /// an entity enumerator
        /// </summary>
        /// <param name="attribute">The entity enumerator</param>
        public async Task ComputeAllObjectsCountAsync(IEntityEnumerator attribute)
        {
            if (_allObjectsCount > 0)
                return;
            //if (attribute is EmptyTrace)
            //    return;

            //puvodni verze
            if (!(attribute is EmptyTrace))
            {
                await foreach (IBitString s in attribute)
                {
                    _allObjectsCount = s.Length;
                    return;
                }
            }
            //foreach (IBitString s in attribute)
            //{
            //    _allObjectsCount = s.Length;
            //    return;
            //}
        }

        /// <summary>
        /// The method that computes the _allObjectsCount field for the first
        /// time. This method is used in order to avoid null references to the 
        /// IBitString enumerators. 
        /// </summary>
        public void FirstComputeAllObjectsCount()
        {
            if (_taskFuncPrx.GetBitStringGenerators().Length == 0)
            {
                return;
            }
            BitStringGeneratorPrx bsGen = _taskFuncPrx.GetBitStringGenerators()[0];
            if (bsGen.GetCategoriesIds().Length == 0)
            {
                return;
            }
            string category = bsGen.GetCategoriesIds()[0];
            BitStringIce bs = bsGen.GetBitString(category);
            if (bs is FuzzyBitStringIce)
            {
                FuzzyBitStringIce fbs = bs as FuzzyBitStringIce;
                _allObjectsCount = fbs.value.Length;
            }
            else
            {
                CrispBitStringIce cbs = bs as CrispBitStringIce;
                _allObjectsCount = cbs.length;
            }
        }

        #endregion

        #region Attributes traces preparing

        /// <summary>
        /// Gets a Boolean attribute setting according to a task semantic
        /// </summary>
        /// <param name="semantic">Task semantic</param>
        /// <param name="booleanAttributes">Array of all boolean attributes
        /// connected to the task</param>
        /// <returns>Boolean attribute with desired semantics.</returns>
        public static IEntitySetting GetBooleanAttributeBySemantic(MarkEnum semantic,
                                                                  BooleanAttribute[] booleanAttributes)
        {
            if (booleanAttributes == null)
                return null;
            foreach (BooleanAttribute booleanAttribute in booleanAttributes)
            {
                if (booleanAttribute.mark == semantic)
                    return booleanAttribute.setting;
            }
            return null;
        }

        /// <summary>
        /// Gets categorial attribute (represented by a bit string generator)
        /// according to a socket semantic
        /// </summary>
        /// <param name="semantic">Semantic of the categorial attribute</param>
        /// <param name="categorialAttributes">Array of all categorial attributes
        /// connected to the task</param>
        /// <returns>Categorial attribute with desired semantics</returns>
        public static BitStringGeneratorPrx[] GetCategorialAttributeBySemantic(MarkEnum semantic,
                                                                               CategorialAttribute[]
                                                                                   categorialAttributes)
        {
            if (categorialAttributes == null)
                return null;
            List<BitStringGeneratorPrx> result = new List<BitStringGeneratorPrx>();
            foreach (CategorialAttribute categorialAttribute in categorialAttributes)
            {
                if (categorialAttribute.mark == semantic)
                    result.Add(categorialAttribute.setting);
            }
            if (result.Count == 0)
                return null;
            else
                return result.ToArray();
        }

        /// <summary>
        /// Converts categorial attribute to a bit string generator representing
        /// that attribute
        /// </summary>
        /// <param name="semantic">Semantic of the categorial attribute</param>
        /// <param name="attribute">The categorial attribute</param>
        /// <returns>Bit string generator of that attribute</returns>
        public static BitStringGeneratorPrx GetCategorialAttributeBySemantic(MarkEnum semantic,
                                                                               CategorialAttribute attribute)
        {
            if (attribute == null)
                return null;
            if (attribute.mark == semantic)
            {
                return attribute.setting;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Creates a bit string trace for a boolean attribute (represented by
        /// <see cref="T:Ferda.Guha.MiningProcessor.Generation.IEntityEnumerator"/>)
        /// for a given semantic.
        /// </summary>
        /// <param name="semantic">The semantic of the Boolean attribute
        /// (antecedent, succedent...)</param>
        /// <param name="booleanAttributes">Array of Boolean attributes</param>
        /// <param name="allowsEmptyBitStrings">If the miner allows empty bit strings</param>
        /// <param name="miningProcessorBase">The mining processor base (because of static
        /// functions)</param>
        /// <returns>Entity enumerator of a boolean attribute</returns>
        public static IEntityEnumerator CreateBooleanAttributeTrace(MarkEnum semantic,
                                                                    BooleanAttribute[] booleanAttributes,
                                                                    bool allowsEmptyBitStrings,
                                                                    MiningProcessorBase miningProcessorBase)
        {
            IEntitySetting setting = GetBooleanAttributeBySemantic(semantic, booleanAttributes);
            if (setting == null)
            {
                if (!allowsEmptyBitStrings)
                    throw Exceptions.EmptyCedentIsNotAllowedError(semantic);
                else
                    return new EmptyTrace();
            }

            if (setting is IMultipleOperandEntitySetting)
            {
                IMultipleOperandEntitySetting mos = (IMultipleOperandEntitySetting)setting;
                if (mos.minLength == 0 && !allowsEmptyBitStrings)
                    throw Exceptions.EmptyCedentIsNotAllowedError(semantic);
            }

            IEntityEnumerator finalResult = Factory.Create(setting, miningProcessorBase, semantic);
            //tady dat jiny zpusob pocitani _allObjectsCount
            miningProcessorBase.FirstComputeAllObjectsCount();

            //miningProcessorBase.ComputeAllObjectsCount(finalResult);

            return finalResult;
        }

        /// <summary>
        /// Creates a categorial attribute trace of a given semantic.
        /// </summary>
        /// <param name="semantic">The semantic</param>
        /// <param name="categorialAttributes">Array of categorial attributes</param>
        /// <param name="allowEmptyCategorialCedent">If the miner allows empty categorial cedent</param>
        /// <param name="miningProcessorBase">The mining processor base (because of static
        /// functions)</param>
        /// <returns>Categorial attribute trace</returns>
        public static async Task<CategorialAttributeTrace[]> CreateCategorialAttributeTraceAsync(MarkEnum semantic,
                                                                                CategorialAttribute[] categorialAttributes,
                                                                                bool allowEmptyCategorialCedent,
                                                                                MiningProcessorBase miningProcessorBase)
        {
            CategorialAttributeTrace[] result =
                CreateCategorialAttributeTrace(semantic, categorialAttributes, allowEmptyCategorialCedent);
            await miningProcessorBase.ComputeAllObjectsCountAsync(result).ConfigureAwait(false);
            return result;
        }

        /// <summary>
        /// Creates a categorial attribute trace of a given semantic.
        /// </summary>
        /// <param name="semantic">The semantic</param>
        /// <param name="categorialAttributes">Array of categorial attributes</param>
        /// <param name="allowEmptyCategorialCedent">If the miner allows empty categorial cedent</param>
        /// <returns>Categorial attribute trace</returns>
        public static CategorialAttributeTrace[] CreateCategorialAttributeTrace(MarkEnum semantic,
                                                    CategorialAttribute[] categorialAttributes,
                                                    bool allowEmptyCategorialCedent)
        {
            BitStringGeneratorPrx[] setting = GetCategorialAttributeBySemantic(semantic, categorialAttributes);
            if (setting == null)
            {
                if (!allowEmptyCategorialCedent)
                    throw Exceptions.EmptyCedentIsNotAllowedError(semantic);
                else
                    return null;
            }
            List<CategorialAttributeTrace> result = new List<CategorialAttributeTrace>();
            foreach (BitStringGeneratorPrx prx in setting)
            {
                result.Add(new CategorialAttributeTrace(prx));
            }
            if (result.Count == 0)
            {
                if (!allowEmptyCategorialCedent)
                    throw Exceptions.EmptyCedentIsNotAllowedError(semantic);
                else
                    return null;
            }

            return result.ToArray();
        }

        /// <summary>
        /// Creates a categorial attribute trace of a given semantic
        /// </summary>
        /// <param name="semantic">The semantic</param>
        /// <param name="attribute">Categorial attributes</param>
        /// <param name="allowEmptyCategorialCedent">If the miner allows empty categorial cedent</param>
        /// <returns>Categorial attribute trace</returns>
        public static CategorialAttributeTrace CreateCategorialAttributeTrace(MarkEnum semantic,
                                                    CategorialAttribute attribute,
                                                    bool allowEmptyCategorialCedent)
        {
            BitStringGeneratorPrx setting = GetCategorialAttributeBySemantic(semantic, attribute);

            if (setting == null)
            {
                if (!allowEmptyCategorialCedent)
                    throw Exceptions.EmptyCedentIsNotAllowedError(semantic);
                else
                    return null;
            }

            return new CategorialAttributeTrace(setting);
        }

        #endregion

        #region Boolean attribute negations and missings retrieving

        /// <summary>
        /// Gets the negation and missings bit strings. Please note
        /// that both negation and missings can be instances
        /// of IEmptyBitString or IFalseBitString.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="missingInformation">The missing information IBitString provider.</param>
        public static async Task<(IBitString missings, IBitString negation)> GetNegationAndMissingsAsync(IBitString input, MissingInformation missingInformation)
        {
            IBitString missings;
            IBitString negation;
            if (input is EmptyBitString)
            {
                missings = FalseBitString.GetInstance();
                negation = FalseBitString.GetInstance();
            }
            else
            {
                missings = await missingInformation.GetValueAsync(input.Identifier.UsedAttributes).ConfigureAwait(false);
                Debug.Assert(!(missings is EmptyBitString));
                if (missings is EmptyBitString)
                    throw new ArgumentException();

                if (missings is FalseBitString)
                    negation = input.Not();
                else
                    negation = input.Or(missings).Not();
            }
            return (missings, negation);
        }

        /// <summary>
        /// Gets the negation of bit string. Please note
        /// that missings can be instances
        /// of IEmptyBitString or IFalseBitString.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="negation">The negation.</param>
        public static void GetNegation(IBitString input, out IBitString negation)
        {
            if (input is EmptyBitString)
                negation = FalseBitString.GetInstance();
            else
                negation = input.Not();
        }

        /// <summary>
        /// Gets the missings bit string. Please note
        /// that missings can be instances
        /// of IFalseBitString.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="missingInformation">The missing information IBitString provider.</param>
        public static async Task<IBitString> GetMissingsAsync(IBitString input, MissingInformation missingInformation)
        {
            IBitString missings;
            if (input is EmptyBitString)
            {
                missings = FalseBitString.GetInstance();
            }
            else
            {
                missings = await missingInformation.GetValueAsync(input.Identifier.UsedAttributes).ConfigureAwait(false);
                Debug.Assert(!(missings is EmptyBitString));
                if (missings is EmptyBitString)
                    throw new ArgumentException();
            }
            return missings;
        }

        #endregion

        #region ISkipOptimalization Members

        /// <summary>
        /// Actual count of objects (depending on condition)
        /// </summary>
        private long _actConditionCountOfObjects = -1;

        /// <summary>
        /// Actual count of objects (depending on condition)
        /// </summary>        
        public long ActConditionCountOfObjects
        {
            set
            {
                _actConditionCountOfObjects = value;
            }
        }

        /// <summary>
        /// Base skip setting treshold
        /// </summary>
        private double _baseSkipSettingTreshold = -1;
        /// <summary>
        /// Base skip setting not treshold
        /// Equals to <c>all objects count - _baseSkipSettingTreshold</c>
        /// </summary>
        private double _baseSkipSettingNotTreshold = -1;

        /// <summary>
        /// Returns a skip step optimalization setting for the Base parameter.
        /// The setting depends on many conditions (mainly how the Base
        /// quantifier is set). The method is used in bit string (entity) enumerators,
        /// in the enumeration method (it is tested iff the newly 
        /// </summary>
        /// <param name="cedentType">For which cedent type is this setting</param>
        /// <returns>The Base skip step optimalization setting</returns>
        public SkipSetting BaseSkipSetting(MarkEnum cedentType)
        {
            if (_baseQuantifierSetting == null)
                return null;

            //if the units set in the quantifier are relative to sum of all frequencies
            //of the attribute - in case of condition it is irrelevant
            if (_baseQuantifierSetting.units == UnitsEnum.RelativeToActCondition)
                if (cedentType == MarkEnum.Condition)
                    return null;
                else
                {
                    return new SkipSetting(
                        _baseQuantifierSetting.relation,
                        _baseQuantifierSetting.treshold * _actConditionCountOfObjects,
                        //opravena verze
                        //_baseSkipSettingNotTreshold = _allObjectsCount - (_baseQuantifierSetting.treshold * _actConditionCountOfObjects)
                        //puvodni verze
                        _baseSkipSettingNotTreshold = _allObjectsCount - _baseSkipSettingTreshold
                        );
                }
            if (_baseQuantifierSetting.units == UnitsEnum.RelativeToMaxFrequency)
                return null;

            if (_baseSkipSettingTreshold < 0 || _baseSkipSettingNotTreshold < 0)
            {
                switch (_baseQuantifierSetting.units)
                {

                    case UnitsEnum.AbsoluteNumber:
                    case UnitsEnum.Irrelevant:
                        _baseSkipSettingTreshold = _baseQuantifierSetting.treshold;
                        _baseSkipSettingNotTreshold = _allObjectsCount - _baseSkipSettingTreshold;
                        break;
                    case UnitsEnum.RelativeToAllObjects:
                        _baseSkipSettingTreshold = _baseQuantifierSetting.treshold * _result.AllObjectsCount;
                        _baseSkipSettingNotTreshold = _allObjectsCount - _baseSkipSettingTreshold;
                        break;
                    case UnitsEnum.RelativeToActCondition:
                    case UnitsEnum.RelativeToMaxFrequency:
                    default:
                        throw new NotImplementedException();
                }
            }
            return new SkipSetting(
                _baseQuantifierSetting.relation,
                _baseSkipSettingTreshold,
                _baseSkipSettingNotTreshold
                );
        }

        #endregion

        #region Tracing

        /// <summary>
        /// Creates an evaluator.
        /// This code is common
        /// to <see cref="FourFoldMiningProcessor.Trace"/> and
        /// <see cref="FourFoldMiningProcessor.TraceBoolean"/>
        /// methods.
        /// </summary>
        /// <param name="evaluationType">Evaluation type</param>
        /// <param name="virtualAttribute">If the evaluator
        /// is used for the virtual attribute, that is 
        /// in the <see cref="FourFoldMiningProcessor.TraceBoolean"/>
        /// method.</param>
        /// <returns>Evaluator</returns>
        protected IEvaluator CreateEvaluator(TaskEvaluationTypeEnum evaluationType,
            bool virtualAttribute)
        {
            if (virtualAttribute)
            {
                if (TaskParams.evaluationType == TaskEvaluationTypeEnum.FirstN)
                    return new FirstNNoResult(this);
                else
                    throw new NotImplementedException();
            }
            else
            {
                if (TaskParams.evaluationType == TaskEvaluationTypeEnum.FirstN)
                    return new FirstN(this);
                else
                    throw new NotImplementedException();

            }

        }

        #endregion
    }
}