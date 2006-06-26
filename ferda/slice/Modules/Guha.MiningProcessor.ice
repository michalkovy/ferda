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
					Ferda::Modules::LongSeq value;
					int length;
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
				
				// BOOLEAN ATTRIBUTE SETTING ENTITIES
			
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
					int minLength;
					int maxLength;
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
					int minLength;
					int maxLength;
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
				
				// MINING TASK BOX MODULE FUNCTIOS
				
				interface BitStringGeneratorProvider
				{
					nonmutating BitStringGenerator* GetBitStringGenerator(GuidStruct attributeId)
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
				
				enum MarkEnum
				{
					Antecedent,
					Succedent,
					Condition,
					RowAttribute,
					ColumnAttribute,
					Attribute,
					FirstSet,
					SecondSet					
				};
				
				struct BooleanAttribute
				{
					MarkEnum mark;
					IEntitySetting setting;
				};
				sequence<BooleanAttribute> BooleanAttributeSeq;

				struct CategorialAttribute
				{
					MarkEnum mark;
					BitStringGenerator* setting;
				};
				sequence<CategorialAttribute> CategorialAttributeSeq;
				
				interface MiningProcessorFunctions
				{
					// pro BooleanAttributesSeq plati, ze tam muze byt zastoupena jenda MarkEnum nejvyse jendou
					// pro CategorialAttributeSeq plati, ze tam muze byt jenda MarkEnum zastoupena vicekrat
					void SetUp(
						BooleanAttributeSeq booleanAttributes, 
						CategorialAttributeSeq categorialAttributes,
						TaskTypeEnum taskType,
						Ferda::ModulesManager::ProgressBar* progressBar,
						Ferda::ModulesManager::Output* output,
						BitStringGeneratorProvider* bitStringGenerator
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
