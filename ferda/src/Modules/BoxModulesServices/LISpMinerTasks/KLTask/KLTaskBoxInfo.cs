using System;
using System.Collections.Generic;
using System.Text;

namespace Ferda.Modules.Boxes.LISpMinerTasks.KLTask
{
	class KLTaskBoxInfo : LISpMinerAbstractTaskBoxInfo
	{
		public const string typeIdentifier = 
			"LISpMinerTasks.KLTask";

		protected override string identifier
		{
			get { return typeIdentifier; }
		}

		public override void CreateFunctions(BoxModuleI boxModule, out Ice.Object iceObject, out IFunctions functions)
		{
			KLTaskFunctionsI result = new KLTaskFunctionsI();
			iceObject = (Ice.Object)result;
			functions = (IFunctions)result;
		}

		public override string[] GetBoxModuleFunctionsIceIds()
		{
			return KLTaskFunctionsI.ids__;
		}
	}
}