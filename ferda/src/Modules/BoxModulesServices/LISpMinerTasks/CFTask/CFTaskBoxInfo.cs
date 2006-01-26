using System;
using System.Collections.Generic;
using System.Text;

namespace Ferda.Modules.Boxes.LISpMinerTasks.CFTask
{
    class CFTaskBoxInfo : LISpMinerAbstractTaskBoxInfo
    {
        public const string typeIdentifier =
            "LISpMinerTasks.CFTask";

        protected override string identifier
        {
            get { return typeIdentifier; }
        }

        public override void CreateFunctions(BoxModuleI boxModule, out Ice.Object iceObject, out IFunctions functions)
        {
            CFTaskFunctionsI result = new CFTaskFunctionsI();
            iceObject = (Ice.Object)result;
            functions = (IFunctions)result;
        }

        public override string[] GetBoxModuleFunctionsIceIds()
        {
            return CFTaskFunctionsI.ids__;
        }
    }
}
