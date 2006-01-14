#ifndef	FERDA_MODULES_BOXES_DataMiningCommon_CategorialPartialCedentSetting
#define FERDA_MODULES_BOXES_DataMiningCommon_CategorialPartialCedentSetting

#include <Modules/BuiltinSequences.ice>
#include <Modules/Modules.ice>
#include <Modules/Common.ice>
#include <Modules/Boxes/DataMiningCommon/Attributes/AbstractAttribute.ice>

module Ferda {
	module Modules {
		module Boxes {
			module DataMiningCommon {
				module CategorialPartialCedentSetting {

					struct CategorialPartialCedentSettingStruct {
						Ferda::Modules::Boxes::DataMiningCommon::Attributes::AbstractAttributeStructSeq attributes;
						int identifier; //boxModule.Manager.getProjectInformation().getProjectIdentifier(Ice.Util.identityToString(this.boxModule.MyIdentity));
						long minLen;
						long maxLen;
					};
					sequence<CategorialPartialCedentSettingStruct> CategorialPartialCedentSettingStructSeq;

					interface CategorialPartialCedentSettingFunctions {
						nonmutating CategorialPartialCedentSettingStruct getCategorialPartialCedentSetting()
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