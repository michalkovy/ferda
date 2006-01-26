using System;
using System.Collections.Generic;

namespace Ferda.Modules.Boxes.LISpMinerTasks.FFTTask.Quantifiers.Functional.Fisher
{
	class FisherBoxInfo : Ferda.Modules.Boxes.LISpMinerTasks.FFTTask.Quantifiers.AbstractFFTTaskQuantifierBoxInfo
	{
		public const string typeIdentifier = 
			"LISpMinerTasks.FFTTask.Quantifiers.Functional.Fisher";

		protected override string identifier
		{
			get { return typeIdentifier; }
		}

		public override void CreateFunctions(BoxModuleI boxModule, out Ice.Object iceObject, out IFunctions functions)
		{
			FisherFunctionsI result = new FisherFunctionsI();
			iceObject = (Ice.Object)result;
			functions = (IFunctions)result;
		}

		public override string[] GetBoxModuleFunctionsIceIds()
		{
			return FisherFunctionsI.ids__;
		}
	}
}