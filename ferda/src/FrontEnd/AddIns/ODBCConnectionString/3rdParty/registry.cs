// created on 1/13/2003 at 9:32 AM
using System;
using Microsoft.Win32;

namespace Guy.Utilities.Reg
{
	enum HKEY{ LocalMachine, CurrentUser}
	public class HKCU
	{
			
		public static object ReadOption(string subkey, string key, object defaultValue ) {
			RegistryKey Key = null;

			try
			{
				Key = Registry.CurrentUser.OpenSubKey( subkey );	
				return Key.GetValue( key, defaultValue );
			} catch ( Exception ) {
				return defaultValue;
			}
		}
		
		public static void WriteOption(string subkey, string key, object Value ) {
			RegistryKey Key = null;
			try
			{
				Key = Registry.CurrentUser.CreateSubKey( subkey ) ;
				Key.SetValue( key, Value); //Convert.ToString( Value ) );
			}			
			catch ( Exception )	{
			}
			finally	{
				if ( Key != null )
					Key.Close();
			}
		}
				
		public static string[] ValueNames(string subkey)
		{
			RegistryKey key = null;
			key = Registry.CurrentUser.OpenSubKey( subkey );	
			return key.GetValueNames();
		}
		
		public static int ValueCount(string subkey)
		{
			RegistryKey key = null;
			key = Registry.CurrentUser.OpenSubKey( subkey );	
			return key.ValueCount;	
		}

	}
	
	public class HKLM
	{
		public static object ReadOption(string subkey, string key, object defaultValue ) {
			RegistryKey Key = null;

			try
			{
				Key = Registry.LocalMachine.OpenSubKey( subkey );	
				return Key.GetValue( key, defaultValue );
			} catch ( Exception ) {
				return defaultValue;
			}
		}
		
		public static void WriteOption(string subkey, string key, object Value ) {
			RegistryKey Key = null;
			try
			{
				Key = Registry.LocalMachine.CreateSubKey( subkey ) ;
				Key.SetValue( key, Value); //Convert.ToString( Value ) );
			}			
			catch ( Exception )	{
			}
			finally	{
				if ( Key != null )
					Key.Close();
			}
		}		
				
		public static string[] ValueNames(string subkey)
		{
			RegistryKey key = null;
			key = Registry.LocalMachine.OpenSubKey( subkey );	
			return key.GetValueNames();
		}
		
		public static int ValueCount(string subkey)
		{
			RegistryKey key = null;
			key = Registry.LocalMachine.OpenSubKey( subkey );	
			return key.ValueCount;	
		}
	}
	
	class ODBCDSN
	{
		private const string ODBC_SOURCES = "Software\\ODBC\\ODBC.INI\\ODBC Data Sources";
		private const string ODBCREG = "Software\\ODBC\\ODBC.INI\\";
		
		public static string OdbcSourceSubkey
		{
			get { return ODBC_SOURCES;}
		}
		
		public static string GetConnectionStringFromDsn(string dsn, HKEY hkey)
		{
			string con;
			string source;
			string server = "localhost";
			string database = "Master";
			string subkey = ODBCREG + "\\" + dsn;
			
			con = 	"server={0};database={1};Trusted_Connection=true";
			if ( hkey == HKEY.CurrentUser)
				source = (string)HKLM.ReadOption(ODBC_SOURCES, dsn, dsn);
			else
				source = (string)HKCU.ReadOption(ODBC_SOURCES, dsn, dsn);
			
			if ( source == "SQL Server" )
			{
				if ( hkey == HKEY.CurrentUser)
				{
					server = (string)HKCU.ReadOption(subkey, "server", server);
					database = (string)HKCU.ReadOption(subkey, "database", database);
				}
				else
				{
					server = (string)HKLM.ReadOption(subkey, "server", server);
					database = (string)HKLM.ReadOption(subkey, "database", database);	
				}
				return string.Format(con, server, database);
			}
			return "dsn=" + dsn;
		}
				
		public static string Server(string dsn, HKEY hkey)
		{
			string source;
			string server = "";
			string subkey = ODBCREG + "\\" + dsn;
			
			if ( hkey == HKEY.CurrentUser)
				source = (string)HKCU.ReadOption(ODBC_SOURCES, dsn, dsn);
			else
				source = (string)HKLM.ReadOption(ODBC_SOURCES, dsn, dsn);
			
			if ( source != "Microsoft Access Driver (*.mdb)" )
			{
				if ( hkey == HKEY.CurrentUser)
					server = (string)HKCU.ReadOption(subkey, "server", server);
				else
					server = (string)HKLM.ReadOption(subkey, "server", server);
			}
			
			return server;
		}
		
		public static string [] DsnList(HKEY hkey)
		{
			string []odbcs;
			if ( hkey == HKEY.CurrentUser )
				odbcs = HKCU.ValueNames(ODBC_SOURCES);
			else
				odbcs = HKLM.ValueNames(ODBC_SOURCES);
			
			return odbcs;
		}
		
		public static string Database(string dsn, HKEY hkey)
		{
			string source;
			string database = "";
			string subkey = ODBCREG + "\\" + dsn;
			
			if ( hkey == HKEY.CurrentUser )
				source = (string)HKCU.ReadOption(ODBC_SOURCES, dsn, dsn);
			else
				source = (string)HKLM.ReadOption(ODBC_SOURCES, dsn, dsn);
			
			if ( source != "Microsoft Access Driver (*.mdb)" )
			{
				if ( hkey == HKEY.CurrentUser)
					database = (string)HKCU.ReadOption(subkey, "database", database);
				else
					database = (string)HKLM.ReadOption(subkey, "database", database);
			}
			else
			{
				if ( hkey == HKEY.CurrentUser)
					database = (string)HKCU.ReadOption(subkey, "DBQ", database);
				else
					database = (string)HKLM.ReadOption(subkey, "DBQ", database);
			}
			
			return database;
		}
		
		public static string provider(string dsn, HKEY hkey)
		{
			string source;
			if ( hkey == HKEY.CurrentUser)
				source = (string)HKCU.ReadOption(ODBC_SOURCES, dsn, dsn);
			else
				source = (string)HKLM.ReadOption(ODBC_SOURCES, dsn, dsn);
			return source;
		}
	}
}
