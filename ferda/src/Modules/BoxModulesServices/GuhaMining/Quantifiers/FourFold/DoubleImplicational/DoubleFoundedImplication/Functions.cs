using System;
using System.Collections.Generic;
using System.Text;
using Ferda.Guha.Data;
using Ferda.Guha.Math;
using Ferda.Guha.Math.Quantifiers;

namespace Ferda.Modules.Boxes.GuhaMining.Quantifiers.FourFold.Implicational.DoubleFoundedImplication
{
    /// <summary>
    /// Computes the <c>strength</c> of double founded implication.
    /// </summary>
    /// <returns>
    /// The strength defined as <c>a / (a + b + c)</c>.
    /// If (a + b + c) = 0, returns NaN.
    /// </returns>
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
                return 1;
            }
        }
        public QuantifierClassEnum[] QuantifierClasses
        {
            get
            {
                return new QuantifierClassEnum[] { QuantifierClassEnum.SigmaDoubleImplicational };
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
                        double a = table.A;
                        double sum = a + table.B + table.C;
                        return (sum > 0) ? (a / sum) : Double.NaN;
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