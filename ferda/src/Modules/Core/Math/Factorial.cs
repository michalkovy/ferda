// Factorial.cs - Functions for fast computation of factorials
//
// Author: Peter Luschny
//
// Copyright (c) Peter Luschny
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

namespace Ferda.Guha.Math
{
    /// <summary>
    /// Provides fast algorithms for factorials computation. These functoins
    /// were created by Peter Luschny.
    /// </summary>
    /// <remarks>
    /// <para>
    /// http://www.luschny.de/math/factorial/FastFactorialFunctions.htm
    /// http://www.luschny.de/math/factorial/approx/SimpleCases.html
    /// http://www.luschny.de/math/factorial/conclusions.html
    /// http://groups.google.com/group/sci.math.num-analysis/msg/521fa1a6fb98a300
    /// A convergent expansion for the gamma function -- different from any expansion 
    /// of Lanczos, and presumably new -- is presented. It converges rapidly when Re(z) 
    /// is large, and is thus well suited for computation of the gamma function.  
    /// </para>
    /// <para>
    /// http://home.att.net/~numericana/answer/info/godfrey.htm
    /// I read your info about the Gamma function
    /// and hope that the enclosed may be of some help.
    /// The first document is a Matlab implementation
    /// of the complex Gamma function good to 13 digits
    /// everywhere in the complex plane.
    /// The second document explains how to EASILY compute
    /// the exact Lanczos coefficients with minimum roundoff error.
    /// http://oldmill.uchicago.edu/~wilder/Code/hpgamma/
    /// Compute Gamma(x) (and x factorial) to arbitrary high precisions
    /// </para>
    /// </remarks>
    public class Factorial
    {
        /*
         * poznamky
         * 
         * double ... 171! = 
         * max double je asi 1.8 x 10^308
         * double.ToString max 15 cisel za des. carkou
         * 
         * long ... 21! = 5.1 x 10^19
         * max long je 9 x 10^18
         * max ulong je 18,5 x 10^18
         * */
        public static double Gamma(double nPlusOne)
        {
            return System.Math.Exp(LnGamma(nPlusOne));
        }

        private static double[] cof = new double[6]
            {
                76.18009172947146,
                -86.50532032941677,
                24.01409824083091,
                -1.231739572450155,
                0.1208650973866179e-2,
                -0.5395239384953e-5
            };

        public static double LnGamma(double nPlusOne)
        {
            if (nPlusOne <= 0)
                throw new ArgumentException();
            double x, y, tmp, ser;
            int j;
            y = x = nPlusOne;
            tmp = x + 5.5;
            tmp -= (x + 0.5)*System.Math.Log(tmp);
            ser = 1.000000000190015;
            for (j = 0; j <= 5; j++)
            {
                ser += cof[j]/++y;
            }
            return -tmp + System.Math.Log(2.5066282746310005*ser/x);
        }

        public static double LnFact(double n)
        {
            if (n < 0)
                throw new ArgumentException();
            if (n == 1 || n == 0)
                return 0.0;
                //if (n <= 100) return a[n] ? a[n] : (a[n]=gammln(n+1.0)); In range of table.
            else
                return LnGamma(n + 1);
        }

        public static long BiCo(int n, int k, out bool overflow)
        {
            double result = System.Math.Floor(0.5 + System.Math.Exp(LnFact(n) - LnFact(k) - LnFact(n - k)));
            if (result > Int64.MaxValue)
                overflow = true;
            else
                overflow = false;
            return (long) result;
            //The floor function cleans up roundoff error for smaller values of n and k.
        }

        class Cantrell
        {
            const double c1 = 0.71251855517070757051E+0;
            const double c2 = 0.54503922554948506451E+1;
            const double c3 = 0.33106364977586039145E+0;
            const double c4 = 0.29914210678640645176E+1;

            const double sqrt2pi = 0.25066282746310005024E+1;

            private double cf4(double z)
            {
                return 1.0/(c1*z + 1.0/(c2*z + 1.0/(c3*z + 1.0/(c4*z))));
            }

            private double cantrell(double z)
            {
                return sqrt2pi*System.Math.Exp(z*(System.Math.Log(z) - 1.0))
                       /(1.0 + 1.0/(24.0*z - 0.5 + cf4(z)));
            }

            private double shift6(double z)
            {
                return (z + 1.0)*(z + 2.0)*(z + 3.0)
                       *(z + 4.0)*(z + 5.0)*(z + 6.0);
            }

            public double factorial(double z)
            {
                if (z < 0.0)
                    throw new ArgumentOutOfRangeException(
                        "Argument must be >= 0.0 but was " + z);

                if (z > 170.0)
                    throw new ArgumentOutOfRangeException(
                        "Argument must be <= 170.0 but was " + z);

                if (z < 8) return cantrell(z + 6.5)/shift6(z);

                return cantrell(z + 0.5);
            }
        }
    }
}