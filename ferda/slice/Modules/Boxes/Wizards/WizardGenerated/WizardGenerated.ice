/*
SLICE DOCUMENTATION
-------------------
	www.zeroc.com

	Some general keywords
	---------------------
	idempotent
		Operations that use the Slice idempotent keyword must not modify object state.
	idempotent
		Operations that use the Slice Idempotent keyword can modify object state, but invoking an operation twice in a row must result in the same object state as invoking it once.

HOW TO GENERATE *.cs FROM *.ice
-------------------------------
	Path to Ice/bin directory (e.g. "c:\Ice-2.1.2\bin\") is in PATH.
	Execute following command in solution root directory.

	$ slice2cs -I../../../slice/ --output-dir ./SampleBoxModules/SampleBoxes/SampleBoxModule/generated ./SampleBoxModules/SampleBoxes/SampleBoxModule/slice/SampleBoxModule.ice

		- Path in argument -I is path to "slice" subdirectory of Ferda project.
		- Path in argument --output-dir is place for generated *.cs files.
		- Following arguments are slice-files to proceed.

	See also
		$ slice2cs --help
*/

#ifndef Wizards_WizardGenerated
#define Wizards_WizardGenerated

#include <Modules/Common.ice> //some common enumerations and structures
#include <Modules/Exceptions.ice> //some exceptions defined
#include <Modules/BuiltinSequences.ice> //sequences of basic types

module Ferda
{
     module Modules
     {
          module Boxes
          {
               module Wizards 
               {
               	module WizardGenerated 
                    {
               		interface WizardGeneratedFunctions
                         {
               			idempotent string HelloWorld();
               		};
               	};
               };
          };
     };
};
#endif;
