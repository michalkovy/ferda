// ****************************************************
//        PSEUDOCODE -- cannot be compiled
//   see EquifrequencyIntervalGenerator.cs for the real code
// ****************************************************


using System;
using System.Collections;
using System.Diagnostics;


namespace RelMiner
{
   public sealed class EquifrequencyIntervalGenerator
   {

        
// global variables:
// cache – cache for partial results with methods
//             SetResult and GetResult
// dataArray - input array of couples {value;quantity}
//                   sorted by values

object FindSplit(int leftBound, int rightBound,
   int intervals)
{
   // test the end of recursion (no splitting)
   if (intervals == 1)
   {
      // one interval spanning the whole segment;
      // compute score…
      return …;
   }

   // test the end of recursion (exact splitting, no choice)
   if (intervals == rightBound - leftBound)
   {
      // series of intervals, where every interval
      // contains exactly one value from the input array;
      // compute score
      return …;
   }

   // cache lookup
   Result result = cache.GetResult(intervals,
                                             leftBound, rightBound);
   if (result != null)  return result;

   // count objects that must be in the left part
   int bestSplit, leftSum = 0, leftIntervals = intervals / 2;
   for (int i = 0; i < leftIntervals; i++)
      leftSum += dataArray[leftBound + i].quantity;

   // add more intervals until optimal point is reached
   for (bestSplit = leftBound + leftIntervals;
         bestSplit < rightBound - (intervals - leftIntervals);
         bestSplit++)
   {
      if (leftSum + dataArray[bestSplit].quantity >
                   opt * leftIntervals)  break;
      leftSum += dataArray[bestSplit].quantity;
   }

   // start testing split points (spreading to left and right)
   int leftSplit = bestSplit;
   int rightSplit = bestSplit + 1;
   bool leftStop = false;  // always at least one solution
   bool rightStop = (rightSplit >
                 rightBound - (intervals - leftIntervals));

   // spread to both sides and search for better solutions
   Result bestResult = new Result();
   bestResult.score = +INF;
   Result leftTmpResult, rightTmpResult;
   float sum, leftLastScore = +INF, rightLastScore = +INF;
   int leftGrowCount = 0, rightGrowCount = 0;

   while (!leftStop || !rightStop)
   {
      if (!leftStop)
      {
         // recursively find solution for left and right part
         leftTmpResult = FindSplit(leftBound, leftSplit,
               leftIntervals);
         rightTmpResult = FindSplit(leftSplit, rightBound,
               intervals - leftIntervals);

         // sum the scores of partial results
         sum = leftTmpResult.score + rightTmpResult.score;

         // the first solution is propagated to the right side
         if (rightLastScore == +INF)  rightLastScore = sum;

         // compare this result to what we have so far
         if (sum < bestResult.score)
         {
            // merge two partial solution to one
            bestResult.Merge(
                      leftTmpResult, rightTmpResult);
            // absolute stop criterium (perfect result)
            if (sum == 0) break;
         }

         // check stop criterium (stopLimit == 5)
         if (sum > stopLimit * bestResult.score)
         {
            // stop spreading to the left
            leftStop = true;
         }

         // check stop criterium (growLimit == 3)
         if (sum < leftLastScore)
         {
            // score is not growing, reset the counter
            leftGrowCount = 0;
         }
         else
         {
            // score is growing, increase the counter
            leftGrowCount++;
            if (leftGrowCount == growLimit)  leftStop = true;
         }
         leftLastScore = sum;

         // can we spread further to the left?
         if (leftSplit <= leftBound + leftIntervals)
         {
            // stop spreading to the left
            leftStop = true;
         }
         else
         {
            // shift the left split point to the next position
            leftSplit--;
         }
      }

      if (!rightStop)
      {
         // find solution for left and right part
         leftTmpResult = FindSplit(leftBound, rightSplit,
               leftIntervals);
         rightTmpResult = FindSplit(rightSplit, rightBound,
               intervals - leftIntervals);

         // …
         // the rest is symmetric to the left part…      
         // …
      }
   }

   // add the best result to cache
   cache.SetResult(intervals,
                      leftBound, rightBound, bestResult);

   // …and return it
   return bestResult;
}



   }
}
