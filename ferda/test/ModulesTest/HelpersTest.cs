using NUnit.Framework;
using System.Diagnostics;

namespace ModulesTest
{
	[TestFixture]
	public class HelpersTest
	{
		[Test]
		public void DatabaseConnectionGetConnection01()
		{
			System.Data.Odbc.OdbcConnection conn
				= Ferda.Modules.Helpers.Data.OdbcConnections.GetConnection(
				TestSetting.ValidOdbcConnectionString, null
				);
            Assert.IsNotNull(conn, "HT001: Helpers.Data.OdbcConnections.GetConnection(" + TestSetting.ValidOdbcConnectionString + ") FAILED!");
		}
		
		[Test]
		//Ferda.Modules.Boxes.DataMiningCommon.Database.BadConnectionStringError
		//[ExpectedException(typeof(System.Exception))]
		public void DatabaseConnectionGetConnection02()
		{
			try
			{
				System.Data.Odbc.OdbcConnection conn
                    = Ferda.Modules.Helpers.Data.OdbcConnections.GetConnection(
					TestSetting.InValidOdbcConnectionString, null
					);
			}
            catch (Ferda.Modules.BadParamsError ex)
			{
                if (ex.restrictionType == Ferda.Modules.restrictionTypeEnum.DbConnectionString)
                    return;
			}
			Assert.Fail("HT002");
		}
	}
}
