using System;
using System.Collections.Generic;
using System.Text;
using Ferda.Guha.Data;
using Ferda.Guha.Math;
using Ferda.Guha.Math.Quantifiers;

namespace Ferda.Modules.Boxes.GuhaMining.Quantifiers.FourFold.Others.AboveBelowAverageImplication
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
                    _boxModule.GetPropertyString(Common.PropMissingInformationHandling)
                    );
            }
        }
        public RelationEnum Relation
        {
            get
            {
                return (RelationEnum)Enum.Parse(
                    typeof(RelationEnum),
                    _boxModule.GetPropertyString(Common.PropRelation)
                    );
            }
        }
        public double Treshold
        {
            get
            {
                return _boxModule.GetPropertyDouble(Common.PropTreshold);
            }
        }
        public BoundTypeEnum FromRowBoundary
        {
            get
            {
                return BoundTypeEnum.All;
            }
        }
        public int FromRowBoundaryIndex
        {
            get
            {
                return 0;
            }
        }
        public BoundTypeEnum ToRowBoundary
        {
            get
            {
                return BoundTypeEnum.All;
            }
        }
        public int ToRowBoundaryIndex
        {
            get
            {
                return 1;
            }
        }
        public BoundTypeEnum FromColumnBoundary
        {
            get
            {
                return BoundTypeEnum.All;
            }
        }
        public int FromColumnBoundaryIndex
        {
            get
            {
                return 0;
            }
        }
        public BoundTypeEnum ToColumnBoundary
        {
            get
            {
                return BoundTypeEnum.All;
            }
        }
        public int ToColumnBoundaryIndex
        {
            get
            {
                return 0;
            }
        }
        public QuantifierClassEnum[] QuantifierClasses
        {
            get
            {
                return new QuantifierClassEnum[] { };
            }
        }
        public PerformanceDifficultyEnum PerformanceDifficulty
        {
            get
            {
                return PerformanceDifficultyEnum.Easy;
            }
        }
        public bool NeedsNumericValues
        {
            get
            {
                return false;
            }
        }
        public CardinalityEnum SupportedData
        {
            get
            {
                return CardinalityEnum.Nominal;
            }
        }
        public UnitsEnum Units
        {
            get
            {
                return UnitsEnum.Irrelevant;
            }
        }
        public bool SupportsFloatContingencyTable
        {
            get
            {
                return true;
            }
        }

        public override bool Compute(QuantifierEvaluateSetting param, Ice.Current current__)
        {
            double value = ComputeValue(param);
            return Guha.Math.Common.Compare(Relation, value, Treshold);
        }

        public override bool ComputeValidValue(QuantifierEvaluateSetting param, out double value, Ice.Current current__)
        {
            value = ComputeValue(param);
            return Guha.Math.Common.Compare(Relation, value, Treshold);
        }

        public override double ComputeValue(QuantifierEvaluateSetting param, Ice.Current current__)
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
                            return (table.A * table.N) / (table.R * table.K);
                    },
                _boxModule.StringIceIdentity
                );
        }

        public override string GetLocalizedBoxLabel(string[] localePrefs, Ice.Current current__)
        {
            return _boxModule.BoxInfo.GetLabel(localePrefs);
        }

        public override string GetLocalizedUserBoxLabel(string[] localePrefs, Ice.Current current__)
        {
            return _boxModule.Manager.getProjectInformation().getUserLabel(_boxModule.StringIceIdentity);
        }

        public override QuantifierSetting GetQuantifierSetting(Ice.Current current__)
        {
            return new QuantifierSetting(
                _boxModule.StringIceIdentity,
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
    }
}
