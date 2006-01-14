// #define PERFCOUNTERS
// #define SCOREGRAPH

//appear from Tomas Karban`s algorithm

using System;
using System.Collections;
using System.Diagnostics;

namespace Ferda.Modules.Boxes.DataMiningCommon.Attributes.EquifrequencyIntervalsAttribute
{
    /// <summary>
    /// This class contains static method that computes equifrequency intervals
    /// on the sorted array of values from database.
    /// </summary>
    /// <remarks>
    /// <para>It is not possible to create instances of this class.</para>
    /// </remarks>
    public sealed class EquifrequencyIntervalGenerator
    {
        private EquifrequencyIntervalGenerator()
        {
            // no need to create instances of this class...
        }


#if PERFCOUNTERS
        private static int recursionCount;
        private static int cacheHit;
        private static int cacheMiss;
#endif 


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
                    Debug.Assert(((IComparable) currentValue).CompareTo((IComparable) sortedValues[i]) <= 0);
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

#if PERFCOUNTERS
            recursionCount = 0;
            cacheHit = 0;
            cacheMiss = 0;
#endif

            // start the recursion
            Result result = FindSplit(dataArray, new Interval(0, dataArray.Count), intervals, (float) totalCount / (float) intervals, cache);

#if PERFCOUNTERS
            Trace.WriteLine("Equifrequency intervals performance:");
            Trace.WriteLine(String.Format("total {0}", totalCount));
            Trace.WriteLine(String.Format("different values {0}", dataArray.Count));
            Trace.WriteLine(String.Format("intervals {0}", intervals));
            Trace.WriteLine(String.Format("recursion {0}", recursionCount));
            Trace.WriteLine(String.Format("cache hits {0}", cacheHit));
            Trace.WriteLine(String.Format("cache misses {0}", cacheMiss));
            Trace.WriteLine(String.Format("result error {0}", result.Cost));
            Trace.WriteLine(String.Empty);
#endif
            
            // get the split points
            object[] resultObjects = new object[intervals - 1];
            for (i = 0; i < intervals - 1; i++)
            {
                resultObjects[i] = ((Data) dataArray[((Interval) result.Intervals[i]).Right]).Value;
            }
            return resultObjects;
        }

		public static object[] GenerateIntervals(int intervals, ArrayList dataArray)
		{
			// assert that the number of intervals is less than or equal to the total number of different values
			if (dataArray.Count < intervals)
				throw new ArgumentOutOfRangeException("intervals", intervals, "The requested number of intervals is less than the total number of different values.");

			// initialize the cache
			ResultCache cache = new ResultCache(intervals);

			// start the recursion
			Result result = FindSplit(dataArray, new Interval(0, dataArray.Count), intervals, (float)dataArray.Count / (float)intervals, cache);

			// get the split points
			object[] resultObjects = new object[intervals - 1];
			for (int i = 0; i < intervals - 1; i++)
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
#if PERFCOUNTERS
            recursionCount++;
#endif

#if SCOREGRAPH
            bool topLevel = (bounds.Left == 0) && (bounds.Right == dataArray.Count);
#endif

            // check if there is enough split points
            Debug.Assert(bounds.Right - bounds.Left >= intervals);

            // test the end of recursion (no splitting)
            if (intervals == 1)
            {
                Result result = new Result();
                result.Intervals.Add(bounds);
                int count = 0;
                for (int i = bounds.Left; i < bounds.Right; i++)
                    count += ((Data) dataArray[i]).Count;
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
                    result.Cost += ResultPenalty(((Data) dataArray[i]).Count, optimum);
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
                leftSum += ((Data) dataArray[bounds.Left + i]).Count;

            // add some more intervals until optimal point is reached
            int bestSplit;
            int leftOptimalSum = (int) Math.Round(optimum * leftIntervals);
            for (bestSplit = bounds.Left + leftIntervals; bestSplit < bounds.Right - (intervals - leftIntervals); bestSplit++)
            {
                if (leftSum + ((Data) dataArray[bestSplit]).Count > leftOptimalSum)
                    break;
                leftSum += ((Data) dataArray[bestSplit]).Count;
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
                    leftTmpResult = FindSplit(dataArray, new Interval(bounds.Left, rightSplit), leftIntervals, optimum, cache);
                    rightTmpResult = FindSplit(dataArray, new Interval(rightSplit, bounds.Right), intervals - leftIntervals, optimum, cache);

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

#if SCOREGRAPH
                    }
#endif
           
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
            if ((float) count > optimum)
                penalty = ((float) count) / optimum - 1.0f;
            else
                penalty = optimum / ((float) count) - 1.0f;
            return penalty * penalty;
        }


        public class Data
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
                if ((object) that == null)
                    return false;

                return Object.ReferenceEquals(this, that) || (this.Left == that.Left) && (this.Right == that.Right);
            }


            public static bool Equals(Interval a, Interval b)
            {
                return Object.Equals(a, b);
            }


            public static bool operator ==(Interval a, Interval b)
            {
                return Object.Equals(a, b);
            }


            public static bool operator !=(Interval a, Interval b)
            {
                return !Object.Equals(a, b);
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
                Result result = (Result) cacheLine[bounds];
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

    }
}
