// Category.cs - class for working with category
//
// Author: Alexander Kuzmin <alexander.kuzmin@gmail.com>
//
// Copyright (c) 2005 Alexander Kuzmin
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
using System.Collections;
using Ferda;

namespace Ferda.FrontEnd.AddIns.EditCategories
{
    #region Data structures definition

    /// <summary>
    /// Category type enum
    /// </summary>
    public enum CategoryType
    {
        Interval, Enumeration
    };

    /// <summary>
    /// Element type enum
    /// </summary>
    internal enum ElementValueType
    {
        LongValue, FloatValue, DateTimeValue, StringValue
    };

    /// <summary>
    /// Categories element
    /// </summary>
    internal struct CategoriesElement
    {
        internal IntervalBoundType bType;
        internal ElementValueType valueType;
        internal long valLong;
        internal float valFloat;
        internal string valString;
        internal string valDateTime;
    };

    #endregion

    /// <summary>
    /// Class for a category. Category can contain intervals or sets.
    /// </summary>
    public class Category
    {
        #region Private variables

        /// <summary>
        /// Category can be either an interval or a set.
        /// </summary>
        private CategoryType cattype = CategoryType.Interval;

        /// <summary>
        /// Frequency value for the category.
        /// </summary>
        private int frequency = 0;

        /// <summary>
        /// Set in the category.
        /// </summary>
        private SingleSet set = new SingleSet(new ArrayList());

        /// <summary>
        /// List of intervals in the multiset.
        /// </summary>
        private ArrayList Intervals = new ArrayList();

        /// <summary>
        /// Name of current category
        /// </summary>
        private String name = "";

        #endregion


        #region Public Getters / setters

        /// <summary>
        /// Gets or sets category type
        /// </summary>
        public CategoryType CatType
        {
            get
            {
                return cattype;
            }
            set
            {
                cattype = value;
            }
        }

        /// <summary>
        /// Gets or sets category name
        /// </summary>
        public String Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }

        /// <summary>
        /// Gets or sets a set in the category
        /// </summary>
        public SingleSet Set
        {
            get
            {
                return this.set;
            }
            set
            {
                this.set = value;
            }
        }

        /// <summary>
        /// Gets or sets frequency in the category
        /// </summary>
        public int Frequency
        {
            get
            {
                return frequency;
            }
            set
            {
                frequency = value;
            }
        }

        #endregion


        #region Category methods

        /// <summary>
        /// Gets all of the intervals in the multiset.
        /// </summary>
        /// <returns>Arraylist with intervals</returns>
        public ArrayList GetIntervals()
        {
            return this.Intervals;
        }


        /// <summary>
        /// Clear set values
        /// </summary>
        public void RemoveSetValues()
        {
            this.Set = new SingleSet(new ArrayList());
        }


        /// <summary>
        /// Removes interval at the required index
        /// </summary>
        /// <param name="index">Index to remove the interval from</param>
        public void RemoveInterval(int index)
        {
            this.Intervals.RemoveAt(index);
        }


        /// <summary>
        /// Method for displaying the values of intervals or sets in the current multiset.
        /// </summary>
        /// <returns>String with desired info</returns>
        public override String ToString()
        {
            String returnValue;
            switch (this.CatType)
            {
                case CategoryType.Enumeration:
                    returnValue = "";
                    returnValue = returnValue + "[ { ";
                    bool first = true;
                    foreach (object value in this.Set.Values)
                    {
                        if (first)
                            returnValue = returnValue + value.ToString();

                        else
                            returnValue = returnValue + "," + value.ToString();

                        first = false;
                    }
                    returnValue = returnValue + " }";
                    break;

                case CategoryType.Interval:
                    returnValue = "[";
                    bool firstRun = true;
                    foreach (Interval interval in this.Intervals)
                    {
                        if (firstRun)
                        {
                            returnValue = returnValue + interval.ToString();
                            firstRun = false;
                        }
                        else
                        {
                            returnValue = returnValue + ", " + interval.ToString();
                        }
                    }
                    break;
                default:
                    throw new Exception("Switch branch not implemented");
            }
            returnValue = returnValue + " ]";
            return returnValue;
        }


        /// <summary>
        /// Method to check whether any of the enum values are in interval
        /// </summary>
        /// <param name="interval"></param>
        /// <returns></returns>
        public bool IntervalDisjunctWithCurrentEnums(Interval interval)
        {
            foreach (object value in this.Set.Values)
            {
                switch (interval.intervalType)
                {
                    case IntervalType.Float:
                        try
                        {
                            float temp = 0;
                            temp = (float)Convert.ToDouble(value);
                            if (interval.IsInInterval(temp))
                                return false;
                        }
                        catch
                        {
                            break;
                        }
                        break;

                    case IntervalType.Long:
                        try
                        {
                            int temp = 0;
                            temp = Convert.ToInt32(value);
                            if (interval.IsInInterval(temp))
                                return false;
                        }
                        catch
                        {
                            break;
                        }
                        break;
                    default:
                        throw new Exception("Switch branch not implemented");
                }
            }
            return true;
        }

        /// <summary>
        /// Adds a new interval to the category.
        /// </summary>
        /// <param name="interval">New interval</param>
        public void AddInterval(Interval interval)
        {
            if (this.CatType.Equals(CategoryType.Interval))
            {
                this.Intervals.Add(interval);
            }
            else
            {
                throw new ArgumentException("Object is not an interval");
            }
        }

        /// <summary>
        /// Adds a new enum to the category.
        /// </summary>
        /// <param name="singleSet">New set</param>
        public void AddSingleSet(SingleSet singleSet)
        {
            if (this.CatType.Equals(CategoryType.Enumeration))
            {
                foreach (object value in singleSet.Values)
                {
                    this.set.AddValue(value);
                }
            }
            else
            {
                throw new ArgumentException("Object is not a Set");
            }
        }

        /// <summary>
        /// Method to determine whether the searched value is contained
        /// in the intervals of the multiset or in its successors.
        /// </summary>
        /// <param name="value">An integer to search for</param>
        /// <returns>If the integer is contained in the intervals.</returns>
        public bool IsInInterval(int value)
        {
            //searching for the value on the current level
            foreach (Interval interval in this.Intervals)
            {
                if (interval.IsInInterval(value))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// To determine, if the searched interval is disjunct with all intervals in the current multiset.
        /// </summary>
        /// <param name="interval">Interval to search for</param>
        /// <returns>Whethter the interval is disjunct</returns>
        public bool IntervalIsDisjunct(Interval interval)
        {
            //searching for the value on the current level
            foreach (Interval interval1 in this.Intervals)
            {
                if (interval1.IntervalIsDisjunct(interval))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Cannot think of having an interval of non-integer values,
        /// leaving unimplemented for now.
        /// </summary>
        /// <param name="value">Object to search for in the interval</param>
        /// <returns>Always false</returns>
        public bool IsInInterval(object value)
        {
            return false;
        }

        /// <summary>
        /// Method to determine whether the searched value is contained
        /// in the sets of the multiset or in its successors.
        /// </summary>
        /// <param name="value">A value to search for</param>
        /// <returns>If the value is contained in the sets.</returns>
        public bool IsInSet(object value)
        {
            //searching for the value on the current level
            if (this.Set.IsInSet(value))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Method for detecting interval type in the current category
        /// </summary>
        /// <returns></returns>
        public IntervalType GetIntervalType()
        {
            if ((this.CatType == CategoryType.Interval) && (this.Intervals.Count > 0))
            {
                Interval interval = (Interval)this.Intervals[0];
                return interval.intervalType;
            }

            else
            {
                return IntervalType.None;
            }
        }

        /// <summary>
        /// Method to determine whether multiset contains any values besides those in its successors.
        /// </summary>
        /// <param name="multiSet"></param>
        /// <returns></returns>
        public bool MultiSetContainsValues()
        {
            if (this.CatType == CategoryType.Interval)
            {
                if (this.GetIntervals().Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                if (this.Set.Values.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Gets interval at the given index
        /// </summary>
        /// <param name="index">Index to get the interval at</param>
        /// <returns></returns>
        public Interval GetInterval(int index)
        {
            return (Interval)this.Intervals[index];
        }

        #endregion


        #region Non-public static methods

        /// <summary>
        /// Method to check whether one interval is disjunct with CategoriesElement values, adds it if true.
        /// </summary>
        /// <param name="interval">Interval to check and add to values if possible</param>
        /// <param name="values">CategoriesElement list to check and add the interval if possible</param>
        /// <returns>true if interval is disjunct</returns>
        internal static bool IntervalIsDisjunctWithCategories(Interval interval, ref List<CategoriesElement> values)
        {
            int lastSmaller = 0;
            int index = 0;
            foreach (CategoriesElement element in values)
            {
                //it is not an interval
                if (element.bType == IntervalBoundType.None)
                {
                    switch (element.valueType)
                    {
                        case ElementValueType.LongValue:
                            try
                            {
                                if (Interval.IsInInterval(interval, element.valLong))
                                {
                                    return false;
                                }
                                else
                                {
                                    if (element.valLong < interval.lowerBound)
                                    {
                                        lastSmaller = index;
                                    }
                                }
                            }
                            catch
                            {
                                throw new Exception("MixedDomain");
                            }
                            break;

                        case ElementValueType.FloatValue:
                            try
                            {
                                if (Interval.IsInInterval(interval, element.valFloat))
                                {
                                    return false;
                                }
                                else
                                {
                                    if (element.valFloat < interval.lowerBoundFl)
                                    {
                                        lastSmaller = index;
                                    }
                                }
                            }
                            catch
                            {
                                throw new Exception("MixedDomain");
                            }
                            break;

                        case ElementValueType.DateTimeValue:
                            throw new Exception("DateTimeNotImplemented");

                        default:
                            throw new Exception("MixedDomain");
                    }
                }
                //it is an interval
                else
                {
                    switch (element.valueType)
                    {
                        case ElementValueType.LongValue:
                            Interval interval1 = new Interval(IntervalType.Long);
                            interval1.lowerBound = element.valLong;
                            interval1.lowerBoundType = element.bType;
                            int tempIndex = values.IndexOf(element);
                            interval1.upperBound = values[tempIndex + 1].valLong;
                            interval1.upperBoundType = values[tempIndex + 1].bType;

                            try
                            {
                                if (!Interval.IntervalsAreDisjunct(interval, interval1))
                                {
                                    return false;
                                }
                                else
                                {
                                    if (interval1.upperBound < interval.lowerBound)
                                    {
                                        lastSmaller = index;
                                    }
                                }
                            }
                            catch
                            {
                                throw new Exception("MixedDomain");
                            }
                            break;

                        case ElementValueType.FloatValue:
                            Interval interval2 = new Interval(IntervalType.Float);
                            interval2.lowerBoundFl = element.valFloat;
                            interval2.lowerBoundType = element.bType;
                            int tempIndex1 = values.IndexOf(element);
                            interval2.upperBoundFl = values[tempIndex1 + 1].valFloat;
                            interval2.upperBoundType = values[tempIndex1 + 1].bType;

                            try
                            {
                                if (!Interval.IntervalsAreDisjunct(interval, interval2))
                                {
                                    return false;
                                }
                                else
                                {
                                    if (interval2.upperBoundFl < interval.lowerBoundFl)
                                    {
                                        lastSmaller = index;
                                    }
                                }
                            }
                            catch
                            {
                                throw new Exception("MixedDomain");
                            }
                            break;

                        case ElementValueType.DateTimeValue:
                            throw new Exception("DateTimeNotImplemented");

                        default:
                            throw new Exception("MixedDomain");
                    }



                }
                index++;
            }
            return false;
        }

        #endregion


        #region Public static methods

        /// <summary>
        /// Method which checks whether the categories are disjunct
        /// </summary>
        /// <param name="categories">List of categories to check for disjunctivity</param>
        /// <returns>True if categories are disjunct</returns>
        public static bool CategoriesDisjunctivityTest(List<Ferda.Modules.CategoriesStruct> categories)
        {
            List<CategoriesElement> allValues = new List<CategoriesElement>();
            bool longCat = false;
            bool floatCat = false;
            bool enums = false;
            bool dateTime = false;

            if (categories.Count > 0)
            {
                CategoriesElement item = new CategoriesElement();

                foreach (Ferda.Modules.CategoriesStruct category in categories)
                {

                    foreach (Ferda.Modules.LongIntervalCategorySeq longSeq in category.longIntervals)
                    {
                        if ((!floatCat) && (!enums) && (!dateTime))
                        {
                            longCat = true;
                        }
                        else
                        {
                            throw new Exception("MixedCategoryType");
                        }
                    }

                    foreach (Ferda.Modules.FloatIntervalCategorySeq floatSeq in category.floatIntervals)
                    {
                        if ((!longCat) && (!enums) && (!dateTime))
                        {
                            floatCat = true;
                        }
                        else
                        {
                            throw new Exception("MixedCategoryType");
                        }
                    }

                    foreach (Ferda.Modules.EnumCategorySeq enumSeq in category.enums)
                    {
                        if ((!longCat) && (!floatCat) && (!dateTime))
                        {
                            enums = true;
                        }
                        else
                        {
                            throw new Exception("MixedCategoryType");
                        }
                    }

                    foreach (Ferda.Modules.DateTimeIntervalCategorySeq dateTimeSeq in category.dateTimeIntervals)
                    {
                        if ((!longCat) && (!enums) && (!floatCat))
                        {
                            dateTime = true;
                        }
                        else
                        {
                            throw new Exception("MixedCategoryType");
                        }
                    }
                }

                //allValues.Add(
                return false;
            }
            else
            {
                return true;
            }
        }

        #endregion
    }
}