using System;
using System.Collections.Generic;
using Ferda.Guha.Data;
using Object = Ice.Object;

namespace Ferda.Modules.Boxes.GuhaMining.Tasks.FourFold
{
    internal class BoxInfo : Boxes.BoxInfo
    {
        public override void CreateFunctions(BoxModuleI boxModule, out Object iceObject, out IFunctions functions)
        {
            Functions result = new Functions();
            iceObject = result;
            functions = result;
        }

        public override string[] GetBoxModuleFunctionsIceIds()
        {
            return Functions.ids__;
        }

        public override string GetDefaultUserLabel(BoxModuleI boxModule)
        {
            return null;
        }

        public override ModulesAskingForCreation[] GetModulesAskingForCreation(string[] localePrefs,
                                                                               BoxModuleI boxModule)
        {
            return new ModulesAskingForCreation[0];
        }

        public override SelectString[] GetPropertyOptions(string propertyName, BoxModuleI boxModule)
        {
            return null;
        }

        public const string typeIdentifier = "GuhaMining.Tasks.FourFold";

        protected override string identifier
        {
            get { return typeIdentifier; }
        }

        public override PropertyValue GetReadOnlyPropertyValue(string propertyName, BoxModuleI boxModule)
        {
            Functions Func = (Functions)boxModule.FunctionsIObj;
            switch (propertyName)
            {
                case Functions.PropDone:
                    return Func.Done;
                case Functions.PropNumberOfHypotheses:
                    return Func.NumberOfHypotheses;
                default:
                    throw new NotImplementedException();
            }
        }

        public override void RunAction(string actionName, BoxModuleI boxModule)
        {
            Functions Func = (Functions)boxModule.FunctionsIObj;
            switch (actionName)
            {
                case "Run":
                    Func.Run();
                    break;
                default:
                    throw Exceptions.NameNotExistError(null, actionName);
            }
        }

        public override void Validate(BoxModuleI boxModule)
        {
            //Functions Func = (Functions)boxModule.FunctionsIObj;
            //TODO
        }
    }
}