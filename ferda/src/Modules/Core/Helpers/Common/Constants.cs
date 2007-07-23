// Constants.cs - Global constants for the system
//
// Author: Tomáš Kuchaø <tomas.kuchar@gmail.com>
//
// Copyright (c) 2006 Tomáš Kuchaø
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

namespace Ferda.Modules.Helpers.Common
{
	/// <summary>
	/// In this class are defined some constats.
	/// </summary>
    public static class Constants
	{
		private const string dbNullCategoryName = "_DbNull";
        /// <summary>
        /// Gets the name of the (DB) null category in attribute.
        /// </summary>
        /// <value>The name of the (DB) null category in attribute.</value>
		public static string DbNullCategoryName
		{
			get { return dbNullCategoryName; }
		}

        private const string emptyStringCategoryName = "_EmptyString";
        /// <summary>
        /// Gets the empty name of the string category.
        /// </summary>
        /// <value>The empty name of the string category.</value>
        public static string EmptyStringCategoryName
        {
            get { return emptyStringCategoryName; }
        } 

		private const char leftClosedInterval = '<';
        /// <summary>
        /// Gets the left closed interval char.
        /// </summary>
        /// <value>The left closed interval char.</value>
		public static char LeftClosedInterval
		{
			get { return leftClosedInterval; }
		}

		private const char rightClosedInterval = '>';
        /// <summary>
        /// Gets the right closed interval char.
        /// </summary>
        /// <value>The right closed interval char.</value>
		public static char RightClosedInterval
		{
			get { return rightClosedInterval; }
		}

		private const char leftOpenedInterval = '(';
        /// <summary>
        /// Gets the left opened interval char.
        /// </summary>
        /// <value>The left opened interval char.</value>
		public static char LeftOpenedInterval
		{
			get { return leftOpenedInterval; }
		} 

		private const char rightOpenedInterval = ')';
        /// <summary>
        /// Gets the right opened interval char.
        /// </summary>
        /// <value>The right opened interval char.</value>
		public static char RightOpenedInterval
		{
			get { return rightOpenedInterval; }
		} 

		private const char separatorInterval = ';';
        /// <summary>
        /// Gets the separator interval char.
        /// </summary>
        /// <value>The separator interval char.</value>
		public static char SeparatorInterval
		{
			get { return separatorInterval; }
		} 

		private const char leftEnum = '[';
        /// <summary>
        /// Gets the left enum char.
        /// </summary>
        /// <value>The left enum char.</value>
		public static char LeftEnum
		{
			get { return leftEnum; }
		} 

		private const char rightEnum = ']';
        /// <summary>
        /// Gets the right enum char.
        /// </summary>
        /// <value>The right enum char.</value>
		public static char RightEnum
		{
			get { return rightEnum; }
		} 

		private const char separatorEnum = ',';
        /// <summary>
        /// Gets the separator enum char.
        /// </summary>
        /// <value>The separator enum char.</value>
		public static char SeparatorEnum
		{
			get { return separatorEnum; }
		} 

		private const char negation = '\u00AC';
        /// <summary>
        /// Gets the negation char.
        /// </summary>
        /// <value>The negation char.</value>
		public static char Negation
		{
			get { return negation; }
		}

		private const char leftFunctionBracket = '(';
        /// <summary>
        /// Gets the left function bracket char.
        /// </summary>
        /// <value>The left function bracket char.</value>
		public static char LeftFunctionBracket
		{
			get { return leftFunctionBracket; }
		}

		private const char rightFunctionBracket = ')';
        /// <summary>
        /// Gets the right function bracket char.
        /// </summary>
        /// <value>The right function bracket char.</value>
		public static char RightFunctionBracket
		{
			get { return rightFunctionBracket; }
		}

		private const char leftSetBracket = '{';
        /// <summary>
        /// Gets the left set bracket char.
        /// </summary>
        /// <value>The left set bracket char.</value>
		public static char LeftSetBracket
		{
			get { return leftSetBracket; }
		}

		private const char rightSetBracket = '}';
        /// <summary>
        /// Gets the right set bracket char.
        /// </summary>
        /// <value>The right set bracket char.</value>
		public static char RightSetBracket
		{
			get { return rightSetBracket; }
		}

		private const char rangeSeparator = '-';
        /// <summary>
        /// Gets the range separator char.
        /// </summary>
        /// <value>The range separator char.</value>
		public static char RangeSeparator
		{
			get { return rangeSeparator; }
		}

		private const char logicalAnd = '&';
        /// <summary>
        /// Gets the logical and char.
        /// </summary>
        /// <value>The logical and char.</value>
		public static char LogicalAnd
		{
			get { return logicalAnd; }
		}
	}
}