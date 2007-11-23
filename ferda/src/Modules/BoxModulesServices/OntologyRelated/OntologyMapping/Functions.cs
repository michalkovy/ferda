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
        public const string PropMapping = "Mapping";
        public const string PropNumberOfMappedPairs = "NumberOfMappedPairs";
        public const string SockOntology = "Ontology";
        public const string SockDatabase = "Database";

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
                    string[] tmpStringArray = Mapping.Split(new char[] { '\n' });
                    return tmpStringArray.Length - 1;
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

        /*TODO smazat - nebude pot�eba, resp. bude muset b�t n�co obdobn�ho*/
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
            
            /*TODO  - p�edtim m�sto "DataPreparation.DataSource.Database" bylo Database.BoxInfo.typeIdentifier
             * asi to bude trochu podobn� jako z�sk�n� identity datab�ze u funkce GetSourceDataTableId
             * 
             * m�sto "ConnectionString" bylo Database.Functions.PropConnectionString
             * 
             * snadno by se to vy�e�ilo tim, �e se OntologyMapping p�ehod� do DataPreparation, 
             * co� by tak mo�n� i m�lo b�t!?
             * jinak lze p�idat referenci datapreparation (obj/debug), ale nevim jestli
             * je tohle v po��dku
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
            /*TODO smazat
             * primaryKeyColumns = new string[2];
            primaryKeyColumns[0] = "ok";
            primaryKeyColumns[1] = "notok";*/

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
        /// Gets the properties (dataproperties) of ontology entity - for individuals it is empty
        /// </summary>
        /// <param name="dataTableColumnName">Name of table column which properties (based on mapping) I want to get</param>
        /// <param name="dataTableColumnName">Name of the data table and its column</param>
        /// <param name="current__">Ice stuff</param>
        /// <returns>Data properties of ontology entity</returns>

        public override StrSeqMap getOntologyEntityProperties(string dataTableColumnName, Ice.Current current__)
        {
            return getOntologyEntityProperties(dataTableColumnName, true);
        }

        public override string[] getDataTablesNames(Current current__)
        {
            return GetDataTablesNames(true);
        }

        public override string[] getColumnsNames(string dataTableName, Current current__)
        {
            return GetColumnsNames(dataTableName, true);
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
