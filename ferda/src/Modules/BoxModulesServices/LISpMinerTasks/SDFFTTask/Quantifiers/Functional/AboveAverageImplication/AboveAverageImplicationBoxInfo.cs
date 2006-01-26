using System;
using System.Collections.Generic;

namespace Ferda.Modules.Boxes.LISpMinerTasks.SDFFTTask.Quantifiers.Functional.AboveAverageImplication
{
	class AboveAverageImplicationBoxInfo : Ferda.Modules.Boxes.LISpMinerTasks.SDFFTTask.Quantifiers.AbstractSDFFTTaskQuantifierBoxInfo
	{
		public const string typeIdentifier = 
			"LISpMinerTasks.SDFFTTask.Quantifiers.Functional.AboveAverageImplication";

		protected override string identifier
		{
			get { return typeIdentifier; }
		}

		public override void CreateFunctions(BoxModuleI boxModule, out Ice.Object iceObject, out IFunctions functions)
		{
			AboveAverageImplicationFunctionsI result = new AboveAverageImplicationFunctionsI();
			iceObject = (Ice.Object)result;
			functions = (IFunctions)result;
		}

		public override string[] GetBoxModuleFunctionsIceIds()
		{
			return AboveAverageImplicationFunctionsI.ids__;
		}
	}
}