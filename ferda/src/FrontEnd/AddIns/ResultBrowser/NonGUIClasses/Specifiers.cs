// FerdaResultBrowserControl.cs - File contains classes describing column specifiers
//
// Author:   Martin Ralbovsky <martin.ralbovsky@gmail.com>
//
// Copyright (c) 2006 Martin Ralbovsky
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
using Ferda.Guha.MiningProcessor;

namespace Ferda.FrontEnd.AddIns.ResultBrowser.NonGUIClasses
{
    /// <summary>
    /// Class that holds information about the specifier, wheather the specifier
    /// is selected and the witdh of the column that represents the specifier.
    /// A specifier can be a mark type (in the sense of antecedent, succedent or
    /// condition), quantifier or an attribute.
    /// </summary>
    public class UsedSpecifier
    {
        /// <summary>
        /// Id of the specifier
        /// </summary>
        public int id;
        /// <summary>
        /// If it is selected
        /// </summary>
        public bool Selected;
        /// <summary>
        /// Width of the column representing the specifier
        /// </summary>
        public int width;
    }

    /// <summary>
    /// A specifier that represents the mark type (antecedent, row attributes,
    /// condition...).
    /// </summary>
    public class UsedMark : UsedSpecifier
    {
        /// <summary>
        /// Type of the mark (antecedent, row attributes, condition...)
        /// </summary>
        public MarkEnum MarkType;
        /// <summary>
        /// Name of the column representing the mark
        /// </summary>
        public string ColumnName;
    }

    /// <summary>
    /// A specifier that represents a quantifier
    /// </summary>
    public class UsedQuantifier : UsedSpecifier
    {
        /// <summary>
        /// Label of the quantifier
        /// </summary>
        public string QuantifierLabel;
        /// <summary>
        /// User label (localized) of the quantifier
        /// </summary>
        public string QuantifierUserLabel;
    }
}
