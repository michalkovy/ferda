using System;
using System.Collections.Generic;
using Ferda.Modules.Boxes.DataMiningCommon.LiteralSetting;
using Ferda.Modules.Boxes.DataMiningCommon.EquivalenceClass;

namespace Ferda.Modules.Boxes.DataMiningCommon.BooleanPartialCedentSetting
{
	class BooleanPartialCedentSettingFunctionsI : BooleanPartialCedentSettingFunctionsDisp_, IFunctions
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