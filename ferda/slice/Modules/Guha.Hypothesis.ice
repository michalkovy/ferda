#ifndef FERDA_GUHA_HYPOTHESIS
#define FERDA_GUHA_HYPOTHESIS

#include <Modules/BuiltinSequences.ice>
#include <Modules/BasicPropertyTypes.ice>
#include <Modules/Exceptions.ice>

module Ferda {
	module Guha {
		module Hypothesis {   
		
			enum OperatorEnum
			{
				Conjunction,
				Disjunction,
				FourFoldTableQuantifier,
				SDFourFoldTableQunatifier,
				KLQuantifier
				//...
			};
			
			interface Formula; //forwad declaration
			
			sequence<Formula> FormulaSeq;
			
			interface Formula
			{
				OperatorEnum GetOperator();
				FormulaSeq GetOperands();
			};

		};
	};
};

#endif