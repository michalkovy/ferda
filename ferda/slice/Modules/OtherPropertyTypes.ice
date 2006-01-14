#ifndef FERDA_MODULES_OTHER_PROPERTY_TYPES
#define FERDA_MODULES_OTHER_PROPERTY_TYPES

#include <Modules/BasicPropertyTypes.ice>
#include <Modules/BuiltinSequences.ice>
#include <Modules/Common.ice>



module Ferda {
	module Modules {

		interface StringSeqTInterface {
			nonmutating StringSeq getStringSeq();
		};

		class StringSeqT extends PropertyValue implements StringSeqTInterface {
			StringSeq stringSeqValue;
		};

		interface CategoriesTInterface {
			CategoriesStruct getCategories();
		};

		class CategoriesT extends PropertyValue implements CategoriesTInterface {
			CategoriesStruct categoriesValue;
		};

		interface GenerationInfoTInterface {
			GeneratingStruct getGenerationInfo();
		};

		class GenerationInfoT extends PropertyValue implements GenerationInfoTInterface {
			GeneratingStruct generationInfoValue;
		};

		interface HypothesesTInterface {
			HypothesisStructSeq getHypothesesValue();
		};

		class HypothesesT extends PropertyValue implements HypothesesTInterface {
			HypothesisStructSeq hypothesesValue;
		};
	};
};
#endif


/*
How to add you oun new PropertyValue datatype?
1 - create the slice design ... see above
2 - implement "src/PropertyTypes/?YourTypeName?TI.cs" (see implementation of other types)
    extend "XmlInclude" attribute in "src/PropertyTypes/ValueT.cs"
    extend in "src/PropertyTypes/ObjectFactoryForPropertyTypes.cs" functions "public Ice.Object create(string type)" and "public static void addFactoryToCommunicator(Ice.Communicator communicator, ObjectFactoryForPropertyTypes factory)"
3 - extend in "src/Modules/Boxex/BoxInfo.cs" function "public PropertyValue GetPropertyDefaultValue(string propertyName)"
How to use your newly created PropertyValue datatype in sockets (properties)? (Described if you are using BoxInfo.cs implementation of IBoxInfo.cs interface)
4 - edit your BoxConfig.xml (XML config file) i.e. add your newly created datatype to your socket (property)
    (XPath): /Box/Sockets/Socket[Name="?NameOfYourSocket?"]/SocketTypes/BoxType/FunctionIceId
    (XPath): /Box/Properties/Property[Name="?NameOfYourSocket?"]/TypeClassIceId
5 - implement (extend) function "public virtual PropertyValue GetPropertyObjectFromInterface(string propertyName, Ice.ObjectPrx objectPrx)" in your box module`s ?BoxName?BoxInfo.cs class
*/