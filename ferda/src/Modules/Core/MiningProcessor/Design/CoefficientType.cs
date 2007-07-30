// CoefficientType.cs - Types of coefficient
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


namespace Ferda.Guha.MiningProcessor.Design
{
    /// <summary>
    /// Enumerates available types of coefficients.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The order of defined types is important, because it is used for sorting
    /// and merging of coefficients. More general coefficients are placed first.
    /// </para>
    /// <para>
    /// The original slice design can be found in 
    /// <c>slice/Modules/Guha.MiningProcessor.ice</c>.
    /// The class diagram representing dependencies of the designed entities
    /// can be found in
    /// <c>slice/Modules/GUha.MiningProcessor.png(csd)</c>.
    /// The csd file stands for class diagram, that can be edited with the 
    /// <c>NClass</c> tool, see <see cref="http://nclass.sourceforge.net"/>.
    /// </para>
    /// </remarks>
    public enum CoefficientTypeEnum
    {
        /// <summary>
        /// Coefficient is constructed as an arbitrary subset of categories.
        /// Can be used for both ordered and unordered categories.
        /// </summary>
        Subsets,

        /// <summary>
        /// Coefficient is constructed as a cyclic interval from ordered categories.
        /// Useful for days of a week or months in a year.
        /// Cannot be used for unordered categories.
        /// </summary>
        CyclicIntervals,

        /// <summary>
        /// Coefficient is constructed as an interval from ordered categories.
        /// Cannot be used for unordered categories.
        /// </summary>
        Intervals,

        /// <summary>
        /// Coefficient is constructed as left or right cuts from ordered categories.
        /// Cuts are intervals that include either minimum value or maximum value.
        /// Cannot be used for unordered categories.
        /// </summary>
        Cuts,

        /// <summary>
        /// Coefficient is constructed as left cuts from ordered categories.
        /// Left cuts are intervals that include minimum value.
        /// Cannot be used for unordered categories.
        /// </summary>
        LeftCuts,

        /// <summary>
        /// Coefficient is constructed as right cuts from ordered categories.
        /// Right cuts are intervals that include maximum value.
        /// Cannot be used for unordered categories.
        /// </summary>
        RightCuts,
    }
}