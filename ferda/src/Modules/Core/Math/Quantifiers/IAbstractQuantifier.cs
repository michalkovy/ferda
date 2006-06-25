using System;
using System.Collections.Generic;
using Ferda.Guha.Data;

namespace Ferda.Guha.Math.Quantifiers
{
    public interface IQuantifierTypeProperties
    {
        double MinValue { get; }
        double MaxValue { get; }

        PerformanceDifficultyEnum QuantifierPerformanceDifficulty { get; }

        bool NeedsNumericValues { get; }

        CardinalityEnum SupportedData { get; }

        QuantifierClassEnum[] QuantifierClasses { get; }

        bool IrrelevantOnUnits { get; }
        bool SupportsFloatContingencyTable { get; }
    }

    public interface IQuantifierSettingProperties
    {
        OperationModeEnum OperationMode { get; }

        Bound FromRow { get; }
        Bound ToRow { get; }
        Bound FromColumn { get; }
        Bound ToColumn { get; }

        UnitsEnum Units { get; }

        double Treshold { get; }

        //TODO ohter params?

        RelationEnum Relation { get; }
    }

    /// <summary>
    /// "Bezne" kvantifikatory, ktere vraci hodnotu, ktera bude nasledne 
    /// porovnana (dle Relation) s Treshold.
    /// </summary>
    public interface IQuantifierValue : IQuantifierTypeProperties
    {
        double Value(IQuantifierValueData data);
    }

    /// <summary>
    /// Kvantifikatory, kde je obecne velmi obtizne vycislit hodnotu, 
    /// ale je snadne vyhodnotit platnost. Tyto kvantifikatory bud vraci
    /// false (a value NaN), nebo true a dovypoctenou hodnotu.
    /// </summary>
    public interface IQuantifierValidate : IQuantifierTypeProperties
    {
        bool Validate(IQuantifierValidateData data, out double value);
    }

    /// <summary>
    /// Kvantifikátory, které nemají pøímo definovanou hodnotu, ale
    /// lze urèit hodnotu, dle níž lze hypotézy vzájemnì porovnávat.
    /// </summary>
    public interface IQuantifierValidateWithValueOfInterest : IQuantifierTypeProperties
    {
        bool Validate(IQuantifierValidateData data, out double value);
    }

    /// <summary>
    /// "Velmi specificky" typ kvantifikatoru, prikladem muze byt E-kvantifikator
    /// kde se vypocitaji dve hodnoty a obe se porovnaji s Treshold, tedy kvantifikator
    /// nema definovanou hodnotu pouze platnost.
    /// Pozn. pokud vstupni hodnoty nemají smysl (NaN by byl obvykle vrácen) je výsledkem
    /// false.
    /// </summary>
    public interface IQuantifierValid : IQuantifierTypeProperties
    {
        bool Validate(IQuantifierValidateData data);
    }

    #region Data for Quantifier - Interfaces
    
    public interface IQuantifierValueData
    {
        ContingencyTable ContingencyTable { get; }
        FourFoldContingencyTable FourFoldContingencyTable { get; }
        SingleDimensionContingecyTable SingleDimensionContingecyTable { get; }
        object GetParam(string paramName);
        double[] NumericValues { get; }
    }

    // U nìkterých kvantifikátorù mùže být obtížné vyèíslit pøesnou hodnotu value,
    // ale mùže být relativnì jednoduché odhadnotu validitu, value je dopoètena až
    // když je splnìna validita
    public interface IQuantifierValidateData : IQuantifierValueData
    {
        double Treshold { get; }
        RelationEnum Relation { get; }
    }
    
    #endregion

    public class QuantifierValidateData : IQuantifierValidateData
    {
        private readonly double[][] _contingencyTable;
        private readonly double _denominator;

        private readonly double _treshold;
        private readonly RelationEnum _relation;
        private readonly Dictionary<string, object> _otherParams;
        
        private readonly double[] _numericValues;

        public QuantifierValidateData(
            double[][] contingencyTable,
            double denominator,
            double treshold,
            RelationEnum relation,
            Dictionary<string, object> otherParams,
            double[] numericValues
            )
        {
            _contingencyTable = contingencyTable;
            _denominator = denominator;

            _treshold = treshold;
            _relation = relation;
            _otherParams = otherParams;

            _numericValues = numericValues;
        }

        #region IQuantifierValidateData Members

        public ContingencyTable ContingencyTable
        {
            get { return new ContingencyTable(_contingencyTable, _denominator); }
        }

        public FourFoldContingencyTable FourFoldContingencyTable
        {
            get { return new FourFoldContingencyTable(_contingencyTable, _denominator); }
        }

        public SingleDimensionContingecyTable SingleDimensionContingecyTable
        {
            get { return new SingleDimensionContingecyTable(_contingencyTable, _denominator); }
        }

        public object GetParam(string paramName)
        {
            if (_otherParams == null)
                throw new NullReferenceException();
            object result;
            if (_otherParams.TryGetValue(paramName, out result))
                return result;
            else
                throw new ArgumentOutOfRangeException();
        }

        public double Treshold
        {
            get { return _treshold; }
        }

        public RelationEnum Relation
        {
            get { return _relation; }
        }

        public double[] NumericValues
        {
            get
            {
                if (_numericValues == null)
                    throw new InvalidOperationException();
                System.Diagnostics.Debug.Assert(_numericValues.Length == ContingencyTable.NumberOfColumns);
                return _numericValues;
            }
        }

        #endregion
    }
}