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
using Ferda.Guha.Attribute.DynamicAlgorithm;

namespace Ferda.Modules.Boxes.DataPreparation.Categorization.EquidistantIntervals
{
    internal static class Retyper<T>
    {
        public static object [] Retype(T[] input)
        {
            object[] result = new object[input.Length];
            for (int i = 0; i < input.Length; i++)
            {
                result[i] = (object)input[i];
            }
            return result;
        }

        public static int[] RetypeToInt(long[] input)
        {
            int[] result = new int[input.Length];
            for (int i = 0; i < input.Length; i++)
            {
                result[i] = (int)input[i];
            }
            return result;
        }
    }

    internal class Functions : AttributeFunctionsDisp_, IFunctions
    {
        /// <summary>
        /// The box module.
        /// </summary>
        protected BoxModuleI _boxModule;

        #region Properties

        public const string PropNameInLiterals = "NameInLiterals";
        public const string PropCountOfCategories = "CountOfCategories";
        public const string PropLength = "Length";
        public const string PropClosedFrom = "ClosedFrom";
        public const string PropXCategory = "XCategory";
        public const string PropIncludeNullCategory = "IncludeNullCategory";
        public const string PropDomain = "Domain";
        public const string PropFrom = "From";
        public const string PropTo = "To";
        public const string PropCardinality = "Cardinality";
        public const string SockColumn = "Column";
        public const string PropCategories = "Categories";

        public GuidStruct Guid
        {
            get { return BoxInfoHelper.GetGuidStructFromProperty("Guid", _boxModule); }
        }

        public string NameInLiterals
        {
            get { return _boxModule.GetPropertyString(PropNameInLiterals); }
        }

        public Side ClosedFrom
        {
            get
            {
                if (_boxModule.GetPropertyString(PropClosedFrom).Equals("Left"))
                    return Side.Left;
                else
                    return Side.Right;
            }
        }

        public long Count
        {
            get
            {
                return  _boxModule.GetPropertyLong(PropCountOfCategories);
            }
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

        public string XCategory
        {
            get { return _boxModule.GetPropertyString(PropXCategory); }
        }

        private string _nullCategoryName = null;

        public StringTI IncludeNullCategory
        {
            get { return _nullCategoryName; }
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

        /*
        /// <summary>
        /// Creates equidistant intervals from the given range of values.
        /// </summary>
        /// <param name="_from">Requested starting point of intervals.</param>
        /// <param name="_to">Requested end point of intervals.</param>
        /// <param name="_length">Requested length of one interval.</param>
        /// <returns>An array of dividing points. The first point returned is the right bound of the first value (the left bound can be -INF), the last point returned is the left bound of the last interval (right bound can be INF).</returns>
        public static object[] GenerateIntevals(double _from, double _to, int _length)
        {
            double actualValue = _from;
            object[] intervalsArray = new object[Convert.ToInt32(Math.Ceiling((_to - _from) / _length)) - 1];

            int index = 0;
            while (actualValue < _to - _length)
            {
                intervalsArray[index] = actualValue + _length;
                actualValue += _length;
                index++;
            }
            return intervalsArray;
        }

        /// <summary>
        /// Creates equidistant intervals from the given range of values.
        /// </summary>
        /// <param name="_from">Requested starting point of intervals.</param>
        /// <param name="_to">Requested end point of intervals.</param>
        /// <param name="_length">Requested length of one interval.</param>
        /// <returns>An array of dividing points. The first point returned is the right bound of the first value (the left bound can be -INF), the last point returned is the left bound of the last interval (right bound can be INF).</returns>
        public static object[] GenerateIntevals(int _from, int _to, int _length)
        {
            int actualValue = _from;
            object[] intervalsArray = new object[Convert.ToInt32(Math.Ceiling(((double)_to - (double)_from) / _length)) - 1];

            int index = 0;
            while (actualValue < _to - _length)
            {
                intervalsArray[index] = actualValue + _length;
                actualValue += _length;
                index++;
            }
            return intervalsArray;
        }

        /// <summary>
        /// Creates equidistant intervals from the given range of values.
        /// </summary>
        /// <param name="_from">Requested starting point of intervals.</param>
        /// <param name="_to">Requested end point of intervals.</param>
        /// <param name="_length">Requested length of one interval.</param>
        /// <returns>An array of dividing points. The first point returned is the right bound of the first value (the left bound can be -INF), the last point returned is the left bound of the last interval (right bound can be INF).</returns>
        public static object[] GenerateIntevals(uint _from, uint _to, uint _length)
        {
            uint actualValue = _from;
            object[] intervalsArray = new object[Convert.ToInt32(Math.Ceiling(((double)_to - (double)_from) / _length)) - 1];

            int index = 0;
            while (actualValue < _to - _length)
            {
                intervalsArray[index] = actualValue + _length;
                actualValue += _length;
                index++;
            }
            return intervalsArray;
        }*/

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
                GenericColumn.ParseValue(To, dataType, out to);
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

        private object[] Retype(int [] input)
        {
            object [] result = new object[input.Length];
            for (int i = 0; i < input.Length; i++)
            {
                result[i] = (object)input[i];
            }
            return result;
        }

        private Guid _cachesReloadFlag = System.Guid.NewGuid();
        private Guid _lastReloadFlag;
        private Dictionary<string, BitStringIce> _cachedValueBitStrings = null;
        private long _lastBSQueryTicks = 0;

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

        public ColumnFunctionsPrx GetColumnFunctionsPrx(bool fallOnError)
        {
            return SocketConnections.GetPrx<ColumnFunctionsPrx>(
                _boxModule,
                SockColumn,
                ColumnFunctionsPrxHelper.checkedCast,
                fallOnError);
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
                            GenericDatabaseCache.GetGenericDatabase(connSetting)[column.dataTable.dataTableName].GetGenericColumn(column.columnSelectExpression);
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

        /// <summary>
        /// Gets the attribute structure of the current attribute
        /// </summary>
        /// <param name="fallOnError">If the method should fall
        /// on error</param>
        /// <returns>The attribute structure</returns>
        public Attribute<IComparable> GetAttribute(bool fallOnError)
        {
            //getting the proxy of a column
            ColumnFunctionsPrx columnPrx = GetColumnFunctionsPrx(fallOnError);
            if (columnPrx == null)
                return null;

            //getting a info about a column
            ColumnInfo columnInfo =
                ExceptionsHandler.GetResult<ColumnInfo>(
                    fallOnError,
                    columnPrx.getColumnInfo,
                    delegate
                    {
                        return null;
                    },
                    _boxModule.StringIceIdentity
                    );
            if (columnInfo == null)
                return null;

            //getting the connection setting
            DatabaseConnectionSettingHelper connSetting =
                new DatabaseConnectionSettingHelper(columnInfo.dataTable.databaseConnectionSetting);

            //creating a new cache
            Dictionary<string, IComparable> cacheSetting = new Dictionary<string, IComparable>();

            //adding the information about the database to the cache
            cacheSetting.Add(
                Datasource.Database.BoxInfo.typeIdentifier + Datasource.Database.Functions.PropConnectionString,
                connSetting);
            //adding the information about the data table to the cache
            cacheSetting.Add(Datasource.DataTable.BoxInfo.typeIdentifier + Datasource.DataTable.Functions.PropName,
                             columnInfo.dataTable.dataTableName);
            //adding the information about the column select expression to the cache
            cacheSetting.Add(
                Datasource.Column.BoxInfo.typeIdentifier + Datasource.Column.Functions.PropSelectExpression,
                columnInfo.columnSelectExpression);
            //adding the  information about the column cardinality to the cache
            cacheSetting.Add(Datasource.Column.BoxInfo.typeIdentifier + Datasource.Column.Functions.PropCardinality,
                             columnInfo.cardinality);
            //adding other attribute information to the cache
            cacheSetting.Add(BoxInfo.typeIdentifier + PropDomain, Domain.ToString());
            cacheSetting.Add(BoxInfo.typeIdentifier + PropFrom, From);
            cacheSetting.Add(BoxInfo.typeIdentifier + PropTo, To);
            cacheSetting.Add(BoxInfo.typeIdentifier + PropLength, Count);

            //count of categories
            int count = (int)Count;

            //Loading the values from cache or reloading it from the database...
            if (_cacheFlag.IsObsolete(connSetting.LastReloadRequest, cacheSetting)
                || (_cachedValue == null && fallOnError))
            {
                _cachesReloadFlag = System.Guid.NewGuid();
                _cachedValue = ExceptionsHandler.GetResult<Attribute<IComparable>>(
                    fallOnError,
                    delegate
                    {
                        //zatim nechapu
                        _nullCategoryName = null;
                        
                        //getting the column
                        GenericColumn column = GetGenericColumn(fallOnError);
                        if (column == null)
                            return null;

                        //creating an empty result
                        Attribute<IComparable> result =
                            (Attribute<IComparable>)Common.GetAttributeObject(column.DbSimpleDataType, true);

                        //string representation of max and min values of the column
                        string stringMin;
                        string stringMax;

                        //getting the min and max values of the column from the statistics
                        if (Domain == DomainEnum.SubDomain)
                        {
                            IComparable from;
                            IComparable to;
                            parseFromTo(column.Explain.dataType, out from, out to);
                            stringMin = (string)from;
                            stringMax = (string)to;
                        }
                        else if (Domain == DomainEnum.WholeDomain)
                        {
                            stringMin = column.Statistics.valueMin;
                            stringMax = column.Statistics.valueMax;
                        }
                        else
                            throw new NotImplementedException();

                        //creating intervals for individual data types
                        IComparable comparableMin;
                        IComparable comparableMax;
                        switch (column.Explain.dataType)
                        {
                            case DbDataTypeEnum.DoubleType:
                            case DbDataTypeEnum.FloatType:
                            case DbDataTypeEnum.DecimalType:
                                double _dmin = Convert.ToDouble(stringMin);
                                double _dmax = Convert.ToDouble(stringMax);
                                comparableMin = _dmin;
                                comparableMax = _dmax;
                                double[] _divisionPointsDouble = 
                                    Ferda.Guha.Attribute.DynamicAlgorithm.EquidistantIntervals.GenerateIntervals(
                                    _dmin, _dmax, count);
                                result.CreateIntervals(BoundaryEnum.Closed, comparableMin, Retyper<double>.Retype(
                                    _divisionPointsDouble),
                                    ClosedFrom, comparableMax, BoundaryEnum.Closed, false);
                                break;

                            case DbDataTypeEnum.IntegerType:
                            case DbDataTypeEnum.ShortIntegerType:
                            case DbDataTypeEnum.UnsignedIntegerType:
                            case DbDataTypeEnum.UnsignedShortIntegerType:
                                int _imin = Convert.ToInt32(stringMin);
                                int _imax = Convert.ToInt32(stringMax);
                                comparableMin = _imin;
                                comparableMax = _imax;
                                long[] _divisionPointsInt =
                                    Ferda.Guha.Attribute.DynamicAlgorithm.EquidistantIntervals.GenerateIntervals(
                                    _imin, _imax, count);
                                result.CreateIntervals(BoundaryEnum.Closed, comparableMin, Retyper<int>.Retype(
                                    Retyper<object>.RetypeToInt(_divisionPointsInt)),
                                    ClosedFrom, comparableMax, BoundaryEnum.Closed, false);
                                break;

                            case DbDataTypeEnum.LongIntegerType:
                            case DbDataTypeEnum.UnsignedLongIntegerType:
                                long _lmin = Convert.ToInt64(stringMin);
                                long _lmax = Convert.ToInt64(stringMax);
                                comparableMin = _lmin;
                                comparableMax = _lmax;
                                long[] _divisionPointsLong =
                                    Ferda.Guha.Attribute.DynamicAlgorithm.EquidistantIntervals.GenerateIntervals(
                                    _lmin, _lmax, count);
                                result.CreateIntervals(BoundaryEnum.Closed, comparableMin, Retyper<long>.Retype(
                                    _divisionPointsLong),
                                    ClosedFrom, comparableMax, BoundaryEnum.Closed, false);
                                break;

                            case DbDataTypeEnum.DateTimeType:
                                DateTime _datemin = Convert.ToDateTime(stringMin);
                                DateTime _datemax = Convert.ToDateTime(stringMax);
                                comparableMin = _datemin;
                                comparableMax = _datemax;
                                DateTime[] _divisionPointsDateTime =
                                    Ferda.Guha.Attribute.DynamicAlgorithm.EquidistantIntervals.GenerateIntervals(
                                    _datemin, _datemax, count, false);
                                result.CreateIntervals(BoundaryEnum.Closed, comparableMin, Retyper<DateTime>.Retype(_divisionPointsDateTime),
                                    ClosedFrom, comparableMax, BoundaryEnum.Closed, false);
                                break;

                            default:
                                throw new ArgumentException("Data type not supported");
                        }
                        _nullCategoryName = result.NullContainingCategory;

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

 //       private Guid _cachesReloadFlag = System.Guid.NewGuid();
  //      private Guid _lastReloadFlag;
   //     private Dictionary<string, BitStringIce> _cachedValueBitStrings = null;
   //     private long _lastBSQueryTicks = 0;
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
                        if (partialResult == null)
                            return null;
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

        #endregion

        #region ICE functions

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

        public override string[] GetMissingInformationCategoryId(Current current__)
        {
            if (String.IsNullOrEmpty(XCategory))
                return new string[0];
            else
                return new string[] { XCategory }; ;
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
            return Guha.Attribute.Serializer.Serialize(GetAttribute(true).Export());
        }

        public override ValuesAndFrequencies getCategoriesAndFrequencies(Current current__)
        {
            return GetCategoriesAndFrequencies(true);
        }

        public override int[] GetCountVector(string masterIdColumn, string masterDatatableName, string detailIdColumn, Current current__)
        {
            GenericColumn _column = GetGenericColumn(true);

            string detailId = String.Empty;
            if (String.IsNullOrEmpty(detailIdColumn))
                detailId =
            GetColumnFunctionsPrx(true).getColumnInfo().dataTable.primaryKeyColumns[0];
            else
                detailId = detailIdColumn;

            DataTable _table = 
                _column.GetCountVector(masterIdColumn, masterDatatableName, detailId);
            int[] result = new int[_table.Rows.Count];
            for (int i = 0; i < _table.Rows.Count; i++)
            {
                result[i] = (int)_table.Rows[i][0];
            }
            return result;
        }

        public override bool GetNextBitString(int skipFirstN, out BitStringIceWithCategoryId bitString, Current current__)
        {
            bitString = new BitStringIceWithCategoryId();
            return false;
        }

        public override long GetMaxBitStringCount(Current current__)
        {
            return 0;
        }

        #endregion

        #region IFunctions Members

        public void setBoxModuleInfo(BoxModuleI boxModule, IBoxInfo boxInfo)
        {
            _boxModule = boxModule;
        }

        #endregion
    }
}