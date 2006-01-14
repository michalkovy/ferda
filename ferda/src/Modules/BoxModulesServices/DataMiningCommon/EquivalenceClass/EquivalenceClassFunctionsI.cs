using System;
using System.Collections.Generic;
using Ferda.Modules.Boxes.DataMiningCommon.Attributes;
using Ferda.Modules.Boxes.DataMiningCommon.LiteralSetting;

namespace Ferda.Modules.Boxes.DataMiningCommon.EquivalenceClass
{
	class EquivalenceClassFunctionsI : EquivalenceClassFunctionsDisp_, IFunctions
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