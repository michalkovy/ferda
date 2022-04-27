using System;
using Ferda.Guha.Data;
using Ferda.Guha.Math;
using Ferda.Guha.Math.Quantifiers;
using Ice;

namespace Ferda.Modules.Boxes.GuhaMining.Quantifiers.TwoDimensional.FunctionalDependence.FunctionOfRow
{
    /// <summary>
    /// <para>
    /// Tests functional dependence of columns on rows. Test
    /// if Col = F(Row). Number of object which fault this depencence
    /// can be computed as <c>Sum(i)(Sum(j)(Freq i,j) - Max(j)(Freq i,j))</c>
    /// i.e. <c>M - Sum(i)(Max(j)(Freq i,j))</c> where <c>M = Sum(i,j)(Freq i,j)</c>.
    /// </para>
    /// <para>
    /// Tests are following: <c>Sum(i)(Max(j)(Freq i,j)) [Relation(default &gt;=)] p * M</c>;
    /// and <c>M - Sum(i)(Max(j)(Freq i,j)) [Opposite! Relation] Tr</c>.
    /// </para>
    /// <para>
    /// This quantifier has not defined value. Returned value is 
    /// <c>Sum(i)(Max(j)(Freq i,j)) / M</c> as good understandable 
    /// value for ordering/comparing hypotheses. Values are from interval (0, 1].
    /// </para>
    /// </summary>
    /// <remarks>
    /// <para>
    /// See [032 Zadání pro KL-Miner.doc] chapter 4.1.1.
    /// </para>
    /// <para>
    /// Sometimes also FunctionSumOfRows
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
            get { return OperationModeEnum.FirstSetFrequencies; }
        }

        public MissingInformationHandlingEnum MissingInformationHandling
        {
            get { return MissingInformationHandlingEnum.Deleting; }
        }

        public RelationEnum Relation
        {
            get
            {
                return (RelationEnum) Enum.Parse(
                                          typeof (RelationEnum),
                                          _boxModule.GetPropertyString(Common.PropRelation)
                                          );
            }
        }

        private bool _running = false;
        private double _treshold;

        public override void BeginOfUse(Current current__)
        {
            _treshold = this.Treshold;
            _running = true;
        }

        public override void EndOfUse(Current current__)
        {
            _running = false;
        }

        public double Treshold
        {
            get { return (_running) ? _treshold : _boxModule.GetPropertyDouble(Common.PropTreshold); }
        }

        public BoundTypeEnum FromRowBoundary
        {
            get
            {
                return (BoundTypeEnum) Enum.Parse(
                                           typeof (BoundTypeEnum),
                                           _boxModule.GetPropertyString(Common.PropFromRowBoundary)
                                           );
            }
        }

        public int FromRowBoundaryIndex
        {
            get { return _boxModule.GetPropertyInt(Common.PropFromRowBoundaryIndex); }
        }

        public BoundTypeEnum ToRowBoundary
        {
            get
            {
                return (BoundTypeEnum) Enum.Parse(
                                           typeof (BoundTypeEnum),
                                           _boxModule.GetPropertyString(Common.PropToRowBoundary)
                                           );
            }
        }

        public int ToRowBoundaryIndex
        {
            get { return _boxModule.GetPropertyInt(Common.PropToRowBoundaryIndex); }
        }

        public BoundTypeEnum FromColumnBoundary
        {
            get
            {
                return (BoundTypeEnum) Enum.Parse(
                                           typeof (BoundTypeEnum),
                                           _boxModule.GetPropertyString(Common.PropFromColumnBoundary)
                                           );
            }
        }

        public int FromColumnBoundaryIndex
        {
            get { return _boxModule.GetPropertyInt(Common.PropFromColumnBoundaryIndex); }
        }

        public BoundTypeEnum ToColumnBoundary
        {
            get
            {
                return (BoundTypeEnum) Enum.Parse(
                                           typeof (BoundTypeEnum),
                                           _boxModule.GetPropertyString(Common.PropToColumnBoundary)
                                           );
            }
        }

        public int ToColumnBoundaryIndex
        {
            get { return _boxModule.GetPropertyInt(Common.PropToColumnBoundaryIndex); }
        }

        public QuantifierClassEnum[] QuantifierClasses
        {
            get { return new QuantifierClassEnum[] {}; }
        }

        public PerformanceDifficultyEnum PerformanceDifficulty
        {
            get { return PerformanceDifficultyEnum.QuiteEasy; }
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
            get
            {
                return (UnitsEnum) Enum.Parse(
                                       typeof (UnitsEnum),
                                       _boxModule.GetPropertyString(Common.PropUnits)
                                       );
            }
        }

        public bool SupportsFloatContingencyTable
        {
            get { return true; }
        }

        public double AbsoluteTreshold
        {
            get { return _boxModule.GetPropertyDouble(Common.PropAbsoluteTreshold); }
        }

        public override bool Compute(QuantifierEvaluateSetting param, Current current__)
        {
            double value;
            return ComputeValidValue(param, out value, current__);
        }

        public override bool ComputeValidValue(QuantifierEvaluateSetting param, out double value, Current current__)
        {
            ContingencyTable table = new ContingencyTable(param);
            double p = Treshold;
            double absTr = AbsoluteTreshold;

            double m = table.Sum;

            // sum of row maximums
            double sumRowMaxFreq = 0;

            for (int r = 0; r < table.NumberOfRows; r++)
            {
                //row maximum
                double rowMaxFreq = Double.MinValue;

                for (int c = 0; c < table.NumberOfColumns; c++)
                {
                    rowMaxFreq = Math.Max(rowMaxFreq, table[r, c]);
                }
                sumRowMaxFreq += rowMaxFreq;
            }

            //Tests:
            if (Guha.Math.Common.Compare(Relation, sumRowMaxFreq, p*m) //test on p
                && Guha.Math.Common.Compare(Relation, (absTr*table.Denominator) - m, -sumRowMaxFreq))
                //test on Tr (opposite relation)
            {
                value = sumRowMaxFreq/m;
                return true;
            }
            else
            {
                value = Double.NaN;
                return false;
            }
        }

        public override string GetLocalizedBoxLabel(string[] localePrefs, Current current__)
        {
            return _boxModule.BoxInfo.GetLabel(localePrefs);
        }

        public override string GetLocalizedUserBoxLabel(string[] localePrefs, Current current__)
        {
            return _boxModule.Manager.getProjectInformation().getUserLabel(_boxModule.StringIceIdentity);
        }

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

        public override bool[] ComputeBatch(QuantifierEvaluateSetting[] param, Current current__)
        {
            return Common.ComputeBatch(param, this.Compute);
        }
    }
}