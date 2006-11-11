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
    public interface IComputeAllObjectsCount
    {
        void ComputeAllObjectsCount(CategorialAttributeTrace[] attributes);
        void ComputeAllObjectsCount(IEntityEnumerator attribute);
    }

    public abstract class MiningProcessorBase : IComputeAllObjectsCount, ISkipOptimalization
    {
        #region Abstract memebers

        protected abstract void getCedents(out ICollection<IEntityEnumerator> booleanCedents,
                           out ICollection<CategorialAttributeTrace[]> categorialCedents);

        public abstract TaskTypeEnum TaskType { get;}

        protected abstract CategorialAttributeTrace[] attributesWhichShouldSupportNumericValues();
        protected abstract List<CategorialAttributeTrace[]> attributesWhichRequestsSomeCardinality();

        public abstract void Trace();

        public abstract IEnumerable<IBitString> TraceBoolean(int[] CountVector, GuidStruct attributeGuid);

        protected abstract void prepareAttributeTraces();

        #endregion

        #region Validate quantifiers and quantifiers x attributes

        private static void testNotOnlyFirstSetOperationMode(bool actual, bool supported)
        {
            if (!supported && actual)
                throw Modules.Exceptions.BadParamsError(
                    null,
                    null,
                    "Property \"Operation mode\" is not set to \"FirstSetOperationMode\" in some quantifier.",
                    restrictionTypeEnum.OtherReason);
        }

        private static void testNeedsNumericValues(bool actual, bool supported)
        {
            if (actual && !supported)
                throw Modules.Exceptions.BadParamsError(
                    null,
                    null,
                    "Not implemented: some quantifier needs numeric values of cardinal attribute, but the task does not support this operations.",
                    restrictionTypeEnum.OtherReason);
        }

        private static void testNotOnlyDeletingMissingInformation(bool actual, bool supported)
        {
            if (actual && !supported)
                throw Modules.Exceptions.BadParamsError(
                    null,
                    null,
                    "Property \"Missing information handling\" is not set to \"Deleting\" in some quantifier.",
                    restrictionTypeEnum.OtherReason);
        }

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
        /// Validates the quantifiers. Dont forget validate quantifiers
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

        private long _totalCount = Int64.MinValue;

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

        protected static long totalCount(ICollection<IEntityEnumerator> booleanCedents,
                                 ICollection<CategorialAttributeTrace[]> categorialCedents)
        {
            unchecked
            {
                long result = 1;
                if (booleanCedents != null)
                    foreach (IEntityEnumerator cedent in booleanCedents)
                    {
                        if (cedent != null)
                            result *= cedent.TotalCount;
                    }
                if (categorialCedents != null)
                    foreach (CategorialAttributeTrace[] cedent in categorialCedents)
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

        protected readonly BooleanAttribute[] _booleanAttributes;
        protected readonly CategorialAttribute[] _categorialAttributes;

        protected readonly Quantifiers _quantifiers;
        public Quantifiers Quantifiers
        {
            get { return _quantifiers; }
        }

        private readonly QuantifierSetting _baseQuantifierSetting;

        private readonly TaskRunParams _taskParams;
        public TaskRunParams TaskParams
        {
            get { return _taskParams; }
        }

        #endregion

        #region Result

        protected Result _result = null;
        public Result Result
        {
            get { return _result; }
        }

        protected SerializableResultInfo _resultInfo = null;
        public SerializableResultInfo ResultInfo
        {
            get { return _resultInfo; }
        }

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

        protected void resultFinish()
        {
            _resultInfo.EndTime = DateTime.Now;
        }

        #endregion

        #region Progress

        private readonly ProgressTaskListener _progressListener;
        private readonly ProgressBarPrx _progressBarPrx;

        private float _progressValue = -1;
        private string _progressMessage = "Loading ...";
        private long _progressLastUpdateTicks = DateTime.Now.Ticks;
        private const long _progressMinCountOfTicksToPublish = 500000;
        // ticks:
        // 1 tick = 100-nanosecond 
        // nano is 0.000 000 001
        // mili is 0.001
        // 0.05 sec is ticks * 500 000

        public float ProgressGetValue(out string message)
        {
            message = _progressMessage;
            return _progressValue;
        }

        public bool ProgressSetValue(float value, string message)
        {
            _progressValue = value;
            _progressMessage = message;

            long actTicks = DateTime.Now.Ticks;
            if (System.Math.Abs(_progressLastUpdateTicks - actTicks) > _progressMinCountOfTicksToPublish)
            {
                _progressLastUpdateTicks = actTicks;
                _progressBarPrx.setValue(value, message);
            }

            if (_progressListener.Stopped)
                return false;
            return true;
        }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="T:MiningProcessorBase"/> class.
        /// </summary>
        /// <param name="booleanAttributes">The boolean attributes.</param>
        /// <param name="categorialAttributes">The categorial attributes.</param>
        /// <param name="quantifiers">The quantifiers.</param>
        /// <param name="taskFuncPrx">The task func PRX.</param>
        /// <param name="taskParams">The task params.</param>
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
            )
        {
            _progressListener = progressListener;
            _progressListener.MinningProcessor = this;
            _progressBarPrx = progressBarPrx;
            _taskParams = taskParams;
            _booleanAttributes = booleanAttributes;
            _categorialAttributes = categorialAttributes;
            ProgressSetValue(-1, "Preparing quantifiers");
            _quantifiers = new Quantifiers(quantifiers, taskFuncPrx);
            _baseQuantifierSetting = _quantifiers.GetBaseQuantifierSetting();
        }

        protected void afterConstruct()
        {
            prepareAttributeTraces();
            ProgressSetValue(-1, "Validating quantifiers and attributes.");
            TestOfQuantifiersXAttributes();
        }

        long _allObjectsCount = -1;

        public void ComputeAllObjectsCount(CategorialAttributeTrace[] attributes)
        {
            if (_allObjectsCount > 0)
                return;
            if (attributes == null || attributes.Length == 0)
                return;
            foreach (CategorialAttributeTrace trace in attributes)
            {
                if (trace.BitStrings.Length == 0)
                {
                    Debug.Assert(false);
                    continue;
                }
                _allObjectsCount = trace.BitStrings[0].Length;
                return;
            }
        }

        public void ComputeAllObjectsCount(IEntityEnumerator attribute)
        {
            if (_allObjectsCount > 0)
                return;
            if (!(attribute is EmptyTrace))
                return;
            foreach (IBitString s in attribute)
            {
                _allObjectsCount = s.Length;
                return;
            }
        }
        #region Attributes traces preparing

        public static IEntitySetting GetBoolanAttributeBySemantic(MarkEnum semantic,
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

        public static IEntityEnumerator CreateBooleanAttributeTrace(MarkEnum semantic,
                                                                    BooleanAttribute[] booleanAttributes,
                                                                    bool allowsEmptyBitStrings,
                                                                    MiningProcessorBase miningProcessorBase)
        {
            IEntitySetting setting = GetBoolanAttributeBySemantic(semantic, booleanAttributes);
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
            miningProcessorBase.ComputeAllObjectsCount(finalResult);
            return finalResult;
        }

        public static CategorialAttributeTrace[] CreateCategorialAttributeTrace(MarkEnum semantic,
                                                                                CategorialAttribute[] categorialAttributes,
                                                                                bool allowEmptyCategorialCedent,
                                                                                MiningProcessorBase miningProcessorBase)
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

            CategorialAttributeTrace[] finalResult = result.ToArray();
            miningProcessorBase.ComputeAllObjectsCount(finalResult);
            return finalResult;
        }

        #endregion

        #region Boolean attribute negations and missings retrieving

        /// <summary>
        /// Gets the negation and missings bit strings. Please note
        /// that both negation and missings can be instances
        /// of IEmptyBitString or IFalseBitString.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="missings">The missings.</param>
        /// <param name="negation">The negation.</param>
        /// <param name="usedAttributes">The used attributes.</param>
        /// <param name="missingInformation">The missing information IBitString provider.</param>
        public static void GetNegationAndMissings(IBitString input, out IBitString missings, out IBitString negation,
                                                  Set<string> usedAttributes, MissingInformation missingInformation)
        {
            if (input is EmptyBitString)
            {
                missings = FalseBitString.GetInstance();
                negation = FalseBitString.GetInstance();
            }
            else
            {
                missings = missingInformation[usedAttributes];
                Debug.Assert(!(missings is EmptyBitString));
                if (missings is EmptyBitString)
                    throw new ArgumentException();

                if (missings is FalseBitString)
                    negation = input.Not();
                else
                    negation = input.Or(missings).Not();
            }
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
        /// <param name="missings">The missings.</param>
        /// <param name="usedAttributes">The used attributes.</param>
        /// <param name="missingInformation">The missing information IBitString provider.</param>
        public static void GetMissings(IBitString input, out IBitString missings,
                                       Set<string> usedAttributes, MissingInformation missingInformation)
        {
            if (input is EmptyBitString)
            {
                missings = FalseBitString.GetInstance();
            }
            else
            {
                missings = missingInformation[usedAttributes];
                Debug.Assert(!(missings is EmptyBitString));
                if (missings is EmptyBitString)
                    throw new ArgumentException();
            }
        }

        #endregion

        #region ISkipOptimalization Members

        private int _actConditionCountOfObjects = -1;
        public int ActConditionCountOfObjects
        {
            set
            {
                _actConditionCountOfObjects = value;
            }
        }

        private double _baseSkipSettingTreshold = -1;
        private double _baseSkipSettingNotTreshold = -1;
        public SkipSetting BaseSkipSetting(MarkEnum cedentType)
        {
            if (_baseQuantifierSetting == null)
                return null;
            if (_baseQuantifierSetting.units == UnitsEnum.RelativeToActCondition)
                if (cedentType == MarkEnum.Condition)
                    return null;
                else
                {
                    return new SkipSetting(
                        _baseQuantifierSetting.relation,
                        _baseQuantifierSetting.treshold * _actConditionCountOfObjects,
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
    }
}