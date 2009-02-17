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
        #region Init, Reset and Check

        public static void Init(string[] args)
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


        }

        #endregion
    }
}
