using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using Ferda.Guha.MiningProcessor;

namespace Ferda.Guha.Attribute
{
    /// <summary>
    /// Provides some essential method like disjunctivity checking
    /// for the attribute.
    /// </summary>
    /// <typeparam name="T">See typeparam in <see cref="T:Ferda.Guha.Attribute.Attribute{T}"/></typeparam>
    public class Axis<T>
        where T : IComparable
    {
        //UNDONE null containing category even in Attribute.cs!!! -> Axis.NotValid(...)

        #region Fields

        private Attribute<T> _attribute;

        // Key: the property value
        // Value: the name of the category
        private SortedDictionary<T, string> _enumValues;

        // Key: the interval
        // Value: the name of the category
        private SortedDictionary<Interval<T>, string> _intervals;

        bool _enumsAreValid = false;
        bool _intervalsAreValid = false;
        bool _disabled = false;

        #endregion

        /// <summary>
        /// Gets or sets a value indicating whether this instace is disabled.
        /// </summary>
        /// <value><c>true</c> if disabled; otherwise, <c>false</c>.</value>
        /// <remarks>
        /// If Axis is Disabled (for performance purposes 
        /// ... often when dynamic attributes are builded)
        /// following procedure should be evaluated when 
        /// the "unchecked" operations finnished.<br />
        /// 1. Axis.Disabled = false; <br />
        /// 3. Axis.CheckDisjunctivity()<br />
        /// Step [3] will failed if categories are not disjunctive.
        /// </remarks>
        public bool Disabled
        {
            get { return _disabled; }
            set
            {
                if (_disabled || value)
                {
                    _enumsAreValid = false;
                    _intervalsAreValid = false;
                }
                _disabled = value;
            }
        }

        #region Constructors

        private Axis()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Ferda.Guha.Attribute.Axis{T}"/> class.
        /// </summary>
        /// <param name="attribute">The to work over attribute.</param>
        internal Axis(Attribute<T> attribute)
        {
            _attribute = attribute;
            //attribute implements comparer of T and Interval<T>
            _enumValues = new SortedDictionary<T, string>(_attribute);
            _intervals = new SortedDictionary<Interval<T>, string>(_attribute);
        }

        #endregion

        /// <summary>
        /// Checks the disjunctivity (if not disabled) of categories in attribute.
        /// </summary>
        public void CheckDisjunctivity()
        {
            if (_disabled)
                return;
            Build(); // falls if enumerations are not disjunctive or requestedNumberOfIntervals are not disjunctive

            if (_intervals.Count == 0 && _enumValues.Count == 0)
                return;

            SortedDictionary<T, string>.KeyCollection.Enumerator enumValuesEnumerator = _enumValues.Keys.GetEnumerator();
            T currentEnum;
            if (enumValuesEnumerator.MoveNext())
                currentEnum = enumValuesEnumerator.Current;
            else
                return;

            int compareResult;
            foreach (KeyValuePair<Interval<T>, string> interval in _intervals)
            {
            CompareIntervalWithNextEnumItem:
                compareResult = _attribute.Compare(interval.Key, currentEnum);
                if (compareResult < 0)
                    continue;
                if (compareResult == 0)
                    throw Exceptions.AttributeCategoriesDisjunctivityError(null, null);
                if (compareResult > 0)
                {
                    if (enumValuesEnumerator.MoveNext())
                    {
                        currentEnum = enumValuesEnumerator.Current;
                        goto CompareIntervalWithNextEnumItem;
                    }
                    else
                        return;
                }
            }
        }

        /// <summary>
        /// Unvalidate axis. It is necessary to call this method when 
        /// enumerations or intervals in the joined attribute changed.
        /// </summary>
        /// <param name="enumsAreNotValid">if set to <c>true</c> enums are not valid.</param>
        /// <param name="intervalsAreNotValid">if set to <c>true</c> intervals are not valid.</param>
        public void NotValid(bool enumsAreNotValid, bool intervalsAreNotValid)
        {
            if (enumsAreNotValid)
                _enumsAreValid = false;
            if (intervalsAreNotValid)
                _intervalsAreValid = false;
        }

        /// <summary>
        /// Rebuilds the axis (if not disabled and was unvalidated) 
        /// and make the instance valid.
        /// </summary>
        /// <exception cref="T:System.InvalidOperationException">Thrown when axis is disabled</exception>
        public void Build()
        {
            if (_disabled)
                throw new InvalidOperationException("Axis is disabled.");
            if (_enumsAreValid && _intervalsAreValid)
                return;
            if (!_enumsAreValid)
                _enumValues.Clear();
            if (!_intervalsAreValid)
                _intervals.Clear();
            try
            {
                string categoryName;
                // process all categories
                foreach (KeyValuePair<string, Category<T>> category in _attribute)
                {
                    categoryName = category.Key;

                    // process enumerations
                    if (!_enumsAreValid)
                    {
                        foreach (T item in category.Value.Enumeration)
                        {
                            _enumValues.Add(item, categoryName);
                        }
                    }

                    // process requestedNumberOfIntervals 
                    if (!_intervalsAreValid)
                    {
                        foreach (Interval<T> interval in category.Value.Intervals)
                        {
                            _intervals.Add(interval, categoryName);
                        }
                    }
                }
            }
            catch (ArgumentException ex)
            // thrown iff *.add(...) method failed i.e. if categories are not disjunctive
            {
                throw Exceptions.AttributeCategoriesDisjunctivityError(ex, null);
            }
            _enumsAreValid = true;
            _intervalsAreValid = true;
        }

        #region Categories in collision

        /// <summary>
        /// Categorieses the in collision. Returns names of categories 
        /// which contains the specified <c>item</c>. If disabled emty
        /// array is returned.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>Names of categories in collision.</returns>
        public string[] CategoriesInCollision(T item)
        {
            if (_disabled)
                return new string[0];

            Build();

            if (_intervals.Count == 0 && _enumValues.Count == 0)
                return new string[0];

            // Thesis:
            // the itemToExclude T can be contained only once in enumerations xor requestedNumberOfIntervals

            string categoryName;

            if (_enumValues.TryGetValue(item, out categoryName))
                return new string[] { categoryName };

            if (
                _intervals.TryGetValue(new Interval<T>(item, BoundaryEnum.Closed, item, BoundaryEnum.Closed, _attribute),
                                       out categoryName))
                return new string[] { categoryName };

            return new string[0];
        }

        /// <summary>
        /// Returns names of categories 
        /// which contains the specified <c>interval</c>. If disabled empty
        /// array is returned.
        /// </summary>
        /// <param name="interval">The interval.</param>
        /// <returns>Names of categories in collision.</returns>
        public string[] CategoriesInCollision(Interval<T> interval)
        {
            if (_disabled)
                return new string[0];

            Build();

            if (_intervals.Count == 0 && _enumValues.Count == 0)
                return new string[0];

            List<string> result = new List<string>();

            int comparationResult;
            // process enumerations
            foreach (KeyValuePair<T, string> item in _enumValues)
            {
                comparationResult = _attribute.Compare(interval, item.Key);
                if (comparationResult > 0)
                    continue;
                else if (comparationResult == 0)
                    result.Add(item.Value);
                else
                    break;
            }
            // process intervals
            foreach (KeyValuePair<Interval<T>, string> item in _intervals)
            {
                comparationResult = _attribute.Compare(interval, item.Key);
                if (comparationResult > 0)
                    continue;
                else if (comparationResult == 0)
                    result.Add(item.Value);
                else
                    break;
            }
            return result.ToArray();
        }

        /// <summary>
        /// Categorieses the in collision. Returns names of categories 
        /// which contains the specified <c>interval</c>. If disabled emty
        /// array is returned.
        /// </summary>
        /// <param name="category">The category.</param>
        /// <returns>Names of categories in collision.</returns>
        public string[] CategoriesInCollision(Category<T> category)
        {
            if (_disabled)
                return new string[0];

            Build();

            if (_intervals.Count == 0 && _enumValues.Count == 0)
                return new string[0];

            List<string> result = new List<string>();

            foreach (Interval<T> interval in category.Intervals)
            {
                result.AddRange(CategoriesInCollision(interval));
            }
            foreach (T item in category.Enumeration)
            {
                result.AddRange(CategoriesInCollision(item));
            }

            if (result.Count <= 1)
                return result.ToArray();

            // remove duplicits
            result.Sort();
            List<string> finalResult = new List<string>();
            string lastValue = null;
            foreach (string item in result)
            {
                if (lastValue != item)
                    finalResult.Add(item);
                lastValue = item;
            }
            return finalResult.ToArray();
        }

        #endregion

        /// <summary>
        /// Gets frequencies of categories in the specified data.
        /// </summary>
        /// <param name="dataTable">
        /// The data table (in column [0] are distinct values, 
        /// in column [1] are its frequencies).
        /// </param>
        /// <returns>
        /// Dictionary where key is name of category in the attribute
        /// and value is the frequency of the category in specified <c>dataTable</c>.</returns>
        public Dictionary<string, int> GetFrequencies(DataTable dataTable)
        {
            Disabled = false;
            Build();

            if (dataTable == null)
                return null;

            Dictionary<string, int> result = new Dictionary<string, int>();
            foreach (KeyValuePair<string, Category<T>> var in _attribute)
                result.Add(var.Key, 0);

            T item;
            string categoryName;
            foreach (DataRow row in dataTable.Rows)
            {
                item = (T)row[0];
                if (_enumValues.TryGetValue(item, out categoryName))
                {
                    Debug.Assert(result.ContainsKey(categoryName), "This should never happend");
                    result[categoryName] += (int)row[1];
                }
                else if (
                    _intervals.TryGetValue(new Interval<T>(item, BoundaryEnum.Closed, item, BoundaryEnum.Closed, _attribute),
                                           out categoryName))
                {
                    Debug.Assert(result.ContainsKey(categoryName), "This should never happend");
                    result[categoryName] += (int)row[1];
                }
                //else not covered               
            }
            return result;
        }

        /// <summary>
        /// Gets array of uncovered values i.e. values that are not covered
        /// by any category in the attribute.
        /// </summary>
        /// <param name="dataTable">The data table with covered values in column [0].</param>
        public List<T> GetUncoveredValues(DataTable dataTable)
        {
            //UNDONE make a test

            Disabled = false;
            Build();

            if (dataTable == null)
                return null;

            List<T> result = new List<T>();

            T item;
            foreach (DataRow row in dataTable.Rows)
            {
                item = (T)row[0];
                if (
                    !_enumValues.ContainsKey(item)
                    &&
                    !_intervals.ContainsKey(new Interval<T>(item, BoundaryEnum.Closed, item, BoundaryEnum.Closed, _attribute))
                    )
                {
                    result.Add(item);
                }
            }
            return result;
        }

        
        private const int _blockSize = 64;
        private const long _one = 1;
        private void setTrueBit(int index, long[] array)
        {
            array[index / _blockSize] |= _one << (index % _blockSize);
            // set false bit : _array[index / _blockSize] &= ~(_one << (index % _blockSize));
        }
        
        /// <summary>
        /// Gets bit strings. Each bit string corresponds to one category and 
        /// contains positive bit on <c>i</c> possition (bit index) if specified dataTable 
        /// has value covered by the category on row <c>i</c>.
        /// </summary>
        /// <param name="dataTable">The table with one column sorted by primary key.</param>
        /// <returns></returns>
        public Dictionary<string, BitString> GetBitStrings(DataTable dataTable)
        {
            Disabled = false;
            Build();

            if (dataTable == null)
                return null;

            // INITIALIZE
            int length = dataTable.Rows.Count;

            if (length == 0)
                return null;

            int arraySize = (length + _blockSize - 1) / _blockSize; // rounding up...
            
            Dictionary<string, BitString> result = new Dictionary<string, BitString>();
            
            foreach (KeyValuePair<string, Category<T>> pair in _attribute)
            {
                long[] tmpBitArray = new long[arraySize];
                tmpBitArray.Initialize();
                result.Add(pair.Key, new BitString(tmpBitArray, length));
            }

            // PROCESS DATA
            
            T item;
            string categoryName;
            int i = -1;
            foreach (DataRow row in dataTable.Rows)
            {
                i++;
                
                item = (T)row[0];
                if (_enumValues.TryGetValue(item, out categoryName))
                {
                    setTrueBit(i, result[categoryName].value);
                }
                else if (
                    _intervals.TryGetValue(new Interval<T>(item, BoundaryEnum.Closed, item, BoundaryEnum.Closed, _attribute),
                                           out categoryName))
                {
                    setTrueBit(i, result[categoryName].value);
                }
                //else not covered
            }
            return result;
            //UNDONE public  IBitString GenerateBitString(string categoryName, DataTable dataTable)
            /*
             * Takhle to je v Rel-Mineru
             * 
             * GetBitString(bitStringDefinition, dataTable)
             * {
             *      for (i=0; i< dataTable.Rows.Count; i++)
             *      {
             *          BS[i] = CheckBitStringCondition(
             *                      bitStringDefinition, 
             *                      dataTable.Rows[i].Column[0]
             *                  );
             *      }
             * }
             * */
        }
    }
}