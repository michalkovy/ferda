using Ferda.ModulesManager;
using NUnit.Framework;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Reflection;
using System;
using Ferda.Guha.MiningProcessor.BitStrings;

namespace Ferda.Modules
{
	[TestFixture]
	public class MiningTest
	{
		private BitString[] bitStrings = new BitString[4];
		private FalseBitString falseBitString = FalseBitString.GetInstance();
	
		[TestFixtureSetUp]
		public void SetUp()
		{
			Random random = new Random();
			for (int i = 0; i < 4; i++)
			{
				long[] bits = new long[1000];
				for (int j = 0; j < 1000; j++)
				{
					bits[j] = random.Next() + (random.Next() << 32);
				}
				bitStrings[i] = new BitString(new BitStringIdentifier(i.ToString(), i.ToString()), 64000, bits);
			}
		}
		
		[Test]
		public void Test_CrossAndSum()
		{
			int sum1, sum2, sum3, sum4;
			BitString.CrossAndSum(bitStrings[0], falseBitString, bitStrings[1], falseBitString,
				(int)bitStrings[0].Sum, 0, (int)bitStrings[1].Sum, 0,
				out sum1, out sum2, out sum3, out sum4);
			Assert.AreEqual(0, sum2);
			Assert.AreEqual(0, sum3);
			Assert.AreEqual(0, sum4);
			Assert.IsTrue(sum1 > 0);
			
			BitString.CrossAndSum(bitStrings[0], bitStrings[1], bitStrings[2], bitStrings[3],
				(int)bitStrings[0].Sum, (int)bitStrings[1].Sum, (int)bitStrings[2].Sum, (int)bitStrings[3].Sum,
				out sum1, out sum2, out sum3, out sum4);
			Assert.IsTrue(sum1 > 0);
			Assert.IsTrue(sum2 > 0);
			Assert.IsTrue(sum3 > 0);
			Assert.IsTrue(sum4 > 0);
		}
	}
}
