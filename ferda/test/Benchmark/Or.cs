// Or.cs
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
using System.Collections.Generic;
using Mono.Simd;

namespace Ferda.Benchmark
{
    /// <summary>
    /// The benchmark for the disjunction operation in bit strings. It does not
    /// use the actual methods implemented in the FerdaMiningProcessor, because
    /// the idea is to test different algorithms and their modifications and then
    /// to use the best one.
    /// </summary>
	public class Or: FerdaBenchmark
    {
        #region Fields

        #endregion

        #region Init, Reset, Check

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
        /// The crisp OR benchmark - safe
        /// </summary>
        [Benchmark]
        public static void SafeOrCrisp()
        {
            //don't use static variables in iterations
            ulong[] tmp = stringUlong;
            ulong[] tmp2 = stringUlong2;
            int count = iterations;
            for (int i = 0; i < count; i++)
            {
                OrSafeCrisp(tmp, tmp2);
            }
        }

        /// <summary>
        /// The crisp OR benchmark - unsafe
        /// </summary>
        [Benchmark]
        public static void UnsafeOrCrisp()
        {
            //don't use static variables in iterations
            ulong[] tmp = stringUlong;
            ulong[] tmp2 = stringUlong2;
            int count = iterations;
            for (int i = 0; i < count; i++)
            {
                OrUnsafeCrisp(tmp, tmp2);
            }
        }

        /// <summary>
        /// The Vector4f OR benchmark - safe
        /// </summary>
        [Benchmark]
        public static void SafeOrFuzzyVector4f()
        {
            //don't use static variables in iterations
            Vector4f[] tmp = stringVector4f;
            Vector4f[] tmp2 = stringVector4f2;
            int count = iterations;
            for (int i = 0; i < count; i++)
            {
                OrSafe4f(tmp, tmp2);
            }
        }

        /// <summary>
        /// The Vector4f OR benchmark - unsafe
        /// </summary>
        [Benchmark]
        public static void UnsafeOrFuzzyVector4f()
        {
            //don't use static variables in iterations
            Vector4f[] tmp = stringVector4f;
            Vector4f[] tmp2 = stringVector4f2;
            int count = iterations;
            for (int i = 0; i < count; i++)
            {
                OrUnsafe4f(tmp, tmp2);
            }
        }

        /// <summary>
        /// The float fuzzy OR benchmark - safe
        /// </summary>
        [Benchmark]
        public static void SafeOrFuzzyFloat()
        {
            //don't use static variables in iterations
            float[] tmp = stringFloat;
            float[] tmp2 = stringFloat2;
            int count = iterations;
            for (int i = 0; i < count; i++)
            {
                OrSafeFloat(tmp, tmp2);
            }
        }

        /// <summary>
        /// The float fuzzy OR benchmark - unsafe
        /// </summary>
        [Benchmark]
        public static void UnsafeOrFuzzyFloat()
        {
            //don't use static variables in iterations
            float[] tmp = stringFloat;
            float[] tmp2 = stringFloat2;
            int count = iterations;
            for (int i = 0; i < count; i++)
            {
                OrUnsafeFloat(tmp, tmp2);
            }
        }

        /// <summary>
        /// The OR benchmark for fuzzy and crips bit strings - using array of floats
        /// </summary>
        [Benchmark]
        public static void SafeOrCrispFuzzyFloat()
        {
            //don't use static variables in iterations
            float[] tmp = stringFloat;
            ulong[] tmp2 = stringUlong;
            int count = iterations;
            for (int i = 0; i < count; i++)
            {
                OrSafeCrispFuzzyFloat(tmp, tmp2);
            }
        }

        /// <summary>
        /// The Vector4f OR benchmark for fuzzy and crisp bit strings - safe, 
        /// naive version
        /// </summary>
        [Benchmark]
        public static void SafeOrCrispFuzzyVector4fNaive()
        {
            //don't use static variables in iterations
            Vector4f[] tmp = stringVector4f;
            ulong[] tmp2 = stringUlong;
            int count = iterations;
            for (int i = 0; i < count; i++)
            {
                OrSafeCrispFuzzyVector4f1(tmp, tmp2);
            }
        }

        /// <summary>
        /// The Vector4f OR benchmark for fuzzy and crisp bit strings - safe,
        /// precomputed version
        /// </summary>
        [Benchmark]
        public static void SafeOrCrispFuzzyVector4fPrecomputed()
        {
            //don't use static variables in iterations
            Vector4f[] tmp = stringVector4f;
            ulong[] tmp2 = stringUlong;
            int count = iterations;
            for (int i = 0; i < count; i++)
            {
                OrSafeCrispFuzzyVector4f2(tmp, tmp2);
            }
        }

        /// <summary>
        /// The Vector4f OR benchmark for fuzzy and crisp bit strings - safe,
        /// precomputed version with shifts in creation of Vector4f
        /// </summary>
        [Benchmark]
        public static void SafeOrCrispFuzzyVector4fPrecomputedShift()
        {
            //don't use static variables in iterations
            Vector4f[] tmp = stringVector4f;
            ulong[] tmp2 = stringUlong;
            int count = iterations;
            for (int i = 0; i < count; i++)
            {
                OrSafeCrispFuzzyVector4f3(tmp, tmp2);
            }
        }

        /// <summary>
        /// The Vector4f OR benchmark for fuzzy and crisp bit strings - safe,
        /// precomputed version with shifts in creation of Vector4f without
        /// static variables
        /// </summary>
        [Benchmark]
        public static void SafeOrCrispFuzzyVector4fPrecomputedShiftNoStatic()
        {
            //don't use static variables in iterations
            Vector4f[] tmp = stringVector4f;
            ulong[] tmp2 = stringUlong;
            int count = iterations;
            for (int i = 0; i < count; i++)
            {
                OrSafeCrispFuzzyVector4f3NoStatic(tmp, tmp2);
            }
        }

        /// <summary>
        /// The Vector4f OR benchmark for fuzzy and crisp bit strings - safe,
        /// precomputed version with shifts in creation of Vector4f without
        /// alocations of new Vector4f's
        /// </summary>
        [Benchmark]
        public static void SafeOrCrispFuzzyVector4fPrecomputedShiftNoAllocation()
        {
            //don't use static variables in iterations
            Vector4f[] tmp = stringVector4f;
            ulong[] tmp2 = stringUlong;
            int count = iterations;
            for (int i = 0; i < count; i++)
            {
                OrSafeCrispFuzzyVector4f3NoAllocation(tmp, tmp2);
            }
        }

        /// <summary>
        /// The Vector4f OR benchmark for fuzzy and crisp bit strings - unsafe,
        /// precomputed version
        /// </summary>
        [Benchmark]
        public static void UnsafeOrCrispFuzzyVector4fPrecomputed()
        {
            Vector4f[] tmp = stringVector4f;
            ulong[] tmp2 = stringUlong;
            int count = iterations;
            for (int i = 0; i < count; i++)
            {
                OrUnsafeCrispFuzzyVector4f2(tmp, tmp2);
            }
        }

        /// <summary>
        /// The Vector4f OR benchmark for fuzzy and crisp bit strings - unsafe,
        /// precomputed version without allocation of new memory in the cycles
        /// </summary>
        [Benchmark]
        public static void UnsafeOrCrispFuzzyVector4fPrecomputedNoAllocation()
        {
            Vector4f[] tmp = stringVector4f;
            ulong[] tmp2 = stringUlong;
            int count = iterations;
            for (int i = 0; i < count; i++)
            {
                OrUnsafeCrispFuzzyVector4f2NoAllocation(tmp, tmp2);
            }
        }

        /// <summary>
        /// The Vector4f OR benchmark for fuzzy and crisp bit strings - unsafe,
        /// precomputed version without static variables, without allocation of new memory
        /// in the cycles
        /// </summary>
        [Benchmark]
        public static void UnsafeOrCrispFuzzyVector4fPrecomputedNoStaticNoAllocation()
        {
            Vector4f[] tmp = stringVector4f;
            ulong[] tmp2 = stringUlong;
            int count = iterations;
            for (int i = 0; i < count; i++)
            {
                OrUnsafeCrispFuzzyVector4f2NoAllocationNoStatic(tmp, tmp2);
            }
        }

        /// <summary>
        /// The Vector4f OR benchmark for fuzzy and crisp bit strings - unsafe,
        /// precomputed version with shifts in creation of Vector4f
        /// </summary>
        [Benchmark]
        public static void UnsafeOrCrispFuzzyVector4fPrecomputedShift()
        {
            Vector4f[] tmp = stringVector4f;
            ulong[] tmp2 = stringUlong;
            int count = iterations;
            for (int i = 0; i < count; i++)
            {
                OrUnsafeCrispFuzzyVector4f3(tmp, tmp2);
            }
        }

        /// <summary>
        /// The Vector4f OR benchmark for fuzzy and crisp bit strings - unsafe,
        /// precomputed version with shifts in creation of Vector4f, 
        /// no static variables
        /// </summary>
        [Benchmark]
        public static void UnsafeOrCrispFuzzyVector4fPrecomputedShiftNoStatic()
        {
            Vector4f[] tmp = stringVector4f;
            ulong[] tmp2 = stringUlong;
            int count = iterations;
            for (int i = 0; i < count; i++)
            {
                OrUnsafeCrispFuzzyVector4f3NoStatic(tmp, tmp2);
            }
        }

        /// <summary>
        /// The Vector4f OR benchmark for fuzzy and crisp bit strings - unsafe,
        /// precomputed version with shifts in creation of Vector4f, 
        /// no allocation of new memory in cycles
        /// </summary>
        [Benchmark]
        public static void UnsafeOrCrispFuzzyVector4fPrecomputedShiftNoAllocation()
        {
            Vector4f[] tmp = stringVector4f;
            ulong[] tmp2 = stringUlong;
            int count = iterations;
            for (int i = 0; i < count; i++)
            {
                OrUnsafeCrispFuzzyVector4f3NoAllocation(tmp, tmp2);
            }
        }

        /// <summary>
        /// The Vector4f OR benchmark for fuzzy and crisp bit strings - unsafe,
        /// precomputed version with shifts in creation of Vector4f, 
        /// no allocation of new memory in cycles + no static variables
        /// </summary>
        [Benchmark]
        public static void UnsafeOrCrispFuzzyVector4fPrecomputedShiftNoStaticNoAllocation()
        {
            Vector4f[] tmp = stringVector4f;
            ulong[] tmp2 = stringUlong;
            int count = iterations;
            for (int i = 0; i < count; i++)
            {
                OrUnsafeCrispFuzzyVector4f3NoAllocationNoStatic(tmp, tmp2);
            }
        }

        #endregion

        #region Real implementation methods

        /// <summary>
        /// The safe implementation of the crisp disjunction
        /// </summary>
        /// <param name="operand1">1. operand</param>
        /// <param name="operand2">2. operand</param>
        /// <returns>Disjunction result</returns>
        static ulong[] OrSafeCrisp(ulong[] operand1, ulong[] operand2)
        {
            for (int i = 0; i < operand1.Length; i++)
            {
                operand1[i] |= operand2[i];
            }
            return operand1;
        }

        /// <summary>
        /// The unsafe implementation of the crisp disjunction
        /// </summary>
        /// <param name="operand1">1. operand</param>
        /// <param name="operand2">2. operand</param>
        /// <returns>Disjunction result</returns>
        static unsafe ulong[] OrUnsafeCrisp(ulong[] operand1, ulong[] operand2)
        {
            fixed (ulong* destPin = operand1, sourcePin = operand2)
            {
                ulong* destPtr = destPin,
                    sourcePtr = sourcePin,
                    stopPtr = destPin + operand1.Length;

                while (destPtr < stopPtr)
                {
                    *destPtr++ |= *sourcePtr++;
                }
            }

            return operand1;
        }

        /// <summary>
        /// The safe implementation of the disjunction with Vector4f
        /// </summary>
        /// <param name="operand1">1. operand</param>
        /// <param name="operand2">2. operand</param>
        /// <returns>Disjunction result</returns>
        static Vector4f[] OrSafe4f(Vector4f[] operand1, Vector4f[] operand2)
        {
            for (int i = 0; i < operand1.Length; i++)
            {
                operand1[i] = operand1[i] + operand2[i] - operand1[i] * operand2[i];
            }
            return operand1;
        }

        /// <summary>
        /// The unsafe implementation of the disjunction with Vector4f
        /// </summary>
        /// <param name="operand1">1. operand</param>
        /// <param name="operand2">2. operand</param>
        /// <returns>Disjunction result</returns>
        static unsafe Vector4f[] OrUnsafe4f(Vector4f[] operand1, Vector4f[] operand2)
        {
            fixed (Vector4f* thisPin = operand1, sourcePin = operand2)
            {
                Vector4f* currentPtr = thisPin, sourcePtr = sourcePin,
                    stopPtr = thisPin + operand1.Length;
                while (currentPtr < stopPtr)
                {
                    *currentPtr = *sourcePtr + *currentPtr - *sourcePtr * (*currentPtr);
                    currentPtr++;
                    sourcePtr++;
                }
            }
            return operand1;
        }

        /// <summary>
        /// The safe implementation of the disjunction with array of floats
        /// </summary>
        /// <param name="operand1">1. operand</param>
        /// <param name="operand2">2. operand</param>
        /// <returns>Disjunction result</returns>
        static float[] OrSafeFloat(float[] operand1, float[] operand2)
        {
            for (int i = 0; i < operand1.Length; i++)
            {
                operand1[i] = operand2[i] + operand1[i] - operand1[i] * operand2[i];
            }
            return operand1;
        }

        /// <summary>
        /// The unsafe implementation of the disjunction with array
        /// of floats
        /// </summary>
        /// <param name="operand1">1. operand</param>
        /// <param name="operand2">2. operand</param>
        /// <returns>Disjunction result</returns>
        static unsafe float[] OrUnsafeFloat(float[] operand1, float[] operand2)
        {
            fixed (float* thisPin = operand1, sourcePin = operand2)
            {
                float* currentPtr = thisPin, sourcePtr = sourcePin,
                    stopPtr = thisPin + operand1.Length;
                while (currentPtr < stopPtr)
                {
                    *currentPtr = *sourcePtr + *currentPtr - (*sourcePtr) * (*currentPtr);
                    currentPtr++;
                    sourcePtr++;
                }
            }
            return operand1;
        }

        /// <summary>
        /// The safe implementation of disjunction between fuzzy and crisp 
        /// bit string using Vector4f. Algorithm 1 - naive
        /// </summary>
        /// <param name="operand1">1. operand (fuzzy)</param>
        /// <param name="operand2">2. operand (crisp)</param>
        /// <returns>Disjunction result (fuzzy)</returns>
        static Vector4f[] OrSafeCrispFuzzyVector4f1(Vector4f[] operand1, ulong[] operand2)
        {
            for (int i = 0; i < operand1.Length; i++)
            {
                Vector4f tmp = new Vector4f(
                    Convert.ToSingle((operand2[4 * i / 64] & (_one << (4 * i % 64))) > 0),
                    Convert.ToSingle((operand2[(4 * i + 1) / 64] & (_one << ((4 * i + 1) % 64))) > 0),
                    Convert.ToSingle((operand2[(4 * i + 2) / 64] & (_one << ((4 * i + 2) % 64))) > 0),
                    Convert.ToSingle((operand2[(4 * i + 3) / 64] & (_one << ((4 * i + 3) % 64))) > 0));
                operand1[i] = tmp + operand1[i] - tmp*operand1[i];
            }
            return operand1;
        }

        /// <summary>
        /// The safe implementation of disjunction between fuzzy and crisp 
        /// bit strings using array of floats
        /// </summary>
        /// <param name="operand1">1. operand (fuzzy)</param>
        /// <param name="operand2">2. operand (crisp)</param>
        /// <returns>Disjunction result (fuzzy)</returns>
        static float[] OrSafeCrispFuzzyFloat(float[] operand1, ulong[] operand2)
        {
            for (int i = 0; i < operand1.Length; i++)
            {
                Single res = Convert.ToSingle((operand2[i / 64] & (_one << (i % 64))) > 0);
                operand1[i] = res + operand1[i] - res * operand1[i];
            }
            return operand1;
        }

        /// <summary>
        /// The safe implementation of disjunction between fuzzy and crisp 
        /// bit strings using array of floats + less accessing the ulong field
        /// </summary>
        /// <param name="operand1">1. operand (fuzzy)</param>
        /// <param name="operand2">2. operand (crisp)</param>
        /// <returns>Disjunction result (fuzzy)</returns>
        static float[] AndSafeCrispFuzzyFloat2(float[] operand1, ulong[] operand2)
        {
            int fltPtr = 0;
            ulong tmpul;
            float tmpfl;
            for (int i = 0; i < operand2.Length; i++)
            {
                tmpul = operand2[i];

                tmpfl = Convert.ToSingle(tmp);
                operand1[fltPtr] = operand1[fltPtr] + tmpfl - operand1 * tmpfl;
                fltPtr++;
                tmpfl = Convert.ToSingle(tmp >> 1);
                operand1[fltPtr] = operand1[fltPtr] + tmpfl - operand1 * tmpfl;
                fltPtr++;
                tmpfl = Convert.ToSingle(tmp >> 2);
                operand1[fltPtr] = operand1[fltPtr] + tmpfl - operand1 * tmpfl;
                fltPtr++;
                tmpfl = Convert.ToSingle(tmp >> 3);
                operand1[fltPtr] = operand1[fltPtr] + tmpfl - operand1 * tmpfl;
                fltPtr++;
                //ADT az do 63
            }
        }

        /// <summary>
        /// The safe implementation of disjunction between fuzy and crisp
        /// bit strings using Vector4f and precomputed uint and float vectors.
        /// The algoritm presumes the number of elements both in ulong and Vector4f
        /// fields to be a multiple of 64.
        /// </summary>
        /// <param name="operand1">1. operand (fuzzy)</param>
        /// <param name="operand2">2. operand (crisp)</param>
        /// <returns>Disjunction result (fuzzy)</returns>
        public static Vector4f[] OrSafeCrispFuzzyVector4f2(Vector4f[] operand1, ulong[] operand2)
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
                tmpF = (new Vector4f(tmpUi.X, tmpUi.Y, tmpUi.Z, tmpUi.W))/sf1;
                operand1[vPtr] = tmpF + operand1[vPtr] - tmpF * operand1[vPtr];
                vPtr++;
                tmpUi = vect & sui2;
                tmpF = (new Vector4f(tmpUi.X, tmpUi.Y, tmpUi.Z, tmpUi.W))/sf2;
                operand1[vPtr] = tmpF + operand1[vPtr] - tmpF * operand1[vPtr];
                vPtr++;
                tmpUi = vect & sui3;
                tmpF = (new Vector4f(tmpUi.X, tmpUi.Y, tmpUi.Z, tmpUi.W))/sf3;
                operand1[vPtr] = tmpF + operand1[vPtr] - tmpF * operand1[vPtr];
                vPtr++;
                tmpUi = vect & sui4;
                tmpF = (new Vector4f(tmpUi.X, tmpUi.Y, tmpUi.Z, tmpUi.W))/sf4;
                operand1[vPtr] = tmpF + operand1[vPtr] - tmpF * operand1[vPtr];
                vPtr++;
                tmpUi = vect & sui5;
                tmpF = (new Vector4f(tmpUi.X, tmpUi.Y, tmpUi.Z, tmpUi.W))/sf5;
                operand1[vPtr] = tmpF + operand1[vPtr] - tmpF * operand1[vPtr];
                vPtr++;
                tmpUi = vect & sui6;
                tmpF = (new Vector4f(tmpUi.X, tmpUi.Y, tmpUi.Z, tmpUi.W))/sf6;
                operand1[vPtr] = tmpF + operand1[vPtr] - tmpF * operand1[vPtr];
                vPtr++;
                tmpUi = vect & sui7;
                tmpF = (new Vector4f(tmpUi.X, tmpUi.Y, tmpUi.Z, tmpUi.W))/sf7;
                operand1[vPtr] = tmpF + operand1[vPtr] - tmpF * operand1[vPtr];
                vPtr++;
                tmpUi = vect & sui8;
                tmpF = (new Vector4f(tmpUi.X, tmpUi.Y, tmpUi.Z, tmpUi.W))/sf8;
                operand1[vPtr] = tmpF + operand1[vPtr] - tmpF * operand1[vPtr];
                vPtr++;

                part = (uint)(operand2[i] >> 32);
                vect = new Vector4ui(part, part, part, part);

                tmpUi = vect & sui1;
                tmpF = (new Vector4f(tmpUi.X, tmpUi.Y, tmpUi.Z, tmpUi.W))/sf1;
                operand1[vPtr] = tmpF + operand1[vPtr] - tmpF * operand1[vPtr];
                vPtr++;
                tmpUi = vect & sui2;
                tmpF = (new Vector4f(tmpUi.X, tmpUi.Y, tmpUi.Z, tmpUi.W))/sf2;
                operand1[vPtr] = tmpF + operand1[vPtr] - tmpF * operand1[vPtr];
                vPtr++;
                tmpUi = vect & sui3;
                tmpF = (new Vector4f(tmpUi.X, tmpUi.Y, tmpUi.Z, tmpUi.W))/sf3;
                operand1[vPtr] = tmpF + operand1[vPtr] - tmpF * operand1[vPtr];
                vPtr++;
                tmpUi = vect & sui4;
                tmpF = (new Vector4f(tmpUi.X, tmpUi.Y, tmpUi.Z, tmpUi.W))/sf4;
                operand1[vPtr] = tmpF + operand1[vPtr] - tmpF * operand1[vPtr];
                vPtr++;
                tmpUi = vect & sui5;
                tmpF = (new Vector4f(tmpUi.X, tmpUi.Y, tmpUi.Z, tmpUi.W))/sf5;
                operand1[vPtr] = tmpF + operand1[vPtr] - tmpF * operand1[vPtr];
                vPtr++;
                tmpUi = vect & sui6;
                tmpF = (new Vector4f(tmpUi.X, tmpUi.Y, tmpUi.Z, tmpUi.W))/sf6;
                operand1[vPtr] = tmpF + operand1[vPtr] - tmpF * operand1[vPtr];
                vPtr++;
                tmpUi = vect & sui7;
                tmpF = (new Vector4f(tmpUi.X, tmpUi.Y, tmpUi.Z, tmpUi.W))/sf7;
                operand1[vPtr] = tmpF + operand1[vPtr] - tmpF * operand1[vPtr];
                vPtr++;
                tmpUi = vect & sui8;
                tmpF = (new Vector4f(tmpUi.X, tmpUi.Y, tmpUi.Z, tmpUi.W))/sf8;
                operand1[vPtr] = tmpF + operand1[vPtr] - tmpF * operand1[vPtr];
                vPtr++;
            }
            return operand1;
        }

        /// <summary>
        /// The safe implementation of disjunction between fuzy and crisp
        /// bit strings using Vector4f and precomputed uint and shifting creation of the Vector4f
        /// The algoritm presumes the number of elements both in ulong and Vector4f
        /// fields to be a multiple of 64.
        /// </summary>
        /// <param name="operand1">1. operand (fuzzy)</param>
        /// <param name="operand2">2. operand (crisp)</param>
        /// <returns>Disjunction result (fuzzy)</returns>
        public static Vector4f[] OrSafeCrispFuzzyVector4f3(Vector4f[] operand1, ulong[] operand2)
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
                operand1[vPtr] = tmpF + operand1[vPtr] - tmpF * operand1[vPtr];
                vPtr++;
                tmpUi = vect & sui2;
                tmpF = new Vector4f(tmpUi.X >> 4, tmpUi.Y >> 5, tmpUi.Z >> 6, tmpUi.W >> 7);
                operand1[vPtr] = tmpF + operand1[vPtr] - tmpF * operand1[vPtr];
                vPtr++;
                tmpUi = vect & sui3;
                tmpF = new Vector4f(tmpUi.X >> 8, tmpUi.Y >> 9, tmpUi.Z >> 10, tmpUi.W >> 11);
                operand1[vPtr] = tmpF + operand1[vPtr] - tmpF * operand1[vPtr];
                vPtr++;
                tmpUi = vect & sui4;
                tmpF = new Vector4f(tmpUi.X >> 12, tmpUi.Y >> 13, tmpUi.Z >> 14, tmpUi.W >> 15);
                operand1[vPtr] = tmpF + operand1[vPtr] - tmpF * operand1[vPtr];
                vPtr++;
                tmpUi = vect & sui5;
                tmpF = new Vector4f(tmpUi.X >> 16, tmpUi.Y >> 17, tmpUi.Z >> 18, tmpUi.W >> 19);
                operand1[vPtr] = tmpF + operand1[vPtr] - tmpF * operand1[vPtr];
                vPtr++;
                tmpUi = vect & sui6;
                tmpF = new Vector4f(tmpUi.X >> 20, tmpUi.Y >> 21, tmpUi.Z >> 22, tmpUi.W >> 23);
                operand1[vPtr] = tmpF + operand1[vPtr] - tmpF * operand1[vPtr];
                vPtr++;
                tmpUi = vect & sui7;
                tmpF = new Vector4f(tmpUi.X >> 24, tmpUi.Y >> 25, tmpUi.Z >> 26, tmpUi.W >> 27);
                operand1[vPtr] = tmpF + operand1[vPtr] - tmpF * operand1[vPtr];
                vPtr++;
                tmpUi = vect & sui8;
                tmpF = new Vector4f(tmpUi.X >> 28, tmpUi.Y >> 29, tmpUi.Z >> 30, tmpUi.W >> 31);
                operand1[vPtr] = tmpF + operand1[vPtr] - tmpF * operand1[vPtr];
                vPtr++;

                part = (uint)(operand2[i] >> 32);
                vect = new Vector4ui(part, part, part, part);

                tmpUi = vect & sui1;
                tmpF = new Vector4f(tmpUi.X, tmpUi.Y >> 1, tmpUi.Z >> 2, tmpUi.W >> 3);
                operand1[vPtr] = tmpF + operand1[vPtr] - tmpF * operand1[vPtr];
                vPtr++;
                tmpUi = vect & sui2;
                tmpF = new Vector4f(tmpUi.X >> 4, tmpUi.Y >> 5, tmpUi.Z >> 6, tmpUi.W >> 7);
                operand1[vPtr] = tmpF + operand1[vPtr] - tmpF * operand1[vPtr];
                vPtr++;
                tmpUi = vect & sui3;
                tmpF = new Vector4f(tmpUi.X >> 8, tmpUi.Y >> 9, tmpUi.Z >> 10, tmpUi.W >> 11);
                operand1[vPtr] = tmpF + operand1[vPtr] - tmpF * operand1[vPtr];
                vPtr++;
                tmpUi = vect & sui4;
                tmpF = new Vector4f(tmpUi.X >> 12, tmpUi.Y >> 13, tmpUi.Z >> 14, tmpUi.W >> 15);
                operand1[vPtr] = tmpF + operand1[vPtr] - tmpF * operand1[vPtr];
                vPtr++;
                tmpUi = vect & sui5;
                tmpF = new Vector4f(tmpUi.X >> 16, tmpUi.Y >> 17, tmpUi.Z >> 18, tmpUi.W >> 19);
                operand1[vPtr] = tmpF + operand1[vPtr] - tmpF * operand1[vPtr];
                vPtr++;
                tmpUi = vect & sui6;
                tmpF = new Vector4f(tmpUi.X >> 20, tmpUi.Y >> 21, tmpUi.Z >> 22, tmpUi.W >> 23);
                operand1[vPtr] = tmpF + operand1[vPtr] - tmpF * operand1[vPtr];
                vPtr++;
                tmpUi = vect & sui7;
                tmpF = new Vector4f(tmpUi.X >> 24, tmpUi.Y >> 25, tmpUi.Z >> 26, tmpUi.W >> 27);
                operand1[vPtr] = tmpF + operand1[vPtr] - tmpF * operand1[vPtr];
                vPtr++;
                tmpUi = vect & sui8;
                tmpF = new Vector4f(tmpUi.X >> 28, tmpUi.Y >> 29, tmpUi.Z >> 30, tmpUi.W >> 31);
                operand1[vPtr] = tmpF + operand1[vPtr] - tmpF * operand1[vPtr];
                vPtr++;
            }
            return operand1;
        }

        /// <summary>
        /// The safe implementation of disjunction between fuzy and crisp
        /// bit strings using Vector4f and precomputed uint and shifting creation of the Vector4f
        /// The algoritm presumes the number of elements both in ulong and Vector4f
        /// fields to be a multiple of 64. The algorithm
        /// doesn't use allocation of new memory in computation of individual Vector4f's,
        /// it assigns to only one Vector4f previously defined. 
        /// </summary>
        /// <param name="operand1">1. operand (fuzzy)</param>
        /// <param name="operand2">2. operand (crisp)</param>
        /// <returns>Disjunction result (fuzzy)</returns>
        public static Vector4f[] OrSafeCrispFuzzyVector4f3NoAllocation(Vector4f[] operand1, ulong[] operand2)
        {
            Vector4f tmpF = new Vector4f();
            Vector4ui tmpUi;
            Vector4ui vect;
            uint part;
            int vPtr = 0;

            for (int i = 0; i < operand2.Length; i++)
            {
                part = (uint)operand2[i]; //last 8 bits
                vect = new Vector4ui(part, part, part, part);

                tmpUi = vect & sui1;
                tmpF.X = tmpUi.X; tmpF.Y = (tmpUi.Y >> 1); tmpF.Z = (tmpUi.Z >> 2); tmpF.W = (tmpUi.W >> 3);
                operand1[vPtr] = tmpF + operand1[vPtr] - tmpF * operand1[vPtr];
                vPtr++;
                tmpUi = vect & sui2;
                tmpF.X = (tmpUi.X >> 4); tmpF.Y = (tmpUi.Y >> 5); tmpF.Z = (tmpUi.Z >> 6); tmpF.W = (tmpUi.W >> 7);
                operand1[vPtr] = tmpF + operand1[vPtr] - tmpF * operand1[vPtr];
                vPtr++;
                tmpUi = vect & sui3;
                tmpF.X = (tmpUi.X >> 8); tmpF.Y = (tmpUi.Y >> 9); tmpF.Z = (tmpUi.Z >> 10); tmpF.W = (tmpUi.W >> 11);
                operand1[vPtr] = tmpF + operand1[vPtr] - tmpF * operand1[vPtr];
                vPtr++;
                tmpUi = vect & sui4;
                tmpF.X = (tmpUi.X >> 12); tmpF.Y = (tmpUi.Y >> 13); tmpF.Z = (tmpUi.Z >> 14); tmpF.W = (tmpUi.W >> 15);
                operand1[vPtr] = tmpF + operand1[vPtr] - tmpF * operand1[vPtr];
                vPtr++;
                tmpUi = vect & sui5;
                tmpF.X = (tmpUi.X >> 16); tmpF.Y = (tmpUi.Y >> 17); tmpF.Z = (tmpUi.Z >> 18); tmpF.W = (tmpUi.W >> 19);
                operand1[vPtr] = tmpF + operand1[vPtr] - tmpF * operand1[vPtr];
                vPtr++;
                tmpUi = vect & sui6;
                tmpF.X = (tmpUi.X >> 20); tmpF.Y = (tmpUi.Y >> 21); tmpF.Z = (tmpUi.Z >> 22); tmpF.W = (tmpUi.W >> 23);
                operand1[vPtr] = tmpF + operand1[vPtr] - tmpF * operand1[vPtr];
                vPtr++;
                tmpUi = vect & sui7;
                tmpF.X = (tmpUi.X >> 24); tmpF.Y = (tmpUi.Y >> 25); tmpF.Z = (tmpUi.Z >> 26); tmpF.W = (tmpUi.W >> 27);
                operand1[vPtr] = tmpF + operand1[vPtr] - tmpF * operand1[vPtr];
                vPtr++;
                tmpUi = vect & sui8;
                tmpF.X = (tmpUi.X >> 28); tmpF.Y = (tmpUi.Y >> 29); tmpF.Z = (tmpUi.Z >> 30); tmpF.W = (tmpUi.W >> 31);
                operand1[vPtr] = tmpF + operand1[vPtr] - tmpF * operand1[vPtr];
                vPtr++;

                part = (uint)(operand2[i] >> 32);
                vect = new Vector4ui(part, part, part, part);

                tmpUi = vect & sui1;
                tmpF.X = tmpUi.X; tmpF.Y = (tmpUi.Y >> 1); tmpF.Z = (tmpUi.Z >> 2); tmpF.W = (tmpUi.W >> 3);
                operand1[vPtr] = tmpF + operand1[vPtr] - tmpF * operand1[vPtr];
                vPtr++;
                tmpUi = vect & sui2;
                tmpF.X = (tmpUi.X >> 4); tmpF.Y = (tmpUi.Y >> 5); tmpF.Z = (tmpUi.Z >> 6); tmpF.W = (tmpUi.W >> 7);
                operand1[vPtr] = tmpF + operand1[vPtr] - tmpF * operand1[vPtr];
                vPtr++;
                tmpUi = vect & sui3;
                tmpF.X = (tmpUi.X >> 8); tmpF.Y = (tmpUi.Y >> 9); tmpF.Z = (tmpUi.Z >> 10); tmpF.W = (tmpUi.W >> 11);
                operand1[vPtr] = tmpF + operand1[vPtr] - tmpF * operand1[vPtr];
                vPtr++;
                tmpUi = vect & sui4;
                tmpF.X = (tmpUi.X >> 12); tmpF.Y = (tmpUi.Y >> 13); tmpF.Z = (tmpUi.Z >> 14); tmpF.W = (tmpUi.W >> 15);
                operand1[vPtr] = tmpF + operand1[vPtr] - tmpF * operand1[vPtr];
                vPtr++;
                tmpUi = vect & sui5;
                tmpF.X = (tmpUi.X >> 16); tmpF.Y = (tmpUi.Y >> 17); tmpF.Z = (tmpUi.Z >> 18); tmpF.W = (tmpUi.W >> 19);
                operand1[vPtr] = tmpF + operand1[vPtr] - tmpF * operand1[vPtr];
                vPtr++;
                tmpUi = vect & sui6;
                tmpF.X = (tmpUi.X >> 20); tmpF.Y = (tmpUi.Y >> 21); tmpF.Z = (tmpUi.Z >> 22); tmpF.W = (tmpUi.W >> 23);
                operand1[vPtr] = tmpF + operand1[vPtr] - tmpF * operand1[vPtr];
                vPtr++;
                tmpUi = vect & sui7;
                tmpF.X = (tmpUi.X >> 24); tmpF.Y = (tmpUi.Y >> 25); tmpF.Z = (tmpUi.Z >> 26); tmpF.W = (tmpUi.W >> 27);
                operand1[vPtr] = tmpF + operand1[vPtr] - tmpF * operand1[vPtr];
                vPtr++;
                tmpUi = vect & sui8;
                tmpF.X = (tmpUi.X >> 28); tmpF.Y = (tmpUi.Y >> 29); tmpF.Z = (tmpUi.Z >> 30); tmpF.W = (tmpUi.W >> 31);
                operand1[vPtr] = tmpF + operand1[vPtr] - tmpF * operand1[vPtr];
                vPtr++;
            }
            return operand1;
        }

        /// <summary>
        /// The safe implementation of disjunction between fuzy and crisp
        /// bit strings using Vector4f and precomputed uint and shifting creation of the Vector4f
        /// The algoritm presumes the number of elements both in ulong and Vector4f
        /// fields to be a multiple of 64. The algorithm
        /// doesn't use allocation of new memory in computation of individual Vector4f's,
        /// it assigns to only one Vector4f previously defined + does not use static variables
        /// </summary>
        /// <param name="operand1">1. operand (fuzzy)</param>
        /// <param name="operand2">2. operand (crisp)</param>
        /// <returns>Disjunction result (fuzzy)</returns>
        public static Vector4f[] OrSafeCrispFuzzyVector4f3NoStaticNoAllocation(Vector4f[] operand1, ulong[] operand2)
        {
            Vector4f tmpF = new Vector4f();
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
                tmpF.X = tmpUi.X; tmpF.Y = (tmpUi.Y >> 1); tmpF.Z = (tmpUi.Z >> 2); tmpF.W = (tmpUi.W >> 3);
                operand1[vPtr] = tmpF + operand1[vPtr] - tmpF * operand1[vPtr];
                vPtr++;
                tmpUi = vect & ui2;
                tmpF.X = (tmpUi.X >> 4); tmpF.Y = (tmpUi.Y >> 5); tmpF.Z = (tmpUi.Z >> 6); tmpF.W = (tmpUi.W >> 7);
                operand1[vPtr] = tmpF + operand1[vPtr] - tmpF * operand1[vPtr];
                vPtr++;
                tmpUi = vect & ui3;
                tmpF.X = (tmpUi.X >> 8); tmpF.Y = (tmpUi.Y >> 9); tmpF.Z = (tmpUi.Z >> 10); tmpF.W = (tmpUi.W >> 11);
                operand1[vPtr] = tmpF + operand1[vPtr] - tmpF * operand1[vPtr];
                vPtr++;
                tmpUi = vect & ui4;
                tmpF.X = (tmpUi.X >> 12); tmpF.Y = (tmpUi.Y >> 13); tmpF.Z = (tmpUi.Z >> 14); tmpF.W = (tmpUi.W >> 15);
                operand1[vPtr] = tmpF + operand1[vPtr] - tmpF * operand1[vPtr];
                vPtr++;
                tmpUi = vect & ui5;
                tmpF.X = (tmpUi.X >> 16); tmpF.Y = (tmpUi.Y >> 17); tmpF.Z = (tmpUi.Z >> 18); tmpF.W = (tmpUi.W >> 19);
                operand1[vPtr] = tmpF + operand1[vPtr] - tmpF * operand1[vPtr];
                vPtr++;
                tmpUi = vect & ui6;
                tmpF.X = (tmpUi.X >> 20); tmpF.Y = (tmpUi.Y >> 21); tmpF.Z = (tmpUi.Z >> 22); tmpF.W = (tmpUi.W >> 23);
                operand1[vPtr] = tmpF + operand1[vPtr] - tmpF * operand1[vPtr];
                vPtr++;
                tmpUi = vect & ui7;
                tmpF.X = (tmpUi.X >> 24); tmpF.Y = (tmpUi.Y >> 25); tmpF.Z = (tmpUi.Z >> 26); tmpF.W = (tmpUi.W >> 27);
                operand1[vPtr] = tmpF + operand1[vPtr] - tmpF * operand1[vPtr];
                vPtr++;
                tmpUi = vect & ui8;
                tmpF.X = (tmpUi.X >> 28); tmpF.Y = (tmpUi.Y >> 29); tmpF.Z = (tmpUi.Z >> 30); tmpF.W = (tmpUi.W >> 31);
                operand1[vPtr] = tmpF + operand1[vPtr] - tmpF * operand1[vPtr];
                vPtr++;

                part = (uint)(operand2[i] >> 32);
                vect = new Vector4ui(part, part, part, part);

                tmpUi = vect & ui1;
                tmpF.X = tmpUi.X; tmpF.Y = (tmpUi.Y >> 1); tmpF.Z = (tmpUi.Z >> 2); tmpF.W = (tmpUi.W >> 3);
                operand1[vPtr] = tmpF + operand1[vPtr] - tmpF * operand1[vPtr];
                vPtr++;
                tmpUi = vect & ui2;
                tmpF.X = (tmpUi.X >> 4); tmpF.Y = (tmpUi.Y >> 5); tmpF.Z = (tmpUi.Z >> 6); tmpF.W = (tmpUi.W >> 7);
                operand1[vPtr] = tmpF + operand1[vPtr] - tmpF * operand1[vPtr];
                vPtr++;
                tmpUi = vect & ui3;
                tmpF.X = (tmpUi.X >> 8); tmpF.Y = (tmpUi.Y >> 9); tmpF.Z = (tmpUi.Z >> 10); tmpF.W = (tmpUi.W >> 11);
                operand1[vPtr] = tmpF + operand1[vPtr] - tmpF * operand1[vPtr];
                vPtr++;
                tmpUi = vect & ui4;
                tmpF.X = (tmpUi.X >> 12); tmpF.Y = (tmpUi.Y >> 13); tmpF.Z = (tmpUi.Z >> 14); tmpF.W = (tmpUi.W >> 15);
                operand1[vPtr] = tmpF + operand1[vPtr] - tmpF * operand1[vPtr];
                vPtr++;
                tmpUi = vect & ui5;
                tmpF.X = (tmpUi.X >> 16); tmpF.Y = (tmpUi.Y >> 17); tmpF.Z = (tmpUi.Z >> 18); tmpF.W = (tmpUi.W >> 19);
                operand1[vPtr] = tmpF + operand1[vPtr] - tmpF * operand1[vPtr];
                vPtr++;
                tmpUi = vect & ui6;
                tmpF.X = (tmpUi.X >> 20); tmpF.Y = (tmpUi.Y >> 21); tmpF.Z = (tmpUi.Z >> 22); tmpF.W = (tmpUi.W >> 23);
                operand1[vPtr] = tmpF + operand1[vPtr] - tmpF * operand1[vPtr];
                vPtr++;
                tmpUi = vect & ui7;
                tmpF.X = (tmpUi.X >> 24); tmpF.Y = (tmpUi.Y >> 25); tmpF.Z = (tmpUi.Z >> 26); tmpF.W = (tmpUi.W >> 27);
                operand1[vPtr] = tmpF + operand1[vPtr] - tmpF * operand1[vPtr];
                vPtr++;
                tmpUi = vect & ui8;
                tmpF.X = (tmpUi.X >> 28); tmpF.Y = (tmpUi.Y >> 29); tmpF.Z = (tmpUi.Z >> 30); tmpF.W = (tmpUi.W >> 31);
                operand1[vPtr] = tmpF + operand1[vPtr] - tmpF * operand1[vPtr];
                vPtr++;
            }
            return operand1;
        }

        /// <summary>
        /// The safe implementation of disjunction between fuzy and crisp
        /// bit strings using Vector4f and precomputed uint and shifting creation of the Vector4f
        /// The algoritm presumes the number of elements both in ulong and Vector4f
        /// fields to be a multiple of 64. No static variables used
        /// </summary>
        /// <param name="operand1">1. operand (fuzzy)</param>
        /// <param name="operand2">2. operand (crisp)</param>
        /// <returns>Disjunction result (fuzzy)</returns>
        public static Vector4f[] OrSafeCrispFuzzyVector4f3NoStatic(Vector4f[] operand1, ulong[] operand2)
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
                operand1[vPtr] = tmpF + operand1[vPtr] - tmpF * operand1[vPtr];
                vPtr++;
                tmpUi = vect & ui2;
                tmpF = new Vector4f(tmpUi.X >> 4, tmpUi.Y >> 5, tmpUi.Z >> 6, tmpUi.W >> 7);
                operand1[vPtr] = tmpF + operand1[vPtr] - tmpF * operand1[vPtr];
                vPtr++;
                tmpUi = vect & ui3;
                tmpF = new Vector4f(tmpUi.X >> 8, tmpUi.Y >> 9, tmpUi.Z >> 10, tmpUi.W >> 11);
                operand1[vPtr] = tmpF + operand1[vPtr] - tmpF * operand1[vPtr];
                vPtr++;
                tmpUi = vect & ui4;
                tmpF = new Vector4f(tmpUi.X >> 12, tmpUi.Y >> 13, tmpUi.Z >> 14, tmpUi.W >> 15);
                operand1[vPtr] = tmpF + operand1[vPtr] - tmpF * operand1[vPtr];
                vPtr++;
                tmpUi = vect & ui5;
                tmpF = new Vector4f(tmpUi.X >> 16, tmpUi.Y >> 17, tmpUi.Z >> 18, tmpUi.W >> 19);
                operand1[vPtr] = tmpF + operand1[vPtr] - tmpF * operand1[vPtr];
                vPtr++;
                tmpUi = vect & ui6;
                tmpF = new Vector4f(tmpUi.X >> 20, tmpUi.Y >> 21, tmpUi.Z >> 22, tmpUi.W >> 23);
                operand1[vPtr] = tmpF + operand1[vPtr] - tmpF * operand1[vPtr];
                vPtr++;
                tmpUi = vect & ui7;
                tmpF = new Vector4f(tmpUi.X >> 24, tmpUi.Y >> 25, tmpUi.Z >> 26, tmpUi.W >> 27);
                operand1[vPtr] = tmpF + operand1[vPtr] - tmpF * operand1[vPtr];
                vPtr++;
                tmpUi = vect & ui8;
                tmpF = new Vector4f(tmpUi.X >> 28, tmpUi.Y >> 29, tmpUi.Z >> 30, tmpUi.W >> 31);
                operand1[vPtr] = tmpF + operand1[vPtr] - tmpF * operand1[vPtr];
                vPtr++;

                part = (uint)(operand2[i] >> 32);
                vect = new Vector4ui(part, part, part, part);

                tmpUi = vect & ui1;
                tmpF = new Vector4f(tmpUi.X, tmpUi.Y >> 1, tmpUi.Z >> 2, tmpUi.W >> 3);
                operand1[vPtr] = tmpF + operand1[vPtr] - tmpF * operand1[vPtr];
                vPtr++;
                tmpUi = vect & ui2;
                tmpF = new Vector4f(tmpUi.X >> 4, tmpUi.Y >> 5, tmpUi.Z >> 6, tmpUi.W >> 7);
                operand1[vPtr] = tmpF + operand1[vPtr] - tmpF * operand1[vPtr];
                vPtr++;
                tmpUi = vect & ui3;
                tmpF = new Vector4f(tmpUi.X >> 8, tmpUi.Y >> 9, tmpUi.Z >> 10, tmpUi.W >> 11);
                operand1[vPtr] = tmpF + operand1[vPtr] - tmpF * operand1[vPtr];
                vPtr++;
                tmpUi = vect & ui4;
                tmpF = new Vector4f(tmpUi.X >> 12, tmpUi.Y >> 13, tmpUi.Z >> 14, tmpUi.W >> 15);
                operand1[vPtr] = tmpF + operand1[vPtr] - tmpF * operand1[vPtr];
                vPtr++;
                tmpUi = vect & ui5;
                tmpF = new Vector4f(tmpUi.X >> 16, tmpUi.Y >> 17, tmpUi.Z >> 18, tmpUi.W >> 19);
                operand1[vPtr] = tmpF + operand1[vPtr] - tmpF * operand1[vPtr];
                vPtr++;
                tmpUi = vect & ui6;
                tmpF = new Vector4f(tmpUi.X >> 20, tmpUi.Y >> 21, tmpUi.Z >> 22, tmpUi.W >> 23);
                operand1[vPtr] = tmpF + operand1[vPtr] - tmpF * operand1[vPtr];
                vPtr++;
                tmpUi = vect & ui7;
                tmpF = new Vector4f(tmpUi.X >> 24, tmpUi.Y >> 25, tmpUi.Z >> 26, tmpUi.W >> 27);
                operand1[vPtr] = tmpF + operand1[vPtr] - tmpF * operand1[vPtr];
                vPtr++;
                tmpUi = vect & ui8;
                tmpF = new Vector4f(tmpUi.X >> 28, tmpUi.Y >> 29, tmpUi.Z >> 30, tmpUi.W >> 31);
                operand1[vPtr] = tmpF + operand1[vPtr] - tmpF * operand1[vPtr];
                vPtr++;
            }
            return operand1;
        }

        /// <summary>
        /// The unsafe implementation of disjunction between fuzy and crisp
        /// bit strings using Vector4f and precomputed uint and float vectors.
        /// The algoritm presumes the number of elements both in ulong and Vector4f
        /// fields to be a multiple of 64.
        /// </summary>
        /// <param name="operand1">1. operand (fuzzy)</param>
        /// <param name="operand2">2. operand (crisp)</param>
        /// <returns>Disjunction result (fuzzy)</returns>
        public unsafe static Vector4f[] OrUnsafeCrispFuzzyVector4f2(Vector4f[] operand1, ulong[] operand2)
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
                        tmpF = (new Vector4f(tmpUi.X, tmpUi.Y, tmpUi.Z, tmpUi.W)) / sf1;
                        *ptrV = tmpF + *ptrV - tmpF* (*ptrV);
                        ptrV++;
                        tmpUi = vect & sui2;
                        tmpF = (new Vector4f(tmpUi.X, tmpUi.Y, tmpUi.Z, tmpUi.W)) / sf2;
                        *ptrV = tmpF + *ptrV - tmpF * (*ptrV);
                        ptrV++;
                        tmpUi = vect & sui3;
                        tmpF = (new Vector4f(tmpUi.X, tmpUi.Y, tmpUi.Z, tmpUi.W)) / sf3;
                        *ptrV = tmpF + *ptrV - tmpF * (*ptrV);
                        ptrV++;
                        tmpUi = vect & sui4;
                        tmpF = (new Vector4f(tmpUi.X, tmpUi.Y, tmpUi.Z, tmpUi.W)) / sf4;
                        *ptrV = tmpF + *ptrV - tmpF * (*ptrV);
                        ptrV++;
                        tmpUi = vect & sui5;
                        tmpF = (new Vector4f(tmpUi.X, tmpUi.Y, tmpUi.Z, tmpUi.W)) / sf5;
                        *ptrV = tmpF + *ptrV - tmpF * (*ptrV);
                        ptrV++;
                        tmpUi = vect & sui6;
                        tmpF = (new Vector4f(tmpUi.X, tmpUi.Y, tmpUi.Z, tmpUi.W)) / sf6;
                        *ptrV = tmpF + *ptrV - tmpF * (*ptrV);
                        ptrV++;
                        tmpUi = vect & sui7;
                        tmpF = (new Vector4f(tmpUi.X, tmpUi.Y, tmpUi.Z, tmpUi.W)) / sf7;
                        *ptrV = tmpF + *ptrV - tmpF * (*ptrV);
                        ptrV++;
                        tmpUi = vect & sui8;
                        tmpF = (new Vector4f(tmpUi.X, tmpUi.Y, tmpUi.Z, tmpUi.W)) / sf8;
                        *ptrV = tmpF + *ptrV - tmpF * (*ptrV);
                        ptrV++;

                        part = (uint)(*ptrUL >> 32);
                        vect = new Vector4ui(part, part, part, part);

                        tmpUi = vect & sui1;
                        tmpF = (new Vector4f(tmpUi.X, tmpUi.Y, tmpUi.Z, tmpUi.W)) / sf1;
                        *ptrV = tmpF + *ptrV - tmpF * (*ptrV);
                        ptrV++;
                        tmpUi = vect & sui2;
                        tmpF = (new Vector4f(tmpUi.X, tmpUi.Y, tmpUi.Z, tmpUi.W)) / sf2;
                        *ptrV = tmpF + *ptrV - tmpF * (*ptrV);
                        ptrV++;
                        tmpUi = vect & sui3;
                        tmpF = (new Vector4f(tmpUi.X, tmpUi.Y, tmpUi.Z, tmpUi.W)) / sf3;
                        *ptrV = tmpF + *ptrV - tmpF * (*ptrV);
                        ptrV++;
                        tmpUi = vect & sui4;
                        tmpF = (new Vector4f(tmpUi.X, tmpUi.Y, tmpUi.Z, tmpUi.W)) / sf4;
                        *ptrV = tmpF + *ptrV - tmpF * (*ptrV);
                        ptrV++;
                        tmpUi = vect & sui5;
                        tmpF = (new Vector4f(tmpUi.X, tmpUi.Y, tmpUi.Z, tmpUi.W)) / sf5;
                        *ptrV = tmpF + *ptrV - tmpF * (*ptrV);
                        ptrV++;
                        tmpUi = vect & sui6;
                        tmpF = (new Vector4f(tmpUi.X, tmpUi.Y, tmpUi.Z, tmpUi.W)) / sf6;
                        *ptrV = tmpF + *ptrV - tmpF * (*ptrV);
                        ptrV++;
                        tmpUi = vect & sui7;
                        tmpF = (new Vector4f(tmpUi.X, tmpUi.Y, tmpUi.Z, tmpUi.W)) / sf7;
                        *ptrV = tmpF + *ptrV - tmpF * (*ptrV);
                        ptrV++;
                        tmpUi = vect & sui8;
                        tmpF = (new Vector4f(tmpUi.X, tmpUi.Y, tmpUi.Z, tmpUi.W)) / sf8;
                        *ptrV = tmpF + *ptrV - tmpF * (*ptrV);
                        ptrV++;

                        ptrUL++;
                    }
                }
            }
            return operand1;
        }

        /// <summary>
        /// The unsafe implementation of disjunction between fuzy and crisp
        /// bit strings using Vector4f and precomputed uint and float vectors. The algorithm
        /// doesn't use allocation of new memory in computation of individual Vector4f's,
        /// it assigns to only one Vector4f previously defined. 
        /// The algoritm presumes the number of elements both in ulong and Vector4f
        /// fields to be a multiple of 64.
        /// </summary>
        /// <param name="operand1">1. operand (fuzzy)</param>
        /// <param name="operand2">2. operand (crisp)</param>
        /// <returns>Disjunction result (fuzzy)</returns>
        public unsafe static Vector4f[] OrUnsafeCrispFuzzyVector4f2NoAllocation(Vector4f[] operand1, ulong[] operand2)
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
                        tmpF /= sf1;
                        *ptrV = tmpF + *ptrV - tmpF * (*ptrV);
                        ptrV++;
                        tmpUi = vect & sui2;
                        tmpF.X = tmpUi.X; tmpF.Y = tmpUi.Y; tmpF.Z = tmpUi.Z; tmpF.W = tmpUi.W;
                        tmpF /= sf2;
                        *ptrV = tmpF + *ptrV - tmpF * (*ptrV);
                        ptrV++;
                        tmpUi = vect & sui3;
                        tmpF.X = tmpUi.X; tmpF.Y = tmpUi.Y; tmpF.Z = tmpUi.Z; tmpF.W = tmpUi.W;
                        tmpF /= sf3;
                        *ptrV = tmpF + *ptrV - tmpF * (*ptrV);
                        ptrV++;
                        tmpUi = vect & sui4;
                        tmpF.X = tmpUi.X; tmpF.Y = tmpUi.Y; tmpF.Z = tmpUi.Z; tmpF.W = tmpUi.W;
                        tmpF /= sf4;
                        *ptrV = tmpF + *ptrV - tmpF * (*ptrV);
                        ptrV++;
                        tmpUi = vect & sui5;
                        tmpF.X = tmpUi.X; tmpF.Y = tmpUi.Y; tmpF.Z = tmpUi.Z; tmpF.W = tmpUi.W;
                        tmpF /= sf5;
                        *ptrV = tmpF + *ptrV - tmpF * (*ptrV);
                        ptrV++;
                        tmpUi = vect & sui6;
                        tmpF.X = tmpUi.X; tmpF.Y = tmpUi.Y; tmpF.Z = tmpUi.Z; tmpF.W = tmpUi.W;
                        tmpF /= sf6;
                        *ptrV = tmpF + *ptrV - tmpF * (*ptrV);
                        ptrV++;
                        tmpUi = vect & sui7;
                        tmpF.X = tmpUi.X; tmpF.Y = tmpUi.Y; tmpF.Z = tmpUi.Z; tmpF.W = tmpUi.W;
                        tmpF /= sf7;
                        *ptrV = tmpF + *ptrV - tmpF * (*ptrV);
                        ptrV++;
                        tmpUi = vect & sui8;
                        tmpF.X = tmpUi.X; tmpF.Y = tmpUi.Y; tmpF.Z = tmpUi.Z; tmpF.W = tmpUi.W;
                        tmpF /= sf8;
                        *ptrV = tmpF + *ptrV - tmpF * (*ptrV);
                        ptrV++;

                        part = (uint)(*ptrUL >> 32);
                        vect = new Vector4ui(part, part, part, part);

                        tmpUi = vect & sui1;
                        tmpF.X = tmpUi.X; tmpF.Y = tmpUi.Y; tmpF.Z = tmpUi.Z; tmpF.W = tmpUi.W;
                        tmpF /= sf1;
                        *ptrV = tmpF + *ptrV - tmpF * (*ptrV);
                        ptrV++;
                        tmpUi = vect & sui2;
                        tmpF.X = tmpUi.X; tmpF.Y = tmpUi.Y; tmpF.Z = tmpUi.Z; tmpF.W = tmpUi.W;
                        tmpF /= sf2;
                        *ptrV = tmpF + *ptrV - tmpF * (*ptrV);
                        ptrV++;
                        tmpUi = vect & sui3;
                        tmpF.X = tmpUi.X; tmpF.Y = tmpUi.Y; tmpF.Z = tmpUi.Z; tmpF.W = tmpUi.W;
                        tmpF /= sf3;
                        *ptrV = tmpF + *ptrV - tmpF * (*ptrV);
                        ptrV++;
                        tmpUi = vect & sui4;
                        tmpF.X = tmpUi.X; tmpF.Y = tmpUi.Y; tmpF.Z = tmpUi.Z; tmpF.W = tmpUi.W;
                        tmpF /= sf4;
                        *ptrV = tmpF + *ptrV - tmpF * (*ptrV);
                        ptrV++;
                        tmpUi = vect & sui5;
                        tmpF.X = tmpUi.X; tmpF.Y = tmpUi.Y; tmpF.Z = tmpUi.Z; tmpF.W = tmpUi.W;
                        tmpF /= sf5;
                        *ptrV = tmpF + *ptrV - tmpF * (*ptrV);
                        ptrV++;
                        tmpUi = vect & sui6;
                        tmpF.X = tmpUi.X; tmpF.Y = tmpUi.Y; tmpF.Z = tmpUi.Z; tmpF.W = tmpUi.W;
                        tmpF /= sf6;
                        *ptrV = tmpF + *ptrV - tmpF * (*ptrV);
                        ptrV++;
                        tmpUi = vect & sui7;
                        tmpF.X = tmpUi.X; tmpF.Y = tmpUi.Y; tmpF.Z = tmpUi.Z; tmpF.W = tmpUi.W;
                        tmpF /= sf7;
                        *ptrV = tmpF + *ptrV - tmpF * (*ptrV);
                        ptrV++;
                        tmpUi = vect & sui8;
                        tmpF.X = tmpUi.X; tmpF.Y = tmpUi.Y; tmpF.Z = tmpUi.Z; tmpF.W = tmpUi.W;
                        tmpF /= sf8;
                        *ptrV = tmpF + *ptrV - tmpF * (*ptrV);
                        ptrV++;

                        ptrUL++;
                    }
                }
            }
            return operand1;
        }

        /// <summary>
        /// The unsafe implementation of disjunction between fuzy and crisp
        /// bit strings using Vector4f and precomputed uint and float vectors. The algorithm
        /// doesn't use allocation of new memory in computation of individual Vector4f's,
        /// it assigns to only one Vector4f previously defined. No static variables used
        /// The algoritm presumes the number of elements both in ulong and Vector4f
        /// fields to be a multiple of 64.
        /// </summary>
        /// <param name="operand1">1. operand (fuzzy)</param>
        /// <param name="operand2">2. operand (crisp)</param>
        /// <returns>Disjunction result (fuzzy)</returns>
        public unsafe static Vector4f[] OrUnsafeCrispFuzzyVector4f2NoAllocationNoStatic(Vector4f[] operand1, ulong[] operand2)
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
                        tmpF /= f1;
                        *ptrV = tmpF + *ptrV - tmpF * (*ptrV);
                        ptrV++;
                        tmpUi = vect & ui2;
                        tmpF.X = tmpUi.X; tmpF.Y = tmpUi.Y; tmpF.Z = tmpUi.Z; tmpF.W = tmpUi.W;
                        tmpF /= f2;
                        *ptrV = tmpF + *ptrV - tmpF * (*ptrV);
                        ptrV++;
                        tmpUi = vect & ui3;
                        tmpF.X = tmpUi.X; tmpF.Y = tmpUi.Y; tmpF.Z = tmpUi.Z; tmpF.W = tmpUi.W;
                        tmpF /= f3;
                        *ptrV = tmpF + *ptrV - tmpF * (*ptrV);
                        ptrV++;
                        tmpUi = vect & ui4;
                        tmpF.X = tmpUi.X; tmpF.Y = tmpUi.Y; tmpF.Z = tmpUi.Z; tmpF.W = tmpUi.W;
                        tmpF /= f4;
                        *ptrV = tmpF + *ptrV - tmpF * (*ptrV);
                        ptrV++;
                        tmpUi = vect & ui5;
                        tmpF.X = tmpUi.X; tmpF.Y = tmpUi.Y; tmpF.Z = tmpUi.Z; tmpF.W = tmpUi.W;
                        tmpF /= f5;
                        *ptrV = tmpF + *ptrV - tmpF * (*ptrV);
                        ptrV++;
                        tmpUi = vect & ui6;
                        tmpF.X = tmpUi.X; tmpF.Y = tmpUi.Y; tmpF.Z = tmpUi.Z; tmpF.W = tmpUi.W;
                        tmpF /= f6;
                        *ptrV = tmpF + *ptrV - tmpF * (*ptrV);
                        ptrV++;
                        tmpUi = vect & ui7;
                        tmpF.X = tmpUi.X; tmpF.Y = tmpUi.Y; tmpF.Z = tmpUi.Z; tmpF.W = tmpUi.W;
                        tmpF /= f7;
                        *ptrV = tmpF + *ptrV - tmpF * (*ptrV);
                        ptrV++;
                        tmpUi = vect & ui8;
                        tmpF.X = tmpUi.X; tmpF.Y = tmpUi.Y; tmpF.Z = tmpUi.Z; tmpF.W = tmpUi.W;
                        tmpF /= f8;
                        *ptrV = tmpF + *ptrV - tmpF * (*ptrV);
                        ptrV++;

                        part = (uint)(*ptrUL >> 32);
                        vect = new Vector4ui(part, part, part, part);

                        tmpUi = vect & ui1;
                        tmpF.X = tmpUi.X; tmpF.Y = tmpUi.Y; tmpF.Z = tmpUi.Z; tmpF.W = tmpUi.W;
                        tmpF /= f1;
                        *ptrV = tmpF + *ptrV - tmpF * (*ptrV);
                        ptrV++;
                        tmpUi = vect & ui2;
                        tmpF.X = tmpUi.X; tmpF.Y = tmpUi.Y; tmpF.Z = tmpUi.Z; tmpF.W = tmpUi.W;
                        tmpF /= f2;
                        *ptrV = tmpF + *ptrV - tmpF * (*ptrV);
                        ptrV++;
                        tmpUi = vect & ui3;
                        tmpF.X = tmpUi.X; tmpF.Y = tmpUi.Y; tmpF.Z = tmpUi.Z; tmpF.W = tmpUi.W;
                        tmpF /= f3;
                        *ptrV = tmpF + *ptrV - tmpF * (*ptrV);
                        ptrV++;
                        tmpUi = vect & ui4;
                        tmpF.X = tmpUi.X; tmpF.Y = tmpUi.Y; tmpF.Z = tmpUi.Z; tmpF.W = tmpUi.W;
                        tmpF /= f4;
                        *ptrV = tmpF + *ptrV - tmpF * (*ptrV);
                        ptrV++;
                        tmpUi = vect & ui5;
                        tmpF.X = tmpUi.X; tmpF.Y = tmpUi.Y; tmpF.Z = tmpUi.Z; tmpF.W = tmpUi.W;
                        tmpF /= f5;
                        *ptrV = tmpF + *ptrV - tmpF * (*ptrV);
                        ptrV++;
                        tmpUi = vect & ui6;
                        tmpF.X = tmpUi.X; tmpF.Y = tmpUi.Y; tmpF.Z = tmpUi.Z; tmpF.W = tmpUi.W;
                        tmpF /= f6;
                        *ptrV = tmpF + *ptrV - tmpF * (*ptrV);
                        ptrV++;
                        tmpUi = vect & ui7;
                        tmpF.X = tmpUi.X; tmpF.Y = tmpUi.Y; tmpF.Z = tmpUi.Z; tmpF.W = tmpUi.W;
                        tmpF /= f7;
                        *ptrV = tmpF + *ptrV - tmpF * (*ptrV);
                        ptrV++;
                        tmpUi = vect & ui8;
                        tmpF.X = tmpUi.X; tmpF.Y = tmpUi.Y; tmpF.Z = tmpUi.Z; tmpF.W = tmpUi.W;
                        tmpF /= f8;
                        *ptrV = tmpF + *ptrV - tmpF * (*ptrV);
                        ptrV++;

                        ptrUL++;
                    }
                }
            }
            return operand1;
        }

        /// <summary>
        /// The unsafe implementation of disjunction between fuzy and crisp
        /// bit strings using Vector4f and precomputed uint and shifting creation of the Vector4f
        /// The algoritm presumes the number of elements both in ulong and Vector4f
        /// fields to be a multiple of 64.
        /// </summary>
        /// <param name="operand1">1. operand (fuzzy)</param>
        /// <param name="operand2">2. operand (crisp)</param>
        /// <returns>Disjunction result (fuzzy)</returns>
        public unsafe static Vector4f[] OrUnsafeCrispFuzzyVector4f3(Vector4f[] operand1, ulong[] operand2)
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
                        tmpF = new Vector4f(tmpUi.X, tmpUi.Y >> 1, tmpUi.Z >> 2, tmpUi.W >> 3);
                        *ptrV = tmpF + *ptrV - tmpF * (*ptrV);
                        ptrV++;
                        tmpUi = vect & sui2;
                        tmpF = new Vector4f(tmpUi.X >> 4, tmpUi.Y >> 5, tmpUi.Z >> 6, tmpUi.W >> 7);
                        *ptrV = tmpF + *ptrV - tmpF * (*ptrV);
                        ptrV++;
                        tmpUi = vect & sui3;
                        tmpF = new Vector4f(tmpUi.X >> 8, tmpUi.Y >> 9, tmpUi.Z >> 10, tmpUi.W >> 11);
                        *ptrV = tmpF + *ptrV - tmpF * (*ptrV);
                        ptrV++;
                        tmpUi = vect & sui4;
                        tmpF = new Vector4f(tmpUi.X >> 12, tmpUi.Y >> 13, tmpUi.Z >> 14, tmpUi.W >> 15);
                        *ptrV = tmpF + *ptrV - tmpF * (*ptrV);
                        ptrV++;
                        tmpUi = vect & sui5;
                        tmpF = new Vector4f(tmpUi.X >> 16, tmpUi.Y >> 17, tmpUi.Z >> 18, tmpUi.W >> 19);
                        *ptrV = tmpF + *ptrV - tmpF * (*ptrV);
                        ptrV++;
                        tmpUi = vect & sui6;
                        tmpF = new Vector4f(tmpUi.X >> 20, tmpUi.Y >> 21, tmpUi.Z >> 22, tmpUi.W >> 23);
                        *ptrV = tmpF + *ptrV - tmpF * (*ptrV);
                        ptrV++;
                        tmpUi = vect & sui7;
                        tmpF = new Vector4f(tmpUi.X >> 24, tmpUi.Y >> 25, tmpUi.Z >> 26, tmpUi.W >> 27);
                        *ptrV = tmpF + *ptrV - tmpF * (*ptrV);
                        ptrV++;
                        tmpUi = vect & sui8;
                        tmpF = new Vector4f(tmpUi.X >> 28, tmpUi.Y >> 29, tmpUi.Z >> 30, tmpUi.W >> 31); ;
                        *ptrV = tmpF + *ptrV - tmpF * (*ptrV);
                        ptrV++;

                        part = (uint)(*ptrUL >> 32);
                        vect = new Vector4ui(part, part, part, part);

                        tmpUi = vect & sui1;
                        tmpF = new Vector4f(tmpUi.X, tmpUi.Y >> 1, tmpUi.Z >> 2, tmpUi.W >> 3);
                        *ptrV = tmpF + *ptrV - tmpF * (*ptrV);
                        ptrV++;
                        tmpUi = vect & sui2;
                        tmpF = new Vector4f(tmpUi.X >> 4, tmpUi.Y >> 5, tmpUi.Z >> 6, tmpUi.W >> 7);
                        *ptrV = tmpF + *ptrV - tmpF * (*ptrV);
                        ptrV++;
                        tmpUi = vect & sui3;
                        tmpF = new Vector4f(tmpUi.X >> 8, tmpUi.Y >> 9, tmpUi.Z >> 10, tmpUi.W >> 11);
                        *ptrV = tmpF + *ptrV - tmpF * (*ptrV);
                        ptrV++;
                        tmpUi = vect & sui4;
                        tmpF = new Vector4f(tmpUi.X >> 12, tmpUi.Y >> 13, tmpUi.Z >> 14, tmpUi.W >> 15);
                        *ptrV = tmpF + *ptrV - tmpF * (*ptrV);
                        ptrV++;
                        tmpUi = vect & sui5;
                        tmpF = new Vector4f(tmpUi.X >> 16, tmpUi.Y >> 17, tmpUi.Z >> 18, tmpUi.W >> 19);
                        *ptrV = tmpF + *ptrV - tmpF * (*ptrV);
                        ptrV++;
                        tmpUi = vect & sui6;
                        tmpF = new Vector4f(tmpUi.X >> 20, tmpUi.Y >> 21, tmpUi.Z >> 22, tmpUi.W >> 23);
                        *ptrV = tmpF + *ptrV - tmpF * (*ptrV);
                        ptrV++;
                        tmpUi = vect & sui7;
                        tmpF = new Vector4f(tmpUi.X >> 24, tmpUi.Y >> 25, tmpUi.Z >> 26, tmpUi.W >> 27);
                        *ptrV = tmpF + *ptrV - tmpF * (*ptrV);
                        ptrV++;
                        tmpUi = vect & sui8;
                        tmpF = new Vector4f(tmpUi.X >> 28, tmpUi.Y >> 29, tmpUi.Z >> 30, tmpUi.W >> 31);
                        *ptrV = tmpF + *ptrV - tmpF * (*ptrV);
                        ptrV++;

                        ptrUL++;
                    }
                }
            }
            return operand1;
        }

        /// <summary>
        /// The unsafe implementation of disjunction between fuzy and crisp
        /// bit strings using Vector4f and precomputed uint and shifting creation of the Vector4f
        /// The algoritm presumes the number of elements both in ulong and Vector4f
        /// fields to be a multiple of 64. The algorithm
        /// doesn't use allocation of new memory in computation of individual Vector4f's,
        /// it assigns to only one Vector4f previously defined. 
        /// </summary>
        /// <param name="operand1">1. operand (fuzzy)</param>
        /// <param name="operand2">2. operand (crisp)</param>
        /// <returns>Disjunction result (fuzzy)</returns>
        public unsafe static Vector4f[] OrUnsafeCrispFuzzyVector4f3NoAllocation(Vector4f[] operand1, ulong[] operand2)
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
                        tmpF.X = tmpUi.X; tmpF.Y = (tmpUi.Y >> 1); tmpF.Z = (tmpUi.Z >> 2); tmpF.W = (tmpUi.W >> 3);
                        *ptrV = tmpF + *ptrV - tmpF * (*ptrV);
                        ptrV++;
                        tmpUi = vect & sui2;
                        tmpF.X = (tmpUi.X >> 4); tmpF.Y = (tmpUi.Y >> 5); tmpF.Z = (tmpUi.Z >> 6); tmpF.W = (tmpUi.W >> 7);
                        *ptrV = tmpF + *ptrV - tmpF * (*ptrV);
                        ptrV++;
                        tmpUi = vect & sui3;
                        tmpF.X = (tmpUi.X >> 8); tmpF.Y = (tmpUi.Y >> 9); tmpF.Z = (tmpUi.Z >> 10); tmpF.W = (tmpUi.W >> 11);
                        *ptrV = tmpF + *ptrV - tmpF * (*ptrV);
                        ptrV++;
                        tmpUi = vect & sui4;
                        tmpF.X = (tmpUi.X >> 12); tmpF.Y = (tmpUi.Y >> 13); tmpF.Z = (tmpUi.Z >> 14); tmpF.W = (tmpUi.W >> 15);
                        *ptrV = tmpF + *ptrV - tmpF * (*ptrV);
                        ptrV++;
                        tmpUi = vect & sui5;
                        tmpF.X = (tmpUi.X >> 16); tmpF.Y = (tmpUi.Y >> 17); tmpF.Z = (tmpUi.Z >> 18); tmpF.W = (tmpUi.W >> 19);
                        *ptrV = tmpF + *ptrV - tmpF * (*ptrV);
                        ptrV++;
                        tmpUi = vect & sui6;
                        tmpF.X = (tmpUi.X >> 20); tmpF.Y = (tmpUi.Y >> 21); tmpF.Z = (tmpUi.Z >> 22); tmpF.W = (tmpUi.W >> 23);
                        *ptrV = tmpF + *ptrV - tmpF * (*ptrV);
                        ptrV++;
                        tmpUi = vect & sui7;
                        tmpF.X = (tmpUi.X >> 24); tmpF.Y = (tmpUi.Y >> 25); tmpF.Z = (tmpUi.Z >> 26); tmpF.W = (tmpUi.W >> 27);
                        *ptrV = tmpF + *ptrV - tmpF * (*ptrV);
                        ptrV++;
                        tmpUi = vect & sui8;
                        tmpF.X = (tmpUi.X >> 28); tmpF.Y = (tmpUi.Y >> 29); tmpF.Z = (tmpUi.Z >> 30); tmpF.W = (tmpUi.W >> 31);
                        *ptrV = tmpF + *ptrV - tmpF * (*ptrV);
                        ptrV++;

                        part = (uint)(*ptrUL >> 32);
                        vect = new Vector4ui(part, part, part, part);

                        tmpUi = vect & sui1;
                        tmpF.X = tmpUi.X; tmpF.Y = (tmpUi.Y >> 1); tmpF.Z = (tmpUi.Z >> 2); tmpF.W = (tmpUi.W >> 3);
                        *ptrV = tmpF + *ptrV - tmpF * (*ptrV);
                        ptrV++;
                        tmpUi = vect & sui2;
                        tmpF.X = (tmpUi.X >> 4); tmpF.Y = (tmpUi.Y >> 5); tmpF.Z = (tmpUi.Z >> 6); tmpF.W = (tmpUi.W >> 7);
                        *ptrV = tmpF + *ptrV - tmpF * (*ptrV);
                        ptrV++;
                        tmpUi = vect & sui3;
                        tmpF.X = (tmpUi.X >> 8); tmpF.Y = (tmpUi.Y >> 9); tmpF.Z = (tmpUi.Z >> 10); tmpF.W = (tmpUi.W >> 11);
                        *ptrV = tmpF + *ptrV - tmpF * (*ptrV);
                        ptrV++;
                        tmpUi = vect & sui4;
                        tmpF.X = (tmpUi.X >> 12); tmpF.Y = (tmpUi.Y >> 13); tmpF.Z = (tmpUi.Z >> 14); tmpF.W = (tmpUi.W >> 15);
                        *ptrV = tmpF + *ptrV - tmpF * (*ptrV);
                        ptrV++;
                        tmpUi = vect & sui5;
                        tmpF.X = (tmpUi.X >> 16); tmpF.Y = (tmpUi.Y >> 17); tmpF.Z = (tmpUi.Z >> 18); tmpF.W = (tmpUi.W >> 19);
                        *ptrV = tmpF + *ptrV - tmpF * (*ptrV);
                        ptrV++;
                        tmpUi = vect & sui6;
                        tmpF.X = (tmpUi.X >> 20); tmpF.Y = (tmpUi.Y >> 21); tmpF.Z = (tmpUi.Z >> 22); tmpF.W = (tmpUi.W >> 23);
                        *ptrV = tmpF + *ptrV - tmpF * (*ptrV);
                        ptrV++;
                        tmpUi = vect & sui7;
                        tmpF.X = (tmpUi.X >> 24); tmpF.Y = (tmpUi.Y >> 25); tmpF.Z = (tmpUi.Z >> 26); tmpF.W = (tmpUi.W >> 27);
                        *ptrV = tmpF + *ptrV - tmpF * (*ptrV);
                        ptrV++;
                        tmpUi = vect & sui8;
                        tmpF.X = (tmpUi.X >> 28); tmpF.Y = (tmpUi.Y >> 29); tmpF.Z = (tmpUi.Z >> 30); tmpF.W = (tmpUi.W >> 31);
                        *ptrV = tmpF + *ptrV - tmpF * (*ptrV);
                        ptrV++;

                        ptrUL++;
                    }
                }
            }
            return operand1;
        }

        /// <summary>
        /// The unsafe implementation of disjunction between fuzy and crisp
        /// bit strings using Vector4f and precomputed uint and shifting creation of the Vector4f
        /// The algoritm presumes the number of elements both in ulong and Vector4f
        /// fields to be a multiple of 64. No static variables used
        /// </summary>
        /// <param name="operand1">1. operand (fuzzy)</param>
        /// <param name="operand2">2. operand (crisp)</param>
        /// <returns>Disjunction result (fuzzy)</returns>
        public unsafe static Vector4f[] OrUnsafeCrispFuzzyVector4f3NoStatic(Vector4f[] operand1, ulong[] operand2)
        {
            Vector4f tmpF;
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
                        tmpF = new Vector4f(tmpUi.X, tmpUi.Y >> 1, tmpUi.Z >> 2, tmpUi.W >> 3);
                        *ptrV = tmpF + *ptrV - tmpF * (*ptrV);
                        ptrV++;
                        tmpUi = vect & ui2;
                        tmpF = new Vector4f(tmpUi.X >> 4, tmpUi.Y >> 5, tmpUi.Z >> 6, tmpUi.W >> 7);
                        *ptrV = tmpF + *ptrV - tmpF * (*ptrV);
                        ptrV++;
                        tmpUi = vect & ui3;
                        tmpF = new Vector4f(tmpUi.X >> 8, tmpUi.Y >> 9, tmpUi.Z >> 10, tmpUi.W >> 11);
                        *ptrV = tmpF + *ptrV - tmpF * (*ptrV);
                        ptrV++;
                        tmpUi = vect & ui4;
                        tmpF = new Vector4f(tmpUi.X >> 12, tmpUi.Y >> 13, tmpUi.Z >> 14, tmpUi.W >> 15);
                        *ptrV = tmpF + *ptrV - tmpF * (*ptrV);
                        ptrV++;
                        tmpUi = vect & ui5;
                        tmpF = new Vector4f(tmpUi.X >> 16, tmpUi.Y >> 17, tmpUi.Z >> 18, tmpUi.W >> 19);
                        *ptrV = tmpF + *ptrV - tmpF * (*ptrV);
                        ptrV++;
                        tmpUi = vect & ui6;
                        tmpF = new Vector4f(tmpUi.X >> 20, tmpUi.Y >> 21, tmpUi.Z >> 22, tmpUi.W >> 23);
                        *ptrV = tmpF + *ptrV - tmpF * (*ptrV);
                        ptrV++;
                        tmpUi = vect & ui7;
                        tmpF = new Vector4f(tmpUi.X >> 24, tmpUi.Y >> 25, tmpUi.Z >> 26, tmpUi.W >> 27);
                        *ptrV = tmpF + *ptrV - tmpF * (*ptrV);
                        ptrV++;
                        tmpUi = vect & ui8;
                        tmpF = new Vector4f(tmpUi.X >> 28, tmpUi.Y >> 29, tmpUi.Z >> 30, tmpUi.W >> 31); ;
                        *ptrV = tmpF + *ptrV - tmpF * (*ptrV);
                        ptrV++;

                        part = (uint)(*ptrUL >> 32);
                        vect = new Vector4ui(part, part, part, part);

                        tmpUi = vect & ui1;
                        tmpF = new Vector4f(tmpUi.X, tmpUi.Y >> 1, tmpUi.Z >> 2, tmpUi.W >> 3);
                        *ptrV = tmpF + *ptrV - tmpF * (*ptrV);
                        ptrV++;
                        tmpUi = vect & ui2;
                        tmpF = new Vector4f(tmpUi.X >> 4, tmpUi.Y >> 5, tmpUi.Z >> 6, tmpUi.W >> 7);
                        *ptrV = tmpF + *ptrV - tmpF * (*ptrV);
                        ptrV++;
                        tmpUi = vect & ui3;
                        tmpF = new Vector4f(tmpUi.X >> 8, tmpUi.Y >> 9, tmpUi.Z >> 10, tmpUi.W >> 11);
                        *ptrV = tmpF + *ptrV - tmpF * (*ptrV);
                        ptrV++;
                        tmpUi = vect & ui4;
                        tmpF = new Vector4f(tmpUi.X >> 12, tmpUi.Y >> 13, tmpUi.Z >> 14, tmpUi.W >> 15);
                        *ptrV = tmpF + *ptrV - tmpF * (*ptrV);
                        ptrV++;
                        tmpUi = vect & ui5;
                        tmpF = new Vector4f(tmpUi.X >> 16, tmpUi.Y >> 17, tmpUi.Z >> 18, tmpUi.W >> 19);
                        *ptrV = tmpF + *ptrV - tmpF * (*ptrV);
                        ptrV++;
                        tmpUi = vect & ui6;
                        tmpF = new Vector4f(tmpUi.X >> 20, tmpUi.Y >> 21, tmpUi.Z >> 22, tmpUi.W >> 23);
                        *ptrV = tmpF + *ptrV - tmpF * (*ptrV);
                        ptrV++;
                        tmpUi = vect & ui7;
                        tmpF = new Vector4f(tmpUi.X >> 24, tmpUi.Y >> 25, tmpUi.Z >> 26, tmpUi.W >> 27);
                        *ptrV = tmpF + *ptrV - tmpF * (*ptrV);
                        ptrV++;
                        tmpUi = vect & ui8;
                        tmpF = new Vector4f(tmpUi.X >> 28, tmpUi.Y >> 29, tmpUi.Z >> 30, tmpUi.W >> 31);
                        *ptrV = tmpF + *ptrV - tmpF * (*ptrV);
                        ptrV++;

                        ptrUL++;
                    }
                }
            }
            return operand1;
        }

        /// <summary>
        /// The unsafe implementation of disjunction between fuzy and crisp
        /// bit strings using Vector4f and precomputed uint and shifting creation of the Vector4f
        /// The algoritm presumes the number of elements both in ulong and Vector4f
        /// fields to be a multiple of 64. The algorithm
        /// doesn't use allocation of new memory in computation of individual Vector4f's,
        /// it assigns to only one Vector4f previously defined. No static variables used
        /// </summary>
        /// <param name="operand1">1. operand (fuzzy)</param>
        /// <param name="operand2">2. operand (crisp)</param>
        /// <returns>Disjunction result (fuzzy)</returns>
        public unsafe static Vector4f[] OrUnsafeCrispFuzzyVector4f3NoAllocationNoStatic(Vector4f[] operand1, ulong[] operand2)
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
                        tmpF.X = tmpUi.X; tmpF.Y = (tmpUi.Y >> 1); tmpF.Z = (tmpUi.Z >> 2); tmpF.W = (tmpUi.W >> 3);
                        *ptrV = tmpF + *ptrV - tmpF * (*ptrV);
                        ptrV++;
                        tmpUi = vect & ui2;
                        tmpF.X = (tmpUi.X >> 4); tmpF.Y = (tmpUi.Y >> 5); tmpF.Z = (tmpUi.Z >> 6); tmpF.W = (tmpUi.W >> 7);
                        *ptrV = tmpF + *ptrV - tmpF * (*ptrV);
                        ptrV++;
                        tmpUi = vect & ui3;
                        tmpF.X = (tmpUi.X >> 8); tmpF.Y = (tmpUi.Y >> 9); tmpF.Z = (tmpUi.Z >> 10); tmpF.W = (tmpUi.W >> 11);
                        *ptrV = tmpF + *ptrV - tmpF * (*ptrV);
                        ptrV++;
                        tmpUi = vect & ui4;
                        tmpF.X = (tmpUi.X >> 12); tmpF.Y = (tmpUi.Y >> 13); tmpF.Z = (tmpUi.Z >> 14); tmpF.W = (tmpUi.W >> 15);
                        *ptrV = tmpF + *ptrV - tmpF * (*ptrV);
                        ptrV++;
                        tmpUi = vect & ui5;
                        tmpF.X = (tmpUi.X >> 16); tmpF.Y = (tmpUi.Y >> 17); tmpF.Z = (tmpUi.Z >> 18); tmpF.W = (tmpUi.W >> 19);
                        *ptrV = tmpF + *ptrV - tmpF * (*ptrV);
                        ptrV++;
                        tmpUi = vect & ui6;
                        tmpF.X = (tmpUi.X >> 20); tmpF.Y = (tmpUi.Y >> 21); tmpF.Z = (tmpUi.Z >> 22); tmpF.W = (tmpUi.W >> 23);
                        *ptrV = tmpF + *ptrV - tmpF * (*ptrV);
                        ptrV++;
                        tmpUi = vect & ui7;
                        tmpF.X = (tmpUi.X >> 24); tmpF.Y = (tmpUi.Y >> 25); tmpF.Z = (tmpUi.Z >> 26); tmpF.W = (tmpUi.W >> 27);
                        *ptrV = tmpF + *ptrV - tmpF * (*ptrV);
                        ptrV++;
                        tmpUi = vect & ui8;
                        tmpF.X = (tmpUi.X >> 28); tmpF.Y = (tmpUi.Y >> 29); tmpF.Z = (tmpUi.Z >> 30); tmpF.W = (tmpUi.W >> 31);
                        *ptrV = tmpF + *ptrV - tmpF * (*ptrV);
                        ptrV++;

                        part = (uint)(*ptrUL >> 32);
                        vect = new Vector4ui(part, part, part, part);

                        tmpUi = vect & ui1;
                        tmpF.X = tmpUi.X; tmpF.Y = (tmpUi.Y >> 1); tmpF.Z = (tmpUi.Z >> 2); tmpF.W = (tmpUi.W >> 3);
                        *ptrV = tmpF + *ptrV - tmpF * (*ptrV);
                        ptrV++;
                        tmpUi = vect & ui2;
                        tmpF.X = (tmpUi.X >> 4); tmpF.Y = (tmpUi.Y >> 5); tmpF.Z = (tmpUi.Z >> 6); tmpF.W = (tmpUi.W >> 7);
                        *ptrV = tmpF + *ptrV - tmpF * (*ptrV);
                        ptrV++;
                        tmpUi = vect & ui3;
                        tmpF.X = (tmpUi.X >> 8); tmpF.Y = (tmpUi.Y >> 9); tmpF.Z = (tmpUi.Z >> 10); tmpF.W = (tmpUi.W >> 11);
                        *ptrV = tmpF + *ptrV - tmpF * (*ptrV);
                        ptrV++;
                        tmpUi = vect & ui4;
                        tmpF.X = (tmpUi.X >> 12); tmpF.Y = (tmpUi.Y >> 13); tmpF.Z = (tmpUi.Z >> 14); tmpF.W = (tmpUi.W >> 15);
                        *ptrV = tmpF + *ptrV - tmpF * (*ptrV);
                        ptrV++;
                        tmpUi = vect & ui5;
                        tmpF.X = (tmpUi.X >> 16); tmpF.Y = (tmpUi.Y >> 17); tmpF.Z = (tmpUi.Z >> 18); tmpF.W = (tmpUi.W >> 19);
                        *ptrV = tmpF + *ptrV - tmpF * (*ptrV);
                        ptrV++;
                        tmpUi = vect & ui6;
                        tmpF.X = (tmpUi.X >> 20); tmpF.Y = (tmpUi.Y >> 21); tmpF.Z = (tmpUi.Z >> 22); tmpF.W = (tmpUi.W >> 23);
                        *ptrV = tmpF + *ptrV - tmpF * (*ptrV);
                        ptrV++;
                        tmpUi = vect & ui7;
                        tmpF.X = (tmpUi.X >> 24); tmpF.Y = (tmpUi.Y >> 25); tmpF.Z = (tmpUi.Z >> 26); tmpF.W = (tmpUi.W >> 27);
                        *ptrV = tmpF + *ptrV - tmpF * (*ptrV);
                        ptrV++;
                        tmpUi = vect & ui8;
                        tmpF.X = (tmpUi.X >> 28); tmpF.Y = (tmpUi.Y >> 29); tmpF.Z = (tmpUi.Z >> 30); tmpF.W = (tmpUi.W >> 31);
                        *ptrV = tmpF + *ptrV - tmpF * (*ptrV);
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
