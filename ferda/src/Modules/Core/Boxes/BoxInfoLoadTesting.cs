using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Ferda.Modules.Boxes
{
    /// <summary>
    /// Contains methods for static testing of box info xml configuration
    /// files and xml localization files.
    /// </summary>
    public static class BoxInfoLoadTesting
    {
        /// <summary>
        /// Tests invocations of basic methods supported by the box info.
        /// </summary>
        /// <param name="boxInfo">The box info.</param>
        /// <param name="localePrefs">The locale preferences.</param>
        public static void Test(BoxInfo boxInfo, string[] localePrefs)
        {
            object dummy;
            dummy = boxInfo.Identifier;
            string[] categories = boxInfo.Categories;
            dummy = boxInfo.GetBoxModuleFunctionsIceIds();
            dummy = boxInfo.Design;
            //boxInfo.GetHelpFile(identifier);
            dummy = boxInfo.Icon;
            foreach (string localePref in localePrefs)
            {
                string[] locales = new string[] { localePref };
                foreach (string category in categories)
                {
                    dummy = boxInfo.GetBoxCategoryLocalizedName(localePref, category);
                }
                dummy = boxInfo.GetHint(locales);
                dummy = boxInfo.GetLabel(locales);
                Debug.Assert(!String.IsNullOrEmpty((string)dummy));

                dummy = boxInfo.GetActions(locales);
                dummy = boxInfo.GetProperties(locales);
                dummy = boxInfo.GetSockets(locales);
                //boxInfo.GetHelpFileInfoSeq(localePrefs)

                dummy = boxInfo.getModulesAskingForCreationNonDynamic(locales);
            }
        }
    }
}
