using System;
using System.Diagnostics;
using System.Collections.Generic;
using Ferda.Guha.Data;
using Object = Ice.Object;
using FixedAtom = Ferda.Modules.Boxes.GuhaMining.FixedAtom;
using AtomSetting = Ferda.Modules.Boxes.GuhaMining.AtomSetting;

namespace Ferda.Modules.Boxes.DataPreparation.Categorization.EquifrequencyIntervals
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
            Functions Func = (Functions)boxModule.FunctionsIObj;
            string label = String.Empty;
            try
            {
                label = Func.GetColumnFunctionsPrx(false).getColumnInfo().columnSelectExpression;
            }
            catch
            {
                return Func.NameInLiterals;
            }
            if (label == String.Empty)
            {
                return Func.NameInLiterals;
            }
            else
            {
                if (Func.NameInLiterals != String.Empty)
                    return label +
                        " - " + Func.NameInLiterals;
                else
                    return label;
            }
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
                    case "FixedAtom":
                        //creating the info about the connections of the new module
                        moduleConnection.socketName =
                            FixedAtom.Functions.SockBitStringGenerator;
                        moduleConnection.boxModuleParam = boxModule.MyProxy;

                        //creating the new (single) module
                        singleModule.modulesConnection =
                            new ModulesConnection[] { moduleConnection };
                        singleModule.newBoxModuleIdentifier =
                            FixedAtom.BoxInfo.typeIdentifier;
                        break;

                    case "AtomSetting":
                        //creating the info about the connections of the new module
                        moduleConnection.socketName =
                            AtomSetting.Functions.SockBitStringGenerator;
                        moduleConnection.boxModuleParam = boxModule.MyProxy;

                        //creating the new (single) module
                        singleModule.modulesConnection =
                            new ModulesConnection[] { moduleConnection };
                        singleModule.newBoxModuleIdentifier =
                            AtomSetting.BoxInfo.typeIdentifier;
                        break;

                    case "StaticAttribute":
                        //creating the info about the connections of the new module
                        moduleConnection.socketName =
                            StaticAttribute.Functions.SockColumn;
                        moduleConnection.boxModuleParam = boxModule.MyProxy;

                        //creating the new (single) module
                        singleModule.modulesConnection =
                            new ModulesConnection[] { moduleConnection };
                        singleModule.newBoxModuleIdentifier =
                            StaticAttribute.BoxInfo.typeIdentifier;

                        Guha.Attribute.Attribute<IComparable> attribute = null;
                        PropertySetting editCategories = null;
                        try
                        {
                            attribute = ((Functions)boxModule.FunctionsIObj).GetAttribute(false);
                            editCategories =
                           new PropertySetting(Functions.PropCategories, new StringTI(
                           Guha.Attribute.Serializer.Serialize(
                           (attribute.Export()))));
                        }
                        catch
                        {
                            editCategories = new PropertySetting();
                        }

                        singleModule.propertySetting = new PropertySetting[] { editCategories };
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

        public override PropertyValue GetReadOnlyPropertyValue(string propertyName, BoxModuleI boxModule)
        {
            Functions Func = (Functions)boxModule.FunctionsIObj;
            switch (propertyName)
            {
                case Functions.PropIncludeNullCategory:
                    return Func.IncludeNullCategory;
                default:
                    throw new NotImplementedException();
            }
        }

        public override SelectString[] GetPropertyOptions(string propertyName, BoxModuleI boxModule)
        {
            Functions Func = (Functions)boxModule.FunctionsIObj;

            switch (propertyName)
            {
                case Functions.PropXCategory:
                    return BoxInfoHelper.GetSelectStringArray(
                        Func.GetCategoriesNames(false)
                        );
                default:
                    return null;
            }
        }

        public const string typeIdentifier = "DataPreparation.Categorization.EquifrequencyIntervals";

        protected override string identifier
        {
            get { return typeIdentifier; }
        }

        public override void Validate(BoxModuleI boxModule)
        {
            Functions Func = (Functions)boxModule.FunctionsIObj;

            // try to invoke methods
            object dummy = Func.GetColumnFunctionsPrx(true);
            dummy = Func.GetAttributeId();
            dummy = Func.GetAttributeNames();
            try
            {
                dummy = Func.GetAttribute(true);
            }
            catch (Exception e)
            {
                throw Exceptions.BadValueError(
                    null,
                    boxModule.StringIceIdentity,
                    "Equidistant interval supports only nominal and higher column semantics",
                    new string[] { Functions.SockColumn },
                    restrictionTypeEnum.OtherReason
                    );
            }
            dummy = Func.GetCategoriesNames(true);
            dummy = Func.GetCategoriesAndFrequencies(true);
            dummy = Func.GetBitStrings(true);
            Debug.Assert(dummy == null);

             if (String.IsNullOrEmpty(Func.NameInLiterals))
                  throw Exceptions.BadValueError(
                      null,
                      boxModule.StringIceIdentity,
                      "Property \"Name in literals\" can not be empty string.",
                      new string[] { Functions.PropNameInLiterals },
                      restrictionTypeEnum.OtherReason
                      );

            CardinalityEnum potentiallyCardinality = Func.PotentiallyCardinality(true);

            if (Common.CompareCardinalityEnums(
                    Func.Cardinality,
                    potentiallyCardinality
                    ) > 1)
            {
                throw Exceptions.BadValueError(
                    null,
                    boxModule.StringIceIdentity,
                    "Unsupported cardinality type for current attribute setting.",
                    new string[] { Functions.PropCardinality },
                    restrictionTypeEnum.OtherReason
                    );
            }

            if (potentiallyCardinality == CardinalityEnum.Nominal)
            {
                throw Exceptions.BadValueError(
                    null,
                    boxModule.StringIceIdentity,
                    "Unsupported cardinality type for current attribute setting.",
                    new string[] { Functions.PropCardinality },
                    restrictionTypeEnum.OtherReason
                    );
            }
        }
    }
}