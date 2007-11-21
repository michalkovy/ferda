// OntologyMappingIce.cs - class for ice communication
//
// Author: Martin Zeman <martinzeman@email.cz>
//
// Copyright (c) 2007 Martin Zeman
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

using Ferda.Modules.Boxes.DataPreparation;
using Ferda.Modules.Boxes.OntologyRelated.OntologyMapping;
using Ferda.Modules.Boxes.OntologyRelated.Ontology;
using Ferda.OntologyRelated.generated.OntologyData;


namespace Ferda.FrontEnd.AddIns.OntologyMapping.MyIce
{
    /// <summary>
    /// Class for ice communication
    /// </summary>
    class OntologyMappingIce : SettingModuleWithStringAbilityDisp_
    {
        #region Private variables

        /// <summary>
        /// Owner of addin
        /// </summary>
        IOwnerOfAddIn ownerOfAddIn;
        
        /// <summary>
        /// L10n resource manager
        /// </summary>
        private ResourceManager resManager;

        /// <summary>
        /// L10n string, for now en-US or cs-CZ
        /// </summary>
        private string localizationString;

        /// <summary>
        /// Resulting DSN string
        /// </summary>
        string returnString;

        #endregion


        #region Constructor

        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="ownerOfAddIn">Owner of addin</param>
        public OntologyMappingIce(Ferda.FrontEnd.AddIns.IOwnerOfAddIn ownerOfAddIn)
        {
            this.ownerOfAddIn = ownerOfAddIn;

            //setting the ResManager resource manager and localization string
            resManager = new ResourceManager("Ferda.FrontEnd.AddIns.OntologyMapping.Localization_en-US",
            Assembly.GetExecutingAssembly());
            localizationString = "en-US";
        }

        #endregion


        #region Other ice

        public override string getLabel(string[] localePrefs, global::Ice.Current current__)
        {
            string locale;
            try
            {
                locale = localePrefs[0];
                localizationString = locale;
                locale = "Ferda.FrontEnd.AddIns.OntologyMapping.Localization_" + locale;
                resManager = new ResourceManager(locale, Assembly.GetExecutingAssembly());
            }
            catch { }
            return resManager.GetString("OntologyMapping");
        }

        public override string getPropertyAbout(PropertyValue value, global::Ice.Current current__)
        {
            return ((StringT)value).getStringValue();
        }

        public override string getIdentifier(global::Ice.Current current__)
        {
            return "OntologyMapping";
        }

        public override PropertyValue convertFromStringAbout(string about, string[] localePrefs, Ice.Current current__)
        {
            return new StringTI(about);
        }

        #endregion


        #region IceRun

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
        public override PropertyValue run(PropertyValue valueBefore, string propertyName, BoxModulePrx boxModuleParam, string[] localePrefs, ManagersEnginePrx manager, out string about, global::Ice.Current current__)
        {
            string locale;
            try
            {
                locale = localePrefs[0];
                localizationString = locale;
                locale = "Ferda.FrontEnd.AddIns.OntologyMapping.Localization_" + locale;
                resManager = new ResourceManager(locale, Assembly.GetExecutingAssembly());
            }
            catch
            {
            }
            
            OntologyMappingFunctionsPrx OntologyMappingPrx = 
                OntologyMappingFunctionsPrxHelper.checkedCast(boxModuleParam.getFunctions());

            DataTableFunctionsPrx DataTablePrx;
            OntologyFunctionsPrx OntologyPrx;
            try
            {
                DataTablePrx =
                    DataTableFunctionsPrxHelper.checkedCast(boxModuleParam.getConnections("DataTable")[0].getFunctions());
            
                OntologyPrx =
                    OntologyFunctionsPrxHelper.checkedCast(boxModuleParam.getConnections("Ontology")[0].getFunctions());
            }
            catch
            {
                /*TODO hodit vyjimku, ze nejsou zapojeny oba required sockety*/
                //throw Ferda.Modules.Exceptions.BoxRuntimeError(null, boxModuleParam.gets.getFunctionsIceId,
                  //    "Ontology box and DataTable box must be connected!");
                about = this.getPropertyAbout(valueBefore);
                return valueBefore;
            }

            OntologyStructure ontology = OntologyPrx.getOntology();

            about = resManager.GetString("OntologyMappingAbout");
            StringT ontologyMapping = (StringT)valueBefore;
            PropertyValue returnValue = new PropertyValue();
            PropertyValue propertyValue = valueBefore;

            OntologyMappingControl control =
                new OntologyMappingControl(
                    DataTablePrx.getColumnExplainSeq(),
                    ontology,
                    localePrefs,
                    ownerOfAddIn
                );

            control.ShowInTaskbar = false;
            control.Disposed += new EventHandler(control_Disposed);
            System.Windows.Forms.DialogResult result = this.ownerOfAddIn.ShowDialog(control);

            if (result == System.Windows.Forms.DialogResult.OK)
            {

                /*IntTI numberOfMappedPairs = new IntTI();
                
                numberOfMappedPairs.intValue = 5;

                System.Windows.Forms.MessageBox.Show("A" + ((IntT)numberOfMappedPairs).getStringValue());
                boxModuleParam.setProperty("NumberOfMappedPairs", (IntT)numberOfMappedPairs);
                System.Windows.Forms.MessageBox.Show("B");
                */
                //StringT tmpstr = new StringTI();
                //tmpstr.stringValue = "jde to";
                //System.Windows.Forms.MessageBox.Show("A" + ((IntT)numberOfMappedPairs).getStringValue());
                
                //boxModuleParam.setProperty("Mapping", tmpstr);
                //ontologyMapping.stringValue = "nazdar";
                
                //boxModuleParam.setProperty("Mapping", ontologyMapping);
                
                //System.Windows.Forms.MessageBox.Show("mmntik");
                //ontologyMapping.stringValue = "druhy nastaveni";
                ontologyMapping.stringValue = this.returnString;
                PropertyValue resultValue = ontologyMapping;

                about = this.getPropertyAbout(resultValue);
                propertyValue = resultValue;
            }
            else
            {
                about = this.getPropertyAbout(valueBefore);
                return valueBefore;
            }

            return propertyValue;
            
        }

        #endregion


        #region Other private methods

        /// <summary>
        /// Handler of disposing event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void control_Disposed(object sender, EventArgs e)
        {
            Ferda.FrontEnd.AddIns.OntologyMapping.OntologyMappingControl control =
                (Ferda.FrontEnd.AddIns.OntologyMapping.OntologyMappingControl)sender;

            this.returnString = control.ReturnMapping;
        }

        #endregion
    }
}
