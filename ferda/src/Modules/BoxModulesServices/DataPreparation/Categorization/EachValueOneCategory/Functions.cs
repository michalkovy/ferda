using System;
using System.Collections.Generic;
using System.Diagnostics;
using Ferda.Guha.Attribute;
using Ferda.Guha.Data;
using Ferda.Modules.Helpers.Caching;
using Ice;
using Exception = System.Exception;
using Ferda.Guha.MiningProcessor;

namespace Ferda.Modules.Boxes.DataPreparation.Categorization.EachValueOneCategory
{
    internal class Functions : AttributeFunctionsDisp_, IFunctions
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

        public const string PropNameInLiterals = "NameInLiterals";
        public const string PropCountOfCategories = "CountOfCategories";
        public const string PropXCategory = "XCategory";
        public const string PropIncludeNullCategory = "IncludeNullCategory";
        public const string PropDomain = "Domain";
        public const string PropFrom = "From";
        public const string PropTo = "To";
        public const string PropCardinality = "Cardinality";
        public const string SockColumn = "Column";

        public string NameInLiterals
        {
            get { return _boxModule.GetPropertyString(PropNameInLiterals); }
        }

        public LongTI CountOfCategories
        {
            get
            {
                Attribute<IComparable> tmp =
                    GetAttribute(false);
                return (tmp != null) ? tmp.Count : 0;
            }
        }

        public string XCategory
        {
            get { return _boxModule.GetPropertyString(PropXCategory); }
        }

        private string _nullCategoryName = null;
        public StringTI IncludeNullCategory
        {
            get { return _nullCategoryName; }
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

        public string From
        {
            get { return _boxModule.GetPropertyString(PropFrom); }
        }

        public string To
        {
            get { return _boxModule.GetPropertyString(PropTo); }
        }

        public CardinalityEnum Cardinality
        {
            get
            {
                return (Guha.Data.CardinalityEnum)Enum.Parse(
                                     typeof(Guha.Data.CardinalityEnum),
                                     _boxModule.GetPropertyString(PropCardinality)
                                     );
            }
        }

        #endregion

        #region Methods

        public ColumnFunctionsPrx GetColumnFunctionsPrx(bool fallOnError)
        {
            return SocketConnections.GetPrx<ColumnFunctionsPrx>(
                _boxModule,
                SockColumn,
                ColumnFunctionsPrxHelper.checkedCast,
                fallOnError);
        }

        private void parseFromTo(DbDataTypeEnum dataType, out IComparable from, out IComparable to)
        {
            try
            {
                GenericColumn.ParseValue(From, dataType, out from);
            }
            catch (Exception e)
            {
                throw Exceptions.BadValueError(
                    e,
                    _boxModule.StringIceIdentity,
                    null,
                    new string[] { PropFrom },
                    restrictionTypeEnum.BadFormat
                    );
            }
            try
            {
                GenericColumn.ParseValue(From, dataType, out to);
            }
            catch (Exception e)
            {
                throw Exceptions.BadValueError(
                    e,
                    _boxModule.StringIceIdentity,
                    null,
                    new string[] { PropTo },
                    restrictionTypeEnum.BadFormat
                    );
            }
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
            cacheSetting.Add(Datasource.Database.BoxInfo.typeIdentifier + Datasource.Database.Functions.PropConnectionString, connSetting);
            cacheSetting.Add(Datasource.DataTable.BoxInfo.typeIdentifier + Datasource.DataTable.Functions.PropName, column.dataTable.dataTableName);
            cacheSetting.Add(Datasource.Column.BoxInfo.typeIdentifier + Datasource.Column.Functions.PropSelectExpression, column.columnSelectExpression);

            if (_cacheFlagColumn.IsObsolete(connSetting.LastReloadRequest, cacheSetting)
                || (_cachedValueColumn == null && fallOnError))
            {
                _cachesReloadFlag = Guid.NewGuid();
                _cachedValueColumn = ExceptionsHandler.GetResult<GenericColumn>(
                    fallOnError,
                    delegate
                    {
                        return
                            GenericDatabaseCache.GetGenericDatabase(connSetting)[column.dataTable.dataTableName].GetGenericColumn
                                (column.columnSelectExpression);
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

        private CacheFlag _cacheFlag = new CacheFlag();
        private Attribute<IComparable> _cachedValue = null;
        public Attribute<IComparable> GetAttribute(bool fallOnError)
        {
            _nullCategoryName = null;

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

            if (_cacheFlag.IsObsolete(connSetting.LastReloadRequest, cacheSetting)
                || (_cachedValue == null && fallOnError))
            {
                _cachesReloadFlag = Guid.NewGuid();
                _cachedValue = ExceptionsHandler.GetResult<Attribute<IComparable>>(
                    fallOnError,
                    delegate
                    {
                        DbDataTypeEnum dataType = prx.getColumnInfo().dataType;

                        //UNDONE Column.getDistinctsAndFrequencies(WHERE from - to)
                        //GenericColumn column = GenericDatabaseCache.GetGenericDatabase(connSetting)[tmp.dataTable.dataTableName].GetGenericColumn(tmp.columnSelectExpression);
                        //System.Data.DataTable dt = column.GetDistinctsAndFrequencies(null);

                        ValuesAndFrequencies df = prx.getDistinctsAndFrequencies();

                        Debug.Assert(df.dataType == dataType);
                        DbSimpleDataTypeEnum simpleDbDataType = GenericColumn.GetSimpleDataType(df.dataType);

                        Attribute<IComparable> result =
                            (Attribute<IComparable>)Common.GetAttributeObject(simpleDbDataType, false);

                        List<object> enumeration = new List<object>();
                        bool containsNull = false;
                        if (Domain != DomainEnum.WholeDomain)
                        {
                            IComparable from;
                            IComparable to;
                            parseFromTo(dataType, out from, out to);
                            foreach (ValueFrequencyPair dfItem in df.data)
                            {
                                IComparable enumItem;
                                if (!GenericColumn.ParseValue(dfItem.value, df.dataType, out enumItem))
                                    containsNull = true;
                                if (enumItem.CompareTo(from) >= 0 && enumItem.CompareTo(to) <= 0)
                                    enumeration.Add(enumItem);
                            }
                        }
                        else
                        {
                            foreach (ValueFrequencyPair dfItem in df.data)
                            {
                                IComparable enumItem;
                                if (!GenericColumn.ParseValue(dfItem.value, df.dataType, out enumItem))
                                    containsNull = true;
                                enumeration.Add(enumItem);
                            }
                        }
                        result.CreateEnums(enumeration.ToArray(), containsNull, true);

                        return result;
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

        private Guid _cachesReloadFlag = Guid.NewGuid();
        private Guid _lastReloadFlag;
        private Dictionary<string, BitString> _cachedValueBitStrings = null;
        public Dictionary<string, BitString> GetBitStrings(bool fallOnError)
        {
            lock (this)
            {
                return ExceptionsHandler.GetResult<Dictionary<string, BitString>>(
                    fallOnError,
                    delegate
                    {
                        // get primary key
                        ColumnFunctionsPrx prx = GetColumnFunctionsPrx(fallOnError);
                        if (prx == null)
                            return null;
                        string[] pks = prx.getColumnInfo().dataTable.primaryKeyColumns;

                        GenericColumn gc = GetGenericColumn(fallOnError);
                        Attribute<IComparable> att = GetAttribute(true);
                        if (_lastReloadFlag == null || _lastReloadFlag != _cachesReloadFlag)
                        {
                            _lastReloadFlag = _cachesReloadFlag;
                            if (gc == null || att == null)
                                return null;
                            _cachedValueBitStrings = att.GetBitStrings(gc.GetSelect(pks));
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

        public BitString GetBitString(string categoryName, bool fallOnError)
        {
            lock (this)
            {
                return ExceptionsHandler.GetResult<BitString>(
                    fallOnError,
                    delegate
                    {
                        Dictionary<string, BitString> cachedValueBitStrings = GetBitStrings(fallOnError);
                        if (cachedValueBitStrings == null)
                            return null;
                        else
                            return cachedValueBitStrings[categoryName];
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

                        if (GenericColumn.CompareCardinality(columnCardinality, CardinalityEnum.Nominal) <= 0)
                            return CardinalityEnum.Nominal;
                        else
                        {
                            if (!ordered)
                                return CardinalityEnum.Nominal;
                            else
                            {
                                if (GenericColumn.CompareCardinality(columnCardinality, CardinalityEnum.Cardinal) >= 0)
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
                        List<IComparable> partialResult = tmp.GetSingleValues(tmp.GetNotMissingsCategorieIds(XCategory));
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
                    return null;
                },
                delegate
                {
                    return null;
                },
                _boxModule.StringIceIdentity
                );
        }

        private GuidStruct getGuidStruct()
        {
            // TODO 
            return new GuidStruct((new Guid()).ToString());
        }

        #endregion

        #region Ice Functions

        public override string getAttribute(Current current__)
        {
            return Guha.Attribute.Serializer.Serialize(GetAttribute(true).Export());
        }

        public override ValuesAndFrequencies getCategoriesAndFrequencies(Current current__)
        {
            return GetCategoriesAndFrequencies(true);
        }

        public override CardinalityEnum GetAttributeCardinality(Current current__)
        {
            if (GenericColumn.CompareCardinality(
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
            return getGuidStruct();
        }

        public override GuidAttributeNamePair[] GetAttributeNames(Current current__)
        {
            return new GuidAttributeNamePair[]
                {
                    new GuidAttributeNamePair(getGuidStruct(), NameInLiterals),
                };
        }

        public override BitString GetBitString(string categoryId, Current current__)
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

        public override string GetMissingInformationCategoryId(Current current__)
        {
            return XCategory;
        }

        #endregion
    }
}