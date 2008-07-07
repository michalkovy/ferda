// Functions.cs - Function objects for the Ontology Mapping box
//
// Author: Martin Zeman <martin.zeman@email.cz>
//
// Copyright (c) 2007 Martin Zeman
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
using System.Diagnostics;
using System.Text;
using Ferda.Guha.Data;
using Ferda.Modules.Helpers.Caching;
using Ferda.Modules.Boxes.DataPreparation;
using Ferda.OntologyRelated.generated.OntologyData;
using Ferda.Modules.Boxes.OntologyRelated.Ontology;
using Ice;

namespace Ferda.Modules.Boxes.OntologyRelated.OntologyMapping
{
    /// <summary>
    /// Class is providing ICE functionality of the Ontology Mapping
    /// box module
    /// </summary>
    public class Functions : OntologyMappingFunctionsDisp_, IFunctions
    {
        /// <summary>
        /// The current box module
        /// </summary>
        protected BoxModuleI _boxModule;

        #region IFunctions Members

        /// <summary>
        /// Sets the <see cref="T:Ferda.Modules.BoxModuleI">box module</see>
        /// and the <see cref="T:Ferda.Modules.Boxes.IBoxInfo">box info</see>.
        /// </summary>
        /// <param name="boxModule">The box module.</param>
        /// <param name="boxInfo">The box info.</param>
        public void setBoxModuleInfo(BoxModuleI boxModule, IBoxInfo boxInfo)
        {
            _boxModule = boxModule;
        }

        #endregion

        #region Properties

        //names of the properties
        public const string PropMapping = "Mapping";
        public const string PropNumberOfMappedPairs = "NumberOfMappedPairs";
        public const string SockOntology = "Ontology";
        public const string SockDatabase = "Database";
        public const string PropPrimaryKeys = "PrimaryKeys";
        
        /// <summary>
        /// Name of the connection string of the database, which is connected to this box
        /// </summary>
        private string cachedConnectionString = "";
        
        /// <summary>
        /// Char which separates strings of mapped pairs (triples if datatable name is count separately)
        /// </summary>
        public const string separatorOuter = "\n";

        /// <summary>
        /// Char which separates datatable name, column name and ontology entity name
        /// </summary>
        public const string separatorInner = "\t";

        /// <summary>
        /// Mapping between datatables columns and ontology entities
        /// </summary>
        public string Mapping
        {
            get { return _boxModule.GetPropertyString(PropMapping); }
        }

        /// <summary>
        /// PrimaryKeys struct (one member of array: dataTableName[separatorOuter]PKcolumn1[separatorInner]PKcolumn2...)
        /// </summary>
        public string[] PrimaryKeys
        {
            get { return _boxModule.GetPropertyStringSeq(PropPrimaryKeys); }
        }

        /// <summary>
        /// Number of Mapped Pairs (table column - ontology entity) in the mapping
        /// </summary>
        public IntTI NumberOfMappedPairs
        {
            get
            {
                if (Mapping != null)
                {
                    string[] tmpStringArray = Mapping.Split(new string[] { separatorOuter }, StringSplitOptions.RemoveEmptyEntries);
                    return tmpStringArray.Length;
                }
                return 0;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the proxy of database box (which is connected
        /// to this Ontology Mapping box)
        /// </summary>
        public DatabaseFunctionsPrx GetDatabaseFunctionsPrx(bool fallOnError)
        {
            return SocketConnections.GetPrx<DatabaseFunctionsPrx>(
                _boxModule,
                SockDatabase,
                DatabaseFunctionsPrxHelper.checkedCast,
                fallOnError);
        }

        /// <summary>
        /// Gets the proxy of Ontology box (which is connected
        /// to this Ontology Mapping box)
        /// </summary>
        /// <param name="fallOnError">Iff the method should fall on error</param>
        /// <returns>Ontology proxy</returns>
        public OntologyFunctionsPrx GetOntologyFunctionsPrx(bool fallOnError)
        {
            return SocketConnections.GetPrx<OntologyFunctionsPrx>(
                _boxModule,
                SockOntology,
                OntologyFunctionsPrxHelper.checkedCast,
                fallOnError);
        }

        /// <summary>
        /// Gets the superClass of the ontology entity from ontology, for individuals it returns the class from which the instance is instantiated
        /// </summary>
        /// <param name="ontologyEntityName">Name of the ontology entity</param>
        /// <param name="fallOnError">Iff the method should fall on error</param>
        /// <returns>SuperClasses of the ontology entity</returns>
        //TODO - mozna smazat - nepouziva se
        public string[] getOntologyEntitySuperClasses(string ontologyEntityName, bool fallOnError)
        {
            OntologyFunctionsPrx prx = GetOntologyFunctionsPrx(fallOnError);
            if (prx == null)
                return null;

            return ExceptionsHandler.GetResult<string[]>(
                fallOnError,
                delegate
                {
                    return prx.getOntologyEntitySuperClasses(ontologyEntityName);
                },
                delegate
                {
                    return new string[0];
                },
                _boxModule.StringIceIdentity
                );            
        }

        /// <summary>
        /// Gets the properties (Data Properties) of ontology entity - for individuals it is empty
        /// </summary>
        /// <param name="dataTableName">Name of the column's table</param>
        /// <param name="columnName">Name of the column which properties (based on mapping) I want to get</param>
        /// <param name="fallOnError">Iff the method should fall on error</param>
        /// <returns>Data properties of ontology entity</returns>
        public StrSeqMap getOntologyEntityProperties(string dataTableName, string columnName, bool fallOnError)
        {
            return ExceptionsHandler.GetResult<StrSeqMap>(
                fallOnError,
                delegate
                    {
                        /// parse current Mapping
                        string[] tmpMappedPairs = Mapping.Split(new string[] { separatorOuter }, StringSplitOptions.RemoveEmptyEntries);
                        string ontologyEntity = "";
                        /// seeking the row of Mapping which represents mapping of the particular column
                        foreach (string tmpMappedPair in tmpMappedPairs)
                        {
                            string[] DataTable_Column_OntEnt = tmpMappedPair.Split(new string[] { separatorInner }, StringSplitOptions.RemoveEmptyEntries);
                            if (DataTable_Column_OntEnt[0] == dataTableName && DataTable_Column_OntEnt[1] == columnName)
                                ontologyEntity = DataTable_Column_OntEnt[2];
                        }
                        /// the column of the datatable is mapped on some entity from the ontology
                        /// if it is empty, then the column is not mapped
                        if (ontologyEntity != "")
                        {
                            OntologyFunctionsPrx prx = GetOntologyFunctionsPrx(true);
                            if (prx != null)
                            {
                                /// getting the data properties from the ontology box
                                return prx.getOntologyEntityProperties(ontologyEntity);
                            }
                        }
                        return null;
                    },
                delegate
                    {
                        return null;
                    },
                _boxModule.StringIceIdentity
                );
        }

        /// <summary>
        /// Gets the annotations (annotations + comments) of ontology entity
        /// </summary>
        /// <param name="dataTableName">Name of the column's table</param>
        /// <param name="columnName">Name of the column which properties (based on mapping) I want to get</param>
        /// <param name="fallOnError">Iff the method should fall on error</param>
        /// <returns>Annotations of ontology entity</returns>
        public string[] getOntologyEntityAnnotations(string dataTableName, string columnName, bool fallOnError)
        {
            return ExceptionsHandler.GetResult<string[]>(
                fallOnError,
                delegate
                {
                    /// parse current Mapping
                    string[] tmpMappedPairs = Mapping.Split(new string[] { separatorOuter }, StringSplitOptions.RemoveEmptyEntries);
                    string ontologyEntity = "";
                    /// seeking the row of Mapping which represents mapping of the particular column
                    foreach (string tmpMappedPair in tmpMappedPairs)
                    {
                        string[] DataTable_Column_OntEnt = tmpMappedPair.Split(new string[] { separatorInner }, StringSplitOptions.RemoveEmptyEntries);
                        if (DataTable_Column_OntEnt[0] == dataTableName && DataTable_Column_OntEnt[1] == columnName)
                            ontologyEntity = DataTable_Column_OntEnt[2];
                    }
                    /// the column of the datatable is mapped on some entity from the ontology
                    /// if it is empty, then the column is not mapped
                    if (ontologyEntity != "")
                    {
                        OntologyFunctionsPrx prx = GetOntologyFunctionsPrx(true);
                        if (prx != null)
                        {
                            /// getting the data properties from the ontology box
                            return prx.getOntologyEntityAnnotations(ontologyEntity);
                        }
                    }
                    return null;
                },
                delegate
                {
                    return null;
                },
                _boxModule.StringIceIdentity
                );
        }

        private CacheFlag _cacheFlag = new CacheFlag();
        private GenericDataTable _cachedValue = null;

        /// <summary>
        /// <para>
        /// Gets the contents of the data table.
        /// </para>
        /// <para>
        /// NOTE: The function is similar to the 
        /// <see cref="Ferda.Modules.Boxes.DataPreparation.Datasource.DataTable.Functions.GetGenericDataTable"/>
        /// function. Apply possible changes also to this function. 
        /// </para>
        /// </summary>
        /// <param name="dataTableName">Name of the data table in the database</param>
        /// <param name="fallOnError">Iff the method should fall on error</param>
        /// <returns>Contents of the data table</returns>
        public GenericDataTable GetGenericDataTable(string dataTableName, bool fallOnError)
        {
            DatabaseFunctionsPrx prx = GetDatabaseFunctionsPrx(fallOnError);
            if (prx == null)
                return null;

            DatabaseConnectionSetting connSettingTmp =
                ExceptionsHandler.GetResult<DatabaseConnectionSetting>(
                    fallOnError,
                    prx.getDatabaseConnectionSetting,
                    delegate
                    {
                        return null;
                    },
                    _boxModule.StringIceIdentity
                    );

            if (connSettingTmp == null)
                return null;

            DatabaseConnectionSettingHelper connSetting = 
                new DatabaseConnectionSettingHelper(connSettingTmp);

            Dictionary<string, IComparable> cacheSetting = new Dictionary<string, IComparable>();
                        
            cacheSetting.Add("DataPreparation.DataSource.Database" + "ConnectionString", connSetting);
            cacheSetting.Add(BoxInfo.typeIdentifier + "dataTableName", dataTableName);

            if (_cacheFlag.IsObsolete(connSetting.LastReloadRequest, cacheSetting)
                || (_cachedValue == null && fallOnError))
            {
                _cachedValue = ExceptionsHandler.GetResult<GenericDataTable>(
                    fallOnError,
                    delegate
                    {
                        string name = dataTableName;
                        return GenericDatabaseCache.GetGenericDatabase(connSetting)[dataTableName];
                    },
                    delegate
                    {
                        return null;
                    },
                    _boxModule.StringIceIdentity
                    );
            }
            return _cachedValue;
        }

        /// <summary>
        /// Gets the names of data tables of the connected database
        /// </summary>
        /// <param name="fallOnError">Iff the method should fall on error</param>
        /// <returns>Array of the names of data tables</returns>
        public string[] GetDataTablesNames(bool fallOnError)
        {
            DatabaseFunctionsPrx prx = GetDatabaseFunctionsPrx(fallOnError);

            //testing, whether the database was not changed, otherwise, the mapping must be changed as well
            if (prx != null)
            {
                DatabaseConnectionSetting connSettingTmp = prx.getDatabaseConnectionSetting();
                if (connSettingTmp != null)
                {
                    //connected database is changed, it is needed, to erase the mapping
                    if (cachedConnectionString != connSettingTmp.connectionString)
                    {
                        //for loaded projects
                        if (cachedConnectionString != "")
                        {
                            string emptyString = "";
                            _boxModule.setProperty(PropMapping, (StringTI)emptyString);
                        }
                        cachedConnectionString = connSettingTmp.connectionString;
                    }
                }
            }

            return ExceptionsHandler.GetResult<string[]>(
                fallOnError,
                delegate
                {
                    if (prx != null)
                        return prx.getDataTablesNames();
                    return new string[0];
                },
                delegate
                {
                    return new string[0];
                },
                _boxModule.StringIceIdentity
                );
        }

        /// <summary>
        /// <para>
        /// Gets the names of columns of specified data table
        /// </para>
        /// <para>
        /// NOTE: The function is similar to the 
        /// <see cref="Ferda.Modules.Boxes.DataPreparation.Datasource.DataTable.Functions.GetColumnsNames"/>
        /// function. Apply possible changes also to this function. 
        /// </para>
        /// </summary>
        /// <param name="dataTableName">Name of the data table table</param>
        /// <param name="fallOnError">Iff the method should fall on error</param>
        /// <returns>Array of the names of data tables</returns>
        public string[] GetColumnsNames(string dataTableName, bool fallOnError)
        {
            return ExceptionsHandler.GetResult<string[]>(
                fallOnError,
                delegate
                {
                    GenericDataTable tmp = GetGenericDataTable(dataTableName, fallOnError);
                    if (tmp != null)
                        return tmp.BasicColumnsNames;
                    return new string[0];
                },
                delegate
                {
                    return new string[0];
                },
                _boxModule.StringIceIdentity
                );
        }

        /// <summary>
        /// Prepares a string array for AddIn SelectPrimaryKeys
        /// the structure of the array is dataTableName'\n'column1'\t'column2
        /// where \n is outer separator and \t inner separator
        /// </summary>
        /// <param name="fallOnError">Iff the method should fall on error</param>
        /// <returns>String array with dataTable names and theirs columns. 
        /// The structure of the array is dataTableName'\n'column1'\t'column2
        /// where \n is outer separator and \t inner separator</returns>
        public string[] GetSelectPrimaryKeysStruct(bool fallOnError)
        {
            string[] dataTablesNames = GetDataTablesNames(fallOnError);

            string[] _SelectPrimaryKeysStruct = new string[dataTablesNames.Length];

            int i = 0;
            foreach (string dataTableName in dataTablesNames)
            {
                string tableColumnsString = dataTableName + separatorOuter;
                string[] columnsNames = GetColumnsNames(dataTableName, fallOnError);
                foreach (string columnName in columnsNames)
                {
                    tableColumnsString += columnName + separatorInner;
                }
                /// removing last unwanted separator
                tableColumnsString = tableColumnsString.Remove(tableColumnsString.Length-separatorOuter.Length);

                _SelectPrimaryKeysStruct[i] = tableColumnsString;
                i++;
            }

            return _SelectPrimaryKeysStruct;
        }

        /// <summary>
        /// <para>
        /// Gets the info about the data table
        /// </para>
        /// <para>
        /// NOTE: The function is similar to the 
        /// <see cref="Ferda.Modules.Boxes.DataPreparation.Datasource.DataTable.Functions.GetDataTableExplain"/>
        /// function. Apply possible changes also to this function. 
        /// </para>
        /// </summary>
        /// <param name="dataTableName">Name of the column's table</param>
        /// <param name="fallOnError">Iff the method should fall on error</param>
        /// <returns>Info about the data table</returns>
        public DataTableExplain GetDataTableExplain(string dataTableName, bool fallOnError)
        {
            return ExceptionsHandler.GetResult<DataTableExplain>(
                fallOnError,
                delegate
                {
                    GenericDataTable tmp = GetGenericDataTable(dataTableName, fallOnError);
                    if (tmp != null)
                        return tmp.Explain;
                    return null;
                },
                delegate
                {
                    return null;
                },
                _boxModule.StringIceIdentity
                );
        }

        /// <summary>
        /// <para>
        /// Tests the unique columns i.e. unicity of values in data in specified
        /// <c>uniqueColumns</c>. (These columns can simulate Primary Key.)
        /// </para>
        /// <para>
        /// NOTE: The function is similar to the 
        /// <see cref="Ferda.Modules.Boxes.DataPreparation.Datasource.DataTable.Functions.TryPrimaryKey"/>
        /// function. Apply possible changes also to this function. 
        /// </para>
        /// </summary>
        /// <param name="dataTableName">name of datatable where the unicity is going to test.</param>
        /// <param name="fallOnError">if set to <c>true</c> the method will fall on error.</param>
        /// <exception cref="T:Ferda.Modules.BadValueError">
        /// <b>DbUniqueKeyError</b>
        /// Thrown when the specified columns does not satisfy the
        /// unicity of values in the database.
        /// </exception>
        public void TryPrimaryKey(string dataTableName, bool fallOnError)
        {
            Debug.Assert(fallOnError);
            ExceptionsHandler.GetResult<object>(
                fallOnError,
                delegate
                {
                    GenericDataTable tmp = GetGenericDataTable(dataTableName, fallOnError);
                    if (tmp != null)
                        tmp.TestUniqueKey(getPrimaryKeyColumns(dataTableName));
                    return null;
                },
                delegate
                {
                    return null;
                },
                _boxModule.StringIceIdentity
                );
        }

        /// <summary>
        /// Gets the array of primary key columns of the data table
        /// </summary>
        /// <param name="dataTableName">Name of the data table</param>
        /// <returns>Primary key columns</returns>
        private string[] getPrimaryKeyColumns(string dataTableName)
        {
            string[] dataTablesNamesAndSelectedColumns = PrimaryKeys;
            foreach (string tmpString in dataTablesNamesAndSelectedColumns)
            {
                //splits on two strings - first is name of datatable, second contains the primary key columns
                string[] splitString = tmpString.Split(new string[] { separatorOuter }, StringSplitOptions.RemoveEmptyEntries);

                //seeked datatable is found and it has primary key set
                if (splitString[0] == dataTableName)
                {   
                    string[] primaryKeyColumns = splitString[1].Split(new string[] { separatorInner }, StringSplitOptions.RemoveEmptyEntries);
                    return primaryKeyColumns;
                }
            }
            return new string[0];
        }

        /// <summary>
        /// <para>
        /// Gets the info about the data table
        /// </para>
        /// <para>
        /// NOTE: The function is similar to the 
        /// <see cref="Ferda.Modules.Boxes.DataPreparation.Datasource.DataTable.Functions.GetDataTableInfo"/>
        /// function. Apply possible changes also to this function. 
        /// </para>
        /// </summary>
        /// <param name="dataTableName">Name of thedata table</param>
        /// <param name="fallOnError">Iff the method should fall on error</param>
        /// <returns>Info about the data table</returns>
        public DataTableInfo GetDataTableInfo(string dataTableName, bool fallOnError)
        {
            string[] primaryKeyColumns = getPrimaryKeyColumns(dataTableName);

            return ExceptionsHandler.GetResult<DataTableInfo>(
                fallOnError,
                delegate
                {
                    DataTableExplain tmp1 = GetDataTableExplain(dataTableName, fallOnError);
                    DatabaseFunctionsPrx tmp2 = GetDatabaseFunctionsPrx(fallOnError);
                    string name = dataTableName;
                    if (tmp1 != null && tmp2 != null)
                        return new DataTableInfo(tmp2.getDatabaseConnectionSetting(),
                                                 dataTableName,
                                                 tmp1.type,
                                                 tmp1.remarks,
                                                 primaryKeyColumns,
                                                 tmp1.recordsCount);
                    return null;
                },
                delegate
                {
                    return null;
                },
                _boxModule.StringIceIdentity
                );
        }

        /// <summary>
        /// The method checks, if the primary keys are set for all the data tables of the database.
        /// </summary>
        public void TryPrimaryKeys()
        {
            try
            {
                foreach (string datatableName in GetDataTablesNames(true))
                {
                    TryPrimaryKey(datatableName, true);
                }
            }
            catch
            {
                BoxRuntimeError result = new BoxRuntimeError(null, "You need to set primary keys of all tables in the database.");
                throw result;
            }
        }

        #endregion

        #region Ice Functions

        /// <summary>
        /// Gets the properties (Data Properties) of ontology entity - for individuals it is empty
        /// </summary>
        /// <param name="dataTableName">Name of the column's table</param>
        /// <param name="columnName">Name of the column which properties (based on mapping) I want to get</param>
        /// <param name="current__">Ice stuff</param>
        /// <returns>Data properties of ontology entity</returns>
        public override StrSeqMap getOntologyEntityProperties(string dataTableName, string columnName, Ice.Current current__)
        {
            return getOntologyEntityProperties(dataTableName, columnName, true);
        }

        /// <summary>
        /// Gets the annotations (annotations + comments) of ontology entity
        /// </summary>
        /// <param name="dataTableName">Name of the column's table</param>
        /// <param name="columnName">Name of the column which properties (based on mapping) I want to get</param>
        /// <param name="current__">Ice stuff</param>
        /// <returns>Annotations of ontology entity</returns>
        public override string[] getOntologyEntityAnnotations(string dataTableName, string columnName, Ice.Current current__)
        {
            return getOntologyEntityAnnotations(dataTableName, columnName, true);
        }

        /// <summary>
        /// Gets the superClass of the ontology entity from ontology, for individuals it returns the class from which the instance is instantiated
        /// </summary>
        /// <param name="ontologyEntityName">Name of the ontology entity</param>
        /// <param name="current__">Ice stuff</param>
        /// <returns>SuperClasses of the ontology entity</returns>
        public override string[] getOntologyEntitySuperClasses(string ontologyEntityName, Ice.Current current__)
        {
            return getOntologyEntitySuperClasses(ontologyEntityName, true);
        }

        /// <summary>
        /// Gets the names of data tables of the connected database
        /// </summary>
        /// <param name="current__">Ice stuff</param>
        /// <returns>Array of the names of the data tables</returns>
        public override string[] getDataTablesNames(Current current__)
        {
            return GetDataTablesNames(true);
        }

        /// <summary>
        /// Gets the names of columns of the specified data table
        /// </summary>
        /// <param name="dataTableName">Name of data table</param>
        /// <param name="current__">Ice stuff</param>
        /// <returns>Array of the names of columns of the data table</returns>
        public override string[] getColumnsNames(string dataTableName, Current current__)
        {
            return GetColumnsNames(dataTableName, true);
        }

        /// <summary>
        /// Gets the mapping of the data columns on the ontology entities
        /// </summary>
        /// <param name="current__">Ice stuff</param>
        /// <returns>Gets the mapping</returns>
        public override string getMapping(Current current__)
        {
            return Mapping == null ? "" : Mapping;
        }

        /// <summary>
        /// Gets the inner mapping separator
        /// </summary>
        /// <param name="current__">Ice stuff</param>
        /// <returns>Inner mapping separator</returns>
        public override string getMappingSeparatorInner(Current current__)
        {
            return separatorInner;
        }

        /// <summary>
        /// Gets the outer mapping separator
        /// </summary>
        /// <param name="current__">Ice stuff</param>
        /// <returns>Outer mapping separator</returns>
        public override string getMappingSeparatorOuter(Current current__)
        {
            return separatorOuter;
        }

        /// <summary>
        /// Gets the info about specified data table
        /// </summary>
        /// <param name="dataTableName">Name of data table</param>
        /// <param name="current__">Ice stuff</param>
        /// <returns>Info about the data table</returns>
        public override DataTableInfo getDataTableInfo(string dataTableName, Current current__)
        {
            return GetDataTableInfo(dataTableName, true);
        }

        /// <summary>
        /// Gets the identificator of the data table
        /// </summary>
        /// <param name="dataTableName">Name of data table</param>
        /// <param name="current__">Ice stuff</param>
        /// <returns>Data table ID</returns>
        public override string GetSourceDataTableId(string dataTableName, Current current__)
        {
            DatabaseFunctionsPrx proxy = GetDatabaseFunctionsPrx(true);

            string result = proxy.getConnectionInfo().databaseName + '\n' + dataTableName;
            return result;
        }

        #endregion
    }
}
