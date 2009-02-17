// Sum.cs
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
using Ferda.Guha.MiningProcessor.BitStrings;

namespace Ferda.Benchmark
{
	public class Not : FerdaBenchmark
    {
        #region Fields
        
        /// <summary>
        /// The (crisp) bit string to be tested
        /// </summary>
        static BitString bs;

        /// <summary>
        /// The fuzzy bit string to be tested
        /// </summary>
        static FuzzyBitString fbs;

        #endregion

        #region Init, Reset and Check

        /// <summary>
        /// Initializes the bit strings
        /// </summary>
        /// <param name="args">Arguments from the command line</param>
        public static void Init(string[] args)
        {
            if (args.Length > 0)
                iterations = Int32.Parse(args[0]);

            Random r = new Random();
            for (int i = 0; i < lengthUlongString; i++)
            {
                stringLong[i] = (long)(uint)r.Next(Int32.MinValue,
                    Int32.MaxValue) |
                    (((long)(uint)r.Next(Int32.MinValue,
                    Int32.MaxValue)) << 32);
            }

            for (int i = 0; i < LengthFloatString; i++)
            {
                stringFloat[i] = (float)r.NextDouble();
            }

            bs = new BitString(new BitStringIdentifier("1", "2"),
                LengthLongString, stringLong);
            fbs = new FuzzyBitString(new BitStringIdentifier("1", "2"),
                stringFloat, false);

        }

        #endregion

        #region Benchmark

        /// <summary>
        /// The crisp not benchmark
        /// </summary>
        [Benchmark]
        public static void NotCrisp()
        {
            //don't use static variables in iterations
            BitString bs2 = new BitString(bs);
            int count = iterations;
            for (int i = 0; i < count; i++)
            {
                bs2.Not();
            }
        }

        /// <summary>
        /// The fuzzy not benchmark
        /// </summary>
        [Benchmark]
        public static void NotFuzzy()
        {
            //don't use static variables in iterations
            FuzzyBitString fbs2 = new FuzzyBitString(fbs);
            int count = iterations;
            for (int i = 0; i < count; i++)
            {
                fbs2.Not();
            }
        }

        #endregion
    }
}
