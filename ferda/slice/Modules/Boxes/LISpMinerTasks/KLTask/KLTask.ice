#ifndef	FERDA_MODULES_BOXES_KLTask
#define FERDA_MODULES_BOXES_KLTask

#include <Modules/BuiltinSequences.ice>
#include <Modules/Boxes/LISpMinerTasks/AbstractLMTask.ice>
#include <Modules/Boxes/DataMiningCommon/BooleanPartialCedentSetting/BooleanPartialCedentSetting.ice>
#include <Modules/Boxes/DataMiningCommon/CategorialPartialCedentSetting/CategorialPartialCedentSetting.ice>
#include <Modules/Boxes/LISpMinerTasks/KLTask/Quantifiers/AbstractKLQuantifier.ice>

module Ferda {
	module Modules {
		module Boxes {
			module KLTask	{

				struct TaskStruct {
					Ferda::Modules::Boxes::DataMiningCommon::CategorialPartialCedentSetting::CategorialPartialCedentSettingStructSeq antecedentSetting;
					Ferda::Modules::Boxes::DataMiningCommon::CategorialPartialCedentSetting::CategorialPartialCedentSettingStructSeq succedentSetting;
					Ferda::Modules::Boxes::DataMiningCommon::BooleanPartialCedentSetting::BooleanPartialCedentSettingStructSeq conditionSetting;
					Ferda::Modules::Boxes::AbstractLMTask::QuantifierSettingStructSeq quantifiers;
				};

				interface KLTaskFunctions extends Ferda::Modules::Boxes::AbstractLMTask::AbstractLMTaskFunctions {
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
#endif;