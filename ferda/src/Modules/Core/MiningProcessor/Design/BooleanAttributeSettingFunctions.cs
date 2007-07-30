// BooleanAttributeSettingsFunctions.cs - Interface for Boolean attribute settings
//
// Authors: Tom� Kucha� <tomas.kuchar@gmail.com>      
// Commented by: Martin Ralbovsk� <martin.ralbovsky@gmail.com>
//
// Copyright (c) 2006 Tom� Kucha�
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

namespace Ferda.Guha.MiningProcessor.Design
{
    /// <summary>
    /// Represents an interface for Boolean attribute seting. It contains
    /// the bit string generator (from the
    /// <see cref="Ferda.Guha.MiningProcessor.Desing.BitStringGeneratorProvider"/>
    /// class and also entity setting.
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
    interface BooleanAttributeSettingFunctions : BitStringGeneratorProvider
    {
        /// <summary>
        /// Get entity setting for the current Boolean attribute. 
        /// That is the entity setting that is represented by the 
        /// current box.
        /// </summary>
        /// <returns>Entity setting</returns>
        IEntitySetting GetEntitySetting();
    }
}
