using Ice;

namespace Ferda.Modules.Boxes.GuhaMining.FixedAtom
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
            string attributeName = Func.GetAttributeName(false);
            if (attributeName == null)
                return null;
            else
            {
                return attributeName + "(" + BoxInfoHelper.SequenceToString(Func.Categories, ", ") + ")";
            }
        }

        public override ModulesAskingForCreation[] GetModulesAskingForCreation(string[] localePrefs,
                                                                               BoxModuleI boxModule)
        {
            return new ModulesAskingForCreation[0];
        }

        public override SelectString[] GetPropertyOptions(string propertyName, BoxModuleI boxModule)
        {
            Functions Func = (Functions) boxModule.FunctionsIObj;

            switch (propertyName)
            {
                case Functions.PropCategories:
                    return BoxInfoHelper.GetSelectStringArray(
                        Func.GetCategoriesIds(false)
                        );
                default:
                    return null;
            }
        }

        public const string typeIdentifier = "GuhaMining.FixedAtom";

        protected override string identifier
        {
            get { return typeIdentifier; }
        }

        public override void Validate(BoxModuleI boxModule)
        {
            Functions Func = (Functions) boxModule.FunctionsIObj;

            if (Func.Categories.Length == 0)
                throw Exceptions.BadValueError(null, boxModule.StringIceIdentity,
                                               "No categories specified in fixed set.",
                                               new string[] {Functions.PropCategories},
                                               restrictionTypeEnum.Minimum);

            string[] categories = Func.GetCategoriesIds(true);
            foreach (string s in Func.Categories)
            {
                bool found = false;
                foreach (string category in categories)
                {
                    if (category == s)
                    {
                        found = true;
                        break;
                    }
                }
                if (!found)
                    throw Exceptions.BadValueError(null, boxModule.StringIceIdentity,
                                                   "Category \"" + s + "\" is not specified in source categories.",
                                                   new string[] {Functions.PropCategories},
                                                   restrictionTypeEnum.OtherReason);
            }

            // try to invoke methods
            object dummy = Func.GetEntitySetting(true);
            dummy = Func.GetAttributeName(true);
        }
    }
}