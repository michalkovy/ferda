using System;
using Ferda.Guha.Data;
using Ferda.Guha.Math;
using Ferda.Guha.Math.Quantifiers;
using Ice;

namespace Ferda.Modules.Boxes.GuhaMining.Quantifiers.OneDimensional.CardinalVariableDistributionCharacteristics.Skewness
{
    /// <summary>
    /// Skewness is defined as 
    /// <c>(N'' - N') / N</c> where 
    /// <c>N''</c> is number of values greater than arithmetic value;
    /// <c>N'</c> is number of values lower than arithmetic value;
    /// <c>N</c> is number of all objects.
    /// </summary>
    /// <remarks>
    /// Value = 0 means that distribution is symetrical along Arithmetic average;
    /// Value &gt; 0 means that predominates deviations to right side;
    /// Value &lt; 0 means that predominates deviations to left side.
    /// </remarks>
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
                return (OperationModeEnum) Enum.Parse(
                                               typeof (OperationModeEnum),
                                               _boxModule.GetPropertyString(Common.PropOperationMode)
                                               );
            }
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
            get { return 0; }
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
            get { return true; }
        }

        public CardinalityEnum SupportedData
        {
            get { return CardinalityEnum.Cardinal; }
        }

        public UnitsEnum Units
        {
            get { return UnitsEnum.RelativeToActCondition; }
        }

        public bool SupportsFloatContingencyTable
        {
            get { return true; }
        }

        public override bool Compute(QuantifierEvaluateSetting param, Current current__)
        {
            double value = ComputeValue(param, current__);
            return Guha.Math.Common.Compare(Relation, value, Treshold);
        }

        public override bool ComputeValidValue(QuantifierEvaluateSetting param, out double value, Current current__)
        {
            value = ComputeValue(param, current__);
            return Guha.Math.Common.Compare(Relation, value, Treshold);
        }

        public override double ComputeValue(QuantifierEvaluateSetting param, Current current__)
        {
            return ExceptionsHandler.TryCatchMethodThrow<double>(
                delegate
                    {
                        SingleDimensionContingecyTable table = new SingleDimensionContingecyTable(param);
                        double[] values = Common.GetNumericValuesAsync(param).Result; //TODO: make sync
                        double arithmeticAverage = AritmeticAverage.Functions.ComputeAritmeticAverage(table, values);
                        double N_ = 0; // N'
                        double N__ = 0; // N''
                        for (int c = 0; c < table.NumberOfColumns; c++)
                        {
                            if (values[c] < arithmeticAverage)
                                N_ += table[c];
                            else if (values[c] > arithmeticAverage)
                                N__ += table[c];
                        }
                        return (N__ - N_);
                        // not divided by table.Sum because only relative frequencies are expected
                    },
                _boxModule.StringIceIdentity
                );
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