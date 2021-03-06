// Functions.cs - functionality for the Automatic Attribute with fuzzy categories box
//
// Author: Martin Ralbovský <martin.ralbovsky@gmail.cz>
//
// Copyright (c) 2009 Martin Ralbovský
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
using Ferda.Guha.Data;
using System.Data;
using Ferda.Guha.MiningProcessor;
using Ice;

namespace Ferda.Modules.Boxes.DataPreparation.Categorization.AutomaticAttributeWithFuzzyCategories
{
    /// <summary>
    /// Class is providing ICE functionality of the SampleBoxModule
    /// box module
    /// </summary>
    public class Functions : FuzzyAttributeFunctionsDisp_, Ferda.Modules.IFunctions
    {
        #region Fields

        protected Ferda.Modules.BoxModuleI _boxModule;
        protected Ferda.Modules.Boxes.IBoxInfo _boxInfo;

        #endregion

        #region IFunctions Members

        /// <summary>
        /// Sets the <see cref="T:Ferda.Modules.BoxModuleI">box module</see>
        /// and the <see cref="T:Ferda.Modules.Boxes.IBoxInfo">box info</see>.
        /// </summary>
        /// <param name="boxModule">The box module.</param>
        /// <param name="boxInfo">The box info.</param>
        void Ferda.Modules.IFunctions.setBoxModuleInfo(Ferda.Modules.BoxModuleI boxModule, Ferda.Modules.Boxes.IBoxInfo boxInfo)
        {
            this._boxModule = boxModule;
            this._boxInfo = boxInfo;
        }

        #endregion

        #region Properties

        /// <summary>
        /// The GUID (unique identifier) of the attribute
        /// </summary>
        public GuidStruct Guid
        {
            get { return BoxInfoHelper.GetGuidStructFromProperty("Guid", _boxModule); }
        }

        /// <summary>
        /// Returns the cardinality of
        /// </summary>
        public CardinalityEnum Cardinality
        {
            get
            {
                return Public.Cardinality(_boxModule);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the proxy of the connected column
        /// </summary>
        /// <param name="fallOnError">If the function should throw an exception on error</param>
        /// <returns>Proxy of the connected column</returns>
        public ColumnFunctionsPrx GetColumnFunctionsPrx(bool fallOnError)
        {
            return SocketConnections.GetPrx<ColumnFunctionsPrx>(
                _boxModule,
                Public.SockColumn,
                ColumnFunctionsPrxHelper.checkedCast,
                fallOnError);
        }

        #endregion

        #region ICE functions

        /// <summary>
        /// Gets the name of the column from which the attribute and bit string generator
        /// is created. Added for PMML purposes. 
        /// </summary>
        /// <param name="current__">ICE stuff</param>
        /// <returns></returns>
        public string GetColumnName(Current current__)
        {
            return GetColumnFunctionsPrx(true).getColumnInfo().columnSelectExpression;
        }

        /// <summary>
        /// Returns information from the column about the values and frequencies
        /// of the column. This fucntion was added to the Slice desing for
        /// the PMML support.
        /// </summary>
        /// <returns>ValuesAndFrequencies structure</returns>
        public ValuesAndFrequencies GetColumnValuesAndFrequencies(Current current__)
        {
            return GetColumnFunctionsPrx(true).getDistinctsAndFrequencies();
        }

        /// <summary>
        /// Returns the identification of the attribute.
        /// </summary>
        /// <param name="current__">ICE stuff</param>
        /// <returns>Identification of the attribute</returns>
        public GuidStruct GetAttributeId(Current current__)
        {
            return Guid;
        }

        /// <summary>
        /// Returns cardinality of the attribute (nominal/ordinal/cyclic ordinal/cardinal).
        /// </summary>
        /// <param name="current__">ICE stuff</param>
        /// <returns>Attribute cardinality</returns>
        public CardinalityEnum GetAttributeCardinality(Current current__)
        {
            return Cardinality;
        }

        /// <summary>
        /// Returns a count vector for this attribute, given the master data table name,
        /// master and detial key columns. It is used for virtual hypotheses attributes.
        /// The count vector is an array of integers 
        /// representing for each item in the master data table how many records are
        /// in the detail data table corresponding to the item. 
        /// More information can
        /// be found in <c>svnroot/publications/diplomky/Kuzmos/diplomka.pdf</c> or
        /// in <c>svnroot/publications/Icde08/ICDE.pdf</c>.
        /// </summary>
        /// <param name="masterIdColumn">ID of the master data table</param>
        /// <param name="masterDatatableName">Name of the master data table</param>
        /// <param name="detailIdColumn">Detail data table ID column</param>
        /// <param name="current__">ICE stuff</param>
        /// <returns>a count vector</returns>
        public int[] GetCountVector(string masterIdColumn, string masterDatatableName, string detailIdColumn, Current current__)
        {
            throw Exceptions.BoxRuntimeError(null,BoxInfo.typeIdentifier,
                "The attribute with fuzzy categories does not support count vector computing");
        }

        /// <summary>
        /// Gets next bit string of the virtual hypotheses attribute. The virtual
        /// hypotheses attribute does not know the exact number of bit strings
        /// to be generated by the miner. Therefore it returns only the next
        /// bit strings.
        /// More information can
        /// be found in <c>svnroot/publications/diplomky/Kuzmos/diplomka.pdf</c> or
        /// in <c>svnroot/publications/Icde08/ICDE.pdf</c>.
        /// </summary>
        /// <param name="skipFirstN"></param>
        /// <param name="bitString">Bit string to be returned</param>
        /// <returns>True iff there is a next bit string in the output
        /// <paramref name="bitString"/></returns>
        /// <param name="current__">ICE stuff</param>
        public bool GetNextBitString(int skipFirstN, out BitStringIceWithCategoryId bitString, Current current__)
        {
            bitString = new BitStringIceWithCategoryId();
            return false;
        }

        /// <summary>
        /// Returns maximal number of bit strings (verfications) that a 
        /// virtual hypotheses attribute can generate. The number is usually
        /// set via a property in the corresponding virtual attribute box.
        /// </summary>
        /// <param name="current__">ICE stuff</param>
        /// <returns>Maximal number of bit strings</returns>
        public long GetMaxBitStringCount(Current current__)
        {
            return 0;
        }

        /// <summary>
        /// Returns identification (category name) of a
        /// category that contains missing information.
        /// If fuzzy case, a implementation decision is that no fuzzy category
        /// contains a missing information, because the fuzzy categories are
        /// constructed from cardinal domains. If the domain contains a value
        /// that is considered missing (e.g. -1), the domain is not fully
        /// cardinal. 
        /// </summary>
        /// <param name="current__">ICE stuff</param>
        /// <returns>Missing information category name
        /// </returns>
        public string[] GetMissingInformationCategoryId(Current current__)
        {
            return new string[0];
        }

        /// <summary>
        /// <para>
        /// Returns numerical values of the categories. These numerical
        /// values can be returned only for the <c>ordinal, cyclic ordinal
        /// and cardinal</c> attributes. Otherwise, <c>null</c> or
        /// <c>double[0]</c> is returned. The value is returned, only
        /// if the category contains no intervals and only one enumeration.
        /// Otherwise, null is returned.
        /// </para>
        /// <para>
        /// In the fuzzy case, each fuzzy category consists of several crisp
        /// values, therefore numeric value for one category cannot be 
        /// assigned.
        /// </para>
        /// </summary>
        /// <param name="current__">ICE stuff</param>
        /// <returns>
        /// Numerical values of the categories. These numerical
        /// values can be returned only for the <c>ordinal, cyclic ordinal
        /// and cardinal</c> attributes. Otherwise, <c>null</c> or
        /// <c>double[0]</c> is returned
        /// </returns>
        public double[] GetCategoriesNumericValues(Current current__)
        {
            return new double[0];
        }

        /// <summary>
        /// <para>
        /// Returns serialized attribute <see cref="Ferda.Guha.Attribute"/>.
        /// This fucntion was added to the Slice desing for
        /// the PMML support - it was removed from 
        /// <see cref="Ferda.Modules.Boxes.DataPreparation.AttributeFunctions"/>.
        /// </para>
        /// <para>
        /// The fuzzy attributes work differently and are not created using
        /// the <see cref="Ferda.Guha.Attribute"/>. Therefore the functions
        /// returns an empty string. This affects functionality of the
        /// PMMLBuilder and ETreeClassifier boxes. 
        /// </para>
        /// </summary>
        /// <param name="current__">ICE stuff</param>
        /// <returns>Serialized attribute</returns>
        public string getAttribute(Current current__)
        {
            return string.Empty;
        }

        public override string HelloWorld(Ice.Current __current)
        {
            return "Hello World!";
        }

        #endregion
    }
}
