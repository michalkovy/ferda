// SelectTablesIce.cs - class for ice communication
//
// Author: Daniel KUpka <kupkd9am@post.cz>
//
// Copyright (c) 2007 Daniel Kupka
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
using Ferda.Modules;
using Ferda.Modules.Boxes;
using Ferda.ModulesManager;
using Ferda.FrontEnd.AddIns;
using System.Resources;
using System.Reflection;
using PropertyInfo=Ferda.Modules.PropertyInfo;
using Ferda.FrontEnd.AddIns.Common.MyIce;

using System.Windows.Forms;
using Ice;

namespace Ferda.FrontEnd.AddIns.FormGenerator.MyIce
{
  //protected BoxModuleI _boxModule;
    /// <summary>
    /// Class for ice communication
    /// </summary>
    class FormGeneratorIce : ModuleForInteractionIce
    {
        #region Private variables

        /// <summary>
        /// Owner of addin
        /// </summary>
        Ferda.FrontEnd.AddIns.IOwnerOfAddIn ownerOfAddIn;

        /// <summary>
        /// L10n resource manager
        /// </summary>
        private ResourceManager resManager;

        /// <summary>
        /// L10n string, for now en-US or cs-CZ
        /// </summary>
        private string localizationString;

        /// <summary>
        /// Resulting string with XML content
        /// </summary>
        string  returnString;

        #endregion


        #region Constructor

        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="ownerOfAddIn">Owner of addin</param>
        public FormGeneratorIce(Ferda.FrontEnd.AddIns.IOwnerOfAddIn ownerOfAddIn):
            base(ownerOfAddIn, null)
        {
            this.ownerOfAddIn = ownerOfAddIn;

            //setting the ResManager resource manager and localization string
           // resManager = new ResourceManager("Ferda.FrontEnd.AddIns.MultiSelectStrings.Localization_en-US",
            //Assembly.GetExecutingAssembly());
            //localizationString = "en-US";
        }

        #endregion


        /// <summary>
        /// Gets a list of sockets needed to be connected in order for the module to
        /// work
        /// </summary>
        /// <param name="__current">ICE stuff</param>
        /// <returns>List of socket names for module to work propertly</returns>
        public override string[] getNeededConnectedSockets(Ice.Current __current)
        {
            return new string[] {  };
        }

        /// <summary>
        /// Gets accepted box types
        /// </summary>
        /// <param name="__current">Ice context</param>
        /// <returns>Array of boxtypes</returns>
        public override Ferda.Modules.BoxType[] getAcceptedBoxTypes(Ice.Current __current)
        {
            Modules.BoxType boxType = new Modules.BoxType();
            boxType.neededSockets = new Modules.NeededSocket[0];
            //boxType.functionIceId = "::Ferda::Modules::Boxes::DataPreparation::DatabaseFunctions";
   boxType.functionIceId = "::Ferda::Modules::Boxes::Wizards::Wizard::WizardFunctions";
            return new Modules.BoxType[] { boxType };
        }

        /// <summary>
        /// Gets hint to the module according for a specified localization
        /// </summary>
        /// <param name="localePrefs">Localization preferences</param>
        /// <param name="__current">Some ICE stuff</param>
        /// <returns>Localized hint</returns>
        public override string getHint(string[] localePrefs, Ice.Current __current)
        {
            //Localize(localePrefs);
            //return resManager.GetString("DataBaseInfoModule");

            return "Run scenario";
        }

        /// <summary>
        /// Gets label of the module according to the localization
        /// </summary>
        /// <param name="localePrefs">Localization specification</param>
        /// <param name="__current">Some ICE stuff</param>
        /// <returns>Localized label</returns>
        public override string getLabel(string[] localePrefs, Ice.Current __current)
        {
           // Localize(localePrefs);
           // return resManager.GetString("DataBaseInfoModule");
            return "Run scenario";

        }

        #region Icerun

        /// <summary>
        /// The method is called by te Ice framework when a module for interaction
        /// is to be displayed
        /// </summary>
        /// <param name="boxModuleParam">Box that is executing this module for interaction</param>
        /// <param name="localePrefs">Localization preferences</param>
        /// <param name="manager">Proxy address of the modules manager</param>
        /// <param name="__current">Ice context</param>
        public override void run(Ferda.Modules.BoxModulePrx boxModuleParam, string[] localePrefs, ManagersEnginePrx manager, Ice.Current __current)
        {
            string label =
                manager.getProjectInformation().getUserLabel(
                Ice.Util.identityToString(boxModuleParam.ice_getIdentity()));

            string id = Ice.Util.identityToString(boxModuleParam.ice_getIdentity());

            IBoxModule box = 
                ownerOfAddIn.ProjectManager.ModulesManager.GetIBoxModuleByIdentity(id);

            IBoxModule form = (box.ConnectedTo())[0];
            string editor = form.GetPropertyString("FormEditor");
        }


        /// <summary>
        /// Ice run
        /// </summary>
        /// <param name="valueBefore">Previous value</param>
        /// <param name="boxModuleParam">boxmoduleparam</param>
        /// <param name="localePrefs">localeprefs</param>
        /// <param name="manager">Manager proxy</param>
        /// <param name="about"></param>
        /// <param name="current__">Ice context</param>
        /// <returns>Modified property value</returns>
        /*public override PropertyValue run(PropertyValue valueBefore, string propertyName, BoxModulePrx boxModuleParam, string[] localePrefs, ManagersEnginePrx manager, out string about, Ice.Current current__)
        {
            string locale;

          //  about = resManager.GetString("FormEditorAbout");
            StringT XMLString = (StringT)valueBefore;
            PropertyValue returnValue = new PropertyValue();
            PropertyValue propertyValue = valueBefore;

            

            FormEditor.WizardFormEditor control =
                new FormEditor.WizardFormEditor(XMLString.getStringValue());

            control.Disposed += new EventHandler(control_Disposed);
            System.Windows.Forms.DialogResult result = this.ownerOfAddIn.ShowDialog(control);

            if (result == System.Windows.Forms.DialogResult.OK)
            {
                XMLString.stringValue = this.returnString;
                PropertyValue resultValue = XMLString;
               // about = this.getPropertyAbout(resultValue);
                about = "Form editor about";
                propertyValue = resultValue;
            }
            else
            {
                //about = this.getPropertyAbout(valueBefore);
                about = "form";
                return valueBefore;
            }

            //PropertyValue propertyValue = null;

            

            //propertyValue = result;
           /* try
            {
                locale = localePrefs[0];
                localizationString = locale;
                locale = "Ferda.FrontEnd.AddIns.MultiSelectStrings.Localization_" + locale;
                resManager = new ResourceManager(locale, Assembly.GetExecutingAssembly());
            }
            catch {}*/

         /*   StringSeqT selectedStrings = (StringSeqT)valueBefore;
            PropertyValue propertyValue = valueBefore;
            PropertyValue returnValue = new PropertyValue();

            // for dynamicaly changing select box params
            SelectString[] selStrings = boxModuleParam.getPropertyOptions(propertyName);
            
            // for static select box params
            if (selStrings == null || selStrings.Length == 0)
            {
                PropertyInfo[] properties = boxModuleParam.getMyFactory().getProperties();
                foreach (PropertyInfo property in properties)
                {
                    if (property.name == propertyName)
                        selStrings = property.selectBoxParams;
                }
            }
            
            MultiSelectStrings.MultiSelectStringsControl listView =
                new MultiSelectStrings.MultiSelectStringsControl(localePrefs,
                ownerOfAddIn,
                selStrings,
                selectedStrings.stringSeqValue
                );

            listView.ShowInTaskbar = false;
            listView.Disposed += new EventHandler(listView_Disposed);
            System.Windows.Forms.DialogResult result = this.ownerOfAddIn.ShowDialog(listView);
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                selectedStrings.stringSeqValue = this.returnStrings;
                PropertyValue resultValue = selectedStrings;
                about = this.getPropertyAbout(resultValue);
                propertyValue = resultValue;
            }
            else
            {
                about = this.getPropertyAbout(valueBefore);
                return valueBefore;
            }*/
          /*  return propertyValue;
        }*/

        #endregion


        #region Other private methods

        /// <summary>
        /// Handler of disposing event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void control_Disposed(object sender, EventArgs e)
        {
            /*Ferda.FrontEnd.AddIns.FormEditor.WizardFormEditor control =
                (Ferda.FrontEnd.AddIns.FormEditor.WizardFormEditor)sender;

            this.returnString = control.ReturnString;*/
        }

        #endregion
    }
}
