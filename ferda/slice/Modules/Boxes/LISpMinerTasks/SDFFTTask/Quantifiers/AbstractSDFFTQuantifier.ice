#ifndef	FERDA_MODULES_BOXES_SDFFTTask_Quantifiers_AbstractQuantifier
#define FERDA_MODULES_BOXES_SDFFTTask_Quantifiers_AbstractQuantifier

#include <Modules/Modules.ice>
#include <Modules/Common.ice>
#include <Modules/Boxes/LISpMinerTasks/AbstractQuantifier.ice>

module Ferda {
	module Modules {
		module Boxes {
			module LISpMinerTasks {
				module SDFFTTask {
					module Quantifiers {
						module AbstractSDFFTQuantifier {

							/*
							class QuantifierSetting extends Ferda::Modules::Boxes::LISpMinerTasks::AbstractQuantifier::AbstractQuantifierSetting {
								FourFoldTableStruct firstFourFoldTable;
								FourFoldTableStruct secondFourFoldTable;
							};
							*/

							interface AbstractSDFFTQuantifierFunctions extends Ferda::Modules::Boxes::LISpMinerTasks::AbstractQuantifier::AbstractQuantifierFunctions {
							};
						};
					};
				};
			};
		};
	};
};
#endif;