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
            return "VirtualColumnGenericName";
           // return ((Functions)boxModule.FunctionsIObj).Name;
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
        public const string typeIdentifier = "DataPreparation.DataSource.VirtualColumn";
        protected override string identifier
        {
            get { return typeIdentifier; }
        }

    }
}