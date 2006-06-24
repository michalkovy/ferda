using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Ferda.Guha.Math;
using Ferda.Guha.Math.Quantifiers;

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
        private readonly BitStringGeneratorProvider _taskFuncPrx;

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


        #region For TopN algorithm purposes

        private readonly QuantifierValueQueueSemantic _quantifierValueQueueSemantic = QuantifierValueQueueSemantic.Unsupported;
        public QuantifierValueQueueSemantic QuantifierValueQueueSemantic
        {
            get { return _quantifierValueQueueSemantic; }
        }

        private readonly PerformanceDifficultyEnum _performanceDifficulty;
        public PerformanceDifficultyEnum PerformanceDifficulty
        {
            get { return _performanceDifficulty; }
        }

        private long _numberOfInfokes = 0;
        public long NumberOfInfokes
        {
            get { return _numberOfInfokes; }
        }

        private long _numberOfFalseInfokes = 0;
        public long NumberOfFalseInfokes
        {
            get { return _numberOfFalseInfokes; }
        }

        #endregion

        #region Constructors

        public Quantifier(QuantifierBaseFunctionsPrx prx, BitStringGeneratorProvider taskFuncPrx)
        {
            _prx = prx;
            _taskFuncPrx = taskFuncPrx;
            _setting = _prx.GetQuantifierSetting();
            _performanceDifficulty = _setting.performanceDifficulty;
            _operationMode = _setting.operationMode;

            if (prx.ice_isA("::Ferda::Guha::Math::Quantifiers::QuantifierValueFunctions"))
            {
                _providesValues = true;
                _providesAtLeastSignificantValues = true;
            }
            else if (prx.ice_isA("::Ferda::Guha::Math::Quantifiers::QuantifierValueBaseFunctions"))
            {
                _providesAtLeastSignificantValues = true;
            }

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
        public Quantifier(QuantifierBaseFunctionsPrx prx, BitStringGeneratorProvider taskFuncPrx, string[] localePrefs)
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
                    _prx.GetLocalizedBoxLabel(_localePrefs);
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
                    _prx.GetLocalizedUserBoxLabel(_localePrefs);
                }
                return _localizedUserLabel;
            }
        }

        #endregion

        private QuantifierEvaluateSetting getQuantifierEvaluateSetting(ContingencyTableHelper contingencyTable)
        {
            QuantifierEvaluateSetting setting = contingencyTable.GetSubTable(
                _setting.FromRow,
                _setting.ToRow,
                _setting.FromColumn,
                _setting.ToColumn,
                _setting.units);
            if (_setting.needsNumericValues)
            {
                Guid numericValuesAttributeId = contingencyTable.NumericValuesAttributeId;
                if (numericValuesAttributeId == null)
                    throw new ArgumentNullException();
                setting.numericValuesAttributeId = new GuidStruct(numericValuesAttributeId.ToString());
                setting.numericValuesProviders = _taskFuncPrx.GetBitStringGenerator(setting.numericValuesAttributeId);
            }
            return setting;
        }

        /// <summary>
        /// !!! SD tasky smi pouzivat jen toto rozhrani.
        /// !!! V result browseru ukazovat hdnoty jen tech kvantifikatoru poskytujicich toto rozhrani.
        /// </summary>
        /// <param name="contingencyTable">The contingency table.</param>
        /// <returns></returns>
        public double Value(ContingencyTableHelper contingencyTable)
        {
            if (!ProvidesValues)
                throw new InvalidOperationException();

            QuantifierEvaluateSetting setting = getQuantifierEvaluateSetting(contingencyTable);

            QuantifierValueFunctionsPrx prx = (QuantifierValueFunctionsPrx)_prx;
            return prx.ComputeValue(setting);
        }

        /// <summary>
        /// Pro ucely TopN. !!! pozor ne vsechny kvantifikatory poskytuji toto rozhrani
        /// </summary>
        /// <param name="contingencyTable">The contingency table.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public bool AtLeastSignificantValidValue(ContingencyTableHelper contingencyTable, out double value)
        {
            if (!ProvidesAtLeastSignificantValues)
                throw new InvalidOperationException();

            QuantifierEvaluateSetting setting = getQuantifierEvaluateSetting(contingencyTable);

            if (ProvidesValues)
            {
                QuantifierValueFunctionsPrx prx = (QuantifierValueFunctionsPrx)_prx;
                return prx.ComputeValidValue(setting, out value);
            }
            else
            {
                QuantifierSignificantValueFunctionsPrx prx = (QuantifierSignificantValueFunctionsPrx)_prx;
                return prx.ComputeValidValue(setting, out value);
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

            if (ProvidesValues)
            {
                QuantifierValueFunctionsPrx prx = (QuantifierValueFunctionsPrx)_prx;
                return prx.Compute(setting);
            }
            else if (ProvidesAtLeastSignificantValues)
            {
                QuantifierSignificantValueFunctionsPrx prx = (QuantifierSignificantValueFunctionsPrx)_prx;
                return prx.Compute(setting);
            }
            else
            {
                QuantifierValidFunctions prx = (QuantifierValidFunctions)_prx;
                return prx.Compute(setting);
            }
        }

        /// <summary>
        /// Pro zjisteni validity u SD kvantifikatoru. Napr typu AbsoluteDifferenceOfQuantifierValues
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool TestTreshold(double value)
        {
            return Ferda.Guha.Math.Common.Compare(_setting.relation, value, _setting.treshold);
        }
    }
}
