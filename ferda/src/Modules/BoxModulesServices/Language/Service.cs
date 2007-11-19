// Service.cs - registering of boxes to the service
//
// Author: Michal Kováč <michal.kovac.develop@centrum.cz>
//
// Copyright (c) 2007 Michal Kováč
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


//#define CONSOLE_APPLICATION_FOR_SERVICE_STATARTUP_DEBUGGING
using Ice;

namespace Ferda.Modules.Boxes.Language
{
    /// <summary>
    /// Represents a IceBox service for common boxes for data mining
    /// </summary>
    public class Service : FerdaServiceI
    {
        /// <summary>
        /// Register box modules to Ice.ObjectAdapter.
        /// </summary>
        /// <remarks>
        /// Remember if you are adding registering of new box module,
        /// you must also change application.xml filePath in config directory.
        /// </remarks>
        protected override void registerBoxes()
        {
            // guha mining setting
            registerBox(Lambda.BoxInfo.typeIdentifier, new Lambda.BoxInfo());
            registerBox(Variable.BoxInfo.typeIdentifier, new Variable.BoxInfo());
            registerBox(Math.BinaryOperation.BoxInfo.typeIdentifier, new Math.BinaryOperation.BoxInfo());
            registerBox(Math.Compare.BoxInfo.typeIdentifier, new Math.Compare.BoxInfo());
            registerBox(Math.IfThenElse.BoxInfo.typeIdentifier, new Math.IfThenElse.BoxInfo());
        }

        /// <summary>
        /// Says that this service has property boxes
        /// </summary>
        protected override bool havePropertyBoxes
        {
            get { return false; }
        }
    }
}
