using System;
using System.Collections.Generic;
using System.Text;

namespace SampleBoxModules.SampleBoxes.SampleBoxModule
{
	class SampleBoxModuleFunctionsI : SampleBoxModuleFunctionsDisp_, Ferda.Modules.IFunctions
	{
		public override string HelloWorld(Ice.Current __current)
		{
			return "Hello World!";
		}

		protected Ferda.Modules.BoxModuleI boxModule;
		protected Ferda.Modules.Boxes.IBoxInfo boxInfo;

		#region IFunctions Members

		void Ferda.Modules.IFunctions.setBoxModuleInfo(Ferda.Modules.BoxModuleI boxModule, Ferda.Modules.Boxes.IBoxInfo boxInfo)
		{
			this.boxModule = boxModule;
			this.boxInfo = boxInfo;
		}

		#endregion
	}
}
