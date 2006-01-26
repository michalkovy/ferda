using System;
using System.Collections.Generic;

namespace Ferda.Modules.Boxes.LISpMinerTasks.SDCFTask.Quantifiers.Functional.DiscreteOrdinaryVariation
{
	class DiscreteOrdinaryVariationBoxInfo : Ferda.Modules.Boxes.LISpMinerTasks.SDCFTask.Quantifiers.AbstractSDCFTaskQuantifierBoxInfo
	{
		public const string typeIdentifier =
			"LISpMinerTasks.SDCFTask.Quantifiers.Functional.DiscreteOrdinaryVariation";

		protected override string identifier
		{
			get { return typeIdentifier; }
		}

		public override void CreateFunctions(BoxModuleI boxModule, out Ice.Object iceObject, out IFunctions functions)
		{
			DiscreteOrdinaryVariationFunctionsI result = new DiscreteOrdinaryVariationFunctionsI();
			iceObject = (Ice.Object)result;
			functions = (IFunctions)result;
		}

		public override string[] GetBoxModuleFunctionsIceIds()
		{
			return DiscreteOrdinaryVariationFunctionsI.ids__;
		}
	}
}