using System;
using System.Collections.Generic;

namespace Ferda.Modules.Boxes.FFTTask.Quantifiers.Functional.FoundedImplication
{
	class FoundedImplicationBoxInfo : Ferda.Modules.Boxes.FFTTask.Quantifiers.AbstractFFTTaskQuantifierBoxInfo
	{
		public const string typeIdentifier = 
			"LISpMinerTasks.FFTTask.Quantifiers.Functional.FoundedImplication";

		protected override string identifier
		{
			get { return typeIdentifier; }
		}

		public override void CreateFunctions(BoxModuleI boxModule, out Ice.Object iceObject, out IFunctions functions)
		{
			FoundedImplicationFunctionsI result = new FoundedImplicationFunctionsI();
			iceObject = (Ice.Object)result;
			functions = (IFunctions)result;
		}

		public override string[] GetBoxModuleFunctionsIceIds()
		{
			return FoundedImplicationFunctionsI.ids__;
		}
	}
}