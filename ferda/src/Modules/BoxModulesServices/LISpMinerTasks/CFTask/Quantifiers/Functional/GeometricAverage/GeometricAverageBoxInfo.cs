using System;
using System.Collections.Generic;

namespace Ferda.Modules.Boxes.CFTask.Quantifiers.Functional.GeometricAverage
{
	class GeometricAverageBoxInfo : Ferda.Modules.Boxes.CFTask.Quantifiers.AbstractCFTaskQuantifierBoxInfo
	{
		public const string typeIdentifier =
			"LISpMinerTasks.CFTask.Quantifiers.Functional.GeometricAverage";

		protected override string identifier
		{
			get { return typeIdentifier; }
		}

		public override void CreateFunctions(BoxModuleI boxModule, out Ice.Object iceObject, out IFunctions functions)
		{
			GeometricAverageFunctionsI result = new GeometricAverageFunctionsI();
			iceObject = (Ice.Object)result;
			functions = (IFunctions)result;
		}

		public override string[] GetBoxModuleFunctionsIceIds()
		{
			return GeometricAverageFunctionsI.ids__;
		}
	}
}