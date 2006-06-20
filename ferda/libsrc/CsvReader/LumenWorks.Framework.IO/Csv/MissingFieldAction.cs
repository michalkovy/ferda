//	LumenWorks.Framework.IO.CSV.MissingFieldAction
//	Copyright (c) 2006 Sébastien Lorion
//
//	Permission is hereby granted, free of charge, to any person obtaining a copy
//	of this software and associated documentation files (the "Software"), to deal
//	in the Software without restriction, including without limitation the rights 
//	to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies
//	of the Software, and to permit persons to whom the Software is furnished to do so, 
//	subject to the following conditions:
//
//	The above copyright notice and this permission notice shall be included in all 
//	copies or substantial portions of the Software.
//
//	THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
//	INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR
//	PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE 
//	FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//	ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System;

namespace LumenWorks.Framework.IO.Csv
{
	/// <summary>
	/// Specifies the action to take when a field is missing.
	/// </summary>
	public enum MissingFieldAction
	{
		/// <summary>
		/// Treat as a parsing error.
		/// </summary>
		TreatAsParseError = 0,

		/// <summary>
		/// Returns an empty value.
		/// </summary>
		ReturnEmptyValue = 1,

		/// <summary>
		/// Returns a null value (<see langword="null"/>).
		/// </summary>
		ReturnNullValue = 2,

		/// <summary>
		/// Returns the partially parsed value.
		/// </summary>
		ReturnPartiallyParsedValue = 3
	}
}