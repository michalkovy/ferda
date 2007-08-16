// Quantifier.cs - represents a quantifier in the mining processor
//
// Authors: Tomáš Kuchaø <tomas.kuchar@gmail.com>      
// Commented by: Martin Ralbovský <martin.ralbovsky@gmail.com>
//
// Copyright (c) 2006 Tomáš Kuchaø
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
    /// <para>
    /// Represents a quantifier for the MiningProcessor. The relevant
    /// information are taken form the quantifier box modules connected
    /// to the task box modules in form of proxies.
    /// </para>
    /// <para>
    /// Please note that <c>OperationModeEnum OperationMode</c> has 
    /// to be processed externaly i.e. out of this class. In other
    /// words this class has no interests in this property. 
    /// </para>
    /// </summary>
    public class Quantifier
    {
        #region Private fields

        /// <summary>
        /// Proxy of the basic functions of a quantifier
        /// (name, settings etc.)
        /// </summary>
        private readonly QuantifierBaseFunctionsPrx _prx;
        /// <summary>
        /// The "numeric values providers" provider.
        /// </summary>
        private readonly BitStringGeneratorProviderPrx _taskFuncPrx;
        /// <summary>
        /// This proxy is used, when the quantifier provides numeric values that
        /// are meaningful to the user.
        /// </summary>
        private readonly QuantifierValueFunctionsPrx _prxValue;
        /// <summary>
        /// This proxy is used, when the quantifier provides numeric values that
        /// are not meaningful to the user, however hypotheses can be sorted
        /// according to these numeric values.
        /// </summary>
        private readonly QuantifierSignificantValueFunctionsPrx _prxSignificantValue;
        /// <summary>
        /// This proxy is used, when the quantifier can only say, whether
        /// the contingency table is valid for the quantifier or not.
        /// </summary>
        private readonly QuantifierValidFunctionsPrx _prxValid;
        /// <summary>
        /// Setting of the quantifier
        /// </summary>
        private readonly QuantifierSetting _setting;

        /// <summary>
        /// Iff the quantifier provides numeric values
        /// meaningful to the user.
        /// </summary>
        private readonly bool _providesValues = false;

        /// <summary>
        /// Iff the quantifier provides numeric values that
        /// are not meaningful to the user, however hypotheses can be sorted
        /// according to these numeric values.
        /// </summary>
        private readonly bool _providesAtLeastSignificantValues = false;

        /// <summary>
        /// Operation mode of the quantifier. 
        /// Please note that <c>OperationMode</c> has 
        /// to be processed externaly i.e. out of this class. In other
        /// words this class has no interests in this property. 
        /// </summary>
        private readonly OperationModeEnum _operationMode;

        /// <summary>
        /// Determines, if the quantifier is four-fold only
        /// </summary>
        private readonly bool _isPureFourFold;

        #endregion

        #region Properties

        /// <summary>
        /// Setting of the quantifier
        /// </summary>
        public QuantifierSetting Setting
        {
            get { return _setting; }
        }

        /// <summary>
        /// Iff the quantifier provides numeric values
        /// meaningful to the user.
        /// </summary>
        public bool ProvidesValues
        {
            get { return _providesValues; }
        }

        /// <summary>
        /// Iff the quantifier provides numeric values that
        /// are not meaningful to the user, however hypotheses can be sorted
        /// according to these numeric values.
        /// </summary>
        public bool ProvidesAtLeastSignificantValues
        {
            get { return _providesAtLeastSignificantValues; }
        }

        /// <summary>
        /// Operation mode of the quantifier. 
        /// Please note that <c>OperationMode</c> has 
        /// to be processed externaly i.e. out of this class. In other
        /// words this class has no interests in this property. 
        /// </summary>
        public OperationModeEnum OperationMode
        {
            get { return _operationMode; }
        }

        #endregion 

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

        /// <summary>
        /// Localization preferences
        /// </summary>
        private readonly string[] _localePrefs = null;

        /// <summary>
        /// The localized label of the quantifier
        /// (the localized name of the quantifier)
        /// </summary>
        private string _localizedLabel = null;

        /// <summary>
        /// The localized label of the quantifier
        /// (the localized name of the quantifier)
        /// </summary>
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

        /// <summary>
        /// The localized user label of the quantifier
        /// (what user sees on desktop)
        /// </summary>
        private string _localizedUserLabel = null;

        /// <summary>
        /// The localized user label of the quantifier
        /// (what user sees on desktop)
        /// </summary>
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

        /// <summary>
        /// Constructs a <see cref="Ferda.Guha.Math.Quantifiers.QuantifierEvaluateSetting"/>
        /// out of the contingancy table stored in <paramref name="contingencyTable"/>
        /// </summary>
        /// <param name="contingencyTable">Contingency table</param>
        /// <returns>Table to be evaluated by quantifier.</returns>
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

        /// <summary>
        /// Computes a numeric value of the quantifier for a given setting
        /// </summary>
        /// <param name="setting">Setting to be evaluated by a quantifier</param>
        /// <returns>Numeric value of the quantifier</returns>
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
        /// !!! Quantifiers of the <c>DifferenceOfQuantifierValues</c> must
        /// use only this method. In the result browser, allow to show values of only
        /// those quantifiers, that support this interface.
        /// </summary>
        /// <param name="contingencyTable">The contingency table.</param>
        /// <returns>Value of the quantifier</returns>
        public double Value(ContingencyTableHelper contingencyTable)
        {
            QuantifierEvaluateSetting setting = getQuantifierEvaluateSetting(contingencyTable);
            return value(setting);
        }

        /// <summary>
        /// Gets value of the quantifier depending on the operation
        /// mode. Used mainly for the SD quantifiers.
        /// </summary>
        /// <param name="hypothesis">Hypothesis</param>
        /// <param name="allObjectsCount">Count of all objects</param>
        /// <returns>Value of quantifier</returns>
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

        #region At least significant values

        /// <summary>
        /// Returns if the quantifier is valid for a given setting + in the 
        /// <paramref name="value"/> parameter show the significant value of the
        /// quantifier. Significant value means the quantifier provides numeric 
        /// values that are not meaningful to the user, however hypotheses can be 
        /// sorted according to these numeric values.
        /// </summary>
        /// <param name="setting">Contingency table setting to be evaluated</param>
        /// <param name="value">Where the significant value is stored</param>
        /// <returns>If the quantifier is valid</returns>
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

        #endregion

        #region Valid

        /// <summary>
        /// How much time did the computation of quantifiers take
        /// (Ice communication)
        /// </summary>
        private static long _iceTicks;
        /// <summary>
        /// How much time did the computation of quantifiers take
        /// </summary>
        public static long IceTicks
        {
            get { return _iceTicks; }
            set { _iceTicks = value; }
        }

        /// <summary>
        /// How much Ice calls were made during the computation of quantifiers
        /// </summary>
        private static long _iceCalls;
        /// <summary>
        /// How much Ice calls were made during the computation of quantifiers
        /// </summary>
        public static long IceCalls
        {
            get { return _iceCalls; }
            set { _iceCalls = value; }
        }
        
        /// <summary>
        /// Determines, if the quantifier is valid for setting
        /// in the parameter.
        /// </summary>
        /// <param name="setting">Contingency table setting</param>
        /// <returns>Iff the quantifier is valid</returns>
        private bool valid(QuantifierEvaluateSetting setting)
        {
            long before = DateTime.Now.Ticks;
            _iceCalls++;
            
            Debug.Assert(!(Setting.needsNumericValues && String.IsNullOrEmpty(setting.numericValuesAttributeId.value)));
            bool result;

            if (ProvidesValues)
            {
                result = _prxValue.Compute(setting);
            }
            else if (ProvidesAtLeastSignificantValues)
            {
                result = _prxSignificantValue.Compute(setting);
            }
            else
            {
                result = _prxValid.Compute(setting);
            }
            
            _iceTicks += DateTime.Now.Ticks - before;
            return result;
        }

        /// <summary>
        /// Examines the validity of a quantifier on an array
        /// of quantifier evaluation (contingency tables) setting. 
        /// </summary>
        /// <param name="setting">Contingency table setting</param>
        /// <returns>Iff the quantifier is valid</returns>
        private bool[] valid(QuantifierEvaluateSetting[] setting)
        {
            long before = DateTime.Now.Ticks;
            _iceCalls++;

            bool[] result;
            
            if (ProvidesValues)
            {
                result = _prxValue.ComputeBatch(setting);
            }
            else if (ProvidesAtLeastSignificantValues)
            {
                result = _prxSignificantValue.ComputeBatch(setting);
            }
            else
            {
                result = _prxValid.ComputeBatch(setting);
            }
            
            _iceTicks += DateTime.Now.Ticks - before;
            return result;
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

        /// <summary>
        /// !!! Pro kvantifikatory neposkytujici jiny interface,
        /// !!! Pro ucely FirstN
        /// </summary>
        /// <param name="contingencyTable">The contingency table.</param>
        /// <returns></returns>
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
        /// <param name="value">Value of the quantifier</param>
        /// <returns>If it is in relation with the treshold</returns>
        public bool TestTreshold(double value)
        {
            return Common.Compare(_setting.relation, value, _setting.treshold);
        }

        #endregion
    }
}