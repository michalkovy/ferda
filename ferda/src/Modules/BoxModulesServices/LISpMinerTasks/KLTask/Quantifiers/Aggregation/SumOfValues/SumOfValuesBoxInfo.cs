using System;
using System.Collections.Generic;

namespace Ferda.Modules.Boxes.KLTask.Quantifiers.Aggregation.SumOfValues
{
	class SumOfValuesBoxInfo : Ferda.Modules.Boxes.KLTask.Quantifiers.AbstractKLTaskQuantifierBoxInfo
	{
		public const string typeIdentifier = 
			"LISpMinerTasks.KLTask.Quantifiers.Aggregation.SumOfValues";

		protected override string identifier
		{
			get { return typeIdentifier; }
		}

		public override void CreateFunctions(BoxModuleI boxModule, out Ice.Object iceObject, out IFunctions functions)
		{
			SumOfValuesFunctionsI result = new SumOfValuesFunctionsI();
			iceObject = (Ice.Object)result;
			functions = (IFunctions)result;
		}

		public override string[] GetBoxModuleFunctionsIceIds()
		{
			return SumOfValuesFunctionsI.ids__;
		}
	}
}