#ifndef FERDA_MODULES_BOXES_COMMON
#define FERDA_MODULES_BOXES_COMMON

#include <Modules/BuiltinSequences.ice>
#include <Modules/BasicPropertyTypes.ice>

module Ferda {
	module Modules {
		
		struct GuidStruct
		{
			string value;
		};
		sequence<GuidStruct> GuidStructSeq;
		sequence<GuidStructSeq> GuidStructSeqSeq;

	/*
		enum GenerationStateEnum
		{
		    DidNotStart,
		    Running,
		    Completed,
		    Interrupted,
		    Canceled,
		    Failed
		};
	*/

	};
};

#endif
