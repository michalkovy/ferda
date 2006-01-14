using System;
using System.Collections.Generic;
using Ferda.Modules.Boxes.DataMiningCommon.Attributes;

namespace Ferda.Modules.Boxes.DataMiningCommon.CategorialPartialCedentSetting
{
	class CategorialPartialCedentSettingFunctionsI : CategorialPartialCedentSettingFunctionsDisp_, IFunctions
	{
		protected BoxModuleI boxModule;
		//protected IBoxInfo boxInfo;

		#region IFunctions Members
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
		public override CategorialPartialCedentSettingStruct getCategorialPartialCedentSetting(Ice.Current __current)
		{
			CategorialPartialCedentSettingStruct result = new CategorialPartialCedentSettingStruct();
			result.attributes = getAbstractAttributeStructSeq();
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
		protected AbstractAttributeStruct[] getAbstractAttributeStructSeq()
		{
			List<AbstractAttributeStruct> result = new List<AbstractAttributeStruct>();
			Ice.ObjectPrx[] prxs = SocketConnections.GetObjectPrxs(boxModule, "Attribute", true);
			foreach (Ice.ObjectPrx prx in prxs)
			{
				result.Add(AbstractAttributeFunctionsPrxHelper.checkedCast(prx).getAbstractAttribute());
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