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
        private const string metabaseLayerDir = "BoxModulesServices/LISpMinerTasks/MetabaseLayer";
		private const string dbDir = "DB";

		private const string uncompressedFileName = "LISpMinerMetabaseEmpty.mdb";
		public static string UncompressedFileName
		{
			get { return uncompressedFileName; }
		}

		private const string compressedFileName = "LISpMinerMetabaseEmpty.mdb.gz";
		public static string CompressedFileName
		{
			get { return compressedFileName; }
		} 

		public static string BaseDir
		{
			get { return Path.Combine(metabaseLayerDir, dbDir); }
		}

		private string NewGUID
		{
			get { return System.Guid.NewGuid().ToString(); }
		}

		private string LISpMinerMetabaseFile;

		private OdbcConnection connection;
		public OdbcConnection Connection
		{
			get { return connection; }
		}

		public Metabase()
		{
			LISpMinerMetabaseFile = Path.Combine(BaseDir, NewGUID + UncompressedFileName);
			GZip.Decompress(
				Path.Combine(BaseDir, Metabase.CompressedFileName),
				LISpMinerMetabaseFile);

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
			string connectionString =
				"Driver={Microsoft Access Driver (*.mdb)};Dbq=" + LISpMinerMetabaseFile + ";Exclusive=1;Uid=admin;Pwd=";
			connection = new OdbcConnection(connectionString);
			connection.Open();
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
                System.IO.File.Delete(LISpMinerMetabaseFile);
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
