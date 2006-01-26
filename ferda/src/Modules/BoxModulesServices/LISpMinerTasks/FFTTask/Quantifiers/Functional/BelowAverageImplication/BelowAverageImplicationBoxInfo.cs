using System;
using System.Collections.Generic;

namespace Ferda.Modules.Boxes.LISpMinerTasks.FFTTask.Quantifiers.Functional.BelowAverageImplication
{
	class BelowAverageImplicationBoxInfo : Ferda.Modules.Boxes.LISpMinerTasks.FFTTask.Quantifiers.AbstractFFTTaskQuantifierBoxInfo
	{
		public const string typeIdentifier = 
			"LISpMinerTasks.FFTTask.Quantifiers.Functional.BelowAverageImplication";

		protected override string identifier
		{
			get { return typeIdentifier; }
		}

		public override void CreateFunctions(BoxModuleI boxModule, out Ice.Object iceObject, out IFunctions functions)
		{
			BelowAverageImplicationFunctionsI result = new BelowAverageImplicationFunctionsI();
			iceObject = (Ice.Object)result;
			functions = (IFunctions)result;
		}

		public override string[] GetBoxModuleFunctionsIceIds()
		{
			return BelowAverageImplicationFunctionsI.ids__;
		}
	}
}