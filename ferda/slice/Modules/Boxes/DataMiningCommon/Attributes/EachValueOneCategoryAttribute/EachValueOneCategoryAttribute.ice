#ifndef	FERDA_MODULES_BOXES_DataMiningCommon_Attributes_EachValueOneCategoryAttribute
#define FERDA_MODULES_BOXES_DataMiningCommon_Attributes_EachValueOneCategoryAttribute

#include <Modules/BuiltinSequences.ice>
#include <Modules/Modules.ice>
#include <Modules/Common.ice>
#include <Modules/Boxes/DataMiningCommon/Attributes/AbstractAttribute.ice>

module Ferda {
	module Modules {
		module Boxes {
			module DataMiningCommon {
				module Attributes {
					module EachValueOneCategoryAttribute	{

						interface EachValueOneCategoryAttributeFunctions extends Ferda::Modules::Boxes::DataMiningCommon::Attributes::AbstractAttributeFunctions {
						};
					};
				};
			};
		};
	};
};
#endif;