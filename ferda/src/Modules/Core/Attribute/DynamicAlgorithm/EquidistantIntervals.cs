// EquidistantIntervals.cs - the algorithm for creating equidistant intervals attribute
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

namespace Ferda.Guha.Attribute.DynamicAlgorithm
{
    /// <summary>
    /// Provides method for generation of equidistant intervals.
    /// </summary>
    /// <remarks>
    /// Supported datatypes are integeral (long) and float (double) 
    /// numbers, time (TimeSpan), date and date time (DateTime). Booleans are not 
    /// supported (there is no reason why it should be) even strings.
    /// </remarks>
    public static class EquidistantIntervals
    {
        /// <summary>
        /// Generates the equidistant intervals as in LISp-Miner - based on intervals length
        /// </summary>
        /// <param name="from">From</param>
        /// <param name="to">To</param>
        /// <param name="length">LEngth of the generated interval</param>
        /// <returns>Split points for the generated intervals</returns>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        /// Thown if <c>length</c> is equal to zero or 
        /// too high i.e. to - from is smaller than length.</exception>
        public static long[] GenerateIntervalsLISp(long from, long to, int length)
        {
            if (length == 0)
                throw new ArgumentOutOfRangeException("length",
                    "Requested length of intervals can not be zero.");

            if((to - from) < length)
                throw new ArgumentOutOfRangeException("length",
                    "Requested length of interval is larger than the maximum possible value.");

            long actualValue = from;
            long[] intervalsArray = 
                new long[Convert.ToInt32(System.Math.Ceiling((double)(to - from) / length)) - 1];

            int index = 0;
            while (actualValue < to - length)
            {
                intervalsArray[index] = actualValue + length;
                actualValue += length;
                index++;
            }
            return intervalsArray;
        }

        /// <summary>
        /// Generates the equidistant intervals as in LISp-Miner - based on intervals length
        /// </summary>
        /// <param name="from">From</param>
        /// <param name="to">To</param>
        /// <param name="length">LEngth of the generated interval</param>
        /// <returns>Split points for the generated intervals</returns>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        /// Thown if <c>length</c> is equal to zero or 
        /// too high i.e. to - from is smaller than length.</exception>
        public static double[] GenerateIntervalsLISp(double from, double to, int length)
        {
            if (length == 0)
                throw new ArgumentOutOfRangeException("length",
                    "Requested length of intervals can not be zero.");

            if ((to - from) < length)
                throw new ArgumentOutOfRangeException("length",
                    "Requested length of interval is larger than the maximum possible value.");

            double actualValue = from;
            double[] intervalsArray =
                new double[Convert.ToInt32(System.Math.Ceiling((double)(to - from) / length)) - 1];

            int index = 0;
            while (actualValue < to - length)
            {
                intervalsArray[index] = actualValue + length;
                actualValue += length;
                index++;
            }
            return intervalsArray;
        }


        /// <summary>
        /// Generates the intervals.
        /// </summary>
        /// <param name="from">From value.</param>
        /// <param name="to">To value.</param>
        /// <param name="requestedNumberOfIntervals">The requested number of intervals.</param>
        /// <returns>Split point for equidistant intervals.</returns>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        /// Thown if <c>requestedNumberOfIntervals</c> is equal to zero or 
        /// too high i.e. there is not enought values between <c>from</c>
        /// and <c>to</c> value.</exception>
        public static long[] GenerateIntervals(long from, long to, int requestedNumberOfIntervals)
        {
            if (requestedNumberOfIntervals == 0)
                throw new ArgumentOutOfRangeException("requestedNumberOfIntervals",
                                                      "Requested number of intervals can not be zero.");
            if (requestedNumberOfIntervals == 1)
                return new long[0] {};

            long[] result = new long[requestedNumberOfIntervals - 1];

            double stepSize = (double) (to - from)/(double) requestedNumberOfIntervals;
            if (stepSize < 1d)
                throw new ArgumentOutOfRangeException("requestedNumberOfIntervals", requestedNumberOfIntervals,
                                                      "The requested number of intervals is greater than the total number of points to make separation between interval.");
            double separator = from + stepSize;
            result[0] = (long) separator;
            int countOfGeneratedSeparators = 1;
            while (countOfGeneratedSeparators < requestedNumberOfIntervals - 1)
            {
                separator += stepSize;
                result[countOfGeneratedSeparators] = (long) separator;
                countOfGeneratedSeparators++;
            }
            if ((long) separator > to)
                throw new Exception();

            return result;
        }

        /// <summary>
        /// Generates the intervals.
        /// </summary>
        /// <param name="from">From value.</param>
        /// <param name="to">To value.</param>
        /// <param name="requestedNumberOfIntervals">The requested number of intervals.</param>
        /// <returns>Split point for equidistant intervals.</returns>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        /// Thown if <c>requestedNumberOfIntervals</c> is equal to zero or 
        /// too high i.e. there is not enought values between <c>from</c>
        /// and <c>to</c> value.</exception>
        public static double[] GenerateIntervals(double from, double to, int requestedNumberOfIntervals)
        {
            if (requestedNumberOfIntervals == 0)
                throw new ArgumentOutOfRangeException("requestedNumberOfIntervals",
                                                      "Requested number of intervals can not be zero.");
            if (requestedNumberOfIntervals == 1)
                return new double[0] {};

            double[] result = new double[requestedNumberOfIntervals - 1];

            double stepSize = (to - from)/requestedNumberOfIntervals;
            if (stepSize <= 0d)
                throw new ArgumentOutOfRangeException("requestedNumberOfIntervals", requestedNumberOfIntervals,
                                                      "The requested number of intervals is greater than the total number of points to make separation between interval.");
            double separator = from + stepSize;
            result[0] = separator;
            int countOfGeneratedSeparators = 1;
            while (countOfGeneratedSeparators < requestedNumberOfIntervals - 1)
            {
                separator += stepSize;
                result[countOfGeneratedSeparators] = separator;
                countOfGeneratedSeparators++;
            }
            if (separator > to)
                throw new Exception();

            return result;
        }

        /// <summary>
        /// Generates the intervals.
        /// </summary>
        /// <param name="from">From value.</param>
        /// <param name="to">To value.</param>
        /// <param name="requestedNumberOfIntervals">The requested number of intervals.</param>
        /// <param name="onlyDate">if set to <c>true</c> only split points with precision to date can be generated.</param>
        /// <returns>Split point for equidistant intervals.</returns>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        /// Thown if <c>requestedNumberOfIntervals</c> is equal to zero or 
        /// too high i.e. there is not enought values between <c>from</c>
        /// and <c>to</c> value.</exception>
        public static DateTime[] GenerateIntervals(DateTime from, DateTime to, int requestedNumberOfIntervals,
                                                   bool onlyDate)
        {
            if (requestedNumberOfIntervals == 0)
                throw new ArgumentOutOfRangeException("requestedNumberOfIntervals",
                                                      "Requested number of intervals can not be zero.");
            if (requestedNumberOfIntervals == 1)
                return new DateTime[0] {};

            DateTime[] result = new DateTime[requestedNumberOfIntervals - 1];

            double stepSize = (double) (to.Ticks - from.Ticks)/(double) requestedNumberOfIntervals;
            if ((int) stepSize < 1)
                throw new ArgumentOutOfRangeException("requestedNumberOfIntervals", requestedNumberOfIntervals,
                                                      "The requested number of intervals is greater than the total number of points to make separation between interval.");
            double addingStepSize = stepSize;
            DateTime previous = new DateTime(from.Ticks + (long) addingStepSize);
            DateTime current;
            result[0] = previous;
            int countOfGeneratedSeparators = 1;
            while (countOfGeneratedSeparators < requestedNumberOfIntervals - 1)
            {
                addingStepSize += stepSize;
                current = new DateTime(from.Ticks + (long) addingStepSize);
                if (onlyDate)
                {
                    if (previous.Year == current.Year
                        && previous.Month == current.Month
                        && previous.Day == current.Day)
                        throw new ArgumentOutOfRangeException("requestedNumberOfIntervals", requestedNumberOfIntervals,
                                                              "The requested number of intervals is greater than the total number of points to make separation between interval.");
                }
                result[countOfGeneratedSeparators] = current;
                previous = current;
                countOfGeneratedSeparators++;
            }
            if (previous.Ticks > to.Ticks)
                throw new Exception();

            return result;
        }

        /// <summary>
        /// Generates the intervals.
        /// </summary>
        /// <param name="from">From value.</param>
        /// <param name="to">To value.</param>
        /// <param name="requestedNumberOfIntervals">The requested number of intervals.</param>
        /// <returns>Split point for equidistant intervals.</returns>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        /// Thown if <c>requestedNumberOfIntervals</c> is equal to zero or
        /// too high i.e. there is not enought values between <c>from</c>
        /// and <c>to</c> value.</exception>
        public static TimeSpan[] GenerateIntervals(TimeSpan from, TimeSpan to, int requestedNumberOfIntervals)
        {
            if (requestedNumberOfIntervals == 0)
                throw new ArgumentOutOfRangeException("requestedNumberOfIntervals",
                                                      "Requested number of intervals can not be zero.");
            if (requestedNumberOfIntervals == 1)
                return new TimeSpan[0] {};

            TimeSpan[] result = new TimeSpan[requestedNumberOfIntervals - 1];

            double stepSize = (double) (to.Ticks - from.Ticks)/(double) requestedNumberOfIntervals;
            if ((int) stepSize < 1)
                throw new ArgumentOutOfRangeException("requestedNumberOfIntervals", requestedNumberOfIntervals,
                                                      "The requested number of intervals is greater than the total number of points to make separation between interval.");
            double addingStepSize = stepSize;
            TimeSpan previous = new TimeSpan(from.Ticks + (long) addingStepSize);
            TimeSpan current;
            result[0] = previous;
            int countOfGeneratedSeparators = 1;
            while (countOfGeneratedSeparators < requestedNumberOfIntervals - 1)
            {
                addingStepSize += stepSize;
                current = new TimeSpan(from.Ticks + (long) addingStepSize);
                result[countOfGeneratedSeparators] = current;
                previous = current;
                countOfGeneratedSeparators++;
            }
            if (previous.Ticks > to.Ticks)
                throw new Exception();

            return result;
        }
    }
}