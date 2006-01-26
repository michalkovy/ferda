#ifndef	FERDA_MODULES_BOXES_SDKLTask
#define FERDA_MODULES_BOXES_SDKLTask

#include <Modules/BuiltinSequences.ice>
#include <Modules/Boxes/LISpMinerTasks/AbstractLMTask.ice>
#include <Modules/Boxes/DataMiningCommon/BooleanPartialCedentSetting/BooleanPartialCedentSetting.ice>
#include <Modules/Boxes/DataMiningCommon/CategorialPartialCedentSetting/CategorialPartialCedentSetting.ice>
#include <Modules/Boxes/LISpMinerTasks/SDKLTask/Quantifiers/AbstractSDKLQuantifier.ice>

module Ferda {
	module Modules {
		module Boxes {
			module LISpMinerTasks {
				module SDKLTask	{

					struct TaskStruct {
						Ferda::Modules::Boxes::DataMiningCommon::CategorialPartialCedentSetting::CategorialPartialCedentSettingStructSeq antecedentSetting;
						Ferda::Modules::Boxes::DataMiningCommon::CategorialPartialCedentSetting::CategorialPartialCedentSettingStructSeq succedentSetting;
						Ferda::Modules::Boxes::DataMiningCommon::BooleanPartialCedentSetting::BooleanPartialCedentSettingStructSeq conditionSetting;
						Ferda::Modules::Boxes::DataMiningCommon::BooleanPartialCedentSetting::BooleanPartialCedentSettingStructSeq firstCedentSetting;
						Ferda::Modules::Boxes::DataMiningCommon::BooleanPartialCedentSetting::BooleanPartialCedentSettingStructSeq secondCedentSetting;
						WorkingWithCedentsFirstSetEnum firstSet;
						WorkingWithCedentsSecondSetEnum secondSet;
						Ferda::Modules::Boxes::LISpMinerTasks::AbstractLMTask::QuantifierSettingStructSeq quantifiers;
					};

					interface SDKLTaskFunctions extends Ferda::Modules::Boxes::LISpMinerTasks::AbstractLMTask::AbstractLMTaskFunctions {
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