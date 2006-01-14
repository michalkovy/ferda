#ifndef	FERDA_MODULES_BOXES_SDCFTask
#define FERDA_MODULES_BOXES_SDCFTask

#include <Modules/BuiltinSequences.ice>
#include <Modules/Boxes/LISpMinerTasks/AbstractLMTask.ice>
#include <Modules/Boxes/DataMiningCommon/BooleanPartialCedentSetting/BooleanPartialCedentSetting.ice>
#include <Modules/Boxes/DataMiningCommon/CategorialPartialCedentSetting/CategorialPartialCedentSetting.ice>
#include <Modules/Boxes/LISpMinerTasks/SDCFTask/Quantifiers/AbstractSDCFQuantifier.ice>

module Ferda {
	module Modules {
		module Boxes {
			module SDCFTask	{

				struct TaskStruct {
					Ferda::Modules::Boxes::DataMiningCommon::CategorialPartialCedentSetting::CategorialPartialCedentSettingStructSeq antecedentSetting;
					Ferda::Modules::Boxes::DataMiningCommon::BooleanPartialCedentSetting::BooleanPartialCedentSettingStructSeq conditionSetting;
					Ferda::Modules::Boxes::DataMiningCommon::BooleanPartialCedentSetting::BooleanPartialCedentSettingStructSeq firstCedentSetting;
					Ferda::Modules::Boxes::DataMiningCommon::BooleanPartialCedentSetting::BooleanPartialCedentSettingStructSeq secondCedentSetting;
					WorkingWithCedentsFirstSetEnum firstSet;
					WorkingWithCedentsSecondSetEnum secondSet;
					Ferda::Modules::Boxes::AbstractLMTask::QuantifierSettingStructSeq quantifiers;
				};

				interface SDCFTaskFunctions extends Ferda::Modules::Boxes::AbstractLMTask::AbstractLMTaskFunctions {
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