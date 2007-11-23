// NodeClassification.cs - structure for classification of a node
// category
//
// Authors: Martin Ralbovský <martin.ralbovsky@gmail.com>
//
// Copyright (c) 2007 Martin Ralbovský 
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

namespace Ferda.Guha.MiningProcessor.DecisionTrees
{
    /// <summary>
    /// Structure for classification of a node category. It does not classify
    /// the node as a whole, but its individual categories.
    /// </summary>
    public class NodeClassification 
    {
        /// <summary>
        /// This bit string shows, how does this (normal) category
        /// classify the classification category. 1 in the bit
        /// string means, that the classification says that the
        /// item is has the classification category and 0 says
        /// that it has not. 
        /// </summary>
        public IBitString classificationBitString;

        /// <summary>
        /// Classification category - which classification category belongs
        /// to this (normal) category
        /// </summary>
        public string classificationCategory = string.Empty;

        /// <summary>
        /// Number of items in the classified category. It is equal to length
        /// of the category bit string. Here is serves as output for IF representation
        /// </summary>
        public long noItemsInCategory = -1;

        /// <summary>
        /// Number of classification errors - the bit strings that have been 
        /// not classified right.
        /// </summary>
        public long noErrors = -1;
    }
}
