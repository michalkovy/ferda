using System;
using System.Collections.Generic;
using System.Diagnostics;
using Ferda.Guha.Math;
using Ferda.Guha.Math.Quantifiers;
using Ferda.Guha.MiningProcessor.Results;
using Ferda.Modules;

namespace Ferda.Guha.MiningProcessor.QuantifierEvaluator
{
    public enum QuantifierValueQueueSemantic
    {
        Greater,
        Lesser,
        Unsupported
    }

    /// <summary>
    /// Please note that <c>OperationModeEnum OperationMode</c> has 
    /// to be processed externaly i.e. out of this class. In other
    /// words this class has no interests in this property. 
    /// </summary>
    public class Quantifier
    {
        private readonly QuantifierBaseFunctionsPrx _prx;
        private readonly BitStringGeneratorProviderPrx _taskFuncPrx;
        private readonly QuantifierValueFunctionsPrx _prxValue;
        private readonly QuantifierSignificantValueFunctionsPrx _prxSignificantValue;
        private readonly QuantifierValidFunctionsPrx _prxValid;

        private readonly QuantifierSetting _setting;

        public QuantifierSetting Setting
        {
            get { return _setting; }
        }

        private readonly bool _providesValues = false;

        public bool ProvidesValues
        {
            get { return _providesValues; }
        }

        private readonly bool _providesAtLeastSignificantValues = false;

        public bool ProvidesAtLeastSignificantValues
        {
            get { return _providesAtLeastSignificantValues; }
        }

        private readonly OperationModeEnum _operationMode;

        public OperationModeEnum OperationMode
        {
            get { return _operationMode; }
        }

        private readonly bool _isPureFourFold;

        #region For TopN algorithm purposes

        private readonly QuantifierValueQueueSemantic _quantifierValueQueueSemantic =
            QuantifierValueQueueSemantic.Unsupported;

        public QuantifierValueQueueSemantic QuantifierValueQueueSemantic
        {
            get { return _quantifierValueQueueSemantic; }
        }

        private readonly PerformanceDifficultyEnum _performanceDifficulty;

        public PerformanceDifficultyEnum PerformanceDifficulty
        {
            get { return _performanceDifficulty; }
        }

        public double ActualEfficiency
        {
            get
            {
                if (NumberOfInvokes == 0)
                    return 0;
                // (-0.5;0.5>
                double efficiency = -0.5d + NumberOfFalseInvokes / NumberOfInvokes;
                return (Int64.MaxValue * efficiency) / (double)NumberOfInvokes;
            }
        }

        private long _numberOfInvokes = 0;

        public long NumberOfInvokes
        {
            get { return _numberOfInvokes; }
        }

        private long _numberOfFalseInvokes = 0;

        public long NumberOfFalseInvokes
        {
            get { return _numberOfFalseInvokes; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Quantifier"/> class.
        /// </summary>
        /// <param name="prx">The quantifier proxy.</param>
        /// <param name="taskFuncPrx">The "numeric values providers" provider.</param>
        public Quantifier(QuantifierBaseFunctionsPrx prx, BitStringGeneratorProviderPrx taskFuncPrx)
        {
            _prx = prx;
            _taskFuncPrx = taskFuncPrx;
            _setting = _prx.GetQuantifierSetting();
            _performanceDifficulty = _setting.performanceDifficulty;
            _operationMode = _setting.operationMode;

            _prxValid = QuantifierValidFunctionsPrxHelper.checkedCast(_prx);

            if (prx.ice_isA("::Ferda::Guha::Math::Quantifiers::QuantifierValueFunctions"))
            {
                _providesValues = true;
                _providesAtLeastSignificantValues = true;
                _prxValue = QuantifierValueFunctionsPrxHelper.checkedCast(_prx);
            }
            else if (prx.ice_isA("::Ferda::Guha::Math::Quantifiers::QuantifierValueBaseFunctions"))
            {
                _providesAtLeastSignificantValues = true;
                _prxSignificantValue = QuantifierSignificantValueFunctionsPrxHelper.checkedCast(_prx);
            }

            _isPureFourFold = prx.ice_isA("::Ferda::Guha::Math::Quantifiers::FourFoldValid")
                              || prx.ice_isA("::Ferda::Guha::Math::Quantifiers::FourFoldValue")
                              || prx.ice_isA("::Ferda::Guha::Math::Quantifiers::FourFoldSignificantValue");

            if (_providesAtLeastSignificantValues)
            {
                switch (_setting.relation)
                {
                    case RelationEnum.Greater:
                    case RelationEnum.GreaterOrEqual:
                        _quantifierValueQueueSemantic = QuantifierValueQueueSemantic.Greater;
                        break;
                    case RelationEnum.Less:
                    case RelationEnum.LessOrEqual:
                        _quantifierValueQueueSemantic = QuantifierValueQueueSemantic.Lesser;
                        break;
                    case RelationEnum.Equal:
                    default:
                        _quantifierValueQueueSemantic = QuantifierValueQueueSemantic.Unsupported;
                        break;
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Quantifier"/> class.
        /// </summary>
        /// <param name="prx">The quantifier proxy.</param>
        /// <param name="taskFuncPrx">The "numeric values providers" provider.</param>
        /// <param name="localePrefs">The locale prefs.</param>
        public Quantifier(QuantifierBaseFunctionsPrx prx, BitStringGeneratorProviderPrx taskFuncPrx,
                          string[] localePrefs)
            : this(prx, taskFuncPrx)
        {
            _localePrefs = localePrefs;
        }

        #endregion

        #region Localization - quantifier (boxes) [user] labels

        private readonly string[] _localePrefs = null;

        private string _localizedLabel = null;

        public string LocalizedLabel
        {
            get
            {
                if (_localizedLabel == null)
                {
                    Debug.Assert(_localePrefs != null && _localePrefs.Length > 0);
                    _localizedLabel = _prx.GetLocalizedBoxLabel(_localePrefs);
                }
                return _localizedLabel;
            }
        }

        private string _localizedUserLabel = null;

        public string LocalizedUserLabel
        {
            get
            {
                if (_localizedUserLabel == null)
                {
                    Debug.Assert(_localePrefs != null && _localePrefs.Length > 0);
                    _localizedUserLabel = _prx.GetLocalizedUserBoxLabel(_localePrefs);
                }
                return _localizedUserLabel;
            }
        }

        #endregion

        private QuantifierEvaluateSetting getQuantifierEvaluateSetting(ContingencyTableHelper contingencyTable)
        {
            QuantifierEvaluateSetting setting;
            if (_isPureFourFold)
            {
                setting = contingencyTable.GetSubTable(
                    _setting.quantifierClasses,
                    _setting.missingInformationHandling,
                    _setting.units);
                if (_setting.needsNumericValues)
                    throw new NotImplementedException();
                setting.numericValuesAttributeId = new GuidStruct(String.Empty);
            }
            else
            {
                setting = contingencyTable.GetSubTable(
                    _setting.FromRow,
                    _setting.ToRow,
                    _setting.FromColumn,
                    _setting.ToColumn,
                    _setting.units);
                if (_setting.needsNumericValues)
                {
                    string numericValuesAttributeGuid = contingencyTable.NumericValuesAttributeGuid;
                    if (numericValuesAttributeGuid == null)
                        throw new ArgumentNullException(
                            "The quantifier needs numeric values, but argument \"NumericValuesAttributeGuid\" is null.");
                    setting.numericValuesAttributeId = new GuidStruct(numericValuesAttributeGuid);
                    setting.numericValuesProviders =
                        _taskFuncPrx.GetBitStringGenerator(setting.numericValuesAttributeId);
                }
                setting.numericValuesAttributeId = new GuidStruct(String.Empty);
            }
            if (setting.denominator <= 0.0d)
                Debug.Assert(false);
            return setting;
        }

        #region OperationMode ... evaluation of Hypothesis

        public QuantifierEvaluateSetting GetFirstTable(Hypothesis hypothesis, long allObjectsCount)
        {
            return getQuantifierEvaluateSetting(
                new ContingencyTableHelper(
                    hypothesis.ContingencyTableA,
                    allObjectsCount,
                    hypothesis.NumericValuesAttributeGuid
                    )
                );
        }

        public QuantifierEvaluateSetting GetSecondTable(Hypothesis hypothesis, long allObjectsCount)
        {
            return getQuantifierEvaluateSetting(
                new ContingencyTableHelper(
                    hypothesis.ContingencyTableB,
                    allObjectsCount,
                    hypothesis.NumericValuesAttributeGuid
                    )
                );
        }

        public QuantifierEvaluateSetting GetFirstDiffSecondTable(Hypothesis hypothesis, long allObjectsCount)
        {
            QuantifierEvaluateSetting first = GetFirstTable(hypothesis, allObjectsCount);
            QuantifierEvaluateSetting second = GetSecondTable(hypothesis, allObjectsCount);
            ContingencyTableHelper result;
            if (_setting.needsNumericValues)
            {
                result = ContingencyTableHelper.OperatorMinus(
                    new ContingencyTableHelper(
                        first.contingencyTable,
                        allObjectsCount,
                        first.denominator,
                        first.numericValuesAttributeId.value
                        ),
                    new ContingencyTableHelper(
                        second.contingencyTable,
                        allObjectsCount,
                        second.denominator,
                        second.numericValuesAttributeId.value
                        )
                    );
                GuidStruct numericValuesAttributeId = new GuidStruct(result.NumericValuesAttributeGuid);
                return new QuantifierEvaluateSetting(
                    result.ContingencyTable,
                    result.Denominator,
                    numericValuesAttributeId,
                    _taskFuncPrx.GetBitStringGenerator(numericValuesAttributeId)
                    );
            }
            else
            {
                result = ContingencyTableHelper.OperatorMinus(
                    new ContingencyTableHelper(
                        first.contingencyTable,
                        allObjectsCount,
                        first.denominator
                        ),
                    new ContingencyTableHelper(
                        second.contingencyTable,
                        allObjectsCount,
                        second.denominator
                        )
                    );
                return new QuantifierEvaluateSetting(
                    result.ContingencyTable,
                    result.Denominator,
                    null,
                    null
                    );
            }

        }

        #endregion

        #region Value
        private double value(QuantifierEvaluateSetting setting)
        {
            if (Setting.needsNumericValues && String.IsNullOrEmpty(setting.numericValuesAttributeId.value))
            {
                Debug.Assert(false);
                return Double.NaN;
            }
            if (!ProvidesValues)
                throw new InvalidOperationException();
            return _prxValue.ComputeValue(setting);
        }

        /// <summary>
        /// !!! kvantifikatory modu DifferenceOfQuantifierValues v SD tascich smi pouzivat jen toto rozhrani.
        /// !!! V result browseru ukazovat hodnoty jen tech kvantifikatoru poskytujicich toto rozhrani.
        /// </summary>
        /// <param name="contingencyTable">The contingency table.</param>
        /// <returns></returns>
        public double Value(ContingencyTableHelper contingencyTable)
        {
            QuantifierEvaluateSetting setting = getQuantifierEvaluateSetting(contingencyTable);
            return value(setting);
        }

        public double Value(Hypothesis hypothesis, long allObjectsCount)
        {
            switch (_setting.operationMode)
            {
                case OperationModeEnum.DifferenceOfFrequencies:
                    return value(GetFirstDiffSecondTable(hypothesis, allObjectsCount));
                case OperationModeEnum.DifferenceOfQuantifierValues:
                    return
                        value(GetFirstTable(hypothesis, allObjectsCount))
                        -
                        value(GetSecondTable(hypothesis, allObjectsCount));
                case OperationModeEnum.FirstSetFrequencies:
                    return value(GetFirstTable(hypothesis, allObjectsCount));
                case OperationModeEnum.SecondSetFrequencies:
                    return value(GetSecondTable(hypothesis, allObjectsCount));
                default:
                    throw new NotImplementedException();
            }
        }
        #endregion

        private bool atLeastSignificantValidValue(QuantifierEvaluateSetting setting, out double value)
        {
            Debug.Assert(!(Setting.needsNumericValues && String.IsNullOrEmpty(setting.numericValuesAttributeId.value)));

            if (!ProvidesAtLeastSignificantValues)
                throw new InvalidOperationException();
            if (ProvidesValues)
            {
                return _prxValue.ComputeValidValue(setting, out value);
            }
            else
            {
                return _prxSignificantValue.ComputeValidValue(setting, out value);
            }
        }

        /// <summary>
        /// Pro ucely TopN. !!! pozor ne vsechny kvantifikatory poskytuji toto rozhrani
        /// </summary>
        /// <param name="contingencyTable">The contingency table.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public bool AtLeastSignificantValidValue(ContingencyTableHelper contingencyTable, out double value)
        {
            QuantifierEvaluateSetting setting = getQuantifierEvaluateSetting(contingencyTable);
            return atLeastSignificantValidValue(setting, out value);
        }

        private bool valid(QuantifierEvaluateSetting setting)
        {
            Debug.Assert(!(Setting.needsNumericValues && String.IsNullOrEmpty(setting.numericValuesAttributeId.value)));

            if (ProvidesValues)
            {
                return _prxValue.Compute(setting);
            }
            else if (ProvidesAtLeastSignificantValues)
            {
                return _prxSignificantValue.Compute(setting);
            }
            else
            {
                return _prxValid.Compute(setting);
            }
        }

        private bool[] valid(QuantifierEvaluateSetting[] setting)
        {
            if (ProvidesValues)
            {
                return _prxValue.ComputeBatch(setting);
            }
            else if (ProvidesAtLeastSignificantValues)
            {
                return _prxSignificantValue.ComputeBatch(setting);
            }
            else
            {
                return _prxValid.ComputeBatch(setting);
            }
        }

        /// <summary>
        /// !!! Pro kvantifikatory neposkytujici jiny interface,
        /// !!! Pro ucely FirstN
        /// </summary>
        /// <param name="contingencyTable">The contingency table.</param>
        /// <returns></returns>
        public bool Valid(ContingencyTableHelper contingencyTable)
        {
            QuantifierEvaluateSetting setting = getQuantifierEvaluateSetting(contingencyTable);
            return valid(setting);
        }

        public bool[] Valid(List<ContingencyTableHelper> contingencyTables)
        {
            QuantifierEvaluateSetting[] setting = new QuantifierEvaluateSetting[contingencyTables.Count];
            for (int i = 0; i < contingencyTables.Count; i++)
            {
                setting[i] = getQuantifierEvaluateSetting(contingencyTables[i]);
            }
            return valid(setting);
        }

        /// <summary>
        /// Pro zjisteni validity u SD kvantifikatoru. Napr typu AbsoluteDifferenceOfQuantifierValues
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool TestTreshold(double value)
        {
            return Common.Compare(_setting.relation, value, _setting.treshold);
        }
    }
}