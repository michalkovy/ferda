#ifndef FERDA_GUHA_ATTRIBUTE
#define FERDA_GUHA_ATTRIBUTE

#include <Modules/BuiltinSequences.ice>
#include <Modules/BasicPropertyTypes.ice>
#include <Modules/Exceptions.ice>

module Ferda {
	module Guha {
		module Attribute {
			enum BoundaryEnum {
				Closed,
				Open,
				Infinity
			};

			struct IntervalStruct {
				::Ferda::Modules::PropertyValue leftValue;
				::Ferda::Modules::PropertyValue rightValue;
				BoundaryEnum leftBoundary;
				BoundaryEnum rightBoundary;
			};

			sequence<IntervalStruct> IntervalStructSeq;

			// kategorie muze obsahovat jak intervaly, tak enumeraci
			struct CategoryStruct {
				string name; // (id) must be unique within one attribute
				IntervalStructSeq intervals;
				::Ferda::Modules::PropertyValueSeq enumeration;
				bool containsNull;
			};
			sequence<CategoryStruct> CategoryStructSeq;

			//dictionary<string, CategoryStruct> CategorySeq;
			//key is name of category
			//dictionary is not so good representation because it can not be serialized by "System.Xml.Serialization.XmlSerializer"

			struct AttributeStruct{
				//?metrika
				//?usporadani
				CategoryStructSeq categories;
			};
		};

	};
};

#endif