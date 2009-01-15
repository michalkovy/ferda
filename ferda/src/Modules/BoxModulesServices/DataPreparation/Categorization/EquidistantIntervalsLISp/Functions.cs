// Functions.cs - functionality for Equidistant intervals box
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
using Ferda.Guha.Attribute.DynamicAlgorithm;
using Ferda.Modules.Boxes.DataPreparation.Categorization;

namespace Ferda.Modules.Boxes.DataPreparation.Categorization.EquidistantIntervalsLISp
{
    internal class Functions : AttributeFunctionsDisp_, IFunctions
    {
        /// <summary>
        /// The box module.
        /// </summary>
        protected BoxModuleI _boxModule;

        #region Properties

        public const string PropNameInBooleanAttributes = "NameInBooleanAttributes";
        public const string PropCountOfCategories = "CountOfCategories";
        public const string PropLength = "Length";
        public const string PropClosedFrom = "ClosedFrom";
        public const string PropXCategory = "XCategory";
        public const string PropIncludeNullCategory = "IncludeNullCategory";
        public const string PropDomain = "Domain";
        public const string PropFrom = "From";
        public const string PropTo = "To";
        public const string PropCardinality = "Cardinality";
        public const string PropCategories = "Categories";

        /// <summary>
        /// Guid
        /// </summary>
        public GuidStruct Guid
        {
            get { return BoxInfoHelper.GetGuidStructFromProperty("Guid", _boxModule); }
        }

        /// <summary>
        /// Name in literals
        /// </summary>
        public string NameInBooleanAttributes
        {
            get { return _boxModule.GetPropertyString(PropNameInBooleanAttributes); }
        }

        /// <summary>
        /// Intervals closed from
        /// </summary>
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

        /// <summary>
        /// Requested length of intervals
        /// </summary>
        public double Length
        {
            get
            {
                return _boxModule.GetPropertyDouble(PropLength);
            }
        }

        /// <summary>
        /// Count of intervals
        /// </summary>
        public LongTI CountOfCategories
        {
            get
            {
                Attribute<IComparable> tmp =
                    GetAttribute(false);
                return (tmp != null) ? tmp.Count : 0;
            }
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
        /// Xcategory
        /// </summary>
        public string XCategory
        {
            get { return _boxModule.GetPropertyString(PropXCategory); }
        }

        private string _nullCategoryName = null;

        /// <summary>
        /// Null category name
        /// </summary>
        public StringTI IncludeNullCategory
        {
            get { return _nullCategoryName; }
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

        #endregion

        #region Methods


        /// <summary>
        /// Parses from and to values
        /// </summary>
        /// <param name="dataType"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
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


        private object[] Retype(int[] input)
        {
            object[] result = new object[input.Length];
            for (int i = 0; i < input.Length; i++)
            {
                result[i] = (object)input[i];
            }
            return result;
        }

        private Guid _cachesReloadFlag = System.Guid.NewGuid();
        private Guid _lastReloadFlag = System.Guid.Empty;
        private Dictionary<string, BitStringIce> _cachedValueBitStrings = null;
        private long _lastBSQueryTicks = 0;

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
            return SocketConnections.GetPrx<ColumnFunctionsPrx>(
                _boxModule,
                Public.SockColumn,
                ColumnFunctionsPrxHelper.checkedCast,
                fallOnError);
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
            cacheSetting.Add(Datasource.Column.BoxInfo.typeIdentifier + Datasource.Column.Functions.PropCardinality,
                 column.cardinality);

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
            cacheSetting.Add(BoxInfo.typeIdentifier + PropLength, (double)Length);
            cacheSetting.Add(BoxInfo.typeIdentifier + PropClosedFrom, ClosedFrom);

            //requested length of intervals
            if (Length == 0)
            {
                return null;
            }

            //Loading the values from cache or recounting them...
            if (_cacheFlag.IsObsolete(connSetting.LastReloadRequest, cacheSetting)
                || (_cachedValue == null && fallOnError))
            {
                _cachesReloadFlag = System.Guid.NewGuid();
                _cachedValue = ExceptionsHandler.GetResult<Attribute<IComparable>>(
                    fallOnError,
                    delegate
                    {
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

                        //getting the min and max values of the column from the user input
                        if (Domain == DomainEnum.SubDomain)
                        {
                            IComparable from;
                            IComparable to;
                            parseFromTo(column.Explain.dataType, out from, out to);
                            stringMin = from.ToString();
                            stringMax = to.ToString();
                        }
                        else if (Domain == DomainEnum.WholeDomain)
                        {
                            //or from the column statistics, if no user input is given
                            stringMin = column.Statistics.valueMin;
                            stringMax = column.Statistics.valueMax;
                        }
                        else
                            throw new NotImplementedException();

                        //creating intervals for individual data types
                        IComparable comparableMin;
                        IComparable comparableMax;

                        //creating intervals of the type based on the connected column datatype
                        //switch branches differ only in the datatype, so only one branch will be explained
                        switch (column.DbSimpleDataType)
                        {
                            case DbSimpleDataTypeEnum.FloatSimpleType:
                            case DbSimpleDataTypeEnum.DoubleSimpleType:
                                //for double or float type, the same algorithm is used
                                //which generates division points of double types
                                //for float, the division points must be retyped to float

                                //converting min and max values for the attribute domain
                                double _dmin = Convert.ToDouble(stringMin);
                                double _dmax = Convert.ToDouble(stringMax);

                                double[] _divisionPointsDouble;
                                try
                                {
                                    //generation of the division points
                                    _divisionPointsDouble =
                                        Ferda.Guha.Attribute.DynamicAlgorithm.EquidistantIntervals.GenerateIntervalsLISp(
                                        _dmin, _dmax, Length);
                                }
                                catch
                                {
                                    throw new ArgumentOutOfRangeException();
                                }

                                if (column.DbSimpleDataType == DbSimpleDataTypeEnum.FloatSimpleType)
                                {
                                    comparableMin = (float)_dmin;
                                    comparableMax = (float)_dmax;
                                    float[] _divisionPointsFloat;
                                    try
                                    {
                                        //retyping double division points to float
                                        _divisionPointsFloat =
                                            Retyper<object>.RetypeDoubleToFloat(_divisionPointsDouble);
                                    }
                                    catch
                                    {
                                        throw new ArgumentOutOfRangeException();
                                    }
                                    //creating intervals in the attribute
                                    result.CreateIntervals(
                                        BoundaryEnum.Closed, comparableMin,
                                        Retyper<float>.Retype(_divisionPointsFloat),
                                        ClosedFrom, comparableMax, BoundaryEnum.Closed,
                                        false
                                        );
                                }
                                else
                                {
                                    comparableMin = _dmin;
                                    comparableMax = _dmax;
                                    result.CreateIntervals(
                                        BoundaryEnum.Closed, comparableMin,
                                        Retyper<double>.Retype(_divisionPointsDouble),
                                        ClosedFrom, comparableMax,
                                        BoundaryEnum.Closed, false
                                        );
                                }
                                break;

                            case DbSimpleDataTypeEnum.StringSimpleType:
                                throw new Exception("Cannot create equidistant intervals from a column containing strings.\nFerda cannot determine how to measure distance between strings.");

                            case DbSimpleDataTypeEnum.ShortSimpleType:
                            case DbSimpleDataTypeEnum.IntegerSimpleType:
                            case DbSimpleDataTypeEnum.LongSimpleType:
                                long _lmin = Convert.ToInt64(stringMin);
                                long _lmax = Convert.ToInt64(stringMax);
                                long[] _divisionPointsLong;

                                try
                                {
                                    _divisionPointsLong =
                                        Ferda.Guha.Attribute.DynamicAlgorithm.EquidistantIntervals.GenerateIntervalsLISp(
                                        _lmin, _lmax, System.Convert.ToInt32(Length));
                                }
                                catch
                                {
                                    throw new ArgumentOutOfRangeException();
                                }

                                if (column.DbSimpleDataType == DbSimpleDataTypeEnum.ShortSimpleType)
                                {
                                    comparableMin = (short)_lmin;
                                    comparableMax = (short)_lmax;
                                    short[] _divisionPointsShort =
                                        Retyper<object>.RetypeLongToShort(_divisionPointsLong);
                                    result.CreateIntervals(
                                        BoundaryEnum.Closed, comparableMin,
                                        Retyper<short>.Retype(_divisionPointsShort),
                                        ClosedFrom, comparableMax,
                                        BoundaryEnum.Closed, false
                                        );
                                }
                                else

                                    if (column.DbSimpleDataType == DbSimpleDataTypeEnum.IntegerSimpleType)
                                    {
                                        comparableMin = (int)_lmin;
                                        comparableMax = (int)_lmax;
                                        int[] _divisionPointsInt;

                                        try
                                        {
                                            _divisionPointsInt =
                                                Retyper<object>.RetypeLongToInt(_divisionPointsLong);
                                        }
                                        catch
                                        {
                                            throw new ArgumentOutOfRangeException();
                                        }
                                        result.CreateIntervals(
                                            BoundaryEnum.Closed, comparableMin,
                                            Retyper<int>.Retype(_divisionPointsInt),
                                            ClosedFrom, comparableMax,
                                            BoundaryEnum.Closed, false
                                            );
                                    }
                                    else
                                    {
                                        comparableMin = (long)_lmin;
                                        comparableMax = (long)_lmax;
                                        result.CreateIntervals(
                                            BoundaryEnum.Closed, comparableMin,
                                            Retyper<long>.Retype(_divisionPointsLong),
                                            ClosedFrom, comparableMax,
                                            BoundaryEnum.Closed, false
                                            );
                                    }
                                break;
                                /*
                            case DbSimpleDataTypeEnum.DateTimeSimpleType:
                                DateTime _datemin = Convert.ToDateTime(stringMin);
                                DateTime _datemax = Convert.ToDateTime(stringMax);
                                comparableMin = _datemin;
                                comparableMax = _datemax;
                                DateTime[] _divisionPointsDateTime;
                                try
                                {
                                    _divisionPointsDateTime =
                                        Ferda.Guha.Attribute.DynamicAlgorithm.EquidistantIntervals.GenerateIntervalsLISp(
                                        _datemin, _datemax, count, false);
                                }
                                catch
                                {
                                    throw new ArgumentOutOfRangeException();
                                }
                                result.CreateIntervals(
                                    BoundaryEnum.Closed, comparableMin,
                                    Retyper<DateTime>.Retype(_divisionPointsDateTime),
                                    ClosedFrom, comparableMax,
                                    BoundaryEnum.Closed, false
                                    );
                                break;*/

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
        /// Gets categories ids
        /// </summary>
        /// <param name="fallOnError"></param>
        /// <returns></returns>
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

        #endregion

        #region ICE functions

        /// <summary>
        /// Gets the name of the column from which the attribute and bit string generator
        /// is created. Added for PMML purposes. 
        /// </summary>
        /// <param name="current__">ICE stuff</param>
        /// <returns></returns>
        public override string GetColumnName(Current current__)
        {
            return GetColumnFunctionsPrx(true).getColumnInfo().columnSelectExpression;
        }

        /// <summary>
        /// Returns cardinality of the attribute (nominal/ordinal/cyclic ordinal/cardinal).
        /// </summary>
        /// <param name="current__">ICE stuff</param>
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
        /// Returns the identification of the attribute.
        /// </summary>
        /// <param name="current__">ICE stuff</param>
        /// <returns>Identification of the attribute</returns>
        public override GuidStruct GetAttributeId(Current current__)
        {
            return Guid;
        }

        /// <summary>
        /// Gets attribute names
        /// </summary>
        /// <param name="current__">ICE stuff</param>
        /// <returns>Attribute names</returns>
        public override GuidAttributeNamePair[] GetAttributeNames(Current current__)
        {
            return new GuidAttributeNamePair[]
                {
                    new GuidAttributeNamePair(Guid, NameInBooleanAttributes),
                };
        }

        /// <summary>
        /// Returns a bit string for category in the 
        /// <paramref name="categoryId"/> parameter.
        /// </summary>
        /// <param name="categoryId">Category identification
        /// (name of the category)</param>
        /// <returns>BitString</returns>
        public override BitStringIce GetBitString(string categoryId, Current current__)
        {
            return GetBitString(categoryId, true);
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
            return GetCategoriesIds(true);
        }

        /// <summary>
        /// Returns numerical values of the categories. These numerical
        /// values can be returned only for the <c>ordinal, cyclic ordinal
        /// and cardinal</c> attributes. Otherwise, <c>null</c> or
        /// <c>double[0]</c> is returned
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
            return GetCategoriesNumericValues(true);
        }

        /// <summary>
        /// Returns identification (category name) of a
        /// category that contains missing information.
        /// </summary>
        /// <param name="current__">ICE stuff</param>
        /// <returns>Missing information category name
        /// </returns>
        public override string[] GetMissingInformationCategoryId(Current current__)
        {
            if (String.IsNullOrEmpty(XCategory))
                return new string[0];
            else
                return new string[] { XCategory }; ;
        }

        /// <summary>
        /// Gets source datatable id
        /// </summary>
        /// <param name="current__">ICEstuff</param>
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
            return Guha.Attribute.Serializer.Serialize(GetAttribute(true).Export());
        }

        /// <summary>
        /// Gets categories and their frequencies in the attribute
        /// </summary>
        /// <param name="current__"></param>
        /// <returns></returns>
        public override ValuesAndFrequencies getCategoriesAndFrequencies(Current current__)
        {
            return GetCategoriesAndFrequencies(true);
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
        /// Returns information from the column about the values and frequencies
        /// of the column. This fucntion was added to the Slice desing for
        /// the PMML support.
        /// </summary>
        /// <returns>ValuesAndFrequencies structure</returns>
        public override ValuesAndFrequencies GetColumnValuesAndFrequencies(Current current__)
        {
            return GetColumnFunctionsPrx(true).getDistinctsAndFrequencies();
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