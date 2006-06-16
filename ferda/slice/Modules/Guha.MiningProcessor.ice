#ifndef FERDA_GUHA_MININGPROCESSOR
#define FERDA_GUHA_MININGPROCESSOR

#include <Modules/BuiltinSequences.ice>
#include <Modules/BasicPropertyTypes.ice>
#include <Modules/Exceptions.ice>
#include <Modules/Guha.Data.ice>

module Ferda {
	module Guha {
		module MiningProcessor {
				
				struct GuidStruct
				{
					string value;
				};
				
				class BitString
				{
					Ferda::Modules::IntSeq value; //TODO mozna blbe
					int lenght;
				};
				
				sequence<GuidStruct> GuidStructSeq;
				sequence<GuidStructSeq> GuidStructSeqSeq;
				
				interface BitStringGenerator
				{
					GuidStruct GetAttributeId();
					Ferda::Guha::Data::CardinalityEnum GetAttributeCardinality();
					
					// vrati id kategorie, krome missing inforamtion kategorie
					// jmena kategorii musi byt setridene (pro attribute cardianlity ">" nominal)
					Ferda::Modules::StringSeq GetCategoriesIds();
					
					Ferda::Modules::DoubleSeq GetCategoriesNumericValues();
					BitString GetBitString(string categoryId);
					string GetMissingInformationCategoryId();
				};		
			
				class IEntitySetting
				{
					GuidStruct id;
				};
				
				interface EntintySettingProvider
				{
					IEntitySetting GetEntitySetting();
				};		
				
				enum ImportanceEnum
				{
					Forced,
					Basic,
					Auxiliary
				};
				
				class EntityImportancePair
				{
					IEntitySetting entity;
					ImportanceEnum importance;
				};
				
				sequence<EntityImportancePair> EntityImportancePairSeq;
				
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
				
				class NegationSetting extends ISingleOperandEntitySetting
				{};
				
				class BothSignsSetting extends ISingleOperandEntitySetting
				{};
				
				// Multiple Operand Entities
				
				class IMultipleOperandEntitySetting extends IEntitySetting
				{
					EntityImportancePairSeq operands;
					GuidStructSeqSeq classesOfEquivalence;
					int minLenght;
					int maxLenght;
				};
				
				class ConjunctionSetting extends IMultipleOperandEntitySetting
				{};
				
				class DisjunctionSetting extends IMultipleOperandEntitySetting
				{};
			
		};
	};
};

#endif
