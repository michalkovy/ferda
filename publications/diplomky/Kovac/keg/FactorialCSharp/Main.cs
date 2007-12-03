// Main.cs
//
//  Copyright (C) 2007 Michal Kováč <michal.kovac.develop@centrum.cz>
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
// project created on 2.12.2007 at 17:19
using System;

namespace FactorialCSharp
{
	class MainClass
	{
		public static void Main(string[] args)
		{
			Console.WriteLine(Factorial(5).ToString());
		}
		
public static int Factorial(int x)
{
	if (x == 0)
	{
		return 1;
	}
	else
	{
		return x * Factorial(x - 1);
	}
}

public static int Factorial2(int x)
{
	return (x == 0) ? 1 : x * Factorial2(x - 1);
}
	}
}