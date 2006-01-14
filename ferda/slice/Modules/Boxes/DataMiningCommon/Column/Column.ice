#ifndef	FERDA_MODULES_BOXES_DataMiningCommon_Column
#define FERDA_MODULES_BOXES_DataMiningCommon_Column

#include <Modules/BuiltinSequences.ice>
#include <Modules/Modules.ice>
#include <Modules/Common.ice>
#include <Modules/Boxes/DataMiningCommon/DataMatrix/DataMatrix.ice>

module Ferda {
	module Modules {
		module Boxes {
			module DataMiningCommon {
				module Column	{

					struct StatisticsStruct {
						string ValueMin;
						string ValueMax;
						string ValueAverage;
						double ValueVariability;
						double ValueStandardDeviation;
						long ValueDistincts;
					};

					enum ColumnTypeEnum {
						Ordinary,
						Derived,
						MultiColumnField
					};

					struct ColumnStruct	{
						Ferda::Modules::Boxes::DataMiningCommon::DataMatrix::DataMatrixStruct dataMatrix;
						ColumnTypeEnum columnType;
						/**
						 *
						 * In column it is a name o the column, in derived column it is a formula.
						 *
						 **/
						string columnSelectExpression;
						ValueSubTypeEnum columnSubType;
						StatisticsStruct statistics;
					};

					interface ColumnCoreFunctions {
					};

					interface ColumnFunctions extends ColumnCoreFunctions {
						nonmutating ColumnStruct getColumn()
							throws
								Ferda::Modules::BoxRuntimeError,
								Ferda::Modules::BadValueError,
								Ferda::Modules::BadParamsError,
								Ferda::Modules::NoConnectionInSocketError;
						nonmutating StringSeq getDistinctValues()
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