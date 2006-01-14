using System;
using System.Collections.Generic;

namespace Ferda.Modules.Boxes.DataMiningCommon.Attributes.EquidistantIntervalsAttribute
{
    class EquidistantIntervalsAttributeBoxInfo : Ferda.Modules.Boxes.DataMiningCommon.Attributes.AbstractDynamicAttributeBoxInfo
    {
        public const string typeIdentifier =
            "DataMiningCommon.Attributes.EquidistantIntervalsAttribute";

        protected override string identifier
        {
            get { return typeIdentifier; }
        }

        public override void CreateFunctions(BoxModuleI boxModule, out Ice.Object iceObject, out IFunctions functions)
        {
            EquidistantIntervalsAttributeFunctionsI result = new EquidistantIntervalsAttributeFunctionsI();
            iceObject = (Ice.Object)result;
            functions = (IFunctions)result;
        }

        public override string[] GetBoxModuleFunctionsIceIds()
        {
            return EquidistantIntervalsAttributeFunctionsI.ids__;
        }

        public override IAbstractDynamicAttribute getFuncIAbstractDynamicAttribute(BoxModuleI boxModule)
        {
            EquidistantIntervalsAttributeFunctionsI Func = (EquidistantIntervalsAttributeFunctionsI)boxModule.FunctionsIObj;
            return Func;
        }

        protected override IAbstractAttribute getFuncIAbstractAttribute(BoxModuleI boxModule)
        {
            EquidistantIntervalsAttributeFunctionsI Func = (EquidistantIntervalsAttributeFunctionsI)boxModule.FunctionsIObj;
            return Func;
        }

        public override PropertyValue GetReadOnlyPropertyValue(String propertyName, BoxModuleI boxModule)
        {
            EquidistantIntervalsAttributeFunctionsI Func = (EquidistantIntervalsAttributeFunctionsI)boxModule.FunctionsIObj;
            switch (propertyName)
            {
                case "Categories":
                    return new Ferda.Modules.CategoriesTI(Func.GetGeneratedAttribute().CategoriesStruct);
                case "CountOfCategories":
                    return new Ferda.Modules.LongTI(Func.GetGeneratedAttribute().CategoriesCount);
                case "IncludeNullCategory":
                    return new Ferda.Modules.StringTI(Func.GetGeneratedAttribute().IncludeNullCategoryName);
                default:
                    throw Ferda.Modules.Exceptions.SwitchCaseNotImplementedError(propertyName);
            }
        }
    }
}