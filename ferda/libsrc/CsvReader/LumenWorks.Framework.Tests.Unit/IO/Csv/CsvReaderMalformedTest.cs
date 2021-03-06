//	LumenWorks.Framework.Tests.Unit.IO.CSV.CsvReaderMalformedTest
//	Copyright (c) 2005 Sébastien Lorion
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


// A special thanks goes to "shriop" at CodeProject for providing many of the standard and Unicode parsing tests.


using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

using NUnit.Framework;

using LumenWorks.Framework.IO.Csv;

namespace LumenWorks.Framework.Tests.Unit.IO.Csv
{
	[TestFixture()]
	public class CsvReaderMalformedTest
	{
		private void MissingFieldUnquotedTest1(MissingFieldAction action)
		{
			const string Data = "a,b,c,d\n1,1,1,1\n2,2\n3,3,3,3";

			try
			{
				using (CsvReader csv = new CsvReader(new StringReader(Data), false))
				{
					csv.MissingFieldAction = action;

					while (csv.ReadNextRecord())
						for (int i = 0; i < csv.FieldCount; i++)
						{
							string s = csv[i];

							if (csv.CurrentRecordIndex == 2)
							{
								if (i > 0)
								{
									switch (action)
									{
										case MissingFieldAction.ReturnEmptyValue:
											Assert.AreEqual(string.Empty, s);
											break;

										case MissingFieldAction.ReturnNullValue:
											Assert.IsNull(s);
											break;

										case MissingFieldAction.ReturnPartiallyParsedValue:
											if (i == 1)
												Assert.AreEqual("2", s);
											else
												Assert.AreEqual(string.Empty, s);
											break;

										default:
											Assert.Fail(string.Format("'{0}' is not handled by this test.", action));
											break;
									}
								}
							}
						}
				}
			}
			catch (MissingFieldCsvException ex)
			{
				if (ex.CurrentRecordIndex == 2 && ex.CurrentPosition == 19)
					throw ex;
			}
		}

		[Test()]
		[ExpectedException(typeof(MissingFieldCsvException))]
		public void MissingFieldUnquotedTest1_TreatAsParseError()
		{
			MissingFieldUnquotedTest1(MissingFieldAction.TreatAsParseError);
		}

		[Test()]
		public void MissingFieldUnquotedTest1_ReturnEmptyValue()
		{
			MissingFieldUnquotedTest1(MissingFieldAction.ReturnEmptyValue);
		}

		[Test()]
		public void MissingFieldUnquotedTest1_ReturnNullValue()
		{
			MissingFieldUnquotedTest1(MissingFieldAction.ReturnNullValue);
		}

		[Test()]
		public void MissingFieldUnquotedTest1_ReturnPartiallyParsedValue()
		{
			MissingFieldUnquotedTest1(MissingFieldAction.ReturnPartiallyParsedValue);
		}

		private void MissingFieldUnquotedTest2(MissingFieldAction action)
		{
			const string Data = "a,b,c,d\n1,1,1,1\n2,2,2\n3,3,3,3";

			try
			{
				// With bufferSize = 10, faulty new line char is at the start of next buffer read
				using (CsvReader csv = new CsvReader(new StringReader(Data), false, 7))
				{
					csv.MissingFieldAction = action;

					while (csv.ReadNextRecord())
						for (int i = 0; i < csv.FieldCount; i++)
						{
							string s = csv[i];

							if (csv.CurrentRecordIndex == 2)
							{
								if (i > 1)
								{
									switch (action)
									{
										case MissingFieldAction.ReturnEmptyValue:
											Assert.AreEqual(string.Empty, s);
											break;

										case MissingFieldAction.ReturnNullValue:
											Assert.IsNull(s);
											break;

										case MissingFieldAction.ReturnPartiallyParsedValue:
											if (i == 2)
												Assert.AreEqual("2", s);
											else
												Assert.AreEqual(string.Empty, s);
											break;

										default:
											Assert.Fail(string.Format("'{0}' is not handled by this test.", action));
											break;
									}
								}
							}
						}
				}
			}
			catch (MissingFieldCsvException ex)
			{
				if (ex.CurrentRecordIndex == 2 && ex.CurrentPosition == 0)
					throw ex;
			}
		}

		[Test()]
		[ExpectedException(typeof(MissingFieldCsvException))]
		public void MissingFieldUnquotedTest2_TreatAsParseError()
		{
			MissingFieldUnquotedTest2(MissingFieldAction.TreatAsParseError);
		}

		[Test()]
		public void MissingFieldUnquotedTest2_ReturnEmptyValue()
		{
			MissingFieldUnquotedTest2(MissingFieldAction.ReturnEmptyValue);
		}

		[Test()]
		public void MissingFieldUnquotedTest2_ReturnNullValue()
		{
			MissingFieldUnquotedTest2(MissingFieldAction.ReturnNullValue);
		}

		[Test()]
		public void MissingFieldUnquotedTest2_ReturnPartiallyParsedValue()
		{
			MissingFieldUnquotedTest2(MissingFieldAction.ReturnPartiallyParsedValue);
		}

		[Test()]
		[ExpectedException(typeof(MissingFieldCsvException))]
		public void MissingFieldQuotedTest1()
		{
			const string Data = "a,b,c,d\n1,1,1,1\n2,\"2\"\n3,3,3,3";

			try
			{
				using (CsvReader csv = new CsvReader(new StringReader(Data), false))
				{
					while (csv.ReadNextRecord())
						for (int i = 0; i < csv.FieldCount; i++)
						{
							string s = csv[i];
						}
				}
			}
			catch (MissingFieldCsvException ex)
			{
				if (ex.CurrentRecordIndex == 2 && ex.CurrentPosition == 21)
					throw ex;
			}
		}

		[Test()]
		[ExpectedException(typeof(MissingFieldCsvException))]
		public void MissingFieldQuotedTest2()
		{
			const string Data = "a,b,c,d\n1,1,1,1\n2,\"2\",\n3,3,3,3";

			try
			{
				using (CsvReader csv = new CsvReader(new StringReader(Data), false, 11))
				{
					while (csv.ReadNextRecord())
						for (int i = 0; i < csv.FieldCount; i++)
						{
							string s = csv[i];
						}
				}
			}
			catch (MissingFieldCsvException ex)
			{
				if (ex.CurrentRecordIndex == 2 && ex.CurrentPosition == 0)
					throw ex;
			}
		}

		[Test()]
		[ExpectedException(typeof(MissingFieldCsvException))]
		public void MissingFieldQuotedTest3()
		{
			const string Data = "a,b,c,d\n1,1,1,1\n2,\"2\"\n\"3\",3,3,3";

			try
			{
				using (CsvReader csv = new CsvReader(new StringReader(Data), false))
				{
					while (csv.ReadNextRecord())
						for (int i = 0; i < csv.FieldCount; i++)
						{
							string s = csv[i];
						}
				}
			}
			catch (MissingFieldCsvException ex)
			{
				if (ex.CurrentRecordIndex == 2 && ex.CurrentPosition == 21)
					throw ex;
			}
		}

		[Test()]
		[ExpectedException(typeof(MissingFieldCsvException))]
		public void MissingFieldQuotedTest4()
		{
			const string Data = "a,b,c,d\n1,1,1,1\n2,\"2\",\n\"3\",3,3,3";

			try
			{
				using (CsvReader csv = new CsvReader(new StringReader(Data), false, 11))
				{
					while (csv.ReadNextRecord())
						for (int i = 0; i < csv.FieldCount; i++)
						{
							string s = csv[i];
						}
				}
			}
			catch (MissingFieldCsvException ex)
			{
				if (ex.CurrentRecordIndex == 2 && ex.CurrentPosition == 0)
					throw ex;
			}
		}
	}
}
