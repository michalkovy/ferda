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
		interface StringTInterface {
			nonmutating string getStringValue();
		};
		
		//::Ferda::Modules::StringT
		class StringT extends PropertyValue implements StringTInterface {
			string stringValue;
		};
		
		interface DoubleTInterface extends StringTInterface {
			nonmutating double getDoubleValue();
		};

		class DoubleT extends PropertyValue implements DoubleTInterface {
			double doubleValue;
		};
		
		interface FloatTInterface extends DoubleTInterface {
			nonmutating float getFloatValue();
		};

		class FloatT extends PropertyValue implements FloatTInterface {
			float floatValue;
		};
		
		interface LongTInterface extends FloatTInterface {
			nonmutating long getLongValue();
		};

		class LongT extends PropertyValue implements LongTInterface {
			long longValue;
		};
		
		interface IntTInterface extends LongTInterface {
			nonmutating int getIntValue();
		};

		class IntT extends PropertyValue implements IntTInterface {
			int intValue;
		};
		
		interface ShortTInterface extends IntTInterface {
			nonmutating short getShortValue();
		};

		class ShortT extends PropertyValue implements ShortTInterface {
			short shortValue;
		};
		
		interface BoolTInterface extends ShortTInterface {
			nonmutating bool getBoolValue();
		};

		class BoolT extends PropertyValue implements BoolTInterface {
			bool boolValue;
		};
		
		interface DateTimeTInterface extends StringTInterface {
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

		interface DateTInterface extends DateTimeTInterface {
			nonmutating void getDateValue(out int year, out short month, out short day);
		};
		class DateT extends PropertyValue implements DateTInterface {
			short day;
			short month;
			int year;
		};
		
		interface TimeTInterface extends StringTInterface {
			nonmutating void getTimeValue(out short hour, out short minute, out short second);
		};
		class TimeT extends PropertyValue implements TimeTInterface {
			short hour;
			short minute;
			short second;
		};

		/* SelectT a SelectOptT není třeba, protože se to dá zjistit na základě
		 * patřičných funkcí/hondot */
	};
};

#endif
