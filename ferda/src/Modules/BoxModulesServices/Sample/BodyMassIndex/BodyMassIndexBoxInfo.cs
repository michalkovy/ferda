using System;
using System.Collections.Generic;
using System.Text;
using Ferda.Modules;

namespace Ferda.Modules.Boxes.Sample.BodyMassIndex
{
    /// <summary>
    /// Implementation of <see cref="T:Ferda.Modules.Boxes.IBoxInfo"/>
    /// using inheritence of abstract class <see cref="T:Ferda.Modules.Boxes.BoxInfo"/>.
    /// This class has to be registered in <see cref="T:Ferda.Modules.FerdaServiceI"/> class ... 
    /// please see <see cref="T:Ferda.Modules.Boxes.Sample.Service"/> and its
    /// method <see cref="M:Ferda.Modules.Boxes.Sample.Service.registerBoxes()"/>.
    /// </summary>
    public class BodyMassIndexBoxInfo : Ferda.Modules.Boxes.BoxInfo
    {
        /// <summary>
        /// Creates <see cref="T:Ice.Object"/> implementing Ice interface of
        /// the box module i.e. box`s functions declared in slice design.
        /// </summary>
        /// <param name="boxModule">Box module, to which created functions
        /// will belong to.</param>
        /// <param name="iceObject">An out parameter returning <see cref="T:Ice.Object"/>
        /// implementing box`s "ice" functions. This value is same as value
        /// of <c>functions</c>.</param>
        /// <param name="functions">An out parameter returning <see cref="T:Ice.Object"/>
        /// implementing box`s "ice" functions. This value is same as value
        /// of <c>iceObject</c>.</param>
        /// <example>
        /// Please see an example for <see cref="T:Ferda.Modules.Boxes.IBoxInfo">IBoxInfo`s</see>
        /// 	<see cref="M:Ferda.Modules.Boxes.IBoxInfo.CreateFunctions(Ferda.Modules.BoxModuleI,Ice.Object,Ferda.Modules.IFunctions)"/>.
        /// </example>
        /// <remarks>
        /// Each instance of the box module has its own functions object but
        /// class implementing <see cref="T:Ferda.Modules.Boxes.IBoxInfo">
        /// this interface</see> is shared by all instances of the box modules
        /// of the same type <see cref="P:Ferda.Modules.Boxes.IBoxInfo.Identifier"/>
        /// </remarks>
        public override void CreateFunctions(BoxModuleI boxModule, out Ice.Object iceObject, out IFunctions functions)
        {
            BodyMassIndexFunctionsI result = new BodyMassIndexFunctionsI();
            iceObject = (Ice.Object)result;
            functions = (IFunctions)result;
        }

        /// <summary>
        /// Gets function`s Ice identifiers of the box module.
        /// </summary>
        /// <returns>
        /// An array of strings representing Ice identifiers
        /// of the box module`s functions.
        /// </returns>
        /// <example>
        /// Please see an example for <see cref="T:Ferda.Modules.Boxes.IBoxInfo">IBoxInfo`s</see>
        /// 	<see cref="M:Ferda.Modules.Boxes.IBoxInfo.GetBoxModuleFunctionsIceIds()"/>.
        /// </example>
        public override string[] GetBoxModuleFunctionsIceIds()
        {
            return BodyMassIndexFunctionsI.ids__;
        }

        /// <summary>
        /// Gets default value for box module user label.
        /// </summary>
        public override string GetDefaultUserLabel(BoxModuleI boxModule)
        {
            BodyMassIndexFunctionsI functionsObject = (BodyMassIndexFunctionsI)boxModule.FunctionsIObj;
            return functionsObject.GetDefaultUserLabel();
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
        public override ModulesAskingForCreation[] GetModulesAskingForCreation(string[] localePrefs, BoxModuleI boxModule)
        {
            Dictionary<string, ModulesAskingForCreation> modulesAFC = this.getModulesAskingForCreationNonDynamic(localePrefs);
            List<ModulesAskingForCreation> result = new List<ModulesAskingForCreation>();
            ModulesAskingForCreation moduleAFC;
            ModulesConnection moduleConnection;
            ModuleAskingForCreation singleModuleAFC;
            foreach (string moduleAFCName in modulesAFC.Keys)
            {
                moduleAFC = modulesAFC[moduleAFCName];
                moduleConnection = new ModulesConnection();
                singleModuleAFC = new ModuleAskingForCreation();
                switch (moduleAFCName)
                {
                    case "Attribute":
                        moduleConnection.socketName = "ColumnOrDerivedColumn";
                        singleModuleAFC.newBoxModuleIdentifier = "DataMiningCommon.Attributes.Attribute";
                        // == Ferda.Modules.Boxes.DataMiningCommon.Attributes.Attribute.AttributeBoxInfo.typeIdentifier;
                        break;
                    case "AtomSetting":
                        moduleConnection.socketName = "Attribute";
                        singleModuleAFC.newBoxModuleIdentifier = "DataMiningCommon.AtomSetting";
                        // == Ferda.Modules.Boxes.DataMiningCommon.AtomSetting.AtomSettingBoxInfo.typeIdentifier;
                        break;
                    case "CategorialPartialCedentSetting":
                        moduleConnection.socketName = "Attribute";
                        singleModuleAFC.newBoxModuleIdentifier = "DataMiningCommon.CategorialPartialCedentSetting";
                        // == Ferda.Modules.Boxes.DataMiningCommon.CategorialPartialCedentSetting.CategorialPartialCedentSettingBoxInfo.typeIdentifier;
                        break;
                    default:
                        throw Ferda.Modules.Exceptions.SwitchCaseNotImplementedError(moduleAFCName);
                }
                moduleConnection.boxModuleParam = boxModule.MyProxy;
                singleModuleAFC.modulesConnection = new ModulesConnection[] { moduleConnection };
                moduleAFC.newModules = new ModuleAskingForCreation[] { singleModuleAFC };
                result.Add(moduleAFC);
            }
            return result.ToArray();
        }

        /// <summary>
        /// Gets array of <see cref="T:Ferda.Modules.SelectString"/> as
        /// options for property, whose options are dynamically variable.
        /// </summary>
        public override SelectString[] GetPropertyOptions(string propertyName, BoxModuleI boxModule)
        {
            return null;
        }

        
        /// <summary>
        /// This is recomended (not required) to have <c>public const string</c> 
        /// field in the BoxInfo implementation which holds the identifier 
        /// of type of the box module.
        /// </summary>
        public const string typeIdentifier = "Sample.BodyMassIndex";

        /// <summary>
        /// Unique identifier of type of Box module
        /// </summary>
        /// <value></value>
        /// <remarks>
        /// This string identifier is parsed i.e. dots (".") are
        /// replaced by <see cref="P:System.IO.Path.DirectorySeparatorChar"/>.
        /// Returned path is combined with application directory`s
        /// <see cref="F:Ferda.Modules.Boxes.BoxInfo.configFilesFolderName">subdirectory</see>
        /// and final path is used for getting stored configuration [localization] XML files.
        /// </remarks>
        protected override string identifier
        {
            get { return typeIdentifier; }
        }
    }
}
