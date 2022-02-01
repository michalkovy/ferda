// Functions.cs - functionality for the Manual Attribute with fuzzy categories box
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
using System.Data;
using Ferda.Guha.Data;
using Ferda.Guha.Attribute;
using Ferda.Guha.MiningProcessor;
using Ferda.Modules.Helpers.Caching;
using Ice;

namespace Ferda.Modules.Boxes.DataPreparation.Categorization.ManualAttributeWithFuzzyCategories
{
    /// <summary>
    /// Class is providing ICE functionality of the SampleBoxModule
    /// box module
    /// </summary>
    public class Functions : AttributeFunctionsDisp_, Ferda.Modules.IFunctions
    {
        #region Fields

        protected Ferda.Modules.BoxModuleI _boxModule;
        protected Ferda.Modules.Boxes.IBoxInfo _boxInfo;
        public const string SocketFuzzyCategories = "FuzzyCategories";

        //caches and flags used for retrieving column
        private CacheFlag _cacheFlagColumn = new CacheFlag();
        private GenericColumn _cachedValueColumn = null;
        private Guid _cachesReloadFlag = System.Guid.NewGuid();

        //caches and flags used for retrieving bit strings
        private Guid _lastReloadFlag = System.Guid.Empty;
        private Dictionary<string, FuzzyBitStringIce> _cachedValueBitStrings = null;

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

        /// <summary>
        /// Retrieves the fuzzy sets edited by the user in the
        /// trapezoidal form. 
        /// </summary>
        public TrapezoidalFuzzySets FuzzySets
        {
            get
            {
                return TrapezoidalFuzzySets.Deserialize(
                    _boxModule.GetPropertyString(SocketFuzzyCategories));
            }
        }

        /// <summary>
        /// Gets the NameInBooleanAttributes property
        /// </summary>
        public string NameInBooleanAttributes
        {
            get
            {
                return _boxModule.GetPropertyString(
                    Public.SockNameInBooleanAttributes);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets generic column connected to the attribute
        /// </summary>
        /// <param name="fallOnError"></param>
        /// <returns></returns>
        public GenericColumn GetGenericColumn(bool fallOnError)
        {
            ColumnFunctionsPrx prx = Public.GetColumnFunctionsPrx(fallOnError, _boxModule);
            if (prx == null)
            {
                return null;
            }
            ColumnInfo column = prx.getColumnInfo();

            DatabaseConnectionSettingHelper connSetting =
                new DatabaseConnectionSettingHelper(column.dataTable.databaseConnectionSetting);

            Dictionary<string, IComparable> cacheSetting = new Dictionary<string, IComparable>();
            cacheSetting.Add(
                Datasource.Database.BoxInfo.typeIdentifier + Datasource.Database.Functions.PropConnectionString,
                connSetting);
            cacheSetting.Add(Datasource.DataTable.BoxInfo.typeIdentifier + Datasource.DataTable.Functions.PropName,
                             column.dataTable.dataTableName);
            cacheSetting.Add(
                Datasource.Column.BoxInfo.typeIdentifier + Datasource.Column.Functions.PropSelectExpression,
                column.columnSelectExpression);

            if (_cacheFlagColumn.IsObsolete(connSetting.LastReloadRequest, cacheSetting)
                || (_cachedValueColumn == null && fallOnError))
            {
                _cachesReloadFlag = System.Guid.NewGuid();
                _cachedValueColumn = ExceptionsHandler.GetResult<GenericColumn>(
                    fallOnError,
                    delegate
                    {
                        return
                            GenericDatabaseCache.GetGenericDatabase(connSetting)
                            [column.dataTable.dataTableName].GetGenericColumn(
                            column.columnSelectExpression, column);

                    },
                    delegate
                    {
                        return null;
                    },
                    _boxModule.StringIceIdentity
                    );
            }
            return _cachedValueColumn;
        }

        /// <summary>
        /// Gets categories and frequencies
        /// </summary>
        /// <param name="fallOnError">If the method should fall on error</param>
        /// <returns>Values and frequencies structure</returns>
        public ValuesAndFrequencies GetCategoriesAndFrequencies(bool fallOnError)
        {
            return ExceptionsHandler.GetResult<ValuesAndFrequencies>(
                fallOnError,
                delegate
                {
                    if (FuzzySets == null)
                    {
                        return null;
                    }
                    ValuesAndFrequencies vls = Public.GetColumnFunctionsPrx(fallOnError,
                        _boxModule).getDistinctsAndFrequencies();
                    if (vls != null)
                    {
                        if (!GenericColumn.GetIsNumericDataType(vls.dataType))
                        {
                            return null;
                        }

                        //result
                        ValuesAndFrequencies result = new ValuesAndFrequencies();
                        result.dataType = vls.dataType;

                        TrapezoidalFuzzySet [] tmpSet = FuzzySets.fuzzySets;
                        ValueFrequencyPair[] list = new ValueFrequencyPair[tmpSet.Length];
                        int j = 0;
                        for (int i = 0; i < tmpSet.Length; i++)
                        {
                            ValueFrequencyPair pair = new ValueFrequencyPair();
                            pair.value = tmpSet[i].Name;

                            double freq = 0;
                            foreach (ValueFrequencyPair p in vls.data)
                            {
                                freq += MemgershipDegree(tmpSet[i], p);
                            }
                            pair.frequency = freq;
                            list[j] = pair;
                            j++;
                        }
                        result.data = list;
                        return result;
                    }
                    return null;
                },
                delegate
                {
                    return null;
                },
                _boxModule.StringIceIdentity
                );
        }

        /// <summary>
        /// Computes a memgership degree of the trapezoidal fuzzy sey in
        /// the parameter <paramref name="trapezoidalFuzzySet"/> for given
        /// data. The data represents one distinct value of the column and
        /// its frequency. 
        /// </summary>
        /// <param name="trapezoidalFuzzySet">The trapezoidal fuzzy set</param>
        /// <param name="pair">The distinct value and frequency pair of the
        /// data.</param>
        /// <returns>
        /// Membership degree of a distinct value in the
        /// column (the membership degree is multipied by its frequency)
        /// </returns>
        private double MemgershipDegree(TrapezoidalFuzzySet trapezoidalFuzzySet, 
            ValueFrequencyPair pair)
        {
            if (pair.value == "Null")
                return 0;
            double value = Convert.ToDouble(pair.value);
            if (value < trapezoidalFuzzySet.A)
                return 0;
            if (value > trapezoidalFuzzySet.B)
                return 0;
            if (value >= trapezoidalFuzzySet.D && value <= trapezoidalFuzzySet.C)
                return pair.frequency; //frequency
            if (value < trapezoidalFuzzySet.D)
                return pair.frequency * (value - trapezoidalFuzzySet.A)
                    / (trapezoidalFuzzySet.D - trapezoidalFuzzySet.A);
            else
                return pair.frequency * (trapezoidalFuzzySet.B - value)
                    / (trapezoidalFuzzySet.B - trapezoidalFuzzySet.C);
        }

        /// <summary>
        /// Computes membership degree of a trapezoidal fuzzy set for
        /// given number. 
        /// </summary>
        /// <param name="set">The trapezoidal fuzzy set</param>
        /// <param name="p">Number</param>
        /// <returns>Membership degree of the number in the trapezoidal fuzzy set.</returns>
        private float MemgershipDegree(TrapezoidalFuzzySet set, float p)
        {
            if (p < set.A)
                return 0f;
            if (p > set.B)
                return 0f;
            if (p >= set.D && p <= set.C)
                return 1f;
            if (p < set.D)
                return Convert.ToSingle((p - set.A) / (set.D - set.A));
            else
                return Convert.ToSingle((set.B - p) / (set.B - set.C));
        }

        /// <summary>
        /// Gets the bit string of a given category. 
        /// </summary>
        /// <param name="categoryName">Name of the category</param>
        /// <param name="fallOnError">If the method should fall on error</param>
        /// <returns>The bit string</returns>
        public BitStringIce GetBitString(string categoryName, bool fallOnError)
        {
            // categoryName is "" if it should be null (throught middleware)
            if (String.IsNullOrEmpty(categoryName))
                if (fallOnError)
                    throw Exceptions.BoxRuntimeError(null, _boxModule.StringIceIdentity,
                                                 "String.IsNullOrEmpty(categoryName) in public BitStringIce GetBitString(string categoryName, bool fallOnError)");
                else
                    return null;

            lock (this)
            {
                return ExceptionsHandler.GetResult<BitStringIce>(
                    fallOnError,
                    delegate
                    {
                        Dictionary<string, FuzzyBitStringIce> cachedValueBitStrings = GetBitStrings(fallOnError);
                        if (cachedValueBitStrings == null)
                        {
                            if (fallOnError)
                                throw Exceptions.BoxRuntimeError(null, _boxModule.StringIceIdentity,
                                                                 "cachedValueBitStrings == null in public BitStringIce GetBitString(string categoryName, bool fallOnError)");
                            else
                                return null;
                        }
                        else
                        {
                            try
                            {
                                BitStringIce bs = cachedValueBitStrings[categoryName] 
                                    as BitStringIce;
                                return bs;
                            }
                            catch (KeyNotFoundException e)
                            {
                                throw Exceptions.BoxRuntimeError(e, _boxModule.StringIceIdentity,
                                                                 "Category named " + categoryName +
                                                                 " was not found in the attribute.");
                            }
                        }
                    },
                    delegate
                    {
                        return null;
                    },
                    _boxModule.StringIceIdentity
                    );
            }
        }

        /// <summary>
        /// Gets a dictionary of names of categories and corresponding bit strings.
        /// The method either uses a cache, or computes the bit strings from
        /// columns.
        /// </summary>
        /// <param name="fallOnError">If the method should fall on error</param>
        /// <returns>Dictionary containing categories names and bit strings</returns>
        public Dictionary<string, FuzzyBitStringIce> GetBitStrings(bool fallOnError)
        {
            lock (this)
            {
                return ExceptionsHandler.GetResult<Dictionary<string, FuzzyBitStringIce>>(
                    fallOnError,
                    delegate
                    {
                        if (_cachedValueBitStrings == null
                            || _lastReloadFlag == System.Guid.Empty
                            || _lastReloadFlag != _cachesReloadFlag
                            )
                        {
                            // get primary key
                            ColumnFunctionsPrx prx = Public.GetColumnFunctionsPrx(fallOnError, _boxModule);
                            if (prx == null)
                                return null;
                            string[] pks = prx.getColumnInfo().dataTable.primaryKeyColumns;

                            GenericColumn gc = GetGenericColumn(fallOnError);

                            if (!GenericColumn.GetIsNumericDataType(prx.getColumnInfo().dataType))
                            {
                                throw Exceptions.BoxRuntimeError(null, _boxModule.StringIceIdentity,
                                                                 "The connected column does not have numerical type.");
                            }

                            if (gc == null)
                            {
                                _cachedValueBitStrings = null;
                                return null;
                            }
                            _cachedValueBitStrings = ComputeFuzzyBitStrings(gc.GetSelect(pks));

                            _lastReloadFlag = _cachesReloadFlag;
                        }
                        return _cachedValueBitStrings;
                    },
                    delegate
                    {
                        return null;
                    },
                    _boxModule.StringIceIdentity
                    );
            }
        }

        /// <summary>
        /// The method computes the fuzzy bit strings from the data.
        /// The null values are treated as zero. 
        /// </summary>
        /// <param name="dataTable">The column data</param>
        /// <returns>
        /// Dictionary of categories and their
        /// bit strings.
        /// </returns>
        private Dictionary<string, FuzzyBitStringIce> ComputeFuzzyBitStrings(DataTable dataTable)
        {
            Dictionary<string, FuzzyBitStringIce> result = new Dictionary<string, FuzzyBitStringIce>();

            foreach (TrapezoidalFuzzySet set in FuzzySets.fuzzySets)
            {
                FuzzyBitStringIce bitString = new FuzzyBitStringIce();
                float[] floats = new float[dataTable.Rows.Count];

                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    if (dataTable.Rows[i][0] is DBNull)
                    {
                        floats[i] = 0;
                    }
                    else
                    {
                        floats[i] = MemgershipDegree(set, Convert.ToSingle(dataTable.Rows[i][0]));
                    }
                }

                bitString.value = floats;
                result.Add(set.Name, bitString);
            }
            return result;
        }

        #endregion

        #region ICE functions

        /// <summary>
        /// Gets attribute names
        /// </summary>
        /// <param name="current__">ICE stuff</param>
        /// <returns>Attribute names</returns>
        public override GuidAttributeNamePair[] GetAttributeNames(Current current__)
        {
            return new GuidAttributeNamePair[]
                {
                    new GuidAttributeNamePair(Guid, Public.NameInBooleanAttributes(_boxModule)),
                };
        }

        /// <summary>
        /// Gets the name of the column from which the attribute and bit string generator
        /// is created. Added for PMML purposes. 
        /// </summary>
        /// <param name="current__">ICE stuff</param>
        /// <returns></returns>
        public override string GetColumnName(Current current__)
        {
            return Public.GetColumnFunctionsPrx(true, _boxModule).getColumnInfo().columnSelectExpression;
        }

        /// <summary>
        /// Returns information from the column about the values and frequencies
        /// of the column. This fucntion was added to the Slice desing for
        /// the PMML support.
        /// </summary>
        /// <returns>ValuesAndFrequencies structure</returns>
        public override ValuesAndFrequencies GetColumnValuesAndFrequencies(Current current__)
        {
            return Public.GetColumnFunctionsPrx(true, _boxModule).getDistinctsAndFrequencies();
        }

        /// <summary>
        /// Returns the identification of the attribute.
        /// </summary>
        /// <param name="current__">ICE stuff</param>
        /// <returns>Identification of the attribute</returns>
        public override GuidStruct GetAttributeId(Current current__)
        {
            return Guid;
        }

        /// <summary>
        /// Returns cardinality of the attribute (nominal/ordinal/cyclic ordinal/cardinal).
        /// </summary>
        /// <param name="current__">ICE stuff</param>
        /// <returns>Attribute cardinality</returns>
        public override CardinalityEnum GetAttributeCardinality(Current current__)
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
        public override int[] GetCountVector(string masterIdColumn, string masterDatatableName, string detailIdColumn, Current current__)
        {
            throw Exceptions.BoxRuntimeError(null, BoxInfo.typeIdentifier,
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
        public override bool GetNextBitString(int skipFirstN, out BitStringIceWithCategoryId bitString, Current current__)
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
        public override long GetMaxBitStringCount(Current current__)
        {
            return 0;
        }

        /// <summary>
        /// Gets identificators of categories. Names of the categories are used
        /// as their identificators. The <see cref="Ferda.Guha.Attribute"/> class
        /// ensures, that the names of categories are unique.
        /// </summary>
        /// <param name="current__">ICE stuff</param>
        /// <returns>Identificators of categories</returns>
        public override string[] GetCategoriesIds(Current current__)
        {
            List<string> list = new List<string>();
            foreach (TrapezoidalFuzzySet set in FuzzySets.fuzzySets)
            {
                list.Add(set.Name);
            }
            return list.ToArray();
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
        public override string[] GetMissingInformationCategoryId(Current current__)
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
        public override double[] GetCategoriesNumericValues(Current current__)
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
        public override string getAttribute(Current current__)
        {
            return string.Empty;
        }

        /// <summary>
        /// Gets categories and their frequencies in the attribute
        /// </summary>
        /// <param name="current__">Ice stuff</param>
        /// <returns>Values and frequencies pair</returns>
        public override ValuesAndFrequencies getCategoriesAndFrequencies(Current current__)
        {
            return GetCategoriesAndFrequencies(true);
        }

        /// <summary>
        /// Returns a bit string for category in the 
        /// <paramref name="categoryId"/> parameter.
        /// </summary>
        /// <param name="categoryId">Category identification
        /// (name of the category)</param>
        /// <param name="current__">Ice stuff</param>
        /// <returns>BitString</returns>
        public override BitStringIce GetBitString(string categoryId, Current current__)
        {
            return GetBitString(categoryId, true);
        }

        /// <summary>
        /// Gets source datatable id
        /// </summary>
        /// <param name="current__">ICEstuff</param>
        /// <returns></returns>
        public override string GetSourceDataTableId(Current current__)
        {
            ColumnFunctionsPrx prx = Public.GetColumnFunctionsPrx(true, _boxModule);
            if (prx != null)
                return prx.GetSourceDataTableId();
            return null;
        }

        #endregion
    }
}
