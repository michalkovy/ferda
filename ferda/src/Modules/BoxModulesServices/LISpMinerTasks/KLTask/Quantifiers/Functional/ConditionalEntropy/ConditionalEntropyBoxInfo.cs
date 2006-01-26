using System;
using System.Collections.Generic;

namespace Ferda.Modules.Boxes.LISpMinerTasks.KLTask.Quantifiers.Functional.ConditionalEntropy
{
	class ConditionalEntropyBoxInfo : Ferda.Modules.Boxes.LISpMinerTasks.KLTask.Quantifiers.AbstractKLTaskQuantifierBoxInfo
	{
		public const string typeIdentifier = 
			"LISpMinerTasks.KLTask.Quantifiers.Functional.ConditionalEntropy";

		protected override string identifier
		{
			get { return typeIdentifier; }
		}

		public override void CreateFunctions(BoxModuleI boxModule, out Ice.Object iceObject, out IFunctions functions)
		{
			ConditionalEntropyFunctionsI result = new ConditionalEntropyFunctionsI();
			iceObject = (Ice.Object)result;
			functions = (IFunctions)result;
		}

		public override string[] GetBoxModuleFunctionsIceIds()
		{
			return ConditionalEntropyFunctionsI.ids__;
		}
	}
}