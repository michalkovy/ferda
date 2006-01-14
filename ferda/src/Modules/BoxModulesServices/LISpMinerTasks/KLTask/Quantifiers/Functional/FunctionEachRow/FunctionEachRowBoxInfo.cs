using System;
using System.Collections.Generic;

namespace Ferda.Modules.Boxes.KLTask.Quantifiers.Functional.FunctionEachRow
{
	class FunctionEachRowBoxInfo : Ferda.Modules.Boxes.KLTask.Quantifiers.AbstractKLTaskQuantifierBoxInfo
	{
		public const string typeIdentifier = 
			"LISpMinerTasks.KLTask.Quantifiers.Functional.FunctionEachRow";

		protected override string identifier
		{
			get { return typeIdentifier; }
		}

		public override void CreateFunctions(BoxModuleI boxModule, out Ice.Object iceObject, out IFunctions functions)
		{
			FunctionEachRowFunctionsI result = new FunctionEachRowFunctionsI();
			iceObject = (Ice.Object)result;
			functions = (IFunctions)result;
		}

		public override string[] GetBoxModuleFunctionsIceIds()
		{
			return FunctionEachRowFunctionsI.ids__;
		}
	}
}