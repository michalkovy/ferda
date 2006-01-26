using System;
using System.Collections.Generic;

namespace Ferda.Modules.Boxes.LISpMinerTasks.CFTask.Quantifiers.Functional.NominalVariation
{
	class NominalVariationBoxInfo : Ferda.Modules.Boxes.LISpMinerTasks.CFTask.Quantifiers.AbstractCFTaskQuantifierBoxInfo
	{
		public const string typeIdentifier =
			"LISpMinerTasks.CFTask.Quantifiers.Functional.NominalVariation";

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