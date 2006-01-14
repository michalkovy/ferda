#ifndef	FERDA_MODULES_BOXES_DataMiningCommon_Attributes_AbstractAttribute
#define FERDA_MODULES_BOXES_DataMiningCommon_Attributes_AbstractAttribute

#include <Modules/BuiltinSequences.ice>
#include <Modules/Modules.ice>
#include <Modules/Common.ice>
#include <Modules/Boxes/DataMiningCommon/Column/Column.ice>

module Ferda {
	module Modules {
		module Boxes {
			module DataMiningCommon {
				module Attributes {

					struct AbstractAttributeStruct {
						Ferda::Modules::Boxes::DataMiningCommon::Column::ColumnStruct column;
						int identifier; //boxModule.Manager.getProjectInformation().getProjectIdentifier(Ice.Util.identityToString(this.boxModule.MyIdentity));
						string nameInLiterals;
						long countOfCategories;
						string xCategory;
						string includeNullCategory;
						CategoriesStruct categories;
					};
					sequence<AbstractAttributeStruct> AbstractAttributeStructSeq;

					interface AbstractAttributeFunctions {
						nonmutating AbstractAttributeStruct getAbstractAttribute()
							throws
								Ferda::Modules::BoxRuntimeError,
								Ferda::Modules::BadValueError,
								Ferda::Modules::BadParamsError,
								Ferda::Modules::NoConnectionInSocketError;
					};
				};
			};
		};
	};
};
#endif;