// DBInteraction.cs - class interacts with ODBC database
//
// Author: Alexander Kuzmin <alexander.kuzmin@gmail.com>
//
// Copyright (c) 2005 Alexander Kuzmin
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
using System.Collections;
using System.Text;
using Ferda.Modules.Boxes.DataMiningCommon.Column;
using Ferda.Modules;
using System.Data.Odbc;
using System.Data;
using System.Reflection;

namespace Ferda.FrontEnd.AddIns.Common.Database
{

    #region CategoryFrequency struct

    /// <summary>
    /// Struct containing count for a frequency
    /// </summary>
    public struct CategoryFrequency
    {
        public String key;
        public long count;
    };

    #endregion

    /// <summary>
    /// Class for interaction with ODBC connection
    /// </summary>
    public class DBInteraction
    {
        #region Private variables

        /// <summary>
        /// Connection string for a database
        /// </summary>
        private String connectionString = String.Empty;

        /// <summary>
        /// Expression for column selection
        /// </summary>
        private String columnSelectExpression;

        /// <summary>
        /// Datamatrix name fo query
        /// </summary>
        private String dataMatrixName;

        /// <summary>
        /// ODBC connection to use in queries
        /// </summary>
        private OdbcConnection connection;

        /// <summary>
        /// Row count in datamatrix
        /// </summary>
        private long rowCount;

        /// <summary>
        /// CategoriesStruct to count frequencies on
        /// </summary>
        private CategoriesStruct categoriesStruct;

        /// <summary>
        /// Column
        /// </summary>
        private ColumnInfo column;

        #endregion


        #region Properties

        public string ConnectionString
        {
            get
            {
                return connectionString;
            }
            set
            {
                connectionString = value;
            }
        }

        public string ColumnSelectExpression
        {
            get
            {
                return columnSelectExpression;
            }
            set
            {
                columnSelectExpression = value;
            }
        }

        public ColumnInfo Column
        {
            get
            {
                return column;
            }
            set
            {
                column = value;
            }
        }

        #endregion


        #region Constructor


        #endregion


        #region Working with ODBC

        /// <summary>
        /// Method for getting odbc connection
        /// </summary>
        /// <returns></returns>
        public OdbcConnection GetConnection()
        {
            try
            {
                return new System.Data.Odbc.OdbcConnection(this.connectionString);
            }

            catch (OdbcException e)
            {
                throw Ferda.Modules.Exceptions.BadParamsError(e, null, "Bad ODBC connection string specified. Could not connect to database.", Ferda.Modules.restrictionTypeEnum.DbConnectionString);
            }
        }

        /// <summary>
        /// Method which queries the ODBC connection for a given query.
        /// </summary>
        /// <param name="query"></param>
        /// <returns>DataTable with result</returns>
        private DataTable GetQueryResultTable(string query)
        {
            try
            {
                OdbcDataAdapter odbcDataAdapter = new OdbcDataAdapter(query, this.connection);

                DataSet dataSet = new DataSet();
                odbcDataAdapter.Fill(dataSet);

                return dataSet.Tables[0];
            }

            catch (OdbcException)
            {
                this.connection.Close();
                throw (new Ferda.Modules.BadParamsError());

            }
            catch
            {
                this.connection.Close();
                throw (new Ferda.Modules.BadParamsError());
            }
        }

        #endregion


        #region Composing WHERE clauses

        /// <summary>
        /// Method which composes WHERE clause for a categoryseq of the LongInterval type.
        /// </summary>
        /// <param name="longInterval">Category of long interval type</param>
        /// <param name="columnSelectExpression">Column select expression</param>
        /// <returns>WHERE clause</returns>
        private String GetWhereClauseLong(LongIntervalStruct[] longIntervalSeq, String columnSelectExpression)
        {
            StringBuilder returnString = new StringBuilder();

            bool first = true;

            foreach (LongIntervalStruct longInterval in longIntervalSeq)
            {
                if (first)
                {
                    first = false;
                    returnString.Append("(");
                }
                else
                {
                    returnString.Append(" OR(");
                }
                //if both bounds of current interval are infinity, no restriction is needed
                if ((longInterval.leftBoundType == BoundaryEnum.Infinity) && (longInterval.rightBoundType == BoundaryEnum.Infinity))
                    return "";



                returnString.Append(columnSelectExpression);
                bool left = false;
                if (longInterval.leftBoundType == BoundaryEnum.Round)
                {

                    returnString.Append(" > " + longInterval.leftBound);
                    left = true;
                }
                else
                {
                    if (longInterval.leftBoundType == BoundaryEnum.Sharp)
                    {

                        returnString.Append(" >= " + longInterval.leftBound);
                        left = true;
                    }
                    //Infinity is left, no restriction needed
                }

                if (longInterval.rightBoundType == BoundaryEnum.Round)
                {
                    if (left)
                    {

                        returnString.Append(" AND " + columnSelectExpression);
                    }

                    returnString.Append(" < " + longInterval.rightBound);
                }
                else
                {
                    if (longInterval.rightBoundType == BoundaryEnum.Sharp)
                    {
                        if (left)
                        {

                            returnString.Append(" AND " + columnSelectExpression);
                        }

                        returnString.Append(" <= " + longInterval.rightBound);
                    }
                    //Infinity is left, no restriction needed
                }
                returnString.Append(")");
            }
            return returnString.ToString();
        }

        /// <summary>
        /// Method which composes WHERE clause for a category of the FloatInterval type.
        /// </summary>
        /// <param name="floatInterval">Category of float interval type</param>
        /// <param name="columnSelectExpression">Column select expression</param>
        /// <returns>WHERE clause</returns>
        private String GetWhereClauseFloat(FloatIntervalStruct[] floatIntervalSeq, String columnSelectExpression)
        {
            StringBuilder returnString = new StringBuilder();
            bool first = true;
            //replacing comma with dot
            System.Text.RegularExpressions.Regex r = new System.Text.RegularExpressions.Regex("[,]");
            foreach (FloatIntervalStruct floatInterval in floatIntervalSeq)
            {
                if (first)
                {
                    first = false;
                    returnString.Append("(");
                }
                else
                {
                    returnString.Append(" OR(");
                }

                //if both bounds are infinity, no restriction is needed
                if ((floatInterval.leftBoundType == BoundaryEnum.Infinity) && (floatInterval.rightBoundType == BoundaryEnum.Infinity))
                    return "";

                returnString.Append(columnSelectExpression);
                bool left = false;

                if (floatInterval.leftBoundType == BoundaryEnum.Round)
                {
                    returnString.Append(" > " + r.Replace(floatInterval.leftBound.ToString(), "."));
                    left = true;
                }
                else
                {
                    if (floatInterval.leftBoundType == BoundaryEnum.Sharp)
                    {
                        returnString.Append(" >= " + r.Replace(floatInterval.leftBound.ToString(), "."));
                        left = true;
                    }
                    //Infinity is left, no restriction needed
                }
                if (floatInterval.rightBoundType == BoundaryEnum.Round)
                {
                    if (left)
                    {
                        returnString.Append(" AND " + columnSelectExpression);
                    }

                    returnString.Append(" < " + r.Replace(floatInterval.rightBound.ToString(), "."));
                }
                else
                {
                    if (floatInterval.rightBoundType == BoundaryEnum.Sharp)
                    {
                        if (left)
                        {
                            returnString.Append(" AND " + columnSelectExpression);
                        }
                        returnString.Append(" <= " + floatInterval.rightBound);
                    }
                    //Infinity is left, no restriction needed
                }
                returnString.Append(")");
            }
            return returnString.ToString();
        }


        /// <summary>
        /// Method which composes WHERE clause for a category of the DateTimeInterval type.
        /// </summary>
        /// <param name="dateTimeInterval">Category of dateTime interval type</param>
        /// <param name="columnSelectExpression">Column select expression</param>
        /// <returns>WHERE clause</returns>
        private String GetWhereClauseDateTime(DateTimeIntervalStruct[] dateTimeIntervalSeq, String columnSelectExpression)
        {
            StringBuilder returnString = new StringBuilder();

            bool first = true;

            foreach (DateTimeIntervalStruct dateTimeInterval in dateTimeIntervalSeq)
            {
                if (first)
                {
                    first = false;
                    returnString.Append("(");
                }
                else
                {
                    returnString.Append(" OR(");
                }

                //if both bounds are infinity, no restriction is needed
                if ((dateTimeInterval.leftBoundType == BoundaryEnum.Infinity) && (dateTimeInterval.rightBoundType == BoundaryEnum.Infinity))
                    return "";



                returnString.Append(columnSelectExpression);
                bool left = false;

                if (dateTimeInterval.leftBoundType == BoundaryEnum.Round)
                {

                    returnString.Append(" > " + dateTimeInterval.leftBound);
                    left = true;
                }
                else
                {
                    if (dateTimeInterval.leftBoundType == BoundaryEnum.Sharp)
                    {

                        returnString.Append(" >= " + dateTimeInterval.leftBound);
                        left = true;
                    }
                    //Infinity is left, no restriction needed
                }

                if (dateTimeInterval.rightBoundType == BoundaryEnum.Round)
                {
                    if (left)
                    {

                        returnString.Append(" AND " + columnSelectExpression);
                    }

                    returnString.Append(" < " + dateTimeInterval.rightBound);
                }
                else
                {
                    if (dateTimeInterval.rightBoundType == BoundaryEnum.Sharp)
                    {
                        if (left)
                        {

                            returnString.Append(" AND " + columnSelectExpression);
                        }

                        returnString.Append(" <= " + dateTimeInterval.rightBound);
                    }
                    //Infinity is left, no restriction needed
                }

                returnString.Append(")");
            }
            return returnString.ToString();
        }


        /// <summary>
        /// Method which composes WHERE clause for a category of the enum type.
        /// </summary>
        /// <param name="categoryEnum">Category of enum type</param>
        /// <param name="columnSelectExpression">Column select expression</param>
        /// <returns>WHERE clause</returns>
        private String GetWhereClauseForEnum(String[] categoryEnum, String columnSelectExpression)
        {
            StringBuilder returnString = new StringBuilder();

            bool first = true;
            foreach (String enumValue in categoryEnum)
            {
                if (enumValue != String.Empty)
                {
                    if (first)
                    {
                        first = false;
                    }
                    else
                    {
                        returnString.Append(" OR ");
                    }

                    returnString.Append("`" + columnSelectExpression + "` = ");
                    if ((this.column.columnSubType == ValueSubTypeEnum.StringType) || (this.column.columnSubType == ValueSubTypeEnum.Unknown))
                        returnString.Append("'");

                    if ((this.column.columnSubType == ValueSubTypeEnum.DoubleType) || (this.column.columnSubType == ValueSubTypeEnum.FloatType) || (this.column.columnSubType == ValueSubTypeEnum.DecimalType))
                    {
                        //replacing comma with dot
                        System.Text.RegularExpressions.Regex r = new System.Text.RegularExpressions.Regex("[,]");
                        returnString.Append(r.Replace(enumValue, "."));
                    }
                    else
                    {
                        returnString.Append(enumValue);
                    }

                    if ((this.column.columnSubType == ValueSubTypeEnum.StringType) || (this.column.columnSubType == ValueSubTypeEnum.Unknown))
                        returnString.Append("'");
                }
            }
            return returnString.ToString();
        }

        #endregion


        #region Other private methods

        /// <summary>
        /// Method which composes the ODBC SQL query to get the table
        /// with the count restrcited by WHERE clause
        /// </summary>
        /// <param name="where">Where clause</param>
        /// <returns>Query string</returns>
        private string GetValueCountQuery(string where)
        {
            string whereCond = String.Empty;
            if (!String.IsNullOrEmpty(where))
                whereCond = " WHERE " + where;

            return "SELECT `" + columnSelectExpression
                + "`, COUNT(1) AS `TmpCnt`"
                + " FROM " + "`" + dataMatrixName + "`"
                + whereCond
                + " GROUP BY " + columnSelectExpression
                + " ORDER BY " + columnSelectExpression;
        }

        #endregion


        #region Public methods

        /// <summary>
        /// Method for counting categories frequences
        /// </summary>
        /// <returns></returns>
        public ArrayList GetCategoriesFrequences(CategoriesStruct categories)
        {
            ArrayList returnList = new ArrayList();

            bool everything = false;

            foreach (DictionaryEntry singleEnum in categories.enums)
            {
                String[] stringSeq = (String[])singleEnum.Value;
                CategoryFrequency newEntry = new CategoryFrequency();
                if (!everything)
                {
                    String whereQuery = this.GetWhereClauseForEnum(stringSeq, this.columnSelectExpression);
                    DataTable tempTable = this.GetQueryResultTable(this.GetValueCountQuery(whereQuery));
                    newEntry.key = singleEnum.Key.ToString();
                    try
                    {
                        foreach (DataRow row in tempTable.Rows)
                        {
                            newEntry.count += Convert.ToInt64(row[1]);
                        }
                    }
                    catch
                    {
                        newEntry.count = 0;
                    }
                }
                else
                {
                    newEntry.key = singleEnum.Key.ToString();
                    newEntry.count = 0;
                }
                returnList.Add(newEntry);
            }

            foreach (DictionaryEntry singleLong in categories.longIntervals)
            {
                LongIntervalStruct[] longSeq = (LongIntervalStruct[])singleLong.Value;
                CategoryFrequency newEntry = new CategoryFrequency();
                if (!everything)
                {
                    String whereQuery = this.GetWhereClauseLong(longSeq, this.columnSelectExpression);
                    if (!whereQuery.Equals(""))
                    {
                        DataTable tempTable = this.GetQueryResultTable(this.GetValueCountQuery(whereQuery));
                        newEntry.key = singleLong.Key.ToString();
                        try
                        {
                            foreach (DataRow row in tempTable.Rows)
                            {
                                newEntry.count += Convert.ToInt64(row[1]);
                            }
                        }
                        catch
                        {
                            newEntry.count = 0;
                        }
                    }
                    else
                    {
                        newEntry.key = singleLong.Key.ToString();
                        newEntry.count = this.rowCount;
                        everything = true;
                    }
                }
                else
                {
                    newEntry.key = singleLong.Key.ToString();
                    newEntry.count = 0;
                }
                returnList.Add(newEntry);
            }

            foreach (DictionaryEntry singleFloat in categories.floatIntervals)
            {
                FloatIntervalStruct[] floatSeq = (FloatIntervalStruct[])singleFloat.Value;
                CategoryFrequency newEntry = new CategoryFrequency();
                if (!everything)
                {
                    String whereQuery = this.GetWhereClauseFloat(floatSeq, this.columnSelectExpression);
                    if (!whereQuery.Equals(""))
                    {
                        DataTable tempTable = this.GetQueryResultTable(this.GetValueCountQuery(whereQuery));
                        newEntry.key = singleFloat.Key.ToString();
                        try
                        {
                            foreach (DataRow row in tempTable.Rows)
                            {
                                newEntry.count += Convert.ToInt64(row[1]);
                            }
                        }
                        catch
                        {
                            newEntry.count = 0;
                        }
                    }
                    else
                    {
                        newEntry.key = singleFloat.Key.ToString();
                        newEntry.count = this.rowCount;
                        everything = true;
                    }
                }
                else
                {
                    newEntry.key = singleFloat.Key.ToString();
                    newEntry.count = 0;
                }
                returnList.Add(newEntry);
            }

            foreach (DictionaryEntry singleDateTime in categories.dateTimeIntervals)
            {
                DateTimeIntervalStruct[] dateTimeSeq = (DateTimeIntervalStruct[])singleDateTime.Value;
                CategoryFrequency newEntry = new CategoryFrequency();
                if (!everything)
                {
                    String whereQuery = this.GetWhereClauseDateTime(dateTimeSeq, this.columnSelectExpression);
                    if (!whereQuery.Equals(""))
                    {
                        DataTable tempTable = this.GetQueryResultTable(this.GetValueCountQuery(whereQuery));
                        newEntry.key = singleDateTime.Key.ToString();
                        try
                        {
                            foreach (DataRow row in tempTable.Rows)
                            {
                                newEntry.count += Convert.ToInt64(row[1]);
                            }
                        }
                        catch
                        {
                            newEntry.count = 0;
                        }
                    }
                    else
                    {
                        newEntry.key = singleDateTime.Key.ToString();
                        newEntry.count = this.rowCount;
                        everything = true;
                    }
                }
                else
                {
                    newEntry.key = singleDateTime.Key.ToString();
                    newEntry.count = 0;
                }
                returnList.Add(newEntry);
            }
            return returnList;
        }

        #endregion
    }
}