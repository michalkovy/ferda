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

        public GuidStruct Guid
        {
            get { return BoxInfoHelper.GetGuidStructFromProperty("Guid", _boxModule); }
        }

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


        protected string XCategory
        {
            get
            {
                return this._boxModule.GetPropertyString(PropXCategory);
            }
        }

        public string From
        {
            get { return _boxModule.GetPropertyString(PropFrom); }
        }

        public string To
        {
            get { return _boxModule.GetPropertyString(PropTo); }
        }

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

        public LongTI CountOfCategories
        {
            get
            {
                return GetAttribute(false).Count;
            }
        }

        private string _nullCategoryName = null;

        public StringTI IncludeNullCategory
        {
            get { return _nullCategoryName; }
        }

        #endregion


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

        public override GuidStruct GetAttributeId(Current current__)
        {
            return Guid;
        }

        public override GuidAttributeNamePair[] GetAttributeNames(Current current__)
        {
            return new GuidAttributeNamePair[]
                {
                    new GuidAttributeNamePair(Guid, NameInLiterals),
                };
        }

        public override BitStringIce GetBitString(string categoryId, Current current__)
        {
            return GetBitString(categoryId, true);
        }

        public override string[] GetCategoriesIds(Current current__)
        {
            return GetCategoriesIds(true);
        }

        public override double[] GetCategoriesNumericValues(Current current__)
        {
            return GetCategoriesNumericValues(true);
        }

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

        public override long GetMaxBitStringCount(Current current__)
        {
            return 0;
        }

        public override string[] GetMissingInformationCategoryId(Current current__)
        {
            if (String.IsNullOrEmpty(XCategory))
                return new string[0];
            else
                return new string[] { XCategory }; ;
        }

        public override bool GetNextBitString(int skipFirstN, out BitStringIceWithCategoryId bitString, Current current__)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override string GetSourceDataTableId(Current current__)
        {
            ColumnFunctionsPrx prx = GetColumnFunctionsPrx(true);
            if (prx != null)
                return prx.GetSourceDataTableId();
            return null;
        }

        public override string getAttribute(Current current__)
        {
            return this._boxModule.GetPropertyString(PropAttribute);
        }

        public override ValuesAndFrequencies getCategoriesAndFrequencies(Current current__)
        {
            return GetCategoriesAndFrequencies(false);
        }

        #region Methods

        /*public BitStringGeneratorPrx GetBitStringGeneratorPrx(bool fallOnError)
        {
            return SocketConnections.GetPrx<BitStringGeneratorPrx>(
                _boxModule,
                SockBitStringGenerator,
                BitStringGeneratorPrxHelper.checkedCast,
                fallOnError);
        }*/

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
        private Guid _lastReloadFlag;
        private Dictionary<string, BitStringIce> _cachedValueBitStrings = null;
        private long _lastBSQueryTicks = 0;
        public Dictionary<string, BitStringIce> GetBitStrings(bool fallOnError)
        {
            lock (this)
            {
                return ExceptionsHandler.GetResult<Dictionary<string, BitStringIce>>(
                    fallOnError,
                    delegate
                    {
                        if (_cachedValueBitStrings == null
                            || String.IsNullOrEmpty(_lastReloadFlag.ToString())
                            || _lastReloadFlag != _cachesReloadFlag
                            || (DateTime.Now.Ticks - _lastBSQueryTicks) > 300000000
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

        public Attribute<IComparable> GetAttribute(bool fallOnError)
        {
            ColumnFunctionsPrx prx = GetColumnFunctionsPrx(fallOnError);
            if (prx == null)
                return null;

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

            DatabaseConnectionSettingHelper connSetting =
                new DatabaseConnectionSettingHelper(tmp.dataTable.databaseConnectionSetting);

            string categories = getAttribute();


            Dictionary<string, IComparable> cacheSetting = new Dictionary<string, IComparable>();
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

            if (!String.IsNullOrEmpty(categories))
                cacheSetting.Add("SerializedAttribute", categories);


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
                        //static attribute connected to attribute
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
                            //static attribute connected to column
                            BoxModulePrx boxModuleParam1 =
                                _boxModule.MyProxy.getConnections("Column")[0];
                            ColumnFunctionsPrx prx2 =
                                       ColumnFunctionsPrxHelper.checkedCast(
                                       boxModuleParam1.getFunctions());
                            columnDataType = prx2.getColumnInfo().dataType;
                        }

                        #region datatype switch
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