using System;
using System.Collections.Generic;
using System.Text;

namespace Ferda.Modules.Boxes.LISpMinerTasks.FFTTask
{
	class FFTTaskBoxInfo : LISpMinerAbstractTaskBoxInfo
	{
		public const string typeIdentifier =
            "LISpMinerTasks.FFTTask";

		protected override string identifier
		{
			get { return typeIdentifier; }
		}

		public override void CreateFunctions(BoxModuleI boxModule, out Ice.Object iceObject, out IFunctions functions)
		{
			FFTTaskFunctionsI result = new FFTTaskFunctionsI();
			iceObject = (Ice.Object)result;
			functions = (IFunctions)result;
		}

		public override string[] GetBoxModuleFunctionsIceIds()
		{
			return FFTTaskFunctionsI.ids__;
		}
	}
}