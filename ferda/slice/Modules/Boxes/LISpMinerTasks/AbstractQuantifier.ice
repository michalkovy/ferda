#ifndef	FERDA_MODULES_BOXES_AbstractQuantifier
#define FERDA_MODULES_BOXES_AbstractQuantifier

#include <Modules/Modules.ice>
#include <Modules/Common.ice>

module Ferda {
	module Modules {
		module Boxes {
			module LISpMinerTasks {
				module AbstractQuantifier {

					interface AbstractQuantifierFunctions;

					interface AbstractQuantifierFunctions {
						nonmutating double Value(Ferda::Modules::AbstractQuantifierSetting setting)
								throws
									Ferda::Modules::BoxRuntimeError,
									Ferda::Modules::BadValueError,
									Ferda::Modules::BadParamsError,
									Ferda::Modules::NoConnectionInSocketError;
						nonmutating bool Validity(Ferda::Modules::AbstractQuantifierSetting setting)
								throws
									Ferda::Modules::BoxRuntimeError,
									Ferda::Modules::BadValueError,
									Ferda::Modules::BadParamsError,
									Ferda::Modules::NoConnectionInSocketError;
						nonmutating string QuantifierIdentifier();
					};
				};
			};
		};
	};
};
#endif;