#ifndef	FERDA_MODULES_BOXES_SDKLTask_Quantifiers_AbstractQuantifier
#define FERDA_MODULES_BOXES_SDKLTask_Quantifiers_AbstractQuantifier

#include <Modules/Modules.ice>
#include <Modules/Common.ice>
#include <Modules/Boxes/LISpMinerTasks/AbstractQuantifier.ice>

module Ferda {
	module Modules {
		module Boxes {
			module LISpMinerTasks {
				module SDKLTask {
					module Quantifiers {
						module AbstractSDKLQuantifier {

							/*
							class QuantifierSetting extends Ferda::Modules::Boxes::LISpMinerTasks::AbstractQuantifier::AbstractQuantifierSetting {
								IntSeqSeq firstContingencyTableRows;
								IntSeqSeq secondContingencyTableRows;
							};
							*/

							interface AbstractSDKLQuantifierFunctions extends Ferda::Modules::Boxes::LISpMinerTasks::AbstractQuantifier::AbstractQuantifierFunctions {
							};
						};
					};
				};
			};
		};
	};
};
#endif;