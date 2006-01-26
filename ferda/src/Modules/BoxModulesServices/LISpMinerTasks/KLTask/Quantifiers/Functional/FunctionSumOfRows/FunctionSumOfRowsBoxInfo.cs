using System;
using System.Collections.Generic;

namespace Ferda.Modules.Boxes.LISpMinerTasks.KLTask.Quantifiers.Functional.FunctionSumOfRows
{
	class FunctionSumOfRowsBoxInfo : Ferda.Modules.Boxes.LISpMinerTasks.KLTask.Quantifiers.AbstractKLTaskQuantifierBoxInfo
	{
		public const string typeIdentifier = 
			"LISpMinerTasks.KLTask.Quantifiers.Functional.FunctionSumOfRows";

		protected override string identifier
		{
			get { return typeIdentifier; }
		}

		public override void CreateFunctions(BoxModuleI boxModule, out Ice.Object iceObject, out IFunctions functions)
		{
			FunctionSumOfRowsFunctionsI result = new FunctionSumOfRowsFunctionsI();
			iceObject = (Ice.Object)result;
			functions = (IFunctions)result;
		}

		public override string[] GetBoxModuleFunctionsIceIds()
		{
			return FunctionSumOfRowsFunctionsI.ids__;
		}
	}
}