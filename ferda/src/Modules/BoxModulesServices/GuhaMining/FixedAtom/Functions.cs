// Functions.cs - Function objects for the FixedAtom box module
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
using Ferda.Guha.MiningProcessor;
using Ice;

namespace Ferda.Modules.Boxes.GuhaMining.FixedAtom
{
    /// <summary>
    /// Class is providing ICE functionality of the FixedAtom
    /// box module
    /// </summary>
    public class Functions : BooleanAttributeSettingFunctionsDisp_, IFunctions
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

        //names of properties
        public const string PropImportance = "Importance";
        public const string PropCategories = "Categories";
        public const string SockBitStringGenerator = "BitStringGenerator";

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
        /// Names of categories contained in the atom
        /// </summary>
        public string[] Categories
        {
            get { return _boxModule.GetPropertyStringSeq(PropCategories); }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Returns the proxy of the bit string generator of the
        /// attribute of the box.
        /// </summary>
        /// <param name="fallOnError">If the function should throw an exception
        /// when error</param>
        /// <returns>Proxy of the bit string generator</returns>
        public BitStringGeneratorPrx GetBitStringGeneratorPrx(bool fallOnError)
        {
            return SocketConnections.GetPrx<BitStringGeneratorPrx>(
                _boxModule,
                SockBitStringGenerator,
                BitStringGeneratorPrxHelper.checkedCast,
                fallOnError);
        }

        /// <summary>
        /// Gets the categories ids (same as names of the connected attribute).
        /// The name are used as ids, because they have to be unique. The x category\
        /// is ommited. 
        /// </summary>
        /// <param name="fallOnError">If the function should throw an exception
        /// when error</param>
        /// <returns>Categories IDs (names).</returns>
        public string[] GetCategoriesIds(bool fallOnError)
        {
            return ExceptionsHandler.GetResult<string[]>(
                fallOnError,
                delegate
                    {
                        BitStringGeneratorPrx bitStringGenerator = GetBitStringGeneratorPrx(fallOnError);
                        if (bitStringGenerator == null)
                            return null;
                        else
                        {
                            return bitStringGenerator.GetCategoriesIds();
                        }
                    },
                delegate
                    {
                        return null;
                    },
                _boxModule.StringIceIdentity
                );
        }

        /// <summary>
        /// Gets the name of the connected attribute
        /// </summary>
        /// <param name="fallOnError">If the function should throw an exception
        /// when error</param>
        /// <returns>Name of the attribute</returns>
        public string GetAttributeName(bool fallOnError)
        {
            return ExceptionsHandler.GetResult<string>(
                fallOnError,
                delegate
                    {
                        BitStringGeneratorPrx bitStringGenerator = GetBitStringGeneratorPrx(fallOnError);
                        if (bitStringGenerator == null)
                            return null;
                        else
                        {
                            GuidAttributeNamePair[] attribNames = bitStringGenerator.GetAttributeNames();
                            string attribId = bitStringGenerator.GetAttributeId().value;
                            if (attribNames.Length == 1 && attribNames[0].id.value == attribId)
                                return attribNames[0].attributeName;
                            else if (fallOnError)
                                throw new BoxRuntimeError();
                            else return null;
                        }
                    },
                delegate
                    {
                        return null;
                    },
                _boxModule.StringIceIdentity
                );
        }

        /// <summary>
        /// Returns the setting of the entity (all Boolean attributes). The
        /// setting <see cref="T:Ferda.Guha.MiningProcessor.IEntitySetting">
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
                        BitStringGeneratorPrx bitStringGenerator = GetBitStringGeneratorPrx(fallOnError);
                        if (bitStringGenerator == null)
                            return null;
                        else
                        {
                            CoefficientFixedSetSettingI result =
                                new CoefficientFixedSetSettingI();
                            result.id = Guid;
                            result.importance = Importance;
                            result.generator = bitStringGenerator;
                            result.categoriesIds = Categories;
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
        /// setting <see cref="T:Ferda.Guha.MiningProcessor.IEntitySetting">
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
            return GetBitStringGeneratorPrx(true).GetAttributeNames();
        }

        /// <summary>
        /// Returns the proxy of the bit string generator
        /// (overridden from <see cref="T:Ferda.Guha.MiningProcessor.BitStringGeneratorProvider"/>)
        /// a specified attribute.
        /// </summary>
        /// <param name="attributeId">ID of the specified attribute.</param>
        /// <param name="current__">ICE stuff</param>
        /// <returns>Proxy of the bit string generator</returns>
        public override BitStringGeneratorPrx GetBitStringGenerator(GuidStruct attributeId, Current current__)
        {
            if (attributeId.value == Guid.value)
                return GetBitStringGeneratorPrx(true);
            return null;
        }

        /// <summary>
        /// Gets the ID of the table that is beeing mined.
        /// </summary>
        /// <param name="current__">ICE stuff</param>
        /// <returns>ID of the mined table</returns>
        public override string GetSourceDataTableId(Current current__)
        {
            BitStringGeneratorPrx prx = GetBitStringGeneratorPrx(true);
            if (prx != null)
                return prx.GetSourceDataTableId();
            return null;
        }

        #endregion
    }
}