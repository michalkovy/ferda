using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using Ferda;
namespace Ferda
{
    namespace FrontEnd.AddIns.EditCategories
    {
        public enum CategoryType
        {
            Interval, Enumeration
        };

        /// <summary>
        /// Class for a multiset. Multiset can contain intervals, sets or further multisets,
        /// thus implicating a tree structure.
        /// </summary>
        public class Category
        {

            #region Private variables

            /// <summary>
            /// Multiset can be either an interval or a set.
            /// </summary>
            private CategoryType cattype = CategoryType.Interval;

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
            /// Frequency value for the category.
            /// </summary>

            private int frequency = 0;

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

            /// <summary>
            /// List of sets in the multiset.
            /// </summary>
            private SingleSet set = new SingleSet(new ArrayList());

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
            /// List of intervals in the multiset.
            /// </summary>
            private ArrayList Intervals = new ArrayList();


            /// <summary>
            /// Name of current multiset
            /// </summary>
            private String name = "";
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

            #endregion


            #region Constructor


            #endregion

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
            /// Adds a new interval to the multiset.
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
            /// Adds a new set to the multiset.
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

        }


    }
}