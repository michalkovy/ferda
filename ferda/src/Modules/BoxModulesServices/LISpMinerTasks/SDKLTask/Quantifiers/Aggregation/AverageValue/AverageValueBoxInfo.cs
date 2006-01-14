using System;
using System.Collections.Generic;

namespace Ferda.Modules.Boxes.SDKLTask.Quantifiers.Aggregation.AverageValue
{
	class AverageValueBoxInfo : Ferda.Modules.Boxes.SDKLTask.Quantifiers.AbstractSDKLTaskQuantifierBoxInfo
	{
		public const string typeIdentifier = 
			"LISpMinerTasks.SDKLTask.Quantifiers.Aggregation.AverageValue";

		protected override string identifier
		{
			get { return typeIdentifier; }
		}

		public override void CreateFunctions(BoxModuleI boxModule, out Ice.Object iceObject, out IFunctions functions)
		{
			AverageValueFunctionsI result = new AverageValueFunctionsI();
			iceObject = (Ice.Object)result;
			functions = (IFunctions)result;
		}

		public override string[] GetBoxModuleFunctionsIceIds()
		{
			return AverageValueFunctionsI.ids__;
		}
	}
}