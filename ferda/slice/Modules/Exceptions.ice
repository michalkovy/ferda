#ifndef FERDA_MODULES_EXCEPTIONS
#define FERDA_MODULES_EXCEPTIONS

#include <Modules/BuiltinSequences.ice>

module Ferda {
	module Modules {

		exception ReadOnlyError{};

		exception ConnectionExistsError{};

		exception BadTypeError{};

		exception ConnectionNotExistError{};

		exception NameNotExistError{};

		exception NeedConnectedSocketError {
			string socketName;
			string boxIdentity;
		};

		exception BoxRuntimeError {
			string boxIdentity;
			string userMessage;
		};

		exception NoConnectionInSocketError extends BoxRuntimeError {
			StringSeq socketsNames;
		};

		exception IsNotConvertibleError {
			string reason;
		};

		enum restrictionTypeEnum {
			Other, // unspecified reason
			Minimum, // the specified value is too less (or equal) i.e. value is out of the accepted values
			Maximum, // the specified value is too big (or equal) i.e. value is out of the accepted values
			Regexp, // the specified value doesn`t satisfy the regular expression restriction
			NotInSelectOptions, // the specified value is not in the select options set
			Missing, // e.g. the missing argument or null argument
			BadFormat, // unable to cast/convert
			DbConnectionString, // bad connection string
			DbTable, // could not found the data matrix (table) in the database
			DbColumn, // could not found the column in the data matrix (table) in the database
			DbColumnDataType, // bad data type of the column
			DbPrimaryKey, // values in the column(s) are not unique i.e. the specified column(s) is(are) not primery
			AttributeCategoriesDisjunctivity // categories of the attribute are not disjunctive
		};

		exception BadParamsError extends BoxRuntimeError{
			restrictionTypeEnum restrictionType;
		};

		exception BadValueError extends BadParamsError{
			StringSeq socketsNames; //socket = socket OR property
		};
	};
};

#endif;
