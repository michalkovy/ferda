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
using System.Numerics;

namespace Ferda.Benchmark
{
	public abstract class FerdaBenchmark
    {
        #region Fields

        protected static int iterations = 10000;
        protected const int lengthUlongString = 100000;

        protected static ulong[] stringUlong = new ulong[lengthUlongString];
        protected static Vector4[] stringVector4 = new Vector4[LengthVector4String];
        protected static Vector<ulong>[] stringVectorUlong1 = new Vector<ulong>[lengthUlongString / Vector<ulong>.Count];
        protected static float[] stringFloat = new float[LengthFloatString];

        /// <summary>
        /// The second ulong array - for conjunction and disjunction
        /// </summary>
        protected static ulong[] stringUlong2 = new ulong[lengthUlongString];

        /// <summary>
        /// The second Vector4 array - for conjunction and disjunction
        /// </summary>
        protected static Vector4[] stringVector42 = new Vector4[LengthVector4String];

        protected static Vector<ulong>[] stringVectorUlong2 = new Vector<ulong>[lengthUlongString / Vector<ulong>.Count];

        /// <summary>
        /// The second float array - for conjunction and disjunction
        /// </summary>
        protected static float[] stringFloat2 = new float[LengthFloatString];

        /// <summary>
        /// The length of the Vector4 string
        /// </summary>
        protected static int LengthVector4String
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

        /// <summary>
        /// The ULONG representing 1
        /// </summary>
        protected const ulong _one = 1ul;

        ////uint precomputed vectors
        //protected static Vector<uint> sui1 = new Vector<uint>(new uint[] { 0x00000001, 0x00000002, 0x00000004, 0x00000008 });
        //protected static Vector<uint> sui2 = new Vector<uint>(new uint[] { 0x00000010, 0x00000020, 0x00000040, 0x00000080 });
        //protected static Vector<uint> sui3 = new Vector<uint>(new uint[] { 0x00000100, 0x00000200, 0x00000400, 0x00000800 });
        //protected static Vector<uint> sui4 = new Vector<uint>(new uint[] { 0x00001000, 0x00002000, 0x00004000, 0x00008000 });
        //protected static Vector<uint> sui5 = new Vector<uint>(new uint[] { 0x00010000, 0x00020000, 0x00040000, 0x00080000 });
        //protected static Vector<uint> sui6 = new Vector<uint>(new uint[] { 0x00100000, 0x00200000, 0x00400000, 0x00800000 });
        //protected static Vector<uint> sui7 = new Vector<uint>(new uint[] { 0x01000000, 0x02000000, 0x04000000, 0x08000000 });
        //protected static Vector<uint> sui8 = new Vector<uint>(new uint[] { 0x10000000, 0x20000000, 0x40000000, 0x80000000 });
        ////float precomputed vectors
        //protected static Vector4 sf1 = new Vector4(0x00000001, 0x00000002, 0x00000004, 0x00000008);
        //protected static Vector4 sf2 = new Vector4(0x00000010, 0x00000020, 0x00000040, 0x00000080);
        //protected static Vector4 sf3 = new Vector4(0x00000100, 0x00000200, 0x00000400, 0x00000800);
        //protected static Vector4 sf4 = new Vector4(0x00001000, 0x00002000, 0x00004000, 0x00008000);
        //protected static Vector4 sf5 = new Vector4(0x00010000, 0x00020000, 0x00040000, 0x00080000);
        //protected static Vector4 sf6 = new Vector4(0x00100000, 0x00200000, 0x00400000, 0x00800000);
        //protected static Vector4 sf7 = new Vector4(0x01000000, 0x02000000, 0x04000000, 0x08000000);
        //protected static Vector4 sf8 = new Vector4(0x10000000, 0x20000000, 0x40000000, 0x80000000);

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
            for (int i = 0; i < lengthUlongString / Vector<ulong>.Count; i++)
            {
                stringVectorUlong1[i] = new Vector<ulong>(stringUlong, i * Vector<ulong>.Count);
            }

            for (int i = 0; i < LengthFloatString; i++)
            {
                stringFloat[i] = (float)r.NextDouble();
            }

            for (int i = 0; i < LengthVector4String; i++)
            {
                Vector4 tmp = new Vector4((float)r.NextDouble(),
                    (float)r.NextDouble(),
                    (float)r.NextDouble(),
                    (float)r.NextDouble());
                stringVector4[i] = tmp;
            }
            for (int i = 0; i < lengthUlongString / Vector<ulong>.Count; i++)
            {
                stringVectorUlong2[i] = new Vector<ulong>(stringUlong2, i * Vector<ulong>.Count);
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

            for (int i = 0; i < LengthVector4String; i++)
            {
                Vector4 tmp = new Vector4((float)r.NextDouble(),
                    (float)r.NextDouble(),
                    (float)r.NextDouble(),
                    (float)r.NextDouble());
                stringVector42[i] = tmp;
            }
        }

        #endregion
    }
}
