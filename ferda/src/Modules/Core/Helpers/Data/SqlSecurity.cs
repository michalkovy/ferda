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
        //TODO
        /*
         * SQL Injection - Continuing to Test
         * The hacker will continue to try other options for bypassing the site's validation and scrubbing routines, in each case carefully examining the server's response. These attempts include:
         *     * Using a single quote in different places in the string to try to take advantage of any ordered validation routines. For example, an e-mail field may be validated for having an @ symbol and a period. If the hacker entered joe'@mysite.com it would fail that validation, but joe@mysite.com' would succeed and expose the vulnerability.
         *     * Using a single quote at the end of a max length string. If the site is escaping single quotes, an attempt to escape the single quote could result in truncation back to the single quote.
         *     * Using two dashes. In SQL Server, this indicates a single-line comment and may cause the server to ignore the remainder of the line.
         *     * Using a semicolon. This indicates to SQL Server that a new command follows and can allow another query to piggyback the prior query.
         *     * Using high Unicode characters that are often downgraded to ASCII "equivalents," including the dangerous single quote.
         *     * Using all of these techniques not just in string fields, but in all fields in case there is any implicit translation being done, or the only format enforcement is through the UI.
         *     * Using a # character. Sometimes you'll find this used as a date/time delimiter.
         *     * Using char equivalents of the suspicious characters.
         * 
         */




		/// <summary>
		/// Returns boolean result that indicates wheter <c>inputSql</c>
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