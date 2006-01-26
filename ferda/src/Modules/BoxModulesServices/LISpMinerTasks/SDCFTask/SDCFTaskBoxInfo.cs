using System;
using System.Collections.Generic;
using System.Text;

namespace Ferda.Modules.Boxes.LISpMinerTasks.SDCFTask
{
    class SDCFTaskBoxInfo : LISpMinerAbstractTaskBoxInfo
    {
        public const string typeIdentifier =
            "LISpMinerTasks.SDCFTask";

        protected override string identifier
        {
            get { return typeIdentifier; }
        }

        public override void CreateFunctions(BoxModuleI boxModule, out Ice.Object iceObject, out IFunctions functions)
        {
            SDCFTaskFunctionsI result = new SDCFTaskFunctionsI();
            iceObject = (Ice.Object)result;
            functions = (IFunctions)result;
        }

        public override string[] GetBoxModuleFunctionsIceIds()
        {
            return SDCFTaskFunctionsI.ids__;
        }
    }
}
