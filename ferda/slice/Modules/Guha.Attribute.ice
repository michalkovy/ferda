#ifndef FERDA_GUHA_ATTRIBUTE
#define FERDA_GUHA_ATTRIBUTE

#include <Modules/BuiltinSequences.ice>
#include <Modules/BasicPropertyTypes.ice>
#include <Modules/Exceptions.ice>

module Ferda {
	module Guha {
		module Attribute {
		
			enum BoundaryEnum {
				Closed,
				Open,
				Infinity
			};
			
		};
	};
};

#endif
