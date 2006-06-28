using System;
using System.Collections.Generic;
using System.Text;
using Ferda.Guha.Data;
using Ferda.Guha.Math;
using Ferda.Guha.Math.Quantifiers;

namespace Ferda.Modules.Boxes.GuhaMining.Quantifiers.TwoDimensional.InformationTheory.InformationDependence
{
    /// <summary>
    /// Information dependence of R on C is defined as I(C,R) / H(R);
    /// Information dependence of C on R is defined as I(C,R) / H(C);
    /// </summary>
    /// <seealso cref="M:Ferda.Guha.Math.InformationTheory.InformationDependenceRC(Ferda.Guha.Math.ContingencyTable)"/>
    /// <seealso cref="M:Ferda.Guha.Math.InformationTheory.InformationDependenceCR(Ferda.Guha.Math.ContingencyTable)"/>
    /// <remarks>Values are from interval [0, INF]</remarks>
    public class Functions : TwoDimensionsValueDisp_, IFunctions
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
                return MissingInformationHandlingEnum.Deleting;
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
                return (BoundTypeEnum)Enum.Parse(
                    typeof(BoundTypeEnum),
                    _boxModule.GetPropertyString(Common.PropFromRowBoundary)
                    );
            }
        }
        public int FromRowBoundaryIndex
        {
            get
            {
                return _boxModule.GetPropertyInt(Common.PropFromRowBoundaryIndex);
            }
        }
        public BoundTypeEnum ToRowBoundary
        {
            get
            {
                return (BoundTypeEnum)Enum.Parse(
                    typeof(BoundTypeEnum),
                    _boxModule.GetPropertyString(Common.PropToRowBoundary)
                    );
            }
        }
        public int ToRowBoundaryIndex
        {
            get
            {
                return _boxModule.GetPropertyInt(Common.PropToRowBoundaryIndex);
            }
        }
        public BoundTypeEnum FromColumnBoundary
        {
            get
            {
                return (BoundTypeEnum)Enum.Parse(
                    typeof(BoundTypeEnum),
                    _boxModule.GetPropertyString(Common.PropFromColumnBoundary)
                    );
            }
        }
        public int FromColumnBoundaryIndex
        {
            get
            {
                return _boxModule.GetPropertyInt(Common.PropFromColumnBoundaryIndex);
            }
        }
        public BoundTypeEnum ToColumnBoundary
        {
            get
            {
                return (BoundTypeEnum)Enum.Parse(
                    typeof(BoundTypeEnum),
                    _boxModule.GetPropertyString(Common.PropToColumnBoundary)
                    );
            }
        }
        public int ToColumnBoundaryIndex
        {
            get
            {
                return _boxModule.GetPropertyInt(Common.PropToColumnBoundaryIndex);
            }
        }
        public QuantifierClassEnum[] QuantifierClasses
        {
            get
            {
                return new QuantifierClassEnum[]{};
            }
        }
        public PerformanceDifficultyEnum PerformanceDifficulty
        {
            get
            {
                return PerformanceDifficultyEnum.Medium;
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
                return UnitsEnum.RelativeToActCondition;
            }
        }
        public bool SupportsFloatContingencyTable
        {
            get
            {
                return true;
            }
        }
        
        DependenceDirection DependenceDirection
        {
            get
            {
                return (DependenceDirection)Enum.Parse(
                    typeof(DependenceDirection),
                    _boxModule.GetPropertyString(Common.PropDependenceDirection)
                    );
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
            ContingencyTable table = new ContingencyTable(param);
            switch (DependenceDirection)
            {
                case DependenceDirection.RowsOnColumns:
                    return Guha.Math.InformationTheory.InformationDependenceRC(table);
                case DependenceDirection.ColumnsOnRows:
                    return Guha.Math.InformationTheory.InformationDependenceCR(table);
                default:
                    throw new NotImplementedException();
            }
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