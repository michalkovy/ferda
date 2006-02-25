using System;
using System.Collections.Generic;
using System.Text;
using Ferda.Modules.Boxes.DataMiningCommon.Column;

namespace Ferda.Modules.Boxes.DataMiningCommon.Attributes
{
    /// <summary>
    /// Interface which should be provided by each attribute box module.
    /// </summary>
    public interface IAbstractAttribute
    {
        /// <summary>
        /// Gets the categories names for property select options (see 
        /// <see cref="T:Ferda.Modules.Boxes.DataMiningCommon.Attributes.AbstractAttributeBoxInfo"/>).
        /// </summary>
        /// <returns></returns>
        SelectString[] GetPropertyCategoriesNames();
    }

    /// <summary>
    /// Abstract <see cref="T:Ferda.Modules.Boxes.BoxInfo"/> 
    /// class for all attribute box modules.
    /// </summary>
    public abstract class AbstractAttributeBoxInfo : BoxInfo
    {
        /// <summary>
        /// Gets default value for box module user label.
        /// </summary>
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

        /// <summary>
        /// Gets the function object of abstract attribute.
        /// </summary>
        /// <param name="boxModule">The box module.</param>
        /// <returns>FunctionsI object.</returns>
        protected abstract IAbstractAttribute getFuncIAbstractAttribute(BoxModuleI boxModule);
        //EachValueOneCategoryAttributeFunctionsI Func = (EachValueOneCategoryAttributeFunctionsI)boxModule.FunctionsIObj;

        /// <summary>
        /// Gets <see cref="T:Ferda.Modules.PropertyValue"/> from
        /// <see cref="T:Ice.ObjectPrx">objectPrx</see> parameter.
        /// </summary>
        public override PropertyValue GetPropertyObjectFromInterface(string propertyName, Ice.ObjectPrx objectPrx)
        {
            if (propertyName == "Categories")
                return new CategoriesTI(CategoriesTInterfacePrxHelper.checkedCast(objectPrx));
            throw Ferda.Modules.Exceptions.NameNotExistError(null, null, null, propertyName);
        }

        /// <summary>
        /// Gets array of <see cref="T:Ferda.Modules.SelectString"/> as
        /// options for property, whose options are dynamically variable.
        /// </summary>
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
