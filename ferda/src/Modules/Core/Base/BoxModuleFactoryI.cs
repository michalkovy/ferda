// BoxModuleFactoryI.cs - factory for boxes on Ferda modules side
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
using Ferda;
using System.Diagnostics;
using Ferda.Modules.Boxes;

namespace Ferda.Modules
{
    /// <summary>
    /// Representation of factory for boxes on Ferda modules side
    /// </summary>
    public class BoxModuleFactoryI : BoxModuleFactoryDisp_
    {
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
        /// Localization preferences.
        /// </summary>
        /// <remarks>
        /// For further information about the localization please
        /// see remarks for <see cref="T:Ferda.Modules.Boxes.IBoxInfo"/>.
        /// </remarks>
        private string[] localePrefs;

        /// <summary>
        /// The <see cref="Ferda.Modules.BoxModuleFactoryCreatorPrx">proxy</see> of the 
        /// <see cref="T:Ferda.Modules.BoxModuleFactoryCreatorI">
        /// box module factory`s creator</see>.
        /// </summary>
        private BoxModuleFactoryCreatorPrx myFactoryCreatorProxy;

        /// <summary>
        /// The <see cref="Ferda.ModulesManager.ManagersEnginePrx">proxy</see> of the 
        /// <see cref="T:Ferda.ModulesManager.ManagersEngineI">
        /// modules manager engine</see>.
        /// </summary>
        private ModulesManager.ManagersEnginePrx manager;

        /// <summary>
        /// Box modules created in the factory.
        /// </summary>
        /// <remarks>
        /// <para>Key: string ice identity.</para>
        /// <para>Value: box module.</para>
        /// </remarks>
        private Dictionary<string, BoxModuleI> boxModules =
            new Dictionary<string, BoxModuleI>();

        //private ModulesManager.OutputPrx output;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Ferda.Modules.BoxModuleFactoryI"/> class.
        /// </summary>
        /// <param name="boxInfo">The box info.</param>
        /// <param name="myFactoryCreatorProxy">The proxy of this factory`s creator.</param>
        /// <param name="localePrefs">The localization preferences.</param>
        /// <param name="manager">The modules manager engine.</param>
        public BoxModuleFactoryI(IBoxInfo boxInfo,
            BoxModuleFactoryCreatorPrx myFactoryCreatorProxy,
            string[] localePrefs,
            ModulesManager.ManagersEnginePrx manager)
        {
            this.boxInfo = boxInfo;
            this.localePrefs = localePrefs;
            this.myFactoryCreatorProxy = myFactoryCreatorProxy;
            this.manager = manager;

            //this.output = this.manager.getOutputInterface();
        }

        /// <summary>
        /// The last time when the session was refreshed.
        /// </summary>
        private System.DateTime lastRefresh;

        /// <summary>
        /// Gets time of the session`s last refresh.
        /// </summary>
        /// <value>The last time when the session was refreshed.</value>
        /// <exception cref="T:Ice.ObjectNotExistException">
        /// Thrown iff factory is destroying or destroyed.
        /// </exception>
        public System.DateTime LastRefresh
        {
            get
            {
                if (_destroy)
                {
                    throw new Ice.ObjectNotExistException();
                }
                return lastRefresh;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="T:Ferda.Modules.BoxModuleFactoryI"/> is destroying.
        /// </summary>
        /// <value><c>true</c> if factory is destroying; otherwise, <c>false</c>.</value>
        private bool _destroy;

        /// <summary>
        /// Creates new box module in the factory.
        /// </summary>
        /// <param name="__current">The Ice.Current.</param>
        /// <returns>
        /// The <see cref="T:Ferda.Modules.BoxModulePrx">proxy</see> of
        /// the newly created <see cref="T:Ferda.Modules.BoxModuleI">box module</see>.
        /// </returns>
        /// <exception cref="T:Ice.ObjectNotExistException">
        /// Thrown iff factory is destroying or destroyed.
        /// </exception>
        public override BoxModulePrx createBoxModule(Ice.Current __current)
        {
            if (_destroy)
            {
                throw new Ice.ObjectNotExistException();
            }
            Ice.Identity boxModuleIdentity = Ice.Util.stringToIdentity(Ice.Util.generateUUID());
            BoxModuleFactoryPrx myProxy = BoxModuleFactoryPrxHelper.uncheckedCast(__current.adapter.addWithUUID(this));
            BoxModuleI boxModule = new BoxModuleI(this.boxInfo, boxModuleIdentity, myProxy, this.manager, __current.adapter, this.localePrefs);
            BoxModulePrx boxModulePrx = boxModule.MyProxy;
            string boxIdentity = Ice.Util.identityToString(boxModulePrx.ice_getIdentity());
            this.boxModules[boxIdentity] = boxModule;
            return boxModulePrx;
        }

        /// <summary>
        /// Destroys the box module with specified <c>boxIdentity</c>.
        /// </summary>
        /// <param name="boxIdentity">The identity of the box.</param>
        /// <param name="__current">The Ice.Current.</param>
        public override void destroyBoxModule(string boxIdentity, Ice.Current __current)
        {
            lock (this.boxModules)
            {
                this.boxModules[boxIdentity].destroy(__current.adapter);
                this.boxModules.Remove(boxIdentity);
                __current.adapter.remove(Ice.Util.stringToIdentity(boxIdentity));
            }
        }

        /// <summary>
        /// Destroys the factory.
        /// </summary>
        /// <param name="__current">The Ice.Current.</param>
        /// <remarks>This method should be called among others 
        /// on shutdown of the server.</remarks>
        /// <exception cref="T:Ice.ObjectNotExistException">
        /// Thrown iff factory is destroying or destroyed.
        /// </exception>
        public override void destroy(Ice.Current __current)
        {
            if (_destroy)
            {
                throw new Ice.ObjectNotExistException();
            }

            _destroy = true;

            lock (this.boxModules)
            {
                try
                {
                    __current.adapter.remove(__current.id);
                    foreach (string boxIdentity in boxModules.Keys)
                    {
                        __current.adapter.remove(Ice.Util.stringToIdentity(boxIdentity));
                    }
                }
                catch (Ice.ObjectAdapterDeactivatedException)
                {
                    // This method is called on shutdown of the server, in which
                    // case this exception is expected.
                }
            }
        }

        /// <summary>
        /// Gets (localized) actions of the box module.
        /// </summary>
        /// <param name="__current">The Ice.Current.</param>
        /// <returns>Array of <see cref="T:Ferda.Modules.ActionInfo"/>.</returns>
        public override ActionInfo[] getActions(Ice.Current __current)
        {
            return this.boxInfo.GetActions(this.localePrefs);
        }

        /// <summary>
        /// Gets (localized) properties of the box module.
        /// </summary>
        /// <param name="__current">The Ice.Current.</param>
        /// <returns>
        /// Array of <seealso cref="T:Ferda.Modules.PropertyInfo">PropertyInfos</seealso>.
        /// </returns>
        public override PropertyInfo[] getProperties(Ice.Current __current)
        {
            return this.boxInfo.GetProperties(this.localePrefs);
        }

        /// <summary>
        /// Gets (localized) sockets of the box module.
        /// </summary>
        /// <param name="__current">The Ice.Current.</param>
        /// <returns>
        /// Array of <seealso cref="T:Ferda.Modules.SocketInfo">SocketInfos</seealso>.
        /// </returns>
        public override SocketInfo[] getSockets(Ice.Current __current)
        {
            SocketInfo[] s = this.boxInfo.GetSockets(this.localePrefs);
            return s;
        }

        /// <summary>
        /// Refreshes the factory i.e. update its 
        /// <see cref="F:Ferda.Modules.BoxModuleFactoryI.lastRefresh"/>
        /// timestamp to <see cref="P:System.DateTime.Now"/>.
        /// </summary>
        /// <param name="__current">The Ice.Current.</param>
        /// <exception cref="T:Ice.ObjectNotExistException">
        /// Thrown iff factory is destroying or destroyed.
        /// </exception>
        public override void refresh(Ice.Current __current)
        {
            if (_destroy)
            {
                throw new Ice.ObjectNotExistException();
            }
            lastRefresh = System.DateTime.Now;
        }

        /// <summary>
        /// Destroys the factory if it is empty.
        /// </summary>
        /// <param name="__current">The Ice.Current.</param>
        /// <returns>True iff the factory is empty (i.e. there
        /// is no box module in within) and therefore the factory
        /// can be and is destroyed.</returns>
        public override bool destroyIfEmpty(Ice.Current __current)
        {
            lock (this.boxModules)
            {
                if (this.boxModules.Count == 0)
                {
                    this.destroy(__current);
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Gets information about the (localized) help files.
        /// </summary>
        /// <param name="__current">The Ice.Current.</param>
        /// <returns>
        /// Array of <seealso cref="T:Ferda.Modules.HelpFileInfo">HelpFileInfos</seealso>.
        /// </returns>
        public override HelpFileInfo[] getHelpFileInfoSeq(Ice.Current __current)
        {
            return this.boxInfo.GetHelpFileInfoSeq(this.localePrefs);
        }

        /// <summary>
        /// Gets the factory creator.
        /// </summary>
        /// <param name="__current">The Ice.Current.</param>
        /// <returns>
        /// The <see cref="Ferda.Modules.BoxModuleFactoryCreatorPrx">proxy</see> of the 
        /// <see cref="T:Ferda.Modules.BoxModuleFactoryCreatorI">
        /// box module factory`s creator</see>.
        /// </returns>
        public override BoxModuleFactoryCreatorPrx getMyFactoryCreator(Ice.Current __current)
        {
            return this.myFactoryCreatorProxy;
        }
    }
}
