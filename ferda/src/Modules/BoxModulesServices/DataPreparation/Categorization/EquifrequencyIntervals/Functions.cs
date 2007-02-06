using System;
using System.Collections.Generic;
using System.Collections;
using System.Diagnostics;
using Ferda.Guha.Attribute;
using Ferda.Guha.Data;
using Ferda.Guha.MiningProcessor;
using Ferda.Modules.Helpers.Caching;
using Ice;
using Common = Ferda.Guha.Attribute.Common;
using Exception = System.Exception;
using System.Data;
using System.Data.Common;

namespace Ferda.Modules.Boxes.DataPreparation.Categorization.EquifrequencyIntervals
{
    internal static class Retyper<T>
    {
        public static object[] Retype(T[] input)
        {
            object[] result = new object[input.Length];
            for (int i = 0; i < input.Length; i++)
            {
                result[i] = (object)input[i];
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

        public LongTI CountOfCategories
        {
            get
            {
                return _boxModule.GetPropertyLong(PropCountOfCategories);
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

        #region Equifrequency intervals generating engine


        /// <summary>
        /// Creates equifrequency intervals from the sorted array of values from database.
        /// </summary>
        /// <param name="intervals">Requested number of intervals.</param>
        /// <param name="sortedValues">It is assumed that the values from database are of type double, although any type that converts to double will do. The array must be already sorted though.</param>
        /// <returns>An array of dividing points. The first point returned is the right bound of the first value (the left bound can be -INF), the last point returned is the left bound of the last interval (right bound can be INF).</returns>
        /// <remarks>
        /// <para>This method is an optimization algorithm. It heuristically checks many different
        /// combinations of dividing points and computes scoring for every such division.
        /// The scoring function is in fact a least square method and the optimization is searching
        /// for point of minimum error. See more details in other types of documentation.</para>
        /// </remarks>
        public static object[] GenerateIntervals(int intervals, object[] sortedValues)
        {
            if ((sortedValues == null) || (sortedValues.Length == 0))
                throw new ArgumentException("The reference to the array of sorted values is null or contains no elements.", "sortedValues");

            if (intervals < 1)
                throw new ArgumentOutOfRangeException("intervals", intervals, "The number of intervals must be at least 1.");

            // create an array list of values and their counts
            ArrayList dataArray = new ArrayList();
            int i = 0;
            object currentValue = null;
            while (i < sortedValues.Length)
            {
                currentValue = sortedValues[i];
                i++;
                if (currentValue != null)
                    break;
            }
            if (currentValue == null)
                throw new ArgumentException("The array of values contains only null values.");

            int currentCount = 1;
            int totalCount = 1;
            while (i < sortedValues.Length)
            {
                if (sortedValues[i] != null)
                {
                    Debug.Assert(((IComparable)currentValue).CompareTo((IComparable)sortedValues[i]) <= 0);
                    if (currentValue.Equals(sortedValues[i]))
                    {
                        currentCount++;
                    }
                    else
                    {
                        dataArray.Add(new Data(currentValue, currentCount));
                        currentValue = sortedValues[i];
                        currentCount = 1;
                    }
                    totalCount++;
                }
                i++;
            }
            dataArray.Add(new Data(currentValue, currentCount));

            // assert that the number of intervals is less than or equal to the total number of different values
            if (dataArray.Count < intervals)
                throw new ArgumentOutOfRangeException("intervals", intervals, "The requested number of intervals is less than the total number of different values.");

            // initialize the cache
            ResultCache cache = new ResultCache(intervals);

            // start the recursion
            Result result = FindSplit(dataArray, new Interval(0, dataArray.Count), intervals, (float)totalCount / (float)intervals, cache);

            // get the split points
            object[] resultObjects = new object[intervals - 1];
            for (i = 0; i < intervals - 1; i++)
            {
                resultObjects[i] = ((Data)dataArray[((Interval)result.Intervals[i]).Right]).Value;
            }
            return resultObjects;
        }

        private const float initPenalty = Single.MaxValue;
        private const float stopLimit = 5.0f;
        private const int growLimit = 3;

        private static Result FindSplit(ArrayList dataArray, Interval bounds, int intervals, float optimum, ResultCache cache)
        {
            // check if there is enough split points
            Debug.Assert(bounds.Right - bounds.Left >= intervals);

            // test the end of recursion (no splitting)
            if (intervals == 1)
            {
                Result result = new Result();
                result.Intervals.Add(bounds);
                int count = 0;
                for (int i = bounds.Left; i < bounds.Right; i++)
                    count += ((Data)dataArray[i]).Count;
                result.Cost = ResultPenalty(count, optimum);
                return result;
            }

            // test the end of recursion (exact splitting, no choice)
            if (intervals == bounds.Right - bounds.Left)
            {
                Result result = new Result();
                result.Cost = 0.0f;
                for (int i = bounds.Left; i < bounds.Right; i++)
                {
                    result.Intervals.Add(new Interval(i, i + 1));
                    result.Cost += ResultPenalty(((Data)dataArray[i]).Count, optimum);
                }
                return result;
            }

            // cache lookup
            {
                Result result = cache.GetResult(intervals, bounds);
                if (result != null)
                    return result;
            }

            // count objects that must be in the left part
            int leftIntervals = intervals / 2;
            int leftSum = 0;
            for (int i = 0; i < leftIntervals; i++)
                leftSum += ((Data)dataArray[bounds.Left + i]).Count;

            // add some more intervals until optimal point is reached
            int bestSplit;
            int leftOptimalSum = (int)Math.Round(optimum * leftIntervals);
            for (bestSplit = bounds.Left + leftIntervals; bestSplit < bounds.Right - (intervals - leftIntervals); bestSplit++)
            {
                if (leftSum + ((Data)dataArray[bestSplit]).Count > leftOptimalSum)
                    break;
                leftSum += ((Data)dataArray[bestSplit]).Count;
            }

            // start testing these split points (spreading to left and right)
            int leftSplit = bestSplit;
            int rightSplit = bestSplit + 1;
            bool leftStop = false;  // there's always at least one solution
            bool rightStop = (rightSplit > bounds.Right - (intervals - leftIntervals));  // go right only if there is another possible split point

            // spread to both sides and search for better solutions
            Result bestResult = new Result(), leftTmpResult, rightTmpResult;
            bestResult.Cost = initPenalty;
            float leftLastScore = initPenalty, rightLastScore = initPenalty;
            int leftGrowCount = 0, rightGrowCount = 0;
            while (!leftStop || !rightStop)
            {
                if (!leftStop)
                {
                    // find solution for left and right part
                    leftTmpResult = FindSplit(dataArray, new Interval(bounds.Left, leftSplit), leftIntervals, optimum, cache);
                    rightTmpResult = FindSplit(dataArray, new Interval(leftSplit, bounds.Right), intervals - leftIntervals, optimum, cache);

                    // sum the costs of partial results
                    float sum = leftTmpResult.Cost + rightTmpResult.Cost;


                    // first solution is propagated to the right side
                    if (rightLastScore == initPenalty)
                    {
                        // save to right last value
                        rightLastScore = sum;
                    }

                    // compare this result to what we have so far
                    if (sum < bestResult.Cost)
                    {
                        // merge two partial solution to one
                        bestResult.Merge(leftTmpResult, rightTmpResult);

                        // absolute stop criterium (perfect result)
                        if (sum == 0.0f)
                            break;
                    }

                    // check stop criterium (result penalty is too big)
                    if (sum > stopLimit * bestResult.Cost)
                    {
                        // stop spreading to the left
                        leftStop = true;
                    }

                    // check stop criterium (result penalty is constantly growing, so there is
                    // probably no hope of getting better result than we have...)
                    if (sum < leftLastScore)
                    {
                        // not growing, reset the counter
                        leftGrowCount = 0;
                    }
                    else
                    {
                        // growing, increase
                        leftGrowCount++;
                        if (leftGrowCount == growLimit)
                            leftStop = true;
                    }
                    leftLastScore = sum;

                    // check if there is possibility to spread further to the left
                    if (leftSplit <= bounds.Left + leftIntervals)
                    {
                        // stop testing spreading to the left
                        leftStop = true;
                    }
                    else
                    {
                        // shift the left split to the next position
                        leftSplit--;
                    }
                }

                if (!rightStop)
                {
                    // find solution for left and right part
                    leftTmpResult = FindSplit(dataArray, new Interval(bounds.Left, rightSplit), leftIntervals, optimum, cache);
                    rightTmpResult = FindSplit(dataArray, new Interval(rightSplit, bounds.Right), intervals - leftIntervals, optimum, cache);

                    // sum the costs of partial results
                    float sum = leftTmpResult.Cost + rightTmpResult.Cost;

                    // compare this result to what we have so far
                    if (sum < bestResult.Cost)
                    {
                        // merge two partial solution to one
                        bestResult.Merge(leftTmpResult, rightTmpResult);
                    }

                    // check stop criterium (result penalty is too big)
                    if (sum > stopLimit * bestResult.Cost)
                    {
                        // stop testing spreading to the right
                        rightStop = true;
                    }

                    // check stop criterium (result penalty is constantly growing, so there is
                    // probably no hope of getting better result than we have...)
                    if (sum < rightLastScore)
                    {
                        // not growing, reset the counter
                        rightGrowCount = 0;
                    }
                    else
                    {
                        // growing, increase
                        rightGrowCount++;
                        if (rightGrowCount == growLimit)
                            rightStop = true;
                    }
                    rightLastScore = sum;

                    // check if there is possibility to spread further to the right
                    if (rightSplit >= bounds.Right - (intervals - leftIntervals))
                    {
                        // stop testing spreading to the right
                        rightStop = true;
                    }
                    else
                    {
                        // shift the right split to the next position
                        rightSplit++;
                    }
                }
            }

            // check the solution
            Debug.Assert(bestResult.Cost < initPenalty);

            // add the best result to cache
            cache.SetResult(intervals, bounds, bestResult);

            // ...and return it
            return bestResult;
        }

        private static float ResultPenalty(int count, float optimum)
        {
            float penalty;
            if ((float)count > optimum)
                penalty = ((float)count) / optimum - 1.0f;
            else
                penalty = optimum / ((float)count) - 1.0f;
            return penalty * penalty;
        }

        private class Data
        {
            public Data(object value, int count)
            {
                Value = value;
                Count = count;
            }

            public object Value
            {
                get
                {
                    return _value;
                }
                set
                {
                    _value = value;
                }
            }
            private object _value;

            public int Count
            {
                get
                {
                    return _count;
                }
                set
                {
                    Debug.Assert(value > 0);
                    _count = value;
                }
            }
            private int _count;
        }

        private class Interval
        {
            public Interval()
            {
            }

            public Interval(int left, int right)
            {
                Left = left;
                Right = right;
            }

            public int Left
            {
                get
                {
                    return _left;
                }
                set
                {
                    Debug.Assert(value >= 0);
                    _left = value;
                    if (_right < _left)
                        _right = _left;
                }
            }
            private int _left;

            public int Right
            {
                get
                {
                    return _right;
                }
                set
                {
                    Debug.Assert(value >= 0);
                    _right = value;
                    if (_left > _right)
                        _left = _right;
                }
            }
            private int _right;

            public override int GetHashCode()
            {
                return (Left << 16) ^ Right;
            }

            public override bool Equals(object obj)
            {
                Interval that = obj as Interval;
                if ((object)that == null)
                    return false;

                return System.Object.ReferenceEquals(this, that) || (this.Left == that.Left) && (this.Right == that.Right);
            }

            public static bool operator ==(Interval a, Interval b)
            {
                return System.Object.Equals(a, b);
            }

            public static bool operator !=(Interval a, Interval b)
            {
                return !System.Object.Equals(a, b);
            }
        }

        private class Result
        {
            public Result()
            {
                Intervals = new ArrayList();
            }

            public float Cost
            {
                get
                {
                    return _cost;
                }
                set
                {
                    Debug.Assert(value >= 0.0f);
                    _cost = value;
                }
            }
            private float _cost;

            public void Merge(Result one, Result two)
            {
                Intervals.Clear();
                Intervals.AddRange(one.Intervals);
                Intervals.AddRange(two.Intervals);
                Cost = one.Cost + two.Cost;
            }

            public ArrayList Intervals;
        }

        private class ResultCache
        {
            public ResultCache(int intervals)
            {
                // we will store results up to the size of ceil(intervals / 2),
                // where intervals is the requested output number of equifrequency intervals
                int size = (intervals + 1) / 2;

                // initialize cache lines (an array of hashtables)
                cacheLines = new Hashtable[size - 1];
            }

            private Hashtable[] cacheLines;

            public Result GetResult(int intervals, Interval bounds)
            {
                Debug.Assert(intervals >= 2);

                // get the index of the cache line
                int index = intervals - 2;

                // check the number of intervals
                if (index >= cacheLines.Length)
                {
                    // cache pseudo-miss... such sizes are not stored in cache
                    return null;
                }

                // get the cache line
                Hashtable cacheLine = cacheLines[index];
                if (cacheLine == null)
                {
#if PERFCOUNTERS
                    cacheMiss++;
#endif

                    // cache miss
                    return null;
                }

                // do the cache lookup
                Result result = (Result)cacheLine[bounds];
                if (result == null)
                {
#if PERFCOUNTERS
                    cacheMiss++;
#endif

                    // cache miss
                    return null;
                }
                else
                {
#if PERFCOUNTERS
                    cacheHit++;
#endif

                    // cache hit
                    return result;
                }
            }

            public void SetResult(int intervals, Interval bounds, Result result)
            {
                Debug.Assert(intervals >= 2);

                // get the index of the cache line
                int index = intervals - 2;

                // ignore storing of results that do not fit to cache
                if (index >= cacheLines.Length)
                    return;

                // get the cache line
                Hashtable cacheLine = cacheLines[index];
                if (cacheLine == null)
                {
                    // create the new cache line
                    cacheLine = new Hashtable();
                    cacheLines[index] = cacheLine;
                }

                // check that the result is not in the cache
                Debug.Assert(!cacheLine.Contains(bounds), "Result cache already contains the result.");

                cacheLine[bounds] = result;
            }
        }


        #endregion

        private object[] TypeToInt(object[] array)
        {
            object[] returnArray = new object[array.Length];

            for (int i = 0; i < array.Length; i++)
            {
                returnArray[i] = (int)array[i];
            }
            return returnArray;
        }

        private object[] TypeToDouble(object[] array)
        {
            object[] returnArray = new object[array.Length];

            for (int i = 0; i < array.Length; i++)
            {
                returnArray[i] = (double)array[i];
            }
            return returnArray;
        }

        private object[] TypeToUInt(object[] array)
        {
            object[] returnArray = new object[array.Length];

            for (int i = 0; i < array.Length; i++)
            {
                returnArray[i] = (uint)array[i];
            }
            return returnArray;
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
            cacheSetting.Add(BoxInfo.typeIdentifier + PropCountOfCategories, (long)CountOfCategories);

            if (_cacheFlag.IsObsolete(connSetting.LastReloadRequest, cacheSetting)
                || (_cachedValue == null && fallOnError))
            {
                _cachesReloadFlag = System.Guid.NewGuid();
                _cachedValue = ExceptionsHandler.GetResult<Attribute<IComparable>>(
                    fallOnError,
                    delegate
                    {
                        _nullCategoryName = null;
                        GenericColumn column = GetGenericColumn(fallOnError);
                        if (column == null)
                            return null;

                        Attribute<IComparable> result =
                            (Attribute<IComparable>)Common.GetAttributeObject(column.DbSimpleDataType, true);

                        //DbDataTypeEnum dataType = prx.getColumnInfo().dataType;
                        //ValuesAndFrequencies df = prx.getDistinctsAndFrequencies();
                        //Debug.Assert(df.dataType == dataType);
                        //DbSimpleDataTypeEnum simpleDbDataType = GenericColumn.GetSimpleDataType(df.dataType);

                        // get primary key
                        //string[] pks = tmp.dataTable.primaryKeyColumns;

                        System.Data.DataTable dt;
                        string _min;
                        string _max;

                        if (Domain == DomainEnum.SubDomain)
                        {
                            IComparable from;
                            IComparable to;
                            parseFromTo(column.Explain.dataType, out from, out to);
                            _min = (string)from;
                            _max = (string)to;
                            string columnSelectExpression =
                                column.GetQuotedQueryIdentifier();

                            dt = column.GetDistinctsAndFrequencies(
                                columnSelectExpression + ">='" + from + "' AND " + columnSelectExpression + "<='" + to + "'"
                                );

                            //dt = column.GetDistincts(columnSelectExpression + ">='" + from + "' AND " + columnSelectExpression + "<='" + to + "'");
                        }
                        else if (Domain == DomainEnum.WholeDomain)
                        {
                            dt = column.GetDistinctsAndFrequencies(String.Empty);
                            //dt = column.GetSelect(pks);
                            _min = column.Statistics.valueMin;
                            _max = column.Statistics.valueMax;
                        }
                        else
                            throw new NotImplementedException();

                       // object[] enumeration =
                       //     new object[dt.Rows.Count];

                        // enumeration.Sort();

                        //  bool containsNull = false;
                        object[] _divisionPoints;
                        int i = 0;
                        //  _divisionPoints = GenerateIntervals((int)CountOfCategories, enumeration.ToArray());
                        IComparable __min;
                        IComparable __max;

                        switch (column.Explain.dataType)
                        {
                            case DbDataTypeEnum.DoubleType:
                            case DbDataTypeEnum.FloatType:
                            case DbDataTypeEnum.DecimalType:
                                double _dmin = Convert.ToDouble(_min);
                                double _dmax = Convert.ToDouble(_max);
                                __min = _dmin;
                                __max = _dmax;

                                Ferda.Guha.Attribute.DynamicAlgorithm.ValueFrequencyPair<double>[] enumeration =
                                    new Ferda.Guha.Attribute.DynamicAlgorithm.ValueFrequencyPair<double>[dt.Rows.Count];
                                foreach (System.Data.DataRow row in dt.Rows)
                                {
                                    object v = row[0];
                                    if (v == null || v is DBNull)
                                        continue;
                                    Ferda.Guha.Attribute.DynamicAlgorithm.ValueFrequencyPair<double> tmpItem =
                                        new Ferda.Guha.Attribute.DynamicAlgorithm.ValueFrequencyPair<double>(
                                        Convert.ToDouble(row[0].ToString()),
                                        Convert.ToInt32(row[1]));
                                    enumeration[i] = tmpItem;
                                    i++;
                                }
                                i = 0;
                                _divisionPoints =
                                    Retyper<double>.Retype(
                                Ferda.Guha.Attribute.DynamicAlgorithm.EquifrequencyIntervals.GenerateIntervals
                            ((int)CountOfCategories, enumeration)
                                );
                                break;

                            case DbDataTypeEnum.IntegerType:
                            case DbDataTypeEnum.ShortIntegerType:
                            case DbDataTypeEnum.UnsignedIntegerType:
                            case DbDataTypeEnum.UnsignedShortIntegerType:
                                int _imin = Convert.ToInt32(_min);
                                int _imax = Convert.ToInt32(_max);

                                Ferda.Guha.Attribute.DynamicAlgorithm.ValueFrequencyPair<int>[] enumeration1 =
    new Ferda.Guha.Attribute.DynamicAlgorithm.ValueFrequencyPair<int>[dt.Rows.Count];
                                foreach (System.Data.DataRow row in dt.Rows)
                                {
                                    object v = row[0];
                                    if (v == null || v is DBNull)
                                        continue;
                                    Ferda.Guha.Attribute.DynamicAlgorithm.ValueFrequencyPair<int> tmpItem1 =
                                        new Ferda.Guha.Attribute.DynamicAlgorithm.ValueFrequencyPair<int>(
                                        Convert.ToInt32(row[0].ToString()),
                                        Convert.ToInt32(row[1]));
                                    enumeration1[i] = tmpItem1;
                                    i++;
                                }
                                i = 0;
                                _divisionPoints =
                                    Retyper<int>.Retype(
                                Ferda.Guha.Attribute.DynamicAlgorithm.EquifrequencyIntervals.GenerateIntervals
                            ((int)CountOfCategories, enumeration1)
                                );

                                __min = _imin;
                                __max = _imax;
                               // _divisionPoints = TypeToInt(_divisionPoints);
                                break;

                            case DbDataTypeEnum.LongIntegerType:
                            case DbDataTypeEnum.UnsignedLongIntegerType:
                                long _lmin = Convert.ToInt64(_min);
                                long _lmax = Convert.ToInt64(_max);

                                Ferda.Guha.Attribute.DynamicAlgorithm.ValueFrequencyPair<long>[] enumeration3 =
    new Ferda.Guha.Attribute.DynamicAlgorithm.ValueFrequencyPair<long>[dt.Rows.Count];
                                foreach (System.Data.DataRow row in dt.Rows)
                                {
                                    object v = row[0];
                                    if (v == null || v is DBNull)
                                        continue;
                                    Ferda.Guha.Attribute.DynamicAlgorithm.ValueFrequencyPair<long> tmpItem3 =
                                        new Ferda.Guha.Attribute.DynamicAlgorithm.ValueFrequencyPair<long>(
                                        Convert.ToInt64(row[0].ToString()),
                                        Convert.ToInt32(row[1]));
                                    enumeration3[i] = tmpItem3;
                                    i++;
                                }
                                i = 0;
                                _divisionPoints =
                                    Retyper<long>.Retype(
                                Ferda.Guha.Attribute.DynamicAlgorithm.EquifrequencyIntervals.GenerateIntervals
                            ((int)CountOfCategories, enumeration3)
                                );

                                __min = _lmin;
                                __max = _lmax;
                                // _divisionPoints = TypeToInt(_divisionPoints);
                                break;

                            case DbDataTypeEnum.DateTimeType:
                                DateTime _dtmin = Convert.ToDateTime(_min);
                                DateTime _dtmax = Convert.ToDateTime(_max);

                                Ferda.Guha.Attribute.DynamicAlgorithm.ValueFrequencyPair<DateTime>[] enumeration2 =
    new Ferda.Guha.Attribute.DynamicAlgorithm.ValueFrequencyPair<DateTime>[dt.Rows.Count];
                                foreach (System.Data.DataRow row in dt.Rows)
                                {
                                    object v = row[0];
                                    if (v == null || v is DBNull)
                                        continue;
                                    Ferda.Guha.Attribute.DynamicAlgorithm.ValueFrequencyPair<DateTime> tmpItem2 =
                                        new Ferda.Guha.Attribute.DynamicAlgorithm.ValueFrequencyPair<DateTime>(
                                        Convert.ToDateTime(row[0].ToString()),
                                        Convert.ToInt32(row[1]));
                                    enumeration2[i] = tmpItem2;
                                    i++;
                                }
                                i = 0;
                                _divisionPoints =
                                    Retyper<DateTime>.Retype(
                                Ferda.Guha.Attribute.DynamicAlgorithm.EquifrequencyIntervals.GenerateIntervals
                            ((int)CountOfCategories, enumeration2)
                                );

                                __min = _dtmin;
                                __max = _dtmax;
                                break;

                            default:
                                throw new ArgumentException("Type not supported");

                        }

                        result.CreateIntervals(BoundaryEnum.Closed, __min, _divisionPoints,
                                    ClosedFrom, __max, BoundaryEnum.Closed, false);


                        //  _nullCategoryName = result.NullContainingCategory;

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

        public override int[] GetCountVector(string masterIdColumn, string masterDatatableName, Current current__)
        {
            GenericColumn _column = GetGenericColumn(true);
            DataTable _table = _column.GetCountVector(masterIdColumn, masterDatatableName);
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

        #region IFunctions Members

        public void setBoxModuleInfo(BoxModuleI boxModule, IBoxInfo boxInfo)
        {
            _boxModule = boxModule;
        }

        #endregion

    }
}