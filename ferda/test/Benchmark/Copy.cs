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
	public class Copy : FerdaBenchmark
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

        [Benchmark]
        public static void StandardInitSpanCopy()
        {
            //don't use static variables in iterations
            ulong[] tmp = stringUlong;
            int count = iterations;
            for (int i = 0; i < count; i++)
            {
                ulong[] tmp1 = new ulong[tmp.Length];
                tmp.CopyTo((Span<ulong>)tmp1);
            }
        }

        [Benchmark]
        public static void UninitializedInitSpanCopy()
        {
            //don't use static variables in iterations
            ulong[] tmp = stringUlong;
            int count = iterations;
            for (int i = 0; i < count; i++)
            {
                ulong[] tmp1 = GC.AllocateUninitializedArray<ulong>(tmp.Length);
                tmp.CopyTo((Span<ulong>)tmp1);
            }
        }

        [Benchmark]
        public static void UninitializedInitAsSpanCopy()
        {
            //don't use static variables in iterations
            ulong[] tmp = stringUlong;
            int count = iterations;
            for (int i = 0; i < count; i++)
            {
                ulong[] tmp1 = GC.AllocateUninitializedArray<ulong>(tmp.Length);
                tmp.AsSpan<ulong>().CopyTo(tmp1);
            }
        }

        [Benchmark]
        public static void UninitializedInitMemoryCopy()
        {
            //don't use static variables in iterations
            ulong[] tmp = stringUlong;
            int count = iterations;
            for (int i = 0; i < count; i++)
            {
                ulong[] tmp1 = GC.AllocateUninitializedArray<ulong>(tmp.Length);
                tmp.CopyTo((Memory<ulong>)tmp1);
            }
        }

        [Benchmark]
        public static void UninitializedInitAsMemoryCopy()
        {
            //don't use static variables in iterations
            ulong[] tmp = stringUlong;
            int count = iterations;
            for (int i = 0; i < count; i++)
            {
                ulong[] tmp1 = GC.AllocateUninitializedArray<ulong>(tmp.Length);
                tmp.AsMemory<ulong>().CopyTo(tmp1);
            }
        }

        [Benchmark]
        public static void UninitializedInitMemoryCopyPinned()
        {
            //don't use static variables in iterations
            ulong[] tmp = stringUlong;
            int count = iterations;
            for (int i = 0; i < count; i++)
            {
                ulong[] tmp1 = GC.AllocateUninitializedArray<ulong>(tmp.Length, true);
                tmp.CopyTo((Memory<ulong>)tmp1);
            }
        }

        [Benchmark]
        public static void UninitializedInitBufferBlockCopy()
        {
            //don't use static variables in iterations
            ulong[] tmp = stringUlong;
            int count = iterations;
            for (int i = 0; i < count; i++)
            {
                ulong[] tmp1 = GC.AllocateUninitializedArray<ulong>(tmp.Length);
                Buffer.BlockCopy(tmp, 0, tmp1, 0, tmp.Length * 8);
            }
        }

        [Benchmark]
        public static void UninitializedInitArrayCopy()
        {
            //don't use static variables in iterations
            ulong[] tmp = stringUlong;
            int count = iterations;
            for (int i = 0; i < count; i++)
            {
                ulong[] tmp1 = GC.AllocateUninitializedArray<ulong>(tmp.Length);
                Array.Copy(tmp, 0, tmp1, 0, tmp.Length);
            }
        }
        #endregion
    }
}
