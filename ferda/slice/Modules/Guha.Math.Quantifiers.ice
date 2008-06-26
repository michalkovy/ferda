#ifndef FERDA_GUHA_MATH_QUANTIFIERS
#define FERDA_GUHA_MATH_QUANTIFIERS

#include <Modules/BuiltinSequences.ice>
#include <Modules/BasicPropertyTypes.ice>
#include <Modules/Exceptions.ice>
#include <Modules/Common.ice>
#include <Modules/Guha.Data.ice>
#include <Modules/Guha.Math.ice>

/*
THE DOCUMENTATION FOR MAJORITY OF STRUCTURES DEFINED BY THIS FILE CAN BE FOUND
IN src\Modules\Core\Math\Quantifiers\IceDesignDocumentation
*/

module Ferda {
	module Guha {
		module MiningProcessor {

				//FORWARD DECLARATION
				interface BitStringGenerator;
		};
	};
};


module Ferda {
	module Guha {
		module Math {
			module Quantifiers {

				enum MissingInformationHandlingEnum
				{
					Deleting,
					Optimistic,
					Secured
				};

				enum OperationModeEnum
				{
					FirstSetFrequencies,
					SecondSetFrequencies,
					DifferenceOfFrequencies,
					DifferenceOfQuantifierValues
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
					int number; //counted from zero
				};

				enum QuantifierClassEnum
				{
					Unknown,
					Implicational,
					DoubleImplicational,
					SigmaDoubleImplicational,
					Equivalency,
					SigmaEquivalency,
					Symetrical,
					FPropertyQuantifier
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

				enum UnitsEnum
				{
					Irrelevant,
					AbsoluteNumber,
					RelativeToActCondition,
					RelativeToAllObjects,
					RelativeToMaxFrequency
				};

				struct QuantifierSetting
				{
					string stringIceIdentity;
					string boxTypeIdentifier;

					MissingInformationHandlingEnum missingInformationHandling;
					OperationModeEnum operationMode;

					Ferda::Guha::Math::RelationEnum relation;
					double treshold;

					// bounds
					Bound FromRow;
					Bound ToRow;
					Bound FromColumn;
					Bound ToColumn;

					QuantifierClassEnumSeq quantifierClasses;
					PerformanceDifficultyEnum performanceDifficulty;
					bool needsNumericValues;
					Ferda::Guha::Data::CardinalityEnum supportedData;
					UnitsEnum units;
					bool supportsFloatContingencyTable;
				};

				struct QuantifierEvaluateSetting
				{
					Ferda::Modules::DoubleSeqSeq contingencyTable;
					double denominator;

					//numeric values
					Ferda::Modules::GuidStruct numericValuesAttributeId;
					Ferda::Guha::MiningProcessor::BitStringGenerator* numericValuesProviders;
				};
				sequence<QuantifierEvaluateSetting> QuantifierEvaluateSettingSeq;


        /*
        Basic functionality of a quantifier
        */
				interface QuantifierBaseFunctions
				{
				  /*
				  Returns a structure giving information about the quantifier
				  */
					idempotent QuantifierSetting GetQuantifierSetting()
						throws Ferda::Modules::BoxRuntimeError;
						
					/*
					Returns the localized label of the box (for showing in result browser...)
					*/
					idempotent string GetLocalizedBoxLabel(Ferda::Modules::StringSeq localePrefs)
						throws Ferda::Modules::BoxRuntimeError;
						
					/*
					Returns the user label of the box
					*/
					idempotent string GetLocalizedUserBoxLabel(Ferda::Modules::StringSeq localePrefs)
						throws Ferda::Modules::BoxRuntimeError;
						
					/*
					Determines if the contingency table determined by param satisfies the
					quantifier
					*/
					idempotent bool Compute(QuantifierEvaluateSetting param)
						throws Ferda::Modules::BoxRuntimeError;
						
					/*
					Method to compute more quantifier testing
					*/
					idempotent Ferda::Modules::BoolSeq ComputeBatch(QuantifierEvaluateSettingSeq param)
						throws Ferda::Modules::BoxRuntimeError;
					
					/*
					At begining of use call this
					*/
					idempotent void BeginOfUse();
					
					/*
					At end of use call this
					*/
					idempotent void EndOfUse();
				};

				interface QuantifierValueBaseFunctions
				{
					idempotent bool ComputeValidValue(QuantifierEvaluateSetting param, out double value)
						throws Ferda::Modules::BoxRuntimeError;
				};

				interface QuantifierValidFunctions extends QuantifierBaseFunctions
				{
				};

				interface QuantifierSignificantValueFunctions extends QuantifierBaseFunctions, QuantifierValueBaseFunctions
				{
				};

        /*
        Interface for quantifiers that provide significant numeric value
        */
				interface QuantifierValueFunctions extends QuantifierBaseFunctions, QuantifierValueBaseFunctions
				{
					idempotent double ComputeValue(QuantifierEvaluateSetting param)
						throws Ferda::Modules::BoxRuntimeError;
				};

				//FourFold
				//TwoDimensions
				//SingleDimension
				//FourFoldTwoDimensions
				//FourFoldSingleDimension
				//SingleDimensionTwoDimensions
				//FourFoldSingleDimensionTwoDimensions

				interface FourFoldValid extends QuantifierValidFunctions{};
				interface TwoDimensionsValid extends QuantifierValidFunctions{};
				interface SingleDimensionValid extends QuantifierValidFunctions{};
				interface FourFoldTwoDimensionsValid extends QuantifierValidFunctions{};
				interface FourFoldSingleDimensionValid extends QuantifierValidFunctions{};
				interface SingleDimensionTwoDimensionsValid extends QuantifierValidFunctions{};
				interface FourFoldSingleDimensionTwoDimensionsValid extends QuantifierValidFunctions{};

				interface FourFoldValue extends QuantifierValueFunctions{};
				interface TwoDimensionsValue extends QuantifierValueFunctions{};
				interface SingleDimensionValue extends QuantifierValueFunctions{};
				interface FourFoldTwoDimensionsValue extends QuantifierValueFunctions{};
				interface FourFoldSingleDimensionValue extends QuantifierValueFunctions{};
				interface SingleDimensionTwoDimensionsValue extends QuantifierValueFunctions{};
				interface FourFoldSingleDimensionTwoDimensionsValue extends QuantifierValueFunctions{};

				interface FourFoldSignificantValue extends QuantifierSignificantValueFunctions{};
				interface TwoDimensionsSignificantValue extends QuantifierSignificantValueFunctions{};
				interface SingleDimensionSignificantValue extends QuantifierSignificantValueFunctions{};
				interface FourFoldTwoDimensionsSignificantValue extends QuantifierSignificantValueFunctions{};
				interface FourFoldSingleDimensionSignificantValue extends QuantifierSignificantValueFunctions{};
				interface SingleDimensionTwoDimensionsSignificantValue extends QuantifierSignificantValueFunctions{};
				interface FourFoldSingleDimensionTwoDimensionsSignificantValue extends QuantifierSignificantValueFunctions{};

				/* 4ft and SD4ft
::Ferda::Guha::Math::Quantifiers::FourFoldValid
::Ferda::Guha::Math::Quantifiers::FourFoldTwoDimensionsValid
::Ferda::Guha::Math::Quantifiers::FourFoldSingleDimensionValid
::Ferda::Guha::Math::Quantifiers::FourFoldSingleDimensionTwoDimensionsValid
::Ferda::Guha::Math::Quantifiers::FourFoldValue
::Ferda::Guha::Math::Quantifiers::FourFoldTwoDimensionsValue
::Ferda::Guha::Math::Quantifiers::FourFoldSingleDimensionValue
::Ferda::Guha::Math::Quantifiers::FourFoldSingleDimensionTwoDimensionsValue
::Ferda::Guha::Math::Quantifiers::FourFoldSignificantValue
::Ferda::Guha::Math::Quantifiers::FourFoldTwoDimensionsSignificantValue
::Ferda::Guha::Math::Quantifiers::FourFoldSingleDimensionSignificantValue
::Ferda::Guha::Math::Quantifiers::FourFoldSingleDimensionTwoDimensionsSignificantValue
				*/

				/* CF and SDCF
::Ferda::Guha::Math::Quantifiers::SingleDimensionValid
::Ferda::Guha::Math::Quantifiers::FourFoldSingleDimensionValid
::Ferda::Guha::Math::Quantifiers::SingleDimensionTwoDimensionsValid
::Ferda::Guha::Math::Quantifiers::FourFoldSingleDimensionTwoDimensionsValid
::Ferda::Guha::Math::Quantifiers::SingleDimensionValue
::Ferda::Guha::Math::Quantifiers::FourFoldSingleDimensionValue
::Ferda::Guha::Math::Quantifiers::SingleDimensionTwoDimensionsValue
::Ferda::Guha::Math::Quantifiers::FourFoldSingleDimensionTwoDimensionsValue
::Ferda::Guha::Math::Quantifiers::SingleDimensionSignificantValue
::Ferda::Guha::Math::Quantifiers::FourFoldSingleDimensionSignificantValue
::Ferda::Guha::Math::Quantifiers::SingleDimensionTwoDimensionsSignificantValue
::Ferda::Guha::Math::Quantifiers::FourFoldSingleDimensionTwoDimensionsSignificantValue
				*/

				/* KL and SDKL
::Ferda::Guha::Math::Quantifiers::TwoDimensionsValid
::Ferda::Guha::Math::Quantifiers::FourFoldTwoDimensionsValid
::Ferda::Guha::Math::Quantifiers::SingleDimensionTwoDimensionsValid
::Ferda::Guha::Math::Quantifiers::FourFoldSingleDimensionTwoDimensionsValid
::Ferda::Guha::Math::Quantifiers::TwoDimensionsValue
::Ferda::Guha::Math::Quantifiers::FourFoldTwoDimensionsValue
::Ferda::Guha::Math::Quantifiers::SingleDimensionTwoDimensionsValue
::Ferda::Guha::Math::Quantifiers::FourFoldSingleDimensionTwoDimensionsValue
::Ferda::Guha::Math::Quantifiers::TwoDimensionsSignificantValue
::Ferda::Guha::Math::Quantifiers::FourFoldTwoDimensionsSignificantValue
::Ferda::Guha::Math::Quantifiers::SingleDimensionTwoDimensionsSignificantValue
::Ferda::Guha::Math::Quantifiers::FourFoldSingleDimensionTwoDimensionsSignificantValue
				*/
			};
		};
	};
};

#endif
