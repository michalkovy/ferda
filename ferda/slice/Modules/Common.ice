#ifndef FERDA_MODULES_BOXES_COMMON
#define FERDA_MODULES_BOXES_COMMON

#include <Modules/BuiltinSequences.ice>
#include <Modules/BasicPropertyTypes.ice>
#include <Modules/Modules.ice>

module Ferda {
	module Modules {
		
		struct GuidStruct
		{
			string value;
		};
		sequence<GuidStruct> GuidStructSeq;
		sequence<GuidStructSeq> GuidStructSeqSeq;

		enum WorkingWithCedentsFirstSetEnum
		{
				Cedent1
		};
		enum WorkingWithCedentsSecondSetEnum
		{
		    Cedent2,
		    Cedent1AndCedent2
		};
		enum CedentEnum {
				Antecedent,
				Succedent,
				Condition,
				FirstSet,
				SecondSet
		};
		enum GenerationStateEnum
		{
		    DidNotStart,
		    Running,
		    Completed,
		    Interrupted,
		    Canceled,
		    Failed
		};
		
		struct GeneratingStruct {
			GenerationStateEnum generationState;
			long generationNrOfTests;
			long generationNrOfHypotheses;
			DateTimeT generationStartTime;
			TimeT generationTotalTime;
		};

				enum LiteralDimensionEnum {
					FirstDimension,
					SecondDimension
				};

				struct LiteralStruct {
					CedentEnum cedentType;
					StringSeq categoriesNames;
					string literalName;
					double categoriesValues;
					int literalIdentifier;
				};
				sequence<LiteralStruct> LiteralStructSeq;

				struct BooleanLiteralStruct {
					CedentEnum cedentType;
					StringSeq categoriesNames;
					bool negation;
					string literalName; //attribute.nameInLiterals
					int literalIdentifier;
				};
				sequence<BooleanLiteralStruct> BooleanLiteralStructSeq;

				struct HypothesisStruct {
					//AbstractQuantifierSetting quantifierSetting;
					BooleanLiteralStructSeq booleanLiterals;
					LiteralStructSeq literals;
				};
				sequence<HypothesisStruct> HypothesisStructSeq;
	};
};

#endif
