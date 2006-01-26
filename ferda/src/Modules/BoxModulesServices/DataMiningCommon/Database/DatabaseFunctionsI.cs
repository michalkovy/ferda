using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;

namespace Ferda.Modules.Boxes.DataMiningCommon.Database
{
    class DatabaseFunctionsI : DatabaseFunctionsDisp_, IFunctions
    {
        protected BoxModuleI boxModule;
        //protected IBoxInfo boxInfo;

        #region IFunctions Members
        public void setBoxModuleInfo(BoxModuleI boxModule, IBoxInfo boxInfo)
        {
            this.boxModule = boxModule;
            //this.boxInfo = boxInfo;
        }
        #endregion

        #region Properties
        protected string connectionString
        {
            get
            {
                return this.boxModule.GetPropertyString("ConnectionString");
            }
        }

        protected DateTimeTI lastReloadInfo
        {
            get
            {
                DateTimeTI result = (DateTimeTI)(this.boxModule.getProperty("LastReloadInfo"));
                if (result.year == 0 || result.month == 0 || result.day == 0)
                {
                    result = new DateTimeTI(DateTime.Now);
                    this.boxModule.setProperty("LastReloadInfo", result);
                }
                return result;
                //return new DateTimeTI(this.boxModule.PropertiesDateTime["LastReloadInfo"]);
            }
            set
            {
                this.boxModule.setProperty("LastReloadInfo", value);
            }
        }
        #endregion

        #region Functions
        public override DatabaseStruct getDatabase(Ice.Current __current)
        {
            DatabaseStruct result = new DatabaseStruct();
            Ferda.Modules.Helpers.Data.Database.TestConnectionString(connectionString, boxModule.StringIceIdentity);
            result.connectionString = connectionString;
            result.lastReloadInfo = lastReloadInfo;
            return result;
        }

        public override DataMatrixInfo[] explainDatabaseStructure(Ice.Current __current)
        {
            return Ferda.Modules.Helpers.Data.Database.Explain(connectionString, null, boxModule.StringIceIdentity);
        }

        #region Cache: DataMatrixNames
        private class dataMatrixNamesCache : Ferda.Modules.Helpers.Caching.Cache
        {
            private string[] value = new string[0];
            public string[] Value(string boxIdentity, DateTimeT lastReloadTime, string connectionString)
            {
                lock (this)
                {
                    Dictionary<string, IComparable> cacheSetting = new Dictionary<string, IComparable>();
                    cacheSetting.Add("ConnectionString", connectionString);
                    if (IsObsolete(lastReloadTime, cacheSetting))
                        value = Ferda.Modules.Helpers.Data.Database.GetTables(connectionString, null, boxIdentity);
                    return value;
                }
            }
        }
        private dataMatrixNamesCache dataMatrixNamesCached = new dataMatrixNamesCache();
        #endregion
        public override string[] getTables(Ice.Current __current)
        {
            return dataMatrixNamesCached.Value(boxModule.StringIceIdentity, lastReloadInfo, connectionString);
        }
        #endregion

        #region Sockets
        #endregion

        #region Actions
        public bool TestConnectionStringAction()
        {
            try
            {
                Ferda.Modules.Helpers.Data.Database.TestConnectionString(connectionString, null);
                return true;
            }
            catch { }
            return false;
        }

        public void ReloadInfoAction()
        {
            lastReloadInfo = new DateTimeTI(DateTime.Now);
            return;
        }
        #endregion

        #region BoxInfo
        #region Cache: Connection`s properties
        public struct ConnectionProperties
        {
            public string DatabaseName;
            public string DataSource;
            public string Driver;
            public string ServerVersion;
        }

        private class connectionPropertiesCache : Ferda.Modules.Helpers.Caching.Cache
        {
            private ConnectionProperties value;
            public ConnectionProperties Value(string boxIdentity, DateTimeT lastReloadTime, string connectionString)
            {
                lock (this)
                {
                    Dictionary<string, IComparable> cacheSetting = new Dictionary<string, IComparable>();
                    cacheSetting.Add("ConnectionString", connectionString);
                    if (IsObsolete(lastReloadTime, cacheSetting))
                    {
                        try
                        {
                            System.Data.Odbc.OdbcConnection conn = Ferda.Modules.Helpers.Data.OdbcConnections.GetConnection(connectionString, null);
                            value.DatabaseName = conn.Database;
                            value.DataSource = conn.DataSource;
                            value.Driver = conn.Driver;
                            try
                            {
                                value.ServerVersion = conn.ServerVersion;
                            }
                            catch (InvalidOperationException)
                            {
                                value.ServerVersion = String.Empty;
                            }
                        }
                        catch (Ferda.Modules.BadParamsError ex)
                        {
                            if (ex.restrictionType != Ferda.Modules.restrictionTypeEnum.DbConnectionString)
                                throw ex;
                            value.DatabaseName = String.Empty;
                            value.DataSource = String.Empty;
                            value.Driver = String.Empty;
                            value.ServerVersion = String.Empty;
                        }
                    }
                    return value;
                }
            }
        }
        private connectionPropertiesCache connectionPropertiesCached = new connectionPropertiesCache();
        public ConnectionProperties getConnectionProperties()
        {
            return connectionPropertiesCached.Value(boxModule.StringIceIdentity, lastReloadInfo, connectionString);
        }
        #endregion

        public string[] GetTables()
        {
            try
            {
                return this.getTables();
            }
            catch (Ferda.Modules.BoxRuntimeError) { }
            return new string[0];
        }
        #endregion
    }
}
