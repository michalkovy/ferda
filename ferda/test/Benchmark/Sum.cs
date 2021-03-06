// Sum.cs
//
//  Copyright (C) 2009 Michal Kováč <michal.kovac.develop@centrum.cz>
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
//
//

using System;
using System.Numerics;
//using Mono.Math;

namespace Ferda.Benchmark
{
    public class Sum : FerdaBenchmark
    {
        #region Fields
        static ulong sumBitResult; // This is what we'll check in double
        static double sumFloatResult; // This is what we'll check in float version

        //static BigInteger[] stringBigInteger = new BigInteger[lengthUlongString];
        static Vector<ulong>[] stringVectorUl = new Vector<ulong>[LengthVectorUlString];

        private static int LengthVectorUlString
        {
            get { return lengthUlongString / Vector<ulong>.Count; }
        }

        #endregion

        #region Init, Reset and Check
        public static void Init(string[] args)
        {
            if (args.Length > 0)
                iterations = Int32.Parse(args[0]);

            InitializeBitcounts();
            Random r = new Random();
            for (int i = 0; i < lengthUlongString; i++)
            {
                stringUlong[i] = (ulong)(uint)r.Next(Int32.MinValue,
                    Int32.MaxValue) |
                    (((ulong)(uint)r.Next(Int32.MinValue,
                    Int32.MaxValue)) << 32);
                //stringBigInteger[i] = new BigInteger(stringUlong[i]);
            }
            for (int i = 0; i < LengthFloatString; i++)
            {
                stringFloat[i] = (float)r.NextDouble();
            }
            for (int i = 0; i < LengthVectorUlString; i++)
            {
                stringVectorUl[i] = new Vector<ulong>(stringUlong, i * Vector<ulong>.Count);
            }
            for (int i = 0; i < LengthVector4String; i++)
            {
                stringVector4[i] = new Vector4(stringFloat[i * 4], stringFloat[i * 4 + 1], stringFloat[i * 4 + 2], stringFloat[i * 4 + 3]);
            }

        }

        public static void Reset()
        {
            sumBitResult = 0;
            sumFloatResult = 0;
        }

        public static void Check()
        {
            if (sumBitResult == 0 && sumFloatResult == 0)
                throw new Exception("Result of sum not set");
            if (sumBitResult > 0)
            {
                if (sumBitResult != PrecomputedBitcount(stringUlong))
                    throw new Exception("Bit sum is not correct");
            }
            if (sumFloatResult != 0)
            {
                float correctSum = FloatSum(stringFloat);
                if ((correctSum / sumFloatResult) > 1.01 || (correctSum / sumFloatResult) < 0.99)
                    throw new Exception("Float sum is not correct");
            }
        }
        #endregion

        #region Benchmarks

        [Benchmark]
        public static void BoolPrecomputed()
        {
            //don't use static variables in iterations
            ulong[] array = stringUlong;
            int count = iterations;
            ulong result = 0;
            for (int i = 0; i < count; i++)
            {
                result = PrecomputedBitcount(array);
            }
            sumBitResult = result;
        }

        [Benchmark]
        public static void BoolQuick()
        {
            //don't use static variables in iterations
            ulong[] array = stringUlong;
            int count = iterations;
            ulong result = 0;
            for (int i = 0; i < count; i++)
            {
                result = QuickSum(array);
            }
            sumBitResult = result;
        }

        [Benchmark]
        public static void BoolQuickWithoutMult()
        {
            //don't use static variables in iterations
            ulong[] array = stringUlong;
            int count = iterations;
            ulong result = 0;
            for (int i = 0; i < count; i++)
            {
                result = QuickSumWithoutMultiply(array);
            }
            sumBitResult = result;
        }

        [Benchmark]
        public static void BoolVectorPopCnt()
        {
            //don't use static variables in iterations
            Vector<ulong>[] vec = stringVectorUl;
            int count = iterations;
            ulong result = 0;
            for (int i = 0; i < count; i++)
            {
                result = VectorSum(vec);
            }
            sumBitResult = result;
        }

        [Benchmark]
        public static void BoolQuickVectorUl()
        {
            //don't use static variables in iterations
            Vector<ulong>[] array = stringVectorUl;
            int count = iterations;
            ulong result = 0;
            for (int i = 0; i < count; i++)
            {
                result = QuickUnsafeVector2uSum(array);
            }
            sumBitResult = result;
        }

        [Benchmark]
        public static void FuzzyFloat()
        {
            //don't use static variables in iterations
            float[] array = stringFloat;
            int count = iterations;
            double result = 0;
            for (int i = 0; i < count; i++)
            {
                result = FloatSum(array);
            }
            sumFloatResult = result;
        }

        [Benchmark]
        public static void FuzzyVector4()
        {
            //don't use static variables in iterations
            Vector4[] array = stringVector4;
            int count = iterations;
            double result = 0;
            for (int i = 0; i < count; i++)
            {
                result = FloatSumVector(array);
            }
            sumFloatResult = result;
        }

        [Benchmark]
        public static void FuzzyVector4Safe()
        {
            //don't use static variables in iterations
            Vector4[] array = stringVector4;
            int count = iterations;
            double result = 0;
            for (int i = 0; i < count; i++)
            {
                result = FloatSumVectorSafe(array);
            }
            sumFloatResult = result;
        }

        /*
         [Benchmark]
         public static void BoolBigInteger()
         {
             //don't use static variables in iterations
             BigInteger[] array = stringBigInteger;
             int count = iterations;
             ulong result = 0;
             for (int i = 0; i < count; i++)
             {
                 result = BigIntegerSum(array);
             }
             sumBitResult = result;
         }*/

        #endregion

        #region Real implementation methods
        const ulong m1 = 0x5555555555555555; //binary: 0101...
        const ulong m2 = 0x3333333333333333; //binary: 00110011..
        const ulong m4 = 0x0f0f0f0f0f0f0f0f; //binary:  4 zeros,  4 ones ...
        const ulong m8 = 0x00ff00ff00ff00ff; //binary:  8 zeros,  8 ones ...
        const ulong m16 = 0x0000ffff0000ffff; //binary: 16 zeros, 16 ones ...
        const ulong m32 = 0x00000000ffffffff; //binary: 32 zeros, 32 ones
        const ulong hff = 0xffffffffffffffff; //binary: all ones
        const ulong h01 = 0x0101010101010101; //the sum of 256 to the power of 0,1,2,3...

        //private static Vector<ulong> M1 = new Vector<ulong>(new ulong[] { 0x5555555555555555, 0x5555555555555555 });
        //private static Vector<ulong> M2 = new Vector<ulong>(new ulong[] { 0x3333333333333333, 0x3333333333333333 });
        //private static Vector<ulong> M4 = new Vector<ulong>(new ulong[] { 0x0f0f0f0f0f0f0f0f, 0x0f0f0f0f0f0f0f0f });
        //private static Vector<uint> H01 = new Vector<uint>(new uint[] { 0x01010101, 0x01010101, 0x01010101, 0x01010101 });

        static unsafe float FloatSum(float[] r)
        {
            float result = 0;

            fixed (float* arrayPtr = r)
            {
                float* currentPtr = arrayPtr, stopPtr = arrayPtr + r.Length;
                while (currentPtr < stopPtr)
                {
                    result += *currentPtr++;
                }
            }

            return result;
        }

        static unsafe float FloatSumVector(Vector4[] r)
        {
            Vector4 result = new Vector4(0, 0, 0, 0);

            fixed (Vector4* arrayPtr = r)
            {
                Vector4* currentPtr = arrayPtr, stopPtr = arrayPtr + r.Length;
                while (currentPtr < stopPtr)
                {
                    result += *currentPtr++;
                }
            }

            return result.X + result.Y + result.Z + result.W;
        }

        static float FloatSumVectorSafe(Vector4[] r)
        {
            Vector4 result = new Vector4(0, 0, 0, 0);

            for (int i = 0; i < r.Length - 1; i++)
            {
                result += r[i];
            }

            return result.X + result.Y + result.Z + result.W;
        }

        /*
        static ulong BigIntegerSum(BigInteger[] r) {
            ulong result = 0;
            for(int i = 0; i < r.Length; i++)
            {
                result += (ulong)r[i].BitCount();
            }
            return result;
        }*/

        //This uses fewer arithmetic operations than any other known  
        //implementation on machines with fast multiplication.
        //It uses 12 arithmetic operations, one of which is a multiply.
        static unsafe ulong QuickSum(ulong[] r)
        {
            ulong result = 0;
            fixed (ulong* arrayPtr = r)
            {
                ulong* currentPtr = arrayPtr, stopPtr = arrayPtr + r.Length;
                while (currentPtr < stopPtr)
                {
                    ulong x = *currentPtr++;
                    x -= (x >> 1) & m1;             //put count of each 2 bits into those 2 bits
                    x = (x & m2) + ((x >> 2) & m2); //put count of each 4 bits into those 4 bits 
                    x = (x + (x >> 4)) & m4;        //put count of each 8 bits into those 8 bits 
                    result += ((x * h01) >> 56);  //returns left 8 bits of x + (x<<8) + (x<<16) + (x<<24) + ...
                }
            }
            return result;
        }

        //This uses fewer arithmetic operations than any other known  
        //implementation on machines with slow multiplication.
        //It uses 17 arithmetic operations.
        static unsafe ulong QuickSumWithoutMultiply(ulong[] r)
        {
            ulong result = 0;
            fixed (ulong* arrayPtr = r)
            {
                ulong* currentPtr = arrayPtr, stopPtr = arrayPtr + r.Length;
                while (currentPtr < stopPtr)
                {
                    ulong x = *currentPtr++;
                    x -= (x >> 1) & m1;             //put count of each 2 bits into those 2 bits
                    x = (x & m2) + ((x >> 2) & m2); //put count of each 4 bits into those 4 bits 
                    x = (x + (x >> 4)) & m4;        //put count of each 8 bits into those 8 bits 
                    x += x >> 8;  //put count of each 16 bits into their lowest 8 bits
                    x += x >> 16;  //put count of each 32 bits into their lowest 8 bits
                    x += x >> 32;  //put count of each 64 bits into their lowest 8 bits
                    result += (x & 0x7f);
                }
            }
            return result;
        }

        static unsafe ulong QuickUnsafeVector2uSum(Vector<ulong>[] r)
        {
            ulong result = 0;
            Span<ulong> stringVectorTmp = stackalloc ulong[Vector<ulong>.Count];
            fixed (Vector<ulong>* ur = r)
            {
                Vector<ulong>* a = ur, kon = ur + r.Length;
                while (a < kon)
                {
                    Vector<ulong> x = *a++;
                    x.CopyTo(stringVectorTmp);
                    foreach (ulong q in stringVectorTmp)
                        result += (ulong)BitOperations.PopCount(q);
                }
            }
            return result;
            //Vector<ulong> M1 = Sum.M1;
            //Vector<ulong> M2 = Sum.M2;
            //Vector<ulong> M4 = Sum.M4;
            //Vector<uint> H01 = Sum.H01;
            //Vector<uint> result = new Vector<uint>(new uint[] { 0, 0, 0, 0 });
            //fixed (Vector<ulong>* ur = r)
            //{
            //    Vector<ulong>* a = ur, kon = ur + r.Length;
            //    while (a < kon)
            //    {
            //        Vector<ulong> x = *a++;
            //        x -= (x >> 1) & M1;             //put count of each 2 bits into those 2 bits
            //        x = (x & M2) + ((x >> 2) & M2); //put count of each 4 bits into those 4 bits 
            //        x = (x + (x >> 4)) & M4;        //put count of each 8 bits into those 8 bits
            //        result += ((((Vector<uint>)x) * H01) >> 24);	//returns left 8 bits of x + (x<<8) + (x<<16) + (x<<24) + ... */
            //    }
            //}
            //return result.X + result.Y + result.W + result.Z;
        }

        static ulong VectorSum(Vector<ulong>[] vectorArray)
        {
            ulong result = 0;
            Span<ulong> stringVectorTmp = stackalloc ulong[Vector<ulong>.Count];
            foreach (Vector<ulong> v in vectorArray)
            {
                v.CopyTo(stringVectorTmp);
                foreach (ulong x in stringVectorTmp)
                {
                    result += (ulong)BitOperations.PopCount(x);
                }
            }
            return result;
        }

        static byte[] _bitcounts = new byte[65536];

        static private void InitializeBitcounts()
        {
            for (int i = 0; i < 65536; i++)
            {
                // Get the bitcount using any method
                _bitcounts[i] = (byte)SparseBitcount(i);
            }
        }

        static uint SparseBitcount(int n)
        {
            uint count = 0;
            while (n != 0)
            {
                count++;
                n &= (n - 1);
            }
            return count;
        }


        static unsafe private uint PrecomputedBitcount(ulong[] r)
        {
            uint result = 0;

            fixed (ulong* arrayPtr = r)
            {
                fixed (Byte* lookup = _bitcounts)
                {
                    ulong* currentPtr = arrayPtr, stopPtr = arrayPtr + r.Length;
                    while (currentPtr < stopPtr)
                    {
                        ulong current = *currentPtr++;

                        result += *(lookup + (uint)(current & 65535));
                        result += *(lookup + (uint)((current >> 16) & 65535));
                        result += *(lookup + (uint)((current >> 32) & 65535));
                        result += *(lookup + (uint)(current >> 48));
                    }
                }
            }


            return result;
        }
        #endregion

    }
}