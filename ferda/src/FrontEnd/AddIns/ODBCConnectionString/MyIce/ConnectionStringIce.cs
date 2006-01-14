using System;
using System.Collections.Generic;
using System.Text;
using Ferda.Modules;
using Ferda.Modules.Boxes;
using Ferda.ModulesManager;
using System.Resources;
using System.Reflection;

namespace Ferda.FrontEnd.AddIns.ODBCConnectionString.MyIce
{
    class ConnectionStringIce : SettingModuleWithStringAbilityDisp_
    {
        #region Private variables
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
        /// Resulting DSN string
        /// </summary>
        string returnString;

        #endregion


        #region Constructor
        public ConnectionStringIce(Ferda.FrontEnd.AddIns.IOwnerOfAddIn ownerOfAddIn)
        {
            this.ownerOfAddIn = ownerOfAddIn;

            //setting the ResManager resource manager and localization string
            resManager = new ResourceManager("Ferda.FrontEnd.AddIns.ODBCConnectionString.Localization_en-US",
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

                locale = "Ferda.FrontEnd.AddIns.ODBCConnectionString.Localization_" + locale;

                resManager = new ResourceManager(locale,
            Assembly.GetExecutingAssembly());

            }

            catch
            {
            }
            return resManager.GetString("ConnectionString");
           // return "TEST";
        }

        /*public override string getPropertyAbout(PropertyValue value, global::Ice.Current current__)
        {
            return resManager.GetString("ConnectionStringAbout");
           // return "TEST";
        }*/

        public override string getPropertyAbout(PropertyValue value, global::Ice.Current current__)
        {
            return ((StringT)value).getStringValue();
        }


        public override string getIdentifier(global::Ice.Current current__)
        {
            return "ODBCConnectionString";
        }

        #endregion


        public override PropertyValue run(PropertyValue valueBefore, BoxModulePrx boxModuleParam, string[] localePrefs, ManagersEnginePrx manager, out string about, global::Ice.Current current__)
        {

            string locale;
            try
            {
                locale = localePrefs[0];

                localizationString = locale;

                locale = "Ferda.FrontEnd.AddIns.ODBCConnectionString.Localization_" + locale;

                resManager = new ResourceManager(locale,
            Assembly.GetExecutingAssembly());

            }

            catch
            {
            }

            about = resManager.GetString("ConnectionStringAbout");

            StringT connectionString = (StringT)valueBefore;
            PropertyValue returnValue = new PropertyValue();
            PropertyValue propertyValue = valueBefore;


            Ferda.FrontEnd.AddIns.ODBCConnectionString.ODBCConnectionStringControl listView = new Ferda.FrontEnd.AddIns.ODBCConnectionString.ODBCConnectionStringControl(localePrefs, connectionString.getStringValue());
            listView.ShowInTaskbar = false;
            listView.Disposed += new EventHandler(listView_Disposed);

            System.Windows.Forms.DialogResult result = this.ownerOfAddIn.ShowDialog(listView);

            if (result == System.Windows.Forms.DialogResult.OK)
            {
                connectionString.stringValue = this.returnString;
                PropertyValue resultValue = connectionString;
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

        void listView_Disposed(object sender, EventArgs e)
        {
               Ferda.FrontEnd.AddIns.ODBCConnectionString.ODBCConnectionStringControl listView =
                   (Ferda.FrontEnd.AddIns.ODBCConnectionString.ODBCConnectionStringControl) sender;

               this.returnString = listView.ReturnString;

        }





        public override PropertyValue convertFromStringAbout(string about, string[] localePrefs, Ice.Current current__)
        {
            return new StringTI(about);
        }
    }
}
