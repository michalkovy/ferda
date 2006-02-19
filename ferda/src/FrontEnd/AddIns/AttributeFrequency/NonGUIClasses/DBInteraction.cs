using System;
using System.Collections;
using System.Text;
using Ferda.Modules.Boxes.DataMiningCommon.Column;
using Ferda.Modules;
using System.Data.Odbc;
using System.Data;
using System.Resources;
using System.Reflection;

namespace Ferda
{
    namespace FrontEnd.AddIns.AttributeFrequency.NonGUIClasses
    {
        #region CategoryFrequency struct
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
            private String connectionString;

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
            /// Resource manager for l10n
            /// </summary>
            private ResourceManager rm;

            /// <summary>
            /// CategoriesStruct to count frequencies on
            /// </summary>
            private CategoriesStruct categoriesStruct;

            /// <summary>
            /// Type of column to coutn frequency on
            /// </summary>
            private ValueSubTypeEnum columnType;
            #endregion


            #region Contructor
            public DBInteraction(ColumnInfo columnInfo, CategoriesStruct categoriesStruct, ResourceManager rm)
            {
                this.connectionString = columnInfo.dataMatrix.database.odbcConnectionString;

                this.columnSelectExpression = columnInfo.columnSelectExpression;

                this.dataMatrixName = columnInfo.dataMatrix.dataMatrixName;

                this.rowCount = columnInfo.dataMatrix.recordsCount;

                this.rm = rm;

                this.categoriesStruct = categoriesStruct;

                this.columnType = columnInfo.columnSubType;

                this.connection = this.GetConnection();

            }
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

            //TODO: Catch all the ODBC expections here

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



                    returnString.Append("`" + columnSelectExpression + "`");
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

                            returnString.Append(" AND `" + columnSelectExpression + "`");
                        }

                        returnString.Append(" < " + longInterval.rightBound);
                    }
                    else
                    {
                        if (longInterval.rightBoundType == BoundaryEnum.Sharp)
                        {
                            if (left)
                            {

                                returnString.Append(" AND `" + columnSelectExpression + "`");
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



                    returnString.Append("`" + columnSelectExpression + "`");
                    bool left = false;

                    if (floatInterval.leftBoundType == BoundaryEnum.Round)
                    {

                        returnString.Append(" > " + floatInterval.leftBound);
                        left = true;
                    }
                    else
                    {
                        if (floatInterval.leftBoundType == BoundaryEnum.Sharp)
                        {

                            returnString.Append(" >= " + floatInterval.leftBound);
                            left = true;
                        }
                        //Infinity is left, no restriction needed
                    }
                    if (floatInterval.rightBoundType == BoundaryEnum.Round)
                    {
                        if (left)
                        {

                            returnString.Append(" AND `" + columnSelectExpression + "`");
                        }

                        returnString.Append(" < " + floatInterval.rightBound);
                    }
                    else
                    {
                        if (floatInterval.rightBoundType == BoundaryEnum.Sharp)
                        {
                            if (left)
                            {

                                returnString.Append(" AND `" + columnSelectExpression + "`");
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



                    returnString.Append("`" + columnSelectExpression + "`");
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

                            returnString.Append(" AND `" + columnSelectExpression + "`");
                        }

                        returnString.Append(" < " + dateTimeInterval.rightBound);
                    }
                    else
                    {
                        if (dateTimeInterval.rightBoundType == BoundaryEnum.Sharp)
                        {
                            if (left)
                            {

                                returnString.Append(" AND `" + columnSelectExpression + "`");
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
                    if (first)
                    {
                        first = false;
                    }
                    else
                    {
                        returnString.Append(" OR ");
                    }

                    returnString.Append("`" + columnSelectExpression + "` = ");
                    if ((this.columnType == ValueSubTypeEnum.StringType) || (this.columnType == ValueSubTypeEnum.Unknown))
                        returnString.Append("'");
                    returnString.Append(enumValue);
                    if ((this.columnType == ValueSubTypeEnum.StringType) || (this.columnType == ValueSubTypeEnum.Unknown))
                        returnString.Append("'");
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
                if (!where.Equals(""))
                {
                    return "SELECT "
                    + "COUNT(" + "`" + this.columnSelectExpression + "`" + ") AS `TmpCnt`"
                    + " FROM " + this.dataMatrixName
                    + " WHERE " + where
                    + " GROUP BY " + "`" + this.columnSelectExpression + "`"
                    + " ORDER BY " + "`" + this.columnSelectExpression + "`";
                }
                else
                {
                    return "SELECT "
                   + "COUNT(" + "`" + this.columnSelectExpression + "`" + ") AS `TmpCnt`"
                   + " FROM " + this.dataMatrixName
                   + " GROUP BY " + "`" + this.columnSelectExpression + "`"
                   + " ORDER BY " + "`" + this.columnSelectExpression + "`";
                }
            }


            #endregion


            #region Public methods

            /// <summary>
            /// Method for counting categories' frequences
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
                                newEntry.count += Convert.ToInt64(row[0]);
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
                                    newEntry.count += Convert.ToInt64(row[0]);
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
                                    newEntry.count += Convert.ToInt64(row[0]);
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
                                    newEntry.count += Convert.ToInt64(row[0]);
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


            /// <summary>
            /// Method for changing the localization manager.
            /// </summary>
            /// <param name="rm"></param>
            public void ChangeRm(ResourceManager rm)
            {
                this.rm = rm;
            }

            #endregion
        }
    }
}