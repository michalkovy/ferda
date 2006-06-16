using System;
using Ferda.Guha.Data;
using Exception=System.Exception;
using Object=Ice.Object;

namespace Ferda.Modules.Boxes.DataPreparation.Datasource.Column
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
            return ((Functions) boxModule.FunctionsIObj).SelectExpression;
        }

        public override ModulesAskingForCreation[] GetModulesAskingForCreation(string[] localePrefs,
                                                                               BoxModuleI boxModule)
        {
            //TODO
            return new ModulesAskingForCreation[0];
            //throw new Exception("The method or operation is not implemented.");
        }

        public override SelectString[] GetPropertyOptions(string propertyName, BoxModuleI boxModule)
        {
            Functions Func = (Functions) boxModule.FunctionsIObj;

            switch (propertyName)
            {
                case Functions.PropSelectExpression:
                    return BoxInfoHelper.GetSelectStringArray(
                        Func.GetColumnsNames(false)
                        );
                default:
                    return null;
            }
        }

        public const string typeIdentifier = "DataPreparation.DataSource.Column";

        protected override string identifier
        {
            get { return typeIdentifier; }
        }

        public override PropertyValue GetReadOnlyPropertyValue(string propertyName, BoxModuleI boxModule)
        {
            Functions Func = (Functions) boxModule.FunctionsIObj;
            switch (propertyName)
            {
                case Functions.PropDataType:
                    return Func.DataType;
                case Functions.PropValueMin:
                    return Func.ValueMin;
                case Functions.PropValueMax:
                    return Func.ValueMax;
                case Functions.PropValueAverage:
                    return Func.ValueAverage;
                case Functions.PropValueVariability:
                    return Func.ValueVariability;
                case Functions.PropValueStandardDeviation:
                    return Func.ValueStandardDeviation;
                case Functions.PropValueDistincts:
                    return Func.ValueDistincts;
                default:
                    throw new NotImplementedException();
            }
        }

        public override void Validate(BoxModuleI boxModule)
        {
            Functions Func = (Functions) boxModule.FunctionsIObj;

            // try to invoke methods
            object dummy = Func.GetDataTableFunctionsPrx(true);
            GenericColumn tmp = Func.GetGenericColumn(true);
            dummy = Func.GetColumnsNames(true);
            dummy = Func.GetColumnStatistics(true);
            dummy = Func.GetColumnExplain(true);
            dummy = Func.GetDistinctsAndFrequencies(true);
            dummy = Func.GetColumnInfo(true);

            if (GenericColumn.CompareCardinality(
                    Func.Cardinality,
                    tmp.PotentiallyCardinality
                    ) > 1)
            {
                BadParamsError ex = new BadParamsError("Unsupported cardinality type for current data type.");
                ex.boxIdentity = boxModule.StringIceIdentity;
                throw ex;
            }
        }
    }
}