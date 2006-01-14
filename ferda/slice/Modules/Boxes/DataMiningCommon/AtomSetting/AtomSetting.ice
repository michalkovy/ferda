#ifndef	FERDA_MODULES_BOXES_DataMiningCommon_AtomSetting
#define FERDA_MODULES_BOXES_DataMiningCommon_AtomSetting

#include <Modules/BuiltinSequences.ice>
#include <Modules/Modules.ice>
#include <Modules/Common.ice>
#include <Modules/Boxes/DataMiningCommon/Attributes/AbstractAttribute.ice>

module Ferda {
	module Modules {
		module Boxes {
			module DataMiningCommon {
				module AtomSetting {

					struct AtomSettingStruct	{
						Ferda::Modules::Boxes::DataMiningCommon::Attributes::AbstractAttributeStruct abstractAttribute;
						int identifier; //boxModule.Manager.getProjectInformation().getProjectIdentifier(Ice.Util.identityToString(this.boxModule.MyIdentity));
						CoefficientTypeEnum coefficientType;
						StringOpt category; //name of category if coefficientType == OneCategory
						long minLen;
						long maxLen;
					};

					//CategoryMustBeSelectedWhenCoefficientTypeIsOneParticularCategoryError
					//MinLenIsGreaterThanMaxLenError

					interface AtomSettingFunctions{
						nonmutating AtomSettingStruct getAtomSetting()
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