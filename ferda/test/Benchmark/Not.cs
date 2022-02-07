// Not.cs
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
    /// The benchmark for the negation operation in bit strings. It does not
    /// use the actual methods implemented in the FerdaMiningProcessor, because
    /// the idea is to test different algorithms and their modifications and then
    /// to use the best one.
    /// </summary>
    public class Not : FerdaBenchmark
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
        /// The crisp not benchmark - safe
        /// </summary>
        [Benchmark]
        public static void SafeNotCrisp()
        {
            //don't use static variables in iterations
            ulong[] tmp = stringUlong;
            int count = iterations;
            for (int i = 0; i < count; i++)
            {
                NotSafeCrisp(tmp);
            }
        }

        /// <summary>
        /// The crisp not benchmark - unsafe
        /// </summary>
        [Benchmark]
        public static void UnsafeNotCrisp()
        {
            //don't use static variables in iterations
            ulong[] tmp = stringUlong;
            int count = iterations;
            for (int i = 0; i < count; i++)
            {
                NotUnsafeCrisp(tmp);
            }
        }

        [Benchmark]
        public static void SafeNotCrispVector()
        {
            //don't use static variables in iterations
            var tmp = stringVectorUlong1;
            int count = iterations;
            for (int i = 0; i < count; i++)
            {
                NotSafeCrispVector(tmp);
            }
            
        }

        /// <summary>
        /// Not benchmark with fuzzy bit strings, Vector4 + safe (managed)
        /// </summary>
        [Benchmark]
        public static void SafeNotFuzzyVector4()
        {
            //don't use static variables in iterations
            Vector4[] tmp = stringVector4;
            int count = iterations;
            for (int i = 0; i < count; i++)
            {
                NotSafe4f(tmp);
            }
        }

        /// <summary>
        /// Not benchmark with fuzzy bit string, array of floats + safe (managed)
        /// </summary>
        [Benchmark]
        public static void SafeNotFuzzyFloat()
        {
            //don't use static variables in iterations
            float[] tmp = stringFloat;
            int count = iterations;
            for (int i = 0; i < count; i++)
            {
                NotSafeFloat(tmp);
            }
        }

        /// <summary>
        /// Not benchmark with fuzzy bit string, array of floats + unsafe (unmanaged)
        /// + arithmetic in 1 operation
        /// </summary>
        [Benchmark]
        public static void UnsafeNotFuzzyVector41Operation()
        {
            //don't use static variables in iterations
            Vector4[] tmp = stringVector4;
            int count = iterations;
            for (int i = 0; i < count; i++)
            {
                NotUnsafe4f1(tmp);
            }
        }

        /// <summary>
        /// Not benchmark with fuzzy bit string, array of floats + unsafe (unmanaged)
        /// + arithmetic in 2 operations
        /// </summary>
        [Benchmark]
        public static void UnsafeNotFuzzyVector42Operation()
        {
            //don't use static variables in iterations
            Vector4[] tmp = stringVector4;
            int count = iterations;
            for (int i = 0; i < count; i++)
            {
                NotUnsafe4f2(tmp);
            }
        }

        /// <summary>
        /// Not benchmark with fuzzy bit string, array of floats + unsafe (unmanaged)
        /// </summary>
        [Benchmark]
        public static void UnsafeNotFuzzyFloat()
        {
            //don't use static variables in iterations
            float[] tmp = stringFloat;
            int count = iterations;
            for (int i = 0; i < count; i++)
            {
                NotUnsafeFloat(tmp);
            }
        }

        #endregion

        #region Real implementation methods

        /// <summary>
        /// The safe (managed) implementation of the crisp negation
        /// </summary>
        /// <param name="source">Bit string to be negated</param>
        /// <returns>Negated bit string</returns>
        static ulong[] NotSafeCrisp(ulong[] source)
        {
            for (int i = 0; i < source.Length; i++)
            {
                source[i] = ~source[i];
            }
            return source;
        }

        /// <summary>
        /// The unsafe (unmanaged) implementation of the crisp negation
        /// </summary>
        /// <param name="source">Bit string to be negated</param>
        /// <returns>Negated bit string</returns>
        static unsafe ulong[] NotUnsafeCrisp(ulong[] source)
        {
            fixed (ulong* pin = source)
            {
                ulong* ptr = pin, stop = pin + source.Length;
                while (ptr < stop)
                {
                    *ptr = ~(*ptr);
                    ptr++;
                }
            }
            return source;
        }

        static Vector<ulong>[] NotSafeCrispVector(Vector<ulong>[] source)
        {
            for (int i = 0; i < source.Length; i++)
            {
                source[i] = ~source[i];
            }
            return source;
        }

        /// <summary>
        /// The save (managed) implementation of the Lukasiewicz negation 
        /// N(x) = 1 - x for Vector4
        /// </summary>
        /// <param name="source">Bit string to be negated</param>
        /// <returns>Negated bit string</returns>
        static Vector4[] NotSafe4f(Vector4[] source)
        {
            Vector4 tmp = new Vector4(1f, 1f, 1f, 1f);
            for (int i = 0; i < source.Length; i++)
            {
                source[i] = tmp - source[i];
            }

            return source;
        }

        /// <summary>
        /// The save (managed) implementation of the Lukasiewicz negation 
        /// N(x) = 1 - x for array of floats
        /// </summary>
        /// <param name="source">Bit string to be negated</param>
        /// <returns>Negated bit string</returns>
        static float[] NotSafeFloat(float[] source)
        {
            for (int i = 0; i < source.Length; i++)
            {
                source[i] = 1 - source[i];
            }
            return source;
        }

        /// <summary>
        /// The unsafe (unmanaged) implementation of the Lukasiewicz negation 
        /// N(x) = 1 - x for array of Vector4.
        /// The method does aritmetic in one operation.
        /// </summary>
        /// <param name="source">Bit string to be negated</param>
        /// <returns>Negated bit string</returns>
        static unsafe Vector4[] NotUnsafe4f1(Vector4[] source)
        {
            Vector4 tmp = new Vector4(1f, 1f, 1f, 1f);

            fixed (Vector4* arrayPtr = source)
            {
                Vector4* currentPtr = arrayPtr, stopPtr = arrayPtr + source.Length;
                while (currentPtr < stopPtr)
                {
                    *currentPtr = tmp - *currentPtr;
                    currentPtr++;
                }
            }

            return source;
        }

        /// <summary>
        /// The unsafe (unmanaged) implementation of the Lukasiewicz negation 
        /// N(x) = 1 - x for array of Vector4.
        /// The method does arithmetic in two operations:
        /// X -= source and X += 1(vector)
        /// </summary>
        /// <param name="source">Bit string to be negated</param>
        /// <returns>Negated bit string</returns>
        static unsafe Vector4[] NotUnsafe4f2(Vector4[] source)
        {
            Vector4 pos1f = new Vector4(1f, 1f, 1f, 1f);
            Vector4 neg1f = new Vector4(-1f, -1f, -1f, -1f);

            fixed (Vector4* arrayPtr = source)
            {
                Vector4* currentPtr = arrayPtr, stopPtr = arrayPtr + source.Length;
                while (currentPtr < stopPtr)
                {
                    *currentPtr -= pos1f;
                    *currentPtr++ *= neg1f;
                }
            }

            return source;
        }

        /// <summary>
        /// The unsave (unmanaged) implementation of the Lukasiewicz negation 
        /// N(x) = 1 - x for array of floats
        /// </summary>
        /// <param name="source">Bit string to be negated</param>
        /// <returns>Negated bit string</returns>
        static unsafe float[] NotUnsafeFloat(float[] source)
        {
            fixed (float* arrayPtr = source)
            {
                float* currentPtr = arrayPtr, stopPtr = arrayPtr + source.Length;
                while (currentPtr < stopPtr)
                {
                    *currentPtr = 1 - *currentPtr;
                    currentPtr++;
                }
            }

            return source;
        }

        #endregion
    }
}
