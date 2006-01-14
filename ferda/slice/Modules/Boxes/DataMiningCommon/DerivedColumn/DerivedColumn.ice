#ifndef	FERDA_MODULES_BOXES_DataMiningCommon_DerivedColumn
#define FERDA_MODULES_BOXES_DataMiningCommon_DerivedColumn

#include <Modules/BuiltinSequences.ice>
#include <Modules/Modules.ice>
#include <Modules/Common.ice>
#include <Modules/Boxes/DataMiningCommon/Column/Column.ice>

module Ferda {
	module Modules {
		module Boxes {
			module DataMiningCommon {
				module DerivedColumn	{

					interface DerivedColumnFunctions extends Ferda::Modules::Boxes::DataMiningCommon::Column::ColumnFunctions {
					};
				};
			};
		};
	};
};
#endif;