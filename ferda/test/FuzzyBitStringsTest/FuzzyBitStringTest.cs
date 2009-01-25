// FuzzyBitStringsTest.cs - NUnit tests for Fuzzy bit strings
//
// Author: Martin Ralbovský <martin.ralbovsky@gmail.cz>
//
// Copyright (c) 2009 Martin Ralbovský
//
// This program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using NUnit.Framework;
using Ferda.Guha.MiningProcessor.BitStrings;

namespace FuzzyBitStringsTest
{
    /// <summary>
    /// Test of functionality of bit strings
    /// </summary>
    [TestFixture]
    public class FuzzyBitStringTest
    {
        [TestFixtureSetUp]
        public void SetUp()
        {
            Debug.Listeners.Clear();
            TextWriterTraceListener t = new TextWriterTraceListener("log.txt");
            Debug.Listeners.Add(t);
            Debug.AutoFlush = true;
            Debug.WriteLine("The fuzzy bit strings test...");
        }

        [TestFixtureTearDown]
        public void TearDown()
        {
            Debug.WriteLine("End of fuzzy bit strings test");
        }

        [Test]
        public void ConstructorAndToStringTest()
        {
            Debug.WriteLine("Constructor and ToString() test");
            BitStringIdentifier o = new BitStringIdentifier("x", "y");

            float f1 = 0.01f;
            float f2 = 0.02f;
            float f3 = 0.03f;
            float f4 = 0.04f;
            float f5 = 0.05f;
            float f6 = 0.06f;
            float f7 = 0.07f;
            float f8 = 0.08f;
            float f9 = 0.09f;
            float f10 = 0.1f;
            float f11 = 0.11f;
            float f12 = 0.12f;

            float[] fa1 = new float[] { f1 };
            float[] fa2 = new float[] { f1, f2 };
            float[] fa3 = new float[] { f1, f2, f3 };
            float[] fa4 = new float[] { f1, f2, f3, f4 };
            float[] fa5 = new float[] { f1, f2, f3, f4, f5 };
            float[] fa6 = new float[] { f1, f2, f3, f4, f5, f6 };
            float[] fa7 = new float[] { f1, f2, f3, f4, f5, f6, f7 };
            float[] fa8 = new float[] { f1, f2, f3, f4, f5, f6, f7, f8 };
            float[] fa9 = new float[] { f1, f2, f3, f4, f5, f6, f7, f8, f9 };
            float[] fa10 = new float[] { f1, f2, f3, f4, f5, f6, f7, f8, f9, f10 };
            float[] fa11 = new float[] { f1, f2, f3, f4, f5, f6, f7, f8, f9, f10, f11 };
            float[] fa12 = new float[] { f1, f2, f3, f4, f5, f6, f7, f8, f9, f10, f11, f12 };

            FuzzyBitString fb1 = new FuzzyBitString(o, fa1);
            FuzzyBitString fb2 = new FuzzyBitString(o, fa2);
            FuzzyBitString fb3 = new FuzzyBitString(o, fa3);
            FuzzyBitString fb4 = new FuzzyBitString(o, fa4);
            FuzzyBitString fb5 = new FuzzyBitString(o, fa5);
            FuzzyBitString fb6 = new FuzzyBitString(o, fa6);
            FuzzyBitString fb7 = new FuzzyBitString(o, fa7);
            FuzzyBitString fb8 = new FuzzyBitString(o, fa8);
            FuzzyBitString fb9 = new FuzzyBitString(o, fa9);
            FuzzyBitString fb10 = new FuzzyBitString(o, fa10);
            FuzzyBitString fb11 = new FuzzyBitString(o, fa11);
            FuzzyBitString fb12 = new FuzzyBitString(o, fa12);

            Debug.WriteLine(fb1.ToString());
            Debug.WriteLine(fb1.Length);
            Debug.WriteLine(fb2.ToString());
            Debug.WriteLine(fb2.Length);
            Debug.WriteLine(fb3.ToString());
            Debug.WriteLine(fb3.Length);
            Debug.WriteLine(fb4.ToString());
            Debug.WriteLine(fb4.Length);
            Debug.WriteLine(fb5.ToString());
            Debug.WriteLine(fb5.Length);
            Debug.WriteLine(fb6.ToString());
            Debug.WriteLine(fb6.Length);
            Debug.WriteLine(fb7.ToString());
            Debug.WriteLine(fb7.Length);
            Debug.WriteLine(fb8.ToString());
            Debug.WriteLine(fb8.Length);
            Debug.WriteLine(fb9.ToString());
            Debug.WriteLine(fb9.Length);
            Debug.WriteLine(fb10.ToString());
            Debug.WriteLine(fb10.Length);
            Debug.WriteLine(fb11.ToString());
            Debug.WriteLine(fb11.Length);
            Debug.WriteLine(fb12.ToString());
            Debug.WriteLine(fb12.Length);
            Debug.WriteLine("Constructor and ToString() test successful");
        }
    }
}
