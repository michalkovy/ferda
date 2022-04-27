// Common.cs - Common functionality for GUHA quantifiers implemented
// in the Ferda system
//
// Author: Tomáš Kuchaø <tomas.kuchar@gmail.com>
// Documented by: Martin Ralbovský <martin.ralbovsky@gmail.com>
//
// Copyright (c) 2006 Tomáš Kuchaø, Martin Ralbovský
//
// This program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA

using System;
using System.Collections.Generic;
using Ferda.Guha.Math.Quantifiers;
using Ferda.Guha.MiningProcessor;
using Ferda.Modules.Helpers.Caching;

namespace Ferda.Modules.Boxes.GuhaMining.Quantifiers
{
    /// <summary>
    /// Class providing static functions that are in common to all
    /// GUHA quantifiers
    /// </summary>
    public static class Common
    {
        #region Properties

        //names of the properties/sockets
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

        /// <summary>
        /// Cache of numeric values of quantifiers computation
        /// </summary>
        private static NumericValuesCache _numericValuesCache = new NumericValuesCache();

        /// <summary>
        /// Retrieves values numerical values of a (probably one dimensional)
        /// contingency table. The function is used when computing distribution
        /// characteristics of a cardinal variable.
        /// </summary>
        /// <param name="param">Settings for quantifier evaluation</param>
        /// <returns>Numerical values of a (one dimensional) contingency table</returns>
        public static async Task<double[]> GetNumericValuesAsync(QuantifierEvaluateSetting param)
        {
            Guid numericValuesAttributeId = new Guid(param.numericValuesAttributeId.value);
            BitStringGeneratorPrx numericValuesProviderPrx = param.numericValuesProviders;
            return await _numericValuesCache.GetValueAsync(
                new GuidPrxPair<BitStringGeneratorPrx>(
                    numericValuesAttributeId,
                    numericValuesProviderPrx
                    )
                ).ConfigureAwait(false);
        }
        
        /// <summary>
        /// Delegate for computation of a quantifier over
        /// <see cref="Ferda.Guha.Math.Quantifiers.QuantifierEvaluateSetting"/>.
        /// </summary>
        /// <param name="param">Setting for quantifier evaluation</param>
        /// <param name="current">Ice current</param>
        /// <returns>Iff the quantifier is valid over the setting</returns>
        public delegate bool ComputeDelegate(QuantifierEvaluateSetting param, Ice.Current current);
        
        /// <summary>
        /// Executes a batch computation of quantifiers.
        /// </summary>
        /// <param name="param">Settings for quantifier evaluation</param>
        /// <param name="compute">Computation delegate</param>
        /// <returns>Array of boolean values corresponding to the validity
        /// of quantifier evaluation settings in <paramref name="param"/></returns>
        public static bool[] ComputeBatch(QuantifierEvaluateSetting[] param, ComputeDelegate compute)
        {
            List<bool> result = new List<bool>(param.Length);
            for (int i = 0; i < param.Length; i++)
            {
                result.Insert(i, compute(param[i],null));
            }
            return result.ToArray();
        }
    }

    /// <summary>
    /// Class for evaluating two equality of two pairs of 
    /// objects identified by their GUID
    /// </summary>
    /// <typeparam name="T">Type of any kind</typeparam>
    public class GuidPrxPair<T> : IEquatable<GuidPrxPair<T>>
    {
        /// <summary>
        /// Identifier of the object
        /// </summary>
        public Guid Guid;
        /// <summary>
        /// Object itself
        /// </summary>
        public T Prx;

        /// <summary>
        /// Default construcor for the class
        /// </summary>
        /// <param name="guid">Identifier of an object</param>
        /// <param name="prx">Object</param>
        public GuidPrxPair(Guid guid, T prx)
        {
            Guid = guid;
            Prx = prx;
        }

        #region IEquatable<GuidPrxPair<T>> Members

        /// <summary>
        /// Function returns equality of objects
        /// </summary>
        /// <param name="other">The compared object</param>
        /// <returns>Equality</returns>
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

    /// <summary>
    /// Cache for numeric values of an attribute. It is a dictionary indexed by the
    /// bit string generator identification.
    /// </summary>
    public class NumericValuesCache : MostRecentlyUsed<GuidPrxPair<BitStringGeneratorPrx>, double[]>
    {
        /// <summary>
        /// Default size of the cache
        /// </summary>
        public const int DefaultNumericValuesCacheSize = 1;

        /// <summary>
        /// Default constructor for the class
        /// </summary>
        public NumericValuesCache()
            : base(DefaultNumericValuesCacheSize)
        {
        }

        /// <summary>
        /// Gets value indexed by the bit string generator identification
        /// </summary>
        /// <param name="key">Key</param>
        /// <returns>Value (numerical values)</returns>
        public override Task<double[]> GetValueExternalAsync(GuidPrxPair<BitStringGeneratorPrx> key)
        {
            return key.Prx.GetCategoriesNumericValuesAsync();
        }
    }
}