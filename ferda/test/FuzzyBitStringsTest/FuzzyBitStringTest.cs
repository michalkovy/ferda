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
using Ferda.Guha.Math;

namespace FuzzyBitStringsTest
{
    /// <summary>
    /// Test of functionality of bit strings
    /// </summary>
    [TestFixture]
    public class FuzzyBitStringTest
    {
        /// <summary>
        /// The dummy bit string identifier
        /// </summary>
        BitStringIdentifier o = new BitStringIdentifier("x", "y");
        /// <summary>
        /// Generator of random numbers
        /// </summary>
        Random r = new Random();

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

            FuzzyBitString fb1 = new FuzzyBitString(o, fa1, true);
            FuzzyBitString fb2 = new FuzzyBitString(o, fa2, true);
            FuzzyBitString fb3 = new FuzzyBitString(o, fa3, true);
            FuzzyBitString fb4 = new FuzzyBitString(o, fa4, true);
            FuzzyBitString fb5 = new FuzzyBitString(o, fa5, true);
            FuzzyBitString fb6 = new FuzzyBitString(o, fa6, true);
            FuzzyBitString fb7 = new FuzzyBitString(o, fa7, true);
            FuzzyBitString fb8 = new FuzzyBitString(o, fa8, true);
            FuzzyBitString fb9 = new FuzzyBitString(o, fa9, true);
            FuzzyBitString fb10 = new FuzzyBitString(o, fa10, true);
            FuzzyBitString fb11 = new FuzzyBitString(o, fa11, true);
            FuzzyBitString fb12 = new FuzzyBitString(o, fa12, true);

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

        [Test]
        public void FillTest()
        {
            Debug.WriteLine("Fill(..) test");

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

            FuzzyBitString fb1 = new FuzzyBitString(o, fa1, true);
            FuzzyBitString fb2 = new FuzzyBitString(o, fa2, true);
            FuzzyBitString fb3 = new FuzzyBitString(o, fa3, true);
            FuzzyBitString fb4 = new FuzzyBitString(o, fa4, true);
            FuzzyBitString fb5 = new FuzzyBitString(o, fa5, true);
            FuzzyBitString fb6 = new FuzzyBitString(o, fa6, true);
            FuzzyBitString fb7 = new FuzzyBitString(o, fa7, true);
            FuzzyBitString fb8 = new FuzzyBitString(o, fa8, true);
            FuzzyBitString fb9 = new FuzzyBitString(o, fa9, true);
            FuzzyBitString fb10 = new FuzzyBitString(o, fa10, true);
            FuzzyBitString fb11 = new FuzzyBitString(o, fa11, true);
            FuzzyBitString fb12 = new FuzzyBitString(o, fa12, true);

            fb1.Fill(0.1f);
            fb2.Fill(0.2f);
            fb3.Fill(0.3f);
            fb4.Fill(0.4f);
            fb5.Fill(0.5f);
            fb6.Fill(0.6f);
            fb7.Fill(0.7f);
            fb8.Fill(0.8f);
            fb9.Fill(0.9f);
            fb10.Fill(0.10f);
            fb11.Fill(0.11f);
            fb12.Fill(0.12f);

            Debug.WriteLine(fb1.ToString());
            Debug.WriteLine(fb2.ToString());
            Debug.WriteLine(fb3.ToString());
            Debug.WriteLine(fb4.ToString());
            Debug.WriteLine(fb5.ToString());
            Debug.WriteLine(fb6.ToString());
            Debug.WriteLine(fb7.ToString());
            Debug.WriteLine(fb8.ToString());
            Debug.WriteLine(fb9.ToString());
            Debug.WriteLine(fb10.ToString());
            Debug.WriteLine(fb11.ToString());
            Debug.WriteLine(fb12.ToString());

            Debug.WriteLine("Fill(..) test successful");
        }

        [Test]
        public void GetBitTest()
        {
            Debug.WriteLine("GetBit(..) test");

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

            float[] fa12 = new float[] { f1, f2, f3, f4, f5, f6, f7, f8, f9, f10, f11, f12 };
            FuzzyBitString fb12 = new FuzzyBitString(o, fa12, true);
            for (int i = 0; i < 12; i++)
            {
                Debug.WriteLine(fb12.GetBit(i));
            }

            Debug.WriteLine("GetBit(..) test successful");
        }

        [Test]
        public void SetBitTest()
        {
            Debug.WriteLine("SetBit(..) test");

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

            float[] fa12 = new float[] { f1, f2, f3, f4, f5, f6, f7, f8, f9, f10, f11, f12 };
            FuzzyBitString fb12 = new FuzzyBitString(o, fa12, true);
            for (int i = 0; i < 12; i++)
            {
                fb12.SetBit(i, 0f);
            }
            Debug.WriteLine(fb12.ToString());
            Debug.WriteLine("SetBit(..) test successful");
        }

        [Test]
        public void NonZeroBitsCountTest()
        {
            Debug.WriteLine("NonZeroBitsCount(..) test");
            
            float[] bitField = new float[100];

            int count = 0;
            for (int i = 0; i < 100; i++)
            {
                int tmp = r.Next(2);
                if (tmp == 1)
                {
                    count++;
                }
                bitField[i] = tmp;
            }
            Debug.WriteLine(count.ToString());


            FuzzyBitString fb = new FuzzyBitString(o, bitField, true);
            Debug.WriteLine(fb.NonZeroBitsCount);

            Debug.Assert(fb.NonZeroBitsCount == count);            
            Debug.WriteLine("NonZeroBitsCount(..) test successful");
        }

        [Test]
        public void SumTest()
        {
            Debug.WriteLine("Sum(..) test");

            float[] bitField1000 = new float[1000];
            float[] bitField1001 = new float[1001];
            float[] bitField1002 = new float[1002];
            float[] bitField1003 = new float[1003];

            for (int i = 0; i < 1000; i++)
            {
                bitField1000[i] = Convert.ToSingle(r.NextDouble());
                bitField1001[i] = bitField1000[i];
                bitField1002[i] = bitField1000[i];
                bitField1003[i] = bitField1000[i];

            }
            bitField1001[1000] = Convert.ToSingle(Convert.ToSingle(r.NextDouble()));
            bitField1002[1000] = Convert.ToSingle(Convert.ToSingle(r.NextDouble()));
            bitField1003[1000] = Convert.ToSingle(Convert.ToSingle(r.NextDouble()));
            bitField1002[1001] = Convert.ToSingle(Convert.ToSingle(r.NextDouble()));
            bitField1003[1001] = Convert.ToSingle(Convert.ToSingle(r.NextDouble()));
            bitField1003[1002] = Convert.ToSingle(Convert.ToSingle(r.NextDouble()));

            float sum1000 = 0f;
            float sum1001 = 0f;
            float sum1002 = 0f;
            float sum1003 = 0f;

            for (int i = 0; i < 1000; i++)
            {
                sum1000 += bitField1000[i];
            }
            sum1001 = sum1000 + bitField1001[1000];
            sum1002 = sum1000 + bitField1002[1000] + bitField1002[1001];
            sum1003 = sum1000 + bitField1003[1000] + bitField1003[1001] + bitField1003[1002];

            FuzzyBitString f1000 = new FuzzyBitString(o, bitField1000, true);
            FuzzyBitString f1001 = new FuzzyBitString(o, bitField1001, true);
            FuzzyBitString f1002 = new FuzzyBitString(o, bitField1002, true);
            FuzzyBitString f1003 = new FuzzyBitString(o, bitField1003, true);

            //Debug.WriteLine(sum1000);
            //Debug.WriteLine(f1000.Sum);
            Debug.Assert(Common.CloseEnough(4, sum1000, f1000.Sum));
            //Debug.WriteLine(sum1001);
            //Debug.WriteLine(f1001.Sum);
            Debug.Assert(Common.CloseEnough(4, sum1001, f1001.Sum));
            //Debug.WriteLine(sum1002);
            //Debug.WriteLine(f1002.Sum);
            Debug.Assert(Common.CloseEnough(4, sum1002, f1002.Sum));
            //Debug.WriteLine(sum1003);
            //Debug.WriteLine(f1003.Sum);
            Debug.Assert(Common.CloseEnough(4, sum1003, f1003.Sum));

            Debug.WriteLine("Sum(..) test successful");
        }

        [Test]
        public void NotTest()
        {
            Debug.WriteLine("Not(..) test");

            float[] bitField1000 = new float[1000];
            float[] bitField1001 = new float[1001];
            float[] bitField1002 = new float[1002];
            float[] bitField1003 = new float[1003];

            for (int i = 0; i < 1000; i++)
            {
                bitField1000[i] = Convert.ToSingle(r.NextDouble());
                bitField1001[i] = bitField1000[i];
                bitField1002[i] = bitField1000[i];
                bitField1003[i] = bitField1000[i];
            }
            bitField1001[1000] = Convert.ToSingle(Convert.ToSingle(r.NextDouble()));
            bitField1002[1000] = Convert.ToSingle(Convert.ToSingle(r.NextDouble()));
            bitField1003[1000] = Convert.ToSingle(Convert.ToSingle(r.NextDouble()));
            bitField1002[1001] = Convert.ToSingle(Convert.ToSingle(r.NextDouble()));
            bitField1003[1001] = Convert.ToSingle(Convert.ToSingle(r.NextDouble()));
            bitField1003[1002] = Convert.ToSingle(Convert.ToSingle(r.NextDouble()));

            FuzzyBitString f1000 = new FuzzyBitString(o, bitField1000, true);
            FuzzyBitString f1001 = new FuzzyBitString(o, bitField1001, true);
            FuzzyBitString f1002 = new FuzzyBitString(o, bitField1002, true);
            FuzzyBitString f1003 = new FuzzyBitString(o, bitField1003, true);

            for (int i = 0; i < 1000; i++)
            {
                bitField1000[i] = 1 - bitField1000[i];
                bitField1001[i] = 1 - bitField1001[i];
                bitField1002[i] = 1 - bitField1002[i];
                bitField1003[i] = 1 - bitField1003[i];
            }

            bitField1001[1000] = 1 - bitField1001[1000];
            bitField1002[1000] = 1 - bitField1002[1000];
            bitField1003[1000] = 1 - bitField1003[1000];
            bitField1002[1001] = 1 - bitField1002[1001];
            bitField1003[1001] = 1 - bitField1003[1001];
            bitField1003[1002] = 1 - bitField1003[1002];

            FuzzyBitString nf1000 = f1000.Not() as FuzzyBitString;
            FuzzyBitString nf1001 = f1001.Not() as FuzzyBitString;
            FuzzyBitString nf1002 = f1002.Not() as FuzzyBitString;
            FuzzyBitString nf1003 = f1003.Not() as FuzzyBitString;

            for (int i = 0; i < 1000; i++)
            {
                //Debug.WriteLine("Should be: " + nf1000.GetBit(i).ToString());
                //Debug.WriteLine("Result: " + bitField1000[i].ToString());
                if (!Common.CloseEnough(4, nf1000.GetBit(i), bitField1000[i]))
                {
                    throw new Exception("The Not() does not work");
                }
                if (!Common.CloseEnough(4, nf1001.GetBit(i), bitField1001[i]))
                {
                    throw new Exception("The Not() does not work");
                }
                if (!Common.CloseEnough(4, nf1002.GetBit(i), bitField1002[i]))
                {
                    throw new Exception("The Not() does not work");
                }
                if (!Common.CloseEnough(4, nf1003.GetBit(i), bitField1003[i]))
                {
                    throw new Exception("The Not() does not work");
                }
            }
            if (!Common.CloseEnough(4, nf1001.GetBit(1000), bitField1001[1000]))
            {
                throw new Exception("The Not() does not work");
            }
            if (!Common.CloseEnough(4, nf1002.GetBit(1000), bitField1002[1000]))
            {
                throw new Exception("The Not() does not work");
            }
            if (!Common.CloseEnough(4, nf1002.GetBit(1001), bitField1002[1001]))
            {
                throw new Exception("The Not() does not work");
            }
            if (!Common.CloseEnough(4, nf1003.GetBit(1000), bitField1003[1000]))
            {
                throw new Exception("The Not() does not work");
            }
            if (!Common.CloseEnough(4, nf1003.GetBit(1002), bitField1003[1002]))
            {
                throw new Exception("The Not() does not work");
            }

            Debug.WriteLine("Not(..) test successful");
        }

        /// <summary>
        /// Only if precomputed numbers work
        /// </summary>
        //[Test]
        //public void StupidAndTest()
        //{
        //    Debug.WriteLine("Stupid And(..) test");

        //    float[] f1 = new float[] { 0.1f, 0.2f, 0.3f, 0.4f };
        //    float[] f2 = new float[] { 0f, 0.1f, 0.2f, 0.3f };

        //    FuzzyBitString fb1 = new FuzzyBitString(o, f1, true);
        //    FuzzyBitString fb2 = new FuzzyBitString(o, f2, true);

        //    IBitString result = fb1.And(fb2);

        //    Debug.WriteLine(result.ToString());
        //}

        [Test]
        public void AndTest()
        {
            Debug.WriteLine("And(..) test");

            float[] bitField1000 = new float[1000];
            float[] bitField1000b = new float[1000];
            float[] bitField1001 = new float[1001];
            float[] bitField1001b = new float[1001];
            float[] bitField1002 = new float[1002];
            float[] bitField1002b = new float[1002];
            float[] bitField1003 = new float[1003];
            float[] bitField1003b = new float[1003];

            for (int i = 0; i < 1000; i++)
            {
                bitField1000[i] = Convert.ToSingle(r.NextDouble());
                bitField1000b[i] = Convert.ToSingle(r.NextDouble());
                bitField1001[i] = Convert.ToSingle(r.NextDouble());
                bitField1001b[i] = Convert.ToSingle(r.NextDouble());
                bitField1002[i] = Convert.ToSingle(r.NextDouble());
                bitField1002b[i] = Convert.ToSingle(r.NextDouble());
                bitField1003[i] = Convert.ToSingle(r.NextDouble());
                bitField1003b[i] = Convert.ToSingle(r.NextDouble());
            }

            bitField1001[1000] = Convert.ToSingle(Convert.ToSingle(r.NextDouble()));
            bitField1001b[1000] = Convert.ToSingle(Convert.ToSingle(r.NextDouble()));
            bitField1002[1000] = Convert.ToSingle(Convert.ToSingle(r.NextDouble()));
            bitField1002b[1000] = Convert.ToSingle(Convert.ToSingle(r.NextDouble()));
            bitField1003[1000] = Convert.ToSingle(Convert.ToSingle(r.NextDouble()));
            bitField1003b[1000] = Convert.ToSingle(Convert.ToSingle(r.NextDouble()));
            bitField1002[1001] = Convert.ToSingle(Convert.ToSingle(r.NextDouble()));
            bitField1002b[1001] = Convert.ToSingle(Convert.ToSingle(r.NextDouble()));
            bitField1003[1001] = Convert.ToSingle(Convert.ToSingle(r.NextDouble()));
            bitField1003b[1001] = Convert.ToSingle(Convert.ToSingle(r.NextDouble()));
            bitField1003[1002] = Convert.ToSingle(Convert.ToSingle(r.NextDouble()));
            bitField1003b[1002] = Convert.ToSingle(Convert.ToSingle(r.NextDouble()));

            FuzzyBitString f1000 = new FuzzyBitString(o, bitField1000, true);
            FuzzyBitString f1000b = new FuzzyBitString(o, bitField1000b, true);
            FuzzyBitString f1001 = new FuzzyBitString(o, bitField1001, true);
            FuzzyBitString f1001b = new FuzzyBitString(o, bitField1001b, true);
            FuzzyBitString f1002 = new FuzzyBitString(o, bitField1002, true);
            FuzzyBitString f1002b = new FuzzyBitString(o, bitField1002b, true);
            FuzzyBitString f1003 = new FuzzyBitString(o, bitField1003, true);
            FuzzyBitString f1003b = new FuzzyBitString(o, bitField1003b, true);

            FuzzyBitString f1000A = f1000.And(f1000b) as FuzzyBitString;
            FuzzyBitString f1001A = f1001.And(f1001b) as FuzzyBitString;
            FuzzyBitString f1002A = f1002.And(f1002b) as FuzzyBitString;
            FuzzyBitString f1003A = f1003.And(f1003b) as FuzzyBitString;

            for (int i = 0; i < 1000; i++)
            {
                //Debug.WriteLine(i);
                //Debug.WriteLine("Result: " + f1000A.GetBit(i).ToString());
                //Debug.WriteLine("Operand 1: " + bitField1000[i].ToString());
                //Debug.WriteLine("Operand 2: " + bitField1000b[i].ToString());
                //Debug.WriteLine("Should be: " + (bitField1000[i] * bitField1000b[i]).ToString());
                if (!Common.CloseEnough(4, f1000A.GetBit(i), bitField1000[i] * bitField1000b[i]))
                {
                    throw new Exception("The And() does not work");
                }
                if (!Common.CloseEnough(4, f1001A.GetBit(i), bitField1001[i] * bitField1001b[i]))
                {
                    throw new Exception("The And() does not work");
                }
                if (!Common.CloseEnough(4, f1002A.GetBit(i), bitField1002[i] * bitField1002b[i]))
                {
                    throw new Exception("The And() does not work");
                }
                if (!Common.CloseEnough(4, f1003A.GetBit(i), bitField1003[i] * bitField1003b[i]))
                {
                    throw new Exception("The And() does not work");
                }
            }

            if (!Common.CloseEnough(4, f1001A.GetBit(1000), bitField1001[1000] * bitField1001b[1000]))
            {
                throw new Exception("The And() does not work");
            }
            if (!Common.CloseEnough(4, f1002A.GetBit(1001), bitField1002[1001] * bitField1002b[1001]))
            {
                throw new Exception("The And() does not work");
            }
            if (!Common.CloseEnough(4, f1003A.GetBit(1002), bitField1003[1002] * bitField1003b[1002]))
            {
                throw new Exception("The And() does not work");
            }

            Debug.WriteLine("And(..) test successful");
        }

        /// <summary>
        /// Test of conjunction between fuzzy and crisp bit strings
        /// </summary>
        [Test]
        public void FuzzyCrispAndTest()
        {
            Debug.WriteLine("Crisp vs. fuzzy And(..) test");

            int[] lengths = new int[] { 400, 801, 1202, 1603 };
            float[] floatArray;
            bool[] boolArray;

            foreach (int length in lengths)
            {
                //filling the arrays
                floatArray = new float[length];
                boolArray = new bool[length];
                for (int i = 0; i < length; i++)
                {
                    floatArray[i] = (float)r.NextDouble();
                    boolArray[i] = Convert.ToBoolean(r.Next(0, 2));
                }

                //construction of the bit strings
                FuzzyBitString fb = new FuzzyBitString(o, floatArray, true);
                BitString b = new BitString(o, length, new long[(length + 63) / 64]);
                //setting the bits in the crisp bit string
                for (int i = 0; i < length; i++)
                {
                    b.SetBit(i, Convert.ToSingle(boolArray[i]));
                }

                FuzzyBitString result = fb.And(b) as FuzzyBitString;

                for (int i = 0; i < length; i++)
                {
                    //Debug.WriteLine("Boolean: " + boolArray[i].ToString());
                    //Debug.WriteLine("Float: " + floatArray[i].ToString());
                    //Debug.WriteLine("Result: " + result.GetBit(i).ToString());
                    //Debug.WriteLine("Should be: " + (floatArray[i] * Convert.ToSingle(boolArray[i])).ToString());
                    if (!Common.CloseEnough(6,
                        result.GetBit(i),
                        floatArray[i] * Convert.ToSingle(boolArray[i])))
                    {
                        throw new Exception("The crisp vs. fuzzy And() does not work");
                    }
                }
            }

            Debug.WriteLine("Crisp vs. fuzzy And(..) test successful");
        }
    }
}
