using System;
using Ferda.Guha.Data;
using Ferda.Guha.Math;
using Ferda.Guha.Math.Quantifiers;
using Ice;

namespace Ferda.Modules.Boxes.GuhaMining.Quantifiers.FourFold.Equivalence.FoundedEquivalence
{
    /// <summary>
    /// Computes the <c>strength</c> of founded equivalence.
    /// </summary>
    /// <returns>
    /// The strength defined as <c>(a + d) / (a + b + c + d)</c>.
    /// If (a + b + c + d) = 0, returns NaN. Values are from interval [0, 1].
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
                return (OperationModeEnum) Enum.Parse(
                                               typeof (OperationModeEnum),
                                               _boxModule.GetPropertyString(Common.PropOperationMode)
                                               );
            }
        }

        public MissingInformationHandlingEnum MissingInformationHandling
        {
            get
            {
                return (MissingInformationHandlingEnum) Enum.Parse(
                                                            typeof (MissingInformationHandlingEnum),
                                                            _boxModule.GetPropertyString(
                                                                Common.PropMissingInformationHandling)
                                                            );
            }
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
            get { return new QuantifierClassEnum[] {QuantifierClassEnum.SigmaEquivalency}; }
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

        public override bool Compute(QuantifierEvaluateSetting param, Current current__)
        {
            double value = ComputeValue(param);
            return Guha.Math.Common.Compare(Relation, value, Treshold);
        }

        public override bool ComputeValidValue(QuantifierEvaluateSetting param, out double value, Current current__)
        {
            value = ComputeValue(param);
            return Guha.Math.Common.Compare(Relation, value, Treshold);
        }

        public override double ComputeValue(QuantifierEvaluateSetting param, Current current__)
        {
            return ExceptionsHandler.TryCatchMethodThrow<double>(
                delegate
                    {
                        FourFoldContingencyTable table = new FourFoldContingencyTable(param);
                        double n = table.N;
                        return (n > 0) ? (table.A + table.D)/n : Double.NaN;
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
    }
}