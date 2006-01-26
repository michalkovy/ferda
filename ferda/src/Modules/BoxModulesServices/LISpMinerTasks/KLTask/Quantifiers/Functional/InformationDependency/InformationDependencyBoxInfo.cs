using System;
using System.Collections.Generic;

namespace Ferda.Modules.Boxes.LISpMinerTasks.KLTask.Quantifiers.Functional.InformationDependency
{
	class InformationDependencyBoxInfo : Ferda.Modules.Boxes.LISpMinerTasks.KLTask.Quantifiers.AbstractKLTaskQuantifierBoxInfo
	{
		public const string typeIdentifier = 
			"LISpMinerTasks.KLTask.Quantifiers.Functional.InformationDependency";

		protected override string identifier
		{
			get { return typeIdentifier; }
		}

		public override void CreateFunctions(BoxModuleI boxModule, out Ice.Object iceObject, out IFunctions functions)
		{
			InformationDependencyFunctionsI result = new InformationDependencyFunctionsI();
			iceObject = (Ice.Object)result;
			functions = (IFunctions)result;
		}

		public override string[] GetBoxModuleFunctionsIceIds()
		{
			return InformationDependencyFunctionsI.ids__;
		}
	}
}