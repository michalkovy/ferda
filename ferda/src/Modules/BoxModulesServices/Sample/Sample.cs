// Saple.cs - Sample IceBox service
//
// Author: Tomáš Kuchař <tomas.kuchar@gmail.com>
//
// Copyright (c) 2005 Tomáš Kuchař
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

namespace Ferda.Modules.Boxes.Sample
{
    /// <summary>
    /// Sample IceBox service with BodyMassIndex box 
    /// </summary>
    public class Service : Ferda.Modules.FerdaServiceI
    {
        /// <summary>
        /// Registers box to ice object adapter
        /// </summary>
        protected override void registerBoxes()
        {
            this.registerBox("BodyMassIndexSampleFactoryCreator", new Sample.BodyMassIndex.BodyMassIndexBoxInfo());
            // if more boxes should be provided by this service, register them here ...
        }
    }
}
