using System;
using System.Collections.Generic;

namespace Ferda.Modules.Boxes.SDCFTask.Quantifiers.Functional.Skewness
{
	class SkewnessBoxInfo : Ferda.Modules.Boxes.SDCFTask.Quantifiers.AbstractSDCFTaskQuantifierBoxInfo
	{
		public const string typeIdentifier =
			"LISpMinerTasks.SDCFTask.Quantifiers.Functional.Skewness";

		protected override string identifier
		{
			get { return typeIdentifier; }
		}

		public override void CreateFunctions(BoxModuleI boxModule, out Ice.Object iceObject, out IFunctions functions)
		{
			SkewnessFunctionsI result = new SkewnessFunctionsI();
			iceObject = (Ice.Object)result;
			functions = (IFunctions)result;
		}

		public override string[] GetBoxModuleFunctionsIceIds()
		{
			return SkewnessFunctionsI.ids__;
		}
	}
}