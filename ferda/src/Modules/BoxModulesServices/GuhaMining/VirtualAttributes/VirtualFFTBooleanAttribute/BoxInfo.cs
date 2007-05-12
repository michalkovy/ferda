using System;
using System.Collections.Generic;
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

        /// <summary>
        /// Gets the box modules asking for creation.
        /// </summary>
        /// <param name="localePrefs">The localization preferences.</param>
        /// <param name="boxModule">The box module.</param>
        /// <returns>
        /// Array of <see cref="T:Ferda.Modules.ModuleAskingForCreation">
        /// Modules Asking For Creation</see>.
        /// </returns>
        public override ModulesAskingForCreation[] GetModulesAskingForCreation(string[] localePrefs,
                                                                               BoxModuleI boxModule)
        {
            //getting the information what is in the config files
            Dictionary<string, ModulesAskingForCreation> modulesAFC =
                getModulesAskingForCreationNonDynamic(localePrefs);
            //creating the structure that will be returned
            List<ModulesAskingForCreation> result =
                new List<ModulesAskingForCreation>();

            ModulesConnection moduleConnection;
            ModuleAskingForCreation singleModule;

            foreach (string moduleAFCname in modulesAFC.Keys)
            {
                singleModule = new ModuleAskingForCreation();
                moduleConnection = new ModulesConnection();
                //no need to set any property
                singleModule.propertySetting = new PropertySetting[] { };

                switch (moduleAFCname)
                {
                    case "ConjunctionSetting":
                        //creating the info about the connections of the new module
                        moduleConnection.socketName =
                            ConjunctionSetting.Functions.SockBooleanAttributeSetting;
                        moduleConnection.boxModuleParam = boxModule.MyProxy;

                        //creating the new (single) module
                        singleModule.modulesConnection =
                            new ModulesConnection[] { moduleConnection };
                        singleModule.newBoxModuleIdentifier =
                            ConjunctionSetting.BoxInfo.typeIdentifier;
                        break;

                    case "DisjunctionSetting":
                        //creating the info about the connections of the new module
                        moduleConnection.socketName =
                            DisjunctionSetting.Functions.SockBooleanAttributeSetting;
                        moduleConnection.boxModuleParam = boxModule.MyProxy;

                        //creating the new (single) module
                        singleModule.modulesConnection =
                            new ModulesConnection[] { moduleConnection };
                        singleModule.newBoxModuleIdentifier =
                            DisjunctionSetting.BoxInfo.typeIdentifier;
                        break;

                    case "Sign":
                        //creating the info about the connections of the new module
                        moduleConnection.socketName =
                            Sign.Functions.SockBooleanAttributeSetting;
                        moduleConnection.boxModuleParam = boxModule.MyProxy;

                        //creating the new (single) module
                        singleModule.modulesConnection =
                            new ModulesConnection[] { moduleConnection };
                        singleModule.newBoxModuleIdentifier =
                            Sign.BoxInfo.typeIdentifier;
                        break;

                    case "ClassOfEquivalence":
                        //creating the info about the connections of the new module
                        moduleConnection.socketName =
                            ClassOfEquivalence.Functions.SockBooleanAttributeSetting;
                        moduleConnection.boxModuleParam = boxModule.MyProxy;

                        //creating the new (single) module
                        singleModule.modulesConnection =
                            new ModulesConnection[] { moduleConnection };
                        singleModule.newBoxModuleIdentifier =
                            ClassOfEquivalence.BoxInfo.typeIdentifier;
                        break;

                    default:
                        throw new NotImplementedException();
                }

                //setting the newModules property of each modules for intearction
                modulesAFC[moduleAFCname].newModules =
                    new ModuleAskingForCreation[] { singleModule };
                result.Add(modulesAFC[moduleAFCname]);
            }

            return result.ToArray();
        }

        
        public override SelectString[] GetPropertyOptions(string propertyName, BoxModuleI boxModule)
        {
            Functions Func = (Functions)boxModule.FunctionsIObj;

            switch (propertyName)
            {
               // case Functions.PropMasterIdColumn:
              //      return BoxInfoHelper.GetSelectStringArray(
               //         Func.GetMasterColumnsNames(false)
               //         );

               // case Functions.PropDetailIdColumn:
              //      return BoxInfoHelper.GetSelectStringArray(
              //          Func.GetDetailColumnsNames(false)
              //          );

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

            dummy = 
                Ferda.Modules.Boxes.GuhaMining.Tasks.Common.GetBooleanAttributes(boxModule, Func);

            dummy = 
                Ferda.Modules.Boxes.GuhaMining.Tasks.Common.GetQuantifierBaseFunctions(boxModule, true);

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