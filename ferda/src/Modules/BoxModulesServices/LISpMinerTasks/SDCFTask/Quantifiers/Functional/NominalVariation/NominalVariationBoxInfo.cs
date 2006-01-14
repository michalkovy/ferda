using System;
using System.Collections.Generic;

namespace Ferda.Modules.Boxes.SDCFTask.Quantifiers.Functional.NominalVariation
{
	class NominalVariationBoxInfo : Ferda.Modules.Boxes.SDCFTask.Quantifiers.AbstractSDCFTaskQuantifierBoxInfo
	{
		public const string typeIdentifier =
			"LISpMinerTasks.SDCFTask.Quantifiers.Functional.NominalVariation";

		protected override string identifier
		{
			get { return typeIdentifier; }
		}

		public override void CreateFunctions(BoxModuleI boxModule, out Ice.Object iceObject, out IFunctions functions)
		{
			NominalVariationFunctionsI result = new NominalVariationFunctionsI();
			iceObject = (Ice.Object)result;
			functions = (IFunctions)result;
		}

		public override string[] GetBoxModuleFunctionsIceIds()
		{
			return NominalVariationFunctionsI.ids__;
		}
	}
}