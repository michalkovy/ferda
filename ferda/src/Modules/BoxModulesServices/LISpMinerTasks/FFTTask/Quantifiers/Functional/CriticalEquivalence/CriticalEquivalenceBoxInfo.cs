using System;
using System.Collections.Generic;

namespace Ferda.Modules.Boxes.LISpMinerTasks.FFTTask.Quantifiers.Functional.CriticalEquivalence
{
	class CriticalEquivalenceBoxInfo : Ferda.Modules.Boxes.LISpMinerTasks.FFTTask.Quantifiers.AbstractFFTTaskQuantifierBoxInfo
	{
		public const string typeIdentifier = 
			"LISpMinerTasks.FFTTask.Quantifiers.Functional.CriticalEquivalence";

		protected override string identifier
		{
			get { return typeIdentifier; }
		}

		public override void CreateFunctions(BoxModuleI boxModule, out Ice.Object iceObject, out IFunctions functions)
		{
			CriticalEquivalenceFunctionsI result = new CriticalEquivalenceFunctionsI();
			iceObject = (Ice.Object)result;
			functions = (IFunctions)result;
		}

		public override string[] GetBoxModuleFunctionsIceIds()
		{
			return CriticalEquivalenceFunctionsI.ids__;
		}
	}
}