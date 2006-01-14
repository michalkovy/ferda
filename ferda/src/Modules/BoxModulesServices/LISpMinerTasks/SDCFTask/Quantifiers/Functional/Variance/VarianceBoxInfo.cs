using System;
using System.Collections.Generic;

namespace Ferda.Modules.Boxes.SDCFTask.Quantifiers.Functional.Variance
{
	class VarianceBoxInfo : Ferda.Modules.Boxes.SDCFTask.Quantifiers.AbstractSDCFTaskQuantifierBoxInfo
	{
		public const string typeIdentifier =
			"LISpMinerTasks.SDCFTask.Quantifiers.Functional.Variance";

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