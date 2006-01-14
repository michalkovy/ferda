using System;
using System.Collections.Generic;

namespace Ferda.Modules.Boxes.CFTask.Quantifiers.Functional.DiscreteOrdinaryVariation
{
	class DiscreteOrdinaryVariationBoxInfo : Ferda.Modules.Boxes.CFTask.Quantifiers.AbstractCFTaskQuantifierBoxInfo
	{
		public const string typeIdentifier =
			"LISpMinerTasks.CFTask.Quantifiers.Functional.DiscreteOrdinaryVariation";

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