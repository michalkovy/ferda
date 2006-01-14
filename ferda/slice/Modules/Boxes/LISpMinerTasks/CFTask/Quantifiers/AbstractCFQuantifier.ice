#ifndef	FERDA_MODULES_BOXES_CFTask_Quantifiers_AbstractQuantifier
#define FERDA_MODULES_BOXES_CFTask_Quantifiers_AbstractQuantifier

#include <Modules/Modules.ice>
#include <Modules/Common.ice>
#include <Modules/Boxes/LISpMinerTasks/AbstractQuantifier.ice>

module Ferda {
	module Modules {
		module Boxes {
			module CFTask {
				module Quantifiers {
					module AbstractCFQuantifier {

						/*
						class QuantifierSetting extends Ferda::Modules::Boxes::AbstractQuantifier::AbstractQuantifierSetting {
							IntSeq contingencyTableRow;
						};
						*/

						interface AbstractCFQuantifierFunctions extends Ferda::Modules::Boxes::AbstractQuantifier::AbstractQuantifierFunctions {
						};
					};
				};
			};
		};
	};
};
#endif;