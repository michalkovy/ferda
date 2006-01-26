using System;
using System.Collections.Generic;

namespace Ferda.Modules.Boxes.LISpMinerTasks.CFTask.Quantifiers.Functional.Variance
{
	class VarianceBoxInfo : Ferda.Modules.Boxes.LISpMinerTasks.CFTask.Quantifiers.AbstractCFTaskQuantifierBoxInfo
	{
		public const string typeIdentifier =
			"LISpMinerTasks.CFTask.Quantifiers.Functional.Variance";

		protected override string identifier
		{
			get { return typeIdentifier; }
		}

		public override void CreateFunctions(BoxModuleI boxModule, out Ice.Object iceObject, out IFunctions functions)
		{
			VarianceFunctionsI result = new VarianceFunctionsI();
			iceObject = (Ice.Object)result;
			functions = (IFunctions)result;
		}

		public override string[] GetBoxModuleFunctionsIceIds()
		{
			return VarianceFunctionsI.ids__;
		}
	}
}