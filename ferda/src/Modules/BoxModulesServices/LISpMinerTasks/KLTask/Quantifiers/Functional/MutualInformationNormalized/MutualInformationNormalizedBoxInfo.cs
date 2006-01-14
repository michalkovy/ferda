using System;
using System.Collections.Generic;

namespace Ferda.Modules.Boxes.KLTask.Quantifiers.Functional.MutualInformationNormalized
{
	class MutualInformationNormalizedBoxInfo : Ferda.Modules.Boxes.KLTask.Quantifiers.AbstractKLTaskQuantifierBoxInfo
	{
		public const string typeIdentifier = 
			"LISpMinerTasks.KLTask.Quantifiers.Functional.MutualInformationNormalized";

		protected override string identifier
		{
			get { return typeIdentifier; }
		}

		public override void CreateFunctions(BoxModuleI boxModule, out Ice.Object iceObject, out IFunctions functions)
		{
			MutualInformationNormalizedFunctionsI result = new MutualInformationNormalizedFunctionsI();
			iceObject = (Ice.Object)result;
			functions = (IFunctions)result;
		}

		public override string[] GetBoxModuleFunctionsIceIds()
		{
			return MutualInformationNormalizedFunctionsI.ids__;
		}
	}
}