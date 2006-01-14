using System;
using System.Collections.Generic;
using Ferda.Modules.Boxes.DataMiningCommon.AtomSetting;

namespace Ferda.Modules.Boxes.DataMiningCommon.LiteralSetting
{
	class LiteralSettingFunctionsI : LiteralSettingFunctionsDisp_, IFunctions
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