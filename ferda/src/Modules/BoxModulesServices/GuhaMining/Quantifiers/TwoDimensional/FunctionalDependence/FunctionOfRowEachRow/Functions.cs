using System;
using Ferda.Guha.Data;
using Ferda.Guha.Math;
using Ferda.Guha.Math.Quantifiers;

namespace Ferda.Modules.Boxes.GuhaMining.Quantifiers.TwoDimensional.FunctionalDependence.FunctionOfRowEachRow
{
    /// <summary>
    /// <para>
    /// Tests functional dependence of columns on rows. Test
    /// if number of object that fault dependence Col = F(Row)
    /// is for each value (row category) relatively "small". 
    /// Number of object which fault this depencence
    /// can be for each <c>i</c> computed as 
    /// <c>(Sum(j)(Freq i,j) - Max(j)(Freq i,j))</c>.
    /// </para>
    /// <para>
    /// Tests are following: for each <c>i</c> is 
    /// <c>Max(j)(Freq i,j) [Relation(default &gt;=)] p * Sum(j)(Freq i,j)</c>;
    /// and for each <c>i</c> is 
    /// <c>Sum(j)(Freq i,j) - Max(j)(Freq i,j) [Opposite! Relation] Tr</c>.
    /// </para>
    /// <para>
    /// This quantifier has not defined value. Returned value 
    /// depends on specified relation. For &gt; is returned Min(i) of <c>Xi</c>
    /// and for &lt; is returned Max(i) of <c>Xi</c>, where 
    /// <c>Xi = (Max(j)(Freq i,j) / Sum(j)(Freq i,j))</c>. Values are from interval (0, 1].
    /// </para>
    /// </summary>
    /// <remarks>
    /// <para>
    /// See [032 Zadání pro KL-Miner.doc] chapter 4.1.3.
    /// </para>
    /// <para>
    /// Sometimes also FunctionEachRow
    /// </para>
    /// </remarks>
    public class Functions : TwoDimensionsSignificantValueDisp_, IFunctions
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
                return PerformanceDifficultyEnum.QuiteEasy;
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

        double AbsoluteTreshold
        {
            get
            {
                return _boxModule.GetPropertyDouble(Common.PropAbsoluteTreshold);
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
            double p = Treshold;
            double absTr = AbsoluteTreshold;

            bool watchForResult = (0 != Guha.Math.Common.GetRelationOrientation(Relation));
            value = Double.NaN;

            for (int r = 0; r < table.NumberOfRows; r++)
            {
                //row maximum
                double rowMaxFreq = Double.MinValue;
                double rowSum = 0;

                for (int c = 0; c < table.NumberOfColumns; c++)
                {
                    rowMaxFreq = System.Math.Max(rowMaxFreq, table[r, c]);
                    rowSum += table[r, c];
                }
                //Tests:
                if (Guha.Math.Common.Compare(Relation, rowMaxFreq, p * rowSum) //test on p
                    && Guha.Math.Common.Compare(Relation, (absTr * table.Denominator) - rowSum, -rowMaxFreq)) //test on Tr (opposite relation)
                {
                    if (watchForResult)
                        value = Guha.Math.Common.GetOrientationBetterValue(Relation, value, rowMaxFreq / rowSum);
                }
                else
                {
                    value = Double.NaN;
                    return false;
                }
            }
            return true;
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
