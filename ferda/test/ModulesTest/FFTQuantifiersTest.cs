using System;
using System.Diagnostics;
using NUnit.Framework;


namespace Ferda.Modules.Quantifiers
{
    [TestFixture]
	public class FFTQuantifiersTest
    {
		private bool closeEnough(int precision, double x, double y)
        {
            if (precision < 1)
                throw new ArgumentOutOfRangeException("precision", precision, "The precision must be at least 1.");
            if ((x == 0.0) && (y == 0.0))
                return true;

            double level = (Math.Abs(x) > Math.Abs(y)) ? Math.Log10(Math.Abs(x)) : Math.Log10(Math.Abs(y));

            return (Math.Abs(x - y) < Math.Pow(10, level - precision));
        }

		FourFoldContingencyTable table;

		[TestFixtureSetUp]
		public void SetUp()
		{
			this.table = new FourFoldContingencyTable(30, 1, 23, 45);
		}

		[Test]
		public void CreateFourFoldTable00()
		{
			int[][] arrayTable = new int[2][] {
				new int[2] { 30, 1 },
				new int[2] { 23, 45 }
			};
			FourFoldContingencyTable table = new FourFoldContingencyTable(arrayTable);
			Assert.IsNotNull(table, "FFTQ001: FFTable wasn`t created!");
		}
		
		[Test]
		public void CreateFourFoldTable01()
		{
			int[][] arrayTable =
				new int[2][] {
					new int[2] { 30, 1 },
					new int[2] { 23, 45 }
				};
			FourFoldContingencyTable table = new FourFoldContingencyTable(arrayTable);
            Assert.IsNotNull(table, "FFTQ002: FFTable wasn`t created!");
		}

		[Test]
		[ExpectedException(typeof(Ferda.Modules.BadParamsError))]
		public void CreateFourFoldTable02()
		{
			int[][] arrayTable =
				new int[2][] {
					new int[2] { -30, 1 },
					new int[2] { 23, 45 }
				};
			FourFoldContingencyTable table = new FourFoldContingencyTable(arrayTable);
            Assert.IsNull(table, "FFTQ003: FFTable was created with frequency less than 0!");
		}

		[Test]
		[ExpectedException(typeof(Ferda.Modules.BadParamsError))]
		public void CreateFourFoldTable03()
		{
			int[][] arrayTable =
				new int[2][] {
					new int[1] { 0 },
					new int[3] { 23, 45, 0 }
				};
			FourFoldContingencyTable table = new FourFoldContingencyTable(arrayTable);
            Assert.IsNull(table, "FFTQ004: FFTable was created with bad shape of table (not int[2,2])!");
		}

		[Test]
		[ExpectedException(typeof(Ferda.Modules.BadParamsError))]
		public void CreateFourFoldTable04()
		{
			int[][] arrayTable =
				new int[3][] {
					new int[2] { 0, 5 },
					new int[2] { 23, 45 },
					new int[2] { 23, 45 }
				};
			FourFoldContingencyTable table = new FourFoldContingencyTable(arrayTable);
            Assert.IsNull(table, "FFTQ005: FFTable was created with bad shape of table (not int[2,2])!");
		}

		[Test]
		public void CreateFourFoldTable05()
		{
			long[,] arrayTable = new long[2, 2] {
					{ 30, 1 },
					{ 23, 45 }
				};
			FourFoldContingencyTable table = new FourFoldContingencyTable(arrayTable);
            Assert.IsNotNull(table, "FFTQ006: FFTable wasn`t created!");
		}

		[Test]
		public void ContingecyTable01()
		{
			FourFoldContingencyTable table = new FourFoldContingencyTable(103, 0, 5, 6074);
            Assert.AreEqual(table.K, 108, "FFTQ007");
            Assert.AreEqual(table.L, 6074, "FFTQ008");
            Assert.AreEqual(table.R, 103, "FFTQ009");
            Assert.AreEqual(table.S, 6079, "FFTQ010");
            Assert.AreEqual(table.N, 6182, "FFTQ011");
            Assert.AreEqual(table.A, 103, "FFTQ012");
            Assert.AreEqual(table.B, 0, "FFTQ013");
            Assert.AreEqual(table.C, 5, "FFTQ014");
            Assert.AreEqual(table.D, 6074, "FFTQ015");
		}

        [Test]
        public void ContingecyTable02()
        {
            FourFoldContingencyTable table = new FourFoldContingencyTable(103, 0, 5, 6074, 2);
            Assert.AreEqual(table.K, (double)108 / 2, "FFTQ016");
            Assert.AreEqual(table.L, (double)6074 / 2, "FFTQ017");
            Assert.AreEqual(table.R, (double)103 / 2, "FFTQ018");
            Assert.AreEqual(table.S, (double)6079 / 2, "FFTQ019");
            Assert.AreEqual(table.N, (double)6182 / 2, "FFTQ020");
            Assert.AreEqual(table.A, (double)103 / 2, "FFTQ021");
            Assert.AreEqual(table.B, (double)0 / 2, "FFTQ022");
            Assert.AreEqual(table.C, (double)5 / 2, "FFTQ023");
            Assert.AreEqual(table.D, (double)6074 / 2, "FFTQ024");
        }

        [Test]
        public void FoundedImplication01()
        {
			Assert.IsTrue(table.FoundedImplicationValidity(0.7D));
            Assert.AreEqual(0.967741935483871D, FourFoldContingencyTable.FoundedImplicationValue(table), "FFTQ025");
        }

		[Test]
		public void FoundedImplication02()
		{
			FourFoldContingencyTable table = new FourFoldContingencyTable(103, 0, 5, 6074);
            Assert.AreEqual(1.0D, FourFoldContingencyTable.FoundedImplicationValue(table), "FFTQ026");
		}
        
        [Test]
        public void AboveAverage01()
        {
            Assert.IsTrue(table.AboveAverageImplicationValidity(1.5D));
            Assert.AreEqual(1.80766889835666D, FourFoldContingencyTable.AboveAverageImplicationValue(table), "FFTQ027");
        }

        [Test]
        public void LowerCriticalImplication01()
        {
			FourFoldContingencyTable table = new FourFoldContingencyTable(10, 2, 0, 0);
            //FourFTQuantifiers.LowerCriticalImplication quantifier = new RelMiner.FourFTQuantifiers.LowerCriticalImplication(0.8f, 0.05f);
            Assert.IsFalse(table.CriticalImplicationValidity(0.8D, 0.05D, CoreRelationEnum.LessThanOrEqualCore), "FFTQ028");
            Assert.AreEqual(0.561894564884689D, table.CriticalImplicationValue(0.05D), "FFTQ029");
			//TODO I`m not sure
        }
    
        [Test]
        public void LowerCriticalImplication02()
        {
			FourFoldContingencyTable table = new FourFoldContingencyTable(100, 1, 0, 0);
            Assert.IsTrue(table.CriticalImplicationValidity(0.8D, 0.05D, CoreRelationEnum.LessThanOrEqualCore), "FFTQ030");
            Assert.AreEqual(0.953892650075111D, table.CriticalImplicationValue(0.05D), "FFTQ031");
			//TODO I`m not sure
        }

        [Test]
        public void ChiSquare01()
        {
			FourFoldContingencyTable table = new FourFoldContingencyTable(21, 1, 71, 141);
            Assert.IsTrue(table.ChiSquareValidity(0.05D), "FFTQ032");
			//Assert.AreEqual(1.48276669831304e-08D, table.ChiSquareValue());
            Assert.IsTrue(table.ChiSquareValue() < 1e-07D, "FFTQ033");  // correct value should be 1.48276595040141e-08
        }

        [Test]
        public void ChiSquare02()
        {
			FourFoldContingencyTable table = new FourFoldContingencyTable(12, 14, 11, 13);
            Assert.IsFalse(table.ChiSquareValidity(0.05D), "FFTQ034");
            Assert.IsTrue(closeEnough(6, table.ChiSquareValue(), 0.981874933633231), "FFTQ035");
        }

        [Test]
        public void ChiSquare03()
        {
			FourFoldContingencyTable table = new FourFoldContingencyTable(252, 143, 847, 602);
            Assert.IsFalse(table.ChiSquareValidity(0.05D), "FFTQ036");
            Assert.IsTrue(closeEnough(6, table.ChiSquareValue(), 0.0550538165123782D), "FFTQ037");
        }

		[Test]
		public void ChiSquare04()
		{
			FourFoldContingencyTable table = new FourFoldContingencyTable(103, 0, 5, 6074);
            Assert.AreEqual(0.0D, table.ChiSquareValue(), "FFTQ038");
			//TODO BUG (chyba hlasena Karbymu)
			//Assert.IsTrue(CloseEnough(6, table.ChiSquareValue(), 0.0550538165123782D));
		}

		[Test]
		public void Fischer01()
		{
			FourFoldContingencyTable table = new FourFoldContingencyTable(103, 0, 5, 6074);
            Assert.AreEqual(0.0f, (float)table.FisherValue(), "FFTQ039");
            Assert.AreEqual(8.46676800899516e-219D, table.FisherValue(), "FFTQ040");
		}

		[Test]
		public void E01()
		{
			FourFoldContingencyTable table = new FourFoldContingencyTable(103, 0, 5, 6074);
            Assert.AreEqual(0.999191200258816D, table.EValue(), "FFTQ041");
            Assert.IsTrue(table.EValidity(0.5D), "FFTQ042");
            Assert.IsFalse(table.EValidity(0.999999D), "FFTQ043");
		}
    }
}
