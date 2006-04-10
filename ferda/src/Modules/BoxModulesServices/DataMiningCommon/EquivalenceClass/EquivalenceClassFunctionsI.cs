// EquivalenceClassFunctionsI.cs - functions object for equivalence class box module
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
using Ferda.Modules.Boxes.DataMiningCommon.Attributes;
using Ferda.Modules.Boxes.DataMiningCommon.LiteralSetting;

namespace Ferda.Modules.Boxes.DataMiningCommon.EquivalenceClass
{
	class EquivalenceClassFunctionsI : EquivalenceClassFunctionsDisp_, IFunctions
	{
        /// <summary>
        /// The box module.
        /// </summary>
		protected BoxModuleI boxModule;
		//protected IBoxInfo boxInfo;

		#region IFunctions Members
        /// <summary>
        /// Sets the <see cref="T:Ferda.Modules.BoxModuleI">box module</see>
        /// and the <see cref="T:Ferda.Modules.Boxes.IBoxInfo">box info</see>.
        /// </summary>
        /// <param name="boxModule">The box module.</param>
        /// <param name="boxInfo">The box info.</param>
        public void setBoxModuleInfo(BoxModuleI boxModule, IBoxInfo boxInfo)
		{
			this.boxModule = boxModule;
			//this.boxInfo = boxInfo;
		}
		#endregion

		#region Properties
		#endregion

		#region Functions
		public override EquivalenceClassStruct getEquivalenceClass(Ice.Current __current)
		{
			EquivalenceClassStruct result = new EquivalenceClassStruct();
			List<int> attributesIdentifiers = new List<int>();
			List<int> literalsIdentifiers = new List<int>();

			Ice.ObjectPrx[] functions = this.boxModule.GetFunctions("LiteralSettingOrAttribute");
			if (functions.Length == 0)
			{
				return result;
			}
			AbstractAttributeFunctionsPrx aprx;
			LiteralSettingFunctionsPrx lprx;
			foreach (Ice.ObjectPrx prx in functions)
			{
				if (prx.ice_isA("::Ferda::Modules::Boxes::DataMiningCommon::Attributes::AbstractAttributeFunctions"))
				{
					aprx = AbstractAttributeFunctionsPrxHelper.checkedCast(prx);
					attributesIdentifiers.Add(aprx.getAbstractAttribute().identifier);
				}
				else if (prx.ice_isA("::Ferda::Modules::Boxes::DataMiningCommon::LiteralSetting::LiteralSettingFunctions"))
				{
					lprx = LiteralSettingFunctionsPrxHelper.checkedCast(prx);
					literalsIdentifiers.Add(lprx.getLiteralSetting().identifier);
				}
				else
					throw new Exception("Bad proxy type!");
			}
			result.attributesIdentifiers = attributesIdentifiers.ToArray();
			result.literalsIdentifiers = literalsIdentifiers.ToArray();
            result.identifier = boxModule.PersistentIdentity;
			return result;
		}
		#endregion

		#region Sockets
		#endregion

		#region Actions
		#endregion

		#region BoxInfo
		#endregion
	}
}