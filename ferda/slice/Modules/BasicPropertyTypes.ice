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
		interface BoolTInterface {
			nonmutating bool getBoolValue();
		};

		class BoolT extends PropertyValue implements BoolTInterface {
			bool boolValue;
		};

		interface ShortTInterface {
			nonmutating short getShortValue();
		};

		class ShortT extends PropertyValue implements ShortTInterface {
			short shortValue;
		};

		interface IntTInterface {
			nonmutating int getIntValue();
		};

		class IntT extends PropertyValue implements IntTInterface {
			int intValue;
		};

		interface LongTInterface {
			nonmutating long getLongValue();
		};

		class LongT extends PropertyValue implements LongTInterface {
			long longValue;
		};

		interface FloatTInterface {
			nonmutating float getFloatValue();
		};

		class FloatT extends PropertyValue implements FloatTInterface {
			float floatValue;
		};

		interface DoubleTInterface {
			nonmutating double getDoubleValue();
		};

		class DoubleT extends PropertyValue implements DoubleTInterface {
			double doubleValue;
		};
		//::Ferda::Modules::StringTInterface*
		interface StringTInterface {
			nonmutating string getStringValue();
		};
		//::Ferda::Modules::StringT
		class StringT extends PropertyValue implements StringTInterface {
			string stringValue;
		};
		interface DateTInterface {
			nonmutating void getDateValue(out int year, out short month, out short day);
		};
		class DateT extends PropertyValue implements DateTInterface {
			short day;
			short month;
			int year;
		};
		interface TimeTInterface {
			nonmutating void getTimeValue(out short hour, out short minute, out short second);
		};
		class TimeT extends PropertyValue implements TimeTInterface {
			short hour;
			short minute;
			short second;
		};
		interface DateTimeTInterface {
			nonmutating void getDateTimeValue(out int year, out short month, out short day,
				out short hour, out short minute, out short second);
		};
		class DateTimeT extends PropertyValue implements DateTimeTInterface {
			short day;
			short month;
			int year;
			short hour;
			short minute;
			short second;
		};

		/* SelectT a SelectOptT není třeba, protože se to dá zjistit na základě
		 * patřičných funkcí/hondot */
	};
};

#endif
