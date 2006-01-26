using System;
using System.Collections.Generic;
using System.Text;

namespace Ferda.Modules.Helpers.Data
{
	/// <summary>
	/// This class contains static methods for secured working with SQL queries.
	/// </summary>
	public static class SqlSecurity
	{
		/// <summary>
		/// Returns boolean value that indicates wheter <c>inputSql</c>
		/// contains any bad word i.e. "UPDATE", "GRANT", etc.
		/// </summary>
		/// <param name="inputSql">Input SQL string.</param>
		/// <returns>Returns true iff <c>inputSql</c> contains any bad word.</returns>
		public static bool ContainsBadWord(string inputSql)
		{
			if (String.IsNullOrEmpty(inputSql))
				return false;

			string[] badWords = new string[] { 
				"ALLOCATE",
				"ALTER",
				"AUTHORIZATION",
				"CATALOG",
				"CLOSE",
				"COLLATE", //?
				"COLLATION", //?
				"COMMIT",
				"CONNECT",
				"CONNECTION",
				"CONSTRAINT",
				"CREATE",
				"DEALLOCATE",
				"DELETE",
				"DESCRIBE",
				"DIAGNOSTICS",
				"DISCONNECT",
				"DOMAIN",
				"DROP",
				"EXEC",
				"EXECUTE",
				"EXTERNAL",
				"FOREIGN",
				"GET",
				"GLOBAL",
				"GO",
				"GOTO",
				"GRANT",
				"IDENTITY",
				"INSENSITIVE",
				"INSERT",
				"ISOLATION",
				"OPEN",
				"OVERLAPS",
				"PREPARE",
				"PRIOR",
				"PRIVILEGES",
				"PROCEDURE",
				"PUBLIC",
				"REFERENCES",
				"REVOKE",
				"ROLLBACK",
				"SCHEMA",
				"SESSION",
				"SESSION_USER",
				"SET",
				"SYSTEM_USER",
				"UPDATE",
				"USER",
				"WRITE"
				};
			foreach (string badWord in badWords)
			{
				if (inputSql.Contains(" " + badWord + " "))
					return true;
			}
			return false;
		}

		/// <summary>
		/// Trims the specified string and escapes "`" character.
		/// </summary>
		/// <param name="inputSql">Input SQL text for process.</param>
		/// <returns>Safe SQL literal witch duplexed "`".</returns>
		public static string SafeSqlObjectName(string inputSql)
		{

            if (String.IsNullOrEmpty(inputSql))
                return String.Empty;
            else
            {
                inputSql.Trim();
                return inputSql.Replace("`", "``");
            }
		}

		/// <summary>
		/// Escapes single quote character (').
		/// </summary>
		/// <param name="inputSql">Input SQL text for process.</param>
		/// <returns>Safe SQL literal witch duplexed single quotes.</returns>
		public static string SafeSqlLiteral(string inputSql)
		{
			if (String.IsNullOrEmpty(inputSql))
				return "";
			return inputSql.Replace("'", "''");
		}

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
	}
}