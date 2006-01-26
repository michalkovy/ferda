using System;
using System.Collections.Generic;

namespace Ferda.Modules.Boxes.LISpMinerTasks.FFTTask.Quantifiers.Functional.E
{
	class EBoxInfo : Ferda.Modules.Boxes.LISpMinerTasks.FFTTask.Quantifiers.AbstractFFTTaskQuantifierBoxInfo
	{
		public const string typeIdentifier = 
			"LISpMinerTasks.FFTTask.Quantifiers.Functional.E";

		protected override string identifier
		{
			get { return typeIdentifier; }
		}

		public override void CreateFunctions(BoxModuleI boxModule, out Ice.Object iceObject, out IFunctions functions)
		{
			EFunctionsI result = new EFunctionsI();
			iceObject = (Ice.Object)result;
			functions = (IFunctions)result;
		}

		public override string[] GetBoxModuleFunctionsIceIds()
		{
			return EFunctionsI.ids__;
		}
	}
}