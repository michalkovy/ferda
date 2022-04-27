// Functions.cs - Function objects for the wizard box module
//
// Author: Daniel Kupka <kupkd9am@post.cz>
//
// Copyright (c) 2007 Daniel Kupka
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

namespace Ferda.Modules.Boxes.Wizards.WizardForm
{
    /// <summary>
    /// Class is providing ICE functionality of the WizardForm
    /// box module
    /// </summary>
	class Functions : WizardFormFunctionsDisp_, Ferda.Modules.IFunctions
    {
        #region ICE functions

        public override string HelloWorld(Ice.Current __current)
		{
			return "Hello World!";
        }

        #endregion

        protected Ferda.Modules.BoxModuleI boxModule;
		protected Ferda.Modules.Boxes.IBoxInfo boxInfo;

		#region IFunctions Members

        /// <summary>
        /// Sets the <see cref="T:Ferda.Modules.BoxModuleI">box module</see>
        /// and the <see cref="T:Ferda.Modules.Boxes.IBoxInfo">box info</see>.
        /// </summary>
        /// <param name="boxModule">The box module.</param>
        /// <param name="boxInfo">The box info.</param>
		void Ferda.Modules.IFunctions.setBoxModuleInfo(Ferda.Modules.BoxModuleI boxModule, Ferda.Modules.Boxes.IBoxInfo boxInfo)
		{
			this.boxModule = boxModule;
			this.boxInfo = boxInfo;
		}

		#endregion
	}
}
