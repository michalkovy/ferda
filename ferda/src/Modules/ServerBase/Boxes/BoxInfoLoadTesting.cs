// BoxInfoLoadTesting.cs - Static methods for testing a box xml configuration
//
// Author: Tomáš Kuchaø <tomas.kuchar@gmail.com>
//
// Copyright (c) 2006 Tomáš Kuchaø
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
