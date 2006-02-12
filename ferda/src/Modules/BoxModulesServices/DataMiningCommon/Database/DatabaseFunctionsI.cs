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
        public string ConnectionString
        {
            get
            {
                return this.boxModule.GetPropertyString(DatabaseBoxInfo.OdbcConnectionStringPropertyName);
            }
        }

        public DateTimeTI LastReloadInfo
        {
            get
            {
                DateTimeTI result = (DateTimeTI)(this.boxModule.getProperty(DatabaseBoxInfo.LastReloadInfoPropertyName));
                if (result.year == 0 || result.month == 0 || result.day == 0)
                {
                    result = new DateTimeTI(DateTime.Now);
                    this.boxModule.setProperty(DatabaseBoxInfo.LastReloadInfoPropertyName, result);
                }
                return result;
                //return new DateTimeTI(this.boxModule.PropertiesDateTime[DatabaseBoxInfo.LastReloadInfoPropertyName]);
            }
            set
            {
                this.boxModule.setProperty(DatabaseBoxInfo.LastReloadInfoPropertyName, value);
            }
        }

        public string[] AcceptableTypesOfTables
        {
            get
            {
                //UNDONE
                return new string[0];
                //DatabaseBoxInfo.AcceptableTypesOfTablesPropertyName
            }
        }
        #endregion

        #region Functions
        public override DatabaseStruct getDatabase(Ice.Current __current)
        {
            return new DatabaseStruct(ConnectionString, LastReloadInfo);
        }

        private ExplainDatabaseStructureCache explainDatabaseStructureCache = new ExplainDatabaseStructureCache();
        public override DataMatrixInfo[] explainDatabaseStructure(Ice.Current __current)
        {
            return explainDatabaseStructureCache.Value(boxModule.StringIceIdentity, LastReloadInfo, ConnectionString, AcceptableTypesOfTables);
        }

        private DataMatrixNamesCache dataMatrixNamesCached = new DataMatrixNamesCache();
        public override string[] getTables(Ice.Current __current)
        {
            return dataMatrixNamesCached.Value(boxModule.StringIceIdentity, LastReloadInfo, ConnectionString, AcceptableTypesOfTables);
        }
        #endregion

        #region BoxInfo
        private ConnectionInfoCache connectionInfoCache = new ConnectionInfoCache();
        public ConnectionInfo GetConnectionInfo()
        {
            return connectionInfoCache.Value(boxModule.StringIceIdentity, LastReloadInfo, ConnectionString);
        }

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
