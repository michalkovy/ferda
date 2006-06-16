using System;
using System.Collections.Generic;
using Ferda.Guha.Attribute;
using Ferda.Guha.Data;
using Ferda.Modules.Helpers.Caching;
using Ice;
using Exception=System.Exception;

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

        public StringTI IncludeNullCategory
        {
            get { return _nullCategoryName; }
        }

        public string Domain
        {
            get { return _boxModule.GetPropertyString(PropDomain); }
        }

        public string From
        {
            get { return _boxModule.GetPropertyString(PropFrom); }
        }

        public string To
        {
            get { return _boxModule.GetPropertyString(PropTo); }
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

        private CacheFlag _cacheFlag = new CacheFlag();
        private Attribute<IComparable> _cachedValue = null;
        //DbDataTypeEnum _columnDataType = DbDataTypeEnum.UnknownType;
        //Ferda.Guha.Data.CardinalityEnum _columnCardinality = Ferda.Guha.Data.CardinalityEnum.Nominal;
        private string _nullCategoryName = null;

        public Attribute<IComparable> GetAttribute(bool fallOnError)
        {
            //_columnDataType = DbDataTypeEnum.UnknownType;
            //_columnCardinality = Ferda.Guha.Data.CardinalityEnum.Nominal;
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

            //_columnDataType = tmp.dataType;
            //_columnCardinality = tmp.cardinality;

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
            cacheSetting.Add(BoxInfo.typeIdentifier + PropDomain, Domain);
            cacheSetting.Add(BoxInfo.typeIdentifier + PropFrom, From);
            cacheSetting.Add(BoxInfo.typeIdentifier + PropTo, To);

            if (_cacheFlag.IsObsolete(connSetting.LastReloadRequest, cacheSetting)
                || (_cachedValue == null && fallOnError))
            {
                _cachedValue = ExceptionsHandler.GetResult<Attribute<IComparable>>(
                    fallOnError,
                    delegate
                        {
                            ValuesAndFrequencies df = prx.getDistinctsAndFrequencies();
                            DbSimpleDataTypeEnum simpleDbDataType = GenericColumn.GetSimpleDataType(df.dataType);

                            Attribute<IComparable> result =
                                (Attribute<IComparable>) Common.GetAttributeObject(simpleDbDataType, false);

                            List<object> enumeration = new List<object>();
                            IComparable from;
                            GenericColumn.TryParseValue(From, df.dataType, out from);
                            IComparable to;
                            GenericColumn.TryParseValue(From, df.dataType, out to);
                            bool containsNull = false;
                            foreach (ValueFrequencyPair dfItem in df.data)
                            {
                                if (dfItem.value == nullValueConstant.value)
                                    containsNull = true;
                                else
                                {
                                    IComparable enumItem;
                                    GenericColumn.TryParseValue(dfItem.value, df.dataType, out enumItem);
                                    if (enumItem.CompareTo(from) >= 0 && enumItem.CompareTo(to) <= 0)
                                        enumeration.Add(enumItem);
                                }
                            }
                            result.CreateEnums(enumeration.ToArray(), containsNull, true);

                            return result;
                            //GenericColumn column = GenericDatabaseCache.GetGenericDatabase(connSetting)[tmp.dataTable.dataTableName].GetGenericColumn(tmp.columnSelectExpression);
                            //System.Data.DataTable dt = column.GetDistinctsAndFrequencies(null); //TODO WHERE
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

        public string[] GetCategoriesNames(bool fallOnError)
        {
            Attribute<IComparable> tmp = GetAttribute(fallOnError);
            return ExceptionsHandler.GetResult<string[]>(
                fallOnError,
                delegate
                    {
                        if (tmp != null)
                        {
                            List<string> result = new List<string>(tmp.Keys);
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

        #endregion

        #region Ice Functions

        public override string getAttribute(Current current__)
        {
            return Guha.Attribute.Serializer.Serialize(GetAttribute(true).Export());
        }

        public override ValuesAndFrequencies getCategoriesAndFrequencies(Current current__)
        {
            //TODO
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion
    }
}