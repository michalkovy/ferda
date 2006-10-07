using System;
using System.Diagnostics;
using System.Collections.Generic;
using Ferda.Guha.Data;
using Object = Ice.Object;

namespace Ferda.Modules.Boxes.DataPreparation.Categorization.EquidistantIntervals
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
            return ((Functions)boxModule.FunctionsIObj).NameInLiterals;
        }

        public override ModulesAskingForCreation[] GetModulesAskingForCreation(string[] localePrefs, BoxModuleI boxModule)
        {
            return new ModulesAskingForCreation[0];
        }

        public override SelectString[] GetPropertyOptions(string propertyName, BoxModuleI boxModule)
        {
            Functions Func = (Functions)boxModule.FunctionsIObj;
            
            switch (propertyName)
            {
                case Functions.PropCountOfIntervals:
                    return BoxInfoHelper.GetSelectStringArray(new List<string>()
                        );
                default:
                    return null;
            }
          //  return null;
        }

        public const string typeIdentifier = "DataPreparation.Categorization.EquidistantIntervals";

        protected override string identifier
        {
            get { return typeIdentifier; }
        }
    }
}