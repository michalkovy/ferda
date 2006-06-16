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
            while (countOfGeneratedSeparators < requestedNumberOfIntervals)
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
            while (countOfGeneratedSeparators < requestedNumberOfIntervals)
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
            while (countOfGeneratedSeparators < requestedNumberOfIntervals)
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
            while (countOfGeneratedSeparators < requestedNumberOfIntervals)
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