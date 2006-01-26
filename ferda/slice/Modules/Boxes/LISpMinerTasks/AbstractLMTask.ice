#ifndef	FERDA_MODULES_BOXES_AbstractLMTask
#define FERDA_MODULES_BOXES_AbstractLMTask

#include <Modules/Common.ice>
#include <Modules/Boxes/LISpMinerTasks/AbstractQuantifier.ice>
#include <Modules/Boxes/DataMiningCommon/BooleanPartialCedentSetting/BooleanPartialCedentSetting.ice>
#include <Modules/Boxes/DataMiningCommon/CategorialPartialCedentSetting/CategorialPartialCedentSetting.ice>

module Ferda {
	module Modules {
		module Boxes {
			module LISpMinerTasks {
				module AbstractLMTask	{

					struct QuantifierSettingStruct {
						string typeIdentifier;
						Ferda::Modules::PropertySettingSeq setting;
					};
					sequence<QuantifierSettingStruct> QuantifierSettingStructSeq;

					struct QuantifierProvider
					{
						Ferda::Modules::Boxes::LISpMinerTasks::AbstractQuantifier::AbstractQuantifierFunctions* functions;
						string localizedBoxLabel;
						string userBoxLabel;
					};
					sequence<QuantifierProvider> QuantifierProviderSeq;

					interface AbstractLMTaskFunctions {
						nonmutating HypothesisStructSeq getResult()
								throws
									Ferda::Modules::BoxRuntimeError,
									Ferda::Modules::BadValueError,
									Ferda::Modules::BadParamsError,
									Ferda::Modules::NoConnectionInSocketError;
						nonmutating GeneratingStruct getGeneratingInfo()
								throws
									Ferda::Modules::BoxRuntimeError,
									Ferda::Modules::BadValueError,
									Ferda::Modules::BadParamsError,
									Ferda::Modules::NoConnectionInSocketError;
						nonmutating QuantifierProviderSeq getQuantifierProviders()
								throws
									Ferda::Modules::BoxRuntimeError,
									Ferda::Modules::BadValueError,
									Ferda::Modules::BadParamsError,
									Ferda::Modules::NoConnectionInSocketError;
					};
				};
			};
		};
	};
};
#endif;