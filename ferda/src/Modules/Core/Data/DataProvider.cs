// DataProvider.cs - static classes for provider invariant names
//
// Author: Tomáš Kuchaø <tomas.kuchar@gmail.com>
//
// Copyright (c) 2006 Tomáš Kuchaø
//
// This program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Ferda.Modules;

namespace Ferda.Guha.Data
{
    /// <summary>
    /// Provides method to get generic factory class/classes through
    /// provider invariant names.
    /// </summary>
    /// <seealso href="http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dnvs05/html/vsgenerics.asp?frame=true&amp;hidetoc=true">
    /// Generic Coding with the ADO.NET 2.0 Base Classes and Factories (MSDN Library)
    /// </seealso>
    /// <seealso href="http://msdn2.microsoft.com/en-us/library/t9f29wbk.aspx">
    /// Writing Provider-Independent Code in ADO.NET (MSDN Library)
    /// </seealso>
    public static class DataProviderHelper
    {
        private static List<string> _factoryClassesInvariantNames = null;

        static DataProviderHelper()
        {
            if (System.Data.OleDb.OleDbFactory.Instance != null)
                DbProviderFactories.RegisterFactory(OleDbInvariantName, System.Data.OleDb.OleDbFactory.Instance);
            if (System.Data.Odbc.OdbcFactory.Instance != null)
                DbProviderFactories.RegisterFactory(OdbcInvariantName, System.Data.Odbc.OdbcFactory.Instance);
        }

        /// <summary>
        /// Gets the factory classes invariant names.
        /// </summary>
        /// <value>The factory classes invariant names.</value>
        public static List<string> FactoryClassesInvariantNames
        {
            get
            {
                if (_factoryClassesInvariantNames == null)
                {
                    DataTable dt = DbProviderFactories.GetFactoryClasses();
                    _factoryClassesInvariantNames = new List<string>(dt.Rows.Count);
                    foreach (DataRow var in dt.Rows)
                        _factoryClassesInvariantNames.Add(var["InvariantName"].ToString());
                }
                return _factoryClassesInvariantNames;
            }
        }

        /// <summary>
        /// Gets the quotes for identifeir for specified <c>providerInvariantName</c>.
        /// </summary>
        /// <param name="providerInvariantName">Name of the provider invariant.</param>
        /// <param name="prefix">The prefix quotation for identifier.</param>
        /// <param name="suffix">The suffix quotation for identifier.</param>
        public static void GetIdentifeirQuotes(string providerInvariantName, out string prefix, out string suffix)
        {
            switch (providerInvariantName)
            {
                case OdbcInvariantName:
                case OleDbInvariantName:
                case OracleClientInvariantName:
                case SqlClientInvariantName:
		case MonoSqliteInvariantName:
                    prefix = "`";
                    suffix = "`";
                    return;
                default:
                    //UNDONE
                    prefix = "`";
                    suffix = "`";
                    return;
            }
        }

        /// <summary>
        /// Provider invariant name for ODBC sources.
        /// </summary>
        public const string OdbcInvariantName = "System.Data.Odbc";

        /// <summary>
        /// Provider invariant name for OLE DB sources.
        /// </summary>
        public const string OleDbInvariantName = "System.Data.OleDb";

        /// <summary>
        /// Provider invariant name for Oracle.
        /// </summary>
        public const string OracleClientInvariantName = "System.Data.OracleClient";

        /// <summary>
        /// Provider invariant name for MS SQL Server.
        /// </summary>
        public const string SqlClientInvariantName = "System.Data.SqlClient";
        
	    /// <summary>
        /// Provider invariant name for SQLite in Mono.
        /// </summary>
        public const string MonoSqliteInvariantName = "Mono.Data.Sqlite";

        /// <summary>
        /// Gets the DB provider factory.
        /// </summary>
        /// <param name="providerInvariantName">Name of the provider invariant.</param>
        /// <returns></returns>
        /// <exception cref="T:Ferda.Modules.BadParamsError">
        /// 	<b>DbProviderInvariantNameError</b>
        /// Thrown iff specified <c>dataProvider</c> is unknown.
        /// </exception>
        public static DbProviderFactory GetDbProviderFactory(string providerInvariantName)
        {
            try
            {
                return DbProviderFactories.GetFactory(providerInvariantName);
            }
            catch (Exception e)
            {
                throw Modules.Exceptions.BadParamsError(e, null, null, restrictionTypeEnum.DbConnectionStringError);
            }
        }

        /*
        /// <summary>
        /// Make the following replacements:
        /// <list type="bullet">
        /// <listheader>
        /// </listheader>
        /// <item>
        /// <description>' becomes '',</description>
        /// </item>
        /// <item>
        /// <description>[ becomes [[],</description>
        /// </item>
        /// <item>
        /// <description>% becomes [%],</description>
        /// </item>
        /// <item>
        /// <description>_ becomes [_].</description>
        /// </item>
        /// </list>
        /// </summary>
        /// <param name="inputSql">Input SQL LIKE clause.</param>
        /// <returns>Safe SQL LIKE clause.</returns>
        public static string SafeSqlLikeClauseLiteral(string inputSql)
        {
            if (String.IsNullOrEmpty(inputSql))
                return "";

            string s = inputSql;
            s = inputSql.Replace("'", "''");
            s = s.Replace("[", "[[]");
            s = s.Replace("%", "[%]");
            s = s.Replace("_", "[_]");
            return s;
        }
        */
    }
}
