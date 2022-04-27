#ifndef FERDA_MODULES_BASIC_PROPERTY_TYPES
#define FERDA_MODULES_BASIC_PROPERTY_TYPES

module Ferda {
	module Modules {
		class PropertyValue {
		};

		sequence<PropertyValue> PropertyValueSeq;

		enum PropertyValueTypesEnum {
			BoolPropertyValueType,
			ShortPropertyValueType,
			IntPropertyValueType,
			LongPropertyValueType,
			FloatPropertyValueType,
			DoublePropertyValueType,
			StringPropertyValueType,
			DatePropertyValueType,
			DateTimePropertyValueType,
			TimePropertyValueType
		};

		/*  B A S I C  T Y P E S  */
		
		//::Ferda::Modules::StringTInterface*
		["cs:tie"] interface StringTInterface {
			idempotent string getStringValue();
		};
		
		//::Ferda::Modules::StringT
		class StringT extends PropertyValue {
			string stringValue;
		};
		
		["cs:tie"] interface DoubleTInterface extends StringTInterface {
			idempotent double getDoubleValue();
		};

		class DoubleT extends PropertyValue {
			double doubleValue;
		};
		
		["cs:tie"] interface FloatTInterface extends DoubleTInterface {
			idempotent float getFloatValue();
		};

		class FloatT extends PropertyValue {
			float floatValue;
		};
		
		["cs:tie"] interface LongTInterface extends FloatTInterface {
			idempotent long getLongValue();
		};

		class LongT extends PropertyValue {
			long longValue;
		};
		
		["cs:tie"] interface IntTInterface extends LongTInterface {
			idempotent int getIntValue();
		};

		class IntT extends PropertyValue {
			int intValue;
		};
		
		["cs:tie"] interface ShortTInterface extends IntTInterface {
			idempotent short getShortValue();
		};

		class ShortT extends PropertyValue {
			short shortValue;
		};
		
		["cs:tie"] interface BoolTInterface extends ShortTInterface {
			idempotent bool getBoolValue();
		};

		class BoolT extends PropertyValue {
			bool boolValue;
		};
		
		["cs:tie"] interface DateTimeTInterface extends StringTInterface {
			idempotent void getDateTimeValue(out int year, out short month, out short day,
				out short hour, out short minute, out short second);
		};
		class DateTimeT extends PropertyValue {
			short day;
			short month;
			int year;
			short hour;
			short minute;
			short second;
		};

		["cs:tie"] interface DateTInterface extends DateTimeTInterface {
			idempotent void getDateValue(out int year, out short month, out short day);
		};
		class DateT extends PropertyValue {
			short day;
			short month;
			int year;
		};
		
		["cs:tie"] interface TimeTInterface extends StringTInterface {
			idempotent void getTimeValue(out short hour, out short minute, out short second);
		};
		class TimeT extends PropertyValue {
			short hour;
			short minute;
			short second;
		};

		/* SelectT a SelectOptT není třeba, protože se to dá zjistit na základě
		 * patřičných funkcí/hondot */
	};
};

#endif
