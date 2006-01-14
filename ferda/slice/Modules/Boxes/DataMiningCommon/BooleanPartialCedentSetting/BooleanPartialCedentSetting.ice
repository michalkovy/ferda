#ifndef	FERDA_MODULES_BOXES_DataMiningCommon_BooleanPartialCedentSetting
#define FERDA_MODULES_BOXES_DataMiningCommon_BooleanPartialCedentSetting

#include <Modules/BuiltinSequences.ice>
#include <Modules/Modules.ice>
#include <Modules/Common.ice>
#include <Modules/Boxes/DataMiningCommon/LiteralSetting/LiteralSetting.ice>
#include <Modules/Boxes/DataMiningCommon/EquivalenceClass/EquivalenceClass.ice>

module Ferda {
	module Modules {
		module Boxes {
			module DataMiningCommon {
				module BooleanPartialCedentSetting {

					struct BooleanPartialCedentSettingStruct {
						Ferda::Modules::Boxes::DataMiningCommon::LiteralSetting::LiteralSettingStructSeq literalSettings;
						Ferda::Modules::Boxes::DataMiningCommon::EquivalenceClass::EquivalenceClassStructSeq equivalenceClasses;
						int identifier; //boxModule.Manager.getProjectInformation().getProjectIdentifier(Ice.Util.identityToString(this.boxModule.MyIdentity));
						long minLen;
						long maxLen;
					};
					sequence<BooleanPartialCedentSettingStruct> BooleanPartialCedentSettingStructSeq;

					interface BooleanPartialCedentSettingFunctions {
						nonmutating BooleanPartialCedentSettingStruct getBooleanPartialCedentSetting()
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