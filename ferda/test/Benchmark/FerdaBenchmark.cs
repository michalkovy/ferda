// FerdaBenchmark.cs - abstract class for all Ferda benchmarks
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
	public abstract class FerdaBenchmark
    {
        #region Fields

        protected static int iterations = 10000;
        protected const int lengthUlongString = 100000;

        protected static ulong[] stringUlong = new ulong[lengthUlongString];
        protected static Vector4f[] stringVector4f = new Vector4f[LengthVector4fString];
        protected static float[] stringFloat = new float[LengthFloatString];

        /// <summary>
        /// The second ulong array - for conjunction and disjunction
        /// </summary>
        protected static ulong[] stringUlong2 = new ulong[lengthUlongString];

        /// <summary>
        /// The second Vector4f array - for conjunction and disjunction
        /// </summary>
        protected static Vector4f[] stringVector4f2 = new Vector4f[LengthVector4fString];

        /// <summary>
        /// The second float array - for conjunction and disjunction
        /// </summary>
        protected static float[] stringFloat2 = new float[LengthFloatString];

        /// <summary>
        /// The length of the Vector4f string
        /// </summary>
        protected static int LengthVector4fString
        {
            get { return lengthUlongString * 64 / 4; }
        }

        /// <summary>
        /// The length of the float string
        /// </summary>
        protected static int LengthFloatString
        {
            get { return lengthUlongString * 64; }
        }

        /// <summary>
        /// The actual number of bitstrings in the Ulong and long fields
        /// </summary>
        protected static int LengthLongString
        {
            get { return lengthUlongString * 64;  }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Common initialization for the 
        /// </summary>
        /// <param name="args"></param>
        public static void CommonInit(string[] args)
        {
            if (args.Length > 0)
                iterations = Int32.Parse(args[0]);

            Random r = new Random();
            for (int i = 0; i < lengthUlongString; i++)
            {
                stringUlong[i] = (ulong)(uint)r.Next(Int32.MinValue,
                    Int32.MaxValue) |
                    (((ulong)(uint)r.Next(Int32.MinValue,
                    Int32.MaxValue)) << 32);
            }

            for (int i = 0; i < LengthFloatString; i++)
            {
                stringFloat[i] = (float)r.NextDouble();
            }

            for (int i = 0; i < LengthVector4fString; i++)
            {
                Vector4f tmp = new Vector4f((float)r.NextDouble(),
                    (float)r.NextDouble(),
                    (float)r.NextDouble(),
                    (float)r.NextDouble());
                stringVector4f[i] = tmp;
            }
        }

        /// <summary>
        /// The initialization used for conjunction and disjunction benchmark
        /// </summary>
        public static void ConjunctionDisjunctionInit()
        {
            Random r = new Random();

            for (int i = 0; i < lengthUlongString; i++)
            {
                stringUlong2[i] = (ulong)(uint)r.Next(Int32.MinValue,
                    Int32.MaxValue) |
                    (((ulong)(uint)r.Next(Int32.MinValue,
                    Int32.MaxValue)) << 32);
            }

            for (int i = 0; i < LengthFloatString; i++)
            {
                stringFloat2[i] = (float)r.NextDouble();
            }

            for (int i = 0; i < LengthVector4fString; i++)
            {
                Vector4f tmp = new Vector4f((float)r.NextDouble(),
                    (float)r.NextDouble(),
                    (float)r.NextDouble(),
                    (float)r.NextDouble());
                stringVector4f2[i] = tmp;
            }
        }

        #endregion
    }
}
