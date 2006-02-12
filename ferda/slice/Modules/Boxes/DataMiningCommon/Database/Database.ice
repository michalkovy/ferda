#ifndef	FERDA_MODULES_BOXES_DataMiningCommon_Database
#define FERDA_MODULES_BOXES_DataMiningCommon_Database

#include <Modules/BuiltinSequences.ice>
#include <Modules/Modules.ice>
#include <Modules/Common.ice>

module Ferda {
	module Modules {
		module Boxes {
			module DataMiningCommon {
				module Database	{

					struct DatabaseStruct	{
						string connectionString;
						DateTimeT lastReloadInfo;
					};

					struct DataMatrixInfo
					{
						string name;
						string type;
						string remarks;
						long rowCount;
					};
					sequence<DataMatrixInfo> DataMatrixInfoSeq;

					//see corresponding fields of System.Data.Odbc.OdbcConnection (.NET Framework)
					struct ConnectionInfo
					{
            string databaseName;
            string dataSource;
            string driver;
            string serverVersion;
					};

					interface	TablesNamesProvider	{
						nonmutating	StringSeq	getTables()
							throws
								Ferda::Modules::BoxRuntimeError,
								Ferda::Modules::BadValueError,
								Ferda::Modules::BadParamsError,
								Ferda::Modules::NoConnectionInSocketError;
					};

					interface DatabaseFunctions extends TablesNamesProvider {
						nonmutating	DatabaseStruct getDatabase()
							throws
								Ferda::Modules::BoxRuntimeError,
								Ferda::Modules::BadValueError,
								Ferda::Modules::BadParamsError,
								Ferda::Modules::NoConnectionInSocketError;
						nonmutating DataMatrixInfoSeq explainDatabaseStructure()
							throws
								Ferda::Modules::BoxRuntimeError,
								Ferda::Modules::BadValueError,
								Ferda::Modules::BadParamsError,
								Ferda::Modules::NoConnectionInSocketError;
					};
				};
			};
		};
	};
};
#endif;