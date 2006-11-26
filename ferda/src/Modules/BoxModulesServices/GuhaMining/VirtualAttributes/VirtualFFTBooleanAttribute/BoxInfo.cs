using System;
using Ferda.Guha.Data;
using Object=Ice.Object;
using Ferda.Modules.Boxes.DataPreparation;

namespace Ferda.Modules.Boxes.GuhaMining.VirtualAttributes.VirtualFFTBooleanAttribute
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
                default:
                    return null;
            }
        }
        
        public override PropertyValue GetReadOnlyPropertyValue(string propertyName, BoxModuleI boxModule)
        {
            Functions Func = (Functions)boxModule.FunctionsIObj;
            switch (propertyName)
            {
                default:
                    throw new NotImplementedException();
            }
        }

        public const string typeIdentifier = "GuhaMining.VirtualAttributes.VirtualFFTBooleanAttribute";
        
        public override void Validate(BoxModuleI boxModule)
        {
            Functions Func = (Functions)boxModule.FunctionsIObj;
            object dummy = Func.GetSourceDataTableId();
            
            DataTableFunctionsPrx _dtPrx = Func.GetMasterDataTableFunctionsPrx(true);
            string[] _primaryKeyColumns = _dtPrx.getDataTableInfo().primaryKeyColumns;
            if (_primaryKeyColumns.Length < 1)
            {
                throw Exceptions.BoxRuntimeError(null, boxModule.StringIceIdentity, "No unique key selected");
            }

            if (Func.CountVector == null)
            {
                throw Exceptions.BoxRuntimeError(null, boxModule.StringIceIdentity, "Unable to get count vector");
            }
            
        }

        protected override string identifier
        {
            get { return typeIdentifier; }
        }

    }
}