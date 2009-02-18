// And.cs
//
//  Copyright (C) 2009 Martin Ralbovský <martin.ralbovsky@gmail.com>
//
// This program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA

using System;
using Mono.Simd;

namespace Ferda.Benchmark
{
    /// <summary>
    /// The benchmark for the conjunction operation in bit strings. It does not
    /// use the actual methods implemented in the FerdaMiningProcessor, because
    /// the idea is to test different algorithms and their modifications and then
    /// to use the best one.
    /// </summary>
	public class And : FerdaBenchmark
    {

        #region Init, Reset and Check

        /// <summary>
        /// Initializes the bit strings
        /// </summary>
        /// <param name="args">Arguments from the command line</param>
        public static void Init(string[] args)
        {
            CommonInit(args);
        }

        #endregion

        #region Benchmark

        /// <summary>
        /// The crisp and benchmark - safe
        /// </summary>
        [Benchmark]
        public static void SafeAndCrisp()
        {
            //don't use static variables in iterations
            ulong[] tmp = stringUlong;
            ulong[] tmp2 = stringUlong2;
            int count = iterations;
            for (int i = 0; i < count; i++)
            {
                AndSafeCrisp(tmp, tmp2);
            }
        }

        #endregion

        #region Real implementation methods

        /// <summary>
        /// The safe (managed) implementation of the crisp conjunction
        /// </summary>
        /// <param name="operand1">1. operand</param>
        /// <param name="operand2">2. operand</param>
        /// <returns>Conjunction result</returns>
        static ulong[] AndSafeCrisp(ulong[] operand1, ulong[] operand2)
        {
            ulong[] result = new ulong[operand1.Length];
            for (int i = 0; i < operand1.Length; i++)
            {
                result[i] = operand1[i] & operand2[i];
            }
            return result;
        }

        /// <summary>
        /// The unsafe (unmanaged) implementation of the crisp conjunction
        /// </summary>
        /// <param name="operand1">1. operand</param>
        /// <param name="operand2">2. operand</param>
        /// <returns>Conjunction result</returns>
        static unsafe ulong[] AndUnsafeCrisp(ulong[] operand1, ulong[] operand2)
        {
            ulong[] result = new ulong[operand1.Length];
            //fixed (
            return result;
        }

        #endregion
    }
}
