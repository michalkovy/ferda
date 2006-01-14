#ifndef	FERDA_MODULES_BOXES_DataMiningCommon_Attributes_EquidistantIntervalsAttribute
#define FERDA_MODULES_BOXES_DataMiningCommon_Attributes_EquidistantIntervalsAttribute

#include <Modules/BuiltinSequences.ice>
#include <Modules/Modules.ice>
#include <Modules/Common.ice>
#include <Modules/Boxes/DataMiningCommon/Attributes/AbstractAttribute.ice>

module Ferda {
	module Modules {
		module Boxes {
			module DataMiningCommon {
				module Attributes {
					module EquidistantIntervalsAttribute	{

						interface EquidistantIntervalsAttributeFunctions extends Ferda::Modules::Boxes::DataMiningCommon::Attributes::AbstractAttributeFunctions {
						};
					};
				};
			};
		};
	};
};
#endif;