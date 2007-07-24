// BitStringIdentifier.cs - Identifier of bit strings
//
// Author: Tomáš Kuchaø <tomas.kuchar@gmail.com>
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

namespace Ferda.Guha.MiningProcessor.BitStrings
{
    /// <summary>
    /// Identifier of a bit string. It consist of name of the attribute
    /// and also name of the category in the attribute. 
    /// </summary>
    [Serializable()]
    public class BitStringIdentifier : IEquatable<BitStringIdentifier>
    {
        /// <summary>
        /// Identifier of the attribute
        /// </summary>
        private readonly string _attributeGuid;
        
        /// <summary>
        /// Identifier of the attribute
        /// </summary>
        public string AttributeGuid
        {
            get { return _attributeGuid; }
        }

        /// <summary>
        /// Identifier of the category
        /// </summary>
        private readonly string _categoryId;

        /// <summary>
        /// Identifier of the category
        /// </summary>
        public string CategoryId
        {
            get { return _categoryId; }
        }

        /// <summary>
        /// Default constructor for the class
        /// </summary>
        /// <param name="attributeGuid">Identifier of the attribute</param>
        /// <param name="categoryId">Identifier of the category</param>
        public BitStringIdentifier(string attributeGuid, string categoryId)
        {
            _attributeGuid = attributeGuid;
            _categoryId = categoryId;
        }

        /// <summary>
        /// Gets hash code of the bit string identifier
        /// </summary>
        /// <returns>Hash code of the bit string identifier</returns>
        public override int GetHashCode()
        {
            return _attributeGuid.GetHashCode() ^ _categoryId.GetHashCode();
        }

        /// <summary>
        /// Determines if two bit string identifiers are the same
        /// </summary>
        /// <param name="obj">Other bit string identifier</param>
        /// <returns>Iff the two identifiers are equal</returns>
        public override bool Equals(object obj)
        {
            return Equals(obj as BitStringIdentifier);
        }
        
        /// <summary>
        /// Operator == for two bit string identifiers. Two bit string
        /// identifiers are equal iff they are from the same attribute
        /// and have the same category.
        /// </summary>
        /// <param name="op1">First operand</param>
        /// <param name="op2">Second operand</param>
        /// <returns>Iff the two operands are equal to each other</returns>
        public static bool operator ==(BitStringIdentifier op1, BitStringIdentifier op2)
        {
            if (ReferenceEquals(op1, op2))
                return true;
            else if (ReferenceEquals(op1, null) || ReferenceEquals(op2, null))
                return false;
            else
                return op1.Equals(op2);
        }

        /// <summary>
        /// Operator != for two bit string identifiers. Two bit string
        /// identifiers are equal iff they are from the same attribute
        /// and have the same category.
        /// </summary>
        /// <param name="op1">First operand</param>
        /// <param name="op2">Second operand</param>
        /// <returns>Iff the two operands are not equal to each other</returns>
        public static bool operator !=(BitStringIdentifier op1, BitStringIdentifier op2)
        {
            return !(op1 == op2);
        }

        #region IEquatable<BitStringIdentifier> Members

        /// <summary>
        /// Determines if two bit string identifiers are the same
        /// </summary>
        /// <param name="obj">Other bit string identifier</param>
        /// <returns>Iff the two identifiers are equal</returns>
        public bool Equals(BitStringIdentifier other)
        {
            if (other == null)
                return false;
            else
                return (_attributeGuid.Equals(other._attributeGuid)
                        && _categoryId.Equals(other._categoryId));
        }

        #endregion
    }
}