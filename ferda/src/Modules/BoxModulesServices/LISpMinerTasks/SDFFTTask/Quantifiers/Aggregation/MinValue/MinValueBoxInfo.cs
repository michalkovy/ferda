using System;
using System.Collections.Generic;

namespace Ferda.Modules.Boxes.SDFFTTask.Quantifiers.Aggregation.MinValue
{
	class MinValueBoxInfo : Ferda.Modules.Boxes.SDFFTTask.Quantifiers.AbstractSDFFTTaskQuantifierBoxInfo
	{
		public const string typeIdentifier = 
			"LISpMinerTasks.SDFFTTask.Quantifiers.Aggregation.MinValue";

		protected override string identifier
		{
			get { return typeIdentifier; }
		}

		public override void CreateFunctions(BoxModuleI boxModule, out Ice.Object iceObject, out IFunctions functions)
		{
			MinValueFunctionsI result = new MinValueFunctionsI();
			iceObject = (Ice.Object)result;
			functions = (IFunctions)result;
		}

		public override string[] GetBoxModuleFunctionsIceIds()
		{
			return MinValueFunctionsI.ids__;
		}
	}
}