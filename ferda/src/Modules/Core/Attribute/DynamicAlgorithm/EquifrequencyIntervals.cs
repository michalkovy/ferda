// EquifrequencyIntervals.cs - algorithm for creating equifrequency intervals
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

#define PERFCOUNTERS
#define SCOREGRAPH

// evolved from Tomas Karban`s equifrequency algorithm

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Ferda.Guha.Attribute.DynamicAlgorithm
{
    /// <summary>
    /// Represents pair [value, frequency], where value can be
    /// any object and frequency is (positive) frequency of the value
    /// in some data.
    /// </summary>
    /// <typeparam name="T">Any type.</typeparam>
    public class ValueFrequencyPair<T>
    {
        private readonly T _value;

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <value>The value.</value>
        public T Value
        {
            get { return _value; }
        }

        private readonly int _frequency;

        /// <summary>
        /// Gets the frequency.
        /// </summary>
        /// <value>The frequency.</value>
        public int Frequency
        {
            get { return _frequency; }
        }

        /// <summary>
        /// Initializes new instance of [value,frequency] pair.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="frequency">The frequency of the value.</param>
        public ValueFrequencyPair(T value, int frequency)
        {
            _value = value;

            Debug.Assert(frequency > 0);
            _frequency = frequency;
        }
    }

    /// <summary>
    /// This class contains static method that computes equifrequency requestedNumberOfIntervals
    /// on the sorted array of values from database.
    /// </summary>
    /// <remarks>
    /// <para>It is not possible to create instances of this class.</para>
    /// </remarks>
    public static class EquifrequencyIntervals
    {
        // supported types (T) ... all
        private const float initPenalty = Single.MaxValue;
        private const float stopLimit = 5.0f;
        private const int growLimit = 3;

#if PERFCOUNTERS
        private static int recursionCount;
        private static int cacheHit;
        private static int cacheMiss;
#endif

        /// <summary>
        /// Creates equifrequency requestedNumberOfIntervals from the sorted array of values from database.
        /// </summary>
        /// <param name="requestedNumberOfIntervals">Requested number of requestedNumberOfIntervals.</param>
        /// <param name="data">The array of pairs [value, frequency]. It must be already sorted though!</param>
        /// <returns>
        /// An array of dividing points. The first point returned is the right
        /// bound of the first value (the left bound can be -INF), the last point
        /// returned is the left bound of the last interval (right bound can be INF).
        /// </returns>
        /// <remarks>
        /// This method is an optimization algorithm. It heuristically checks many different
        /// combinations of dividing points and computes scoring for every such division.
        /// The scoring function is in fact a least square method and the optimization is searching
        /// for point of minimum error. See more details in other types of documentation.
        /// </remarks>
        public static T[] GenerateIntervals<T>(int requestedNumberOfIntervals, ValueFrequencyPair<T>[] data)
        {
            // assert that the requested number of requestedNumberOfIntervals is less than or equal to the total number of different values
            if (data.Length < requestedNumberOfIntervals)
                throw new ArgumentOutOfRangeException("requestedNumberOfIntervals", requestedNumberOfIntervals,
                                                      "The requested number of intervals is greater than the total number of different values.");

            // initialize the cache
            resultCache cache = new resultCache(requestedNumberOfIntervals);

#if PERFCOUNTERS
            recursionCount = 0; // count of findSplit(...) calls
            cacheHit = 0;
            cacheMiss = 0;
#endif

            // start the recursion
            resultOption result = findSplit<T>(
                data,
                new interval(0, data.Length), // at the beginning of recursion there is one interval 
                requestedNumberOfIntervals,
                (float) data.Length/(float) requestedNumberOfIntervals,
                cache
                );

#if PERFCOUNTERS
            Trace.WriteLine("Equifrequency requestedNumberOfIntervals performance:");
            int totalCount = 0;
            foreach (ValueFrequencyPair<T> pair in data)
            {
                totalCount += pair.Frequency;
            }
            Trace.WriteLine(String.Format("total {0}", totalCount));
            Trace.WriteLine(String.Format("different values {0}", data.Length));
            Trace.WriteLine(String.Format("requestedNumberOfIntervals {0}", requestedNumberOfIntervals));
            Trace.WriteLine(String.Format("recursion {0}", recursionCount));
            Trace.WriteLine(String.Format("cache hits {0}", cacheHit));
            Trace.WriteLine(String.Format("cache misses {0}", cacheMiss));
            Trace.WriteLine(String.Format("result error {0}", result.Cost));
            Trace.WriteLine(String.Empty);
#endif

            // get the split points
            T[] resultObjects = new T[requestedNumberOfIntervals - 1];
            for (int i = 0; i < requestedNumberOfIntervals - 1; i++)
            {
                resultObjects[i] = data[result.Intervals[i].Right].Value;
            }
            return resultObjects;
        }

        private static resultOption findSplit<T>(ValueFrequencyPair<T>[] data, interval bounds,
                                                 int requestedNumberOfIntervals, float optimum, resultCache cache)
        {
#if PERFCOUNTERS
            recursionCount++;
#endif

#if SCOREGRAPH
            bool topLevel = (bounds.Left == 0) && (bounds.Right == data.Length);
#endif

            // check if there is enough split points
            Debug.Assert(bounds.Right - bounds.Left >= requestedNumberOfIntervals);

            // test the end of recursion (no splitting)
            if (requestedNumberOfIntervals == 1)
            {
                resultOption result = new resultOption();
                result.Intervals.Add(bounds);
                int count = 0;
                for (int i = bounds.Left; i < bounds.Right; i++)
                    count += data[i].Frequency;
                result.Cost = resultPenalty(count, optimum);
                return result;
            }

            // test the end of recursion (exact splitting, no choice)
            if (requestedNumberOfIntervals == bounds.Right - bounds.Left)
            {
                resultOption result = new resultOption();
                result.Cost = 0.0f;
                for (int i = bounds.Left; i < bounds.Right; i++)
                {
                    result.Intervals.Add(new interval(i, i + 1));
                    result.Cost += resultPenalty(data[i].Frequency, optimum);
                }
                return result;
            }

            // cache lookup
            {
                resultOption result = cache.GetResult(requestedNumberOfIntervals, bounds);
                if (result != null)
                    return result;
            }

            // count objects that must be in the left part
            int leftIntervals = requestedNumberOfIntervals/2;
            int leftSum = 0;
            for (int i = 0; i < leftIntervals; i++)
                leftSum += data[bounds.Left + i].Frequency;

            // add some more requestedNumberOfIntervals until optimal point is reached
            int bestSplit;
            int leftOptimalSum = (int) System.Math.Round(optimum*leftIntervals);
            for (bestSplit = bounds.Left + leftIntervals;
                 bestSplit < bounds.Right - (requestedNumberOfIntervals - leftIntervals);
                 bestSplit++)
            {
                if (leftSum + data[bestSplit].Frequency > leftOptimalSum)
                    break;
                leftSum += data[bestSplit].Frequency;
            }

            // start testing these split points (spreading to left and right)
            int leftSplit = bestSplit;
            int rightSplit = bestSplit + 1;
            bool leftStop = false; // there's always at least one solution
            bool rightStop = (rightSplit > bounds.Right - (requestedNumberOfIntervals - leftIntervals));
                // go right only if there is another possible split point

            // spread to both sides and search for better solutions
            resultOption bestResult = new resultOption(), leftTmpResult, rightTmpResult;
            bestResult.Cost = initPenalty;
            float leftLastScore = initPenalty, rightLastScore = initPenalty;
            int leftGrowCount = 0, rightGrowCount = 0;
            while (!leftStop || !rightStop)
            {
                if (!leftStop)
                {
                    // find solution for left and right part
                    leftTmpResult =
                        findSplit<T>(data, new interval(bounds.Left, leftSplit), leftIntervals, optimum, cache);
                    rightTmpResult =
                        findSplit<T>(data, new interval(leftSplit, bounds.Right),
                                     requestedNumberOfIntervals - leftIntervals, optimum, cache);

                    // sum the costs of partial results
                    float sum = leftTmpResult.Cost + rightTmpResult.Cost;

#if SCOREGRAPH
                    if (topLevel)
                    {
                        // top level in the recursion
                        Trace.WriteLine(String.Format("{0};{1}", leftSplit, sum));
                    }
#endif

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

#if SCOREGRAPH
                    if (!topLevel)
                    {
#endif
                        // check stop criterium (result penalty is too big)
                        if (sum > stopLimit*bestResult.Cost)
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
#if SCOREGRAPH
                    }
#endif

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
                    leftTmpResult =
                        findSplit<T>(data, new interval(bounds.Left, rightSplit), leftIntervals, optimum, cache);
                    rightTmpResult =
                        findSplit<T>(data, new interval(rightSplit, bounds.Right),
                                     requestedNumberOfIntervals - leftIntervals, optimum, cache);

                    // sum the costs of partial results
                    float sum = leftTmpResult.Cost + rightTmpResult.Cost;

#if SCOREGRAPH
                    if (topLevel)
                    {
                        // top level in the recursion
                        Trace.WriteLine(String.Format("{0};{1}", rightSplit, sum));
                    }
#endif

                    // compare this result to what we have so far
                    if (sum < bestResult.Cost)
                    {
                        // merge two partial solution to one
                        bestResult.Merge(leftTmpResult, rightTmpResult);
                    }

#if SCOREGRAPH
                    if (!topLevel)
                    {
#endif
                        // check stop criterium (result penalty is too big)
                        if (sum > stopLimit*bestResult.Cost)
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
#if SCOREGRAPH
                    }
#endif

                    // check if there is possibility to spread further to the right
                    if (rightSplit >= bounds.Right - (requestedNumberOfIntervals - leftIntervals))
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
            cache.SetResult(requestedNumberOfIntervals, bounds, bestResult);

            // ...and return it
            return bestResult;
        }

        /// <summary>
        /// Gets result`s penalty.
        /// </summary>
        /// <param name="count">The count.</param>
        /// <param name="optimum">The optimum.</param>
        /// <returns>The penalty of result.</returns>
        private static float resultPenalty(float count, float optimum)
        {
            float penalty;
            if (count > optimum)
                penalty = count/optimum - 1.0f;
            else
                penalty = optimum/count - 1.0f;
            return penalty*penalty;
        }

        /// <summary>
        /// (in context of the Equifrequency algorithm) Holds (left, right) 
        /// indexes (to algorithm input [value, frequency] array) of values 
        /// which actually forms interval (in algorithm output meaning).
        /// </summary>
        private class interval
        {
            /// <summary>
            /// Constructs a new interval
            /// </summary>
            /// <param name="left">Left bound of an interval</param>
            /// <param name="right">Right bound of an interval</param>
            public interval(int left, int right)
            {
                Left = left;
                Right = right;
            }

            private int _left;

            /// <summary>
            /// Index (in data array) of ValueFrequencyPair of left value of the interval.
            /// </summary>
            /// <value>The left.</value>
            public int Left
            {
                get { return _left; }
                set
                {
                    Debug.Assert(value >= 0);
                    _left = value;
                    if (_right < _left)
                        _right = _left;
                }
            }

            private int _right;

            /// <summary>
            /// Index (in data array) of ValueFrequencyPair of right value of the interval.
            /// </summary>
            /// <value>The right.</value>
            public int Right
            {
                get { return _right; }
                set
                {
                    Debug.Assert(value >= 0);
                    _right = value;
                    if (_left > _right)
                        _left = _right;
                }
            }

            public override int GetHashCode()
            {
                return (Left << 16) ^ Right;
            }

            public override bool Equals(object obj)
            {
                interval that = obj as interval;
                if ((object) that == null)
                    return false;

                return ReferenceEquals(this, that) || (Left == that.Left) && (Right == that.Right);
            }

            public static bool Equals(interval a, interval b)
            {
                return Object.Equals(a, b);
            }

            public static bool operator ==(interval a, interval b)
            {
                if (ReferenceEquals(a, b))
                    return true;
                else if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
                    return false;
                else
                return Object.Equals(a, b);
            }

            public static bool operator !=(interval a, interval b)
            {
                return !(a == b);
            }
        }

        /// <summary>
        /// One result option (i.e. how could a equifrequency intervals
        /// split be like).
        /// </summary>
        private class resultOption
        {
            private List<interval> _intervals = new List<interval>();

            public List<interval> Intervals
            {
                get { return _intervals; }
            }

            private float _cost;

            public float Cost
            {
                get { return _cost; }
                set
                {
                    Debug.Assert(value >= 0.0f);
                    _cost = value;
                }
            }

            public void Merge(resultOption one, resultOption two)
            {
                Intervals.Clear();
                Intervals.AddRange(one.Intervals);
                Intervals.AddRange(two.Intervals);
                Cost = one.Cost + two.Cost;
            }
        }

        /// <summary>
        /// Cache of results of equifrequency intervals creation algorithm
        /// </summary>
        private class resultCache
        {
            public resultCache(int intervals)
            {
                // we will store results up to the size of ceil(requestedNumberOfIntervals / 2),
                // where requestedNumberOfIntervals is the requested output number of equifrequency requestedNumberOfIntervals
                int size = (intervals + 1)/2;

                // initialize cache lines (an array of hashtables)
                cacheLines = new Dictionary<interval, resultOption>[size - 1];
            }

            private Dictionary<interval, resultOption>[] cacheLines;

            public resultOption GetResult(int intervals, interval bounds)
            {
                Debug.Assert(intervals >= 2);

                // get the index of the cache line
                int index = intervals - 2;

                // check the number of requestedNumberOfIntervals
                if (index >= cacheLines.Length)
                {
                    // cache pseudo-miss... such sizes are not stored in cache
                    return null;
                }

                // get the cache line
                Dictionary<interval, resultOption> cacheLine = cacheLines[index];
                if (cacheLine == null) // cache miss
                {
#if PERFCOUNTERS
                    cacheMiss++;
#endif
                    return null;
                }

                resultOption result;
                // do the cache lookup
                if (cacheLine.TryGetValue(bounds, out result))
                {
#if PERFCOUNTERS
                    cacheHit++;
#endif
                    return result;
                }
                else
                {
#if PERFCOUNTERS
                    cacheMiss++;
#endif
                    return null;
                }
            }


            public void SetResult(int intervals, interval bounds, resultOption result)
            {
                Debug.Assert(intervals >= 2);

                // get the index of the cache line
                int index = intervals - 2;

                // ignore storing of results that do not fit to cache
                if (index >= cacheLines.Length)
                    return;

                if (cacheLines[index] == null)
                {
                    cacheLines[index] = new Dictionary<interval, resultOption>();
                }
                else
                {
                    // check that the result is not in the cache
                    Debug.Assert(!cacheLines[index].ContainsKey(bounds), "result cache already contains the result.");
                }

                cacheLines[index][bounds] = result;
            }
        }
    }
}