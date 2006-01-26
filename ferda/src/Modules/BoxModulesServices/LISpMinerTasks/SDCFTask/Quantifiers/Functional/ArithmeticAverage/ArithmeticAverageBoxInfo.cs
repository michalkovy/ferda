using System;
using System.Collections.Generic;

namespace Ferda.Modules.Boxes.LISpMinerTasks.SDCFTask.Quantifiers.Functional.ArithmeticAverage
{
	class ArithmeticAverageBoxInfo : Ferda.Modules.Boxes.LISpMinerTasks.SDCFTask.Quantifiers.AbstractSDCFTaskQuantifierBoxInfo
	{
		public const string typeIdentifier =
			"LISpMinerTasks.SDCFTask.Quantifiers.Functional.ArithmeticAverage";

		protected override string identifier
		{
			get { return typeIdentifier; }
		}

		public override void CreateFunctions(BoxModuleI boxModule, out Ice.Object iceObject, out IFunctions functions)
		{
			ArithmeticAverageFunctionsI result = new ArithmeticAverageFunctionsI();
			iceObject = (Ice.Object)result;
			functions = (IFunctions)result;
		}

		public override string[] GetBoxModuleFunctionsIceIds()
		{
			return ArithmeticAverageFunctionsI.ids__;
		}
	}
}