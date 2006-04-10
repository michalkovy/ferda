// LiteralSettingFunctionsI.cs - functions for literal setting box module
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
using Ferda.Modules.Boxes.DataMiningCommon.AtomSetting;

namespace Ferda.Modules.Boxes.DataMiningCommon.LiteralSetting
{
	class LiteralSettingFunctionsI : LiteralSettingFunctionsDisp_, IFunctions
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
		protected GaceTypeEnum GaceType
		{
			get
			{
				return (GaceTypeEnum)Enum.Parse(
					typeof(GaceTypeEnum),
					this.boxModule.GetPropertyString("GaceType"),
					true);
			}
		}

		protected LiteralTypeEnum LiteralType
		{
			get
			{
			return (LiteralTypeEnum)Enum.Parse(
				typeof(LiteralTypeEnum),
				this.boxModule.GetPropertyString("LiteralType"),
				true);
			}
		}
		#endregion

		#region Functions
		public override LiteralSettingStruct getLiteralSetting(Ice.Current __current)
		{
			LiteralSettingStruct result = new LiteralSettingStruct();
			result.atomSetting = this.getAtomSettingFunctionsPrx().getAtomSetting();
            result.identifier = boxModule.PersistentIdentity;
			result.gaceType = GaceType;
			result.literalType = LiteralType;
			return result;
		}
		#endregion

		#region Sockets
		protected AtomSettingFunctionsPrx getAtomSettingFunctionsPrx()
		{
			return AtomSettingFunctionsPrxHelper.checkedCast(
				SocketConnections.GetObjectPrx(boxModule, "AtomSetting")
			);
		}
		#endregion

		#region Actions
		#endregion

		#region BoxInfo
		public AtomSetting.AtomSettingStruct GetAtomSetting()
		{
			try
			{
				return getAtomSettingFunctionsPrx().getAtomSetting();

			}
            catch (Ferda.Modules.BoxRuntimeError) { }
            //Ferda.Modules.BadValueError ... Number of categories is 0
			return new AtomSettingStruct();
		}
		#endregion
	}
}