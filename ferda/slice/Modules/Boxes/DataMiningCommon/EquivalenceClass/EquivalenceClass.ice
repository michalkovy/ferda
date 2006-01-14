#ifndef	FERDA_MODULES_BOXES_DataMiningCommon_EquivalenceClass
#define FERDA_MODULES_BOXES_DataMiningCommon_EquivalenceClass

#include <Modules/BuiltinSequences.ice>
#include <Modules/Modules.ice>
#include <Modules/Common.ice>
#include <Modules/Boxes/DataMiningCommon/AtomSetting/AtomSetting.ice>

module Ferda {
	module Modules {
		module Boxes {
			module DataMiningCommon {
				module EquivalenceClass {

					struct EquivalenceClassStruct	{
						int identifier; //boxModule.Manager.getProjectInformation().getProjectIdentifier(Ice.Util.identityToString(this.boxModule.MyIdentity));
						IntSeq attributesIdentifiers;
						IntSeq literalsIdentifiers;
					};
					sequence<EquivalenceClassStruct> EquivalenceClassStructSeq;

					interface EquivalenceClassFunctions {
						nonmutating EquivalenceClassStruct getEquivalenceClass()
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