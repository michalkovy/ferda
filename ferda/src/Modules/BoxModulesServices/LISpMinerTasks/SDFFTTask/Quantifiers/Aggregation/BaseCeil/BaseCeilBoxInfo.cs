using System;
using System.Collections.Generic;

namespace Ferda.Modules.Boxes.SDFFTTask.Quantifiers.Aggregation.BaseCeil
{
	class BaseCeilBoxInfo : Ferda.Modules.Boxes.SDFFTTask.Quantifiers.AbstractSDFFTTaskQuantifierBoxInfo
	{
		public const string typeIdentifier = 
			"LISpMinerTasks.SDFFTTask.Quantifiers.Aggregation.BaseCeil";

		protected override string identifier
		{
			get { return typeIdentifier; }
		}

		public override void CreateFunctions(BoxModuleI boxModule, out Ice.Object iceObject, out IFunctions functions)
		{
			BaseCeilFunctionsI result = new BaseCeilFunctionsI();
			iceObject = (Ice.Object)result;
			functions = (IFunctions)result;
		}

		public override string[] GetBoxModuleFunctionsIceIds()
		{
			return BaseCeilFunctionsI.ids__;
		}
	}
}