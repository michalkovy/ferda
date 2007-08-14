// tablesOfBitStrings.cs - possible 4FT contingency tables
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
using System.Text;
using Ferda.Guha.MiningProcessor.BitStrings;

namespace Ferda.Guha.MiningProcessor.Miners
{
    /// <summary>
    /// Nine-fold table of bit strings (used for the 4FT procedure).
    /// In addition to the four-fold table, this table contains
    /// missing information frequencies. The A and B fields
    /// can be any cedents (in the 4FT procedure, they are used
    /// as condition and antecedent).
    /// </summary>
    internal class nineFoldTableOfBitStrings
    {
        /// <summary>
        /// Bit string for positive A AND positive B
        /// </summary>
        public IBitString pApB;
        /// <summary>
        /// Bit string for positive A AND missing B
        /// </summary>
        public IBitString pAxB;
        /// <summary>
        /// Bit string for positive A AND negative B
        /// </summary>
        public IBitString pAnB;

        /// <summary>
        /// Bit string for missing A AND positive B
        /// </summary>
        public IBitString xApB;
        /// <summary>
        /// Bit string for missing A AND missing B
        /// </summary>
        public IBitString xAxB;
        /// <summary>
        /// Bit string for missing A AND negative B
        /// </summary>
        public IBitString xAnB;

        /// <summary>
        /// Bit string for negative A AND positive B
        /// </summary>
        public IBitString nApB;
        /// <summary>
        /// Bit string for negative A AND missing B
        /// </summary>
        public IBitString nAxB;
        /// <summary>
        /// Bit string for negative A AND negative B
        /// </summary>
        public IBitString nAnB;
    }

    /// <summary>
    /// Four-fold table of bit strings (used for the 4FT procedure)
    /// </summary>
    internal class fourFoldTableOfBitStrings
    {
        /// <summary>
        /// Bit string for positive succedent AND positive antecedent
        /// </summary>
        public IBitString pSpA;
        /// <summary>
        /// Bit string for positive succedent AND negative antecedent
        /// </summary>
        public IBitString pSnA;

        /// <summary>
        /// Bit string for negative succedent AND positive antecedent
        /// </summary>
        public IBitString nSpA;
        /// <summary>
        /// Bit string for negative succedent ANd negative antecedent
        /// </summary>
        public IBitString nSnA;
    }
}
