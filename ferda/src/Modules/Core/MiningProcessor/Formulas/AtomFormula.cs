// AtomFormula.cs - Formula for atoms (basic Boolean attributes)
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
using System.Collections.Generic;
using Ferda.Guha.MiningProcessor.BitStrings;

namespace Ferda.Guha.MiningProcessor.Formulas
{
    /// <summary>
    /// The atom formula. Atom is a basic Boolean attribute. There are
    /// coefficients defined in the atom. 
    /// </summary>
    [Serializable()]
    public class AtomFormula : BooleanAttributeFormula
    {
        /// <summary>
        /// The bit string that is associated with this atom formula
        /// </summary>
        private readonly BitStringIdentifier _bitStringIdentifier;

        /// <summary>
        /// The bit string that is associated with this atom formula
        /// </summary>
        public BitStringIdentifier BitStringIdentifier
        {
            get { return _bitStringIdentifier; }
        }

        /// <summary>
        /// Default contructor of the class
        /// </summary>
        /// <param name="bitStringIdentifier">Bit string associated
        /// with this formula.</param>
        public AtomFormula(BitStringIdentifier bitStringIdentifier)
        {
            _bitStringIdentifier = bitStringIdentifier;
        }

        /// <summary>
        /// Converts the instance of the class to string. 
        /// </summary>
        /// <returns>String representation of the class</returns>
        public override string ToString()
        {
            if (_bitStringIdentifier == EmptyBitString.EmptyBitStringIdentifier)
                return "";
            return WriteAtom(
                _bitStringIdentifier.AttributeGuid,
                new string[] {_bitStringIdentifier.CategoryId}
                );
        }

        /// <summary>
        /// Function returns a text representation of the atom. 
        /// </summary>
        /// <param name="attributeGuid">Identification of the attribute of the atom</param>
        /// <param name="categories">Categories of the atom</param>
        /// <returns>Text representation of the atom.</returns>
        public static string WriteAtom(string attributeGuid, IEnumerable<string> categories)
        {
            return AttributeNameInLiteralsProvider.GetAttributeNameInLiterals(attributeGuid)
                   + "("
                   + FormulaHelper.SequenceToString(
                         categories,
                         FormulaSeparator.AtomMembers,
                         false
                         )
                   + ")";
        }
    }
}