using System.Collections.Generic;
using Ice;

namespace Ferda.Modules.Boxes.GuhaMining.DisjunctionSetting
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
            Functions Func = (Functions) boxModule.FunctionsIObj;

            string[] inputBoxesLabels = Func.GetInputBoxesLabels();

            if (inputBoxesLabels.Length == 0)
                return null;
            else
            {
                List<string> result = new List<string>();
                foreach (string s in inputBoxesLabels)
                {
                    if (s.Contains("\u2227"))
                        result.Add("(" + s + ")");
                    else
                        result.Add(s);
                }
                return BoxInfoHelper.SequenceToString(result, "\u2228");
            }
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

        public const string typeIdentifier = "GuhaMining.DisjunctionSetting";

        protected override string identifier
        {
            get { return typeIdentifier; }
        }

        public override void Validate(BoxModuleI boxModule)
        {
            Functions Func = (Functions) boxModule.FunctionsIObj;

            // try to invoke methods
            object dummy = Func.GetEntitySetting(true);
            dummy = Func.GetAttributeNames();
        }
    }
}