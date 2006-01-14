using System;
using System.Collections.Generic;

namespace Ferda.Modules.Boxes.CFTask.Quantifiers.Aggregation.AnyValue
{
	class AnyValueBoxInfo : Ferda.Modules.Boxes.CFTask.Quantifiers.AbstractCFTaskQuantifierBoxInfo
	{
		public const string typeIdentifier =
            "LISpMinerTasks.CFTask.Quantifiers.Aggregation.AnyValue";

		protected override string identifier
		{
			get { return typeIdentifier; }
		}

		public override void CreateFunctions(BoxModuleI boxModule, out Ice.Object iceObject, out IFunctions functions)
		{
			AnyValueFunctionsI result = new AnyValueFunctionsI();
			iceObject = (Ice.Object)result;
			functions = (IFunctions)result;
		}

		public override string[] GetBoxModuleFunctionsIceIds()
		{
			return AnyValueFunctionsI.ids__;
		}
	}
}