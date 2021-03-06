// BooleanAttributeFormula.cs - Formula for Boolean attributes
//
// Authors: Tom?? Kucha? <tomas.kuchar@gmail.com>      
// Commented by: Martin Ralbovsk? <martin.ralbovsky@gmail.com>
//
// Copyright (c) 2006 Tom?? Kucha?
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

using Ferda.Modules.Helpers.Common;

namespace Ferda.Guha.MiningProcessor.Formulas
{
    /// <summary>
    /// The class represents a Boolean attribute formula. There are also
    /// derived specific formulas. Boolean attributes is used in the 4FT
    /// procedure for antecedents and succedents.
    /// </summary>
    public abstract class BooleanAttributeFormula : Formula
    {
        public abstract Set<string> UsedAttributes
        {
            get;
        }
    }
}