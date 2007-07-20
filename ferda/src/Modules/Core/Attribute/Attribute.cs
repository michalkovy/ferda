// Attribute.cs - functionality of the attributes in Ferda data preparation
//
// Author: Tomáš Kuchaø <tomas.kuchar@gmail.com>
//
// Copyright (c) 2006 Tomáš Kuchaø
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
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Ferda.Guha.Data;
using Ferda.Guha.MiningProcessor;

namespace Ferda.Guha.Attribute
{
    /// <summary>
    /// Attribute holds collection of categories i.e. enumerations of values and 
    /// (iff source domain datatype is ordinal or cardinal) intervals.
    /// All categories has to be disjunctive. All categories doesn`t have
    /// to cover neither whole domain of the source nor all possible values 
    /// of the source.
    /// </summary>
    /// <typeparam name="T">Domain type</typeparam>
    public class Attribute<T> :
        IComparer<T>,
        IComparer<Interval<T>>,
        IComparer<Category<T>>,
        IEnumerable<KeyValuePair<string, Category<T>>>
        // partially IDictionary<string, Category<T>>
        where T : IComparable
    {
        #region Fields

        // Key: name of the category
        // Value: the category
        private Dictionary<string, Category<T>> _categories = new Dictionary<string, Category<T>>();

        Axis<T> _axis;

        private string _nullContainingCategory = null;

        private readonly bool _intervalsAllowed = false;

        private readonly DbSimpleDataTypeEnum _dbDataType;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets name of the null containing category.
        /// </summary>
        /// <value>The name of the null containing category.</value>
        /// <remarks>
        /// If equals to Null than no category contains null.
        /// </remarks>
        public string NullContainingCategory
        {
            get
            {
                if (_nullContainingCategory != null && !_categories.ContainsKey(_nullContainingCategory))
                    throw new Exception("This should never happend.");
                return _nullContainingCategory;
            }
            set
            {
                if (value != null && !_categories.ContainsKey(value))
                    throw new ArgumentOutOfRangeException("NullContainingCategory doesn`t exist in categories");
                _nullContainingCategory = value;
            }
        }

        /// <summary>
        /// Gets a value indicating whether is set the null containing category.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if is set the null containing category; otherwise, <c>false</c>.
        /// </value>
        public bool IsSetNullContainingCategory
        {
            get { return NullContainingCategory != null; }
        }

        /// <summary>
        /// Gets the axis.
        /// </summary>
        /// <value>The axis.</value>
        public Axis<T> Axis
        {
            get { return _axis; }
        }

        /// <summary>
        /// Gets a value indicating whether intervals are allowed.
        /// </summary>
        /// <value><c>true</c> if intervals are allowed; otherwise, <c>false</c>.</value>
        public bool IntervalsAllowed
        {
            get { return _intervalsAllowed; }
        }

        /// <summary>
        /// Gets the DB data type of the T type.
        /// </summary>
        /// <value>The DB data type of the T type.</value>
        public DbSimpleDataTypeEnum DbDataType
        {
            get { return _dbDataType; }
        }

        #endregion

        #region Categories Names

        /// <summary>
        /// Gets the name of the category.
        /// </summary>
        /// <param name="category">The category.</param>
        /// <returns></returns>
        public string GetCategoryName(Category<T> category)
        {
            if (_categories.ContainsValue(category))
            {
                foreach (KeyValuePair<string, Category<T>> cat in _categories)
                {
                    if (ReferenceEquals(cat.Value, category))
                        return cat.Key;
                }
            }
            throw new ArgumentOutOfRangeException("Specified category was not found.");
        }

        /// <summary>
        /// Renames the category. (Iff necessary NullContainingCategory is updated)
        /// </summary>
        /// <param name="oldName">The old name.</param>
        /// <param name="newName">The new name.</param>
        public void RenameCategory(string oldName, string newName)
        {
            if (String.IsNullOrEmpty(oldName))
                throw new ArgumentException("Specified old name is null or empty string.");
            if (String.IsNullOrEmpty(newName))
                throw new ArgumentException("Specified new name is null or empty string.");
            if (!_categories.ContainsKey(oldName))
                throw new ArgumentException("There is no category named " + oldName + ".", "oldName");
            if (_categories.ContainsKey(newName))
                throw new ArgumentException("Specified new name is already used.");
            _categories.Add(newName, _categories[oldName]);
            if (NullContainingCategory == oldName)
                NullContainingCategory = newName;
            _categories.Remove(oldName);
            Axis.NotValid(true, true);
        }

        #endregion

        #region Constructors

        private Attribute(DbSimpleDataTypeEnum dbDataType)
        {
            _axis = new Axis<T>(this);
            _dbDataType = dbDataType;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Ferda.Guha.Attribute.Attribute`1"/> class.
        /// </summary>
        /// <param name="dbDataType">Type of the db data.</param>
        /// <param name="intervalsAllowed">if set to <c>true</c> creation of intervals allowed (only for ordinal data).</param>
        public Attribute(DbSimpleDataTypeEnum dbDataType, bool intervalsAllowed)
            : this(dbDataType)
        {
            _intervalsAllowed = intervalsAllowed;
        }

        private void loadAttribute(AttributeSerializable<T> attribute, bool lazyDisjunctivityChecking)
        {
            if (lazyDisjunctivityChecking)
            {
                // disable Axis
                Axis.Disabled = true;
            }

            foreach (CategorySerializable<T> category in attribute.Categories)
            {
                _categories.Add(category.Name, new Category<T>(category, this));
            }
            NullContainingCategory = attribute.NullContainingCategoryName;

            if (lazyDisjunctivityChecking)
            {
                // enable Axis
                Axis.Disabled = false;
                Reduce();
                Axis.CheckDisjunctivity();
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Ferda.Guha.Attribute.Attribute`1"/> class.
        /// </summary>
        /// <param name="dbDataType">Type of the db data.</param>
        /// <param name="serializedAttribute">The serialized attribute.</param>
        /// <param name="lazyDisjunctivityChecking">if set to <c>true</c> [lazy disjunctivity checking].</param>
        public Attribute(DbSimpleDataTypeEnum dbDataType, AttributeSerializable<T> serializedAttribute, bool lazyDisjunctivityChecking)
            : this(dbDataType, serializedAttribute.IntervalsAllowed)
        {
            loadAttribute(serializedAttribute, lazyDisjunctivityChecking);
        }

        /// <summary>
        /// Exports this instance to serializable form.
        /// </summary>
        /// <returns></returns>
        public AttributeSerializable<T> Export()
        {
            lock (this)
            {
                AttributeSerializable<T> result = new AttributeSerializable<T>();
                result.NullContainingCategoryName = NullContainingCategory;
                result.IntervalsAllowed = IntervalsAllowed;
                result.DbDataType = DbDataType;

                List<CategorySerializable<T>> categories = new List<CategorySerializable<T>>();
                foreach (KeyValuePair<string, Category<T>> pair in _categories)
                {
                    categories.Add(pair.Value.Export(pair.Key));
                }

                result.Categories = categories.ToArray();
                return result;
            }
        }

        #endregion

        #region Data Evaluation

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
            return Axis.GetFrequencies(dataTable);
        }

        /// <summary>
        /// Gets list of uncovered values i.e. values that are not covered
        /// by any category in the attribute.
        /// </summary>
        /// <param name="dataTable">The data table with covered values in column [0].</param>
        public List<T> GetUncoveredValues(DataTable dataTable)
        {
            return Axis.GetUncoveredValues(dataTable);
        }


        /// <summary>
        /// Gets bit strings. Each bit string corresponds to one category and 
        /// contains positive bit on <c>i</c> possition (bit index) if specified dataTable 
        /// has value covered by the category on row <c>i</c>.
        /// </summary>
        /// <param name="dataTable">The table with one column sorted by primary key.</param>
        /// <returns></returns>
        public Dictionary<string, BitStringIce> GetBitStrings(DataTable dataTable)
        {
            return Axis.GetBitStrings(dataTable);
        }

        #endregion

        #region Collisions (Reduce, Join, Exclude)

        /// <summary>
        /// Reduces the attribute ().
        /// </summary>
        /// <remarks>
        /// Categories can contains intervals and enumeration values that are 
        /// not disjunctive (if lazy reduction is on OR when new interval or enumeration
        /// value was just added/updated)
        /// </remarks>
        /// <example>
        /// If there is category like:
        /// <code>{0};(0;1&gt;;$lt0.5;16);{7;8;9}</code> than reduction of this category is <code>&lt;0;16)</code>
        /// </example>
        public void Reduce()
        {
            foreach (KeyValuePair<string, Category<T>> category in _categories)
            {
                category.Value.Reduce();
            }
        }

        /// <summary>
        /// Joins the categories of specified names (<c>categories</c>).
        /// New category will be created, old categories will be removed.
        /// </summary>
        /// <param name="categories">The categories.</param>
        /// <param name="newCategoryNameCreationType">New type of the category name creation.</param>
        /// <param name="ownNewCategoryName">Name of the own new category.</param>
        /// <param name="newCategoryName">New name of the category.</param>
        public void JoinCategories(string[] categories, NewCategoryName newCategoryNameCreationType,
                                   string ownNewCategoryName, out string newCategoryName)
        {
            // Axis.NotValid(true, true);
            // ... not needed this is called when needed in add/update/remove methods above

            if (categories.Length == 0)
                throw new ArgumentException("Empty list of categories to join.");

            List<T> newEnumeration = new List<T>();
            List<Interval<T>> newIntervals = new List<Interval<T>>();
            Category<T> currentCategory;
            foreach (string categoryName in categories)
            {
                if (_categories.TryGetValue(categoryName, out currentCategory))
                {
                    newEnumeration.AddRange(currentCategory.Enumeration);
                    newIntervals.AddRange(currentCategory.Intervals);
                }
                else
                    throw new ArgumentException("There is no category named \"" + categoryName + "\" in categories.");
            }

            // all categories were found ... the operation will be executed.
            foreach (string categoryName in categories)
            {
                // old categories will be removed
                Remove(categoryName);
            }

            // new (joined) category will be added
            Category<T> newCategory = new Category<T>(newIntervals, newEnumeration, this);
            newCategory.Reduce();

            if (newCategoryNameCreationType == NewCategoryName.Default)
                newCategoryName = newCategory.DefaultLabel;
            else if (newCategoryNameCreationType == NewCategoryName.JoinPreviousNames)
                newCategoryName = Ferda.Modules.Helpers.Common.Print.SequenceToString(categories, Common.CategoriesNamesSeparator);
            else // if (newCategoryNameSpecification == NewCategoryName.Own)
                newCategoryName = ownNewCategoryName;

            _categories.Add(newCategoryName, newCategory);
            //needless
            //if (newEnumeration.Count > 0)
            //    Axis.NotValid(true, false);
            //if (newIntervals.Count > 0)
            //    Axis.NotValid(false, true);
        }

        /// <summary>
        /// Excludes the specified item from specified <c>fromCategories</c>.
        /// This method is used mainly by <b>force</b> mode of add or update 
        /// [item to enumeration of category] method (typically the 
        /// <c>itemToExclude</c> is added or updated).
        /// </summary>
        /// <param name="itemToExclude">The itemToExclude.</param>
        /// <param name="fromCategories">From categories.</param>
        public void Exclude(T itemToExclude, string[] fromCategories)
        {
            // Axis.NotValid(true, true); 
            // ... not needed this is called when needed in add/update/remove methods above

            if (fromCategories == null || fromCategories.Length == 0)
                return;

            Category<T> currentCategory;
            // often (maybe always) fromCategories.Length <= 1
            foreach (string categoryName in fromCategories)
            {
                if (_categories.TryGetValue(categoryName, out currentCategory))
                {
                    currentCategory.Enumeration.Remove(itemToExclude);
                RestartIntervalEnumerator:
                    foreach (Interval<T> interval in currentCategory.Intervals)
                    {
                        if (Compare(interval, itemToExclude) == 0)
                        {
                            // the itemToExclude is on some boundary
                            //  try left boundary
                            if (interval.LeftBoundary == BoundaryEnum.Closed
                                && Compare(interval.RightValue, itemToExclude) == 0)
                                currentCategory.Intervals.Update(currentCategory.Intervals.IndexOf(interval),
                                                                 interval.LeftValue, BoundaryEnum.Open,
                                                                 interval.RightValue, interval.RightBoundary, true);
                            //  try right boundary
                            else if (interval.RightBoundary == BoundaryEnum.Closed
                                     && Compare(interval.RightValue, itemToExclude) == 0)
                                currentCategory.Intervals.Update(currentCategory.Intervals.IndexOf(interval),
                                                                 interval.LeftValue, interval.LeftBoundary,
                                                                 interval.RightValue, BoundaryEnum.Open, true);
                            // itemToExclude is inside the interval -> split
                            else
                            {
                                currentCategory.Intervals.Update(currentCategory.Intervals.IndexOf(interval),
                                                                 interval.LeftValue, interval.LeftBoundary,
                                                                 itemToExclude, BoundaryEnum.Open, true);
                                currentCategory.Intervals.Add(itemToExclude, BoundaryEnum.Open, interval.RightValue,
                                                              interval.RightBoundary, true);
                            }
                            goto RestartIntervalEnumerator;
                        }
                    }
                }
                else
                    throw new ArgumentException("There is no category named \"" + categoryName + "\" in categories.");
            }
        }

        /// <summary>
        /// Excludes the specified interval from specified <c>fromCategories</c>.
        /// This method is used mainly by <b>force</b> mode of add or update 
        /// [interval to intervals of category] method (typically the 
        /// <c>intervalToExclude</c> is added or updated).
        /// </summary>
        /// <param name="intervalToExclude">The interval to exclude.</param>
        /// <param name="fromCategories">From categories.</param>
        public void Exclude(Interval<T> intervalToExclude, string[] fromCategories)
        {
            // Axis.NotValid(true, true); 
            // ... not needed this is called when needed in add/update/remove methods above

            if (fromCategories == null || fromCategories.Length == 0)
                return;

            Category<T> currentCategory;
            // often (maybe always) fromCategories.Length <= 1
            foreach (string categoryName in fromCategories)
            {
                if (_categories.TryGetValue(categoryName, out currentCategory))
                {
                RestartEnumerationEnumerator:
                    foreach (T enumItem in currentCategory.Enumeration)
                    {
                        if (Compare(intervalToExclude, enumItem) == 0)
                        {
                            currentCategory.Enumeration.Remove(enumItem);
                            goto RestartEnumerationEnumerator;
                        }
                    }
                RestartIntervalEnumerator:
                    foreach (Interval<T> intervalItem in currentCategory.Intervals)
                    {
                        if (Compare(intervalToExclude, intervalItem) == 0)
                        {
                            // interval is      ?a, b?
                            // intervalItem is  ?c, d?

                            int compareA2C =
                                compareIntervalBounds(intervalToExclude.LeftValue, intervalToExclude.LeftBoundary,
                                                      intervalItem.LeftValue, intervalItem.LeftBoundary, Side.Left);
                            int compareB2D =
                                compareIntervalBounds(intervalToExclude.RightValue, intervalToExclude.RightBoundary,
                                                      intervalItem.RightValue, intervalItem.RightBoundary, Side.Right);
                            // compareX2Y 
                            //  >1 when X>Y
                            //  <1 when X<Y
                            //  =0 otherwise

                            if (compareA2C <= 0 && compareB2D >= 0)
                            // intervalItem is whole covered by interval
                            // -> remove intervalItem
                            // a <= c << d <= b
                            {
                                currentCategory.Intervals.Remove(intervalItem);
                            }
                            else if (compareA2C > 0 && compareB2D >= 0)
                            // intervalItem overreach interval from left side
                            // -> right side of intervalItem will be updated (moved more to left)
                            // c << a <= d <= b
                            {
                                currentCategory.Intervals.Update(currentCategory.Intervals.IndexOf(intervalItem),
                                                                 intervalItem.LeftValue,
                                                                 intervalItem.LeftBoundary,
                                                                 intervalToExclude.LeftValue,
                                                                 (intervalToExclude.LeftBoundary == BoundaryEnum.Closed)
                                                                     ? BoundaryEnum.Open
                                                                     : BoundaryEnum.Closed,
                                                                 true);
                            }
                            else if (compareA2C <= 0 && compareB2D < 0)
                            // intervalItem overreach interval from rigth side
                            // -> left side of intervalItem will be updated (moved more to right)
                            // a <= c <= b << d
                            {
                                currentCategory.Intervals.Update(currentCategory.Intervals.IndexOf(intervalItem),
                                                                 intervalToExclude.RightValue,
                                                                 (intervalToExclude.RightBoundary == BoundaryEnum.Closed)
                                                                     ? BoundaryEnum.Open
                                                                     : BoundaryEnum.Closed,
                                                                 intervalItem.RightValue,
                                                                 intervalItem.RightBoundary,
                                                                 true);
                            }
                            else
                            // intervalItem overreach interval to both sides
                            // -> left part will be shortened from right
                            // -> new interval will be added ... the right part
                            // c << a << b << d
                            {
                                T rightValueCache = intervalItem.RightValue;
                                BoundaryEnum rightBoundaryCache = intervalItem.RightBoundary;
                                currentCategory.Intervals.Update(currentCategory.Intervals.IndexOf(intervalItem),
                                                                 intervalItem.LeftValue,
                                                                 intervalItem.LeftBoundary,
                                                                 intervalToExclude.LeftValue,
                                                                 (intervalToExclude.LeftBoundary == BoundaryEnum.Closed)
                                                                     ? BoundaryEnum.Open
                                                                     : BoundaryEnum.Closed,
                                                                 true);
                                currentCategory.Intervals.Add(
                                    intervalToExclude.RightValue,
                                    (intervalToExclude.RightBoundary == BoundaryEnum.Closed)
                                        ? BoundaryEnum.Open
                                        : BoundaryEnum.Closed,
                                    rightValueCache,
                                    rightBoundaryCache,
                                    true);
                            }
                            goto RestartIntervalEnumerator;
                        }
                    }
                }
                else
                    throw new ArgumentException(string.Format("There is no category named \"{0}\" in categories.", categoryName));
            }
        }

        #endregion

        #region Create [dynamic] attribute

        /// <summary>
        /// Creates the intervals.
        /// </summary>
        /// <param name="startBoundary">The start boundary.</param>
        /// <param name="startValue">The start value.</param>
        /// <param name="separators">The separators.</param>
        /// <param name="intervalsClosedFrom">The intervals created by separators will be closed from that side.</param>
        /// <param name="endValue">The end value.</param>
        /// <param name="endBoundary">The end boundary.</param>
        /// <param name="lazyDisjunctivityChecking">if set to <c>true</c> lazy disjunctivity checking is on.</param>
        public void CreateIntervals(BoundaryEnum startBoundary, T startValue, object[] separators,
                                    Side intervalsClosedFrom, T endValue, BoundaryEnum endBoundary,
                                    bool lazyDisjunctivityChecking)
        {
            if (_categories.Count > 0)
                throw new InvalidOperationException("The attribute already contains some categories");

            bool axisWasDisabled = Axis.Disabled;
            if (lazyDisjunctivityChecking)
            {
                // disable Axis
                Axis.Disabled = true;
            }

            T innerLeftValue = startValue;
            BoundaryEnum innerLeftBoundary = startBoundary;
            BoundaryEnum innerRightBoundary = (intervalsClosedFrom == Side.Left)
                                                  ? BoundaryEnum.Open
                                                  : BoundaryEnum.Closed;
            for (int i = 0; i < separators.Length + 1; i++)
            {
                if (i == 1) // after first iteration inner left boundary must be changed
                    innerLeftBoundary = (intervalsClosedFrom == Side.Left) ? BoundaryEnum.Closed : BoundaryEnum.Open;

                Interval<T> newInterval;

                if (i == separators.Length) // last interval is closed by end value and by end boundary
                {
                    newInterval = new Interval<T>(innerLeftValue, innerLeftBoundary, endValue, endBoundary, this);
                }
                else
                {
                    Object separator = separators[i];
                    newInterval =
                        new Interval<T>(innerLeftValue, innerLeftBoundary, (T)separator, innerRightBoundary, this);
                    innerLeftValue = (T)separator;
                }

                //UNDONE this should be optimized
                string categoryName = newInterval.ToString();
                Add(categoryName);
                this[categoryName].Intervals.Add(newInterval.LeftValue, newInterval.LeftBoundary, newInterval.RightValue,
                                                 newInterval.RightBoundary, true);
            }

            if (lazyDisjunctivityChecking)
            {
                Axis.Disabled = axisWasDisabled;
                Reduce();
                Axis.CheckDisjunctivity();
            }
        }

        /// <summary>
        /// Creates the enumerations by specified <c>values</c>. Each value will represent
        /// one category i.e. category: value.ToString() =&gt; {(T)value}.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <param name="addNullCategory">if set to <c>true</c> null category (empty, null containing) will be created and added.</param>
        /// <param name="lazyDisjunctivityChecking">if set to <c>true</c> lazy disjunctivity checking is on.</param>
        public void CreateEnums(IEnumerable<object> values, bool addNullCategory, bool lazyDisjunctivityChecking)
        {
            if (_categories.Count > 0)
                throw new InvalidOperationException("The attribute already contains some categories");

            bool axisWasDisabled = Axis.Disabled;
            if (lazyDisjunctivityChecking)
            {
                // disable Axis
                Axis.Disabled = true;
            }

            if (addNullCategory)
            {
                Add(Common.NullValue);
                NullContainingCategory = Common.NullValue;
            }

            string categoryName;
            foreach (object value in values)
            {
                //if (value == null || value is DBNull)
                //    continue;
                // in values should not be true (value == null)
                // for that purposes is there parameter "addNullCategory"
                categoryName = value.ToString();
                Add(categoryName);
                this[categoryName].Enumeration.Add((T)value, false);
            }

            if (lazyDisjunctivityChecking)
            {
                // enable Axis
                Axis.Disabled = axisWasDisabled;
                Reduce();
                Axis.CheckDisjunctivity();
            }
        }

        #endregion

        /// <summary>
        /// Returns a <see cref="T:System.String"></see> that represents the current attribute.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"></see> that represents the current attribute.
        /// </returns>
        public override string ToString()
        {
            const string categoryNameValueSeparator = ": ";
            const string categoriesSeparator = ";\n";

            if (_categories.Count == 0)
                return "There are no categories in the attribute.";
            StringBuilder result = new StringBuilder();
            foreach (KeyValuePair<string, Category<T>> category in _categories)
            {
                result.Append(category.Key + categoryNameValueSeparator + category.Value.ToString() +
                              categoriesSeparator);
            }
            return result.ToString();
        }

        /// <summary>
        /// Gets the names of categories.
        /// </summary>
        /// <param name="ordered">if set to <c>true</c> names are ordered by categories ord number.</param>
        /// <returns></returns>
        public List<string> GetCategoriesIds(out bool ordered)
        {
            ordered = true;
            List<string> result = GetSort();
            if (result == null)
            {
                ordered = false;
                result = new List<string>(_categories.Keys);
            }
            return result;
        }

        /// <summary>
        /// Gets the names of categories exclude the specified <c>missing category</c>.
        /// </summary>
        /// <param name="missingCategoryName">Name of the missing category.</param>
        /// <returns></returns>
        public List<string> GetNotMissingsCategorieIds(string missingCategoryName)
        {
            bool dummy;
            List<string> result = GetCategoriesIds(out dummy);
            if (missingCategoryName == null)
                return result;
            if (result.Contains(missingCategoryName))
                result.Remove(missingCategoryName);
            return result;
        }

        /// <summary>
        /// Gets the single values of specified <c>categories</c> or null.
        /// </summary>
        /// <param name="categories">The categories.</param>
        /// <returns></returns>
        public List<T> GetSingleValues(List<string> categories)
        {
            Category<T> category;
            List<T> result = new List<T>();
            foreach (string categoryName in categories)
            {
                if (!_categories.TryGetValue(categoryName, out category))
                    throw new ArgumentException();
                else if (
                    (category.Intervals.Count > 0)
                    ||
                    (category.Enumeration.Count > 1)
                    ||
                    (category.Enumeration.Count == 0)
                    )
                {
                    return null;
                }
                else if (category.Enumeration.Count == 1)
                {
                    result.Add(category.Enumeration[0]);
                }
            }
            return result;
        }

        /// <summary>
        /// Makes default sort of categories i.e. if categories can be sorted
        /// than corresponding ord numbers are saved to <c>OrdNumber</c>
        /// of categories. Otherwise exception is thrown.
        /// </summary>
        /// <exception cref="T:Ferda.Guha.Attribute.NotComparableCollisionException">
        /// Thrown if intervals and enumertions in some category can 
        /// not be sorted i.e. they are not disjunctive ... it can 
        /// appear when <c>LazyReduction</c> is <c>true</c>.<br />
        /// Or thrown if categories can not be ordered.<br />
        /// Or thrown if more than one category is empty.<br />
        /// </exception>
        public void SetDefaultOrds()
        {
            List<string> defaultSort = GetDefaultSort();
            int i = 0;
            foreach (string s in defaultSort)
            {
                i++;
                this[s].OrdNumber = i;
            }
        }

        /// <summary>
        /// Gets default sort of categories.
        /// </summary>
        /// <exception cref="T:Ferda.Guha.Attribute.NotComparableCollisionException">
        /// Thrown if intervals and enumertions in some category can 
        /// not be sorted i.e. they are not disjunctive ... it can 
        /// appear when <c>LazyReduction</c> is <c>true</c>.<br />
        /// Or thrown if categories can not be ordered.<br />
        /// Or thrown if more than one category is empty.<br />
        /// </exception>
        public List<string> GetDefaultSort()
        {
            SortedList<Category<T>, string> sortedList = new SortedList<Category<T>, string>(this);
            try
            {
                foreach (KeyValuePair<string, Category<T>> pair in _categories)
                {
                    sortedList.Add(pair.Value, pair.Key);
                    // throws argument exception iff already contains 
                    // specified key i.e. category, so Equal() method
                    // on category is defined in that way that two 
                    // categories which can not be ordered (&lt; &gt;)
                    // are equal.
                }
            }
            catch (NotComparableCollisionException)
            {
                throw;
            }
            catch (ArgumentException e)
            {
                throw new NotComparableCollisionException(e);
                // sorted array already contains specified key
                // i.e. (thanks overloading Compare() and Equal()) categories are unsortable.
            }
            List<string> result = new List<string>();
            foreach (KeyValuePair<Category<T>, string> pair in sortedList)
            {
                result.Add(pair.Value);
            }
            return result;
        }

        /// <summary>
        /// Gets the sort of categories (if OrdNumbers of categories
        /// are valueable i.e. linear ordering is possible), otherwise 
        /// returns null.
        /// </summary>
        /// <returns>Names of categories in sort of OrdNubmers or null 
        /// (if OrdNumbers are not defined in due form).</returns>
        public List<string> GetSort()
        {
            SortedList<int, string> sortedList = new SortedList<int, string>();
            try
            {
                foreach (KeyValuePair<string, Category<T>> pair in _categories)
                {
                    if (sortedList.ContainsKey(pair.Value.OrdNumber))
                        return null;
                    else 
                        sortedList.Add(pair.Value.OrdNumber, pair.Key);
                    // throws argument exception iff already contains 
                    // specified key i.e. category with same OrdNumber
                }
            }
            catch (ArgumentException)
            {
                // sorted array already contains specified key
                // i.e. sort is not valuaeble.
                return null;
            }
            List<string> result = new List<string>();
            foreach (KeyValuePair<int, string> pair in sortedList)
            {
                result.Add(pair.Value);
            }
            return result;
        }

        #region IComparer<Category<T>> Members

        /// <summary>
        /// Compares two objects and returns a value indicating whether one is less than, equal to, or greater than the other.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>
        /// Value Condition:<br />
        /// If x and y are empty returns 0;<br />
        /// If x is empty returns Int32.MinValue;<br />
        /// If y is empty returns Int32.MaxValue;<br />
        /// If x is less than y returns -1.<br />
        /// If x is greater than y returns 1.<br />
        /// Zero if not comparable.<br />
        /// </returns>
        /// <exception cref="T:Ferda.Guha.Attribute.NotComparableCollisionException">
        /// Thrown if intervals and enumertions in some category can 
        /// not be sorted i.e. they are not disjunctive ... it can 
        /// appear when <c>LazyReduction</c> is <c>true</c>.
        /// </exception>
        public int Compare(Category<T> x, Category<T> y)
        {
            lock (this)
            {
                ArrayList xSorted = x.SortEnumerationsAndIntervals();
                ArrayList ySorted = y.SortEnumerationsAndIntervals();

                if (xSorted.Count == 0 && ySorted.Count == 0)
                    return 0;
                else if (xSorted.Count == 0)
                    return Int32.MinValue;
                else if (ySorted.Count == 0)
                    return Int32.MaxValue;

                Type intervalType = typeof(Interval<T>);
                int? result = null;
                int comparationResult;
                foreach (object xItem in xSorted)
                {
                    foreach (object yItem in ySorted)
                    {
                        if (xItem.GetType() == intervalType)
                        {
                            if (yItem.GetType() == intervalType)
                                comparationResult = Compare((Interval<T>)xItem, (Interval<T>)yItem);
                            else
                                comparationResult = Compare((Interval<T>)xItem, (T)yItem);
                        }
                        else //(xItem.GetType() == typeof(T))
                        {
                            if (yItem.GetType() == intervalType)
                                comparationResult = Compare((T)xItem, (Interval<T>)yItem);
                            else
                                comparationResult = Compare((T)xItem, (T)yItem);
                        }
                        if (result.HasValue)
                        {
                            if (result != comparationResult || comparationResult == 0)
                                return 0;
                        }
                        else
                        {
                            result = comparationResult;
                        }
                    }
                }
                if (result.HasValue)
                    return result.Value;
                else
                    return Int32.MinValue;
            }
        }

        #endregion

        #region IComparer<T> Members + IComparer<interval<T>> Members

        /// <summary>
        /// Compares two objects and returns a value indicating whether one (a) 
        /// is less than (-1), equal to (0), or greater than (1) the other (b).
        /// </summary>
        /// <param name="a">The first object to compare.</param>
        /// <param name="b">The second object to compare.</param>
        /// <returns>
        /// <list type="table">
        /// <listheader>
        /// <term>Value</term>
        /// <description>Condition</description>
        /// </listheader>
        /// <item>
        /// <term>Less than zero</term>
        /// <description>a is less than b</description>
        /// </item>
        /// <item>
        /// <term>Zero</term>
        /// <description>a equals b</description>
        /// </item>
        /// <item>
        /// <term>Greater than zero</term>
        /// <description>a is greater than b</description>
        /// </item>
        /// </list>
        /// </returns>
        public int Compare(T a, T b)
        {
            //Less than zero 
            // This instance is less than obj. 
            //Zero 
            // This instance is equal to obj. 
            //Greater than zero 
            // This instance is greater than obj. 
            return a.CompareTo(b);
        }

        /// <summary>
        /// Determines whether the <c>first</c> interval 
        /// is less than the <c>second</c> interval.
        /// </summary>
        /// <param name="first">The first interval.</param>
        /// <param name="second">The second interval.</param>
        /// <returns>
        /// <c>true</c> if both intervals are disjunctive and moreover 
        /// the <c>first</c> is less than the <c>second</c>; otherwise, <c>false</c>.
        /// </returns>
        private bool isFirstLessThanSecond(Interval<T> first, Interval<T> second)
        {
            if (first.RightBoundary == BoundaryEnum.Infinity)
                return false;
            if (second.LeftBoundary == BoundaryEnum.Infinity)
                return false;

            int comparationResult = Compare(first.RightValue, second.LeftValue);
            return (
                       (comparationResult < 0)
                       ||
                       (comparationResult == 0 &&
                        !(first.RightBoundary == BoundaryEnum.Closed && second.LeftBoundary == BoundaryEnum.Closed))
                   );
        }

        /// <summary>
        /// Compares the interval bounds.
        /// </summary>
        /// <param name="firstValue">The first value.</param>
        /// <param name="firstBoundary">The first boundary.</param>
        /// <param name="secondValue">The second value.</param>
        /// <param name="secondBoundary">The second boundary.</param>
        /// <param name="intervalSide">The interval side.</param>
        /// <example>
        /// <code>
        /// [pseudocode]
        /// ...
        /// a = (1.12, open)
        /// b = (1.12, closed)
        /// a &gt; b iff side is right
        /// a &lt; b iff side is left
        /// ...
        /// </code>
        /// </example>
        /// <returns>
        /// 1 when first &gt; second;
        /// -1 when first &lt; second;
        /// 0 otherwise
        /// </returns>
        private int compareIntervalBounds(T firstValue, BoundaryEnum firstBoundary, T secondValue,
                                          BoundaryEnum secondBoundary, Side intervalSide)
        {
            if (firstBoundary == BoundaryEnum.Infinity && secondBoundary == BoundaryEnum.Infinity)
                return 0;

            int valueCompare = Compare(firstValue, secondValue);
            if (valueCompare == 0)
                if (firstBoundary == secondBoundary)
                    return 0;
                else if (firstBoundary == BoundaryEnum.Open && secondBoundary == BoundaryEnum.Closed)
                    if (intervalSide == Side.Left)
                        return 1; // 2<, 1( -> 2<1
                    else
                        return -1; // 1), 2> -> 1<2
                else //if (firstBoundary == BoundaryEnum.Closed && secondBoundary == BoundaryEnum.Open)
                    if (intervalSide == Side.Left)
                        return -1; // 1<, 2( -> 1<2
                    else
                        return 1; // 2), 1> -> 2<1
            else // values are <>
                return valueCompare;
        }

        /// <summary>
        /// Compares two specified intervals.
        /// (result is greater than zero iff the first interval <c>a</c> is greater than 
        /// the second interval <c>b</c>)
        /// </summary>
        /// <param name="a">The interval a.</param>
        /// <param name="b">The interval b.</param>
        /// <returns>
        /// <list type="table">
        /// <listheader>
        /// <term>Value</term>
        /// <description>Condition</description>
        /// </listheader>
        /// <item>
        /// <term>Less than zero</term>
        /// <description>first interval <c>a</c> is less than the second interval <c>b</c></description>
        /// </item>
        /// <item>
        /// <term>Zero</term>
        /// <description>Both interval are not disjunctive</description>
        /// </item>
        /// <item>
        /// <term>Greater than zero</term>
        /// <description>first interval <c>a</c> is greater than the second interval <c>b</c></description>
        /// </item>
        /// </list>
        /// </returns>
        public int Compare(Interval<T> a, Interval<T> b)
        {
            // a < b
            if (isFirstLessThanSecond(a, b))
                return -1;
            // b < a
            else if (isFirstLessThanSecond(b, a))
                return 1;
            // intervals are not disjunctive
            else
                return 0;
        }

        /// <summary>
        /// Compares the specified value <c>a</c> and interval <c>b</c>.
        /// (result is greater than zero iff the value <c>a</c> is greater than 
        /// the interval <c>b</c> i.e. <c>a</c> is outside on the right side of the interval)
        /// </summary>
        /// <param name="a">The value a.</param>
        /// <param name="b">The interval b.</param>
        /// <returns></returns>
        public int Compare(T a, Interval<T> b)
        {
            return -Compare(b, a);
        }

        /// <summary>
        /// Compares the specified value <c>b</c> and interval <c>a</c>.
        /// (result is greater than zero iff the interval <c>a</c> is greater than 
        /// the value <c>b</c> i.e. <c>b</c> is outside on the left side of the interval)
        /// </summary>
        /// <param name="a">The interval a.</param>
        /// <param name="b">The value b.</param>
        /// <returns>
        /// <list type="table">
        /// <listheader>
        /// <term>Value</term>
        /// <description>Condition</description>
        /// </listheader>
        /// <item>
        /// <term>Less than zero</term>
        /// <description><c>b</c> value is outside on the right side of the <c>a</c> interval</description>
        /// </item>
        /// <item>
        /// <term>Zero</term>
        /// <description><c>b</c> value is inside the <c>a</c> interval</description>
        /// </item>
        /// <item>
        /// <term>Greater than zero</term>
        /// <description><c>b</c> value is outside on the left side of the <c>a</c> interval</description>
        /// </item>
        /// </list>
        /// </returns>
        public int Compare(Interval<T> a, T b)
        {
            // a = ? a1, a2 ?
            int b2aLeft;
            if (a.LeftBoundary == BoundaryEnum.Infinity)
                b2aLeft = 1;
            else
                b2aLeft = Compare(b, a.LeftValue);

            int b2aRight;
            if (a.RightBoundary == BoundaryEnum.Infinity)
                b2aRight = -1;
            else
                b2aRight = Compare(b, a.RightValue);

            // a1 > b
            if (b2aLeft < 0)
                return 1;

            // a2 < b
            if (b2aRight > 0)
                return -1;

            // a1 < b < a2
            if (b2aLeft > 0 && b2aRight < 0)
                return 0;

            // b = a1
            if (b2aLeft == 0)
            {
                // a is a1 excluding
                if (a.LeftBoundary == BoundaryEnum.Open)
                    return 1;
                else
                    return 0;
            }

            // b = a2
            if (b2aRight == 0)
            {
                if (a.RightBoundary == BoundaryEnum.Open)
                    return -1;
                else
                    return 0;
            }

            throw new Exception("This should never happend. See implementation above for faults.");
        }

        #endregion

        #region Dictionary

        #region IDictionary<string,Category<T>> Members

        /// <summary>
        /// Adds new category of specified name (<c>key</c>). The newly created
        /// category should be filled up by adding some intervals/values of enumeration.
        /// </summary>
        /// <param name="key">The key.</param>
        public void Add(string key)
        {
            _categories.Add(key, new Category<T>(this));
        }

        /// <summary>
        /// Determines whether the attribute contains category of specified name (<c>key</c>).
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>
        /// 	<c>true</c> if the attribute contains category of specified name (<c>key</c>); otherwise, <c>false</c>.
        /// </returns>
        public bool ContainsKey(string key)
        {
            return _categories.ContainsKey(key);
        }

        /// <summary>
        /// Gets the names of categories in the attribute.
        /// </summary>
        /// <value>The keys.</value>
        public ICollection<string> Keys
        {
            get { return _categories.Keys; }
        }

        /// <summary>
        /// Removes the category of specified name (<c>key</c>).
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public bool Remove(string key)
        {
            Axis.NotValid(true, true);
            if (key == NullContainingCategory)
                NullContainingCategory = null;
            return _categories.Remove(key);
        }

        /// <summary>
        /// Tries to get the value (the category of specified name (<c>key</c>)).
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public bool TryGetValue(string key, out Category<T> value)
        {
            return _categories.TryGetValue(key, out value);
        }

        /// <summary>
        /// Gets the values (categories of the attribute).
        /// </summary>
        /// <value>The values.</value>
        public ICollection<Category<T>> Values
        {
            get { return _categories.Values; }
        }

        /// <summary>
        /// Gets the <see cref="T:Ferda.Guha.Attribute.Category`1"/> with the specified key.
        /// </summary>
        /// <value></value>
        public Category<T> this[string key]
        {
            get { return _categories[key]; }
        }

        #endregion

        #region ICollection<KeyValuePair<string,Category<T>>> Members

        /// <summary>
        /// Clears this instance.
        /// </summary>
        public void Clear()
        {
            _categories.Clear();
            _nullContainingCategory = null;
            Axis.NotValid(true, true);
        }

        /// <summary>
        /// Gets the count of categories in the attribute.
        /// </summary>
        /// <value>The count of categories in the attribute.</value>
        public int Count
        {
            get { return _categories.Count; }
        }

        #endregion

        #region IEnumerable<KeyValuePair<string,Category<T>>> Members

        /// <summary>
        /// Returns an enumerator that iterates through the collection of categories.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"></see> that can be used to iterate through the collection of categories.
        /// </returns>
        public IEnumerator<KeyValuePair<string, Category<T>>> GetEnumerator()
        {
            return _categories.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        /// <summary>
        /// Returns an enumerator that iterates through a collection of categories.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"></see> object that can be used to iterate through the collection of categories.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return _categories.GetEnumerator();
        }

        #endregion

        #endregion
    }

    /// <summary>
    /// Name of newly created category by join.
    /// </summary>
    public enum NewCategoryName
    {
        /// <summary>
        /// Defalt label (derived by content) of newly created category
        /// </summary>
        Default,

        /// <summary>
        /// Concatenation of previous names
        /// </summary>
        JoinPreviousNames,

        /// <summary>
        /// User specified new name
        /// </summary>
        Own
    }
}