using System;
using System.Collections.Generic;
using System.Text;

namespace Ferda.Modules.Boxes.LISpMinerTasks.SDFFTTask
{
	class SDFFTTaskBoxInfo : LISpMinerAbstractTaskBoxInfo
	{
		public const string typeIdentifier = 
			"LISpMinerTasks.SDFFTTask";

		protected override string identifier
		{
			get { return typeIdentifier; }
		}

		public override void CreateFunctions(BoxModuleI boxModule, out Ice.Object iceObject, out IFunctions functions)
		{
			SDFFTTaskFunctionsI result = new SDFFTTaskFunctionsI();
			iceObject = (Ice.Object)result;
			functions = (IFunctions)result;
		}

		public override string[] GetBoxModuleFunctionsIceIds()
		{
			return SDFFTTaskFunctionsI.ids__;
		}
	}
}