using System;
using Ferda.Guha.Data;

namespace Ferda.Guha.Math.Quantifiers
{
    #region Aggregatinal

    public class Maximum : IQuantifierValue
    {
        #region IQuantifierValue Members

        public double Value(IQuantifierValueData data)
        {
            ContingencyTable table = data.ContingencyTable;
            return table.Max / table.Denominator;
        }

        #endregion

        #region IQuantifierTypeProperties Members

        public double MinValue
        {
            get { return 0; }
        }

        public double MaxValue
        {
            get { return Double.MaxValue; }
        }

        public PerformanceDifficultyEnum QuantifierPerformanceDifficulty
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

        public QuantifierClassEnum[] QuantifierClasses
        {
            get { return new QuantifierClassEnum[] { }; }
        }

        public bool IrrelevantOnUnits
        {
            get { return false; }
        }

        public bool SupportsFloatContingencyTable
        {
            get { return true; }
        }

        #endregion
    }

    public class Minimum : IQuantifierValue
    {
        #region IQuantifierValue Members

        public double Value(IQuantifierValueData data)
        {
            ContingencyTable table = data.ContingencyTable;
            return table.Min / table.Denominator;
        }

        #endregion

        #region IQuantifierTypeProperties Members

        public double MinValue
        {
            get { return 0; }
        }

        public double MaxValue
        {
            get { return Double.MaxValue; }
        }

        public PerformanceDifficultyEnum QuantifierPerformanceDifficulty
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

        public QuantifierClassEnum[] QuantifierClasses
        {
            get { return new QuantifierClassEnum[] { }; }
        }

        public bool IrrelevantOnUnits
        {
            get { return false; }
        }

        public bool SupportsFloatContingencyTable
        {
            get { return true; }
        }

        #endregion
    }

    public class Sum : IQuantifierValue
    {
        #region IQuantifierValue Members

        public double Value(IQuantifierValueData data)
        {
            ContingencyTable table = data.ContingencyTable;
            return table.Sum / table.Denominator;
        }

        #endregion

        #region IQuantifierTypeProperties Members

        public double MinValue
        {
            get { return 0; }
        }

        public double MaxValue
        {
            get { return Double.MaxValue; }
        }

        public PerformanceDifficultyEnum QuantifierPerformanceDifficulty
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

        public QuantifierClassEnum[] QuantifierClasses
        {
            get { return new QuantifierClassEnum[] { }; }
        }

        public bool IrrelevantOnUnits
        {
            get { return false; }
        }

        public bool SupportsFloatContingencyTable
        {
            get { return true; }
        }

        #endregion
    }

    public class Average : IQuantifierValue
    {
        #region IQuantifierValue Members

        public double Value(IQuantifierValueData data)
        {
            ContingencyTable table = data.ContingencyTable;
            return table.Sum / (table.Denominator * (table.NumberOfRows * table.NumberOfColumns));
        }

        #endregion

        #region IQuantifierTypeProperties Members

        public double MinValue
        {
            get { return 0; }
        }

        public double MaxValue
        {
            get { return Double.MaxValue; }
        }

        public PerformanceDifficultyEnum QuantifierPerformanceDifficulty
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

        public QuantifierClassEnum[] QuantifierClasses
        {
            get { return new QuantifierClassEnum[] { }; }
        }

        public bool IrrelevantOnUnits
        {
            get { return false; }
        }

        public bool SupportsFloatContingencyTable
        {
            get { return true; }
        }

        #endregion
    }

    public class Any : IQuantifierValidate
    {
        #region IQuantifierValue Members

        public bool Validate(IQuantifierValidateData data, out double value)
        {
            ContingencyTable table = data.ContingencyTable;
            int orientation = Common.GetRelationOrientation(data.Relation);
            if (orientation == 0)
            {
                for (int r = 0; r < table.NumberOfRows; r++)
                {
                    for (int c = 0; c < table.NumberOfColumns; c++)
                    {
                        if (Common.Compare(data.Relation, table[r, c] / table.Denominator, data.Treshold))
                        {
                            value = table[r, c];
                            return true;
                        }
                    }
                }
            }
            else if (orientation > 0)
            {
                if (Common.Compare(data.Relation, table.Max / table.Denominator, data.Treshold))
                {
                    value = table.Max;
                    return true;
                }
            }
            else // if (orientation < 0)
            {
                if (Common.Compare(data.Relation, table.Min / table.Denominator, data.Treshold))
                {
                    value = table.Min;
                    return true;
                }
            }
            value = Double.NaN;
            return false;
        }

        #endregion

        #region IQuantifierTypeProperties Members

        public double MinValue
        {
            get { return 0; }
        }

        public double MaxValue
        {
            get { return Double.MaxValue; }
        }

        public PerformanceDifficultyEnum QuantifierPerformanceDifficulty
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

        public QuantifierClassEnum[] QuantifierClasses
        {
            get { return new QuantifierClassEnum[] { }; }
        }

        public bool IrrelevantOnUnits
        {
            get { return false; }
        }

        public bool SupportsFloatContingencyTable
        {
            get { return true; }
        }

        #endregion
    }

    #endregion

    #region Information Theory

    /// <summary>
    /// Mutual information normalized. Value of Mutual information I(X,Y)
    /// is normalized into interval [0,1].
    /// MI*(X,Y) = I(X,Y) / Min{ H(X), H(Y) } = 
    /// ( H(X) + H(Y) - H(X,Y) ) / Min{ H(X), H(Y) }.
    /// </summary>
    /// <seealso cref="M:Ferda.Guha.Math.InformationTheory.MutualInformationNormalized(Ferda.Guha.Math.ContingencyTable)"/>
    public class MutualInformationNormalized : IQuantifierValue
    {
        #region IQuantifierValue Members

        public double Value(IQuantifierValueData data)
        {
            return InformationTheory.MutualInformationNormalized(data.ContingencyTable);
        }

        #endregion

        #region IQuantifierTypeProperties Members

        public double MinValue
        {
            get { return 0.0d; }
        }

        public double MaxValue
        {
            get { return 1.0d; }
        }

        public PerformanceDifficultyEnum QuantifierPerformanceDifficulty
        {
            get { return PerformanceDifficultyEnum.Medium; }
        }

        public bool NeedsNumericValues
        {
            get { return false; }
        }

        public CardinalityEnum SupportedData
        {
            get { return CardinalityEnum.Nominal; }
        }

        public QuantifierClassEnum[] QuantifierClasses
        {
            get { return new QuantifierClassEnum[] { }; }
        }

        public bool IrrelevantOnUnits
        {
            get { return false; }
        }

        public bool SupportsFloatContingencyTable
        {
            get { return true; }
        }

        #endregion
    }

    /// <summary>
    /// Information dependence of R on C is defined as I(C,R) / H(R);
    /// Information dependence of C on R is defined as I(C,R) / H(C);
    /// </summary>
    /// <seealso cref="M:Ferda.Guha.Math.InformationTheory.InformationDependenceRC(Ferda.Guha.Math.ContingencyTable)"/>
    /// <seealso cref="M:Ferda.Guha.Math.InformationTheory.InformationDependenceCR(Ferda.Guha.Math.ContingencyTable)"/>
    public class InformationDependence : IQuantifierValue
    {
        #region IQuantifierValue Members

        public double Value(IQuantifierValueData data)
        {
            ContingencyTable table = data.ContingencyTable;

            DependenceDirection direction = (DependenceDirection)data.GetParam("DependenceDirection");

            switch (direction)
            {
                case DependenceDirection.RowsOnColumns:
                    return InformationTheory.InformationDependenceRC(table);
                case DependenceDirection.ColumnsOnRows:
                    return InformationTheory.InformationDependenceCR(table);
                default:
                    throw new NotImplementedException();
            }
        }

        #endregion

        #region IQuantifierTypeProperties Members

        public double MinValue
        {
            get { return 0.0d; }
        }

        public double MaxValue
        {
            get { return Double.MaxValue; }
        }

        public PerformanceDifficultyEnum QuantifierPerformanceDifficulty
        {
            get { return PerformanceDifficultyEnum.Medium; }
        }

        public bool NeedsNumericValues
        {
            get { return false; }
        }

        public CardinalityEnum SupportedData
        {
            get { return CardinalityEnum.Nominal; }
        }

        public QuantifierClassEnum[] QuantifierClasses
        {
            get { return new QuantifierClassEnum[] { }; }
        }

        public bool IrrelevantOnUnits
        {
            get { return false; }
        }

        public bool SupportsFloatContingencyTable
        {
            get { return true; }
        }

        #endregion
    }

    /// <summary>
    /// Conditional entropy H(R|C) of R on C is defined as H(C,R) - H(C);
    /// Conditional entropy H(C|R) of C on R is defined as H(C,R) - H(R);
    /// </summary>
    /// <seealso cref="M:Ferda.Guha.Math.InformationTheory.ConditionalRCEntropyValue(Ferda.Guha.Math.ContingencyTable)"/>
    /// <seealso cref="M:Ferda.Guha.Math.InformationTheory.ConditionalCREntropyValue(Ferda.Guha.Math.ContingencyTable)"/>
    public class ConditionalEntropy : IQuantifierValue
    {
        #region IQuantifierValue Members

        public double Value(IQuantifierValueData data)
        {
            ContingencyTable table = data.ContingencyTable;

            DependenceDirection direction = (DependenceDirection)data.GetParam("DependenceDirection");

            switch (direction)
            {
                case DependenceDirection.RowsOnColumns:
                    return InformationTheory.ConditionalRCEntropyValue(table);
                case DependenceDirection.ColumnsOnRows:
                    return InformationTheory.ConditionalCREntropyValue(table);
                default:
                    throw new NotImplementedException();
            }
        }

        #endregion

        #region IQuantifierTypeProperties Members

        public double MinValue
        {
            get { return 0.0d; }
        }

        public double MaxValue
        {
            get { return Double.MaxValue; }
        }

        public PerformanceDifficultyEnum QuantifierPerformanceDifficulty
        {
            get { return PerformanceDifficultyEnum.Medium; }
        }

        public bool NeedsNumericValues
        {
            get { return false; }
        }

        public CardinalityEnum SupportedData
        {
            get { return CardinalityEnum.Nominal; }
        }

        public QuantifierClassEnum[] QuantifierClasses
        {
            get { return new QuantifierClassEnum[] { }; }
        }

        public bool IrrelevantOnUnits
        {
            get { return false; }
        }

        public bool SupportsFloatContingencyTable
        {
            get { return true; }
        }

        #endregion
    }

    #endregion

}