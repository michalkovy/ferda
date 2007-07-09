// Functions.cs - Function objects for the ClassOfEquivalence box module
//
// Author: Tomáš Kuchaø <tomas.kuchar@gmail.com>
// Documented by: Martin Ralbovský <martin.ralbovsky@gmail.com>
//
// Copyright (c) 2006 Tomáš Kuchaø, Martin Ralbovský
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

using System.Collections.Generic;
using Ferda.Guha.MiningProcessor;
using Ice;

namespace Ferda.Modules.Boxes.GuhaMining.ClassOfEquivalence
{
    /// <summary>
    /// Class is providing ICE functionality of the ClassOfEquivalence
    /// box module
    /// </summary>
    internal class Functions : EquivalenceClassFunctionsDisp_, IFunctions
    {
        /// <summary>
        /// The box module.
        /// </summary>
        protected BoxModuleI _boxModule;

        //protected IBoxInfo _boxInfo;

        #region IFunctions Members

        /// <summary>
        /// Sets the <see cref="T:Ferda.Modules.BoxModuleI">box module</see>
        /// and the <see cref="T:Ferda.Modules.Boxes.IBoxInfo">box info</see>.
        /// </summary>
        /// <param name="boxModule">The box module.</param>
        /// <param name="boxInfo">The box info.</param>
        public void setBoxModuleInfo(BoxModuleI boxModule, IBoxInfo boxInfo)
        {
            _boxModule = boxModule;
            //_boxInfo = boxInfo;
        }

        #endregion

        #region Properties

        //names of the sockets
        public const string SockBooleanAttributeSetting = "BooleanAttributeSetting";

        #endregion

        #region Methods

        public List<BooleanAttributeSettingFunctionsPrx> GetBooleanAttributeSettingFunctionsPrxs(bool fallOnError)
        {
            return SocketConnections.GetPrxs<BooleanAttributeSettingFunctionsPrx>(
                _boxModule,
                SockBooleanAttributeSetting,
                BooleanAttributeSettingFunctionsPrxHelper.checkedCast,
                false,
                fallOnError);
        }

        #endregion

        #region Ice Functions

        /// <summary>
        /// Returns IDs of atoms that are in the equivalence class represented
        /// by the box. 
        /// </summary>
        /// <param name="current__">ICE stuff</param>
        /// <returns>IDs of atom in this equivalence class</returns>
        public override GuidStruct[] GetEquivalenceClass(Current current__)
        {
            List<BooleanAttributeSettingFunctionsPrx> booleanAttributes = GetBooleanAttributeSettingFunctionsPrxs(true);
            List<GuidStruct> result = new List<GuidStruct>();
            foreach (BooleanAttributeSettingFunctionsPrx prx in booleanAttributes)
            {
                result.Add(prx.GetEntitySetting().id);
            }
            return result.ToArray();
        }

        #endregion
    }
}