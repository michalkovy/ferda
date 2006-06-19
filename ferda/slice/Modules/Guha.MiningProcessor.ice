#ifndef FERDA_GUHA_MININGPROCESSOR
#define FERDA_GUHA_MININGPROCESSOR

#include <Modules/BuiltinSequences.ice>
#include <Modules/BasicPropertyTypes.ice>
#include <Modules/Exceptions.ice>
#include <Modules/Guha.Data.ice>
#include <ModulesManager/ManagersEngine.ice>

module Ferda {
	module Guha {
		module MiningProcessor {
				
				struct GuidStruct
				{
					string value;
				};
				
				// BIT STRING GENERATOR
				
				class BitString
				{
					Ferda::Modules::IntSeq value; //TODO mozna blbe
					int lenght;
				};
				
				struct GuidAttributeNamePair
				{
					GuidStruct id;
					string attributeName;								
				};
				sequence<GuidAttributeNamePair> GuidAttributeNamePairSeq;
				
				interface AttributeNameProvider
				{
					GuidAttributeNamePairSeq GetAttributeNames()
						throws Ferda::Modules::BoxRuntimeError;
				};
				
				sequence<GuidStruct> GuidStructSeq;
				sequence<GuidStructSeq> GuidStructSeqSeq;
				
				interface BitStringGenerator extends AttributeNameProvider
				{
					GuidStruct GetAttributeId()
						throws Ferda::Modules::BoxRuntimeError;
						
					Ferda::Guha::Data::CardinalityEnum GetAttributeCardinality()
						throws Ferda::Modules::BoxRuntimeError;
					
					// vrati id kategorie, krome missing inforamtion kategorie
					// jmena kategorii musi byt setridene (pro attribute cardianlity ">" nominal)
					Ferda::Modules::StringSeq GetCategoriesIds()
						throws Ferda::Modules::BoxRuntimeError;
					
					Ferda::Modules::DoubleSeq GetCategoriesNumericValues()
						throws Ferda::Modules::BoxRuntimeError;
					
					BitString GetBitString(string categoryId)
						throws Ferda::Modules::BoxRuntimeError;
						
					string GetMissingInformationCategoryId()
						throws Ferda::Modules::BoxRuntimeError;
				};
				
				// BOOLEAN CEDEN SETTING ENTITIES
			
				enum ImportanceEnum
				{
					Forced,
					Basic,
					Auxiliary
				};			
			
				class IEntitySetting
				{
					GuidStruct id;
					ImportanceEnum importance;
				};
				sequence<IEntitySetting> IEntitySettingSeq;
				
				// Leaf Entities
				
				class ILeafEntitySetting extends IEntitySetting
				{
					BitStringGenerator* generator;
				};

				class CoefficientFixedSetSetting extends ILeafEntitySetting
				{
					Ferda::Modules::StringSeq categoriesIds;
				};
				
				enum CoefficientTypeEnum
				{
					Subsets,
					CyclicIntervals,
					Intervals,
					Cuts,
					LeftCuts,
					RightCuts
				};

				class CoefficientSetting extends ILeafEntitySetting
				{
					int minLenght;
					int maxLenght;
					CoefficientTypeEnum coefficientType;
				};
				
				// Single Operand Entities

				class ISingleOperandEntitySetting extends IEntitySetting
				{
					IEntitySetting operand;
				};
				
				enum SignTypeEnum
				{
					Positive,
					Negative,
					Both,
				};
				
				class NegationSetting extends ISingleOperandEntitySetting
				{};
				
				class BothSignsSetting extends ISingleOperandEntitySetting
				{};
				
				// Multiple Operand Entities

				class IMultipleOperandEntitySetting extends IEntitySetting
				{
					IEntitySettingSeq operands;
					GuidStructSeqSeq classesOfEquivalence;
					int minLenght;
					int maxLenght;
				};
				
				class ConjunctionSetting extends IMultipleOperandEntitySetting
				{};
				
				class DisjunctionSetting extends IMultipleOperandEntitySetting
				{};
				
				// SETTING BOX MODULES
				
				interface BooleanAttributeSettingFunctions extends AttributeNameProvider
				{
					nonmutating IEntitySetting GetEntitySetting()
						throws Ferda::Modules::BoxRuntimeError;
				};
				
				interface EquivalenceClassFunctions
				{
					nonmutating GuidStructSeq GetEquivalenceClass()
						throws Ferda::Modules::BoxRuntimeError;
				};
				// MINING PROCESSOR SERVICE FUNCTIONS
				
				enum TaskTypeEnum
				{
					FourFold,
					KL,
					CF,
					SDFourFold,
					SDKL,
					SDCF					
				};
				
				interface MiningProcessorFunctions
				{
					void SetUp(
						IEntitySettingSeq booleanCedents, 
						TaskTypeEnum taskType,
						Ferda::ModulesManager::ProgressBar* progressBar,
						Ferda::ModulesManager::Output* output
						)
						throws Ferda::Modules::BoxRuntimeError;
						
					void Start()
						throws Ferda::Modules::BoxRuntimeError;
						
					void Stop()
						throws Ferda::Modules::BoxRuntimeError;
						
					//ProgressTask
					
					void GetResult() //TODO
						throws Ferda::Modules::BoxRuntimeError;
				};			
		};
	};
};

#endif
