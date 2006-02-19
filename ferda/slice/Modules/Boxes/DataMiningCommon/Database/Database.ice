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

					struct DatabaseInfo	{
						string odbcConnectionString;
						DateTimeT lastReloadInfo;
					};

					struct DataMatrixSchemaInfo
					{
						string name;
						string type;
						string remarks;
						long rowCount;
					};
					sequence<DataMatrixSchemaInfo> DataMatrixSchemaInfoSeq;

					//see corresponding fields of System.Data.Odbc.OdbcConnection (.NET Framework)
					struct ConnectionInfo
					{
            string databaseName;
            string dataSource;
            string driver;
            string serverVersion;
					};

					interface	DataMatrixNamesProviderFunctions	{
						nonmutating	StringSeq	getDataMatrixNames()
							throws
								Ferda::Modules::BoxRuntimeError,
								Ferda::Modules::BadValueError,
								Ferda::Modules::BadParamsError,
								Ferda::Modules::NoConnectionInSocketError;
					};

					interface DatabaseFunctions extends DataMatrixNamesProviderFunctions {
						nonmutating	DatabaseInfo getDatabaseInfo()
							throws
								Ferda::Modules::BoxRuntimeError,
								Ferda::Modules::BadValueError,
								Ferda::Modules::BadParamsError,
								Ferda::Modules::NoConnectionInSocketError;
						nonmutating DataMatrixSchemaInfoSeq explain()
							throws
								Ferda::Modules::BoxRuntimeError,
								Ferda::Modules::BadValueError,
								Ferda::Modules::BadParamsError,
								Ferda::Modules::NoConnectionInSocketError;
						nonmutating ConnectionInfo getConnectionInfo()
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