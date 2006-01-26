using System;
using System.Collections.Generic;

namespace Ferda.Modules.Boxes.LISpMinerTasks.KLTask.Quantifiers.Functional.ChiSquareTest
{
	class ChiSquareTestBoxInfo : Ferda.Modules.Boxes.LISpMinerTasks.KLTask.Quantifiers.AbstractKLTaskQuantifierBoxInfo
	{
		public const string typeIdentifier = 
			"LISpMinerTasks.KLTask.Quantifiers.Functional.ChiSquareTest";

		protected override string identifier
		{
			get { return typeIdentifier; }
		}

		public override void CreateFunctions(BoxModuleI boxModule, out Ice.Object iceObject, out IFunctions functions)
		{
			ChiSquareTestFunctionsI result = new ChiSquareTestFunctionsI();
			iceObject = (Ice.Object)result;
			functions = (IFunctions)result;
		}

		public override string[] GetBoxModuleFunctionsIceIds()
		{
			return ChiSquareTestFunctionsI.ids__;
		}
	}
}