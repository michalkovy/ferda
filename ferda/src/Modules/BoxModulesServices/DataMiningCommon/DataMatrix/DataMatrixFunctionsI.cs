using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data;
using Ferda.Modules.Boxes.DataMiningCommon.Database;

namespace Ferda.Modules.Boxes.DataMiningCommon.DataMatrix
{
    class DataMatrixFunctionsI : DataMatrixFunctionsDisp_, IFunctions
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
        public string DataMatrixName
        {
            get
            {
                return this.boxModule.GetPropertyString(DataMatrixBoxInfo.DataMatrixNamePropertyName);
            }
        }

        public string[] PrimaryKeyColumns
        {
            get
            {
                return Ferda.Modules.Helpers.Common.Csv.Csv2Strings(
                    this.boxModule.GetPropertyString(DataMatrixBoxInfo.PrimaryKeyColumnsPropertyName));
            }
        }

        private RecordsCountCache recordsCountCache = new RecordsCountCache();
        public long RecordsCount
        {
            get
            {
                DatabaseStruct databaseStruct = GetDatabaseFunctionsPrx().getDatabase();
                return recordsCountCache.Value(
                    boxModule.StringIceIdentity,
                    databaseStruct.lastReloadInfo,
                    databaseStruct.connectionString,
                    DataMatrixName);
            }
        }
        #endregion

        #region Functions
        public override DataMatrixStruct getDataMatrix(Ice.Current __current)
        {
            DatabaseStruct databaseStruct = this.GetDatabaseFunctionsPrx().getDatabase();

            DataMatrixStruct result = new DataMatrixStruct();

            result.database = databaseStruct;
            result.dataMatrixName = DataMatrixName;
            result.recordsCount = RecordsCount;
            result.primaryKeyColumns = PrimaryKeyColumns;
            result.explainDataMatrix = explainDataMatrixStructure();

            return result;
        }

        private ColumnsNamesCache columnsNamesCache = new ColumnsNamesCache();
        public override string[] getColumns(Ice.Current __current)
        {
            DatabaseStruct databaseStruct = GetDatabaseFunctionsPrx().getDatabase();
            return columnsNamesCache.Value(
                boxModule.StringIceIdentity,
                databaseStruct.lastReloadInfo,
                databaseStruct.connectionString,
                DataMatrixName);
        }

        private ExplainDataMatrixStructureCache explainDataMatrixStructureCache = new ExplainDataMatrixStructureCache();
        public override ColumnInfo[] explainDataMatrixStructure(Ice.Current __current)
        {
            DatabaseStruct databaseStruct = GetDatabaseFunctionsPrx().getDatabase();
            return explainDataMatrixStructureCache.Value(
                boxModule.StringIceIdentity,
                databaseStruct.lastReloadInfo,
                databaseStruct.connectionString,
                DataMatrixName);
        }

        #endregion

        #region Sockets
        public DatabaseFunctionsPrx GetDatabaseFunctionsPrx()
        {
            return DatabaseFunctionsPrxHelper.checkedCast(
                SocketConnections.GetObjectPrx(boxModule, "Database")
                );
        }
        #endregion

        #region BoxInfo
        public string[] GetTablesNames()
        {
            try
            {
                return GetDatabaseFunctionsPrx().getTables();
            }
            catch (Ferda.Modules.BoxRuntimeError) { }
            return new string[0];
        }

        public long GetRecordsCount()
        {
            try
            {
                return RecordsCount;
            }
            catch (Ferda.Modules.BoxRuntimeError) { }
            return 0;
        }

        public string[] GetColumns()
        {
            try
            {
                return getColumns();
            }
            catch (Ferda.Modules.BoxRuntimeError) { }
            return new string[0];
        }
        #endregion

        private string connectionString
        {
            get
            {
                return GetDatabaseFunctionsPrx().getDatabase().connectionString;
            }
        }
    }
}
