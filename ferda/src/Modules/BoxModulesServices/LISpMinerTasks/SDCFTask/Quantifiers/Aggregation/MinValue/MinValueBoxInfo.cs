using System;
using System.Collections.Generic;

namespace Ferda.Modules.Boxes.SDCFTask.Quantifiers.Aggregation.MinValue
{
	class MinValueBoxInfo : Ferda.Modules.Boxes.SDCFTask.Quantifiers.AbstractSDCFTaskQuantifierBoxInfo
	{
		public const string typeIdentifier =
			"LISpMinerTasks.SDCFTask.Quantifiers.Aggregation.MinValue";

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