#ifndef	FERDA_MODULES_BOXES_CFTask
#define FERDA_MODULES_BOXES_CFTask

#include <Modules/BuiltinSequences.ice>
#include <Modules/Boxes/LISpMinerTasks/AbstractLMTask.ice>
#include <Modules/Boxes/DataMiningCommon/BooleanPartialCedentSetting/BooleanPartialCedentSetting.ice>
#include <Modules/Boxes/DataMiningCommon/CategorialPartialCedentSetting/CategorialPartialCedentSetting.ice>
#include <Modules/Boxes/LISpMinerTasks/CFTask/Quantifiers/AbstractCFQuantifier.ice>

module Ferda {
	module Modules {
		module Boxes {
			module LISpMinerTasks {
				module CFTask	{

					struct TaskStruct {
						Ferda::Modules::Boxes::DataMiningCommon::CategorialPartialCedentSetting::CategorialPartialCedentSettingStructSeq antecedentSetting;
						Ferda::Modules::Boxes::DataMiningCommon::BooleanPartialCedentSetting::BooleanPartialCedentSettingStructSeq conditionSetting;
						Ferda::Modules::Boxes::LISpMinerTasks::AbstractLMTask::QuantifierSettingStructSeq quantifiers;
					};

					interface CFTaskFunctions extends Ferda::Modules::Boxes::LISpMinerTasks::AbstractLMTask::AbstractLMTaskFunctions {
						nonmutating TaskStruct getTask()
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