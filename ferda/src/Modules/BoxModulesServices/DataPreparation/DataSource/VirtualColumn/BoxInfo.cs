using System;
using Ferda.Guha.Data;
using Object=Ice.Object;

namespace Ferda.Modules.Boxes.DataPreparation.Datasource.VirtualColumn
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
           // return ((Functions)boxModule.FunctionsIObj).Name;
            //return String.Empty;
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
                case Functions.PropMasterIdColumn:
                    return BoxInfoHelper.GetSelectStringArray(
                        Func.GetMasterColumnsNames(false)
                        );

                case Functions.PropDetailIdColumn:
                    return BoxInfoHelper.GetSelectStringArray(
                        Func.GetDetailColumnsNames(false)
                        );

                case Functions.PropDetailResultColumn:
                    return BoxInfoHelper.GetSelectStringArray(
                        Func.GetDetailColumnsNames(false)
                        );
                default:
                    return null;
            }
        }
        
        public override PropertyValue GetReadOnlyPropertyValue(string propertyName, BoxModuleI boxModule)
        {
            Functions Func = (Functions)boxModule.FunctionsIObj;
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
        
        public const string typeIdentifier = "DataPreparation.DataSource.VirtualColumn";
        
        public override void Validate(BoxModuleI boxModule)
        {
            Functions Func = (Functions)boxModule.FunctionsIObj;

            // try to invoke methods
            object dummy = Func.GetDataTableFunctionsPrx(true);
            GenericColumn tmp = Func.GetGenericColumn(true);
            dummy = Func.GetColumnsNames(true);
            dummy = Func.GetColumnStatistics(true);
            dummy = Func.GetColumnExplain(true);
            //dummy = Func.GetDistinctsAndFrequencies(true);
            dummy = Func.GetColumnInfo(true);

            if (Common.CompareCardinalityEnums(
                    Func.Cardinality,
                    tmp.PotentiallyCardinality
                    ) > 1)
            {
                throw Exceptions.BadValueError(
                    null,
                    boxModule.StringIceIdentity,
                    "Unsupported cardinality type for current data type.",
                    new string[] { Functions.PropCardinality },
                    restrictionTypeEnum.OtherReason
                    );
            }
        }

        protected override string identifier
        {
            get { return typeIdentifier; }
        }

    }
}