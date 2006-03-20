/*
 * 
 * Created on 5/11/2004 at 10:21 AM
 *
 * REVISION HISTORY
 *
 * Author		Date		changes
 * GBaseke		5/11/2004 	Initial Revision
 */

using System;
using System.Runtime.InteropServices;
using System.ComponentModel.Design.Serialization;
using System.Collections;
using System.Windows;
using System.Data.OleDb;

namespace SA
{
	/// <summary>
	/// Description of ODBCCP32.	
	/// </summary>
	public class ODBCCP32
	{
		public ODBCCP32()
		{
		}
		
		#region Interop Methods

		/// <summary>
		/// Win32 API Imports
		/// </summary>
		[DllImport( "ODBCCP32.dll")]private static extern bool SQLManageDataSources(IntPtr hwnd);
		[DllImport( "ODBCCP32.dll")]private static extern bool SQLCreateDataSource(IntPtr hwnd, string lpszDS);
        [DllImport( "ODBCCP32.dll")]private static extern bool SQLConfigDataSource(
            IntPtr parent, int request, string driver, string attributes);

		#endregion


		#region Error Code
		public enum SQLError
		{
			ODBC_ERROR_GENERAL_ERR = 1,
			ODBC_ERROR_INVALID_BUFF_LEN,
			ODBC_ERROR_INVALID_HWND,
			ODBC_ERROR_INVALID_STR,
			ODBC_ERROR_INVALID_REQUEST_TYPE,
			ODBC_ERROR_COMPONENT_NOT_FOUND,
			ODBC_ERROR_INVALID_NAME,
			ODBC_ERROR_INVALID_KEYWORD_VALUE,
			ODBC_ERROR_INVALID_DSN,
			ODBC_ERROR_INVALID_INF,
			ODBC_ERROR_REQUEST_FAILED,
			ODBC_ERROR_INVALID_PATH,
			ODBC_ERROR_LOAD_LIB_FAILED,
			ODBC_ERROR_INVALID_PARAM_SEQUENCE,
			ODBC_ERROR_INVALID_LOG_FILE,
			ODBC_ERROR_USER_CANCELED,
			ODBC_ERROR_USAGE_UPDATE_FAILED,
			ODBC_ERROR_CREATE_DSN_FAILED,
			ODBC_ERROR_WRITING_SYSINFO_FAILED,
			ODBC_ERROR_REMOVE_DSN_FAILED,
			ODBC_ERROR_OUT_OF_MEM,      
			ODBC_ERROR_OUTPUT_STRING_TRUNCATED
		}
		#endregion

		
		#region Methods
		public bool ManageDatasources(IntPtr hwnd)
		{
			return SQLManageDataSources(hwnd);
		}
		
		public bool CreateDatasource(IntPtr hwnd, string szDsn)
		{
			return SQLCreateDataSource(hwnd, szDsn);
		}

		#endregion
	}
}
