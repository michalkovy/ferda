#ifndef	FERDA_MODULES_BOXES_FFTTask
#define FERDA_MODULES_BOXES_FFTTask

#include <Modules/BuiltinSequences.ice>
#include <Modules/Boxes/LISpMinerTasks/AbstractLMTask.ice>
#include <Modules/Boxes/DataMiningCommon/BooleanPartialCedentSetting/BooleanPartialCedentSetting.ice>
#include <Modules/Boxes/DataMiningCommon/CategorialPartialCedentSetting/CategorialPartialCedentSetting.ice>
#include <Modules/Boxes/LISpMinerTasks/FFTTask/Quantifiers/AbstractFFTQuantifier.ice>

module Ferda {
	module Modules {
		module Boxes {
			module FFTTask	{

				struct TaskStruct {
					Ferda::Modules::Boxes::DataMiningCommon::BooleanPartialCedentSetting::BooleanPartialCedentSettingStructSeq antecedentSetting;
					Ferda::Modules::Boxes::DataMiningCommon::BooleanPartialCedentSetting::BooleanPartialCedentSettingStructSeq succedentSetting;
					Ferda::Modules::Boxes::DataMiningCommon::BooleanPartialCedentSetting::BooleanPartialCedentSettingStructSeq conditionSetting;
					Ferda::Modules::Boxes::AbstractLMTask::QuantifierSettingStructSeq quantifiers;
					MissigTypeEnum missingType;
					bool prolong100A;
					bool prolong100S;
					bool includeSymetric;
				};

				interface FFTTaskFunctions extends Ferda::Modules::Boxes::AbstractLMTask::AbstractLMTaskFunctions {
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