#ifndef	FERDA_MODULES_BOXES_SDFFTTask
#define FERDA_MODULES_BOXES_SDFFTTask

#include <Modules/BuiltinSequences.ice>
#include <Modules/Boxes/LISpMinerTasks/AbstractLMTask.ice>
#include <Modules/Boxes/DataMiningCommon/BooleanPartialCedentSetting/BooleanPartialCedentSetting.ice>
#include <Modules/Boxes/DataMiningCommon/CategorialPartialCedentSetting/CategorialPartialCedentSetting.ice>
#include <Modules/Boxes/LISpMinerTasks/SDFFTTask/Quantifiers/AbstractSDFFTQuantifier.ice>

module Ferda {
	module Modules {
		module Boxes {
			module SDFFTTask	{

				struct TaskStruct {
					Ferda::Modules::Boxes::DataMiningCommon::BooleanPartialCedentSetting::BooleanPartialCedentSettingStructSeq antecedentSetting;
					Ferda::Modules::Boxes::DataMiningCommon::BooleanPartialCedentSetting::BooleanPartialCedentSettingStructSeq succedentSetting;
					Ferda::Modules::Boxes::DataMiningCommon::BooleanPartialCedentSetting::BooleanPartialCedentSettingStructSeq conditionSetting;
					Ferda::Modules::Boxes::DataMiningCommon::BooleanPartialCedentSetting::BooleanPartialCedentSettingStructSeq firstCedentSetting;
					Ferda::Modules::Boxes::DataMiningCommon::BooleanPartialCedentSetting::BooleanPartialCedentSettingStructSeq secondCedentSetting;
					WorkingWithCedentsFirstSetEnum firstSet;
					WorkingWithCedentsSecondSetEnum secondSet;
					Ferda::Modules::Boxes::AbstractLMTask::QuantifierSettingStructSeq quantifiers;
					MissigTypeEnum missingType;
				};

				interface SDFFTTaskFunctions extends Ferda::Modules::Boxes::AbstractLMTask::AbstractLMTaskFunctions {
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