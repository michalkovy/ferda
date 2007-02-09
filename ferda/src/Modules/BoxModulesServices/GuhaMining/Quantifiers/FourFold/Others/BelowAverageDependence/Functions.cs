// Functions.cs - Function objects for the Below average dependence box module
//
// Author: Martin Ralbovský <martin.ralbovsky@gmail.com>
//
// Copyright (c) 2006 Tomáš Kuchaø, Martin Ralbovský
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
using Ferda.Guha.Data;
using Ferda.Guha.Math;
using Ferda.Guha.Math.Quantifiers;
using Ice;

namespace Ferda.Modules.Boxes.GuhaMining.Quantifiers.FourFold.Others.BelowAverageDependence
{
    /// <summary>
    /// Computes the above/below average difference.
    /// </summary>
    /// <returns>
    /// <para>
    /// This quiantifier computes value <c>(a / (a + b)) / ((a + c) / (a + b + c + d))</c>
    /// i.e. <c>(a * (a + b + c + d)) / ((a + b) * (a + c))</c> and its semantics depends
    /// on specified <c>Relation</c> and <c>Treshold</c> where 0 &lt; Treshold &lt; +INF. 
    /// </para>
    /// <para>
    /// E.g. if Relation is &gt;= and Treshold is 1.1 than contingency table 
    /// satisfies Above Average condition with p = 0.1. 
    /// Or if Relation is &lt;= and Treshold is 0.8 than contingency table 
    /// satisfies Below Average condition with p = 0.2.
    /// </para>
    /// <para>
    /// If (a + c) = 0, returns NaN. (explanation: the succedent does 
    /// not exist at all, so its occurence is neither above nor below 
    /// average when antecedent holds true); 
    /// </para>
    /// <para>
    /// If a = 0, returns -INF (explanation: the succedent exists only 
    /// when antecedent is not true, so its occurence is +INF-times smaller 
    /// than in average, i.e. the below-average-part should be always true)
    /// </para>
    /// </returns>
    /// <remarks>
    /// <para>
    /// Above average difference is defined as 
    /// <c>(a / (a + b)) &gt;= (1 + p) ((a + c) / (a + b + c + d))</c> where
    /// <c>0 &lt; p</c>
    /// It says that P(succ|ant) is p-times greather than P(succ)
    /// i.e. P(succ|ant)/P(succ) >= 1+p.
    /// </para>
    /// <para>
    /// Below average difference is defined as 
    /// <c>(a / (a + b)) &lt;= (1 - p) ((a + c) / (a + b + c + d))</c> where
    /// <c>0 &lt; p &lt; 1</c>
    /// It says that P(succ|ant) is p-times lesser than P(succ)
    /// i.e. P(succ|ant)/P(succ) >= 1-p.
    /// </para>
    /// <para>
    /// Outside average difference is defined as 
    /// <c>(a / (a + b)) &lt;= (1 - p) ((a + c) / (a + b + c + d))</c> where
    /// <c>0 &lt; p &lt; 1</c>
    /// It says that P(succ|ant) is p-times lesser than P(succ)
    /// i.e. P(succ|ant)/P(succ) >= 1-p.
    /// </para>
    /// <para>
    /// There can be defined outside average also.<br />
    /// Outside average strength value defined as maximum of two values:<br />
    /// <c>(a / (a + b)) * ((a + b + c + d) / (a + c))</c> and<br />
    /// <c>((a + b) / a) * ((a + c) / (a + b + c + d))</c>.
    /// This can be simulated by two calls of this quantifier with 
    /// corresponding setting.
    /// </para>
    /// </remarks>
    public class Functions : FourFoldValueDisp_, IFunctions
    {
        /// <summary>
        /// The box module.
        /// </summary>
        protected BoxModuleI _boxModule;
        //protected IBoxInfo _boxInfo;

        #region IFunctions Members

        /// <summary>
        /// Sets the <see cref="T:Ferda.Modules.BoxModuleI">box module</see>
        /// and the <see cref="T:Ferda.Modules.Boxes.IBoxInfo">box info</see>.
        /// </summary>
        /// <param name="boxModule">The box module.</param>
        /// <param name="boxInfo">The box info.</param>
        public void setBoxModuleInfo(BoxModuleI boxModule, IBoxInfo boxInfo)
        {
            _boxModule = boxModule;
            //_boxInfo = boxInfo;
        }

        #endregion

        #region Properties
        /*
        THE DOCUMENTATION FOR MAJORITY OF STRUCTURES USED IN THIS REGION CAN BE 
        FOUND IN src\Modules\Core\Math\Quantifiers\IceDesignDocumentation        
        */

        public OperationModeEnum OperationMode
        {
            get
            {
                return (OperationModeEnum)Enum.Parse(
                                               typeof(OperationModeEnum),
                                               _boxModule.GetPropertyString(Common.PropOperationMode)
                                               );
            }
        }

        public MissingInformationHandlingEnum MissingInformationHandling
        {
            get
            {
                return (MissingInformationHandlingEnum)Enum.Parse(
                                                            typeof(MissingInformationHandlingEnum),
                                                            _boxModule.GetPropertyString(
                                                                Common.PropMissingInformationHandling)
                                                            );
            }
        }

        public RelationEnum Relation
        {
            get
            {
                return RelationEnum.GreaterOrEqual;
            }
        }

        public double Treshold
        {
            get { return _boxModule.GetPropertyDouble(Common.PropTreshold); }
        }

        public BoundTypeEnum FromRowBoundary
        {
            get { return BoundTypeEnum.All; }
        }

        public int FromRowBoundaryIndex
        {
            get { return 0; }
        }

        public BoundTypeEnum ToRowBoundary
        {
            get { return BoundTypeEnum.All; }
        }

        public int ToRowBoundaryIndex
        {
            get { return 1; }
        }

        public BoundTypeEnum FromColumnBoundary
        {
            get { return BoundTypeEnum.All; }
        }

        public int FromColumnBoundaryIndex
        {
            get { return 0; }
        }

        public BoundTypeEnum ToColumnBoundary
        {
            get { return BoundTypeEnum.All; }
        }

        public int ToColumnBoundaryIndex
        {
            get { return 1; }
        }

        public QuantifierClassEnum[] QuantifierClasses
        {
            get { return new QuantifierClassEnum[] { }; }
        }

        public PerformanceDifficultyEnum PerformanceDifficulty
        {
            get { return PerformanceDifficultyEnum.Easy; }
        }

        public bool NeedsNumericValues
        {
            get { return false; }
        }

        public CardinalityEnum SupportedData
        {
            get { return CardinalityEnum.Nominal; }
        }

        public UnitsEnum Units
        {
            get { return UnitsEnum.Irrelevant; }
        }

        public bool SupportsFloatContingencyTable
        {
            get { return true; }
        }

        #endregion

        #region QuantifierValueFunctions implementation

        /// <summary>
        /// The function determines if the contingency table determined by
        /// <paramref name="param"/> satisfies the quantifier. If it does
        /// and if the quantifier provides significant numerical value,
        /// the value is stored in the <paramref name="value"/> parameter.
        /// </summary>
        /// <param name="param">
        /// The quantifier setting includes the
        /// contingency table and other information
        /// </param>
        /// <param name="value">
        /// The numerical value of the quantifier
        /// </param>
        /// <param name="current__">Ice stuff</param>
        /// <returns>If the settings satisfies the quantifier</returns>
        public override bool ComputeValidValue(QuantifierEvaluateSetting param, out double value, Current current__)
        {
            value = ComputeValue(param);
            return Guha.Math.Common.Compare(Relation, value, Treshold);
        }

        /// <summary>
        /// Computes the value of the quantifier
        /// </summary>
        /// <param name="param">
        /// The quantifier setting includes the
        /// contingency table and other information
        /// </param>
        /// <param name="current__">Ice stuff</param>
        /// <returns>The value of the quantifier</returns>
        public override double ComputeValue(QuantifierEvaluateSetting param, Current current__)
        {
            return ExceptionsHandler.TryCatchMethodThrow<double>(
                delegate
                {
                    FourFoldContingencyTable table = new FourFoldContingencyTable(param);
                    if ((table.A + table.C) == 0)
                        return Double.NaN;
                    else if (table.A == 0)
                        return Double.NegativeInfinity;
                    else
                        return 1 - ((table.A * table.N) / (table.R * table.K));
                },
                _boxModule.StringIceIdentity
                );
        }

        #endregion

        #region QuantifierBaseFunctions implementation

        /// <summary>
        /// Returns the localized label of the box (for showing in result browser...)
        /// </summary>
        /// <param name="localePrefs">Localization preferences</param>
        /// <param name="current__">Ice stuff</param>
        /// <returns>Localized label of the box</returns>
        public override string GetLocalizedBoxLabel(string[] localePrefs, Current current__)
        {
            return _boxModule.BoxInfo.GetLabel(localePrefs);
        }

        /// <summary>
        /// Returns the user label of the box
        /// </summary>
        /// <param name="localePrefs">Localization preferences</param>
        /// <param name="current__">Ice stuff</param>
        /// <returns></returns>
        public override string GetLocalizedUserBoxLabel(string[] localePrefs, Current current__)
        {
            return _boxModule.Manager.getProjectInformation().getUserLabel(_boxModule.StringIceIdentity);
        }

        /// <summary>
        /// Determines if the contingency table determined by
        /// <paramref name="param"/> satisfies the quantifier
        /// </summary>
        /// <param name="param">
        /// The quantifier setting includes the
        /// contingency table and other information
        /// </param>
        /// <param name="current__">Ice stuff</param>
        /// <returns>If the settings satisfies the quantifier</returns>
        public override bool Compute(QuantifierEvaluateSetting param, Current current__)
        {
            double value = ComputeValue(param);
            return Guha.Math.Common.Compare(Relation, value, Treshold);
        }

        /// <summary>
        /// Returns a structure giving information about the quantifier
        /// </summary>
        /// <param name="current__">Ice stuff</param>
        /// <returns>Quantifier setting</returns>
        public override QuantifierSetting GetQuantifierSetting(Current current__)
        {
            return new QuantifierSetting(
                _boxModule.StringIceIdentity,
                _boxModule.BoxInfo.Identifier,
                MissingInformationHandling,
                OperationMode,
                Relation,
                Treshold,
                new Bound(FromRowBoundary, FromRowBoundaryIndex),
                new Bound(ToRowBoundary, ToRowBoundaryIndex),
                new Bound(FromColumnBoundary, FromColumnBoundaryIndex),
                new Bound(ToColumnBoundary, ToColumnBoundaryIndex),
                QuantifierClasses,
                PerformanceDifficulty,
                NeedsNumericValues,
                SupportedData,
                Units,
                SupportsFloatContingencyTable
                );
        }

        /// <summary>
        /// Method to compute more quantifier testing
        /// </summary>
        /// <param name="param">More quantifier settings</param>
        /// <param name="current__">Ice stuff</param>
        /// <returns>Result of more quantifiers testing</returns>
        public override bool[] ComputeBatch(QuantifierEvaluateSetting[] param, Current current__)
        {
            return Common.ComputeBatch(param, this.Compute);
        }

        #endregion
    }
}
