#ifndef FERDA_GUHA_MININGPROCESSOR
#define FERDA_GUHA_MININGPROCESSOR

#include <Modules/Modules.ice>
#include <Modules/BuiltinSequences.ice>
#include <Modules/BasicPropertyTypes.ice>
#include <Modules/Exceptions.ice>
#include <Modules/Guha.Data.ice>
#include <Modules/Guha.Math.Quantifiers.ice>
#include <ModulesManager/ManagersEngine.ice>

/*
SLICE DESIGN OF THE FERDA GUHA MINING PROCESSOR

The class design diagram representing dependencies of the designed entities
can be found in "slice/Modules/GUha.MiningProcessor.png(csd)".
The csd file stands for class diagram, that can be edited with the 
NCLass tool, see "http://nclass.sourceforge.net".

Explanation of structures, interfaces and their methods is located in
"src/Modules/Core/MiningProcessor/Design/*"
*/

module Ferda {
	module Guha {
		module Math {
			module Quantifiers {

				//FORWARD DECLARATION

				interface QuantifierBaseFunctions;
				sequence<QuantifierBaseFunctions*> QuantifierBaseFunctionsPrxSeq;

			};
		};
	};
};

module Ferda {
	module Guha {
		module MiningProcessor {

				// BIT STRING GENERATOR

				class BitStringIce
				{
					Ferda::Modules::LongSeq value;
					int length;
				};
				
								
				struct BitStringIceWithCategoryId
				{
					BitStringIce bitString;
					string categoryId;
				};

				struct GuidAttributeNamePair
				{
					Ferda::Modules::GuidStruct id;
					string attributeName;
				};
				sequence<GuidAttributeNamePair> GuidAttributeNamePairSeq;
				
				interface SourceDataTableIdProvider
				{
					string GetSourceDataTableId()
						throws Ferda::Modules::BoxRuntimeError;
				};

				interface AttributeNameProvider extends SourceDataTableIdProvider 
				{
					GuidAttributeNamePairSeq GetAttributeNames()
						throws Ferda::Modules::BoxRuntimeError;
				};

				interface BitStringGenerator extends AttributeNameProvider
				{
					Ferda::Modules::GuidStruct GetAttributeId()
						throws Ferda::Modules::BoxRuntimeError;

					Ferda::Guha::Data::CardinalityEnum GetAttributeCardinality()
						throws Ferda::Modules::BoxRuntimeError;

					// vrati id vsech kategorii, krome missing inforamtion kategorie
					// jmena kategorii musi byt setridene (pro attribute cardianlity ">" nominal)
					Ferda::Modules::StringSeq GetCategoriesIds()
						throws Ferda::Modules::BoxRuntimeError;

					// kdyz atribut neposkytuje numericka data vraci Null resp. double[0]
					Ferda::Modules::DoubleSeq GetCategoriesNumericValues()
						throws Ferda::Modules::BoxRuntimeError;

					BitStringIce GetBitString(string categoryId)
						throws Ferda::Modules::BoxRuntimeError;

					Ferda::Modules::StringOpt GetMissingInformationCategoryId()
						throws Ferda::Modules::BoxRuntimeError;
						
					//---relacni DM informace	
					//vraci countvector
					Ferda::Modules::IntSeq GetCountVector(string masterIdColumn, string masterDataTableName, string detailIdColumn)
						throws Ferda::Modules::BoxRuntimeError;
						
				     //da dalsi bitstring
					 bool GetNextBitString(int skipFirstN, out BitStringIceWithCategoryId bitString)
						throws Ferda::Modules::BoxRuntimeError;
						
					//maximalni # bs, ktere podminer vygeneruje
					long GetMaxBitStringCount()
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
					Ferda::Modules::GuidStruct id;
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
					SubsetsOneOne,
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
					Ferda::Modules::GuidStructSeqSeq classesOfEquivalence;
					int minLength;
					int maxLength;
				};

				class ConjunctionSetting extends IMultipleOperandEntitySetting
				{};

				class DisjunctionSetting extends IMultipleOperandEntitySetting
				{};

				// SETTING BOX MODULES

				interface BitStringGeneratorProvider
				{
					idempotent BitStringGenerator* GetBitStringGenerator(Ferda::Modules::GuidStruct attributeId)
						throws Ferda::Modules::BoxRuntimeError;
				};

				interface BooleanAttributeSettingFunctions extends AttributeNameProvider, BitStringGeneratorProvider
				{
					idempotent IEntitySetting GetEntitySetting()
						throws Ferda::Modules::BoxRuntimeError;
				};
				
				interface BooleanAttributeSettingWithBSGenerationAbilityFunctions extends BooleanAttributeSettingFunctions, BitStringGenerator
				{	
				};

				interface EquivalenceClassFunctions
				{
					idempotent Ferda::Modules::GuidStructSeq GetEquivalenceClass()
						throws Ferda::Modules::BoxRuntimeError;
				};

				// MINING TASK BOX MODULE FUNCTIOS

				interface MiningTaskFunctions extends AttributeNameProvider, BitStringGeneratorProvider
				{
					Ferda::Guha::Math::Quantifiers::QuantifierBaseFunctionsPrxSeq GetQuantifiers()
						throws Ferda::Modules::BoxRuntimeError;
					string GetResult(out string statistics)
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
				
				enum ResultTypeEnum
				{
					Trace,
					TraceBoolean,
					TraceReal
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

				enum TaskEvaluationTypeEnum
				{
					FirstN
					//TopN
				};
				
				enum WorkingWithSecondSetModeEnum
				{
					None,
					Cedent2,
		    	          Cedent1AndCedent2
				};

				struct TaskRunParams
				{
					TaskTypeEnum taskType;
					ResultTypeEnum resultType;
					TaskEvaluationTypeEnum evaluationType;
					long maxSizeOfResult; // N from FirstN or TopN
					int skipFirstN; //skip first N hypotheses for Rel DM purposes
					WorkingWithSecondSetModeEnum sdWorkingWithSecondSetMode; // for SD tasks only
				};

				interface MiningProcessorFunctions
				{
					// returned string is serialized Ferda.Guha.MiningProcessor.SerializableResult
					// returned string in out param is serialized Ferda.Guha.MiningProcessor.SerializableResultInfo

					// pro BooleanAttributesSeq plati, ze tam muze byt zastoupena jenda MarkEnum nejvyse jendou
					// pro CategorialAttributeSeq plati, ze tam muze byt jenda MarkEnum zastoupena vicekrat
					string Run(
						Ferda::Modules::BoxModule* taskBoxModule,
						BooleanAttributeSeq booleanAttributes,
						CategorialAttributeSeq categorialAttributes,
						Ferda::Guha::Math::Quantifiers::QuantifierBaseFunctionsPrxSeq quantifiers,
						TaskRunParams taskParams,
						BitStringGeneratorProvider* bitStringGenerator,
						Ferda::ModulesManager::Output* output,
						Ferda::Modules::GuidStruct attributeId,
						Ferda::Modules::IntSeq countVector,
						out string resultInfo
						)
						throws Ferda::Modules::BoxRuntimeError;
						
					BitStringIceWithCategoryId GetNextBitString()
						throws Ferda::Modules::BoxRuntimeError;

				};
		};
	};
};

#endif
