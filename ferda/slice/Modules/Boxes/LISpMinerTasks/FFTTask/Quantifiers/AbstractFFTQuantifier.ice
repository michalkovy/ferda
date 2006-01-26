#ifndef	FERDA_MODULES_BOXES_FFTTask_Quantifiers_AbstractQuantifier
#define FERDA_MODULES_BOXES_FFTTask_Quantifiers_AbstractQuantifier

#include <Modules/Modules.ice>
#include <Modules/Common.ice>
#include <Modules/Boxes/LISpMinerTasks/AbstractQuantifier.ice>

module Ferda {
	module Modules {
		module Boxes {
			module LISpMinerTasks {
				module FFTTask {
					module Quantifiers {
						module AbstractFFTQuantifier {

							/*
							class QuantifierSetting extends Ferda::Modules::Boxes::LISpMinerTasks::AbstractQuantifier::AbstractQuantifierSetting {
								FourFoldTableStruct fourFoldTable;
							};
							*/

							interface AbstractFFTQuantifierFunctions extends Ferda::Modules::Boxes::LISpMinerTasks::AbstractQuantifier::AbstractQuantifierFunctions {
							};
						};
					};
				};
			};
		};
	};
};
#endif;