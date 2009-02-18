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

        /// <summary>
        /// The crisp and benchmark - unsafe
        /// </summary>
        [Benchmark]
        public static void UnsafeAndCrisp()
        {
            //don't use static variables in iterations
            ulong[] tmp = stringUlong;
            ulong[] tmp2 = stringUlong2;
            int count = iterations;
            for (int i = 0; i < count; i++)
            {
                AndUnsafeCrisp(tmp, tmp2);
            }
        }

        /// <summary>
        /// The Vector4f and benchmark - safe
        /// </summary>
        [Benchmark]
        public static void SafeAndFuzzyVector4f()
        {
            //don't use static variables in iterations
            Vector4f[] tmp = stringVector4f;
            Vector4f[] tmp2 = stringVector4f2;
            int count = iterations;
            for (int i = 0; i < count; i++)
            {
                AndSafe4f(tmp, tmp2);
            }
        }

        /// <summary>
        /// The Vector4f and benchmark - unsafe
        /// </summary>
        [Benchmark]
        public static void UnsafeAndFuzzyVector4f()
        {
            //don't use static variables in iterations
            Vector4f[] tmp = stringVector4f;
            Vector4f[] tmp2 = stringVector4f2;
            int count = iterations;
            for (int i = 0; i < count; i++)
            {
                AndUnsafe4f(tmp, tmp2);
            }
        }

        /// <summary>
        /// The float fuzzy and benchmark - safe
        /// </summary>
        [Benchmark]
        public static void SafeAndFuzzyFloat()
        {
            //don't use static variables in iterations
            float[] tmp = stringFloat;
            float[] tmp2 = stringFloat2;
            int count = iterations;
            for (int i = 0; i < count; i++)
            {
                AndSafeFloat(tmp, tmp2);
            }
        }

        /// <summary>
        /// The float fuzzy and benchmark - unsafe
        /// </summary>
        [Benchmark]
        public static void UnsafeAndFuzzyFloat()
        {
            //don't use static variables in iterations
            float[] tmp = stringFloat;
            float[] tmp2 = stringFloat2;
            int count = iterations;
            for (int i = 0; i < count; i++)
            {
                AndUnsafeFloat(tmp, tmp2);
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
            for (int i = 0; i < operand1.Length; i++)
            {
                operand1[i] &= operand2[i];
            }
            return operand1;
        }

        /// <summary>
        /// The unsafe (unmanaged) implementation of the crisp conjunction
        /// </summary>
        /// <param name="operand1">1. operand</param>
        /// <param name="operand2">2. operand</param>
        /// <returns>Conjunction result</returns>
        static unsafe ulong[] AndUnsafeCrisp(ulong[] operand1, ulong[] operand2)
        {
            fixed (ulong* destPin = operand1, sourcePin = operand2)
            {
                ulong* destPtr = destPin, 
                    sourcePtr = sourcePin, 
                    stopPtr = destPin + operand1.Length;

                while (destPtr < stopPtr)
                {
                    *destPtr++ &= *sourcePtr++;
                }
            }

            return operand1;
        }

        /// <summary>
        /// The safe (managed) implementation of the conjunction with Vector4f
        /// </summary>
        /// <param name="operand1">1. operand</param>
        /// <param name="operand2">2. operand</param>
        /// <returns>Conjunction result</returns>
        static Vector4f[] AndSafe4f(Vector4f[] operand1, Vector4f[] operand2)
        {
            for (int i = 0; i < operand1.Length; i++)
            {
                operand1[i] *= operand2[i];
            }
            return operand1;
        }

        /// <summary>
        /// The unsafe (unmanaged) implementation of the conjunction with Vector4f
        /// </summary>
        /// <param name="operand1">1. operand</param>
        /// <param name="operand2">2. operand</param>
        /// <returns>Conjunction result</returns>
        static unsafe Vector4f[] AndUnsafe4f(Vector4f[] operand1, Vector4f[] operand2)
        {
            fixed (Vector4f* thisPin = operand1, sourcePin = operand2)
            {
                Vector4f* currentPtr = thisPin, sourcePtr = sourcePin,
                    stopPtr = thisPin + operand1.Length;
                while (currentPtr < stopPtr)
                {
                    *currentPtr++ *= *sourcePtr++;
                }
            }
            return operand1;
        }

        /// <summary>
        /// The safe (managed) implementation of the conjunction with array of floats
        /// </summary>
        /// <param name="operand1">1. operand</param>
        /// <param name="operand2">2. operand</param>
        /// <returns>Conjunction result</returns>
        static float[] AndSafeFloat(float[] operand1, float[] operand2)
        {
            for (int i = 0; i < operand1.Length; i++)
            {
                operand1[i] *= operand2[i];
            }
            return operand1;
        }

        /// <summary>
        /// The unsafe (unmanaged) implementation of the conjunction with array
        /// of floats
        /// </summary>
        /// <param name="operand1">1. operand</param>
        /// <param name="operand2">2. operand</param>
        /// <returns>Conjunction result</returns>
        static unsafe float[] AndUnsafeFloat(float[] operand1, float[] operand2)
        {
            fixed (float* thisPin = operand1, sourcePin = operand2)
            {
                float* currentPtr = thisPin, sourcePtr = sourcePin,
                    stopPtr = thisPin + operand1.Length;
                while (currentPtr < stopPtr)
                {
                    *currentPtr++ *= *sourcePtr++;
                }
            }
            return operand1;
        }

        #endregion
    }
}
