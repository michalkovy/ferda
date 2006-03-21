using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.IO.Compression;
using System.Data.Odbc;

namespace Ferda.Modules.MetabaseLayer
{
	public class Metabase : IDisposable
	{
        private static string metabaseDbDir =
            "BoxModulesServices"
            + System.IO.Path.DirectorySeparatorChar
            + "LISpMinerTasks"
            + System.IO.Path.DirectorySeparatorChar
            + "MetabaseLayer"
            + System.IO.Path.DirectorySeparatorChar
            + "DB";

		private const string uncompressedFileName = "LISpMinerMetabaseEmpty.mdb";

		private const string compressedFileName = "LISpMinerMetabaseEmpty.mdb.gz";

		private static string newGUID
		{
			get { return System.Guid.NewGuid().ToString(); }
		}

        private static string[] mdbDrivers = new string[] { "Microsoft Access Driver (*.mdb)", "Driver do Microsoft Access (*.mdb)" };

		private string lispMinerMetabaseFile;

		private OdbcConnection connection;
		public OdbcConnection Connection
		{
			get { return connection; }
		}

		public Metabase()
		{
            lispMinerMetabaseFile = Path.Combine(metabaseDbDir, newGUID + uncompressedFileName);
			GZip.Decompress(
                Path.Combine(metabaseDbDir, Metabase.compressedFileName),
				lispMinerMetabaseFile);

			/* www.connectionstrings.com
			 * Access
			 * * ODBC
			 * * * Standard Security:
			 * * * * "Driver={Microsoft Access Driver (*.mdb)};Dbq=C:\mydatabase.mdb;Uid=Admin;Pwd=;"
			 * * * Workgroup:
			 * * * * "Driver={Microsoft Access Driver (*.mdb)};Dbq=C:\mydatabase.mdb;SystemDB=C:\mydatabase.mdw;"
			 * * * Exclusive:
			 * * * * "Driver={Microsoft Access Driver (*.mdb)};Dbq=C:\mydatabase.mdb;Exclusive=1;Uid=admin;Pwd="
			 */
            Exception ex = null;
            foreach (string mdbDriver in mdbDrivers)
            {
                try
                {
                    string connectionString =
                        "Driver={" + mdbDriver + "};Dbq=" + lispMinerMetabaseFile + ";Uid=admin;Pwd=";
                    connection = new OdbcConnection(connectionString);
                    connection.Open();
                    ex = null;
                    break;
                }
                catch (System.Data.Odbc.OdbcException e) {
                    ex = e;
                }
            }
            if (ex != null)
                throw ex;
		}

		~Metabase()
		{
			FinalizeMe();
		}

		public void FinalizeMe()
		{
            try
            {
                connection.Close();
#if !DEBUG
                System.IO.File.Delete(lispMinerMetabaseFile);
#endif
            }
            catch (ArgumentNullException) { }
            catch (PathTooLongException) { }
            catch (DirectoryNotFoundException) { }
            catch (IOException) { }
            catch (NotSupportedException) { }
            catch (InvalidOperationException) { }
            catch (NullReferenceException) { }
		}

		#region IDisposable Members

		public void Dispose()
		{
			FinalizeMe();
			this.connection.Dispose();
		}

		#endregion
}
}
