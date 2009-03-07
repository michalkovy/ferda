// Enumeration.cs - fuzzy set of the trapezoidal shape
//
// Author: Martin Ralbovský <martin.ralbovsky@gmail.com>
//
// Copyright (c) 2009 Martin Ralbovský
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

namespace Ferda.Guha.Attribute
{
    /// <summary>
    /// A simplified representation of a trapezoidal fuzzy set. Only 4 points
    /// and a name of the fuzzy set is stored
    /// </summary>
    public class TrapezoidalFuzzySet
    {
        /// <summary>
        /// Name of the fuzzy set
        /// </summary>
        public string Name;

        /// <summary>
        /// A (start of the trapezoid)
        /// </summary>
        public double A;

        /// <summary>
        /// B (end of the trapezoid)
        /// </summary>
        public double B;

        /// <summary>
        /// C (descending peak)
        /// </summary>
        public double C;

        /// <summary>
        /// D (ascending peak)
        /// </summary>
        public double D;
    }
}
