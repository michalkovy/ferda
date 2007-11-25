#ifndef FERDA_LANGUAGE
#define FERDA_LANGUAGE

module Ferda {
	module Modules {
		module Boxes {
			module Language {
				//neni pravda ze zasuvka pro standard input musi mit tento interface
				//zasuvka bude mit opet interface CommandFunctions
				interface CommandTextStream{
					void write(string text);
					void closeStream();
				};

				interface CommandFunctions{
				//	idempotent void setStandardOutput(CommandTextStream* textStream);
				//	idempotent void setErrorOutput(CommandTextStream* textStream);
				//CommandTextStream* getInput();
					int ExecuteCommand(CommandTextStream* standardOutput, CommandTextStream* errorOutput);
				};
			};
		};
	};
};
#endif
