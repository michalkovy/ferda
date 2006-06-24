#ifndef FERDA_GUHA_MATH
#define FERDA_GUHA_MATH

#include <Modules/BuiltinSequences.ice>
#include <Modules/BasicPropertyTypes.ice>
#include <Modules/Exceptions.ice>
#include <Modules/Guha.Data.ice>

module Ferda {
	module Guha {
		module Math {   
			
			enum RelationEnum
			{
				Greater,
				GreaterOrEqual,
				Equal,
				LessOrEqual,
				Less
			};
				
		};
	};
};

#endif
