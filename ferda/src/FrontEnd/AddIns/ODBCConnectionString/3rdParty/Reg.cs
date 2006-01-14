/*
 * 
 * Created on 5/10/2004 at 2:37 PM
 *
 * REVISION HISTORY
 *
 * Author		Date		changes
 * GBaseke		5/10/2004 	Initial Revision
 */

using System;
using Guy.Utilities.Reg;

namespace SA
{
	/// <summary>
	/// Description of Reg.	
	/// </summary>
	public class Reg
	{
		private const string SAKEY = "software\\BASEKE\\Datasources\\";
		public Reg()
		{
		}
		
		public static string ReadDsn()
		{
			string subkey = SAKEY + "database";
			string dsn = (string)HKLM.ReadOption(subkey, "dsn", "");
			return dsn;
		}
		
		public static void WriteDsn(string dsn)
		{
			string subkey = SAKEY + "database";				
			HKLM.WriteOption(subkey, "dsn", dsn);
		}
	}
}
