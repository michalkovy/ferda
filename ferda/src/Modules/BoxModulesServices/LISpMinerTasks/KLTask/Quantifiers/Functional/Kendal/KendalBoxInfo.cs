using System;
using System.Collections.Generic;

namespace Ferda.Modules.Boxes.LISpMinerTasks.KLTask.Quantifiers.Functional.Kendal
{
	class KendalBoxInfo : Ferda.Modules.Boxes.LISpMinerTasks.KLTask.Quantifiers.AbstractKLTaskQuantifierBoxInfo
	{
		public const string typeIdentifier = 
			"LISpMinerTasks.KLTask.Quantifiers.Functional.Kendal";

		protected override string identifier
		{
			get { return typeIdentifier; }
		}

		public override void CreateFunctions(BoxModuleI boxModule, out Ice.Object iceObject, out IFunctions functions)
		{
			KendalFunctionsI result = new KendalFunctionsI();
			iceObject = (Ice.Object)result;
			functions = (IFunctions)result;
		}

		public override string[] GetBoxModuleFunctionsIceIds()
		{
			return KendalFunctionsI.ids__;
		}
	}
}