// BoxModuleFactoryI.cs - creator of factory for boxes on Ferda modules side
//
// Authors: 
//   Michal Kováč <michal.kovac.develop@centrum.cz>
//   Tomáš Kuchař <tomas.kuchar@gmail.com>
//
// Copyright (c) 2005 Michal Kováč, Tomáš Kuchař 
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
using Ferda.Modules.Boxes;

namespace Ferda.Modules
{
    /// <summary>
    /// Representation of creator of factory for boxes on Ferda modules side
    /// </summary>
    public class BoxModuleFactoryCreatorI : BoxModuleFactoryCreatorDisp_
    {
        /// <summary>
        /// Thread for testing factory`s meaning of live.
        /// </summary>
        /// <remarks>
        /// Newly created factories are added to the 
        /// <see cref="T:Ferda.Modules.ReapThread"/>
        /// which periodically tests if the factories are 
        /// still used, if not they are destroyed.
        /// </remarks>
        private ReapThread reaper;

        /// <summary>
        /// Class providing some fundamental functionality
        /// of the box module required by <b>Modules Manager</b>.
        /// </summary>
        /// <remarks>
        /// The <see cref="T:Ferda.Modules.Boxex.IBoxInfo"/> provides 
        /// some fundamental functionality so if you are developing 
        /// new box module you don`t have to bother about implementing the 
        /// <b>Factory Creator</b> moreover if you are using e.g. 
        /// <see cref="T:Ferda.Modules.Boxex.BoxInfo"/> implementatiion of
        /// the <see cref="T:Ferda.Modules.Boxex.IBoxInfo"/> interface you
        /// don`t need to understand the theory about <b>Factory Creators</b>
        /// and <b>Factories</b> in practice.
        /// </remarks>
        private IBoxInfo boxInfo;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Ferda.Modules.BoxModuleFactoryCreatorI"/> class.
        /// </summary>
        /// <param name="boxInfo">The box info.</param>
        /// <param name="reapThread">The reap thread.</param>
        public BoxModuleFactoryCreatorI(IBoxInfo boxInfo, ReapThread reapThread)
        {
            this.boxInfo = boxInfo;
            this.reaper = reapThread;
        }

        /// <summary>
        /// Creates the box module factory.
        /// </summary>
        /// <param name="localePrefs">The localization preferences.</param>
        /// <param name="manager">The modules manager engine.</param>
        /// <param name="__current">The Ice.Current.</param>
        /// <returns>
        /// The <see cref="Ferda.Modules.BoxModuleFactoryPrx">proxy</see> of the 
        /// <see cref="T:Ferda.Modules.BoxModuleFactoryI">box module factory</see>.
        /// </returns>
        /// <seealso cref="T:Ferda.Modules.BoxModuleFactoryI"/>
        public override BoxModuleFactoryPrx createBoxModuleFactory(string[] localePrefs, Ferda.ModulesManager.ManagersEnginePrx manager, Ice.Current __current)
        {
            BoxModuleFactoryCreatorPrx myProxy = BoxModuleFactoryCreatorPrxHelper.uncheckedCast(__current.adapter.addWithUUID(this));
            BoxModuleFactoryI boxModuleFactory = new BoxModuleFactoryI(this.boxInfo, myProxy, localePrefs, manager);
            BoxModuleFactoryPrx boxModuleFactoryPrx = BoxModuleFactoryPrxHelper.uncheckedCast(__current.adapter.addWithUUID(boxModuleFactory));
            this.reaper.Add(boxModuleFactoryPrx, boxModuleFactory);
            return boxModuleFactoryPrx;
        }

        /// <summary>
        /// Array of <see cref="T:System.String">Strings</see> 
        /// as list of names of categories, in which this 
        /// box module belongs to.
        /// </summary>
        /// <param name="__current">The Ice.Current.</param>
        /// <returns>
        /// Names of categories, in which the box module belongs to.
        /// </returns>
        /// <remarks>
        /// <para>
        /// Box module can be in any number of categories.
        /// </para>
        /// <para>
        /// These names are not localized  i.e. the name of 
        /// the category is an identifier of the category.
        /// </para>
        /// <para>
        /// For localization of this identifiers use 
        /// <see cref="M:Ferda.Modules.Boxes.IBoxInfo.GetBoxCategoryLocalizedName(System.String,System.String)"/>.
        /// </para>
        /// </remarks>
        /// <seealso cref="M:Ferda.Modules.Boxes.IBoxInfo.GetBoxCategoryLocalizedName(System.String,System.String)"/>
        public override string[] getBoxCategories(Ice.Current __current)
        {
            return this.boxInfo.Categories;
        }

        /// <summary>
        /// Gets the localized name of the box`s category.
        /// </summary>
        /// <param name="locale">Name of the culture i.e. (one) localization prefrence.</param>
        /// <param name="categoryName">Name of the category.</param>
        /// <param name="__current">The Ice.Current.</param>
        /// <returns>
        /// Localized name of the category named <c>categoryName</c>.
        /// </returns>
        public override string[] getBoxCategoryLocalizedName(string locale, string categoryName, Ice.Current __current)
        {
            string localizedCategoryName = this.boxInfo.GetBoxCategoryLocalizedName(locale, categoryName);
            if (String.IsNullOrEmpty(localizedCategoryName))
                return new string[0];
            else
                return new string[] { localizedCategoryName };
        }

        /// <summary>
        /// Gets function`s Ice identifiers of the box module.
        /// </summary>
        /// <param name="__current">The Ice.Current.</param>
        /// <returns>
        /// An array of strings representing Ice identifiers 
        /// of the box module`s functions.
        /// </returns>
        public override string[] getBoxModuleFunctionsIceIds(Ice.Current __current)
        {
            return this.boxInfo.GetBoxModuleFunctionsIceIds();
        }

        /// <summary>
        /// Gets the <see href="http://www.w3.org/tr/2000/cr-svg-20001102/index.html">
        /// Scalable Vector Graphics (SVG)</see> design.
        /// </summary>
        /// <param name="__current">The Ice.Current.</param>
        /// <returns>The string representation of SVG design file.</returns>
        public override string getDesign(Ice.Current __current)
        {
            return this.boxInfo.Design;
        }

        /// <summary>
        /// Gets localized hint (short suggestion) of the box module.
        /// </summary>
        /// <param name="localePrefs">The localization preferences.</param>
        /// <param name="__current">The Ice.Current.</param>
        /// <returns>Localized hint (short suggestion) of the box module.</returns>
        public override string getHint(string[] localePrefs, Ice.Current __current)
        {
            return this.boxInfo.GetHint(localePrefs);
        }

        /// <summary>
        /// Gets help file as aray of <see cref="T:System.Byte">Bytes</see>.
        /// </summary>
        /// <param name="identifier">The identifier of the help file.</param>
        /// <param name="__current">The Ice.Current.</param>
        /// <returns>Content of the help file as array of <see cref="T:System.Byte">Bytes</see>.</returns>
        public override byte[] getHelpFile(string identifier, Ice.Current __current)
        {
            return this.boxInfo.GetHelpFile(identifier);
        }

        /// <summary>
        /// Gets the box module`s icon.
        /// </summary>
        /// <param name="__current">The Ice.Current.</param>
        /// <returns>
        /// The box module`s icon i.e. content of the "*.ico" file 
        /// as array of <see cref="T:System.Byte">Bytes</see>.
        /// </returns>
        public override byte[] getIcon(Ice.Current __current)
        {
            return this.boxInfo.Icon;
        }

        /// <summary>
        /// The identifier of the box module`s type. It has to be unique!
        /// </summary>
        /// <param name="__current">The Ice.Current.</param>
        /// <returns>
        /// The identifier of the box module`s type. Please remember that the 
        /// identifier is used for identification of the box module type/kind 
        /// so that if new instance of some box module`s type wants be created 
        /// the <see cref="T:Ferda.Modules.BoxModuleFactoryCreatorI"/>
        /// of <see cref="T:Ferda.Modules.BoxModuleFactoryI"/> with the specified 
        /// <see cref="M:Ferda.Modules.BoxModuleFactoryCreatorI.getIdentifier(Ice.Current)">identifier</see> 
        /// i.e. type is used.
        /// </returns>
        public override string getIdentifier(Ice.Current __current)
        {
            return this.boxInfo.Identifier;
        }

        /// <summary>
        /// Gets localized label of the box module.
        /// </summary>
        /// <param name="localePrefs">The localization preferences.</param>
        /// <param name="__current">The Ice.Current.</param>
        /// <returns>Localized label of the box module.</returns>
        public override string getLabel(string[] localePrefs, Ice.Current __current)
        {
            return this.boxInfo.GetLabel(localePrefs);
        }
    }
}
