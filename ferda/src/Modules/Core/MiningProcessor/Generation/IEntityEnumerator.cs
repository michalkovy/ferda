// IEntityEnumerator.cs - iterates through bit string provider
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
using Ferda.Modules.Helpers.Common;

namespace Ferda.Guha.MiningProcessor.Generation
{
    /// <summary>
    /// <para>
    /// The class represents basic interface for an entity enumerator.
    /// What is it and how does it work? The 4FT procedure for example
    /// iterates through all possible conditions, antecedents and succedents and
    /// then computes the 4ft-table and quantifiers (+ some other stuff)
    /// on the bit strings of antecedent and succedent. The possible antecedent
    /// and succedent (their bit strings) are generated from antecedent and
    /// succedent setting, which is basically a Boolean attribute setting.
    /// The generation is implemented by an enumerator. This enumerator
    /// implements this interface :)
    /// </para>
    /// <para>
    /// Please note that no
    /// inheritor of this interface can iterates through 
    /// missing information IBitString i.e. the BitString of 
    /// missing information can not get to output!
    /// </para>
    /// </summary>
    public interface IEntityEnumerator : IEnumerable<IBitString>
    {
        /// <summary>
        /// Total number of bit strings in this enumerator
        /// </summary>
        long TotalCount { get;}
        /// <summary>
        /// Set of used attributes by enumerator
        /// </summary>
        Set<string> UsedAttributes { get; }
        /// <summary>
        /// Cedent type of the enumerator
        /// </summary>
        MarkEnum CedentType { get;}
    }
}