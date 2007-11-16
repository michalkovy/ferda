// Functions.cs - Function objects for the DisjunctionSetting box module
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

using System;
using System.Collections.Generic;
using Ferda.Guha.MiningProcessor;
using Ice;

namespace Ferda.Modules.Boxes.GuhaMining.DisjunctionSetting
{
    /// <summary>
    /// Class is providing ICE functionality of the DisjunctionSetting
    /// box module
    /// </summary>
    internal class Functions : BooleanAttributeSettingFunctionsDisp_, IFunctions
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

        //names of the properties
        public const string PropImportance = "Importance";
        public const string PropMinimalLength = "MinimalLength";
        public const string PropMaximalLength = "MaximalLength";
        public const string SockBooleanAttributeSetting = "BooleanAttributeSetting";

        /// <summary>
        /// The GUID identifier of the atom
        /// </summary>
        public GuidStruct Guid
        {
            get { return BoxInfoHelper.GetGuidStructFromProperty("Guid", _boxModule); }
        }

        /// <summary>
        /// Importance of the atom (basic/auxiliary/forced)
        /// </summary>
        public ImportanceEnum Importance
        {
            get
            {
                return (ImportanceEnum) Enum.Parse(
                                            typeof (ImportanceEnum),
                                            _boxModule.GetPropertyString(PropImportance)
                                            );
            }
        }

        /// <summary>
        /// Minimal length of the atom
        /// </summary>
        public int MinimalLength
        {
            get { return _boxModule.GetPropertyInt(PropMinimalLength); }
        }

        /// <summary>
        /// Maximal length of the atom
        /// </summary>
        public int MaximalLength
        {
            get { return _boxModule.GetPropertyInt(PropMaximalLength); }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Returns proxies of boxes connected to the Boolean attribute setting
        /// socket
        /// </summary>
        /// <param name="fallOnError">If the function should throw an exception
        /// when error</param>
        /// <returns>Proxies of connected boxes</returns>
        public List<BooleanAttributeSettingFunctionsPrx> GetBooleanAttributeSettingFunctionsPrxs(bool fallOnError)
        {
            return SocketConnections.GetPrxs<BooleanAttributeSettingFunctionsPrx>(
                _boxModule,
                SockBooleanAttributeSetting,
                BooleanAttributeSettingFunctionsPrxHelper.checkedCast,
                true,
                fallOnError);
        }

        /// <summary>
        /// Gets the label of the boxes connected to the Boolean Attribute setting socket
        /// </summary>
        /// <returns>Connected boxes labels</returns>
        public string[] GetInputBoxesLabels()
        {
            return SocketConnections.GetInputBoxesLabels(_boxModule, SockBooleanAttributeSetting);
        }

        /// <summary>
        /// Returns the setting of the entity (all Boolean attributes). The
        /// setting <see cref="T:Ferda.Guha.MiningProcessor.IEntitySetting"/>
        /// contains identification information and the importance
        /// of this entity (BooleanAttribute). 
        /// </summary>
        /// <param name="fallOnError">If the function should throw an exception
        /// when error</param>
        /// <returns>The entity setting</returns>
        public IEntitySetting GetEntitySetting(bool fallOnError)
        {
            return ExceptionsHandler.GetResult<IEntitySetting>(
                fallOnError,
                delegate
                    {
                        List<BooleanAttributeSettingFunctionsPrx> subFormulas =
                            GetBooleanAttributeSettingFunctionsPrxs(fallOnError);
                        if (subFormulas == null)
                            return null;
                        else
                        {
                            DisjunctionSettingI result =
                                new DisjunctionSettingI();
                            result.id = Guid;
                            result.importance = Importance;
                            result.maxLength = MaximalLength;
                            result.minLength = MinimalLength;
                            List<IEntitySetting> operands = new List<IEntitySetting>();
                            foreach (BooleanAttributeSettingFunctionsPrx prx in subFormulas)
                            {
                                operands.Add(prx.GetEntitySetting());
                            }
                            result.operands = operands.ToArray();
                            return result;
                        }
                    },
                delegate
                    {
                        return null;
                    },
                _boxModule.StringIceIdentity
                );
        }

        #endregion

        #region Ice Functions

        /// <summary>
        /// Returns the setting of the entity (all Boolean attributes). The
        /// setting <see cref="T:Ferda.Guha.MiningProcessor.IEntitySetting"/>
        /// contains identification information and the importance
        /// of this entity (BooleanAttribute). 
        /// </summary>
        /// <param name="current__">ICE stuff</param>
        /// <returns>The entity setting</returns>
        public override IEntitySetting GetEntitySetting(Current current__)
        {
            return GetEntitySetting(true);
        }

        /// <summary>
        /// Returns attribute names (overridden from 
        /// <see cref="T:Ferda.Guha.MiningProcessor.AttributeNameProvider"/>)
        /// of this boolean attribute and their identification.
        /// </summary>
        /// <param name="current__">ICE stuff</param>
        /// <returns>Attribute names</returns>
        public override GuidAttributeNamePair[] GetAttributeNames(Current current__)
        {
            List<GuidAttributeNamePair> result = new List<GuidAttributeNamePair>();
            List<BooleanAttributeSettingFunctionsPrx> subFormulas =
                GetBooleanAttributeSettingFunctionsPrxs(true);
            foreach (BooleanAttributeSettingFunctionsPrx prx in subFormulas)
            {
                result.AddRange(prx.GetAttributeNames());
            }
            return result.ToArray();
        }

        /// <summary>
        /// Returns the proxy of the bit string generator
        /// (overridden from <see cref="T:Ferda.Guha.MiningProcessor.BitStringGeneratorProvider"/>)
        /// a specified attribute.
        /// </summary>
        /// <param name="attributeGuid">ID of the specified attribute.</param>
        /// <param name="current__">ICE stuff</param>
        /// <returns>Proxy of the bit string generator</returns>
        public override BitStringGeneratorPrx GetBitStringGenerator(GuidStruct attributeGuid, Current current__)
        {
            BitStringGeneratorPrx result;
            List<BooleanAttributeSettingFunctionsPrx> subFormulas =
                GetBooleanAttributeSettingFunctionsPrxs(true);
            foreach (BooleanAttributeSettingFunctionsPrx prx in subFormulas)
            {
                result = prx.GetBitStringGenerator(attributeGuid);
                if (result != null)
                    return result;
            }
            return null;
        }

        /// <summary>
        /// Gets the ID of the table that is beeing mined.
        /// </summary>
        /// <param name="current__">ICE stuff</param>
        /// <returns>ID of the mined table</returns>
        public override string GetSourceDataTableId(Current current__)
        {
            List<BooleanAttributeSettingFunctionsPrx> prxs = GetBooleanAttributeSettingFunctionsPrxs(true);
            string last = null;
            if (prxs != null)
                foreach (BooleanAttributeSettingFunctionsPrx prx in prxs)
                {
                    string newer = prx.GetSourceDataTableId();
                    if (String.IsNullOrEmpty(last))
                        last = newer;
                        /*
                    else if (last != newer)
                        throw Exceptions.BadValueError(null, _boxModule.StringIceIdentity,
                                                       "Mining over only source data table is supported.",
                                                       new string[] {SockBooleanAttributeSetting},
                                                       restrictionTypeEnum.OtherReason);*/
                }
            return last;
        }

        #endregion
    }
}