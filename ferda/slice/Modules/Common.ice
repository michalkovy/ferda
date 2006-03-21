#ifndef FERDA_MODULES_BOXES_COMMON
#define FERDA_MODULES_BOXES_COMMON

#include <Modules/BuiltinSequences.ice>
#include <Modules/BasicPropertyTypes.ice>
#include <Modules/Modules.ice>

module Ferda {
	module Modules {

		enum CardinalityEnum
		{
			Nominal,
			Ordinal,
			Cardinal
		};

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
		enum ValueSubTypeEnum
		{
				Unknown,
		    BooleanType,
		    //DateType,
		    DateTimeType,
		    TimeType,
		    ShortIntegerType,
		    IntegerType,
		    LongIntegerType,
		    UnsignedShortIntegerType,
		    UnsignedIntegerType,
		    UnsignedLongIntegerType,
		    FloatType,
		    DoubleType,
		    DecimalType,
		    StringType
		};
		enum CoefficientTypeEnum
		{
		    Subset,
		    OneParticularCategory,
		    Interval,
		    CyclicalInterval,
		    Cut,
		    LeftCut,
		    RightCut
		};
		enum LiteralTypeEnum
		{
		    Basic,
		    Remaining
		};
		enum GaceTypeEnum
		{
		    Positive,
		    Negative,
		    Both
		};
		enum MissigTypeEnum
		{
		    Deleting,
		    PesimisticFillUp,
		    OptimisticFillUp
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
		enum OperationModeEnum
		{
		    FirstSetFrequencies,
		    SecondSetFrequencies,
		    DifferencesOfAbsoluteFrequencies,
		    DifferencesOfRelativeFrequencies,
		    DifferenceOfQuantifierValues,
		    AbsoluteDifferenceOfQuantifierValues
		};
		enum DirectionEnum
		{
			RowsOnColumns,
			ColumnsOnRows
		};
		enum RelationEnum
		{
		    Equal,
		    LessThan,
		    LessThanOrEqual,
		    GreaterThan,
		    GreaterThanOrEqual
		};
		enum UnitsEnum
		{
		    AbsoluteNumber,
				RelativeToActCondition,
		    RelativeToAllObjects,
		    RelativeToMaxFrequency
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


				struct AbstractQuantifierSetting
				{
					IntSeqSeq firstContingencyTableRows;
					IntSeqSeq secondContingencyTableRows;
					DoubleSeq numericValues; // for CF&SDCF quantifeirs over numeric values of the attribute
					long allObjectsCount;
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
					AbstractQuantifierSetting quantifierSetting;
					BooleanLiteralStructSeq booleanLiterals;
					LiteralStructSeq literals;
				};
				sequence<HypothesisStruct> HypothesisStructSeq;

				struct GeneratingStruct {
					GenerationStateEnum generationState;
					long generationNrOfTests;
					long generationNrOfHypotheses;
					DateTimeT generationStartTime;
					TimeT generationTotalTime;
				};
	};
};

#endif
