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
        #region Fields

        /// <summary>
        /// The ULONG representing 1
        /// </summary>
        private const ulong _one = 1ul;

        //uint precomputed vectors
        static Vector4ui sui1 = new Vector4ui(0x00000001, 0x00000002, 0x00000004, 0x00000008);
        static Vector4ui sui2 = new Vector4ui(0x00000010, 0x00000020, 0x00000040, 0x00000080);
        static Vector4ui sui3 = new Vector4ui(0x00000100, 0x00000200, 0x00000400, 0x00000800);
        static Vector4ui sui4 = new Vector4ui(0x00001000, 0x00002000, 0x00004000, 0x00008000);
        static Vector4ui sui5 = new Vector4ui(0x00010000, 0x00020000, 0x00040000, 0x00080000);
        static Vector4ui sui6 = new Vector4ui(0x00100000, 0x00200000, 0x00400000, 0x00800000);
        static Vector4ui sui7 = new Vector4ui(0x01000000, 0x02000000, 0x04000000, 0x08000000);
        static Vector4ui sui8 = new Vector4ui(0x10000000, 0x20000000, 0x40000000, 0x80000000);
        //float precomputed vectors
        static Vector4f sf1 = new Vector4f(0x00000001, 0x00000002, 0x00000004, 0x00000008);
        static Vector4f sf2 = new Vector4f(0x00000010, 0x00000020, 0x00000040, 0x00000080);
        static Vector4f sf3 = new Vector4f(0x00000100, 0x00000200, 0x00000400, 0x00000800);
        static Vector4f sf4 = new Vector4f(0x00001000, 0x00002000, 0x00004000, 0x00008000);
        static Vector4f sf5 = new Vector4f(0x00010000, 0x00020000, 0x00040000, 0x00080000);
        static Vector4f sf6 = new Vector4f(0x00100000, 0x00200000, 0x00400000, 0x00800000);
        static Vector4f sf7 = new Vector4f(0x01000000, 0x02000000, 0x04000000, 0x08000000);
        static Vector4f sf8 = new Vector4f(0x10000000, 0x20000000, 0x40000000, 0x80000000);

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
        /// The Vector4f and benchmark for fuzzy and crisp bit strings - unsafe,
        /// precomputed version
        /// </summary>
        [Benchmark]
        public static void UnsafeAndCrispFuzzyVector4fPrecomputed()
        {
            Vector4f[] tmp = stringVector4f;
            ulong[] tmp2 = stringUlong;
            int count = iterations;
            for (int i = 0; i < count; i++)
            {
                AndUnsafeCrispFuzzyVector4f2(tmp, tmp2);
            }
        }

        /// <summary>
        /// The Vector4f and benchmark for fuzzy and crisp bit strings - unsafe,
        /// precomputed version without allocation of new memory in the cycles
        /// </summary>
        [Benchmark]
        public static void UnsafeAndCrispFuzzyVector4fPrecomputedNoAllocation()
        {
            Vector4f[] tmp = stringVector4f;
            ulong[] tmp2 = stringUlong;
            int count = iterations;
            for (int i = 0; i < count; i++)
            {
                AndUnsafeCrispFuzzyVector4f2NoAllocation(tmp, tmp2);
            }
        }

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
        /// The Vector4f and benchmark for fuzzy and crisp bit strings - safe, 
        /// naive version
        /// </summary>
        [Benchmark]
        public static void SafeAndCrispFuzzyVector4fNaive()
        {
            //don't use static variables in iterations
            Vector4f[] tmp = stringVector4f;
            ulong[] tmp2 = stringUlong;
            int count = iterations;
            for (int i = 0; i < count; i++)
            {
                AndSafeCrispFuzzyVector4f1(tmp, tmp2);
            }
        }

        /// <summary>
        /// The Vector4f and benchmark for fuzzy and crisp bit strings - safe,
        /// precomputed version
        /// </summary>
        [Benchmark]
        public static void SafeAndCrispFuzzyVector4fPrecomputed()
        {
            //don't use static variables in iterations
            Vector4f[] tmp = stringVector4f;
            ulong[] tmp2 = stringUlong;
            int count = iterations;
            for (int i = 0; i < count; i++)
            {
                AndSafeCrispFuzzyVector4f2(tmp, tmp2);
            }
        }

        /// <summary>
        /// The Vector4f and benchmark for fuzzy and crisp bit strings - safe,
        /// precomputed version without static variables
        /// </summary>
        [Benchmark]
        public static void SafeAndCrispFuzzyVector4fPrecomputedNoStatic()
        {
            //don't use static variables in iterations
            Vector4f[] tmp = stringVector4f;
            ulong[] tmp2 = stringUlong;
            int count = iterations;
            for (int i = 0; i < count; i++)
            {
                AndSafeCrispFuzzyVector4f2NoStatic(tmp, tmp2);
            }
        }

        /// <summary>
        /// The Vector4f and benchmark for fuzzy and crisp bit strings - safe,
        /// precomputed version with shifts in creation of Vector4f
        /// </summary>
        [Benchmark]
        public static void SafeAndCrispFuzzyVector4fPrecomputedShift()
        {
            //don't use static variables in iterations
            Vector4f[] tmp = stringVector4f;
            ulong[] tmp2 = stringUlong;
            int count = iterations;
            for (int i = 0; i < count; i++)
            {
                AndSafeCrispFuzzyVector4f3(tmp, tmp2);
            }
        }

        /// <summary>
        /// The Vector4f and benchmark for fuzzy and crisp bit strings - safe,
        /// precomputed version with shifts in creation of Vector4f without
        /// static variables
        /// </summary>
        [Benchmark]
        public static void SafeAndCrispFuzzyVector4fPrecomputedShiftNoStatic()
        {
            //don't use static variables in iterations
            Vector4f[] tmp = stringVector4f;
            ulong[] tmp2 = stringUlong;
            int count = iterations;
            for (int i = 0; i < count; i++)
            {
                AndSafeCrispFuzzyVector4f3NoStatic(tmp, tmp2);
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
                AndSafeCrispFuzzyFloat(tmp, tmp2);
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

        /// <summary>
        /// The safe (managed) implementation of conjunction between fuzzy and crisp 
        /// bit string using Vector4f. Algorithm 1 - naive
        /// </summary>
        /// <param name="operand1">1. operand (fuzzy)</param>
        /// <param name="operand2">2. operand (crisp)</param>
        /// <returns>Conjunction result (fuzzy)</returns>
        static Vector4f[] AndSafeCrispFuzzyVector4f1(Vector4f[] operand1, ulong[] operand2)
        {
            for (int i = 0; i < operand1.Length; i++)
            {
                Vector4f tmp = new Vector4f(
                    Convert.ToSingle((operand2[4 * i / 64] & (_one << (4 * i % 64))) > 0),
                    Convert.ToSingle((operand2[(4 * i + 1) / 64] & (_one << ((4 * i + 1) % 64))) > 0),
                    Convert.ToSingle((operand2[(4 * i + 2) / 64] & (_one << ((4 * i + 2) % 64))) > 0),
                    Convert.ToSingle((operand2[(4 * i + 3) / 64] & (_one << ((4 * i + 3) % 64))) > 0));
                operand1[i] *= tmp;
            }
            return operand1;
        }

        /// <summary>
        /// The safe (managed) implementation of conjunction between fuzzy and crisp 
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
        /// The safe (managed) implementation of conjunction between fuzy and crisp
        /// bit strings using Vector4f and precomputed uint and float vectors.
        /// The algoritm presumes the number of elements both in ulong and Vector4f
        /// fields to be a multiple of 64.
        /// </summary>
        /// <param name="operand1">1. operand (fuzzy)</param>
        /// <param name="operand2">2. operand (crisp)</param>
        /// <returns>Conjunction result (fuzzy)</returns>
        public static Vector4f[] AndSafeCrispFuzzyVector4f2(Vector4f[] operand1, ulong[] operand2)
        {
            Vector4f tmpF;
            Vector4ui tmpUi;
            Vector4ui vect;
            uint part;
            int vPtr = 0;

            for (int i = 0; i < operand2.Length; i++)
            {
                part = (uint)operand2[i]; //last 8 bits
                vect = new Vector4ui(part, part, part, part);

                tmpUi = vect & sui1;
                tmpF = new Vector4f(tmpUi.X , tmpUi.Y , tmpUi.Z ,  tmpUi.W ) ;
                operand1[vPtr++] *= tmpF / sf1;
                tmpUi = vect & sui2;
                tmpF = new Vector4f(tmpUi.X, tmpUi.Y, tmpUi.Z, tmpUi.W);
                operand1[vPtr++] *= tmpF / sf2;
                tmpUi = vect & sui3;
                tmpF = new Vector4f(tmpUi.X, tmpUi.Y, tmpUi.Z, tmpUi.W);
                operand1[vPtr++] *= tmpF / sf3;
                tmpUi = vect & sui4;
                tmpF = new Vector4f(tmpUi.X, tmpUi.Y, tmpUi.Z, tmpUi.W);
                operand1[vPtr++] *= tmpF / sf4;
                tmpUi = vect & sui5;
                tmpF = new Vector4f(tmpUi.X, tmpUi.Y, tmpUi.Z, tmpUi.W);
                operand1[vPtr++] *= tmpF / sf5;
                tmpUi = vect & sui6;
                tmpF = new Vector4f(tmpUi.X, tmpUi.Y, tmpUi.Z, tmpUi.W);
                operand1[vPtr++] *= tmpF / sf6;
                tmpUi = vect & sui7;
                tmpF = new Vector4f(tmpUi.X, tmpUi.Y, tmpUi.Z, tmpUi.W);
                operand1[vPtr++] *= tmpF / sf7;
                tmpUi = vect & sui8;
                tmpF = new Vector4f(tmpUi.X, tmpUi.Y, tmpUi.Z, tmpUi.W);
                operand1[vPtr++] *= tmpF / sf8;

                part = (uint)(operand2[i] >> 32);
                vect = new Vector4ui(part, part, part, part);

                tmpUi = vect & sui1;
                tmpF = new Vector4f(tmpUi.X, tmpUi.Y, tmpUi.Z, tmpUi.W);
                operand1[vPtr++] *= tmpF / sf1;
                tmpUi = vect & sui2;
                tmpF = new Vector4f(tmpUi.X, tmpUi.Y, tmpUi.Z, tmpUi.W);
                operand1[vPtr++] *= tmpF / sf2;
                tmpUi = vect & sui3;
                tmpF = new Vector4f(tmpUi.X, tmpUi.Y, tmpUi.Z, tmpUi.W);
                operand1[vPtr++] *= tmpF / sf3;
                tmpUi = vect & sui4;
                tmpF = new Vector4f(tmpUi.X, tmpUi.Y, tmpUi.Z, tmpUi.W);
                operand1[vPtr++] *= tmpF / sf4;
                tmpUi = vect & sui5;
                tmpF = new Vector4f(tmpUi.X, tmpUi.Y, tmpUi.Z, tmpUi.W);
                operand1[vPtr++] *= tmpF / sf5;
                tmpUi = vect & sui6;
                tmpF = new Vector4f(tmpUi.X, tmpUi.Y, tmpUi.Z, tmpUi.W);
                operand1[vPtr++] *= tmpF / sf6;
                tmpUi = vect & sui7;
                tmpF = new Vector4f(tmpUi.X, tmpUi.Y, tmpUi.Z, tmpUi.W);
                operand1[vPtr++] *= tmpF / sf7;
                tmpUi = vect & sui8;
                tmpF = new Vector4f(tmpUi.X, tmpUi.Y, tmpUi.Z, tmpUi.W);
                operand1[vPtr++] *= tmpF / sf8;
            }
            return operand1;
        }

        /// <summary>
        /// The safe (managed) implementation of conjunction between fuzy and crisp
        /// bit strings using Vector4f and precomputed uint and float vectors.
        /// The algoritm presumes the number of elements both in ulong and Vector4f
        /// fields to be a multiple of 64. There are no static variables used. 
        /// </summary>
        /// <param name="operand1">1. operand (fuzzy)</param>
        /// <param name="operand2">2. operand (crisp)</param>
        /// <returns>Conjunction result (fuzzy)</returns>
        public static Vector4f[] AndSafeCrispFuzzyVector4f2NoStatic(Vector4f[] operand1, ulong[] operand2)
        {
            Vector4f tmpF;
            Vector4ui tmpUi;
            Vector4ui vect;
            uint part;
            int vPtr = 0;

            Vector4ui ui1 = sui1;
            Vector4ui ui2 = sui2;
            Vector4ui ui3 = sui3;
            Vector4ui ui4 = sui4;
            Vector4ui ui5 = sui5;
            Vector4ui ui6 = sui6;
            Vector4ui ui7 = sui7;
            Vector4ui ui8 = sui8;
            Vector4f f1 = sf1;
            Vector4f f2 = sf2;
            Vector4f f3 = sf3;
            Vector4f f4 = sf4;
            Vector4f f5 = sf5;
            Vector4f f6 = sf6;
            Vector4f f7 = sf7;
            Vector4f f8 = sf8;

            for (int i = 0; i < operand2.Length; i++)
            {
                part = (uint)operand2[i]; //last 8 bits
                vect = new Vector4ui(part, part, part, part);

                tmpUi = vect & ui1;
                tmpF = new Vector4f(tmpUi.X, tmpUi.Y, tmpUi.Z, tmpUi.W);
                operand1[vPtr++] *= tmpF / f1;
                tmpUi = vect & ui2;
                tmpF = new Vector4f(tmpUi.X, tmpUi.Y, tmpUi.Z, tmpUi.W);
                operand1[vPtr++] *= tmpF / f2;
                tmpUi = vect & ui3;
                tmpF = new Vector4f(tmpUi.X, tmpUi.Y, tmpUi.Z, tmpUi.W);
                operand1[vPtr++] *= tmpF / f3;
                tmpUi = vect & ui4;
                tmpF = new Vector4f(tmpUi.X, tmpUi.Y, tmpUi.Z, tmpUi.W);
                operand1[vPtr++] *= tmpF / f4;
                tmpUi = vect & ui5;
                tmpF = new Vector4f(tmpUi.X, tmpUi.Y, tmpUi.Z, tmpUi.W);
                operand1[vPtr++] *= tmpF / f5;
                tmpUi = vect & ui6;
                tmpF = new Vector4f(tmpUi.X, tmpUi.Y, tmpUi.Z, tmpUi.W);
                operand1[vPtr++] *= tmpF / f6;
                tmpUi = vect & ui7;
                tmpF = new Vector4f(tmpUi.X, tmpUi.Y, tmpUi.Z, tmpUi.W);
                operand1[vPtr++] *= tmpF / f7;
                tmpUi = vect & ui8;
                tmpF = new Vector4f(tmpUi.X, tmpUi.Y, tmpUi.Z, tmpUi.W);
                operand1[vPtr++] *= tmpF / f8;

                part = (uint)(operand2[i] >> 32);
                vect = new Vector4ui(part, part, part, part);

                tmpUi = vect & ui1;
                tmpF = new Vector4f(tmpUi.X, tmpUi.Y, tmpUi.Z, tmpUi.W);
                operand1[vPtr++] *= tmpF / f1;
                tmpUi = vect & ui2;
                tmpF = new Vector4f(tmpUi.X, tmpUi.Y, tmpUi.Z, tmpUi.W);
                operand1[vPtr++] *= tmpF / f2;
                tmpUi = vect & ui3;
                tmpF = new Vector4f(tmpUi.X, tmpUi.Y, tmpUi.Z, tmpUi.W);
                operand1[vPtr++] *= tmpF / f3;
                tmpUi = vect & ui4;
                tmpF = new Vector4f(tmpUi.X, tmpUi.Y, tmpUi.Z, tmpUi.W);
                operand1[vPtr++] *= tmpF / f4;
                tmpUi = vect & ui5;
                tmpF = new Vector4f(tmpUi.X, tmpUi.Y, tmpUi.Z, tmpUi.W);
                operand1[vPtr++] *= tmpF / f5;
                tmpUi = vect & ui6;
                tmpF = new Vector4f(tmpUi.X, tmpUi.Y, tmpUi.Z, tmpUi.W);
                operand1[vPtr++] *= tmpF / f6;
                tmpUi = vect & ui7;
                tmpF = new Vector4f(tmpUi.X, tmpUi.Y, tmpUi.Z, tmpUi.W);
                operand1[vPtr++] *= tmpF / f7;
                tmpUi = vect & ui8;
                tmpF = new Vector4f(tmpUi.X, tmpUi.Y, tmpUi.Z, tmpUi.W);
                operand1[vPtr++] *= tmpF / f8;
            }
            return operand1;
        }

        /// <summary>
        /// The safe (managed) implementation of conjunction between fuzy and crisp
        /// bit strings using Vector4f and precomputed uint and shifting creation of the Vector4f
        /// The algoritm presumes the number of elements both in ulong and Vector4f
        /// fields to be a multiple of 64.
        /// </summary>
        /// <param name="operand1">1. operand (fuzzy)</param>
        /// <param name="operand2">2. operand (crisp)</param>
        /// <returns>Conjunction result (fuzzy)</returns>
        public static Vector4f[] AndSafeCrispFuzzyVector4f3(Vector4f[] operand1, ulong[] operand2)
        {
            Vector4f tmpF;
            Vector4ui tmpUi;
            Vector4ui vect;
            uint part;
            int vPtr = 0;

            for (int i = 0; i < operand2.Length; i++)
            {
                part = (uint)operand2[i]; //last 8 bits
                vect = new Vector4ui(part, part, part, part);

                tmpUi = vect & sui1;
                tmpF = new Vector4f(tmpUi.X, tmpUi.Y >> 1, tmpUi.Z >> 2, tmpUi.W >> 3);
                operand1[vPtr++] *= tmpF;
                tmpUi = vect & sui2;
                tmpF = new Vector4f(tmpUi.X >> 4, tmpUi.Y >> 5, tmpUi.Z >> 6, tmpUi.W >> 7);
                operand1[vPtr++] *= tmpF;
                tmpUi = vect & sui3;
                tmpF = new Vector4f(tmpUi.X >> 8, tmpUi.Y >> 9, tmpUi.Z >> 10, tmpUi.W >> 11);
                operand1[vPtr++] *= tmpF;
                tmpUi = vect & sui4;
                tmpF = new Vector4f(tmpUi.X >> 12, tmpUi.Y >> 13, tmpUi.Z >> 14, tmpUi.W >> 15);
                operand1[vPtr++] *= tmpF;
                tmpUi = vect & sui5;
                tmpF = new Vector4f(tmpUi.X >> 16, tmpUi.Y >> 17, tmpUi.Z >> 18, tmpUi.W >> 19);
                operand1[vPtr++] *= tmpF;
                tmpUi = vect & sui6;
                tmpF = new Vector4f(tmpUi.X >> 20, tmpUi.Y >> 21, tmpUi.Z >> 22, tmpUi.W >> 23);
                operand1[vPtr++] *= tmpF;
                tmpUi = vect & sui7;
                tmpF = new Vector4f(tmpUi.X >> 24, tmpUi.Y >> 25, tmpUi.Z >> 26, tmpUi.W >> 27);
                operand1[vPtr++] *= tmpF;
                tmpUi = vect & sui8;
                tmpF = new Vector4f(tmpUi.X >> 28, tmpUi.Y >> 29, tmpUi.Z >> 30, tmpUi.W >> 31);
                operand1[vPtr++] *= tmpF;

                part = (uint)(operand2[i] >> 32);
                vect = new Vector4ui(part, part, part, part);

                tmpUi = vect & sui1;
                tmpF = new Vector4f(tmpUi.X, tmpUi.Y >> 1, tmpUi.Z >> 2, tmpUi.W >> 3);
                operand1[vPtr++] *= tmpF;
                tmpUi = vect & sui2;
                tmpF = new Vector4f(tmpUi.X >> 4, tmpUi.Y >> 5, tmpUi.Z >> 6, tmpUi.W >> 7);
                operand1[vPtr++] *= tmpF;
                tmpUi = vect & sui3;
                tmpF = new Vector4f(tmpUi.X >> 8, tmpUi.Y >> 9, tmpUi.Z >> 10, tmpUi.W >> 11);
                operand1[vPtr++] *= tmpF;
                tmpUi = vect & sui4;
                tmpF = new Vector4f(tmpUi.X >> 12, tmpUi.Y >> 13, tmpUi.Z >> 14, tmpUi.W >> 15);
                operand1[vPtr++] *= tmpF;
                tmpUi = vect & sui5;
                tmpF = new Vector4f(tmpUi.X >> 16, tmpUi.Y >> 17, tmpUi.Z >> 18, tmpUi.W >> 19);
                operand1[vPtr++] *= tmpF;
                tmpUi = vect & sui6;
                tmpF = new Vector4f(tmpUi.X >> 20, tmpUi.Y >> 21, tmpUi.Z >> 22, tmpUi.W >> 23);
                operand1[vPtr++] *= tmpF;
                tmpUi = vect & sui7;
                tmpF = new Vector4f(tmpUi.X >> 24, tmpUi.Y >> 25, tmpUi.Z >> 26, tmpUi.W >> 27);
                operand1[vPtr++] *= tmpF;
                tmpUi = vect & sui8;
                tmpF = new Vector4f(tmpUi.X >> 28, tmpUi.Y >> 29, tmpUi.Z >> 30, tmpUi.W >> 31);
                operand1[vPtr++] *= tmpF;
            }
            return operand1;
        }

        /// <summary>
        /// The safe (managed) implementation of conjunction between fuzy and crisp
        /// bit strings using Vector4f and precomputed uint and shifting creation of the Vector4f
        /// The algoritm presumes the number of elements both in ulong and Vector4f
        /// fields to be a multiple of 64. No static variables used
        /// </summary>
        /// <param name="operand1">1. operand (fuzzy)</param>
        /// <param name="operand2">2. operand (crisp)</param>
        /// <returns>Conjunction result (fuzzy)</returns>
        public static Vector4f[] AndSafeCrispFuzzyVector4f3NoStatic(Vector4f[] operand1, ulong[] operand2)
        {
            Vector4f tmpF;
            Vector4ui tmpUi;
            Vector4ui vect;
            uint part;
            int vPtr = 0;

            Vector4ui ui1 = sui1;
            Vector4ui ui2 = sui2;
            Vector4ui ui3 = sui3;
            Vector4ui ui4 = sui4;
            Vector4ui ui5 = sui5;
            Vector4ui ui6 = sui6;
            Vector4ui ui7 = sui7;
            Vector4ui ui8 = sui8;

            for (int i = 0; i < operand2.Length; i++)
            {
                part = (uint)operand2[i]; //last 8 bits
                vect = new Vector4ui(part, part, part, part);

                tmpUi = vect & ui1;
                tmpF = new Vector4f(tmpUi.X, tmpUi.Y >> 1, tmpUi.Z >> 2, tmpUi.W >> 3);
                operand1[vPtr++] *= tmpF;
                tmpUi = vect & ui2;
                tmpF = new Vector4f(tmpUi.X >> 4, tmpUi.Y >> 5, tmpUi.Z >> 6, tmpUi.W >> 7);
                operand1[vPtr++] *= tmpF;
                tmpUi = vect & ui3;
                tmpF = new Vector4f(tmpUi.X >> 8, tmpUi.Y >> 9, tmpUi.Z >> 10, tmpUi.W >> 11);
                operand1[vPtr++] *= tmpF;
                tmpUi = vect & ui4;
                tmpF = new Vector4f(tmpUi.X >> 12, tmpUi.Y >> 13, tmpUi.Z >> 14, tmpUi.W >> 15);
                operand1[vPtr++] *= tmpF;
                tmpUi = vect & ui5;
                tmpF = new Vector4f(tmpUi.X >> 16, tmpUi.Y >> 17, tmpUi.Z >> 18, tmpUi.W >> 19);
                operand1[vPtr++] *= tmpF;
                tmpUi = vect & ui6;
                tmpF = new Vector4f(tmpUi.X >> 20, tmpUi.Y >> 21, tmpUi.Z >> 22, tmpUi.W >> 23);
                operand1[vPtr++] *= tmpF;
                tmpUi = vect & ui7;
                tmpF = new Vector4f(tmpUi.X >> 24, tmpUi.Y >> 25, tmpUi.Z >> 26, tmpUi.W >> 27);
                operand1[vPtr++] *= tmpF;
                tmpUi = vect & ui8;
                tmpF = new Vector4f(tmpUi.X >> 28, tmpUi.Y >> 29, tmpUi.Z >> 30, tmpUi.W >> 31);
                operand1[vPtr++] *= tmpF;

                part = (uint)(operand2[i] >> 32);
                vect = new Vector4ui(part, part, part, part);

                tmpUi = vect & ui1;
                tmpF = new Vector4f(tmpUi.X, tmpUi.Y >> 1, tmpUi.Z >> 2, tmpUi.W >> 3);
                operand1[vPtr++] *= tmpF;
                tmpUi = vect & ui2;
                tmpF = new Vector4f(tmpUi.X >> 4, tmpUi.Y >> 5, tmpUi.Z >> 6, tmpUi.W >> 7);
                operand1[vPtr++] *= tmpF;
                tmpUi = vect & ui3;
                tmpF = new Vector4f(tmpUi.X >> 8, tmpUi.Y >> 9, tmpUi.Z >> 10, tmpUi.W >> 11);
                operand1[vPtr++] *= tmpF;
                tmpUi = vect & ui4;
                tmpF = new Vector4f(tmpUi.X >> 12, tmpUi.Y >> 13, tmpUi.Z >> 14, tmpUi.W >> 15);
                operand1[vPtr++] *= tmpF;
                tmpUi = vect & ui5;
                tmpF = new Vector4f(tmpUi.X >> 16, tmpUi.Y >> 17, tmpUi.Z >> 18, tmpUi.W >> 19);
                operand1[vPtr++] *= tmpF;
                tmpUi = vect & ui6;
                tmpF = new Vector4f(tmpUi.X >> 20, tmpUi.Y >> 21, tmpUi.Z >> 22, tmpUi.W >> 23);
                operand1[vPtr++] *= tmpF;
                tmpUi = vect & ui7;
                tmpF = new Vector4f(tmpUi.X >> 24, tmpUi.Y >> 25, tmpUi.Z >> 26, tmpUi.W >> 27);
                operand1[vPtr++] *= tmpF;
                tmpUi = vect & ui8;
                tmpF = new Vector4f(tmpUi.X >> 28, tmpUi.Y >> 29, tmpUi.Z >> 30, tmpUi.W >> 31);
                operand1[vPtr++] *= tmpF;
            }
            return operand1;
        }

        /// <summary>
        /// The unsafe (unmanaged) implementation of conjunction between fuzy and crisp
        /// bit strings using Vector4f and precomputed uint and float vectors.
        /// The algoritm presumes the number of elements both in ulong and Vector4f
        /// fields to be a multiple of 64.
        /// </summary>
        /// <param name="operand1">1. operand (fuzzy)</param>
        /// <param name="operand2">2. operand (crisp)</param>
        /// <returns>Conjunction result (fuzzy)</returns>
        public unsafe static Vector4f[] AndUnsafeCrispFuzzyVector4f2(Vector4f[] operand1, ulong[] operand2)
        {
            Vector4f tmpF;
            Vector4ui tmpUi;
            Vector4ui vect;
            uint part;

            fixed (ulong* pinUL = operand2)
            {
                fixed (Vector4f* pinV = operand1)
                {
                    ulong* ptrUL = pinUL, stopUL = pinUL + operand2.Length;
                    Vector4f* ptrV = pinV;

                    //the main cycle
                    while (ptrUL < stopUL)
                    {
                        part = (uint)*ptrUL; //last 8 bits
                        vect = new Vector4ui(part, part, part, part);

                        tmpUi = vect & sui1;
                        tmpF = new Vector4f(tmpUi.X, tmpUi.Y, tmpUi.Z, tmpUi.W);
                        *ptrV *= tmpF / sf1;
                        ptrV++;
                        tmpUi = vect & sui2;
                        tmpF = new Vector4f(tmpUi.X, tmpUi.Y, tmpUi.Z, tmpUi.W);
                        *ptrV *= tmpF / sf2;
                        ptrV++;
                        tmpUi = vect & sui3;
                        tmpF = new Vector4f(tmpUi.X, tmpUi.Y, tmpUi.Z, tmpUi.W);
                        *ptrV *= tmpF / sf3;
                        ptrV++;
                        tmpUi = vect & sui4;
                        tmpF = new Vector4f(tmpUi.X, tmpUi.Y, tmpUi.Z, tmpUi.W);
                        *ptrV *= tmpF / sf4;
                        ptrV++;
                        tmpUi = vect & sui5;
                        tmpF = new Vector4f(tmpUi.X, tmpUi.Y, tmpUi.Z, tmpUi.W);
                        *ptrV *= tmpF / sf5;
                        ptrV++;
                        tmpUi = vect & sui6;
                        tmpF = new Vector4f(tmpUi.X, tmpUi.Y, tmpUi.Z, tmpUi.W);
                        *ptrV *= tmpF / sf6;
                        ptrV++;
                        tmpUi = vect & sui7;
                        tmpF = new Vector4f(tmpUi.X, tmpUi.Y, tmpUi.Z, tmpUi.W);
                        *ptrV *= tmpF / sf7;
                        ptrV++;
                        tmpUi = vect & sui8;
                        tmpF = new Vector4f(tmpUi.X, tmpUi.Y, tmpUi.Z, tmpUi.W);
                        *ptrV *= tmpF / sf8;
                        ptrV++;

                        part = (uint)(*ptrUL >> 32);
                        vect = new Vector4ui(part, part, part, part);

                        tmpUi = vect & sui1;
                        tmpF = new Vector4f(tmpUi.X, tmpUi.Y, tmpUi.Z, tmpUi.W);
                        *ptrV *= tmpF / sf1;
                        ptrV++;
                        tmpUi = vect & sui2;
                        tmpF = new Vector4f(tmpUi.X, tmpUi.Y, tmpUi.Z, tmpUi.W);
                        *ptrV *= tmpF / sf2;
                        ptrV++;
                        tmpUi = vect & sui3;
                        tmpF = new Vector4f(tmpUi.X, tmpUi.Y, tmpUi.Z, tmpUi.W);
                        *ptrV *= tmpF / sf3;
                        ptrV++;
                        tmpUi = vect & sui4;
                        tmpF = new Vector4f(tmpUi.X, tmpUi.Y, tmpUi.Z, tmpUi.W);
                        *ptrV *= tmpF / sf4;
                        ptrV++;
                        tmpUi = vect & sui5;
                        tmpF = new Vector4f(tmpUi.X, tmpUi.Y, tmpUi.Z, tmpUi.W);
                        *ptrV *= tmpF / sf5;
                        ptrV++;
                        tmpUi = vect & sui6;
                        tmpF = new Vector4f(tmpUi.X, tmpUi.Y, tmpUi.Z, tmpUi.W);
                        *ptrV *= tmpF / sf6;
                        ptrV++;
                        tmpUi = vect & sui7;
                        tmpF = new Vector4f(tmpUi.X, tmpUi.Y, tmpUi.Z, tmpUi.W);
                        *ptrV *= tmpF / sf7;
                        ptrV++;
                        tmpUi = vect & sui8;
                        tmpF = new Vector4f(tmpUi.X, tmpUi.Y, tmpUi.Z, tmpUi.W);
                        *ptrV *= tmpF / sf8;
                        ptrV++;

                        ptrUL++;
                    }
                }
            }
            return operand1;
        }

        /// <summary>
        /// The unsafe (unmanaged) implementation of conjunction between fuzy and crisp
        /// bit strings using Vector4f and precomputed uint and float vectors. The algorithm
        /// doesn't use allocation of new memory in computation of individual Vector4f's,
        /// it assigns to only one Vector4f previously defined. 
        /// The algoritm presumes the number of elements both in ulong and Vector4f
        /// fields to be a multiple of 64.
        /// </summary>
        /// <param name="operand1">1. operand (fuzzy)</param>
        /// <param name="operand2">2. operand (crisp)</param>
        /// <returns>Conjunction result (fuzzy)</returns>
        public unsafe static Vector4f[] AndUnsafeCrispFuzzyVector4f2NoAllocation(Vector4f[] operand1, ulong[] operand2)
        {
            Vector4f tmpF = new Vector4f();
            Vector4ui tmpUi;
            Vector4ui vect;
            uint part;

            fixed (ulong* pinUL = operand2)
            {
                fixed (Vector4f* pinV = operand1)
                {
                    ulong* ptrUL = pinUL, stopUL = pinUL + operand2.Length;
                    Vector4f* ptrV = pinV;

                    //the main cycle
                    while (ptrUL < stopUL)
                    {
                        part = (uint)*ptrUL; //last 8 bits
                        vect = new Vector4ui(part, part, part, part);

                        tmpUi = vect & sui1;
                        tmpF.X = tmpUi.X; tmpF.Y = tmpUi.Y; tmpF.Z = tmpUi.Z; tmpF.W = tmpUi.W;
                        *ptrV *= tmpF / sf1;
                        ptrV++;
                        tmpUi = vect & sui2;
                        tmpF.X = tmpUi.X; tmpF.Y = tmpUi.Y; tmpF.Z = tmpUi.Z; tmpF.W = tmpUi.W;
                        *ptrV *= tmpF / sf2;
                        ptrV++;
                        tmpUi = vect & sui3;
                        tmpF.X = tmpUi.X; tmpF.Y = tmpUi.Y; tmpF.Z = tmpUi.Z; tmpF.W = tmpUi.W;
                        *ptrV *= tmpF / sf3;
                        ptrV++;
                        tmpUi = vect & sui4;
                        tmpF.X = tmpUi.X; tmpF.Y = tmpUi.Y; tmpF.Z = tmpUi.Z; tmpF.W = tmpUi.W;
                        *ptrV *= tmpF / sf4;
                        ptrV++;
                        tmpUi = vect & sui5;
                        tmpF.X = tmpUi.X; tmpF.Y = tmpUi.Y; tmpF.Z = tmpUi.Z; tmpF.W = tmpUi.W;
                        *ptrV *= tmpF / sf5;
                        ptrV++;
                        tmpUi = vect & sui6;
                        tmpF.X = tmpUi.X; tmpF.Y = tmpUi.Y; tmpF.Z = tmpUi.Z; tmpF.W = tmpUi.W;
                        *ptrV *= tmpF / sf6;
                        ptrV++;
                        tmpUi = vect & sui7;
                        tmpF.X = tmpUi.X; tmpF.Y = tmpUi.Y; tmpF.Z = tmpUi.Z; tmpF.W = tmpUi.W;
                        *ptrV *= tmpF / sf7;
                        ptrV++;
                        tmpUi = vect & sui8;
                        tmpF.X = tmpUi.X; tmpF.Y = tmpUi.Y; tmpF.Z = tmpUi.Z; tmpF.W = tmpUi.W;
                        *ptrV *= tmpF / sf8;
                        ptrV++;

                        part = (uint)(*ptrUL >> 32);
                        vect = new Vector4ui(part, part, part, part);

                        tmpUi = vect & sui1;
                        tmpF.X = tmpUi.X; tmpF.Y = tmpUi.Y; tmpF.Z = tmpUi.Z; tmpF.W = tmpUi.W;
                        *ptrV *= tmpF / sf1;
                        ptrV++;
                        tmpUi = vect & sui2;
                        tmpF.X = tmpUi.X; tmpF.Y = tmpUi.Y; tmpF.Z = tmpUi.Z; tmpF.W = tmpUi.W;
                        *ptrV *= tmpF / sf2;
                        ptrV++;
                        tmpUi = vect & sui3;
                        tmpF.X = tmpUi.X; tmpF.Y = tmpUi.Y; tmpF.Z = tmpUi.Z; tmpF.W = tmpUi.W;
                        *ptrV *= tmpF / sf3;
                        ptrV++;
                        tmpUi = vect & sui4;
                        tmpF.X = tmpUi.X; tmpF.Y = tmpUi.Y; tmpF.Z = tmpUi.Z; tmpF.W = tmpUi.W;
                        *ptrV *= tmpF / sf4;
                        ptrV++;
                        tmpUi = vect & sui5;
                        tmpF.X = tmpUi.X; tmpF.Y = tmpUi.Y; tmpF.Z = tmpUi.Z; tmpF.W = tmpUi.W;
                        *ptrV *= tmpF / sf5;
                        ptrV++;
                        tmpUi = vect & sui6;
                        tmpF.X = tmpUi.X; tmpF.Y = tmpUi.Y; tmpF.Z = tmpUi.Z; tmpF.W = tmpUi.W;
                        *ptrV *= tmpF / sf6;
                        ptrV++;
                        tmpUi = vect & sui7;
                        tmpF.X = tmpUi.X; tmpF.Y = tmpUi.Y; tmpF.Z = tmpUi.Z; tmpF.W = tmpUi.W;
                        *ptrV *= tmpF / sf7;
                        ptrV++;
                        tmpUi = vect & sui8;
                        tmpF.X = tmpUi.X; tmpF.Y = tmpUi.Y; tmpF.Z = tmpUi.Z; tmpF.W = tmpUi.W;
                        *ptrV *= tmpF / sf8;
                        ptrV++;

                        ptrUL++;
                    }
                }
            }
            return operand1;
        }

        /// <summary>
        /// The unsafe (unmanaged) implementation of conjunction between fuzy and crisp
        /// bit strings using Vector4f and precomputed uint and float vectors. The algorithm
        /// doesn't use allocation of new memory in computation of individual Vector4f's,
        /// it assigns to only one Vector4f previously defined. No static variables used
        /// The algoritm presumes the number of elements both in ulong and Vector4f
        /// fields to be a multiple of 64.
        /// </summary>
        /// <param name="operand1">1. operand (fuzzy)</param>
        /// <param name="operand2">2. operand (crisp)</param>
        /// <returns>Conjunction result (fuzzy)</returns>
        public unsafe static Vector4f[] AndUnsafeCrispFuzzyVector4fNoAllocationNoStatic(Vector4f[] operand1, ulong[] operand2)
        {
            Vector4f tmpF = new Vector4f();
            Vector4ui tmpUi;
            Vector4ui vect;
            uint part;

            Vector4ui ui1 = sui1;
            Vector4ui ui2 = sui2;
            Vector4ui ui3 = sui3;
            Vector4ui ui4 = sui4;
            Vector4ui ui5 = sui5;
            Vector4ui ui6 = sui6;
            Vector4ui ui7 = sui7;
            Vector4ui ui8 = sui8;
            Vector4f f1 = sf1;
            Vector4f f2 = sf2;
            Vector4f f3 = sf3;
            Vector4f f4 = sf4;
            Vector4f f5 = sf5;
            Vector4f f6 = sf6;
            Vector4f f7 = sf7;
            Vector4f f8 = sf8;

            fixed (ulong* pinUL = operand2)
            {
                fixed (Vector4f* pinV = operand1)
                {
                    ulong* ptrUL = pinUL, stopUL = pinUL + operand2.Length;
                    Vector4f* ptrV = pinV;

                    //the main cycle
                    while (ptrUL < stopUL)
                    {
                        part = (uint)*ptrUL; //last 8 bits
                        vect = new Vector4ui(part, part, part, part);

                        tmpUi = vect & ui1;
                        tmpF.X = tmpUi.X; tmpF.Y = tmpUi.Y; tmpF.Z = tmpUi.Z; tmpF.W = tmpUi.W;
                        *ptrV *= tmpF / f1;
                        ptrV++;
                        tmpUi = vect & ui2;
                        tmpF.X = tmpUi.X; tmpF.Y = tmpUi.Y; tmpF.Z = tmpUi.Z; tmpF.W = tmpUi.W;
                        *ptrV *= tmpF / f2;
                        ptrV++;
                        tmpUi = vect & ui3;
                        tmpF.X = tmpUi.X; tmpF.Y = tmpUi.Y; tmpF.Z = tmpUi.Z; tmpF.W = tmpUi.W;
                        *ptrV *= tmpF / f3;
                        ptrV++;
                        tmpUi = vect & ui4;
                        tmpF.X = tmpUi.X; tmpF.Y = tmpUi.Y; tmpF.Z = tmpUi.Z; tmpF.W = tmpUi.W;
                        *ptrV *= tmpF / f4;
                        ptrV++;
                        tmpUi = vect & ui5;
                        tmpF.X = tmpUi.X; tmpF.Y = tmpUi.Y; tmpF.Z = tmpUi.Z; tmpF.W = tmpUi.W;
                        *ptrV *= tmpF / f5;
                        ptrV++;
                        tmpUi = vect & ui6;
                        tmpF.X = tmpUi.X; tmpF.Y = tmpUi.Y; tmpF.Z = tmpUi.Z; tmpF.W = tmpUi.W;
                        *ptrV *= tmpF / f6;
                        ptrV++;
                        tmpUi = vect & ui7;
                        tmpF.X = tmpUi.X; tmpF.Y = tmpUi.Y; tmpF.Z = tmpUi.Z; tmpF.W = tmpUi.W;
                        *ptrV *= tmpF / f7;
                        ptrV++;
                        tmpUi = vect & ui8;
                        tmpF.X = tmpUi.X; tmpF.Y = tmpUi.Y; tmpF.Z = tmpUi.Z; tmpF.W = tmpUi.W;
                        *ptrV *= tmpF / f8;
                        ptrV++;

                        part = (uint)(*ptrUL >> 32);
                        vect = new Vector4ui(part, part, part, part);

                        tmpUi = vect & ui1;
                        tmpF.X = tmpUi.X; tmpF.Y = tmpUi.Y; tmpF.Z = tmpUi.Z; tmpF.W = tmpUi.W;
                        *ptrV *= tmpF / f1;
                        ptrV++;
                        tmpUi = vect & ui2;
                        tmpF.X = tmpUi.X; tmpF.Y = tmpUi.Y; tmpF.Z = tmpUi.Z; tmpF.W = tmpUi.W;
                        *ptrV *= tmpF / f2;
                        ptrV++;
                        tmpUi = vect & ui3;
                        tmpF.X = tmpUi.X; tmpF.Y = tmpUi.Y; tmpF.Z = tmpUi.Z; tmpF.W = tmpUi.W;
                        *ptrV *= tmpF / f3;
                        ptrV++;
                        tmpUi = vect & ui4;
                        tmpF.X = tmpUi.X; tmpF.Y = tmpUi.Y; tmpF.Z = tmpUi.Z; tmpF.W = tmpUi.W;
                        *ptrV *= tmpF / f4;
                        ptrV++;
                        tmpUi = vect & ui5;
                        tmpF.X = tmpUi.X; tmpF.Y = tmpUi.Y; tmpF.Z = tmpUi.Z; tmpF.W = tmpUi.W;
                        *ptrV *= tmpF / f5;
                        ptrV++;
                        tmpUi = vect & ui6;
                        tmpF.X = tmpUi.X; tmpF.Y = tmpUi.Y; tmpF.Z = tmpUi.Z; tmpF.W = tmpUi.W;
                        *ptrV *= tmpF / f6;
                        ptrV++;
                        tmpUi = vect & ui7;
                        tmpF.X = tmpUi.X; tmpF.Y = tmpUi.Y; tmpF.Z = tmpUi.Z; tmpF.W = tmpUi.W;
                        *ptrV *= tmpF / f7;
                        ptrV++;
                        tmpUi = vect & ui8;
                        tmpF.X = tmpUi.X; tmpF.Y = tmpUi.Y; tmpF.Z = tmpUi.Z; tmpF.W = tmpUi.W;
                        *ptrV *= tmpF / f8;
                        ptrV++;

                        ptrUL++;
                    }
                }
            }
            return operand1;
        }

        #endregion
    }
}
