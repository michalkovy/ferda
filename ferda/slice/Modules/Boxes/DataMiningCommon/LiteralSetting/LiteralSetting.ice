#ifndef	FERDA_MODULES_BOXES_DataMiningCommon_LiteralSetting
#define FERDA_MODULES_BOXES_DataMiningCommon_LiteralSetting

#include <Modules/BuiltinSequences.ice>
#include <Modules/Modules.ice>
#include <Modules/Common.ice>
#include <Modules/Boxes/DataMiningCommon/AtomSetting/AtomSetting.ice>

module Ferda {
	module Modules {
		module Boxes {
			module DataMiningCommon {
				module LiteralSetting {

				struct LiteralSettingStruct	{
					Ferda::Modules::Boxes::DataMiningCommon::AtomSetting::AtomSettingStruct atomSetting;
					int identifier; //boxModule.Manager.getProjectInformation().getProjectIdentifier(Ice.Util.identityToString(this.boxModule.MyIdentity));
					LiteralTypeEnum literalType;
					GaceTypeEnum gaceType;
				};
				sequence<LiteralSettingStruct> LiteralSettingStructSeq;

				interface LiteralSettingFunctions{
					nonmutating LiteralSettingStruct getLiteralSetting()
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