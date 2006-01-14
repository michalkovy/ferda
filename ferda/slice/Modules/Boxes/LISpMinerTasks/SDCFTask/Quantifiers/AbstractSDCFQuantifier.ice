#ifndef	FERDA_MODULES_BOXES_SDCFTask_Quantifiers_AbstractQuantifier
#define FERDA_MODULES_BOXES_SDCFTask_Quantifiers_AbstractQuantifier

#include <Modules/Modules.ice>
#include <Modules/Common.ice>
#include <Modules/Boxes/LISpMinerTasks/AbstractQuantifier.ice>

module Ferda {
	module Modules {
		module Boxes {
			module SDCFTask {
				module Quantifiers {
					module AbstractSDCFQuantifier {

						/*
						class QuantifierSetting extends Ferda::Modules::Boxes::AbstractQuantifier::AbstractQuantifierSetting {
							IntSeq firstContingencyTableRow;
							IntSeq secondContingencyTableRow;
						};
						*/

						interface AbstractSDCFQuantifierFunctions extends Ferda::Modules::Boxes::AbstractQuantifier::AbstractQuantifierFunctions {
						};
					};
				};
			};
		};
	};
};
#endif;