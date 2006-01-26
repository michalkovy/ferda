using System;
using System.Collections.Generic;

namespace Ferda.Modules.Boxes.LISpMinerTasks.FFTTask.Quantifiers.Functional.BaseCeil
{
	class BaseCeilBoxInfo : Ferda.Modules.Boxes.LISpMinerTasks.FFTTask.Quantifiers.AbstractFFTTaskQuantifierBoxInfo
	{
		public const string typeIdentifier = "LISpMinerTasks.FFTTask.Quantifiers.Functional.BaseCeil";

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