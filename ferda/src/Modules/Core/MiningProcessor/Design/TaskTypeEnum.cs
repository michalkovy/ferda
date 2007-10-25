// TaskTypeEnum.cs - defines type of the GUHA task
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
    /// Defines type of the GUHA task (procedure).
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
    public enum TaskTypeEnum
    {
        /// <summary>
        /// 4FT Procedure
        /// </summary>
        FourFold,
        /// <summary>
        /// KL Procedure
        /// </summary>
        KL,
        /// <summary>
        /// CF Procedure
        /// </summary>
        CF,
        /// <summary>
        /// SD4FT Procedure
        /// </summary>
        SDFourFold,
        /// <summary>
        /// SDKL Procedure
        /// </summary>
        SDKL,
        /// <summary>
        /// SDCF Procedure
        /// </summary>
        SDCF,
        /// <summary>
        /// ETree Procedure
        /// </summary>
        ETree
    }
}
