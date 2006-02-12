using System;
using System.Collections.Generic;

namespace Ferda.Modules.Boxes.DataMiningCommon.Attributes.EquifrequencyIntervalsAttribute
{
	class EquifrequencyIntervalsAttributeBoxInfo : Ferda.Modules.Boxes.DataMiningCommon.Attributes.AbstractDynamicAttributeBoxInfo
	{
		public const string typeIdentifier = 
			"DataMiningCommon.Attributes.EquifrequencyIntervalsAttribute";

		protected override string identifier
		{
			get { return typeIdentifier; }
		}

		public override void CreateFunctions(BoxModuleI boxModule, out Ice.Object iceObject, out IFunctions functions)
		{
			EquifrequencyIntervalsAttributeFunctionsI result = new EquifrequencyIntervalsAttributeFunctionsI();
			iceObject = (Ice.Object)result;
			functions = (IFunctions)result;
		}

		public override string[] GetBoxModuleFunctionsIceIds()
		{
			return EquifrequencyIntervalsAttributeFunctionsI.ids__;
		}

		public override IAbstractDynamicAttribute getFuncIAbstractDynamicAttribute(BoxModuleI boxModule)
		{
			EquifrequencyIntervalsAttributeFunctionsI Func = (EquifrequencyIntervalsAttributeFunctionsI)boxModule.FunctionsIObj;
			return Func;
		}

		protected override IAbstractAttribute getFuncIAbstractAttribute(BoxModuleI boxModule)
		{
			EquifrequencyIntervalsAttributeFunctionsI Func = (EquifrequencyIntervalsAttributeFunctionsI)boxModule.FunctionsIObj;
			return Func;
		}

		public override PropertyValue GetReadOnlyPropertyValue(String propertyName, BoxModuleI boxModule)
		{
			EquifrequencyIntervalsAttributeFunctionsI Func = (EquifrequencyIntervalsAttributeFunctionsI)boxModule.FunctionsIObj;
			switch (propertyName)
			{
                //case "Categories":
                //    return new Ferda.Modules.CategoriesTI(Func.GetGeneratedAttribute().CategoriesStruct);
				case "IncludeNullCategory":
                    return new Ferda.Modules.StringTI(Func.GetGeneratedAttribute().IncludeNullCategoryName);
				default:
					throw Ferda.Modules.Exceptions.SwitchCaseNotImplementedError(propertyName);
			}
		}
	}
}