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
using System.Numerics;

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
        #region Fields

        #endregion

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
                ulong[] tmp1 = GC.AllocateUninitializedArray<ulong>(tmp.Length);
                tmp.CopyTo((Span<ulong>)tmp1);
                AndSafeCrisp(tmp1, tmp2);
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
                ulong[] tmp1 = GC.AllocateUninitializedArray<ulong>(tmp.Length);
                tmp.CopyTo((Span<ulong>)tmp1);
                AndUnsafeCrisp(tmp1, tmp2);
            }
        }

        [Benchmark]
        public static void UnsafeAndNewCrisp()
        {
            //don't use static variables in iterations
            ulong[] tmp = stringUlong;
            ulong[] tmp2 = stringUlong2;
            int count = iterations;
            for (int i = 0; i < count; i++)
            {
                ulong[] res = GC.AllocateUninitializedArray<ulong>(tmp.Length);
                AndUnsafeNewCrisp(tmp, tmp2, res);
            }
        }

        [Benchmark]
        public static void CrispVector()
        {
            var tmp = stringVectorUlong1;
            var tmp2 = stringVectorUlong2;
            int count = iterations;
            for (int i = 0; i < count; i++)
            {
                Vector<ulong>[] tmp1 = GC.AllocateUninitializedArray<Vector<ulong>>(tmp.Length);
                tmp.CopyTo((Span<Vector<ulong>>)tmp1);
                AndCrispVector(tmp1, tmp2);
            }

        }

        [Benchmark]
        public static void CrispVectorNew()
        {
            var tmp = stringVectorUlong1;
            var tmp2 = stringVectorUlong2;
            int count = iterations;
            for (int i = 0; i < count; i++)
            {
                Vector<ulong>[] res = GC.AllocateUninitializedArray<Vector<ulong>>(tmp.Length);
                AndCrispNewVector(tmp, tmp2, res);
            }

        }

        /// <summary>
        /// The Vector4 and benchmark - safe
        /// </summary>
        [Benchmark]
        public static void SafeAndFuzzyVector4()
        {
            //don't use static variables in iterations
            Vector4[] tmp = stringVector4;
            Vector4[] tmp2 = stringVector42;
            int count = iterations;
            for (int i = 0; i < count; i++)
            {
                Vector4[] tmp1 = GC.AllocateUninitializedArray<Vector4>(tmp.Length);
                tmp.CopyTo((Span<Vector4>)tmp1);
                AndSafe4f(tmp1, tmp2);
            }
        }

        /// <summary>
        /// The Vector4 and benchmark - unsafe
        /// </summary>
        [Benchmark]
        public static void UnsafeAndFuzzyVector4()
        {
            //don't use static variables in iterations
            Vector4[] tmp = stringVector4;
            Vector4[] tmp2 = stringVector42;
            int count = iterations;
            for (int i = 0; i < count; i++)
            {
                Vector4[] tmp1 = GC.AllocateUninitializedArray<Vector4>(tmp.Length);
                tmp.CopyTo((Span<Vector4>)tmp1);
                AndUnsafe4f(tmp1, tmp2);
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
                float[] tmp1 = GC.AllocateUninitializedArray<float>(tmp.Length);
                tmp.CopyTo((Span<float>)tmp1);
                AndSafeFloat(tmp1, tmp2);
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
                float[] tmp1 = GC.AllocateUninitializedArray<float>(tmp.Length);
                tmp.CopyTo((Span<float>)tmp1);
                AndUnsafeFloat(tmp1, tmp2);
            }
        }

        /// <summary>
        /// The and benchmark for fuzzy and crips bit strings - using array of floats
        /// </summary>
        [Benchmark]
        public static void SafeAndCrispFuzzyFloat()
        {
            //don't use static variables in iterations
            float[] tmp = stringFloat;
            ulong[] tmp2 = stringUlong;
            int count = iterations;
            for (int i = 0; i < count; i++)
            {
                float[] tmp1 = GC.AllocateUninitializedArray<float>(tmp.Length);
                tmp.CopyTo((Span<float>)tmp1);
                AndSafeCrispFuzzyFloat(tmp1, tmp2);
            }
        }

        /// <summary>
        /// The and benchmark for fuzzy and crips bit strings - using array 
        /// of floats, less access to the float array
        /// </summary>
        [Benchmark]
        public static void SafeAndCrispFuzzyFloat2()
        {
            //don't use static variables in iterations
            float[] tmp = stringFloat;
            ulong[] tmp2 = stringUlong;
            int count = iterations;
            for (int i = 0; i < count; i++)
            {
                float[] tmp1 = GC.AllocateUninitializedArray<float>(tmp.Length);
                tmp.CopyTo((Span<float>)tmp1);
                AndSafeCrispFuzzyFloat2(tmp1, tmp2);
            }
        }

        /// <summary>
        /// The Vector4 and benchmark for fuzzy and crisp bit strings - safe, 
        /// naive version
        /// </summary>
        [Benchmark]
        public static void SafeAndCrispFuzzyVector4Naive()
        {
            //don't use static variables in iterations
            Vector4[] tmp = stringVector4;
            ulong[] tmp2 = stringUlong;
            int count = iterations;
            for (int i = 0; i < count; i++)
            {
                Vector4[] tmp1 = GC.AllocateUninitializedArray<Vector4>(tmp.Length);
                tmp.CopyTo((Span<Vector4>)tmp1);
                AndSafeCrispFuzzyVector41(tmp1, tmp2);
            }
        }

        ///// <summary>
        ///// The Vector4 and benchmark for fuzzy and crisp bit strings - safe,
        ///// precomputed version
        ///// </summary>
        //[Benchmark]
        //public static void SafeAndCrispFuzzyVector4Precomputed()
        //{
        //    //don't use static variables in iterations
        //    Vector4[] tmp = stringVector4;
        //    ulong[] tmp2 = stringUlong;
        //    int count = iterations;
        //    for (int i = 0; i < count; i++)
        //    {
        //        AndSafeCrispFuzzyVector42(tmp, tmp2);
        //    }
        //}

        ///// <summary>
        ///// The Vector4 and benchmark for fuzzy and crisp bit strings - safe,
        ///// precomputed version with shifts in creation of Vector4
        ///// </summary>
        //[Benchmark]
        //public static void SafeAndCrispFuzzyVector4PrecomputedShift()
        //{
        //    //don't use static variables in iterations
        //    Vector4[] tmp = stringVector4;
        //    ulong[] tmp2 = stringUlong;
        //    int count = iterations;
        //    for (int i = 0; i < count; i++)
        //    {
        //        AndSafeCrispFuzzyVector43(tmp, tmp2);
        //    }
        //}

        ///// <summary>
        ///// The Vector4 and benchmark for fuzzy and crisp bit strings - safe,
        ///// precomputed version with shifts in creation of Vector4 without
        ///// static variables
        ///// </summary>
        //[Benchmark]
        //public static void SafeAndCrispFuzzyVector4PrecomputedShiftNoStatic()
        //{
        //    //don't use static variables in iterations
        //    Vector4[] tmp = stringVector4;
        //    ulong[] tmp2 = stringUlong;
        //    int count = iterations;
        //    for (int i = 0; i < count; i++)
        //    {
        //        AndSafeCrispFuzzyVector43NoStatic(tmp, tmp2);
        //    }
        //}

        ///// <summary>
        ///// The Vector4 and benchmark for fuzzy and crisp bit strings - safe,
        ///// precomputed version with shifts in creation of Vector4 without
        ///// alocations of new Vector4's
        ///// </summary>
        //[Benchmark]
        //public static void SafeAndCrispFuzzyVector4PrecomputedShiftNoAllocation()
        //{
        //    //don't use static variables in iterations
        //    Vector4[] tmp = stringVector4;
        //    ulong[] tmp2 = stringUlong;
        //    int count = iterations;
        //    for (int i = 0; i < count; i++)
        //    {
        //        AndSafeCrispFuzzyVector43NoAllocation(tmp, tmp2);
        //    }
        //}

        ///// <summary>
        ///// The Vector4 and benchmark for fuzzy and crisp bit strings - unsafe,
        ///// precomputed version
        ///// </summary>
        //[Benchmark]
        //public static void UnsafeAndCrispFuzzyVector4Precomputed()
        //{
        //    Vector4[] tmp = stringVector4;
        //    ulong[] tmp2 = stringUlong;
        //    int count = iterations;
        //    for (int i = 0; i < count; i++)
        //    {
        //        AndUnsafeCrispFuzzyVector42(tmp, tmp2);
        //    }
        //}

        ///// <summary>
        ///// The Vector4 and benchmark for fuzzy and crisp bit strings - unsafe,
        ///// precomputed version without allocation of new memory in the cycles
        ///// </summary>
        //[Benchmark]
        //public static void UnsafeAndCrispFuzzyVector4PrecomputedNoAllocation()
        //{
        //    Vector4[] tmp = stringVector4;
        //    ulong[] tmp2 = stringUlong;
        //    int count = iterations;
        //    for (int i = 0; i < count; i++)
        //    {
        //        AndUnsafeCrispFuzzyVector42NoAllocation(tmp, tmp2);
        //    }
        //}

        ///// <summary>
        ///// The Vector4 and benchmark for fuzzy and crisp bit strings - unsafe,
        ///// precomputed version without static variables, without allocation of new memory
        ///// in the cycles
        ///// </summary>
        //[Benchmark]
        //public static void UnsafeAndCrispFuzzyVector4PrecomputedNoStaticNoAllocation()
        //{
        //    Vector4[] tmp = stringVector4;
        //    ulong[] tmp2 = stringUlong;
        //    int count = iterations;
        //    for (int i = 0; i < count; i++)
        //    {
        //        AndUnsafeCrispFuzzyVector42NoAllocationNoStatic(tmp, tmp2);
        //    }
        //}

        ///// <summary>
        ///// The Vector4 and benchmark for fuzzy and crisp bit strings - unsafe,
        ///// precomputed version with shifts in creation of Vector4
        ///// </summary>
        //[Benchmark]
        //public static void UnsafeAndCrispFuzzyVector4PrecomputedShift()
        //{
        //    Vector4[] tmp = stringVector4;
        //    ulong[] tmp2 = stringUlong;
        //    int count = iterations;
        //    for (int i = 0; i < count; i++)
        //    {
        //        AndUnsafeCrispFuzzyVector43(tmp, tmp2);
        //    }
        //}

        ///// <summary>
        ///// The Vector4 and benchmark for fuzzy and crisp bit strings - unsafe,
        ///// precomputed version with shifts in creation of Vector4, 
        ///// no static variables
        ///// </summary>
        //[Benchmark]
        //public static void UnsafeAndCrispFuzzyVector4PrecomputedShiftNoStatic()
        //{
        //    Vector4[] tmp = stringVector4;
        //    ulong[] tmp2 = stringUlong;
        //    int count = iterations;
        //    for (int i = 0; i < count; i++)
        //    {
        //        AndUnsafeCrispFuzzyVector43NoStatic(tmp, tmp2);
        //    }
        //}

        ///// <summary>
        ///// The Vector4 and benchmark for fuzzy and crisp bit strings - unsafe,
        ///// precomputed version with shifts in creation of Vector4, 
        ///// no allocation of new memory in cycles
        ///// </summary>
        //[Benchmark]
        //public static void UnsafeAndCrispFuzzyVector4PrecomputedShiftNoAllocation()
        //{
        //    Vector4[] tmp = stringVector4;
        //    ulong[] tmp2 = stringUlong;
        //    int count = iterations;
        //    for (int i = 0; i < count; i++)
        //    {
        //        AndUnsafeCrispFuzzyVector43NoAllocation(tmp, tmp2);
        //    }
        //}

        ///// <summary>
        ///// The Vector4 and benchmark for fuzzy and crisp bit strings - unsafe,
        ///// precomputed version with shifts in creation of Vector4, 
        ///// no allocation of new memory in cycles + no static variables
        ///// </summary>
        //[Benchmark]
        //public static void UnsafeAndCrispFuzzyVector4PrecomputedShiftNoStaticNoAllocation()
        //{
        //    Vector4[] tmp = stringVector4;
        //    ulong[] tmp2 = stringUlong;
        //    int count = iterations;
        //    for (int i = 0; i < count; i++)
        //    {
        //        AndUnsafeCrispFuzzyVector43NoAllocationNoStatic(tmp, tmp2);
        //    }
        //}

        #endregion

        #region Real implementation methods

        /// <summary>
        /// The safe implementation of the crisp conjunction
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
        /// The unsafe implementation of the crisp conjunction
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

        static unsafe ulong[] AndUnsafeNewCrisp(ulong[] operand1, ulong[] operand2, ulong[] result)
        {
            fixed (ulong* operand1Pin = operand1, operand2Pin = operand2, resultPin = result)
            {
                ulong* resultPtr = resultPin,
                    operand1Ptr = operand1Pin,
                    operand2Ptr = operand2Pin,
                    stopPtr = resultPtr + operand1.Length;

                while (resultPtr < stopPtr)
                {
                    *resultPtr++ = *operand1Ptr++ & *operand2Ptr++;
                }
            }

            return result;
        }

        static Vector<ulong>[] AndCrispVector(Vector<ulong>[] operand1, Vector<ulong>[] operand2)
        {
            for (int i = 0; i < operand1.Length; i++)
            {
                operand1[i] &= operand2[i];
            }
            return operand1;
        }

        static Vector<ulong>[] AndCrispNewVector(Vector<ulong>[] operand1, Vector<ulong>[] operand2, Vector<ulong>[] result)
        {
            for (int i = 0; i < operand1.Length; i++)
            {
                result[i] = operand1[i] & operand2[i];
            }
            return result;
        }

        /// <summary>
        /// The safe (managed) implementation of the conjunction with Vector4
        /// </summary>
        /// <param name="operand1">1. operand</param>
        /// <param name="operand2">2. operand</param>
        /// <returns>Conjunction result</returns>
        static Vector4[] AndSafe4f(Vector4[] operand1, Vector4[] operand2)
        {
            for (int i = 0; i < operand1.Length; i++)
            {
                operand1[i] *= operand2[i];
            }
            return operand1;
        }

        /// <summary>
        /// The unsafe implementation of the conjunction with Vector4
        /// </summary>
        /// <param name="operand1">1. operand</param>
        /// <param name="operand2">2. operand</param>
        /// <returns>Conjunction result</returns>
        static unsafe Vector4[] AndUnsafe4f(Vector4[] operand1, Vector4[] operand2)
        {
            fixed (Vector4* thisPin = operand1, sourcePin = operand2)
            {
                Vector4* currentPtr = thisPin, sourcePtr = sourcePin,
                    stopPtr = thisPin + operand1.Length;
                while (currentPtr < stopPtr)
                {
                    *currentPtr++ *= *sourcePtr++;
                }
            }
            return operand1;
        }

        /// <summary>
        /// The safe implementation of the conjunction with array of floats
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
        /// The unsafe implementation of the conjunction with array
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

        /// <summary>
        /// The safe (managed) implementation of conjunction between fuzzy and crisp 
        /// bit string using Vector4. Algorithm 1 - naive
        /// </summary>
        /// <param name="operand1">1. operand (fuzzy)</param>
        /// <param name="operand2">2. operand (crisp)</param>
        /// <returns>Conjunction result (fuzzy)</returns>
        static Vector4[] AndSafeCrispFuzzyVector41(Vector4[] operand1, ulong[] operand2)
        {
            for (int i = 0; i < operand1.Length; i++)
            {
                Vector4 tmp = new Vector4(
                    Convert.ToSingle((operand2[4 * i / 64] & (_one << (4 * i % 64))) > 0),
                    Convert.ToSingle((operand2[(4 * i + 1) / 64] & (_one << ((4 * i + 1) % 64))) > 0),
                    Convert.ToSingle((operand2[(4 * i + 2) / 64] & (_one << ((4 * i + 2) % 64))) > 0),
                    Convert.ToSingle((operand2[(4 * i + 3) / 64] & (_one << ((4 * i + 3) % 64))) > 0));
                operand1[i] *= tmp;
            }
            return operand1;
        }

        /// <summary>
        /// The safe implementation of conjunction between fuzzy and crisp 
        /// bit strings using array of floats
        /// </summary>
        /// <param name="operand1">1. operand (fuzzy)</param>
        /// <param name="operand2">2. operand (crisp)</param>
        /// <returns>Conjunction result (fuzzy)</returns>
        static float[] AndSafeCrispFuzzyFloat(float[] operand1, ulong[] operand2)
        {
            for (int i = 0; i < operand1.Length; i++)
            {
                operand1[i] *= Convert.ToSingle((operand2[i / 64] & (_one << (i % 64))) > 0);
            }
            return operand1;
        }

        /// <summary>
        /// The safe implementation of conjunction between fuzzy and crisp 
        /// bit strings using array of floats + less accessing the ulong field
        /// </summary>
        /// <param name="operand1">1. operand (fuzzy)</param>
        /// <param name="operand2">2. operand (crisp)</param>
        /// <returns>Conjunction result (fuzzy)</returns>
        static float[] AndSafeCrispFuzzyFloat2(float[] operand1, ulong[] operand2)
        {
            ulong tmp;
            int fltPtr = 0;
            for (int i = 0; i < operand2.Length; i++)
            {
                tmp = operand2[i];

                operand1[fltPtr] *= Convert.ToSingle(tmp);
                fltPtr++;
                operand1[fltPtr] *= Convert.ToSingle(tmp >> 1);
                fltPtr++;
                operand1[fltPtr] *= Convert.ToSingle(tmp >> 2);
                fltPtr++;
                operand1[fltPtr] *= Convert.ToSingle(tmp >> 3);
                fltPtr++;
                operand1[fltPtr] *= Convert.ToSingle(tmp >> 4);
                fltPtr++;
                operand1[fltPtr] *= Convert.ToSingle(tmp >> 5);
                fltPtr++;
                operand1[fltPtr] *= Convert.ToSingle(tmp >> 6);
                fltPtr++;
                operand1[fltPtr] *= Convert.ToSingle(tmp >> 7);
                fltPtr++;
                operand1[fltPtr] *= Convert.ToSingle(tmp >> 8);
                fltPtr++;
                operand1[fltPtr] *= Convert.ToSingle(tmp >> 9);
                fltPtr++;
                operand1[fltPtr] *= Convert.ToSingle(tmp >> 12);
                fltPtr++;
                operand1[fltPtr] *= Convert.ToSingle(tmp >> 13);
                fltPtr++;
                operand1[fltPtr] *= Convert.ToSingle(tmp >> 14);
                fltPtr++;
                operand1[fltPtr] *= Convert.ToSingle(tmp >> 15);
                fltPtr++;
                operand1[fltPtr] *= Convert.ToSingle(tmp >> 16);
                fltPtr++;
                operand1[fltPtr] *= Convert.ToSingle(tmp >> 17);
                fltPtr++;
                operand1[fltPtr] *= Convert.ToSingle(tmp >> 18);
                fltPtr++;
                operand1[fltPtr] *= Convert.ToSingle(tmp >> 19);
                fltPtr++;
                operand1[fltPtr] *= Convert.ToSingle(tmp >> 20);
                fltPtr++;
                operand1[fltPtr] *= Convert.ToSingle(tmp >> 21);
                fltPtr++;
                operand1[fltPtr] *= Convert.ToSingle(tmp >> 22);
                fltPtr++;
                operand1[fltPtr] *= Convert.ToSingle(tmp >> 23);
                fltPtr++;
                operand1[fltPtr] *= Convert.ToSingle(tmp >> 24);
                fltPtr++;
                operand1[fltPtr] *= Convert.ToSingle(tmp >> 25);
                fltPtr++;
                operand1[fltPtr] *= Convert.ToSingle(tmp >> 26);
                fltPtr++;
                operand1[fltPtr] *= Convert.ToSingle(tmp >> 27);
                fltPtr++;
                operand1[fltPtr] *= Convert.ToSingle(tmp >> 28);
                fltPtr++;
                operand1[fltPtr] *= Convert.ToSingle(tmp >> 29);
                fltPtr++;
                operand1[fltPtr] *= Convert.ToSingle(tmp >> 30);
                fltPtr++;
                operand1[fltPtr] *= Convert.ToSingle(tmp >> 31);
                fltPtr++;
                operand1[fltPtr] *= Convert.ToSingle(tmp >> 32);
                fltPtr++;
                operand1[fltPtr] *= Convert.ToSingle(tmp >> 33);
                fltPtr++;
                operand1[fltPtr] *= Convert.ToSingle(tmp >> 34);
                fltPtr++;
                operand1[fltPtr] *= Convert.ToSingle(tmp >> 35);
                fltPtr++;
                operand1[fltPtr] *= Convert.ToSingle(tmp >> 36);
                fltPtr++;
                operand1[fltPtr] *= Convert.ToSingle(tmp >> 37);
                fltPtr++;
                operand1[fltPtr] *= Convert.ToSingle(tmp >> 38);
                fltPtr++;
                operand1[fltPtr] *= Convert.ToSingle(tmp >> 39);
                fltPtr++;
                operand1[fltPtr] *= Convert.ToSingle(tmp >> 40);
                fltPtr++;
                operand1[fltPtr] *= Convert.ToSingle(tmp >> 41);
                fltPtr++;
                operand1[fltPtr] *= Convert.ToSingle(tmp >> 42);
                fltPtr++;
                operand1[fltPtr] *= Convert.ToSingle(tmp >> 43);
                fltPtr++;
                operand1[fltPtr] *= Convert.ToSingle(tmp >> 44);
                fltPtr++;
                operand1[fltPtr] *= Convert.ToSingle(tmp >> 45);
                fltPtr++;
                operand1[fltPtr] *= Convert.ToSingle(tmp >> 46);
                fltPtr++;
                operand1[fltPtr] *= Convert.ToSingle(tmp >> 47);
                fltPtr++;
                operand1[fltPtr] *= Convert.ToSingle(tmp >> 48);
                fltPtr++;
                operand1[fltPtr] *= Convert.ToSingle(tmp >> 49);
                fltPtr++;
                operand1[fltPtr] *= Convert.ToSingle(tmp >> 50);
                fltPtr++;
                operand1[fltPtr] *= Convert.ToSingle(tmp >> 51);
                fltPtr++;
                operand1[fltPtr] *= Convert.ToSingle(tmp >> 52);
                fltPtr++;
                operand1[fltPtr] *= Convert.ToSingle(tmp >> 53);
                fltPtr++;
                operand1[fltPtr] *= Convert.ToSingle(tmp >> 54);
                fltPtr++;
                operand1[fltPtr] *= Convert.ToSingle(tmp >> 55);
                fltPtr++;
                operand1[fltPtr] *= Convert.ToSingle(tmp >> 56);
                fltPtr++;
                operand1[fltPtr] *= Convert.ToSingle(tmp >> 57);
                fltPtr++;
                operand1[fltPtr] *= Convert.ToSingle(tmp >> 58);
                fltPtr++;
                operand1[fltPtr] *= Convert.ToSingle(tmp >> 59);
                fltPtr++;
                operand1[fltPtr] *= Convert.ToSingle(tmp >> 60);
                fltPtr++;
                operand1[fltPtr] *= Convert.ToSingle(tmp >> 61);
                fltPtr++;
                operand1[fltPtr] *= Convert.ToSingle(tmp >> 62);
                fltPtr++;
                operand1[fltPtr] *= Convert.ToSingle(tmp >> 63);
                fltPtr++;
            }

            return operand1;
        }

        ///// <summary>
        ///// The safe implementation of conjunction between fuzy and crisp
        ///// bit strings using Vector4 and precomputed uint and float vectors.
        ///// The algoritm presumes the number of elements both in ulong and Vector4
        ///// fields to be a multiple of 64.
        ///// </summary>
        ///// <param name="operand1">1. operand (fuzzy)</param>
        ///// <param name="operand2">2. operand (crisp)</param>
        ///// <returns>Conjunction result (fuzzy)</returns>
        //public static Vector4[] AndSafeCrispFuzzyVector42(Vector4[] operand1, ulong[] operand2)
        //{
        //    Vector4 tmpF;
        //    Vector<uint> tmpUi;
        //    Vector<uint> vect;
        //    uint part;
        //    int vPtr = 0;

        //    for (int i = 0; i < operand2.Length; i++)
        //    {
        //        part = (uint)operand2[i]; //last 8 bits
        //        vect = new Vector<uint>(part, part, part, part);

        //        tmpUi = vect & sui1;
        //        tmpF = new Vector4(tmpUi.X , tmpUi.Y , tmpUi.Z ,  tmpUi.W ) ;
        //        operand1[vPtr++] *= tmpF / sf1;
        //        tmpUi = vect & sui2;
        //        tmpF = new Vector4(tmpUi.X, tmpUi.Y, tmpUi.Z, tmpUi.W);
        //        operand1[vPtr++] *= tmpF / sf2;
        //        tmpUi = vect & sui3;
        //        tmpF = new Vector4(tmpUi.X, tmpUi.Y, tmpUi.Z, tmpUi.W);
        //        operand1[vPtr++] *= tmpF / sf3;
        //        tmpUi = vect & sui4;
        //        tmpF = new Vector4(tmpUi.X, tmpUi.Y, tmpUi.Z, tmpUi.W);
        //        operand1[vPtr++] *= tmpF / sf4;
        //        tmpUi = vect & sui5;
        //        tmpF = new Vector4(tmpUi.X, tmpUi.Y, tmpUi.Z, tmpUi.W);
        //        operand1[vPtr++] *= tmpF / sf5;
        //        tmpUi = vect & sui6;
        //        tmpF = new Vector4(tmpUi.X, tmpUi.Y, tmpUi.Z, tmpUi.W);
        //        operand1[vPtr++] *= tmpF / sf6;
        //        tmpUi = vect & sui7;
        //        tmpF = new Vector4(tmpUi.X, tmpUi.Y, tmpUi.Z, tmpUi.W);
        //        operand1[vPtr++] *= tmpF / sf7;
        //        tmpUi = vect & sui8;
        //        tmpF = new Vector4(tmpUi.X, tmpUi.Y, tmpUi.Z, tmpUi.W);
        //        operand1[vPtr++] *= tmpF / sf8;

        //        part = (uint)(operand2[i] >> 32);
        //        vect = new Vector<uint>(part, part, part, part);

        //        tmpUi = vect & sui1;
        //        tmpF = new Vector4(tmpUi.X, tmpUi.Y, tmpUi.Z, tmpUi.W);
        //        operand1[vPtr++] *= tmpF / sf1;
        //        tmpUi = vect & sui2;
        //        tmpF = new Vector4(tmpUi.X, tmpUi.Y, tmpUi.Z, tmpUi.W);
        //        operand1[vPtr++] *= tmpF / sf2;
        //        tmpUi = vect & sui3;
        //        tmpF = new Vector4(tmpUi.X, tmpUi.Y, tmpUi.Z, tmpUi.W);
        //        operand1[vPtr++] *= tmpF / sf3;
        //        tmpUi = vect & sui4;
        //        tmpF = new Vector4(tmpUi.X, tmpUi.Y, tmpUi.Z, tmpUi.W);
        //        operand1[vPtr++] *= tmpF / sf4;
        //        tmpUi = vect & sui5;
        //        tmpF = new Vector4(tmpUi.X, tmpUi.Y, tmpUi.Z, tmpUi.W);
        //        operand1[vPtr++] *= tmpF / sf5;
        //        tmpUi = vect & sui6;
        //        tmpF = new Vector4(tmpUi.X, tmpUi.Y, tmpUi.Z, tmpUi.W);
        //        operand1[vPtr++] *= tmpF / sf6;
        //        tmpUi = vect & sui7;
        //        tmpF = new Vector4(tmpUi.X, tmpUi.Y, tmpUi.Z, tmpUi.W);
        //        operand1[vPtr++] *= tmpF / sf7;
        //        tmpUi = vect & sui8;
        //        tmpF = new Vector4(tmpUi.X, tmpUi.Y, tmpUi.Z, tmpUi.W);
        //        operand1[vPtr++] *= tmpF / sf8;
        //    }
        //    return operand1;
        //}

        ///// <summary>
        ///// The safe implementation of conjunction between fuzy and crisp
        ///// bit strings using Vector4 and precomputed uint and shifting creation of the Vector4
        ///// The algoritm presumes the number of elements both in ulong and Vector4
        ///// fields to be a multiple of 64.
        ///// </summary>
        ///// <param name="operand1">1. operand (fuzzy)</param>
        ///// <param name="operand2">2. operand (crisp)</param>
        ///// <returns>Conjunction result (fuzzy)</returns>
        //public static Vector4[] AndSafeCrispFuzzyVector43(Vector4[] operand1, ulong[] operand2)
        //{
        //    Vector4 tmpF;
        //    Vector<uint> tmpUi;
        //    Vector<uint> vect;
        //    uint part;
        //    int vPtr = 0;

        //    for (int i = 0; i < operand2.Length; i++)
        //    {
        //        part = (uint)operand2[i]; //last 8 bits
        //        vect = new Vector<uint>(part, part, part, part);

        //        tmpUi = vect & sui1;
        //        tmpF = new Vector4(tmpUi.X, tmpUi.Y >> 1, tmpUi.Z >> 2, tmpUi.W >> 3);
        //        operand1[vPtr++] *= tmpF;
        //        tmpUi = vect & sui2;
        //        tmpF = new Vector4(tmpUi.X >> 4, tmpUi.Y >> 5, tmpUi.Z >> 6, tmpUi.W >> 7);
        //        operand1[vPtr++] *= tmpF;
        //        tmpUi = vect & sui3;
        //        tmpF = new Vector4(tmpUi.X >> 8, tmpUi.Y >> 9, tmpUi.Z >> 10, tmpUi.W >> 11);
        //        operand1[vPtr++] *= tmpF;
        //        tmpUi = vect & sui4;
        //        tmpF = new Vector4(tmpUi.X >> 12, tmpUi.Y >> 13, tmpUi.Z >> 14, tmpUi.W >> 15);
        //        operand1[vPtr++] *= tmpF;
        //        tmpUi = vect & sui5;
        //        tmpF = new Vector4(tmpUi.X >> 16, tmpUi.Y >> 17, tmpUi.Z >> 18, tmpUi.W >> 19);
        //        operand1[vPtr++] *= tmpF;
        //        tmpUi = vect & sui6;
        //        tmpF = new Vector4(tmpUi.X >> 20, tmpUi.Y >> 21, tmpUi.Z >> 22, tmpUi.W >> 23);
        //        operand1[vPtr++] *= tmpF;
        //        tmpUi = vect & sui7;
        //        tmpF = new Vector4(tmpUi.X >> 24, tmpUi.Y >> 25, tmpUi.Z >> 26, tmpUi.W >> 27);
        //        operand1[vPtr++] *= tmpF;
        //        tmpUi = vect & sui8;
        //        tmpF = new Vector4(tmpUi.X >> 28, tmpUi.Y >> 29, tmpUi.Z >> 30, tmpUi.W >> 31);
        //        operand1[vPtr++] *= tmpF;

        //        part = (uint)(operand2[i] >> 32);
        //        vect = new Vector<uint>(part, part, part, part);

        //        tmpUi = vect & sui1;
        //        tmpF = new Vector4(tmpUi.X, tmpUi.Y >> 1, tmpUi.Z >> 2, tmpUi.W >> 3);
        //        operand1[vPtr++] *= tmpF;
        //        tmpUi = vect & sui2;
        //        tmpF = new Vector4(tmpUi.X >> 4, tmpUi.Y >> 5, tmpUi.Z >> 6, tmpUi.W >> 7);
        //        operand1[vPtr++] *= tmpF;
        //        tmpUi = vect & sui3;
        //        tmpF = new Vector4(tmpUi.X >> 8, tmpUi.Y >> 9, tmpUi.Z >> 10, tmpUi.W >> 11);
        //        operand1[vPtr++] *= tmpF;
        //        tmpUi = vect & sui4;
        //        tmpF = new Vector4(tmpUi.X >> 12, tmpUi.Y >> 13, tmpUi.Z >> 14, tmpUi.W >> 15);
        //        operand1[vPtr++] *= tmpF;
        //        tmpUi = vect & sui5;
        //        tmpF = new Vector4(tmpUi.X >> 16, tmpUi.Y >> 17, tmpUi.Z >> 18, tmpUi.W >> 19);
        //        operand1[vPtr++] *= tmpF;
        //        tmpUi = vect & sui6;
        //        tmpF = new Vector4(tmpUi.X >> 20, tmpUi.Y >> 21, tmpUi.Z >> 22, tmpUi.W >> 23);
        //        operand1[vPtr++] *= tmpF;
        //        tmpUi = vect & sui7;
        //        tmpF = new Vector4(tmpUi.X >> 24, tmpUi.Y >> 25, tmpUi.Z >> 26, tmpUi.W >> 27);
        //        operand1[vPtr++] *= tmpF;
        //        tmpUi = vect & sui8;
        //        tmpF = new Vector4(tmpUi.X >> 28, tmpUi.Y >> 29, tmpUi.Z >> 30, tmpUi.W >> 31);
        //        operand1[vPtr++] *= tmpF;
        //    }
        //    return operand1;
        //}

        ///// <summary>
        ///// The safe implementation of conjunction between fuzy and crisp
        ///// bit strings using Vector4 and precomputed uint and shifting creation of the Vector4
        ///// The algoritm presumes the number of elements both in ulong and Vector4
        ///// fields to be a multiple of 64. The algorithm
        ///// doesn't use allocation of new memory in computation of individual Vector4's,
        ///// it assigns to only one Vector4 previously defined. 
        ///// </summary>
        ///// <param name="operand1">1. operand (fuzzy)</param>
        ///// <param name="operand2">2. operand (crisp)</param>
        ///// <returns>Conjunction result (fuzzy)</returns>
        //public static Vector4[] AndSafeCrispFuzzyVector43NoAllocation(Vector4[] operand1, ulong[] operand2)
        //{
        //    Vector4 tmpF = new Vector4();
        //    Vector<uint> tmpUi;
        //    Vector<uint> vect;
        //    uint part;
        //    int vPtr = 0;

        //    for (int i = 0; i < operand2.Length; i++)
        //    {
        //        part = (uint)operand2[i]; //last 8 bits
        //        vect = new Vector<uint>(part, part, part, part);

        //        tmpUi = vect & sui1;
        //        tmpF.X = tmpUi.X; tmpF.Y = (tmpUi.Y >> 1); tmpF.Z = (tmpUi.Z >> 2); tmpF.W = (tmpUi.W >> 3);
        //        operand1[vPtr++] *= tmpF;
        //        tmpUi = vect & sui2;
        //        tmpF.X = (tmpUi.X >> 4); tmpF.Y = (tmpUi.Y >> 5); tmpF.Z = (tmpUi.Z >> 6); tmpF.W = (tmpUi.W >> 7);
        //        operand1[vPtr++] *= tmpF;
        //        tmpUi = vect & sui3;
        //        tmpF.X = (tmpUi.X >> 8); tmpF.Y = (tmpUi.Y >> 9); tmpF.Z = (tmpUi.Z >> 10); tmpF.W = (tmpUi.W >> 11);
        //        operand1[vPtr++] *= tmpF;
        //        tmpUi = vect & sui4;
        //        tmpF.X = (tmpUi.X >> 12); tmpF.Y = (tmpUi.Y >> 13); tmpF.Z = (tmpUi.Z >> 14); tmpF.W = (tmpUi.W >> 15);
        //        operand1[vPtr++] *= tmpF;
        //        tmpUi = vect & sui5;
        //        tmpF.X = (tmpUi.X >> 16); tmpF.Y = (tmpUi.Y >> 17); tmpF.Z = (tmpUi.Z >> 18); tmpF.W = (tmpUi.W >> 19);
        //        operand1[vPtr++] *= tmpF;
        //        tmpUi = vect & sui6;
        //        tmpF.X = (tmpUi.X >> 20); tmpF.Y = (tmpUi.Y >> 21); tmpF.Z = (tmpUi.Z >> 22); tmpF.W = (tmpUi.W >> 23);
        //        operand1[vPtr++] *= tmpF;
        //        tmpUi = vect & sui7;
        //        tmpF.X = (tmpUi.X >> 24); tmpF.Y = (tmpUi.Y >> 25); tmpF.Z = (tmpUi.Z >> 26); tmpF.W = (tmpUi.W >> 27);
        //        operand1[vPtr++] *= tmpF;
        //        tmpUi = vect & sui8;
        //        tmpF.X = (tmpUi.X >> 28); tmpF.Y = (tmpUi.Y >> 29); tmpF.Z = (tmpUi.Z >> 30); tmpF.W = (tmpUi.W >> 31);
        //        operand1[vPtr++] *= tmpF;

        //        part = (uint)(operand2[i] >> 32);
        //        vect = new Vector<uint>(part, part, part, part);

        //        tmpUi = vect & sui1;
        //        tmpF.X = tmpUi.X; tmpF.Y = (tmpUi.Y >> 1); tmpF.Z = (tmpUi.Z >> 2); tmpF.W = (tmpUi.W >> 3);
        //        operand1[vPtr++] *= tmpF;
        //        tmpUi = vect & sui2;
        //        tmpF.X = (tmpUi.X >> 4); tmpF.Y = (tmpUi.Y >> 5); tmpF.Z = (tmpUi.Z >> 6); tmpF.W = (tmpUi.W >> 7);
        //        operand1[vPtr++] *= tmpF;
        //        tmpUi = vect & sui3;
        //        tmpF.X = (tmpUi.X >> 8); tmpF.Y = (tmpUi.Y >> 9); tmpF.Z = (tmpUi.Z >> 10); tmpF.W = (tmpUi.W >> 11);
        //        operand1[vPtr++] *= tmpF;
        //        tmpUi = vect & sui4;
        //        tmpF.X = (tmpUi.X >> 12); tmpF.Y = (tmpUi.Y >> 13); tmpF.Z = (tmpUi.Z >> 14); tmpF.W = (tmpUi.W >> 15);
        //        operand1[vPtr++] *= tmpF;
        //        tmpUi = vect & sui5;
        //        tmpF.X = (tmpUi.X >> 16); tmpF.Y = (tmpUi.Y >> 17); tmpF.Z = (tmpUi.Z >> 18); tmpF.W = (tmpUi.W >> 19);
        //        operand1[vPtr++] *= tmpF;
        //        tmpUi = vect & sui6;
        //        tmpF.X = (tmpUi.X >> 20); tmpF.Y = (tmpUi.Y >> 21); tmpF.Z = (tmpUi.Z >> 22); tmpF.W = (tmpUi.W >> 23);
        //        operand1[vPtr++] *= tmpF;
        //        tmpUi = vect & sui7;
        //        tmpF.X = (tmpUi.X >> 24); tmpF.Y = (tmpUi.Y >> 25); tmpF.Z = (tmpUi.Z >> 26); tmpF.W = (tmpUi.W >> 27);
        //        operand1[vPtr++] *= tmpF;
        //        tmpUi = vect & sui8;
        //        tmpF.X = (tmpUi.X >> 28); tmpF.Y = (tmpUi.Y >> 29); tmpF.Z = (tmpUi.Z >> 30); tmpF.W = (tmpUi.W >> 31);
        //        operand1[vPtr++] *= tmpF;
        //    }
        //    return operand1;
        //}

        ///// <summary>
        ///// The safe implementation of conjunction between fuzy and crisp
        ///// bit strings using Vector4 and precomputed uint and shifting creation of the Vector4
        ///// The algoritm presumes the number of elements both in ulong and Vector4
        ///// fields to be a multiple of 64. The algorithm
        ///// doesn't use allocation of new memory in computation of individual Vector4's,
        ///// it assigns to only one Vector4 previously defined + does not use static variables
        ///// </summary>
        ///// <param name="operand1">1. operand (fuzzy)</param>
        ///// <param name="operand2">2. operand (crisp)</param>
        ///// <returns>Conjunction result (fuzzy)</returns>
        //public static Vector4[] AndSafeCrispFuzzyVector43NoStaticNoAllocation(Vector4[] operand1, ulong[] operand2)
        //{
        //    Vector4 tmpF = new Vector4();
        //    Vector<uint> tmpUi;
        //    Vector<uint> vect;
        //    uint part;
        //    int vPtr = 0;

        //    Vector<uint> ui1 = sui1;
        //    Vector<uint> ui2 = sui2;
        //    Vector<uint> ui3 = sui3;
        //    Vector<uint> ui4 = sui4;
        //    Vector<uint> ui5 = sui5;
        //    Vector<uint> ui6 = sui6;
        //    Vector<uint> ui7 = sui7;
        //    Vector<uint> ui8 = sui8;

        //    for (int i = 0; i < operand2.Length; i++)
        //    {
        //        part = (uint)operand2[i]; //last 8 bits
        //        vect = new Vector<uint>(part, part, part, part);

        //        tmpUi = vect & ui1;
        //        tmpF.X = tmpUi.X; tmpF.Y = (tmpUi.Y >> 1); tmpF.Z = (tmpUi.Z >> 2); tmpF.W = (tmpUi.W >> 3);
        //        operand1[vPtr++] *= tmpF;
        //        tmpUi = vect & ui2;
        //        tmpF.X = (tmpUi.X >> 4); tmpF.Y = (tmpUi.Y >> 5); tmpF.Z = (tmpUi.Z >> 6); tmpF.W = (tmpUi.W >> 7);
        //        operand1[vPtr++] *= tmpF;
        //        tmpUi = vect & ui3;
        //        tmpF.X = (tmpUi.X >> 8); tmpF.Y = (tmpUi.Y >> 9); tmpF.Z = (tmpUi.Z >> 10); tmpF.W = (tmpUi.W >> 11);
        //        operand1[vPtr++] *= tmpF;
        //        tmpUi = vect & ui4;
        //        tmpF.X = (tmpUi.X >> 12); tmpF.Y = (tmpUi.Y >> 13); tmpF.Z = (tmpUi.Z >> 14); tmpF.W = (tmpUi.W >> 15);
        //        operand1[vPtr++] *= tmpF;
        //        tmpUi = vect & ui5;
        //        tmpF.X = (tmpUi.X >> 16); tmpF.Y = (tmpUi.Y >> 17); tmpF.Z = (tmpUi.Z >> 18); tmpF.W = (tmpUi.W >> 19);
        //        operand1[vPtr++] *= tmpF;
        //        tmpUi = vect & ui6;
        //        tmpF.X = (tmpUi.X >> 20); tmpF.Y = (tmpUi.Y >> 21); tmpF.Z = (tmpUi.Z >> 22); tmpF.W = (tmpUi.W >> 23);
        //        operand1[vPtr++] *= tmpF;
        //        tmpUi = vect & ui7;
        //        tmpF.X = (tmpUi.X >> 24); tmpF.Y = (tmpUi.Y >> 25); tmpF.Z = (tmpUi.Z >> 26); tmpF.W = (tmpUi.W >> 27);
        //        operand1[vPtr++] *= tmpF;
        //        tmpUi = vect & ui8;
        //        tmpF.X = (tmpUi.X >> 28); tmpF.Y = (tmpUi.Y >> 29); tmpF.Z = (tmpUi.Z >> 30); tmpF.W = (tmpUi.W >> 31);
        //        operand1[vPtr++] *= tmpF;

        //        part = (uint)(operand2[i] >> 32);
        //        vect = new Vector<uint>(part, part, part, part);

        //        tmpUi = vect & ui1;
        //        tmpF.X = tmpUi.X; tmpF.Y = (tmpUi.Y >> 1); tmpF.Z = (tmpUi.Z >> 2); tmpF.W = (tmpUi.W >> 3);
        //        operand1[vPtr++] *= tmpF;
        //        tmpUi = vect & ui2;
        //        tmpF.X = (tmpUi.X >> 4); tmpF.Y = (tmpUi.Y >> 5); tmpF.Z = (tmpUi.Z >> 6); tmpF.W = (tmpUi.W >> 7);
        //        operand1[vPtr++] *= tmpF;
        //        tmpUi = vect & ui3;
        //        tmpF.X = (tmpUi.X >> 8); tmpF.Y = (tmpUi.Y >> 9); tmpF.Z = (tmpUi.Z >> 10); tmpF.W = (tmpUi.W >> 11);
        //        operand1[vPtr++] *= tmpF;
        //        tmpUi = vect & ui4;
        //        tmpF.X = (tmpUi.X >> 12); tmpF.Y = (tmpUi.Y >> 13); tmpF.Z = (tmpUi.Z >> 14); tmpF.W = (tmpUi.W >> 15);
        //        operand1[vPtr++] *= tmpF;
        //        tmpUi = vect & ui5;
        //        tmpF.X = (tmpUi.X >> 16); tmpF.Y = (tmpUi.Y >> 17); tmpF.Z = (tmpUi.Z >> 18); tmpF.W = (tmpUi.W >> 19);
        //        operand1[vPtr++] *= tmpF;
        //        tmpUi = vect & ui6;
        //        tmpF.X = (tmpUi.X >> 20); tmpF.Y = (tmpUi.Y >> 21); tmpF.Z = (tmpUi.Z >> 22); tmpF.W = (tmpUi.W >> 23);
        //        operand1[vPtr++] *= tmpF;
        //        tmpUi = vect & ui7;
        //        tmpF.X = (tmpUi.X >> 24); tmpF.Y = (tmpUi.Y >> 25); tmpF.Z = (tmpUi.Z >> 26); tmpF.W = (tmpUi.W >> 27);
        //        operand1[vPtr++] *= tmpF;
        //        tmpUi = vect & ui8;
        //        tmpF.X = (tmpUi.X >> 28); tmpF.Y = (tmpUi.Y >> 29); tmpF.Z = (tmpUi.Z >> 30); tmpF.W = (tmpUi.W >> 31);
        //        operand1[vPtr++] *= tmpF;
        //    }
        //    return operand1;
        //}

        ///// <summary>
        ///// The safe implementation of conjunction between fuzy and crisp
        ///// bit strings using Vector4 and precomputed uint and shifting creation of the Vector4
        ///// The algoritm presumes the number of elements both in ulong and Vector4
        ///// fields to be a multiple of 64. No static variables used
        ///// </summary>
        ///// <param name="operand1">1. operand (fuzzy)</param>
        ///// <param name="operand2">2. operand (crisp)</param>
        ///// <returns>Conjunction result (fuzzy)</returns>
        //public static Vector4[] AndSafeCrispFuzzyVector43NoStatic(Vector4[] operand1, ulong[] operand2)
        //{
        //    Vector4 tmpF;
        //    Vector<uint> tmpUi;
        //    Vector<uint> vect;
        //    uint part;
        //    int vPtr = 0;

        //    Vector<uint> ui1 = sui1;
        //    Vector<uint> ui2 = sui2;
        //    Vector<uint> ui3 = sui3;
        //    Vector<uint> ui4 = sui4;
        //    Vector<uint> ui5 = sui5;
        //    Vector<uint> ui6 = sui6;
        //    Vector<uint> ui7 = sui7;
        //    Vector<uint> ui8 = sui8;

        //    for (int i = 0; i < operand2.Length; i++)
        //    {
        //        part = (uint)operand2[i]; //last 8 bits
        //        vect = new Vector<uint>(part, part, part, part);

        //        tmpUi = vect & ui1;
        //        tmpF = new Vector4(tmpUi.X, tmpUi.Y >> 1, tmpUi.Z >> 2, tmpUi.W >> 3);
        //        operand1[vPtr++] *= tmpF;
        //        tmpUi = vect & ui2;
        //        tmpF = new Vector4(tmpUi.X >> 4, tmpUi.Y >> 5, tmpUi.Z >> 6, tmpUi.W >> 7);
        //        operand1[vPtr++] *= tmpF;
        //        tmpUi = vect & ui3;
        //        tmpF = new Vector4(tmpUi.X >> 8, tmpUi.Y >> 9, tmpUi.Z >> 10, tmpUi.W >> 11);
        //        operand1[vPtr++] *= tmpF;
        //        tmpUi = vect & ui4;
        //        tmpF = new Vector4(tmpUi.X >> 12, tmpUi.Y >> 13, tmpUi.Z >> 14, tmpUi.W >> 15);
        //        operand1[vPtr++] *= tmpF;
        //        tmpUi = vect & ui5;
        //        tmpF = new Vector4(tmpUi.X >> 16, tmpUi.Y >> 17, tmpUi.Z >> 18, tmpUi.W >> 19);
        //        operand1[vPtr++] *= tmpF;
        //        tmpUi = vect & ui6;
        //        tmpF = new Vector4(tmpUi.X >> 20, tmpUi.Y >> 21, tmpUi.Z >> 22, tmpUi.W >> 23);
        //        operand1[vPtr++] *= tmpF;
        //        tmpUi = vect & ui7;
        //        tmpF = new Vector4(tmpUi.X >> 24, tmpUi.Y >> 25, tmpUi.Z >> 26, tmpUi.W >> 27);
        //        operand1[vPtr++] *= tmpF;
        //        tmpUi = vect & ui8;
        //        tmpF = new Vector4(tmpUi.X >> 28, tmpUi.Y >> 29, tmpUi.Z >> 30, tmpUi.W >> 31);
        //        operand1[vPtr++] *= tmpF;

        //        part = (uint)(operand2[i] >> 32);
        //        vect = new Vector<uint>(part, part, part, part);

        //        tmpUi = vect & ui1;
        //        tmpF = new Vector4(tmpUi.X, tmpUi.Y >> 1, tmpUi.Z >> 2, tmpUi.W >> 3);
        //        operand1[vPtr++] *= tmpF;
        //        tmpUi = vect & ui2;
        //        tmpF = new Vector4(tmpUi.X >> 4, tmpUi.Y >> 5, tmpUi.Z >> 6, tmpUi.W >> 7);
        //        operand1[vPtr++] *= tmpF;
        //        tmpUi = vect & ui3;
        //        tmpF = new Vector4(tmpUi.X >> 8, tmpUi.Y >> 9, tmpUi.Z >> 10, tmpUi.W >> 11);
        //        operand1[vPtr++] *= tmpF;
        //        tmpUi = vect & ui4;
        //        tmpF = new Vector4(tmpUi.X >> 12, tmpUi.Y >> 13, tmpUi.Z >> 14, tmpUi.W >> 15);
        //        operand1[vPtr++] *= tmpF;
        //        tmpUi = vect & ui5;
        //        tmpF = new Vector4(tmpUi.X >> 16, tmpUi.Y >> 17, tmpUi.Z >> 18, tmpUi.W >> 19);
        //        operand1[vPtr++] *= tmpF;
        //        tmpUi = vect & ui6;
        //        tmpF = new Vector4(tmpUi.X >> 20, tmpUi.Y >> 21, tmpUi.Z >> 22, tmpUi.W >> 23);
        //        operand1[vPtr++] *= tmpF;
        //        tmpUi = vect & ui7;
        //        tmpF = new Vector4(tmpUi.X >> 24, tmpUi.Y >> 25, tmpUi.Z >> 26, tmpUi.W >> 27);
        //        operand1[vPtr++] *= tmpF;
        //        tmpUi = vect & ui8;
        //        tmpF = new Vector4(tmpUi.X >> 28, tmpUi.Y >> 29, tmpUi.Z >> 30, tmpUi.W >> 31);
        //        operand1[vPtr++] *= tmpF;
        //    }
        //    return operand1;
        //}

        ///// <summary>
        ///// The unsafe implementation of conjunction between fuzy and crisp
        ///// bit strings using Vector4 and precomputed uint and float vectors.
        ///// The algoritm presumes the number of elements both in ulong and Vector4
        ///// fields to be a multiple of 64.
        ///// </summary>
        ///// <param name="operand1">1. operand (fuzzy)</param>
        ///// <param name="operand2">2. operand (crisp)</param>
        ///// <returns>Conjunction result (fuzzy)</returns>
        //public unsafe static Vector4[] AndUnsafeCrispFuzzyVector42(Vector4[] operand1, ulong[] operand2)
        //{
        //    Vector4 tmpF;
        //    Vector<uint> tmpUi;
        //    Vector<uint> vect;
        //    uint part;

        //    fixed (ulong* pinUL = operand2)
        //    {
        //        fixed (Vector4* pinV = operand1)
        //        {
        //            ulong* ptrUL = pinUL, stopUL = pinUL + operand2.Length;
        //            Vector4* ptrV = pinV;

        //            //the main cycle
        //            while (ptrUL < stopUL)
        //            {
        //                part = (uint)*ptrUL; //last 8 bits
        //                vect = new Vector<uint>(part, part, part, part);

        //                tmpUi = vect & sui1;
        //                tmpF = new Vector4(tmpUi.X, tmpUi.Y, tmpUi.Z, tmpUi.W);
        //                *ptrV *= tmpF / sf1;
        //                ptrV++;
        //                tmpUi = vect & sui2;
        //                tmpF = new Vector4(tmpUi.X, tmpUi.Y, tmpUi.Z, tmpUi.W);
        //                *ptrV *= tmpF / sf2;
        //                ptrV++;
        //                tmpUi = vect & sui3;
        //                tmpF = new Vector4(tmpUi.X, tmpUi.Y, tmpUi.Z, tmpUi.W);
        //                *ptrV *= tmpF / sf3;
        //                ptrV++;
        //                tmpUi = vect & sui4;
        //                tmpF = new Vector4(tmpUi.X, tmpUi.Y, tmpUi.Z, tmpUi.W);
        //                *ptrV *= tmpF / sf4;
        //                ptrV++;
        //                tmpUi = vect & sui5;
        //                tmpF = new Vector4(tmpUi.X, tmpUi.Y, tmpUi.Z, tmpUi.W);
        //                *ptrV *= tmpF / sf5;
        //                ptrV++;
        //                tmpUi = vect & sui6;
        //                tmpF = new Vector4(tmpUi.X, tmpUi.Y, tmpUi.Z, tmpUi.W);
        //                *ptrV *= tmpF / sf6;
        //                ptrV++;
        //                tmpUi = vect & sui7;
        //                tmpF = new Vector4(tmpUi.X, tmpUi.Y, tmpUi.Z, tmpUi.W);
        //                *ptrV *= tmpF / sf7;
        //                ptrV++;
        //                tmpUi = vect & sui8;
        //                tmpF = new Vector4(tmpUi.X, tmpUi.Y, tmpUi.Z, tmpUi.W);
        //                *ptrV *= tmpF / sf8;
        //                ptrV++;

        //                part = (uint)(*ptrUL >> 32);
        //                vect = new Vector<uint>(part, part, part, part);

        //                tmpUi = vect & sui1;
        //                tmpF = new Vector4(tmpUi.X, tmpUi.Y, tmpUi.Z, tmpUi.W);
        //                *ptrV *= tmpF / sf1;
        //                ptrV++;
        //                tmpUi = vect & sui2;
        //                tmpF = new Vector4(tmpUi.X, tmpUi.Y, tmpUi.Z, tmpUi.W);
        //                *ptrV *= tmpF / sf2;
        //                ptrV++;
        //                tmpUi = vect & sui3;
        //                tmpF = new Vector4(tmpUi.X, tmpUi.Y, tmpUi.Z, tmpUi.W);
        //                *ptrV *= tmpF / sf3;
        //                ptrV++;
        //                tmpUi = vect & sui4;
        //                tmpF = new Vector4(tmpUi.X, tmpUi.Y, tmpUi.Z, tmpUi.W);
        //                *ptrV *= tmpF / sf4;
        //                ptrV++;
        //                tmpUi = vect & sui5;
        //                tmpF = new Vector4(tmpUi.X, tmpUi.Y, tmpUi.Z, tmpUi.W);
        //                *ptrV *= tmpF / sf5;
        //                ptrV++;
        //                tmpUi = vect & sui6;
        //                tmpF = new Vector4(tmpUi.X, tmpUi.Y, tmpUi.Z, tmpUi.W);
        //                *ptrV *= tmpF / sf6;
        //                ptrV++;
        //                tmpUi = vect & sui7;
        //                tmpF = new Vector4(tmpUi.X, tmpUi.Y, tmpUi.Z, tmpUi.W);
        //                *ptrV *= tmpF / sf7;
        //                ptrV++;
        //                tmpUi = vect & sui8;
        //                tmpF = new Vector4(tmpUi.X, tmpUi.Y, tmpUi.Z, tmpUi.W);
        //                *ptrV *= tmpF / sf8;
        //                ptrV++;

        //                ptrUL++;
        //            }
        //        }
        //    }
        //    return operand1;
        //}

        ///// <summary>
        ///// The unsafe implementation of conjunction between fuzy and crisp
        ///// bit strings using Vector4 and precomputed uint and float vectors. The algorithm
        ///// doesn't use allocation of new memory in computation of individual Vector4's,
        ///// it assigns to only one Vector4 previously defined. 
        ///// The algoritm presumes the number of elements both in ulong and Vector4
        ///// fields to be a multiple of 64.
        ///// </summary>
        ///// <param name="operand1">1. operand (fuzzy)</param>
        ///// <param name="operand2">2. operand (crisp)</param>
        ///// <returns>Conjunction result (fuzzy)</returns>
        //public unsafe static Vector4[] AndUnsafeCrispFuzzyVector42NoAllocation(Vector4[] operand1, ulong[] operand2)
        //{
        //    Vector4 tmpF = new Vector4();
        //    Vector<uint> tmpUi;
        //    Vector<uint> vect;
        //    uint part;

        //    fixed (ulong* pinUL = operand2)
        //    {
        //        fixed (Vector4* pinV = operand1)
        //        {
        //            ulong* ptrUL = pinUL, stopUL = pinUL + operand2.Length;
        //            Vector4* ptrV = pinV;

        //            //the main cycle
        //            while (ptrUL < stopUL)
        //            {
        //                part = (uint)*ptrUL; //last 8 bits
        //                vect = new Vector<uint>(part, part, part, part);

        //                tmpUi = vect & sui1;
        //                tmpF.X = tmpUi.X; tmpF.Y = tmpUi.Y; tmpF.Z = tmpUi.Z; tmpF.W = tmpUi.W;
        //                *ptrV *= tmpF / sf1;
        //                ptrV++;
        //                tmpUi = vect & sui2;
        //                tmpF.X = tmpUi.X; tmpF.Y = tmpUi.Y; tmpF.Z = tmpUi.Z; tmpF.W = tmpUi.W;
        //                *ptrV *= tmpF / sf2;
        //                ptrV++;
        //                tmpUi = vect & sui3;
        //                tmpF.X = tmpUi.X; tmpF.Y = tmpUi.Y; tmpF.Z = tmpUi.Z; tmpF.W = tmpUi.W;
        //                *ptrV *= tmpF / sf3;
        //                ptrV++;
        //                tmpUi = vect & sui4;
        //                tmpF.X = tmpUi.X; tmpF.Y = tmpUi.Y; tmpF.Z = tmpUi.Z; tmpF.W = tmpUi.W;
        //                *ptrV *= tmpF / sf4;
        //                ptrV++;
        //                tmpUi = vect & sui5;
        //                tmpF.X = tmpUi.X; tmpF.Y = tmpUi.Y; tmpF.Z = tmpUi.Z; tmpF.W = tmpUi.W;
        //                *ptrV *= tmpF / sf5;
        //                ptrV++;
        //                tmpUi = vect & sui6;
        //                tmpF.X = tmpUi.X; tmpF.Y = tmpUi.Y; tmpF.Z = tmpUi.Z; tmpF.W = tmpUi.W;
        //                *ptrV *= tmpF / sf6;
        //                ptrV++;
        //                tmpUi = vect & sui7;
        //                tmpF.X = tmpUi.X; tmpF.Y = tmpUi.Y; tmpF.Z = tmpUi.Z; tmpF.W = tmpUi.W;
        //                *ptrV *= tmpF / sf7;
        //                ptrV++;
        //                tmpUi = vect & sui8;
        //                tmpF.X = tmpUi.X; tmpF.Y = tmpUi.Y; tmpF.Z = tmpUi.Z; tmpF.W = tmpUi.W;
        //                *ptrV *= tmpF / sf8;
        //                ptrV++;

        //                part = (uint)(*ptrUL >> 32);
        //                vect = new Vector<uint>(part, part, part, part);

        //                tmpUi = vect & sui1;
        //                tmpF.X = tmpUi.X; tmpF.Y = tmpUi.Y; tmpF.Z = tmpUi.Z; tmpF.W = tmpUi.W;
        //                *ptrV *= tmpF / sf1;
        //                ptrV++;
        //                tmpUi = vect & sui2;
        //                tmpF.X = tmpUi.X; tmpF.Y = tmpUi.Y; tmpF.Z = tmpUi.Z; tmpF.W = tmpUi.W;
        //                *ptrV *= tmpF / sf2;
        //                ptrV++;
        //                tmpUi = vect & sui3;
        //                tmpF.X = tmpUi.X; tmpF.Y = tmpUi.Y; tmpF.Z = tmpUi.Z; tmpF.W = tmpUi.W;
        //                *ptrV *= tmpF / sf3;
        //                ptrV++;
        //                tmpUi = vect & sui4;
        //                tmpF.X = tmpUi.X; tmpF.Y = tmpUi.Y; tmpF.Z = tmpUi.Z; tmpF.W = tmpUi.W;
        //                *ptrV *= tmpF / sf4;
        //                ptrV++;
        //                tmpUi = vect & sui5;
        //                tmpF.X = tmpUi.X; tmpF.Y = tmpUi.Y; tmpF.Z = tmpUi.Z; tmpF.W = tmpUi.W;
        //                *ptrV *= tmpF / sf5;
        //                ptrV++;
        //                tmpUi = vect & sui6;
        //                tmpF.X = tmpUi.X; tmpF.Y = tmpUi.Y; tmpF.Z = tmpUi.Z; tmpF.W = tmpUi.W;
        //                *ptrV *= tmpF / sf6;
        //                ptrV++;
        //                tmpUi = vect & sui7;
        //                tmpF.X = tmpUi.X; tmpF.Y = tmpUi.Y; tmpF.Z = tmpUi.Z; tmpF.W = tmpUi.W;
        //                *ptrV *= tmpF / sf7;
        //                ptrV++;
        //                tmpUi = vect & sui8;
        //                tmpF.X = tmpUi.X; tmpF.Y = tmpUi.Y; tmpF.Z = tmpUi.Z; tmpF.W = tmpUi.W;
        //                *ptrV *= tmpF / sf8;
        //                ptrV++;

        //                ptrUL++;
        //            }
        //        }
        //    }
        //    return operand1;
        //}

        ///// <summary>
        ///// The unsafe implementation of conjunction between fuzy and crisp
        ///// bit strings using Vector4 and precomputed uint and float vectors. The algorithm
        ///// doesn't use allocation of new memory in computation of individual Vector4's,
        ///// it assigns to only one Vector4 previously defined. No static variables used
        ///// The algoritm presumes the number of elements both in ulong and Vector4
        ///// fields to be a multiple of 64.
        ///// </summary>
        ///// <param name="operand1">1. operand (fuzzy)</param>
        ///// <param name="operand2">2. operand (crisp)</param>
        ///// <returns>Conjunction result (fuzzy)</returns>
        //public unsafe static Vector4[] AndUnsafeCrispFuzzyVector42NoAllocationNoStatic(Vector4[] operand1, ulong[] operand2)
        //{
        //    Vector4 tmpF = new Vector4();
        //    Vector<uint> tmpUi;
        //    Vector<uint> vect;
        //    uint part;

        //    Vector<uint> ui1 = sui1;
        //    Vector<uint> ui2 = sui2;
        //    Vector<uint> ui3 = sui3;
        //    Vector<uint> ui4 = sui4;
        //    Vector<uint> ui5 = sui5;
        //    Vector<uint> ui6 = sui6;
        //    Vector<uint> ui7 = sui7;
        //    Vector<uint> ui8 = sui8;
        //    Vector4 f1 = sf1;
        //    Vector4 f2 = sf2;
        //    Vector4 f3 = sf3;
        //    Vector4 f4 = sf4;
        //    Vector4 f5 = sf5;
        //    Vector4 f6 = sf6;
        //    Vector4 f7 = sf7;
        //    Vector4 f8 = sf8;

        //    fixed (ulong* pinUL = operand2)
        //    {
        //        fixed (Vector4* pinV = operand1)
        //        {
        //            ulong* ptrUL = pinUL, stopUL = pinUL + operand2.Length;
        //            Vector4* ptrV = pinV;

        //            //the main cycle
        //            while (ptrUL < stopUL)
        //            {
        //                part = (uint)*ptrUL; //last 8 bits
        //                vect = new Vector<uint>(part, part, part, part);

        //                tmpUi = vect & ui1;
        //                tmpF.X = tmpUi.X; tmpF.Y = tmpUi.Y; tmpF.Z = tmpUi.Z; tmpF.W = tmpUi.W;
        //                *ptrV *= tmpF / f1;
        //                ptrV++;
        //                tmpUi = vect & ui2;
        //                tmpF.X = tmpUi.X; tmpF.Y = tmpUi.Y; tmpF.Z = tmpUi.Z; tmpF.W = tmpUi.W;
        //                *ptrV *= tmpF / f2;
        //                ptrV++;
        //                tmpUi = vect & ui3;
        //                tmpF.X = tmpUi.X; tmpF.Y = tmpUi.Y; tmpF.Z = tmpUi.Z; tmpF.W = tmpUi.W;
        //                *ptrV *= tmpF / f3;
        //                ptrV++;
        //                tmpUi = vect & ui4;
        //                tmpF.X = tmpUi.X; tmpF.Y = tmpUi.Y; tmpF.Z = tmpUi.Z; tmpF.W = tmpUi.W;
        //                *ptrV *= tmpF / f4;
        //                ptrV++;
        //                tmpUi = vect & ui5;
        //                tmpF.X = tmpUi.X; tmpF.Y = tmpUi.Y; tmpF.Z = tmpUi.Z; tmpF.W = tmpUi.W;
        //                *ptrV *= tmpF / f5;
        //                ptrV++;
        //                tmpUi = vect & ui6;
        //                tmpF.X = tmpUi.X; tmpF.Y = tmpUi.Y; tmpF.Z = tmpUi.Z; tmpF.W = tmpUi.W;
        //                *ptrV *= tmpF / f6;
        //                ptrV++;
        //                tmpUi = vect & ui7;
        //                tmpF.X = tmpUi.X; tmpF.Y = tmpUi.Y; tmpF.Z = tmpUi.Z; tmpF.W = tmpUi.W;
        //                *ptrV *= tmpF / f7;
        //                ptrV++;
        //                tmpUi = vect & ui8;
        //                tmpF.X = tmpUi.X; tmpF.Y = tmpUi.Y; tmpF.Z = tmpUi.Z; tmpF.W = tmpUi.W;
        //                *ptrV *= tmpF / f8;
        //                ptrV++;

        //                part = (uint)(*ptrUL >> 32);
        //                vect = new Vector<uint>(part, part, part, part);

        //                tmpUi = vect & ui1;
        //                tmpF.X = tmpUi.X; tmpF.Y = tmpUi.Y; tmpF.Z = tmpUi.Z; tmpF.W = tmpUi.W;
        //                *ptrV *= tmpF / f1;
        //                ptrV++;
        //                tmpUi = vect & ui2;
        //                tmpF.X = tmpUi.X; tmpF.Y = tmpUi.Y; tmpF.Z = tmpUi.Z; tmpF.W = tmpUi.W;
        //                *ptrV *= tmpF / f2;
        //                ptrV++;
        //                tmpUi = vect & ui3;
        //                tmpF.X = tmpUi.X; tmpF.Y = tmpUi.Y; tmpF.Z = tmpUi.Z; tmpF.W = tmpUi.W;
        //                *ptrV *= tmpF / f3;
        //                ptrV++;
        //                tmpUi = vect & ui4;
        //                tmpF.X = tmpUi.X; tmpF.Y = tmpUi.Y; tmpF.Z = tmpUi.Z; tmpF.W = tmpUi.W;
        //                *ptrV *= tmpF / f4;
        //                ptrV++;
        //                tmpUi = vect & ui5;
        //                tmpF.X = tmpUi.X; tmpF.Y = tmpUi.Y; tmpF.Z = tmpUi.Z; tmpF.W = tmpUi.W;
        //                *ptrV *= tmpF / f5;
        //                ptrV++;
        //                tmpUi = vect & ui6;
        //                tmpF.X = tmpUi.X; tmpF.Y = tmpUi.Y; tmpF.Z = tmpUi.Z; tmpF.W = tmpUi.W;
        //                *ptrV *= tmpF / f6;
        //                ptrV++;
        //                tmpUi = vect & ui7;
        //                tmpF.X = tmpUi.X; tmpF.Y = tmpUi.Y; tmpF.Z = tmpUi.Z; tmpF.W = tmpUi.W;
        //                *ptrV *= tmpF / f7;
        //                ptrV++;
        //                tmpUi = vect & ui8;
        //                tmpF.X = tmpUi.X; tmpF.Y = tmpUi.Y; tmpF.Z = tmpUi.Z; tmpF.W = tmpUi.W;
        //                *ptrV *= tmpF / f8;
        //                ptrV++;

        //                ptrUL++;
        //            }
        //        }
        //    }
        //    return operand1;
        //}

        ///// <summary>
        ///// The unsafe implementation of conjunction between fuzy and crisp
        ///// bit strings using Vector4 and precomputed uint and shifting creation of the Vector4
        ///// The algoritm presumes the number of elements both in ulong and Vector4
        ///// fields to be a multiple of 64.
        ///// </summary>
        ///// <param name="operand1">1. operand (fuzzy)</param>
        ///// <param name="operand2">2. operand (crisp)</param>
        ///// <returns>Conjunction result (fuzzy)</returns>
        //public unsafe static Vector4[] AndUnsafeCrispFuzzyVector43(Vector4[] operand1, ulong[] operand2)
        //{
        //    Vector4 tmpF;
        //    Vector<uint> tmpUi;
        //    Vector<uint> vect;
        //    uint part;

        //    fixed (ulong* pinUL = operand2)
        //    {
        //        fixed (Vector4* pinV = operand1)
        //        {
        //            ulong* ptrUL = pinUL, stopUL = pinUL + operand2.Length;
        //            Vector4* ptrV = pinV;

        //            //the main cycle
        //            while (ptrUL < stopUL)
        //            {
        //                part = (uint)*ptrUL; //last 8 bits
        //                vect = new Vector<uint>(part, part, part, part);

        //                tmpUi = vect & sui1;
        //                tmpF = new Vector4(tmpUi.X, tmpUi.Y >> 1, tmpUi.Z >> 2, tmpUi.W >> 3);
        //                *ptrV *= tmpF;
        //                ptrV++;
        //                tmpUi = vect & sui2;
        //                tmpF = new Vector4(tmpUi.X >> 4, tmpUi.Y >> 5, tmpUi.Z >> 6, tmpUi.W >> 7);
        //                *ptrV *= tmpF;
        //                ptrV++;
        //                tmpUi = vect & sui3;
        //                tmpF = new Vector4(tmpUi.X >> 8, tmpUi.Y >> 9, tmpUi.Z >> 10, tmpUi.W >> 11);
        //                *ptrV *= tmpF;
        //                ptrV++;
        //                tmpUi = vect & sui4;
        //                tmpF = new Vector4(tmpUi.X >> 12, tmpUi.Y >> 13, tmpUi.Z >> 14, tmpUi.W >> 15);
        //                *ptrV *= tmpF;
        //                ptrV++;
        //                tmpUi = vect & sui5;
        //                tmpF = new Vector4(tmpUi.X >> 16, tmpUi.Y >> 17, tmpUi.Z >> 18, tmpUi.W >> 19);
        //                *ptrV *= tmpF;
        //                ptrV++;
        //                tmpUi = vect & sui6;
        //                tmpF = new Vector4(tmpUi.X >> 20, tmpUi.Y >> 21, tmpUi.Z >> 22, tmpUi.W >> 23);
        //                *ptrV *= tmpF;
        //                ptrV++;
        //                tmpUi = vect & sui7;
        //                tmpF = new Vector4(tmpUi.X >> 24, tmpUi.Y >> 25, tmpUi.Z >> 26, tmpUi.W >> 27);
        //                *ptrV *= tmpF;
        //                ptrV++;
        //                tmpUi = vect & sui8;
        //                tmpF = new Vector4(tmpUi.X >> 28, tmpUi.Y >> 29, tmpUi.Z >> 30, tmpUi.W >> 31); ;
        //                *ptrV *= tmpF;
        //                ptrV++;

        //                part = (uint)(*ptrUL >> 32);
        //                vect = new Vector<uint>(part, part, part, part);

        //                tmpUi = vect & sui1;
        //                tmpF = new Vector4(tmpUi.X, tmpUi.Y >> 1, tmpUi.Z >> 2, tmpUi.W >> 3);
        //                *ptrV *= tmpF;
        //                ptrV++;
        //                tmpUi = vect & sui2;
        //                tmpF = new Vector4(tmpUi.X >> 4, tmpUi.Y >> 5, tmpUi.Z >> 6, tmpUi.W >> 7);
        //                *ptrV *= tmpF;
        //                ptrV++;
        //                tmpUi = vect & sui3;
        //                tmpF = new Vector4(tmpUi.X >> 8, tmpUi.Y >> 9, tmpUi.Z >> 10, tmpUi.W >> 11);
        //                *ptrV *= tmpF;
        //                ptrV++;
        //                tmpUi = vect & sui4;
        //                tmpF = new Vector4(tmpUi.X >> 12, tmpUi.Y >> 13, tmpUi.Z >> 14, tmpUi.W >> 15);
        //                *ptrV *= tmpF;
        //                ptrV++;
        //                tmpUi = vect & sui5;
        //                tmpF = new Vector4(tmpUi.X >> 16, tmpUi.Y >> 17, tmpUi.Z >> 18, tmpUi.W >> 19);
        //                *ptrV *= tmpF;
        //                ptrV++;
        //                tmpUi = vect & sui6;
        //                tmpF = new Vector4(tmpUi.X >> 20, tmpUi.Y >> 21, tmpUi.Z >> 22, tmpUi.W >> 23);
        //                *ptrV *= tmpF;
        //                ptrV++;
        //                tmpUi = vect & sui7;
        //                tmpF = new Vector4(tmpUi.X >> 24, tmpUi.Y >> 25, tmpUi.Z >> 26, tmpUi.W >> 27);
        //                *ptrV *= tmpF;
        //                ptrV++;
        //                tmpUi = vect & sui8;
        //                tmpF = new Vector4(tmpUi.X >> 28, tmpUi.Y >> 29, tmpUi.Z >> 30, tmpUi.W >> 31);
        //                *ptrV *= tmpF;
        //                ptrV++;

        //                ptrUL++;
        //            }
        //        }
        //    }
        //    return operand1;
        //}

        ///// <summary>
        ///// The unsafe implementation of conjunction between fuzy and crisp
        ///// bit strings using Vector4 and precomputed uint and shifting creation of the Vector4
        ///// The algoritm presumes the number of elements both in ulong and Vector4
        ///// fields to be a multiple of 64. The algorithm
        ///// doesn't use allocation of new memory in computation of individual Vector4's,
        ///// it assigns to only one Vector4 previously defined. 
        ///// </summary>
        ///// <param name="operand1">1. operand (fuzzy)</param>
        ///// <param name="operand2">2. operand (crisp)</param>
        ///// <returns>Conjunction result (fuzzy)</returns>
        //public unsafe static Vector4[] AndUnsafeCrispFuzzyVector43NoAllocation(Vector4[] operand1, ulong[] operand2)
        //{
        //    Vector4 tmpF = new Vector4();
        //    Vector<uint> tmpUi;
        //    Vector<uint> vect;
        //    uint part;

        //    fixed (ulong* pinUL = operand2)
        //    {
        //        fixed (Vector4* pinV = operand1)
        //        {
        //            ulong* ptrUL = pinUL, stopUL = pinUL + operand2.Length;
        //            Vector4* ptrV = pinV;

        //            //the main cycle
        //            while (ptrUL < stopUL)
        //            {
        //                part = (uint)*ptrUL; //last 8 bits
        //                vect = new Vector<uint>(part, part, part, part);

        //                tmpUi = vect & sui1;
        //                tmpF.X = tmpUi.X; tmpF.Y = (tmpUi.Y >> 1); tmpF.Z = (tmpUi.Z >> 2); tmpF.W = (tmpUi.W >> 3);
        //                *ptrV *= tmpF;
        //                ptrV++;
        //                tmpUi = vect & sui2;
        //                tmpF.X = (tmpUi.X >> 4); tmpF.Y = (tmpUi.Y >> 5); tmpF.Z = (tmpUi.Z >> 6); tmpF.W = (tmpUi.W >> 7);
        //                *ptrV *= tmpF;
        //                ptrV++;
        //                tmpUi = vect & sui3;
        //                tmpF.X = (tmpUi.X >> 8); tmpF.Y = (tmpUi.Y >> 9); tmpF.Z = (tmpUi.Z >> 10); tmpF.W = (tmpUi.W >> 11);
        //                *ptrV *= tmpF;
        //                ptrV++;
        //                tmpUi = vect & sui4;
        //                tmpF.X = (tmpUi.X >> 12); tmpF.Y = (tmpUi.Y >> 13); tmpF.Z = (tmpUi.Z >> 14); tmpF.W = (tmpUi.W >> 15);
        //                *ptrV *= tmpF;
        //                ptrV++;
        //                tmpUi = vect & sui5;
        //                tmpF.X = (tmpUi.X >> 16); tmpF.Y = (tmpUi.Y >> 17); tmpF.Z = (tmpUi.Z >> 18); tmpF.W = (tmpUi.W >> 19);
        //                *ptrV *= tmpF;
        //                ptrV++;
        //                tmpUi = vect & sui6;
        //                tmpF.X = (tmpUi.X >> 20); tmpF.Y = (tmpUi.Y >> 21); tmpF.Z = (tmpUi.Z >> 22); tmpF.W = (tmpUi.W >> 23);
        //                *ptrV *= tmpF;
        //                ptrV++;
        //                tmpUi = vect & sui7;
        //                tmpF.X = (tmpUi.X >> 24); tmpF.Y = (tmpUi.Y >> 25); tmpF.Z = (tmpUi.Z >> 26); tmpF.W = (tmpUi.W >> 27);
        //                *ptrV *= tmpF;
        //                ptrV++;
        //                tmpUi = vect & sui8;
        //                tmpF.X = (tmpUi.X >> 28); tmpF.Y = (tmpUi.Y >> 29); tmpF.Z = (tmpUi.Z >> 30); tmpF.W = (tmpUi.W >> 31);
        //                *ptrV *= tmpF;
        //                ptrV++;

        //                part = (uint)(*ptrUL >> 32);
        //                vect = new Vector<uint>(part, part, part, part);

        //                tmpUi = vect & sui1;
        //                tmpF.X = tmpUi.X; tmpF.Y = (tmpUi.Y >> 1); tmpF.Z = (tmpUi.Z >> 2); tmpF.W = (tmpUi.W >> 3);
        //                *ptrV *= tmpF;
        //                ptrV++;
        //                tmpUi = vect & sui2;
        //                tmpF.X = (tmpUi.X >> 4); tmpF.Y = (tmpUi.Y >> 5); tmpF.Z = (tmpUi.Z >> 6); tmpF.W = (tmpUi.W >> 7);
        //                *ptrV *= tmpF;
        //                ptrV++;
        //                tmpUi = vect & sui3;
        //                tmpF.X = (tmpUi.X >> 8); tmpF.Y = (tmpUi.Y >> 9); tmpF.Z = (tmpUi.Z >> 10); tmpF.W = (tmpUi.W >> 11);
        //                *ptrV *= tmpF;
        //                ptrV++;
        //                tmpUi = vect & sui4;
        //                tmpF.X = (tmpUi.X >> 12); tmpF.Y = (tmpUi.Y >> 13); tmpF.Z = (tmpUi.Z >> 14); tmpF.W = (tmpUi.W >> 15);
        //                *ptrV *= tmpF;
        //                ptrV++;
        //                tmpUi = vect & sui5;
        //                tmpF.X = (tmpUi.X >> 16); tmpF.Y = (tmpUi.Y >> 17); tmpF.Z = (tmpUi.Z >> 18); tmpF.W = (tmpUi.W >> 19);
        //                *ptrV *= tmpF;
        //                ptrV++;
        //                tmpUi = vect & sui6;
        //                tmpF.X = (tmpUi.X >> 20); tmpF.Y = (tmpUi.Y >> 21); tmpF.Z = (tmpUi.Z >> 22); tmpF.W = (tmpUi.W >> 23);
        //                *ptrV *= tmpF;
        //                ptrV++;
        //                tmpUi = vect & sui7;
        //                tmpF.X = (tmpUi.X >> 24); tmpF.Y = (tmpUi.Y >> 25); tmpF.Z = (tmpUi.Z >> 26); tmpF.W = (tmpUi.W >> 27);
        //                *ptrV *= tmpF;
        //                ptrV++;
        //                tmpUi = vect & sui8;
        //                tmpF.X = (tmpUi.X >> 28); tmpF.Y = (tmpUi.Y >> 29); tmpF.Z = (tmpUi.Z >> 30); tmpF.W = (tmpUi.W >> 31);
        //                *ptrV *= tmpF;
        //                ptrV++;

        //                ptrUL++;
        //            }
        //        }
        //    }
        //    return operand1;
        //}

        ///// <summary>
        ///// The unsafe implementation of conjunction between fuzy and crisp
        ///// bit strings using Vector4 and precomputed uint and shifting creation of the Vector4
        ///// The algoritm presumes the number of elements both in ulong and Vector4
        ///// fields to be a multiple of 64. No static variables used
        ///// </summary>
        ///// <param name="operand1">1. operand (fuzzy)</param>
        ///// <param name="operand2">2. operand (crisp)</param>
        ///// <returns>Conjunction result (fuzzy)</returns>
        //public unsafe static Vector4[] AndUnsafeCrispFuzzyVector43NoStatic(Vector4[] operand1, ulong[] operand2)
        //{
        //    Vector4 tmpF;
        //    Vector<uint> tmpUi;
        //    Vector<uint> vect;
        //    uint part;

        //    Vector<uint> ui1 = sui1;
        //    Vector<uint> ui2 = sui2;
        //    Vector<uint> ui3 = sui3;
        //    Vector<uint> ui4 = sui4;
        //    Vector<uint> ui5 = sui5;
        //    Vector<uint> ui6 = sui6;
        //    Vector<uint> ui7 = sui7;
        //    Vector<uint> ui8 = sui8;

        //    fixed (ulong* pinUL = operand2)
        //    {
        //        fixed (Vector4* pinV = operand1)
        //        {
        //            ulong* ptrUL = pinUL, stopUL = pinUL + operand2.Length;
        //            Vector4* ptrV = pinV;

        //            //the main cycle
        //            while (ptrUL < stopUL)
        //            {
        //                part = (uint)*ptrUL; //last 8 bits
        //                vect = new Vector<uint>(part, part, part, part);

        //                tmpUi = vect & ui1;
        //                tmpF = new Vector4(tmpUi.X, tmpUi.Y >> 1, tmpUi.Z >> 2, tmpUi.W >> 3);
        //                *ptrV *= tmpF;
        //                ptrV++;
        //                tmpUi = vect & ui2;
        //                tmpF = new Vector4(tmpUi.X >> 4, tmpUi.Y >> 5, tmpUi.Z >> 6, tmpUi.W >> 7);
        //                *ptrV *= tmpF;
        //                ptrV++;
        //                tmpUi = vect & ui3;
        //                tmpF = new Vector4(tmpUi.X >> 8, tmpUi.Y >> 9, tmpUi.Z >> 10, tmpUi.W >> 11);
        //                *ptrV *= tmpF;
        //                ptrV++;
        //                tmpUi = vect & ui4;
        //                tmpF = new Vector4(tmpUi.X >> 12, tmpUi.Y >> 13, tmpUi.Z >> 14, tmpUi.W >> 15);
        //                *ptrV *= tmpF;
        //                ptrV++;
        //                tmpUi = vect & ui5;
        //                tmpF = new Vector4(tmpUi.X >> 16, tmpUi.Y >> 17, tmpUi.Z >> 18, tmpUi.W >> 19);
        //                *ptrV *= tmpF;
        //                ptrV++;
        //                tmpUi = vect & ui6;
        //                tmpF = new Vector4(tmpUi.X >> 20, tmpUi.Y >> 21, tmpUi.Z >> 22, tmpUi.W >> 23);
        //                *ptrV *= tmpF;
        //                ptrV++;
        //                tmpUi = vect & ui7;
        //                tmpF = new Vector4(tmpUi.X >> 24, tmpUi.Y >> 25, tmpUi.Z >> 26, tmpUi.W >> 27);
        //                *ptrV *= tmpF;
        //                ptrV++;
        //                tmpUi = vect & ui8;
        //                tmpF = new Vector4(tmpUi.X >> 28, tmpUi.Y >> 29, tmpUi.Z >> 30, tmpUi.W >> 31); ;
        //                *ptrV *= tmpF;
        //                ptrV++;

        //                part = (uint)(*ptrUL >> 32);
        //                vect = new Vector<uint>(part, part, part, part);

        //                tmpUi = vect & ui1;
        //                tmpF = new Vector4(tmpUi.X, tmpUi.Y >> 1, tmpUi.Z >> 2, tmpUi.W >> 3);
        //                *ptrV *= tmpF;
        //                ptrV++;
        //                tmpUi = vect & ui2;
        //                tmpF = new Vector4(tmpUi.X >> 4, tmpUi.Y >> 5, tmpUi.Z >> 6, tmpUi.W >> 7);
        //                *ptrV *= tmpF;
        //                ptrV++;
        //                tmpUi = vect & ui3;
        //                tmpF = new Vector4(tmpUi.X >> 8, tmpUi.Y >> 9, tmpUi.Z >> 10, tmpUi.W >> 11);
        //                *ptrV *= tmpF;
        //                ptrV++;
        //                tmpUi = vect & ui4;
        //                tmpF = new Vector4(tmpUi.X >> 12, tmpUi.Y >> 13, tmpUi.Z >> 14, tmpUi.W >> 15);
        //                *ptrV *= tmpF;
        //                ptrV++;
        //                tmpUi = vect & ui5;
        //                tmpF = new Vector4(tmpUi.X >> 16, tmpUi.Y >> 17, tmpUi.Z >> 18, tmpUi.W >> 19);
        //                *ptrV *= tmpF;
        //                ptrV++;
        //                tmpUi = vect & ui6;
        //                tmpF = new Vector4(tmpUi.X >> 20, tmpUi.Y >> 21, tmpUi.Z >> 22, tmpUi.W >> 23);
        //                *ptrV *= tmpF;
        //                ptrV++;
        //                tmpUi = vect & ui7;
        //                tmpF = new Vector4(tmpUi.X >> 24, tmpUi.Y >> 25, tmpUi.Z >> 26, tmpUi.W >> 27);
        //                *ptrV *= tmpF;
        //                ptrV++;
        //                tmpUi = vect & ui8;
        //                tmpF = new Vector4(tmpUi.X >> 28, tmpUi.Y >> 29, tmpUi.Z >> 30, tmpUi.W >> 31);
        //                *ptrV *= tmpF;
        //                ptrV++;

        //                ptrUL++;
        //            }
        //        }
        //    }
        //    return operand1;
        //}

        ///// <summary>
        ///// The unsafe implementation of conjunction between fuzy and crisp
        ///// bit strings using Vector4 and precomputed uint and shifting creation of the Vector4
        ///// The algoritm presumes the number of elements both in ulong and Vector4
        ///// fields to be a multiple of 64. The algorithm
        ///// doesn't use allocation of new memory in computation of individual Vector4's,
        ///// it assigns to only one Vector4 previously defined. No static variables used
        ///// </summary>
        ///// <param name="operand1">1. operand (fuzzy)</param>
        ///// <param name="operand2">2. operand (crisp)</param>
        ///// <returns>Conjunction result (fuzzy)</returns>
        //public unsafe static Vector4[] AndUnsafeCrispFuzzyVector43NoAllocationNoStatic(Vector4[] operand1, ulong[] operand2)
        //{
        //    Vector4 tmpF = new Vector4();
        //    Vector<uint> tmpUi;
        //    Vector<uint> vect;
        //    uint part;

        //    Vector<uint> ui1 = sui1;
        //    Vector<uint> ui2 = sui2;
        //    Vector<uint> ui3 = sui3;
        //    Vector<uint> ui4 = sui4;
        //    Vector<uint> ui5 = sui5;
        //    Vector<uint> ui6 = sui6;
        //    Vector<uint> ui7 = sui7;
        //    Vector<uint> ui8 = sui8;

        //    fixed (ulong* pinUL = operand2)
        //    {
        //        fixed (Vector4* pinV = operand1)
        //        {
        //            ulong* ptrUL = pinUL, stopUL = pinUL + operand2.Length;
        //            Vector4* ptrV = pinV;

        //            //the main cycle
        //            while (ptrUL < stopUL)
        //            {
        //                part = (uint)*ptrUL; //last 8 bits
        //                vect = new Vector<uint>(part, part, part, part);

        //                tmpUi = vect & ui1;
        //                tmpF.X = tmpUi.X; tmpF.Y = (tmpUi.Y >> 1); tmpF.Z = (tmpUi.Z >> 2); tmpF.W = (tmpUi.W >> 3);
        //                *ptrV *= tmpF;
        //                ptrV++;
        //                tmpUi = vect & ui2;
        //                tmpF.X = (tmpUi.X >> 4); tmpF.Y = (tmpUi.Y >> 5); tmpF.Z = (tmpUi.Z >> 6); tmpF.W = (tmpUi.W >> 7);
        //                *ptrV *= tmpF;
        //                ptrV++;
        //                tmpUi = vect & ui3;
        //                tmpF.X = (tmpUi.X >> 8); tmpF.Y = (tmpUi.Y >> 9); tmpF.Z = (tmpUi.Z >> 10); tmpF.W = (tmpUi.W >> 11);
        //                *ptrV *= tmpF;
        //                ptrV++;
        //                tmpUi = vect & ui4;
        //                tmpF.X = (tmpUi.X >> 12); tmpF.Y = (tmpUi.Y >> 13); tmpF.Z = (tmpUi.Z >> 14); tmpF.W = (tmpUi.W >> 15);
        //                *ptrV *= tmpF;
        //                ptrV++;
        //                tmpUi = vect & ui5;
        //                tmpF.X = (tmpUi.X >> 16); tmpF.Y = (tmpUi.Y >> 17); tmpF.Z = (tmpUi.Z >> 18); tmpF.W = (tmpUi.W >> 19);
        //                *ptrV *= tmpF;
        //                ptrV++;
        //                tmpUi = vect & ui6;
        //                tmpF.X = (tmpUi.X >> 20); tmpF.Y = (tmpUi.Y >> 21); tmpF.Z = (tmpUi.Z >> 22); tmpF.W = (tmpUi.W >> 23);
        //                *ptrV *= tmpF;
        //                ptrV++;
        //                tmpUi = vect & ui7;
        //                tmpF.X = (tmpUi.X >> 24); tmpF.Y = (tmpUi.Y >> 25); tmpF.Z = (tmpUi.Z >> 26); tmpF.W = (tmpUi.W >> 27);
        //                *ptrV *= tmpF;
        //                ptrV++;
        //                tmpUi = vect & ui8;
        //                tmpF.X = (tmpUi.X >> 28); tmpF.Y = (tmpUi.Y >> 29); tmpF.Z = (tmpUi.Z >> 30); tmpF.W = (tmpUi.W >> 31);
        //                *ptrV *= tmpF;
        //                ptrV++;

        //                part = (uint)(*ptrUL >> 32);
        //                vect = new Vector<uint>(part, part, part, part);

        //                tmpUi = vect & ui1;
        //                tmpF.X = tmpUi.X; tmpF.Y = (tmpUi.Y >> 1); tmpF.Z = (tmpUi.Z >> 2); tmpF.W = (tmpUi.W >> 3);
        //                *ptrV *= tmpF;
        //                ptrV++;
        //                tmpUi = vect & ui2;
        //                tmpF.X = (tmpUi.X >> 4); tmpF.Y = (tmpUi.Y >> 5); tmpF.Z = (tmpUi.Z >> 6); tmpF.W = (tmpUi.W >> 7);
        //                *ptrV *= tmpF;
        //                ptrV++;
        //                tmpUi = vect & ui3;
        //                tmpF.X = (tmpUi.X >> 8); tmpF.Y = (tmpUi.Y >> 9); tmpF.Z = (tmpUi.Z >> 10); tmpF.W = (tmpUi.W >> 11);
        //                *ptrV *= tmpF;
        //                ptrV++;
        //                tmpUi = vect & ui4;
        //                tmpF.X = (tmpUi.X >> 12); tmpF.Y = (tmpUi.Y >> 13); tmpF.Z = (tmpUi.Z >> 14); tmpF.W = (tmpUi.W >> 15);
        //                *ptrV *= tmpF;
        //                ptrV++;
        //                tmpUi = vect & ui5;
        //                tmpF.X = (tmpUi.X >> 16); tmpF.Y = (tmpUi.Y >> 17); tmpF.Z = (tmpUi.Z >> 18); tmpF.W = (tmpUi.W >> 19);
        //                *ptrV *= tmpF;
        //                ptrV++;
        //                tmpUi = vect & ui6;
        //                tmpF.X = (tmpUi.X >> 20); tmpF.Y = (tmpUi.Y >> 21); tmpF.Z = (tmpUi.Z >> 22); tmpF.W = (tmpUi.W >> 23);
        //                *ptrV *= tmpF;
        //                ptrV++;
        //                tmpUi = vect & ui7;
        //                tmpF.X = (tmpUi.X >> 24); tmpF.Y = (tmpUi.Y >> 25); tmpF.Z = (tmpUi.Z >> 26); tmpF.W = (tmpUi.W >> 27);
        //                *ptrV *= tmpF;
        //                ptrV++;
        //                tmpUi = vect & ui8;
        //                tmpF.X = (tmpUi.X >> 28); tmpF.Y = (tmpUi.Y >> 29); tmpF.Z = (tmpUi.Z >> 30); tmpF.W = (tmpUi.W >> 31);
        //                *ptrV *= tmpF;
        //                ptrV++;

        //                ptrUL++;
        //            }
        //        }
        //    }
        //    return operand1;
        //}

        #endregion
    }
}
