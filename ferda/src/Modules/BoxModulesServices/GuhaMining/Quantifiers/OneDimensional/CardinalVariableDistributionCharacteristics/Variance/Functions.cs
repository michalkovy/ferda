using System;
using System.Collections.Generic;
using System.Text;
using Ferda.Guha.Data;
using Ferda.Guha.Math;
using Ferda.Guha.Math.Quantifiers;

namespace Ferda.Modules.Boxes.GuhaMining.Quantifiers.OneDimensional.CardinalVariableDistributionCharacteristics.Variance
{
    /// <summary>
    /// Variance is defined as 
    /// <c>Sum(i)( (Freq i) * ((Value i) - AritmeticAverage)^2 )</c>
    /// </summary>
    public class Functions : SingleDimensionValueDisp_, IFunctions
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
                return 0;
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
                return PerformanceDifficultyEnum.QuiteEasy;
            }
        }
        public bool NeedsNumericValues
        {
            get
            {
                return true;
            }
        }
        public CardinalityEnum SupportedData
        {
            get
            {
                return CardinalityEnum.Cardinal;
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
                        SingleDimensionContingecyTable table = new SingleDimensionContingecyTable(param);
                        double[] values = Common.GetNumericValues(param);
                        return ComputeVariance(table, values);
                    },
                _boxModule.StringIceIdentity
                );
        }

        public static double ComputeVariance(SingleDimensionContingecyTable table, double[] values)
        {
            double result = 0;
            double arithmeticAverage = AritmeticAverage.Functions.ComputeAritmeticAverage(table, values);
            for (int c = 0; c < table.NumberOfColumns; c++)
            {
                result += table[c] * System.Math.Pow(values[c] - arithmeticAverage, 2);
            }
            return result / table.Denominator;
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