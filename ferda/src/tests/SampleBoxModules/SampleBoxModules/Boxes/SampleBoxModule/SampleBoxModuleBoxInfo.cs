using System;
using System.Collections.Generic;
using System.Text;
using Ferda.Modules.Boxes;

namespace SampleBoxModules.SampleBoxes.SampleBoxModule
{
    /// <summary>
    /// This class implements most of functions used by <see cref="T:Ferda.ModulesManager"/>.
    /// For more information <see cref="T:Ferda.Modules.Boxes.BoxInfo"/>.
    /// </summary>
    /// <seealso cref="T:Ferda.Modules.BoxModuleI"/>
    /// <seealso cref="T:Ferda.Modules.BoxModuleFactoryI"/>
    /// <seealso cref="T:Ferda.Modules.BoxModuleFactoryCreatorI"/>
    class SampleBoxModuleBoxInfo : BoxInfo
    {


        /* Other functions to override
         * public virtual PropertyValue GetPropertyObjectFromInterface(string propertyName, Ice.ObjectPrx objectPrx)
         * public virtual PropertyValue GetReadOnlyPropertyValue(string propertyName, BoxModuleI boxModule)
         * public virtual bool IsPropertySet(string propertyName, PropertyValue propertyValue)
         * public virtual DynamicHelpItem[] GetDynamicHelpItems(string[] localePrefs, BoxModuleI boxModule)
         * public virtual void RunAction(string actionName, BoxModuleI boxModule)
         * */
        public override void CreateFunctions(Ferda.Modules.BoxModuleI boxModule, out Ice.Object iceObject, out Ferda.Modules.IFunctions functions)
        {

            //TODO BODY
            throw new Exception("The method or operation is not implemented.");
        }

        public override string[] GetBoxModuleFunctionsIceIds()
        {//TODO BODY
            throw new Exception("The method or operation is not implemented.");
        }

        public override string GetDefaultUserLabel(Ferda.Modules.BoxModuleI boxModule)
        {//TODO BODY
            throw new Exception("The method or operation is not implemented.");
        }

        public override Ferda.Modules.ModulesAskingForCreation[] GetModulesAskingForCreation(string[] localePrefs, Ferda.Modules.BoxModuleI boxModule)
        {//TODO BODY
            throw new Exception("The method or operation is not implemented.");
        }

        public override Ferda.Modules.SelectString[] GetPropertyOptions(string propertyName, Ferda.Modules.BoxModuleI boxModule)
        {//TODO BODY
            throw new Exception("The method or operation is not implemented.");
        }

        protected override string identifier
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }
    }
}
