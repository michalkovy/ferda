using System;
using System.Collections.Generic;
using System.Text;
using Ferda.Modules.Boxes;
using Ferda.Modules;
using Ferda.Modules.Boxes.DataMiningCommon.Column;

namespace Ferda.Modules.MetabaseLayer
{
	public class Constants
	{
		public Constants()
		{
			cedentEnumDictionary = new Dictionary<CedentEnum, int>();
			cedentEnumDictionary.Add(CedentEnum.Antecedent, 2);
			cedentEnumDictionary.Add(CedentEnum.Succedent, 3);
			cedentEnumDictionary.Add(CedentEnum.Condition, 4);
			cedentEnumDictionary.Add(CedentEnum.FirstSet, 5);
			cedentEnumDictionary.Add(CedentEnum.SecondSet, 6);
			cedentEnumDictionaryBackward = new Dictionary<int, CedentEnum>();
			cedentEnumDictionaryBackward.Add(2, CedentEnum.Antecedent);
			cedentEnumDictionaryBackward.Add(3, CedentEnum.Succedent);
			cedentEnumDictionaryBackward.Add(4, CedentEnum.Condition);
			cedentEnumDictionaryBackward.Add(5, CedentEnum.FirstSet);
			cedentEnumDictionaryBackward.Add(6, CedentEnum.SecondSet);

			literalTypeEnumDictionary = new Dictionary<LiteralTypeEnum, int>();
			literalTypeEnumDictionary.Add(LiteralTypeEnum.Basic, 1);
			literalTypeEnumDictionary.Add(LiteralTypeEnum.Remaining, 2);

			gaceTypeEnumDictionary = new Dictionary<GaceTypeEnum, int>();
			gaceTypeEnumDictionary.Add(GaceTypeEnum.Both, 3);
			gaceTypeEnumDictionary.Add(GaceTypeEnum.Negative, 2);
			gaceTypeEnumDictionary.Add(GaceTypeEnum.Positive, 1);

			coefficientTypeEnumDictionary = new Dictionary<CoefficientTypeEnum, int>();
			coefficientTypeEnumDictionary.Add(CoefficientTypeEnum.Cut, 4);
			coefficientTypeEnumDictionary.Add(CoefficientTypeEnum.CyclicalInterval, 10);
			coefficientTypeEnumDictionary.Add(CoefficientTypeEnum.Interval, 3);
			coefficientTypeEnumDictionary.Add(CoefficientTypeEnum.LeftCut, 5);
			coefficientTypeEnumDictionary.Add(CoefficientTypeEnum.OneParticularCategory, 2);
			coefficientTypeEnumDictionary.Add(CoefficientTypeEnum.RightCut, 6);
			coefficientTypeEnumDictionary.Add(CoefficientTypeEnum.Subset, 1);

			categoryTypeEnumDictionary = new Dictionary<CategoryTypeEnum, int>();
			categoryTypeEnumDictionary.Add(CategoryTypeEnum.Enumeration, 1);
			categoryTypeEnumDictionary.Add(CategoryTypeEnum.Interval, 2);

			boundaryEnumDictionary = new Dictionary<BoundaryEnum, int>();
			//boundaryEnumDictionary.Add(BoundaryEnum.Infinity, -1);
			boundaryEnumDictionary.Add(BoundaryEnum.Round, 2);
			boundaryEnumDictionary.Add(BoundaryEnum.Sharp, 1);

			columnSubTypeEnumDictionary = new Dictionary<ColumnTypeEnum, int>();
			columnSubTypeEnumDictionary.Add(ColumnTypeEnum.Derived, 2);
			columnSubTypeEnumDictionary.Add(ColumnTypeEnum.MultiColumnField, 3);
			columnSubTypeEnumDictionary.Add(ColumnTypeEnum.Ordinary, 1);

			valueSubTypeEnumDictionary = new Dictionary<ValueSubTypeEnum, int>();
			valueSubTypeEnumDictionary.Add(ValueSubTypeEnum.BooleanType, 4);
			valueSubTypeEnumDictionary.Add(ValueSubTypeEnum.DateTimeType, 5);
			//valueSubTypeEnumDictionary.Add(ValueSubTypeEnum.DateType, 5);
			valueSubTypeEnumDictionary.Add(ValueSubTypeEnum.DecimalType, 2);
			valueSubTypeEnumDictionary.Add(ValueSubTypeEnum.DoubleType, 2);
			valueSubTypeEnumDictionary.Add(ValueSubTypeEnum.FloatType, 2);
			valueSubTypeEnumDictionary.Add(ValueSubTypeEnum.IntegerType, 1);
			valueSubTypeEnumDictionary.Add(ValueSubTypeEnum.LongIntegerType, 1);
			valueSubTypeEnumDictionary.Add(ValueSubTypeEnum.ShortIntegerType, 1);
			valueSubTypeEnumDictionary.Add(ValueSubTypeEnum.StringType, 3);
			//valueSubTypeEnumDictionary.Add(ValueSubTypeEnum.TimeType, 5);
			//valueSubTypeEnumDictionary.Add(ValueSubTypeEnum.Unknown, );
			valueSubTypeEnumDictionary.Add(ValueSubTypeEnum.UnsignedIntegerType, 1);
			valueSubTypeEnumDictionary.Add(ValueSubTypeEnum.UnsignedLongIntegerType, 1);
			valueSubTypeEnumDictionary.Add(ValueSubTypeEnum.UnsignedShortIntegerType, 1);

			infinityTypeEnumDictionary = new Dictionary<InfinityTypeEnum, int>();
			infinityTypeEnumDictionary.Add(InfinityTypeEnum.MinusInfinity, 3);
			infinityTypeEnumDictionary.Add(InfinityTypeEnum.None, 1);
			infinityTypeEnumDictionary.Add(InfinityTypeEnum.PlusInfinity, 2);

			taskTypeEnumDictionary = new Dictionary<TaskTypeEnum, int>();
			taskTypeEnumDictionary.Add(TaskTypeEnum.FFT, 1);
			taskTypeEnumDictionary.Add(TaskTypeEnum.SDFFT, 9);
			taskTypeEnumDictionary.Add(TaskTypeEnum.CF, 7);
			taskTypeEnumDictionary.Add(TaskTypeEnum.SDCF, 10);
			taskTypeEnumDictionary.Add(TaskTypeEnum.KL, 6);
			taskTypeEnumDictionary.Add(TaskTypeEnum.SDKL, 8);

			missingTypeEnumDictionary = new Dictionary<MissigTypeEnum, int>();
			missingTypeEnumDictionary.Add(MissigTypeEnum.Deleting, 1);
			missingTypeEnumDictionary.Add(MissigTypeEnum.OptimisticFillUp, 3);
			missingTypeEnumDictionary.Add(MissigTypeEnum.PesimisticFillUp, 2);

			relationEnumDictionary = new Dictionary<RelationEnum, int>();
			relationEnumDictionary.Add(RelationEnum.Equal, 1);
			relationEnumDictionary.Add(RelationEnum.GreaterThan, 4);
			relationEnumDictionary.Add(RelationEnum.GreaterThanOrEqual, 5);
			relationEnumDictionary.Add(RelationEnum.LessThan, 2);
			relationEnumDictionary.Add(RelationEnum.LessThanOrEqual, 3);

			unitsEnumDictionary = new Dictionary<UnitsEnum, int>();
			unitsEnumDictionary.Add(UnitsEnum.AbsoluteNumber, 4);
			unitsEnumDictionary.Add(UnitsEnum.RelativeToActCondition, 1);
			unitsEnumDictionary.Add(UnitsEnum.RelativeToAllObjects, 2);
			unitsEnumDictionary.Add(UnitsEnum.RelativeToMaxFrequency, 3);

			operationModeEnumDictionary = new Dictionary<OperationModeEnum, int>();
			operationModeEnumDictionary.Add(OperationModeEnum.AbsoluteDifferenceOfQuantifierValues, 6);
			operationModeEnumDictionary.Add(OperationModeEnum.DifferenceOfQuantifierValues, 5);
			operationModeEnumDictionary.Add(OperationModeEnum.DifferencesOfAbsoluteFrequencies, 3);
			operationModeEnumDictionary.Add(OperationModeEnum.DifferencesOfRelativeFrequencies, 4);
			operationModeEnumDictionary.Add(OperationModeEnum.FirstSetFrequencies, 1);
			operationModeEnumDictionary.Add(OperationModeEnum.SecondSetFrequencies, 2);
		}

		private Dictionary<OperationModeEnum, int> operationModeEnumDictionary;
		public Dictionary<OperationModeEnum, int> OperationModeEnumDictionary
		{
			get { return operationModeEnumDictionary; }
		}

		private Dictionary<UnitsEnum, int> unitsEnumDictionary;
		public Dictionary<UnitsEnum, int> UnitsEnumDictionary
		{
			get { return unitsEnumDictionary; }
		}

		private Dictionary<RelationEnum, int> relationEnumDictionary;
		public Dictionary<RelationEnum, int> RelationEnumDictionary
		{
			get { return relationEnumDictionary; }
		}

		public int SdJoinedSet(WorkingWithCedentsSecondSetEnum set)
		{
			if (set == WorkingWithCedentsSecondSetEnum.Cedent1AndCedent2)
				return this.TrueValue;
			else
				return this.FalseValue;
		}

		public string DateTimeStructToString(DateTimeStruct dateTimeStruct)
		{
            return new DateTime(dateTimeStruct.year, dateTimeStruct.month, dateTimeStruct.day, dateTimeStruct.hour, dateTimeStruct.minute, dateTimeStruct.second).ToString("yy-MM-dd T");
		}
		public string DateTimeToString(DateTime dateTime)
		{
			return dateTime.ToString();
		}

		public int GetBool(bool value)
		{
			if (value)
				return TrueValue;
			else
				return FalseValue;
		}

		public int FalseValue
		{
			get { return 0; }
		}

		public int TrueValue
		{
			get { return -1; }
		}

		public int UserID
		{
			get { return 1; }
		}

		public int wSavedCountUsed
		{
			get { return 100; }
		}

		public int wUpdateVer
		{
			get { return 0; }
		}

		private Dictionary<MissigTypeEnum, int> missingTypeEnumDictionary;
		public Dictionary<MissigTypeEnum, int> MissingTypeEnumDictionary
		{
			get { return missingTypeEnumDictionary; }
		}

		private Dictionary<TaskTypeEnum, int> taskTypeEnumDictionary;
		public Dictionary<TaskTypeEnum, int> TaskTypeEnumDictionary
		{
			get { return taskTypeEnumDictionary; }
		}

		private Dictionary<InfinityTypeEnum, int> infinityTypeEnumDictionary;
		public Dictionary<InfinityTypeEnum, int> InfinityTypeEnumDictionary
		{
			get { return infinityTypeEnumDictionary; }
		}

		private Dictionary<ValueSubTypeEnum, int> valueSubTypeEnumDictionary;
		public Dictionary<ValueSubTypeEnum, int> ValueSubTypeEnumDictionary
		{
			get { return valueSubTypeEnumDictionary; }
		}

		private Dictionary<ColumnTypeEnum, int> columnSubTypeEnumDictionary;
		public Dictionary<ColumnTypeEnum, int> ColumnSubTypeEnumDictionary
		{
			get { return columnSubTypeEnumDictionary; }
		}

		private Dictionary<BoundaryEnum, int> boundaryEnumDictionary;
		public Dictionary<BoundaryEnum, int> BoundaryEnumDictionary
		{
			get { return boundaryEnumDictionary; }
		}

		private Dictionary<CategoryTypeEnum, int> categoryTypeEnumDictionary;
		public Dictionary<CategoryTypeEnum, int> CategoryTypeEnumDictionary
		{
			get { return categoryTypeEnumDictionary; }
		}
			
		private Dictionary<CoefficientTypeEnum, int> coefficientTypeEnumDictionary;
		public Dictionary<CoefficientTypeEnum, int> CoefficientTypeEnumDictionary
		{
			get { return coefficientTypeEnumDictionary; }
		}

		private Dictionary<GaceTypeEnum, int> gaceTypeEnumDictionary;
		public Dictionary<GaceTypeEnum, int> GaceTypeEnumDictionary
		{
			get { return gaceTypeEnumDictionary; }
		}

		private Dictionary<LiteralTypeEnum, int> literalTypeEnumDictionary;
		public Dictionary<LiteralTypeEnum, int> LiteralTypeEnumDictionary
		{
			get { return literalTypeEnumDictionary; }
		}

		private Dictionary<CedentEnum, int> cedentEnumDictionary;
		public Dictionary<CedentEnum, int> CedentEnumDictionary
		{
			get { return cedentEnumDictionary; }
		}
		private Dictionary<int, CedentEnum> cedentEnumDictionaryBackward;
		public Dictionary<int, CedentEnum> CedentEnumDictionaryBackward
		{
			get { return cedentEnumDictionaryBackward; }
		}
	}
}
