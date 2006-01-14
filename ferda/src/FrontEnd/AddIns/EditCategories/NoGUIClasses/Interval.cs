using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace Ferda
{
    namespace FrontEnd.AddIns.EditCategories
    {
        public enum IntervalBoundType
        {
            Sharp, Round, Infinity
        };

        public enum IntervalType
        {
            Long,Float,DateTime, None
        };

        /// <summary>
        /// Class implementing an interval and basic functions for working with it
        /// </summary>
        public class Interval : IComparable
        {

            #region Private variables

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
            /// Lower bound of the interval
            /// </summary>
            private int lbound;
            public int lowerBound
            {
                get
                {
                    return lbound;
                }

                set
                {
                    lbound = value;
                }
            }

            /// <summary>
            /// Upper bound of the interval
            /// </summary>

            private int ubound;
            public int upperBound
            {
                get
                {
                    return ubound;
                }

                set
                {
                    ubound = value;
                }
            }


            /// <summary>
            /// Lower bound of the interval
            /// </summary>

            private float lboundf;
            public float lowerBoundFl
            {
                get
                {
                    return lboundf;
                }

                set
                {
                    lboundf = value;
                }
            }

            /// <summary>
            /// Upper bound of the interval
            /// </summary>
            private float uboundf;
            public float upperBoundFl
            {
                get
                {
                    return uboundf;
                }

                set
                {
                    uboundf = value;
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
                    return lboundtype;
                }

                set
                {
                    lboundtype = value;
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
                    return uboundtype;
                }

                set
                {
                    uboundtype = value;
                }
            }

            #endregion

            /// <summary>
            /// Constructor which sets upper and lower bound
            /// </summary>
            /// <param name="lowerBound">Lower bound</param>
            /// <param name="upperBound">Upper bound</param>
            public Interval(int lowerBound, int upperBound, IntervalBoundType lowerBoundType, IntervalBoundType upperBoundType)
            {
                this.lowerBound = lowerBound;
                this.upperBound = upperBound;
                this.upperBoundType = upperBoundType;
                this.lowerBoundType = lowerBoundType;
                this.intervalType = IntervalType.Long;
            }

            public Interval(float lowerBound, float upperBound, IntervalBoundType lowerBoundType, IntervalBoundType upperBoundType)
            {
                this.lowerBoundFl = lowerBound;
                this.upperBoundFl = upperBound;
                this.upperBoundType = upperBoundType;
                this.lowerBoundType = lowerBoundType;
                this.intervalType = IntervalType.Float;
            }


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
                throw new ArgumentException("Object is not an Interval");
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
                //
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
        }
    }
}