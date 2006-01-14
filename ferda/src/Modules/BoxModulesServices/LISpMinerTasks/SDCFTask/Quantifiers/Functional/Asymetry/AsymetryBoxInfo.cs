using System;
using System.Collections.Generic;

namespace Ferda.Modules.Boxes.SDCFTask.Quantifiers.Functional.Asymetry
{
	class AsymetryBoxInfo : Ferda.Modules.Boxes.SDCFTask.Quantifiers.AbstractSDCFTaskQuantifierBoxInfo
	{
		public const string typeIdentifier =
			"LISpMinerTasks.SDCFTask.Quantifiers.Functional.Asymetry";

		protected override string identifier
		{
			get { return typeIdentifier; }
		}

		public override void CreateFunctions(BoxModuleI boxModule, out Ice.Object iceObject, out IFunctions functions)
		{
			AsymetryFunctionsI result = new AsymetryFunctionsI();
			iceObject = (Ice.Object)result;
			functions = (IFunctions)result;
		}

		public override string[] GetBoxModuleFunctionsIceIds()
		{
			return AsymetryFunctionsI.ids__;
		}
	}
}