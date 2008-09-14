// BoxInfo.cs - BoxInfo class for the ETree Classifier
//
// Authors: Martin Ralbovský <martin.ralbovsky@gmail.com> 
//
// Copyright (c) 2008 Martin Ralbovský
//
// This program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA

using System;
using System.Collections.Generic;
using System.Text;

namespace Ferda.Modules.Boxes.GuhaMining.Classification.ETreeClassifier
{
    /// <summary>
    /// This class implements most of functions used by <see cref="T:Ferda.ModulesManager"/>.
    /// For more information <see cref="T:Ferda.Modules.Boxes.BoxInfo"/>.
    /// </summary>
    /// <seealso cref="T:Ferda.Modules.BoxModuleI"/>
    /// <seealso cref="T:Ferda.Modules.BoxModuleFactoryI"/>
    /// <seealso cref="T:Ferda.Modules.BoxModuleFactoryCreatorI"/>
    internal class BoxInfo : Boxes.BoxInfo
    {
        /// <summary>
        /// Functions creates an object of <see cref="T:Ferda.Modules.IFunctions">IFuntions</see>
        /// type that provides functionality of the box
        /// </summary>
        /// <param name="boxModule">Current box module</param>
        /// <param name="iceObject">ICE stuff</param>
        /// <param name="functions">The new created functions object</param>
        public override void CreateFunctions(Ferda.Modules.BoxModuleI boxModule, out Ice.Object iceObject, out Ferda.Modules.IFunctions functions)
        {
            Functions result = new Functions();
            iceObject = result;
            functions = result;
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
        /// <see cref="M:Ferda.Modules.Boxes.IBoxInfo.GetBoxModuleFunctionsIceIds()"/>.
        /// </example>
        public override string[] GetBoxModuleFunctionsIceIds()
        {
            return Functions.ids__;
        }

        /// <summary>
        /// Gets default value for box module user label.
        /// </summary>
        /// <param name="boxModule">A module that returns the label</param>
        /// <returns>The user label</returns>
        public override string GetDefaultUserLabel(Ferda.Modules.BoxModuleI boxModule)
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
        public override Ferda.Modules.ModulesAskingForCreation[] GetModulesAskingForCreation(string[] localePrefs, Ferda.Modules.BoxModuleI boxModule)
        {
            return new ModulesAskingForCreation[] { };
        }

        /// <summary>
        /// Gets array of <see cref="T:Ferda.Modules.SelectString"/> as
        /// options for property, whose options are dynamically variable.
        /// </summary>
        /// <param name="boxModule">The current module</param>
        /// <param name="propertyName">Name of the property</param>
        /// <returns>String options of the property</returns>
        public override SelectString[] GetPropertyOptions(string propertyName, BoxModuleI boxModule)
        {
            return null;
        }

        /// <summary>
        /// Gets value of readonly property value.
        /// </summary>
        /// <param name="propertyName">Name of readonly property.</param>
        /// <param name="boxModule">Box module.</param>
        /// <returns>
        /// A <see cref="T:Ferda.Modules.PropertyValue"/> of
        /// readonly property named <c>propertyName</c>.
        /// </returns>
        public override PropertyValue GetReadOnlyPropertyValue(string propertyName, BoxModuleI boxModule)
        {
            Functions Func = (Functions)boxModule.FunctionsIObj;
            switch (propertyName)
            {
                case Functions.SockTruePositive:
                    return new IntTI(Func.TruePositive);
                case Functions.SockFalseNegative:
                    return new IntTI(Func.FalseNegative);
                case Functions.SockFalsePositive:
                    return new IntTI(Func.FalsePositive);
                case Functions.SockTrueNegative:
                    return new IntTI(Func.TrueNegative);
                default:
                    throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Validates the box
        /// </summary>
        /// <param name="boxModule">box instance to be validated</param>
        public override void Validate(BoxModuleI boxModule)
        {
            Functions Func = (Functions)boxModule.FunctionsIObj;

            //checking if the attribute names and the names of columns of the attributes match
            List<string> dbNames = new List<string>(Func.GetDatatableColumnNames());
            List<string> colNames = new List<string>(Func.GetAttributeColumnNames());

            foreach (string col in colNames)
            {
                if (!dbNames.Contains(col))
                {
                    BoxRuntimeError error = new BoxRuntimeError(null, 
                        "Some columns in the ETree construction do not occur in the database.");
                    throw error;
                }
            }

            Func.RetrieveDecisionTrees();

            if (Func.decisionTrees.Length == 0)
            {
                BoxRuntimeError error = new BoxRuntimeError(null,
                    "In order to classify, there has to be at least one tree in the ETree results.");
                throw error;
            }

            if (Func.TreeNumber > Func.decisionTrees.Length - 1)
            {
                BoxRuntimeError error = new BoxRuntimeError(null,
                    "Number of tree to be used for classification is higher than number of all the trees in the result.");
                throw error;
            }
        }

        /// <summary>
        /// Executes (runs) action specified by <c>actionName</c>.
        /// </summary>
        /// <param name="actionName">Name of the action.</param>
        /// <param name="boxModule">The Box module.</param>
        /// <exception cref="T:Ferda.Modules.NameNotExistError">
        /// Thrown if action named <c>actionName</c> doesn`t exist.
        /// </exception>
        /// <exception cref="T:Ferda.Modules.BoxRuntimeError">
        /// Thrown if any runtime error occured while executing the action.
        /// </exception>
        public override void RunAction(string actionName, BoxModuleI boxModule)
        {
            Validate(boxModule);

            Functions Func = (Functions)boxModule.FunctionsIObj;

            switch (actionName)
            {
                case "Classify":
                    Func.Classify();
                    break;
                default:
                    throw Exceptions.NameNotExistError(null, actionName);
            }
        }

        #region Type Identifier

        /// <summary>
        /// This is recomended (not required) to have <c>public const string</c>
        /// field in the BoxInfo implementation which holds the identifier
        /// of type of the box module.
        /// </summary>
        public const string typeIdentifier = "GuhaMining.Classification.ETreeClassifier";

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

        #endregion

        /* Other functions to override
    * public virtual PropertyValue GetPropertyObjectFromInterface(string propertyName, Ice.ObjectPrx objectPrx)
    * public virtual bool IsPropertySet(string propertyName, PropertyValue propertyValue)
    * public virtual DynamicHelpItem[] GetDynamicHelpItems(string[] localePrefs, BoxModuleI boxModule)
    * */
    }
}
