#ifndef FERDA_MODULES_BOXES_COMMON
#define FERDA_MODULES_BOXES_COMMON

#include <Modules/BuiltinSequences.ice>
#include <Modules/BasicPropertyTypes.ice>
#include <Modules/Modules.ice>

module Ferda {
	module Modules {

		enum AttributeDomainEnum
		{
			WholeDomain,
			SubDomainValueBounds,
			SubDomainNumberOfValuesBounds
		};

		enum BoundaryEnum
		{
			Sharp,
			Round,
			Infinity
		};

		struct DateStruct
		{
			int year;
			short month;
			short day;
		};
		struct TimeStruct
		{
			short hour;
			short minute;
			short second;
		};
		struct DateTimeStruct
		{
			int year;
			short month;
			short day;
			short hour;
			short minute;
			short second;
		};

		struct DateTimeIntervalStruct
		{
			BoundaryEnum leftBoundType;
			BoundaryEnum rightBoundType;
			DateTimeStruct leftBound;
			DateTimeStruct rightBound;
		};
		sequence<DateTimeIntervalStruct> DateTimeIntervalStructSeq;
		dictionary<string, DateTimeIntervalStructSeq> DateTimeIntervalCategorySeq;

		struct LongIntervalStruct
		{
			BoundaryEnum leftBoundType;
			BoundaryEnum rightBoundType;
			long leftBound;
			long rightBound;
		};
		sequence<LongIntervalStruct> LongIntervalStructSeq;
		dictionary<string, LongIntervalStructSeq> LongIntervalCategorySeq;

		struct FloatIntervalStruct
		{
			BoundaryEnum leftBoundType;
			BoundaryEnum rightBoundType;
			float leftBound;
			float rightBound;
		};
		sequence<FloatIntervalStruct> FloatIntervalStructSeq;
		dictionary<string, FloatIntervalStructSeq> FloatIntervalCategorySeq;

		dictionary<string, StringSeq> EnumCategorySeq;

		struct CategoriesStruct
		{
			DateTimeIntervalCategorySeq dateTimeIntervals;
			LongIntervalCategorySeq longIntervals;
			FloatIntervalCategorySeq floatIntervals;
			EnumCategorySeq enums;
		};

		enum SidesEnum
		{
			Left,
			Right
		};
		enum RangeEnum
		{
			Half,
			All
		};
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
