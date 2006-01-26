using System;
using System.Collections.Generic;
using System.Text;

namespace Ferda.Modules.Boxes.LISpMinerTasks.SDKLTask
{
	class SDKLTaskBoxInfo : LISpMinerAbstractTaskBoxInfo
	{
		public const string typeIdentifier =
            "LISpMinerTasks.SDKLTask";

		protected override string identifier
		{
			get { return typeIdentifier; }
		}

		public override void CreateFunctions(BoxModuleI boxModule, out Ice.Object iceObject, out IFunctions functions)
		{
			SDKLTaskFunctionsI result = new SDKLTaskFunctionsI();
			iceObject = (Ice.Object)result;
			functions = (IFunctions)result;
		}

		public override string[] GetBoxModuleFunctionsIceIds()
		{
			return SDKLTaskFunctionsI.ids__;
		}
	}
}