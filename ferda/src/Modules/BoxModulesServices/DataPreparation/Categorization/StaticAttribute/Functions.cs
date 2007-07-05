// Functions.cs - functionality for Static attribute box
//
// Author: Alexander Kuzmin <alexander.kuzmin@gmail.com>
//
// Copyright (c) 2007 Alexander Kuzmin
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
using Ferda.Guha.Attribute;
using Ferda.Guha.Data;
using Ferda.Guha.MiningProcessor;
using Ferda.Modules.Helpers.Caching;
using Ice;
using Common = Ferda.Guha.Attribute.Common;
using Exception = System.Exception;
using System.Data;
using System.Data.Common;

namespace Ferda.Modules.Boxes.DataPreparation.Categorization.StaticAttribute
{
    internal class Functions : AttributeFunctionsDisp_, IFunctions
    {
        #region Properties

        public const string PropCardinality = "Cardinality";
        public const string PropNameInLiterals = "NameInLiterals";
        public const string PropCountOfCategories = "CountOfCategories";
        public const string PropXCategory = "XCategory";
        public const string PropIncludeNullCategory = "IncludeNullCategory";
        public const string PropDomain = "Domain";
        public const string PropFrom = "From";
        public const string PropTo = "To";
        public const string PropAttribute = "Categories";
        public const string SockColumn = "Column";
        public const string SockBSGen = "BitStringGenerator";


        /// <summary>
        /// Guid
        /// </summary>
        public GuidStruct Guid
        {
            get { return BoxInfoHelper.GetGuidStructFromProperty("Guid", _boxModule); }
        }

        /// <summary>
        /// Attribute cardinality
        /// </summary>
        public CardinalityEnum Cardinality
        {
            get
            {
                return (CardinalityEnum)Enum.Parse(
                                             typeof(CardinalityEnum),
                                             _boxModule.GetPropertyString(PropCardinality)
                                             );
            }
        }

        /// <summary>
        /// Name in literals
        /// </summary>
        public string NameInLiterals
        {
            get { return _boxModule.GetPropertyString(PropNameInLiterals); }
        }


        /* protected string IncludeNullCategory
         {
             get
             {
                 return this._boxModule.GetPropertyString(PropIncludeNullCategory);
             }
         }*/

        /// <summary>
        /// Xcategory
        /// </summary>
        protected string XCategory
        {
            get
            {
                return this._boxModule.GetPropertyString(PropXCategory);
            }
        }

        /// <summary>
        /// "From" restriction
        /// </summary>
        public string From
        {
            get { return _boxModule.GetPropertyString(PropFrom); }
        }

        /// <summary>
        /// "To" restriction
        /// </summary>
        public string To
        {
            get { return _boxModule.GetPropertyString(PropTo); }
        }

        /// <summary>
        /// Attribute domain
        /// </summary>
        public DomainEnum Domain
        {
            get
            {
                return (DomainEnum)Enum.Parse(
                                        typeof(DomainEnum),
                                        _boxModule.GetPropertyString(PropDomain)
                                        );
            }
        }

        /// <summary>
        /// Count of categories
        /// </summary>
        public LongTI CountOfCategories
        {
            get
            {
                return (GetAttribute(false) == null) ? 0 : GetAttribute(false).Count;   
            }
        }

        private string _nullCategoryName = null;

        /// <summary>
        /// Null category name
        /// </summary>
        public StringTI IncludeNullCategory
        {
            get { return _nullCategoryName; }
        }

        #endregion

        /// <summary>
        /// Gets attribute cardinality
        /// </summary>
        /// <param name="current__"></param>
        /// <returns>Attribute cardinality</returns>
        public override CardinalityEnum GetAttributeCardinality(Current current__)
        {
            if (Guha.Data.Common.CompareCardinalityEnums(
                    Cardinality,
                    PotentiallyCardinality(true)
                    ) > 1)
            {
                throw Exceptions.BadValueError(
                    null,
                    _boxModule.StringIceIdentity,
                    "Unsupported cardinality type for current attribute setting.",
                    new string[] { PropCardinality },
                    restrictionTypeEnum.OtherReason
                    );
            }
            return Cardinality;
        }

        /// <summary>
        /// Gets attribute id
        /// </summary>
        /// <param name="current__"></param>
        /// <returns></returns>
        public override GuidStruct GetAttributeId(Current current__)
        {
            return Guid;
        }

        /// <summary>
        /// Gets attribute names
        /// </summary>
        /// <param name="current__"></param>
        /// <returns></returns>
        public override GuidAttributeNamePair[] GetAttributeNames(Current current__)
        {
            return new GuidAttributeNamePair[]
                {
                    new GuidAttributeNamePair(Guid, NameInLiterals),
                };
        }

        /// <summary>
        /// Gets bitstring for specified category id
        /// </summary>
        /// <param name="categoryId">Categoryid of the required bitstring</param>
        /// <param name="current__"></param>
        /// <returns></returns>
        public override BitStringIce GetBitString(string categoryId, Current current__)
        {
            return GetBitString(categoryId, true);
        }

        /// <summary>
        /// Gets categories ids
        /// </summary>
        /// <param name="current__"></param>
        /// <returns></returns>
        public override string[] GetCategoriesIds(Current current__)
        {
            return GetCategoriesIds(true);
        }

        /// <summary>
        /// Gets categories numeric values
        /// </summary>
        /// <param name="current__"></param>
        /// <returns></returns>
        public override double[] GetCategoriesNumericValues(Current current__)
        {
            return GetCategoriesNumericValues(true);
        }

        /// <summary>
        /// Gets count vector for the relational DM purposes
        /// </summary>
        /// <param name="masterIdColumn">Master table id column</param>
        /// <param name="masterDatatableName">Master table name</param>
        /// <param name="detailIdColumn">Detail table id column</param>
        /// <param name="current__"></param>
        /// <returns></returns>
        public override int[] GetCountVector(string masterIdColumn, string masterDatatableName, string detailIdColumn, Current current__)
        {
            string detailId = String.Empty;
            if (String.IsNullOrEmpty(detailIdColumn))
                detailId =
            GetColumnFunctionsPrx(true).getColumnInfo().dataTable.primaryKeyColumns[0];
            else
                detailId = detailIdColumn;
            GenericColumn _column = GetGenericColumn(true);
            DataTable _table = _column.GetCountVector(
                masterIdColumn, masterDatatableName, detailId);
            int[] result = new int[_table.Rows.Count];
            for (int i = 0; i < _table.Rows.Count; i++)
            {
                result[i] = (int)_table.Rows[i][0];
            }
            return result;
        }

        /// <summary>
        /// Gets maximal bitstring count (for the relational DM purposes)
        /// </summary>
        /// <param name="current__"></param>
        /// <returns></returns>
        public override long GetMaxBitStringCount(Current current__)
        {
            return 0;
        }

        /// <summary>
        /// Gets missing information category id
        /// </summary>
        /// <param name="current__"></param>
        /// <returns></returns>
        public override string[] GetMissingInformationCategoryId(Current current__)
        {
            if (String.IsNullOrEmpty(XCategory))
                return new string[0];
            else
                return new string[] { XCategory }; ;
        }

        /// <summary>
        /// Gets next bitstring for virtual column - not implemented here, as the attribute is not virtual
        /// </summary>
        /// <param name="skipFirstN"></param>
        /// <param name="bitString"></param>
        /// <param name="current__"></param>
        /// <returns></returns>
        public override bool GetNextBitString(int skipFirstN, out BitStringIceWithCategoryId bitString, Current current__)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        /// <summary>
        /// Gets source datatable id
        /// </summary>
        /// <param name="current__"></param>
        /// <returns></returns>
        public override string GetSourceDataTableId(Current current__)
        {
            ColumnFunctionsPrx prx = GetColumnFunctionsPrx(true);
            if (prx != null)
                return prx.GetSourceDataTableId();
            return null;
        }

        /// <summary>
        /// Gets serialized attribute
        /// </summary>
        /// <param name="current__"></param>
        /// <returns></returns>
        public override string getAttribute(Current current__)
        {
            return this._boxModule.GetPropertyString(PropAttribute);
        }

        /// <summary>
        /// Gets categories and their frequencies in the attribute
        /// </summary>
        /// <param name="current__"></param>
        /// <returns></returns>
        public override ValuesAndFrequencies getCategoriesAndFrequencies(Current current__)
        {
            return GetCategoriesAndFrequencies(false);
        }

        #region Methods

        /// <summary>
        /// Counts potential cardinality
        /// </summary>
        /// <param name="fallOnError"></param>
        /// <returns></returns>
        public CardinalityEnum PotentiallyCardinality(bool fallOnError)
        {
            return ExceptionsHandler.GetResult<CardinalityEnum>(
                fallOnError,
                delegate
                {
                    ColumnFunctionsPrx prx = GetColumnFunctionsPrx(fallOnError);
                    Attribute<IComparable> tmp = GetAttribute(fallOnError);
                    GenericColumn tmp2 = GetGenericColumn(fallOnError);
                    if (tmp != null && tmp2 != null && prx != null)
                    {
                        CardinalityEnum columnCardinality = prx.getColumnInfo().cardinality;

                        bool ordered;
                        tmp.GetCategoriesIds(out ordered);

                        if (Guha.Data.Common.CompareCardinalityEnums(columnCardinality, CardinalityEnum.Nominal) <=
                            0)
                            return CardinalityEnum.Nominal;
                        else
                        {
                            if (!ordered)
                                return CardinalityEnum.Nominal;
                            else
                            {
                                if (
                                    Guha.Data.Common.CompareCardinalityEnums(columnCardinality,
                                                                             CardinalityEnum.Cardinal) >= 0)
                                {
                                    bool isSingleValueCategories = (GetCategoriesNumericValues(fallOnError) != null);
                                    if (isSingleValueCategories)
                                        return CardinalityEnum.Cardinal;
                                }
                                else
                                {
                                    return columnCardinality; // Ordinal / OrdinalCyclic
                                }
                            }
                        }
                    }
                    return CardinalityEnum.Nominal;
                },
                delegate
                {
                    return CardinalityEnum.Nominal;
                },
                _boxModule.StringIceIdentity
                );
        }

        /// <summary>
        /// Returns categories numeric values
        /// </summary>
        /// <param name="fallOnError"></param>
        /// <returns></returns>
        public double[] GetCategoriesNumericValues(bool fallOnError)
        {
            return ExceptionsHandler.GetResult<double[]>(
                fallOnError,
                delegate
                {
                    Attribute<IComparable> tmp = GetAttribute(fallOnError);
                    GenericColumn tmp2 = GetGenericColumn(fallOnError);
                    if (tmp != null && tmp2 != null)
                    {
                        if (!tmp2.IsNumericDataType)
                            return null;
                        List<IComparable> partialResult =
                            tmp.GetSingleValues(tmp.GetNotMissingsCategorieIds(XCategory));
                        List<double> result = new List<double>(partialResult.Count);
                        foreach (IComparable comparable in partialResult)
                        {
                            result.Add((double)comparable);
                        }
                        return result.ToArray();
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

        private CacheFlag _cacheFlagColumn = new CacheFlag();
        private GenericColumn _cachedValueColumn = null;

        /// <summary>
        /// Gets generic column connected to the attribute
        /// </summary>
        /// <param name="fallOnError"></param>
        /// <returns></returns>
        public GenericColumn GetGenericColumn(bool fallOnError)
        {
            ColumnFunctionsPrx prx = GetColumnFunctionsPrx(fallOnError);
            if (prx == null)
                return null;
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
                            column.columnSelectExpression,
                            column);
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

        private Guid _cachesReloadFlag = System.Guid.NewGuid();
        private Guid _lastReloadFlag = System.Guid.Empty;
        private Dictionary<string, BitStringIce> _cachedValueBitStrings = null;
        private long _lastBSQueryTicks = 0;

        /// <summary>
        /// Gets bitstring of the attribute
        /// </summary>
        /// <param name="fallOnError"></param>
        /// <returns></returns>
        public Dictionary<string, BitStringIce> GetBitStrings(bool fallOnError)
        {
            lock (this)
            {
                return ExceptionsHandler.GetResult<Dictionary<string, BitStringIce>>(
                    fallOnError,
                    delegate
                    {
                        if (_cachedValueBitStrings == null
                            || _lastReloadFlag == System.Guid.Empty
                            || _lastReloadFlag != _cachesReloadFlag
                            //|| (DateTime.Now.Ticks - _lastBSQueryTicks) > 300000000
                            )
                        // ticks:
                        // 1 tick = 100-nanosecond 
                        // nano is 0.000 000 001
                        // mili is 0.001
                        // 0.3 sec is ticks * 3 000 000
                        {
                            // get primary key
                            ColumnFunctionsPrx prx = GetColumnFunctionsPrx(fallOnError);
                            if (prx == null)
                                return null;
                            string[] pks = prx.getColumnInfo().dataTable.primaryKeyColumns;

                            GenericColumn gc = GetGenericColumn(fallOnError);
                            Attribute<IComparable> att = GetAttribute(true);
                            //if (String.IsNullOrEmpty(_lastReloadFlag.ToString()) || _lastReloadFlag != _cachesReloadFlag)
                            {
                                if (gc == null || att == null)
                                {
                                    _cachedValueBitStrings = null;
                                    return null;
                                }
                                _cachedValueBitStrings = att.GetBitStrings(gc.GetSelect(pks));
                            }

                            _lastBSQueryTicks = DateTime.Now.Ticks;
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
        /// Gets bitstring with specified category name
        /// </summary>
        /// <param name="categoryName">Category name to get bitstring for</param>
        /// <param name="fallOnError"></param>
        /// <returns></returns>
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
                        Dictionary<string, BitStringIce> cachedValueBitStrings = GetBitStrings(fallOnError);
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
                                return cachedValueBitStrings[categoryName];
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
        /// Gets categories names
        /// </summary>
        /// <param name="fallOnError"></param>
        /// <returns></returns>
        public string[] GetCategoriesNames(bool fallOnError)
        {
            return ExceptionsHandler.GetResult<string[]>(
                fallOnError,
                delegate
                {
                    Attribute<IComparable> tmp = GetAttribute(fallOnError);
                    if (tmp != null)
                    {
                        bool dummy;
                        List<string> result = tmp.GetCategoriesIds(out dummy);
                        return result.ToArray();
                    }
                    return new string[0];
                },
                delegate
                {
                    return new string[0];
                },
                _boxModule.StringIceIdentity
                );
        }

        /// <summary>
        /// Gets categories ids (they are actually names of the categories
        /// that need to be unique). The missing category is ommited.
        /// </summary>
        /// <param name="fallOnError">If the function should throw an exception
        /// when error</param>
        /// <returns>Categories names</returns>
        public string[] GetCategoriesIds(bool fallOnError)
        {
            return ExceptionsHandler.GetResult<string[]>(
                fallOnError,
                delegate
                {
                    Attribute<IComparable> tmp = GetAttribute(fallOnError);
                    if (tmp != null)
                    {
                        List<string> result = tmp.GetNotMissingsCategorieIds(XCategory);
                        return result.ToArray();
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

        private CacheFlag _cacheFlag = new CacheFlag();
        private Attribute<IComparable> _cachedValue = null;

        /// <summary>
        /// Gets attribute
        /// </summary>
        /// <param name="fallOnError"></param>
        /// <returns></returns>
        public Attribute<IComparable> GetAttribute(bool fallOnError)
        {
            //getting the proxy of a column
            ColumnFunctionsPrx prx = GetColumnFunctionsPrx(fallOnError);
            if (prx == null)
                return null;

            //getting a info about a column
            ColumnInfo tmp =
                ExceptionsHandler.GetResult<ColumnInfo>(
                    fallOnError,
                    prx.getColumnInfo,
                    delegate
                    {
                        return null;
                    },
                    _boxModule.StringIceIdentity
                    );

            if (tmp == null)
                return null;

            //getting the connection setting
            DatabaseConnectionSettingHelper connSetting =
                new DatabaseConnectionSettingHelper(tmp.dataTable.databaseConnectionSetting);

            //getting the serialized attribute set in the previous boxes
            string categories = getAttribute();

            //creating a new cache entry
            Dictionary<string, IComparable> cacheSetting = new Dictionary<string, IComparable>();

            //adding attribute information to the cache
            cacheSetting.Add(
                Datasource.Database.BoxInfo.typeIdentifier + Datasource.Database.Functions.PropConnectionString,
                connSetting);
            cacheSetting.Add(Datasource.DataTable.BoxInfo.typeIdentifier + Datasource.DataTable.Functions.PropName,
                             tmp.dataTable.dataTableName);
            cacheSetting.Add(
                Datasource.Column.BoxInfo.typeIdentifier + Datasource.Column.Functions.PropSelectExpression,
                tmp.columnSelectExpression);
            cacheSetting.Add(Datasource.Column.BoxInfo.typeIdentifier + Datasource.Column.Functions.PropCardinality,
                             tmp.cardinality);
            cacheSetting.Add(BoxInfo.typeIdentifier + PropDomain, Domain.ToString());
            cacheSetting.Add(BoxInfo.typeIdentifier + PropFrom, From);
            cacheSetting.Add(BoxInfo.typeIdentifier + PropTo, To);

            //adding serialized attribute to cache only if it has been set in the previous box
            if (!String.IsNullOrEmpty(categories))
                cacheSetting.Add("SerializedAttribute", categories);

            //Loading the values from cache or recounting them...
            if (_cacheFlag.IsObsolete(connSetting.LastReloadRequest, cacheSetting)
                || (_cachedValue == null && fallOnError))
            {
                _cachesReloadFlag = System.Guid.NewGuid();
                _cachedValue = ExceptionsHandler.GetResult<Attribute<IComparable>>(
                    fallOnError,
                    delegate
                    {
                        Attribute<IComparable> attribute;
                        DbDataTypeEnum columnDataType;
                        //this is the case when static attribute connected to attribute box
                        //need to jump twice to get to the column box
                        try
                        {
                            BoxModulePrx boxModuleParamNew =
                                _boxModule.MyProxy.getConnections("Column")[0];
                            BoxModulePrx boxModuleParam1 =
                                boxModuleParamNew.getConnections("Column")[0];
                            ColumnFunctionsPrx prx2 =
                                       ColumnFunctionsPrxHelper.checkedCast(
                                       boxModuleParam1.getFunctions());
                            columnDataType = prx2.getColumnInfo().dataType;
                            if (String.IsNullOrEmpty(categories))
                            {
                                AttributeFunctionsPrx prx3 =
                                       AttributeFunctionsPrxHelper.checkedCast(
                                       boxModuleParam1.getFunctions());
                                try
                                {
                                    categories = prx3.getAttribute();
                                }
                                catch
                                {
                                    return null;
                                }
                            }
                        }
                        catch
                        {
                            if (String.IsNullOrEmpty(categories))
                            {
                                return null;
                            }
                            //this is the case when static attribute connected to column
                            //need to jump once to get to the column box
                            BoxModulePrx boxModuleParam1 =
                                _boxModule.MyProxy.getConnections("Column")[0];
                            ColumnFunctionsPrx prx2 =
                                       ColumnFunctionsPrxHelper.checkedCast(
                                       boxModuleParam1.getFunctions());
                            columnDataType = prx2.getColumnInfo().dataType;
                        }

                        #region datatype switch

                        //creating the attribute of the correct datatype
                        switch (columnDataType)
                        {
                            case DbDataTypeEnum.BooleanType:
                                attribute =
                                    Retyper<IComparable, Boolean>.ToIComparable(
                                    Guha.Attribute.Serializer.Deserialize<Boolean>(categories));
                                break;

                            case DbDataTypeEnum.DateTimeType:
                                attribute =
                                    Retyper<IComparable, DateTime>.ToIComparable(
                                    Guha.Attribute.Serializer.Deserialize<DateTime>(categories));
                                break;

                            case DbDataTypeEnum.DoubleType:
                                attribute =
                                    Retyper<IComparable, Double>.ToIComparable(
                                    Guha.Attribute.Serializer.Deserialize<Double>(categories));
                                break;

                            case DbDataTypeEnum.FloatType:
                            case DbDataTypeEnum.DecimalType:
                                attribute =
                                    Retyper<IComparable, Single>.ToIComparable(
                                    Guha.Attribute.Serializer.Deserialize<Single>(categories));
                                break;


                            case DbDataTypeEnum.IntegerType:
                            case DbDataTypeEnum.UnsignedIntegerType:
                                attribute =
                                   Retyper<IComparable, Int32>.ToIComparable(
                                    Guha.Attribute.Serializer.Deserialize<Int32>(categories));
                                break;

                            case DbDataTypeEnum.ShortIntegerType:
                            case DbDataTypeEnum.UnsignedShortIntegerType:
                                attribute =
                                   Retyper<IComparable, Int16>.ToIComparable(
                                    Guha.Attribute.Serializer.Deserialize<Int16>(categories));
                                break;

                            case DbDataTypeEnum.LongIntegerType:
                            case DbDataTypeEnum.UnsignedLongIntegerType:
                                attribute =
                                   Retyper<IComparable, Int64>.ToIComparable(
                                    Guha.Attribute.Serializer.Deserialize<Int64>(categories));
                                break;

                            case DbDataTypeEnum.StringType:
                                attribute =
                                   Retyper<IComparable, String>.ToIComparable(
                                    Guha.Attribute.Serializer.Deserialize<String>(categories));
                                break;

                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                        #endregion

                        _nullCategoryName = attribute.NullContainingCategory;

                        return attribute;
                    },
                    delegate
                    {
                        return null;
                    },
                    _boxModule.StringIceIdentity
                    );
            }
            return _cachedValue;
        }

        /// <summary>
        /// Gets categories and frequencies
        /// </summary>
        /// <param name="fallOnError"></param>
        /// <returns></returns>
        public ValuesAndFrequencies GetCategoriesAndFrequencies(bool fallOnError)
        {
            return ExceptionsHandler.GetResult<ValuesAndFrequencies>(
                fallOnError,
                delegate
                {
                    Attribute<IComparable> tmp = GetAttribute(fallOnError);
                    GenericColumn tmp2 = GetGenericColumn(fallOnError);
                    if (tmp != null && tmp2 != null)
                    {
                        Dictionary<string, int> categoriesFrequencies = tmp.GetFrequencies(
                            tmp2.GetDistinctsAndFrequencies(null)
                            );
                        return new ValuesAndFrequencies(
                            tmp2.Explain.dataType,
                            BoxInfoHelper.GetValueFrequencyPairArray(categoriesFrequencies)
                            );
                    }

                    if (tmp == null)
                    {
                        return new ValuesAndFrequencies(
                            tmp2.Explain.dataType,
                            BoxInfoHelper.GetValueFrequencyPairArray(new Dictionary<string,int>())
                            );
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
        /// Gets proxy of the connected column box
        /// </summary>
        /// <param name="fallOnError"></param>
        /// <returns></returns>
        public ColumnFunctionsPrx GetColumnFunctionsPrx(bool fallOnError)
        {
            BoxModulePrx boxModuleParam1;
            try
            {
                BoxModulePrx boxModuleParamNew = _boxModule.MyProxy.getConnections("Column")[0];
                boxModuleParam1 = boxModuleParamNew.getConnections("Column")[0];
            }
            catch
            {
                try
                {
                    boxModuleParam1 = _boxModule.MyProxy.getConnections("Column")[0];
                }
                catch
                {
                    return null;
                }
            }
            return DataPreparation.ColumnFunctionsPrxHelper.checkedCast(
                boxModuleParam1.getFunctions());

        }

        /// <summary>
        /// Returns connected bitstring generator
        /// </summary>
        /// <param name="fallOnError"></param>
        /// <returns></returns>
        public BitStringGeneratorPrx GetBitStringGeneratorPrx(bool fallOnError)
        {
            return SocketConnections.GetPrx<BitStringGeneratorPrx>(
                _boxModule, SockBSGen, BitStringGeneratorPrxHelper.checkedCast,
                fallOnError);
        }


        #endregion

        #region IFunctions Members

        /// <summary>
        /// The box module.
        /// </summary>
        protected BoxModuleI _boxModule;

        //protected IBoxInfo _boxInfo;

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
    }
}