using System;
using Ferda.Guha.Data;
using Exception=System.Exception;
using Object=Ice.Object;

namespace Ferda.Modules.Boxes.DataPreparation.Categorization.EachValueOneCategory
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
            return ((Functions) boxModule.FunctionsIObj).NameInLiterals;
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
                case Functions.PropXCategory:
                    return BoxInfoHelper.GetSelectStringArray(
                        Func.GetCategoriesNames(false)
                        );
                default:
                    return null;
            }
        }

        public const string typeIdentifier = "DataPreparation.Categorization.EachValueOneCategory";

        protected override string identifier
        {
            get { return typeIdentifier; }
        }

        public override PropertyValue GetReadOnlyPropertyValue(string propertyName, BoxModuleI boxModule)
        {
            Functions Func = (Functions) boxModule.FunctionsIObj;
            switch (propertyName)
            {
                case Functions.PropCountOfCategories:
                    return Func.CountOfCategories;
                case Functions.PropIncludeNullCategory:
                    return Func.IncludeNullCategory;
                default:
                    throw new NotImplementedException();
            }
        }

        public override void Validate(BoxModuleI boxModule)
        {
            Functions Func = (Functions) boxModule.FunctionsIObj;

            // try to invoke methods
            object dummy = Func.GetColumnFunctionsPrx(true);
            dummy = Func.GetAttribute(true);
            dummy = Func.GetCategoriesNames(true);
            dummy = Func.GetCategoriesAndFrequencies(true);
            dummy = Func.GetBitStrings(true);

            if (GenericColumn.CompareCardinality(
                   Func.Cardinality,
                   Func.PotentiallyCardinality(true)
                   ) > 1)
            {
                throw Exceptions.BadValueError(
                    null,
                    boxModule.StringIceIdentity,
                    "Unsupported cardinality type for current attribute setting.",
                    new string[]{Functions.PropCardinality},
                    restrictionTypeEnum.OtherReason
                );
            }
        }
    }
}