// BooleanPartialCedentSettingFunctionsI.cs - functions object for boolean partial cedent setting box module
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
using Ferda.Modules.Boxes.DataMiningCommon.LiteralSetting;
using Ferda.Modules.Boxes.DataMiningCommon.EquivalenceClass;

namespace Ferda.Modules.Boxes.DataMiningCommon.BooleanPartialCedentSetting
{
	class BooleanPartialCedentSettingFunctionsI : BooleanPartialCedentSettingFunctionsDisp_, IFunctions
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
		protected long MinLen
		{
			get
			{
				return this.boxModule.GetPropertyLong("MinLen");
			}
		}

		protected long MaxLen
		{
			get
			{
				return this.boxModule.GetPropertyLong("MaxLen");
			}
		}
		#endregion

		#region Functions
		public override BooleanPartialCedentSettingStruct getBooleanPartialCedentSetting(Ice.Current __current)
		{
			BooleanPartialCedentSettingStruct result = new BooleanPartialCedentSettingStruct();
			result.equivalenceClasses = getEquivalenceClassStructSeq();
			result.literalSettings = getLiteralSettingStructSeq();
			result.minLen = MinLen;
			result.maxLen = MaxLen;
            result.identifier = boxModule.PersistentIdentity;
			if (result.minLen > result.maxLen)
			{
                throw Ferda.Modules.Exceptions.BadValueError(null, boxModule.StringIceIdentity, "Min length is greater than max length!", new string[] { "MinLen", "MaxLen" }, restrictionTypeEnum.Other);
			}
			return result;
		}
		#endregion

		#region Sockets
		protected LiteralSettingStruct[] getLiteralSettingStructSeq()
		{
			List<LiteralSettingStruct> result = new List<LiteralSettingStruct>();
			Ice.ObjectPrx[] prxs = SocketConnections.GetObjectPrxs(boxModule, "LiteralSetting", true);
			foreach (Ice.ObjectPrx prx in prxs)
			{
				result.Add(LiteralSettingFunctionsPrxHelper.checkedCast(prx).getLiteralSetting());
			}
			return result.ToArray();
		}

		protected EquivalenceClassStruct[] getEquivalenceClassStructSeq()
		{
			List<EquivalenceClassStruct> result = new List<EquivalenceClassStruct>();
			Ice.ObjectPrx[] prxs = SocketConnections.GetObjectPrxs(boxModule, "EquivalenceClass", false);
			foreach (Ice.ObjectPrx prx in prxs)
			{
				result.Add(EquivalenceClassFunctionsPrxHelper.checkedCast(prx).getEquivalenceClass());
			}
			return result.ToArray();
		}
		#endregion

		#region Actions
		#endregion

		#region BoxInfo
		#endregion
	}
}