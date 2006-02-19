using System;
using System.Collections.Generic;
using System.Text;
using Ferda.Modules.Boxes.DataMiningCommon.Column;

namespace Ferda.Modules.Boxes.DataMiningCommon.Attributes
{
    public interface IAbstractAttribute
    {
        SelectString[] GetPropertyCategoriesNames();
    }

    public abstract class AbstractAttributeBoxInfo : BoxInfo
    {
        public override string GetDefaultUserLabel(BoxModuleI boxModule)
        {
            string nameInLiterals = boxModule.GetPropertyString("NameInLiterals");
            if (!String.IsNullOrEmpty(nameInLiterals))
                return nameInLiterals;
            Ice.ObjectPrx objectPrx;
            if (Ferda.Modules.Boxes.SocketConnections.TryGetObjectPrx(boxModule, "ColumnOrDerivedColumn", out objectPrx))
            {
                ColumnFunctionsPrx columnFunctionsPrx =
                    ColumnFunctionsPrxHelper.checkedCast(objectPrx);
                return columnFunctionsPrx.getColumnInfo().columnSelectExpression;
            }
            return null;
        }

        protected abstract IAbstractAttribute getFuncIAbstractAttribute(BoxModuleI boxModule);
        //EachValueOneCategoryAttributeFunctionsI Func = (EachValueOneCategoryAttributeFunctionsI)boxModule.FunctionsIObj;

        public override PropertyValue GetPropertyObjectFromInterface(string propertyName, Ice.ObjectPrx objectPrx)
        {
            if (propertyName == "Categories")
                return new CategoriesTI(CategoriesTInterfacePrxHelper.checkedCast(objectPrx));
            throw Ferda.Modules.Exceptions.NameNotExistError(null, null, null, propertyName);
        }

        public override SelectString[] GetPropertyOptions(string propertyName, BoxModuleI boxModule)
        {
            switch (propertyName)
            {
                case "XCategory":
                    return getFuncIAbstractAttribute(boxModule).GetPropertyCategoriesNames();
                case "IncludeNullCategory":
                    return getFuncIAbstractAttribute(boxModule).GetPropertyCategoriesNames();
                default:
                    return null;
            }
        }
    }
}
