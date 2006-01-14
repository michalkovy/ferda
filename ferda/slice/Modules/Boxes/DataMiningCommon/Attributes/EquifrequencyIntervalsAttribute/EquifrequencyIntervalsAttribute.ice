#ifndef	FERDA_MODULES_BOXES_DataMiningCommon_Attributes_EquifrequencyIntervalsAttribute
#define FERDA_MODULES_BOXES_DataMiningCommon_Attributes_EquifrequencyIntervalsAttribute

#include <Modules/BuiltinSequences.ice>
#include <Modules/Modules.ice>
#include <Modules/Common.ice>
#include <Modules/Boxes/DataMiningCommon/Attributes/AbstractAttribute.ice>

module Ferda {
	module Modules {
		module Boxes {
			module DataMiningCommon {
				module Attributes {
					module EquifrequencyIntervalsAttribute	{

						interface EquifrequencyIntervalsAttributeFunctions extends Ferda::Modules::Boxes::DataMiningCommon::Attributes::AbstractAttributeFunctions {
						};
					};
				};
			};
		};
	};
};
#endif;