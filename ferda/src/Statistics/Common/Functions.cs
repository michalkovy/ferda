using System;
using System.Collections.Generic;
using System.Text;

namespace Ferda.Statistics.Common
{
    internal class Functions
    {
        /// <summary>
        /// Returns minimum from 2-dimension table
        /// </summary>
        /// <param name="table">2-dimension array of integers</param>
        /// <returns>Minimal value</returns>
        public static int Min(int [] [] table)
        {
            int returnValue = table[0][0];
            foreach (int[] values in table)
            {
                foreach (int value in values)
                {
                    returnValue = value < returnValue ? returnValue = value : returnValue;
                }
            }

            return returnValue;
        }

        /// <summary>
        /// Returns maximum from 2-dimension table
        /// </summary>
        /// <param name="table">2-dimension array of integers</param>
        /// <returns>Maximal value</returns>
        public static int Max(int[][] table)
        {
            int returnValue = table[0][0];
            foreach (int[] values in table)
            {
                foreach (int value in values)
                {
                    returnValue = value > returnValue ? returnValue = value : returnValue;
                }
            }

            return returnValue;
        }

        /// <summary>
        /// Returns sum of 2-dimension table
        /// </summary>
        /// <param name="table">2-dimension array of integers</param>
        /// <returns>Sum</returns>
        public static int Sum(int[][] table)
        {
            int returnValue = 0;

            foreach (int[] values in table)
            {
                foreach (int value in values)
                {
                    returnValue += value;
                }
            }

            return returnValue;
        }
    }
}
