using System;
using System.Collections.Generic;
using System.Text;
using Ferda.Guha.Data;
using Ferda.Guha.Math;
using Ferda.Guha.Math.Quantifiers;

namespace Ferda.Modules.Boxes.GuhaMining.Quantifiers.OneOrTwoDimensional.Aggregation.AnyValue
{
    public class Functions : SingleDimensionTwoDimensionsSignificantValueDisp_, IFunctions
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
                return OperationModeEnum.FirstSetFrequencies;
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
                return (UnitsEnum)Enum.Parse(
                    typeof(UnitsEnum),
                    _boxModule.GetPropertyString(Common.PropUnits)
                    );
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
            double value;
            return ComputeValidValue(param, out value);
        }

        public override bool ComputeValidValue(QuantifierEvaluateSetting param, out double value, Ice.Current current__)
        {
            ContingencyTable table = new ContingencyTable(param);
            int orientation = Guha.Math.Common.GetRelationOrientation(Relation);
            if (orientation == 0)
            {
                for (int r = 0; r < table.NumberOfRows; r++)
                {
                    for (int c = 0; c < table.NumberOfColumns; c++)
                    {
                        if (Guha.Math.Common.Compare(Relation, table[r, c] / table.Denominator, Treshold))
                        {
                            value = Treshold;
                            return true;
                        }
                    }
                }
            }
            else if (orientation > 0)
            {
                if (Guha.Math.Common.Compare(Relation, table.Max / table.Denominator, Treshold))
                {
                    value = table.Max / table.Denominator;
                    return true;
                }
            }
            else // if (orientation < 0)
            {
                if (Guha.Math.Common.Compare(Relation, table.Min / table.Denominator, Treshold))
                {
                    value = table.Min / table.Denominator;
                    return true;
                }
            }
            value = Double.NaN;
            return false;
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
