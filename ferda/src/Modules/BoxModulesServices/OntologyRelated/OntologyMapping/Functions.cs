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
using Ice;

namespace Ferda.Modules.Boxes.OntologyRelated.OntologyMapping
{
    /// <summary>
    /// Class is providing ICE functionality of the Ontology Mapping
    /// box module
    /// </summary>
    internal class Functions : OntologyMappingFunctionsDisp_, IFunctions
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
        public const string Mapping = "Mapping";
        public const string PropNumberOfMappedPairs = "NumberOfMappedPairs";
        public const string SockOntology = "Ontology";
        public const string SockDatabase = "Database";

        /*TODO smazat*/
        public const string PropName = "Name";
        public const string PropPrimaryKeyColumns = "PrimaryKeyColumns";

        public string Name
        {
            get { return _boxModule.GetPropertyString(PropName); }
        }

        public string[] PrimaryKeyColumns
        {
            get { return _boxModule.GetPropertyStringSeq(PropPrimaryKeyColumns); }
        }
        /*TODO smazat - konec*/

        public IntTI NumberOfMappedPairs
        {
            get
            {
                //OntologyStructure tmpOntology = getOntology(false);
                //return (tmpOntology != null) ? tmpOntology.OntologyClassMap.Values.Count.ToString() : "0";
                return 777;
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

        /*TODO smazat - nebude potøeba, resp. bude muset být nìco obdobnýho*/
        public string GetName(bool fallOnError)
        {
            /*if (String.IsNullOrEmpty(Name) && fallOnError)
            {
                throw Exceptions.BadValueError(null, _boxModule.StringIceIdentity,
                                               "Property is not set.", new string[] { PropName },
                                               restrictionTypeEnum.Missing);
            }
            else
                return Name;*/
            return "test name";
        }

        public StrSeqMap getOntologyEntityProperties(string dataTableColumnName, bool fallOnError)
        {
            return ExceptionsHandler.GetResult<StrSeqMap>(
                fallOnError,
                delegate
                    {
                        /*GenericDatabase tmp = GetGenericDatabase(fallOnError);
                        if (tmp != null)
                        {
                            List<DataTableExplain> result = new List<DataTableExplain>();
                            foreach (GenericDataTable table in tmp)
                            {
                                if (table.IsAcceptable(accepts))
                                    result.Add(table.Explain);
                            }
                            return result.ToArray();
                        }
                        return new DataTableExplain[0];*/
                        StrSeqMap dataPropertiesMap = new StrSeqMap();

                        /*TODO - upravit odpovidajici vyhledani informaci z ontologie na zaklade parametru dataTableColumnName a mapovani*/

                        string[] values = new string[3] { "15", "20", "30" };
                        dataPropertiesMap.Add("DomainDividingValues", values);
                        return dataPropertiesMap;
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
        
        public GenericDataTable GetGenericDataTable(bool fallOnError)
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
            cacheSetting.Add(BoxInfo.typeIdentifier + PropName, Name);

            if (_cacheFlag.IsObsolete(connSetting.LastReloadRequest, cacheSetting)
                || (_cachedValue == null && fallOnError))
            {
                _cachedValue = ExceptionsHandler.GetResult<GenericDataTable>(
                    fallOnError,
                    delegate
                    {
                        string name = GetName(fallOnError);
                        return GenericDatabaseCache.GetGenericDatabase(connSetting)[Name];
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

        public string[] GetColumnsNames(bool fallOnError)
        {
            return ExceptionsHandler.GetResult<string[]>(
                fallOnError,
                delegate
                {
                    GenericDataTable tmp = GetGenericDataTable(fallOnError);
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

        public DataTableExplain GetDataTableExplain(bool fallOnError)
        {
            return ExceptionsHandler.GetResult<DataTableExplain>(
                fallOnError,
                delegate
                {
                    GenericDataTable tmp = GetGenericDataTable(fallOnError);
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

        public DataTableInfo GetDataTableInfo(bool fallOnError)
        {
            return ExceptionsHandler.GetResult<DataTableInfo>(
                fallOnError,
                delegate
                {
                    DataTableExplain tmp1 = GetDataTableExplain(fallOnError);
                    DatabaseFunctionsPrx tmp2 = GetDatabaseFunctionsPrx(fallOnError);
                    string name = GetName(fallOnError);
                    if (tmp1 != null && tmp2 != null)
                        return new DataTableInfo(tmp2.getDatabaseConnectionSetting(),
                                                 name,
                                                 tmp1.type,
                                                 tmp1.remarks,
                                                 PrimaryKeyColumns,
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
        /// Gets the properties (dataproperties) of ontology entity - for individuals it is empty
        /// </summary>
        /// <param name="dataTableColumnName">Name of table column which properties (based on mapping) I want to get</param>
        /// <param name="current__">Ice stuff</param>
        /// <returns>Data properties of ontology entity</returns>

        public override StrSeqMap getOntologyEntityProperties(string dataTableColumnName, Ice.Current __current)
        {
            return getOntologyEntityProperties(dataTableColumnName, true);
        }

        public override string[] getDataTablesNames(Current current__)
        {
            return GetDataTablesNames(true);
        }

        public override string[] getColumnsNames(Current current__)
        {
            return GetColumnsNames(true);
        }

        public override DataTableInfo getDataTableInfo(Current current__)
        {
            return GetDataTableInfo(true);
        }

        public override string GetSourceDataTableId(string dataTableColumnName, Current current__)
        {
            /*TODO zmìnit podle toho, jak to upraví Martin v Datatable
             * pravdìpodobnì databaseBoxIceIdentity + name of table*/
            return _boxModule.StringIceIdentity;
        }

        #endregion
    }
}
