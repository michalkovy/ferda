// CategorialAttributeTrace.cs - trace of categorial attribute
//
// Authors: Tomáš Kuchaø <tomas.kuchar@gmail.com>      
// Commented by: Martin Ralbovský <martin.ralbovsky@gmail.com>
//
// Copyright (c) 2006 Tomáš Kuchaø
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
using Ferda.Guha.Data;
using Ferda.Guha.MiningProcessor.BitStrings;
using Ferda.Guha.MiningProcessor.Formulas;

namespace Ferda.Guha.MiningProcessor.Generation
{
    /// <summary>
    /// Trace of categorial attribute. The class is used for 
    /// computing procedures that deal with categorial attributes. 
    /// It contains bit strings of the categorial attribute
    /// along with attribute cardinality and other info.
    /// </summary>
    public class CategorialAttributeTrace
    {
        #region Fieds

        /// <summary>
        /// Formula (categorial attribute) that identifies the class
        /// </summary>
        private readonly CategorialAttributeFormula _identifier;

        /// <summary>
        /// The bit strings of the categorial attribute. One bit string 
        /// represents bit string of a category of the attribute. There
        /// is a simple cache implemented for optimizing Ice layer calls.
        /// </summary>
        private IBitString[] _bitStrings;

        /// <summary>
        /// If the supporting of numeric values was initialized
        /// (optimizing Ice layer calls)
        /// </summary>
        private bool _supportsNumericValues = false;
        /// <summary>
        /// Iff the attribute supports numeric values (iff the values of
        /// the attribute are ordinal/cardinal
        /// </summary>
        private bool _supportsNumericValuesInitialized = false;

        /// <summary>
        /// Cardinality of the attribute (nominal/ordinal/cyclic ordinal/cardinal)
        /// </summary>
        private CardinalityEnum _attributeCardinality = CardinalityEnum.Nominal;
        /// <summary>
        /// If the cardinality was initialized (optimizing Ice layer calls)
        /// </summary>
        private bool _attributeCardinalityInitialized = false;

        /// <summary>
        /// The associated generator of bit strings. The generator 
        /// returns all the bit strings of all the categories of
        /// an attribute.
        /// </summary>
        private readonly BitStringGeneratorPrx _generator;

        /// <summary>
        /// Number of categories of the categorial attribute
        /// </summary>
        private int _noOfCategories = -1;

        #endregion

        #region Properties

        /// <summary>
        /// Formula (categorial attribute) that identifies the class
        /// </summary>
        public CategorialAttributeFormula Identifier
        {
            get { return _identifier; }
        }

        /// <summary>
        /// Number of categories of the categorial attribute
        /// </summary>
        public int NoOfCategories
        {
            get
            {
                if (_noOfCategories == -1)
                {
                    _noOfCategories = _generator.GetCategoriesIds().Length;
                }
                return _noOfCategories;
            }
        }

        /// <summary>
        /// The bit strings of the categorial attribute. One bit string 
        /// represents bit string of a category of the attribute. There
        /// is a simple cache implemented for optimizing Ice layer calls.
        /// </summary>
        public IBitString[] BitStrings
        {
            get
            {
                if (_bitStrings == null)
                {
                    _bitStrings = Helpers.GetBitStrings(_generator, _identifier.AttributeGuid);
                }
                return _bitStrings;
            }
        }

        /// <summary>
        /// If the supporting of numeric values was initialized
        /// (optimizing Ice layer calls)
        /// </summary>
        public bool SupportsNumericValues
        {
            get
            {
                if (!_supportsNumericValuesInitialized)
                {
                    double[] numericValues = _generator.GetCategoriesNumericValues();
                    _supportsNumericValues = !(numericValues == null || numericValues.Length == 0);
                }
                return _supportsNumericValues;
            }
        }

        /// <summary>
        /// Cardinality of the attribute (nominal/ordinal/cyclic ordinal/cardinal)
        /// </summary>
        public CardinalityEnum AttributeCardinality
        {
            get
            {
                if (!_attributeCardinalityInitialized)
                {
                    _attributeCardinality = _generator.GetAttributeCardinality();
                }
                return _attributeCardinality;
            }
        }

        #endregion

        /// <summary>
        /// Retrieves the bit string for a category determined by an identifier
        /// in the parameter. Uses the bit string cache.
        /// </summary>
        /// <param name="categoryId">Identification of the category</param>
        /// <returns>Bit string of the category</returns>
        public IBitString CategoryBitString(string categoryId)
        {
            IBitStringCache cache = BitStringCache.GetInstance(_generator);
            return cache[Identifier.AttributeGuid, categoryId];
        }

        /// <summary>
        /// Default constructor of the class
        /// </summary>
        /// <param name="generator">The bit string generator of this
        /// categorial attribute</param>
        public CategorialAttributeTrace(BitStringGeneratorPrx generator)
        {
            if (generator == null)
                throw new ArgumentNullException("generator");
            _generator = generator;
            _identifier = new CategorialAttributeFormula(generator.GetAttributeId().value);
        }
    }
}