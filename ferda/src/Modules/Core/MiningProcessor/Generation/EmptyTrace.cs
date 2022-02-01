// EmptyTrace.cs - empty bit string enumerator
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
using System.Collections;
using System.Collections.Generic;
using Ferda.Guha.MiningProcessor.BitStrings;
using Ferda.Modules.Helpers.Common;

namespace Ferda.Guha.MiningProcessor.Generation
{
    /// <summary>
    /// Represents a empty bit string enumerator
    /// </summary>
    public class EmptyTrace : IEntityEnumerator
    {
        #region IEntityEnumerator Members

        /// <summary>
        /// Total number of bit strings in this enumerator
        /// </summary>
        public long TotalCount
        {
            get { return 1; }
        }

        /// <summary>
        /// Cedent type of the enumerator
        /// </summary>
        public MarkEnum CedentType
        {
            get { throw new NotImplementedException("This should be never invoked."); }
        }

        #endregion

        #region IEnumerable<IBitString> Members

        /// <summary>
        /// Gets the enumerator. Always returns an empty bit string
        /// </summary>
        /// <returns>Empty bit string</returns>
        public IEnumerator<IBitString> GetEnumerator()
        {
            yield return EmptyBitString.GetInstance();
        }

        #endregion

        #region IEnumerable Members

        /// <summary>
        /// Gets the enumerator
        /// </summary>
        /// <returns>The enumerator</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Set of used attributes by enumerator
        /// </summary>
        public Set<string> UsedAttributes
        {
            get { return new Set<string>(); }
        }

        /// <summary>
        /// Set of used entities by enumerator
        /// </summary>
        public Set<string> UsedEntities
        {
            get { return new Set<string>(); }
        }

        #endregion
    }
}