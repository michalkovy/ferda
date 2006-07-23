using System;
using Ferda.Guha.Math.Quantifiers;
using Ferda.Guha.MiningProcessor;
using Ferda.Modules.Helpers.Caching;

namespace Ferda.Modules.Boxes.GuhaMining.Quantifiers
{
    public static class Common
    {
        #region Properties

        public const string PropOperationMode = "OperationMode";
        public const string PropMissingInformationHandling = "MissingInformationHandling";
        public const string PropRelation = "Relation";
        public const string PropTreshold = "Treshold";
        public const string PropFromRowBoundary = "FromRowBoundary";
        public const string PropToRowBoundary = "ToRowBoundary";
        public const string PropFromColumnBoundary = "FromColumnBoundary";
        public const string PropToColumnBoundary = "ToColumnBoundary";
        public const string PropFromRowBoundaryIndex = "FromRowBoundaryIndex";
        public const string PropToRowBoundaryIndex = "ToRowBoundaryIndex";
        public const string PropFromColumnBoundaryIndex = "FromColumnBoundaryIndex";
        public const string PropToColumnBoundaryIndex = "ToColumnBoundaryIndex";
        public const string PropQuantifierClasses = "QuantifierClasses";
        public const string PropPerformanceDifficulty = "PerformanceDifficulty";
        public const string PropNeedsNumericValues = "NeedsNumericValues";
        public const string PropSupportedData = "SupportedData";
        public const string PropUnits = "Units";
        public const string PropSupportsFloatContingencyTable = "SupportsFloatContingencyTable";

        //TODO
        public const string PropDependenceDirection = "DependenceDirection";
        public const string PropAbsoluteTreshold = "AbsoluteTreshold";

        #endregion

        private static NumericValuesCache _numericValuesCache = new NumericValuesCache();

        public static double[] GetNumericValues(QuantifierEvaluateSetting param)
        {
            Guid numericValuesAttributeId = new Guid(param.numericValuesAttributeId.value);
            BitStringGeneratorPrx numericValuesProviderPrx = param.numericValuesProviders;
            return _numericValuesCache[
                new GuidPrxPair<BitStringGeneratorPrx>(
                    numericValuesAttributeId,
                    numericValuesProviderPrx
                    )
                ];
        }
    }

    public class GuidPrxPair<T> : IEquatable<GuidPrxPair<T>>
    {
        public Guid Guid;
        public T Prx;

        public GuidPrxPair(Guid guid, T prx)
        {
            Guid = guid;
            Prx = prx;
        }

        #region IEquatable<GuidPrxPair<T>> Members

        public bool Equals(GuidPrxPair<T> other)
        {
            if (other == null)
                return false;
            if (other.Guid == Guid)
                return true;
            return false;
        }

        #endregion
    }

    public class NumericValuesCache : MostRecentlyUsed<GuidPrxPair<BitStringGeneratorPrx>, double[]>
    {
        public const int DefaultNumericValuesCacheSize = 1;

        public NumericValuesCache()
            : base(DefaultNumericValuesCacheSize)
        {
        }

        public override double[] GetValue(GuidPrxPair<BitStringGeneratorPrx> key)
        {
            return key.Prx.GetCategoriesNumericValues();
        }
    }
}