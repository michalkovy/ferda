// CoefficientSetting.cs - Generation of coefficient bit strings
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

namespace Ferda.Guha.MiningProcessor.Design
{
    /// <summary>
    /// The class is used for generation of bit strings from an attribute
    /// defined by a coefficient setting. Coefficient allows to create 
    /// specific and meaningful subsets of an attribute, for their types
    /// see <see cref="Ferda.Guha.MiningProcessor.Design.CoefficientTypeEnum"/>.
    /// This is used in the <c>Atom setting</c> box module. 
    /// </summary>
    /// <remarks>
    /// The original slice design can be found in 
    /// <c>slice/Modules/Guha.MiningProcessor.ice</c>.
    /// The class diagram representing dependencies of the designed entities
    /// can be found in
    /// <c>slice/Modules/GUha.MiningProcessor.png(csd)</c>.
    /// The csd file stands for class diagram, that can be edited with the 
    /// <c>NClass</c> tool, see <see cref="http://nclass.sourceforge.net"/>.
    /// </remarks>
    public class CoefficientSetting : ILeafEntitySetting
    {
        /// <summary>
        /// Minimal length of the coefficient type
        /// </summary>
        public int MinimalLength
        {
            get { throw new Exception("The method or operation is not implemented."); }
            set { throw new Exception("The method or operation is not implemented."); }
        }

        /// <summary>
        /// Maximal length of the coefficient type
        /// </summary>
        public int MaximalLength
        {
            get { throw new Exception("The method or operation is not implemented."); }
            set { throw new Exception("The method or operation is not implemented."); }
        }

        /// <summary>
        /// The coefficient type
        /// </summary>
        public CoefficientType CoefficientType
        {
            get { throw new NotImplementedException(); }
            set
            {
            }
        }
    }
}