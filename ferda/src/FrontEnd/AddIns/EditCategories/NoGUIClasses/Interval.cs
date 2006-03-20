// Interval.cs - class for working with interval
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

namespace Ferda.FrontEnd.AddIns.EditCategories
{
    #region Datastructures definition

    /// <summary>
    /// Interval bound type
    /// </summary>
    public enum IntervalBoundType
    {
        Sharp, Round, Infinity, None
    };

    /// <summary>
    /// Available interval types
    /// </summary>
    public enum IntervalType
    {
        Long, Float, DateTime, None
    };

    #endregion
    
    /// <summary>
    /// Class implementing an interval and basic functions for working with it
    /// </summary>
    public class Interval : IComparable
    {

        #region Private variables + public get/set

        /// <summary>
        /// Indicates whether left bound is set
        /// </summary>
        private bool leftSet = false;

        /// <summary>
        /// Indicates whether right bound is set
        /// </summary>
        private bool rightSet = false;

        /// <summary>
        /// Indicates whether left bound type is set
        /// </summary>
        private bool leftTypeSet = false;

        /// <summary>
        /// Indicates whether right bound type is set
        /// </summary>
        private bool rightTypeSet = false;

        /// <summary>
        /// Type of interval
        /// </summary>
        private IntervalType itype = IntervalType.None;
        public IntervalType intervalType
        {
            get
            {
                return itype;
            }
            set
            {
                itype = value;
            }
        }

        /// <summary>
        /// Lower long bound of the interval
        /// </summary>
        private long lbound;

        public long lowerBound
        {
            get
            {
                if (this.intervalType != IntervalType.Long)
                {
                    throw new Exception("InvalidIntervalType");
                }
                else
                {
                    if (!leftSet)
                    {
                        throw new Exception("LeftBoundNotSet");
                    }
                    return lbound;
                }
            }
            set
            {
                if (this.intervalType != IntervalType.Long)
                {
                    throw new Exception("InvalidIntervalType");
                }
                else
                {
                    lbound = value;
                    leftSet = true;
                }
            }
        }

        /// <summary>
        /// Upper bound of the interval
        /// </summary>
        private long ubound;
        public long upperBound
        {
            get
            {
                if (this.intervalType != IntervalType.Long)
                {
                    throw new Exception("InvalidIntervalType");
                }
                else
                {
                    if (!rightSet)
                    {
                        throw new Exception("RightBoundNotSet");
                    }
                    return ubound;
                }
            }

            set
            {
                if (this.intervalType != IntervalType.Long)
                {
                    throw new Exception("InvalidIntervalType");
                }
                else
                {
                    ubound = value;
                    rightSet = true;
                }
            }
        }

        /// <summary>
        /// Lower float bound of the interval
        /// </summary>
        private float lboundf;
        public float lowerBoundFl
        {
            get
            {
                if (this.intervalType != IntervalType.Float)
                {
                    throw new Exception("InvalidIntervalType");
                }
                else
                {
                    if (!leftSet)
                    {
                        throw new Exception("LeftBoundNotSet");
                    }
                    return lboundf;
                }
            }
            set
            {
                if (this.intervalType != IntervalType.Float)
                {
                    throw new Exception("InvalidIntervalType");
                }
                else
                {
                    lboundf = value;
                    leftSet = true;
                }
            }
        }

        /// <summary>
        /// Upper float bound of the interval
        /// </summary>
        private float uboundf;
        public float upperBoundFl
        {
            get
            {
                if (this.intervalType != IntervalType.Float)
                {
                    throw new Exception("InvalidIntervalType");
                }
                else
                {
                    if (!rightSet)
                    {
                        throw new Exception("RightBoundNotSet");
                    }
                    return uboundf;
                }
            }
            set
            {
                if (this.intervalType != IntervalType.Float)
                {
                    throw new Exception("InvalidIntervalType");
                }
                else
                {
                    uboundf = value;
                    rightSet = true;
                }
            }
        }



        /// <summary>
        /// Type of lower bound of the interval
        /// </summary>
        private IntervalBoundType lboundtype;
        public IntervalBoundType lowerBoundType
        {
            get
            {
                if (!leftTypeSet)
                {
                    throw new Exception("LeftBountTypeNotSet");
                }
                return lboundtype;
            }
            set
            {
                lboundtype = value;
                leftTypeSet = true;
            }
        }


        /// <summary>
        /// Type of upper bound for the interval
        /// </summary>
        private IntervalBoundType uboundtype;
        public IntervalBoundType upperBoundType
        {
            get
            {
                if (!rightTypeSet)
                {
                    throw new Exception("RightBoundTypeNotSet");
                }
                return uboundtype;
            }
            set
            {
                uboundtype = value;
                rightTypeSet = true;
            }
        }

        #endregion


        #region Constructor

        /// <summary>
        /// Constructor to create an interval of the desired type
        /// </summary>
        /// <param name="intervalType">Type of interval to create</param>
        public Interval(IntervalType intervalType)
        {
            this.intervalType = intervalType;
        }

        #endregion


        #region CompareTo overload

        /// <summary>
        ///Overloaded CompareTo method to correctly compare intervals
        ///in this particular case, it's enough to compare lower bounds,
        ///as the intervals are kept disjunct
        /// </summary>
        /// <param name="obj">An object, only interval is accepted</param>
        /// <returns>Not specified</returns>
        public int CompareTo(object obj)
        {
            if (obj is Interval)
            {
                Interval interval = (Interval)obj;
                return lowerBound.CompareTo(interval.lowerBound);
            }
            throw new ArgumentException("NotInterval");
        }

        #endregion


        #region Non-static disjunctivity tests

        /// <summary>
        /// Checks whether the value is in current interval.
        /// </summary>
        /// <param name="value">Integer value to check</param>
        /// <returns>Whether the checked value is in the current interval</returns>
        public bool IsInInterval(float value)
        {
            switch (this.upperBoundType)
            {
                case IntervalBoundType.Round:
                    switch (this.lowerBoundType)
                    {
                        // ( , )
                        case IntervalBoundType.Round:
                            if ((value > this.lowerBoundFl) && (value < this.upperBoundFl))
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        // < , )
                        case IntervalBoundType.Sharp:
                            if ((value >= this.lowerBoundFl) && (value < this.upperBoundFl))
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        // (-inf, )
                        case IntervalBoundType.Infinity:
                            if (value < this.upperBoundFl)
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        default:
                            break;
                    }
                    break;
                case IntervalBoundType.Sharp:
                    switch (this.lowerBoundType)
                    {
                        // ( , >
                        case IntervalBoundType.Round:
                            if ((value > this.lowerBoundFl) && (value <= this.upperBoundFl))
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        // < , >
                        case IntervalBoundType.Sharp:
                            if ((value >= this.lowerBoundFl) && (value <= this.upperBoundFl))
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        //(-inf , >
                        case IntervalBoundType.Infinity:
                            if (value <= this.upperBoundFl)
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        default:
                            break;
                    }
                    break;
                case IntervalBoundType.Infinity:
                    switch (this.lowerBoundType)
                    {
                        // ( , +inf)
                        case IntervalBoundType.Round:
                            if (value > this.lowerBoundFl)
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        // < , +inf)
                        case IntervalBoundType.Sharp:
                            if (value >= this.lowerBoundFl)
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        //(-inf , +inf)
                        case IntervalBoundType.Infinity:
                            return true;
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }
            return false;
        }

        /// <summary>
        /// Checks whether the interval is disjunct with the current one.
        /// </summary>
        /// <param name="interval">Interval to check</param>
        /// <returns>Whether the checked interval is disjunct with the current one</returns>
        public bool IntervalIsDisjunct(Interval interval1)
        {
            bool special = false;
            float intervalUpperBound;
            float intervalLowerBound;
            float thisUpperBound;
            float thisLowerBound;
            if (interval1.intervalType == IntervalType.Long)
            {
                intervalUpperBound = interval1.upperBound;
                intervalUpperBound = (int)intervalUpperBound;
                intervalLowerBound = interval1.lowerBound;
                intervalLowerBound = (int)intervalLowerBound;
                thisUpperBound = this.upperBound;
                thisUpperBound = (int)thisUpperBound;
                thisLowerBound = this.lowerBound;
                thisLowerBound = (int)thisLowerBound;
            }

            else
            {
                if (interval1.intervalType == IntervalType.Float)
                {
                    intervalUpperBound = interval1.upperBoundFl;
                    intervalLowerBound = interval1.lowerBoundFl;
                    thisLowerBound = this.lowerBoundFl;
                    thisUpperBound = this.upperBoundFl;
                }
                else
                {
                    return true;
                }
            }
            if (interval1.upperBoundType != IntervalBoundType.Infinity)
            {
                if (interval1.upperBoundType == IntervalBoundType.Sharp)
                {
                    if (this.IsInInterval(intervalUpperBound))
                    {
                        return false;
                    }
                }
                else
                {
                    special = true;
                }
            }
            else
            {
                if (this.upperBoundType != IntervalBoundType.Infinity)
                {
                    if (this.upperBoundType == IntervalBoundType.Sharp)
                    {
                        if (interval1.IsInInterval(thisUpperBound))
                        {
                            return false;
                        }
                    }
                    else
                    {
                        special = true;
                    }
                }
                else
                {
                    //both upper bounds are infinite, cannot be disjunct
                    return false;
                }
            }
            if (interval1.lowerBoundType != IntervalBoundType.Infinity)
            {
                if (interval1.lowerBoundType == IntervalBoundType.Sharp)
                {
                    if (this.IsInInterval(intervalLowerBound))
                    {
                        return false;
                    }
                }
                else
                {
                    special = true;
                }
            }
            else
            {
                if (this.lowerBoundType != IntervalBoundType.Infinity)
                {
                    if (this.lowerBoundType == IntervalBoundType.Sharp)
                    {
                        if (interval1.IsInInterval(thisLowerBound))
                        {
                            return false;
                        }
                    }
                    else
                    {
                        special = true;
                    }
                }
                else
                {
                    //both lower bounds are minus infinity, cannot be disjunct
                    return false;
                }
            }
            if (special)
            {
                if (this.upperBoundType == IntervalBoundType.Sharp)
                {
                    if (interval1.IsInInterval(thisUpperBound))
                    {
                        return false;
                    }
                }
                if (this.lowerBoundType == IntervalBoundType.Sharp)
                {
                    if (interval1.IsInInterval(thisLowerBound))
                    {
                        return false;
                    }
                }
                if (interval1.upperBoundType == IntervalBoundType.Infinity)
                {
                    if (interval1.lowerBoundType == IntervalBoundType.Round)
                    {
                        if (interval1.IsInInterval(thisUpperBound))
                        {
                            return false;
                        }
                    }
                    if (interval1.lowerBoundType == IntervalBoundType.Sharp)
                    {
                        if (thisUpperBound != intervalLowerBound)
                        {
                            if (interval1.IsInInterval(thisUpperBound))
                            {
                                return false;
                            }
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
                if (interval1.lowerBoundType == IntervalBoundType.Infinity)
                {
                    if (interval1.upperBoundType == IntervalBoundType.Round)
                    {
                        if (interval1.IsInInterval(thisLowerBound))
                        {
                            return false;
                        }
                    }
                    if (interval1.upperBoundType == IntervalBoundType.Sharp)
                    {
                        if (thisLowerBound != intervalUpperBound)
                        {
                            if (interval1.IsInInterval(thisLowerBound))
                            {
                                return false;
                            }
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
                if (this.upperBoundType == IntervalBoundType.Infinity)
                {
                    if (this.lowerBoundType == IntervalBoundType.Round)
                    {
                        if (this.IsInInterval(intervalUpperBound))
                        {
                            return false;
                        }
                    }
                    if (this.lowerBoundType == IntervalBoundType.Sharp)
                    {
                        if (intervalUpperBound != thisLowerBound)
                        {
                            if (this.IsInInterval(intervalUpperBound))
                            {
                                return false;
                            }
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
                if (this.lowerBoundType == IntervalBoundType.Infinity)
                {
                    if (this.upperBoundType == IntervalBoundType.Round)
                    {
                        if (this.IsInInterval(intervalLowerBound))
                        {
                            return false;
                        }
                    }
                    if (this.upperBoundType == IntervalBoundType.Sharp)
                    {
                        if (intervalLowerBound != thisUpperBound)
                        {
                            if (this.IsInInterval(intervalLowerBound))
                            {
                                return false;
                            }
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
                if ((thisLowerBound > intervalLowerBound) && (thisLowerBound < intervalUpperBound))
                {
                    return false;
                }
                if ((thisUpperBound > intervalLowerBound) && (thisUpperBound < intervalUpperBound))
                {
                    return false;
                }
                if ((intervalLowerBound > thisLowerBound) && (intervalLowerBound < thisUpperBound))
                {
                    return false;
                }
                if ((intervalUpperBound > thisLowerBound) && (intervalUpperBound < thisUpperBound))
                {
                    return false;
                }
                if ((intervalLowerBound == thisLowerBound) && (intervalUpperBound == thisUpperBound))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Checks whether the value is in current interval.
        /// </summary>
        /// <param name="value">Integer value to check</param>
        /// <returns>Whether the checked value is in the current interval</returns>
        public bool IsInInterval(int value)
        {
            switch (this.upperBoundType)
            {
                case IntervalBoundType.Round:
                    switch (this.lowerBoundType)
                    {
                        // ( , )
                        case IntervalBoundType.Round:
                            if ((value > this.lowerBound) && (value < this.upperBound))
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        // < , )
                        case IntervalBoundType.Sharp:
                            if ((value >= this.lowerBound) && (value < this.upperBound))
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        // (-inf, )
                        case IntervalBoundType.Infinity:
                            if (value < this.upperBound)
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        default:
                            break;
                    }
                    break;
                case IntervalBoundType.Sharp:
                    switch (this.lowerBoundType)
                    {
                        // ( , >
                        case IntervalBoundType.Round:
                            if ((value > this.lowerBound) && (value <= this.upperBound))
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        // < , >
                        case IntervalBoundType.Sharp:
                            if ((value >= this.lowerBound) && (value <= this.upperBound))
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        //(-inf , >
                        case IntervalBoundType.Infinity:
                            if (value <= this.upperBound)
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        default:
                            break;
                    }
                    break;
                case IntervalBoundType.Infinity:
                    switch (this.lowerBoundType)
                    {
                        // ( , +inf)
                        case IntervalBoundType.Round:
                            if (value > this.lowerBound)
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        // < , +inf)
                        case IntervalBoundType.Sharp:
                            if (value >= this.lowerBound)
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        //(-inf , +inf)
                        case IntervalBoundType.Infinity:
                            return true;
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }
            return false;
        }

        /// <summary>
        /// Method to check whether interval contains the given value
        /// </summary>
        /// <param name="value">Value to check for</param>
        /// <returns>True if value is contained in the interval</returns>
        public bool IsInInterval(object value)
        {
            try
            {
                float temp = (float)Convert.ToDouble(value);
                return this.IsInInterval(temp);
            }
            catch
            {
                throw new ArgumentException("Cannot be converted to float");
            }
        }

        #endregion


        #region Static methods for disjunctivity tests

        /// <summary>
        /// Checks whether the value is contained in the interval
        /// </summary>
        /// <param name="interval">Checked interval</param>
        /// <param name="value">Checked value</param>
        /// <returns>True if interval contains value</returns>
        public static bool IsInInterval(Interval interval, float value)
        {
            if (interval.intervalType != IntervalType.Float)
            {
                throw new Exception("InvalidIntervalType");
            }
            //return true;
            switch (interval.upperBoundType)
            {
                case IntervalBoundType.Round:
                    switch (interval.lowerBoundType)
                    {
                        // ( , )
                        case IntervalBoundType.Round:
                            if ((value > interval.lowerBoundFl) && (value < interval.upperBoundFl))
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        // < , )
                        case IntervalBoundType.Sharp:
                            if ((value >= interval.lowerBoundFl) && (value < interval.upperBoundFl))
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        // (-inf, )
                        case IntervalBoundType.Infinity:
                            if (value < interval.upperBoundFl)
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        default:
                            break;
                    }
                    break;
                case IntervalBoundType.Sharp:
                    switch (interval.lowerBoundType)
                    {
                        // ( , >
                        case IntervalBoundType.Round:
                            if ((value > interval.lowerBoundFl) && (value <= interval.upperBoundFl))
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        // < , >
                        case IntervalBoundType.Sharp:
                            if ((value >= interval.lowerBoundFl) && (value <= interval.upperBoundFl))
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        //(-inf , >
                        case IntervalBoundType.Infinity:
                            if (value <= interval.upperBoundFl)
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        default:
                            break;
                    }
                    break;
                case IntervalBoundType.Infinity:
                    switch (interval.lowerBoundType)
                    {
                        // ( , +inf)
                        case IntervalBoundType.Round:
                            if (value > interval.lowerBoundFl)
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        // < , +inf)
                        case IntervalBoundType.Sharp:
                            if (value >= interval.lowerBoundFl)
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        //(-inf , +inf)
                        case IntervalBoundType.Infinity:
                            return true;
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }
            return false;
        }

        /// <summary>
        /// Checks whether the value is contained in the interval
        /// </summary>
        /// <param name="interval">Checked interval</param>
        /// <param name="value">Checked value</param>
        /// <returns>True if interval contains value</returns>
        public static bool IsInInterval(Interval interval, long value)
        {
            if (interval.intervalType != IntervalType.Long)
            {
                throw new Exception("InvalidIntervalType");
            }

            switch (interval.upperBoundType)
            {
                case IntervalBoundType.Round:
                    switch (interval.lowerBoundType)
                    {
                        // ( , )
                        case IntervalBoundType.Round:
                            if ((value > interval.lowerBound) && (value < interval.upperBound))
                            {
                                return true;
                            }

                            else
                            {
                                return false;
                            }

                        // < , )
                        case IntervalBoundType.Sharp:
                            if ((value >= interval.lowerBound) && (value < interval.upperBound))
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }

                        // (-inf, )

                        case IntervalBoundType.Infinity:
                            if (value < interval.upperBound)
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        default:
                            break;
                    }
                    break;

                case IntervalBoundType.Sharp:
                    switch (interval.lowerBoundType)
                    {
                        // ( , >
                        case IntervalBoundType.Round:
                            if ((value > interval.lowerBound) && (value <= interval.upperBound))
                            {
                                return true;
                            }

                            else
                            {
                                return false;
                            }
                        // < , >
                        case IntervalBoundType.Sharp:
                            if ((value >= interval.lowerBound) && (value <= interval.upperBound))
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }

                        //(-inf , >
                        case IntervalBoundType.Infinity:
                            if (value <= interval.upperBound)
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        default:
                            break;
                    }
                    break;

                case IntervalBoundType.Infinity:
                    switch (interval.lowerBoundType)
                    {
                        // ( , +inf)
                        case IntervalBoundType.Round:
                            if (value > interval.lowerBound)
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }

                        // < , +inf)
                        case IntervalBoundType.Sharp:
                            if (value >= interval.lowerBound)
                            {
                                return true;
                            }

                            else
                            {
                                return false;
                            }

                        //(-inf , +inf)
                        case IntervalBoundType.Infinity:
                            return true;

                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }
            return false;
        }

        /// <summary>
        /// Checks whether the value is contained in the interval 
        /// </summary>
        /// <param name="interval">Checked interval</param>
        /// <param name="value">Checked value</param>
        /// <returns>True if interval contains value</returns>
        public static bool IsInInterval(Interval interval, object value)
        {
            throw new Exception("MethodNotImplemented");
        }

        /// <summary>
        /// Method to check whether the two intervals are disjunct
        /// </summary>
        /// <param name="interval1">First interval</param>
        /// <param name="interval2">Second interval</param>
        /// <returns>True if checked intervals are disjunct</returns>
        public static bool IntervalsAreDisjunct(Interval interval1, Interval interval2)
        {
            if (interval1.intervalType != interval2.intervalType)
            {
                throw new Exception("DifferentIntervalTypes");
            }

            bool special = false;
            float int2UBFloat = 0;
            float int2LBFloat = 0;
            float int1UBFloat = 0;
            float int1LBFloat = 0;

            long int1UBLong = 0;
            long int2UBLong = 0;
            long int1LBLong = 0;
            long int2LBLong = 0;

            switch (interval1.intervalType)
            {
                case IntervalType.Long:
                    int2UBLong = interval2.upperBound;
                    int2LBLong = interval2.lowerBound;
                    int1UBLong = interval1.upperBound;
                    int1LBLong = interval1.lowerBound;
                    break;

                case IntervalType.Float:
                    int2UBFloat = interval2.upperBoundFl;
                    int2LBFloat = interval2.lowerBoundFl;
                    int1LBFloat = interval1.lowerBoundFl;
                    int1UBFloat = interval1.upperBoundFl;
                    break;

                default:
                    throw new Exception("IntervalTypeNotImplemented");
            }


            if (interval2.upperBoundType != IntervalBoundType.Infinity)
            {
                if (interval2.upperBoundType == IntervalBoundType.Sharp)
                {
                    switch (interval1.intervalType)
                    {
                        case IntervalType.Long:
                            if (Interval.IsInInterval(interval1, int2UBLong))
                            {
                                return false;
                            }
                            break;

                        case IntervalType.Float:
                            if (Interval.IsInInterval(interval1, int2UBFloat))
                            {
                                return false;
                            }
                            break;

                        default:
                            throw new Exception("SwitchBranchNotImplemented");
                    }
                }
                else
                {
                    special = true;
                }
            }
            else
            {
                if (interval1.upperBoundType != IntervalBoundType.Infinity)
                {
                    if (interval1.upperBoundType == IntervalBoundType.Sharp)
                    {
                        switch (interval1.intervalType)
                        {
                            case IntervalType.Long:
                                if (Interval.IsInInterval(interval2, int1UBLong))
                                {
                                    return false;
                                }
                                break;

                            case IntervalType.Float:
                                if (Interval.IsInInterval(interval2, int1UBFloat))
                                {
                                    return false;
                                }
                                break;

                            default:
                                throw new Exception("SwitchBranchNotImplemented");
                        }
                    }
                    else
                    {
                        special = true;
                    }
                }
                else
                {
                    //both upper bounds are infinite, cannot be disjunct
                    return false;
                }
            }

            if (interval2.lowerBoundType != IntervalBoundType.Infinity)
            {
                if (interval2.lowerBoundType == IntervalBoundType.Sharp)
                {
                    switch (interval1.intervalType)
                    {
                        case IntervalType.Long:
                            if (Interval.IsInInterval(interval1, int2LBLong))
                            {
                                return false;
                            }
                            break;

                        case IntervalType.Float:
                            if (Interval.IsInInterval(interval1, int2LBFloat))
                            {
                                return false;
                            }
                            break;

                        default:
                            throw new Exception("SwitchBranchNotImplemented");
                    }
                }
                else
                {
                    special = true;
                }
            }
            else
            {
                if (interval1.lowerBoundType != IntervalBoundType.Infinity)
                {
                    if (interval1.lowerBoundType == IntervalBoundType.Sharp)
                    {
                        switch (interval1.intervalType)
                        {
                            case IntervalType.Long:
                                if (Interval.IsInInterval(interval2, int1LBLong))
                                {
                                    return false;
                                }
                                break;

                            case IntervalType.Float:
                                if (Interval.IsInInterval(interval2, int1LBFloat))
                                {
                                    return false;
                                }
                                break;

                            default:
                                throw new Exception("SwitchBranchNotImplemented");
                        }
                    }
                    else
                    {
                        special = true;
                    }
                }
                else
                {
                    //both lower bounds are minus infinity, cannot be disjunct
                    return false;
                }
            }

            if (special)
            {
                if (interval1.upperBoundType == IntervalBoundType.Sharp)
                {
                    switch (interval1.intervalType)
                    {
                        case IntervalType.Long:
                            if (Interval.IsInInterval(interval2, int1UBLong))
                            {
                                return false;
                            }
                            break;

                        case IntervalType.Float:
                            if (Interval.IsInInterval(interval2, int1UBFloat))
                            {
                                return false;
                            }
                            break;

                        default:
                            throw new Exception("SwitchBranchNotImplemented");
                    }
                }
                if (interval1.lowerBoundType == IntervalBoundType.Sharp)
                {
                    switch (interval1.intervalType)
                    {
                        case IntervalType.Long:
                            if (Interval.IsInInterval(interval2, int1LBLong))
                            {
                                return false;
                            }
                            break;

                        case IntervalType.Float:
                            if (Interval.IsInInterval(interval2, int1LBFloat))
                            {
                                return false;
                            }
                            break;

                        default:
                            throw new Exception("SwitchBranchNotImplemented");
                    }
                }

                if (interval2.upperBoundType == IntervalBoundType.Infinity)
                {
                    if (interval2.lowerBoundType == IntervalBoundType.Round)
                    {
                        switch (interval1.intervalType)
                        {
                            case IntervalType.Long:
                                if (Interval.IsInInterval(interval2, int1UBLong))
                                {
                                    return false;
                                }
                                break;

                            case IntervalType.Float:
                                if (Interval.IsInInterval(interval2, int1UBFloat))
                                {
                                    return false;
                                }
                                break;

                            default:
                                throw new Exception("SwitchBranchNotImplemented");
                        }
                    }
                    if (interval2.lowerBoundType == IntervalBoundType.Sharp)
                    {
                        switch (interval1.intervalType)
                        {
                            case IntervalType.Long:
                                if (int1UBLong != int2LBLong)
                                {
                                    if (Interval.IsInInterval(interval2, int1UBLong))
                                    {
                                        return false;
                                    }
                                }
                                else
                                {
                                    return false;
                                }
                                break;

                            case IntervalType.Float:
                                if (int1UBFloat != int2LBFloat)
                                {
                                    if (Interval.IsInInterval(interval2, int1UBFloat))
                                    {
                                        return false;
                                    }
                                }
                                else
                                {
                                    return false;
                                }
                                break;

                            default:
                                throw new Exception("SwitchBranchNotImplemented");
                        }
                    }
                }
                if (interval2.lowerBoundType == IntervalBoundType.Infinity)
                {
                    if (interval2.upperBoundType == IntervalBoundType.Round)
                    {
                        switch (interval1.intervalType)
                        {
                            case IntervalType.Long:
                                if (Interval.IsInInterval(interval2, int1LBLong))
                                {
                                    return false;
                                }
                                break;

                            case IntervalType.Float:
                                if (Interval.IsInInterval(interval2, int1LBFloat))
                                {
                                    return false;
                                }
                                break;

                            default:
                                throw new Exception("SwitchBranchNotImplemented");
                        }
                    }
                    if (interval2.upperBoundType == IntervalBoundType.Sharp)
                    {
                        switch (interval1.intervalType)
                        {
                            case IntervalType.Long:
                                if (int1LBLong != int2UBLong)
                                {
                                    if (Interval.IsInInterval(interval2, int1LBLong))
                                    {
                                        return false;
                                    }
                                }
                                else
                                {
                                    return false;
                                }
                                break;

                            case IntervalType.Float:
                                if (int1LBFloat != int2UBFloat)
                                {
                                    if (Interval.IsInInterval(interval2, int1LBFloat))
                                    {
                                        return false;
                                    }
                                }
                                else
                                {
                                    return false;
                                }
                                break;

                            default:
                                throw new Exception("SwitchBranchNotImplemented");
                        }

                    }
                }
                if (interval1.upperBoundType == IntervalBoundType.Infinity)
                {
                    if (interval1.lowerBoundType == IntervalBoundType.Round)
                    {
                        switch (interval1.intervalType)
                        {
                            case IntervalType.Long:
                                if (Interval.IsInInterval(interval1, int2UBLong))
                                {
                                    return false;
                                }
                                break;

                            case IntervalType.Float:
                                if (Interval.IsInInterval(interval1, int2UBFloat))
                                {
                                    return false;
                                }
                                break;

                            default:
                                throw new Exception("SwitchBranchNotImplemented");
                        }
                    }
                    if (interval1.lowerBoundType == IntervalBoundType.Sharp)
                    {
                        switch (interval1.intervalType)
                        {
                            case IntervalType.Long:
                                if (int2UBLong != int1LBLong)
                                {
                                    if (Interval.IsInInterval(interval1, int2UBLong))
                                    {
                                        return false;
                                    }
                                }
                                else
                                {
                                    return false;
                                }
                                break;

                            case IntervalType.Float:
                                if (int2UBFloat != int1LBFloat)
                                {
                                    if (Interval.IsInInterval(interval1, int2UBFloat))
                                    {
                                        return false;
                                    }
                                }
                                else
                                {
                                    return false;
                                }
                                break;

                            default:
                                throw new Exception("SwitchBranchNotImplemented");
                        }
                    }
                }
                if (interval1.lowerBoundType == IntervalBoundType.Infinity)
                {
                    if (interval1.upperBoundType == IntervalBoundType.Round)
                    {
                        switch (interval1.intervalType)
                        {
                            case IntervalType.Long:
                                if (Interval.IsInInterval(interval1, int2LBLong))
                                {
                                    return false;
                                }
                                break;

                            case IntervalType.Float:
                                if (Interval.IsInInterval(interval1, int2LBFloat))
                                {
                                    return false;
                                }
                                break;

                            default:
                                throw new Exception("SwitchBranchNotImplemented");
                        }

                    }
                    if (interval1.upperBoundType == IntervalBoundType.Sharp)
                    {
                        switch (interval1.intervalType)
                        {
                            case IntervalType.Long:
                                if (int2LBLong != int1UBLong)
                                {
                                    if (Interval.IsInInterval(interval1, int2LBLong))
                                    {
                                        return false;
                                    }
                                }
                                else
                                {
                                    return false;
                                }
                                break;

                            case IntervalType.Float:
                                if (int2LBFloat != int1UBFloat)
                                {
                                    if (Interval.IsInInterval(interval1, int2LBFloat))
                                    {
                                        return false;
                                    }
                                }
                                else
                                {
                                    return false;
                                }
                                break;

                            default:
                                throw new Exception("SwitchBranchNotImplemented");
                        }
                    }
                }


                switch (interval1.intervalType)
                {
                    case IntervalType.Long:
                        if ((int1LBLong > int2LBLong) && (int1LBLong < int2UBLong))
                        {
                            return false;
                        }
                        if ((int1UBLong > int2LBLong) && (int1UBLong < int2UBLong))
                        {
                            return false;
                        }

                        if ((int2LBLong > int1LBLong) && (int2LBLong < int1UBLong))
                        {
                            return false;
                        }

                        if ((int2UBLong > int1LBLong) && (int2UBLong < int1UBLong))
                        {
                            return false;
                        }
                        if ((int2LBLong == int1LBLong) && (int2UBLong == int1UBLong))
                        {
                            return false;
                        }
                        break;

                    case IntervalType.Float:
                        if ((int1LBFloat > int2LBFloat) && (int1LBFloat < int2UBFloat))
                        {
                            return false;
                        }
                        if ((int1UBFloat > int2LBFloat) && (int1UBFloat < int2UBFloat))
                        {
                            return false;
                        }

                        if ((int2LBFloat > int1LBFloat) && (int2LBFloat < int1UBFloat))
                        {
                            return false;
                        }

                        if ((int2UBFloat > int1LBFloat) && (int2UBFloat < int1UBFloat))
                        {
                            return false;
                        }
                        if ((int2LBFloat == int1LBFloat) && (int2UBFloat == int1UBFloat))
                        {
                            return false;
                        }
                        break;

                    default:
                        throw new Exception("SwitchBranchNotImplemented");
                }
            }
            return true;
        }



        #endregion


        #region Composing strings

        /// <summary>
        /// Method to display the interval as a readable string
        /// </summary>
        /// <returns>String representation of the interval</returns>
        public override string ToString()
        {
            string returnValue = "";
            bool lowerInf = false;
            switch (this.lowerBoundType)
            {
                case IntervalBoundType.Round:
                    returnValue = returnValue + " ( ";
                    break;
                case IntervalBoundType.Sharp:
                    returnValue = returnValue + " < ";
                    break;
                case IntervalBoundType.Infinity:
                    returnValue = returnValue + " ( -inf ";
                    lowerInf = true;
                    break;
                default:
                    return "Internal error";
            }
            if (!lowerInf)
            {
                switch (this.intervalType)
                {
                    case IntervalType.Long:
                        returnValue = returnValue + this.lowerBound + ",";
                        break;
                    case IntervalType.Float:
                        returnValue = returnValue + this.lowerBoundFl + ",";
                        break;
                    default:
                        throw new Exception("Switch branch not implemented");
                }
            }
            else
            {
                returnValue = returnValue + ",";
            }
            switch (this.upperBoundType)
            {
                case IntervalBoundType.Round:
                    switch (this.intervalType)
                    {
                        case IntervalType.Long:
                            returnValue = returnValue + this.upperBound + " )";
                            break;
                        case IntervalType.Float:
                            returnValue = returnValue + this.upperBoundFl + " )";
                            break;
                        default:
                            throw new Exception("Switch branch not implemented");
                    }
                    break;
                case IntervalBoundType.Sharp:
                    switch (this.intervalType)
                    {
                        case IntervalType.Long:
                            returnValue = returnValue + this.upperBound + " >";
                            break;
                        case IntervalType.Float:
                            returnValue = returnValue + this.upperBoundFl + " >";
                            break;
                        default:
                            throw new Exception("Switch branch not implemented");
                    }
                    break;
                case IntervalBoundType.Infinity:
                    returnValue = returnValue + " +inf )";
                    break;
                default:
                    throw new Exception("Switch branch not implemented");
            }
            return returnValue;
        }

        #endregion
    }
}