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

        private static int LengthVector4fString
        {
            get { return lengthUlongString * 64 / 4; }
        }


        #endregion
    }
}
