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

        public  DatabaseFunctionsPrx GetDatabaseFunctionsPrx(bool fallOnError)
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

        /*TODO smazat - nebude potøeba, resp. bude muset být nìco obdobnýho*/
        /*public string GetName(bool fallOnError)
        {
            if (String.IsNullOrEmpty(Name) && fallOnError)
            {
                throw Exceptions.BadValueError(null, _boxModule.StringIceIdentity,
                                               "Property is not set.", new string[] { PropName },
                                               restrictionTypeEnum.Missing);
            }
            else
                return Name;
        }*/

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

            DatabaseConnectionSettingHelper connSetting = new DatabaseConnectionSettingHelper(connSettingTmp);

            Dictionary<string, IComparable> cacheSetting = new Dictionary<string, IComparable>();
            
            /*TODO  - pøedtim místo "DataPreparation.DataSource.Database" bylo Database.BoxInfo.typeIdentifier
             * asi to bude trochu podobnì jako získání identity databáze u funkce GetSourceDataTableId
             * 
             * místo "ConnectionString" bylo Database.Functions.PropConnectionString
             * 
             * snadno by se to vyøešilo tim, že se OntologyMapping pøehodí do DataPreparation, 
             * což by tak možná i mìlo být!?
             * jinak lze pøidat referenci datapreparation (obj/debug), ale nevim jestli
             * je tohle v poøádku
             */
            
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

        public string[] GetDataTablesNames(bool fallOnError)
        {
            DatabaseFunctionsPrx prx = GetDatabaseFunctionsPrx(fallOnError);
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

        public DataTableInfo GetDataTableInfo(string dataTableName, string[] primaryKeyColumns, bool fallOnError)
        {
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

        public override string[] getDataTablesNames(Current current__)
        {
            return GetDataTablesNames(true);
        }

        public override string[] getColumnsNames(string dataTableName, Current current__)
        {
            return GetColumnsNames(dataTableName, true);
        }

        public override string getMapping(Current current__)
        {
            return Mapping == null ? "" : Mapping;
        }

        public override string getMappingSeparatorInner(Current current__)
        {
            return separatorInner;
        }

        public override string getMappingSeparatorOuter(Current current__)
        {
            return separatorOuter;
        }

        public override DataTableInfo getDataTableInfo(string dataTableName, string[] primaryKeyColumns, Current current__)
        {
            return GetDataTableInfo(dataTableName, primaryKeyColumns, true);
        }

        public override string GetSourceDataTableId(string dataTableName, Current current__)
        {
            DatabaseFunctionsPrx proxy = GetDatabaseFunctionsPrx(true);

            string result = proxy.getConnectionInfo().databaseName + '\n' + dataTableName;
            return result;
        }

        #endregion
    }
}
