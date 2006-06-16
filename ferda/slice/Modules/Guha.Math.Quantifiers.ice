#ifndef FERDA_GUHA_MATH_QUANTIFIERS
#define FERDA_GUHA_MATH_QUANTIFIERS

#include <Modules/BuiltinSequences.ice>
#include <Modules/BasicPropertyTypes.ice>
#include <Modules/Exceptions.ice>
#include <Modules/Guha.Data.ice>

module Ferda {
	module Guha {
		module Math {   
			module Quantifiers {
			
				enum OperationModeEnum
				{
					FirstSetFrequencies,
					SecondSetFrequencies,
					DifferencesOfAbsoluteFrequencies,
					DifferencesOfRelativeFrequencies,
					DifferenceOfQuantifierValues,
					AbsoluteDifferenceOfQuantifierValues
				};
				
				enum QuantifierSemanticEnum
				{
					MinimizeValue,
					MaximizeValue,
					OnlyEquals
				};
				
				enum BoundTypeEnum
				{
					Number,
					All,
					Half
				};
				
				struct Bound
				{
					BoundTypeEnum boundType;
					int number;
				};

				enum QuantifierClassEnum
				{
					Unknown,
					Implicational,
					DoubleImplicational,
					SigmaDoubleImplicational,
					Equivalency,
					SigmaEquivalency
				};
				sequence<QuantifierClassEnum> QuantifierClassEnumSeq;
				
				enum PerformanceDifficultyEnum
				{
					Easy,
					QuiteEasy,
					Medium, //default
					QuiteDifficult,
					Difficult
				};

				class QuantifierSetting
				{
					OperationModeEnum operationMode;
					QuantifierSemanticEnum quantifierSemantic;
					PerformanceDifficultyEnum quantifierPerformanceDifficulty;
					bool needsNumericValues;
					Bound FromRow;
					Bound ToRow;
					Bound FromColumn;
					Bound ToColumn;
					Ferda::Guha::Data::CardinalityEnum supportedData;
					QuantifierClassEnumSeq quantifierClasses;
					bool supportsFloatContingencyTable;					
				};
				
				enum UnitsEnum
				{
					Irrelevant,
					AbsoluteNumber,
					RelativeToActCondition,
					RelativeToAllObjects,
					RelativeToMaxFrequency
				};
				
			};
		};
	};
};

#endif
