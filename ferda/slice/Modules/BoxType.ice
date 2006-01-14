#ifndef FERDA_MODULES_BOX_TYPE
#define FERDA_MODULES_BOX_TYPE

module Ferda {
	module Modules {
		
		struct NeededSocket {

			/**
			 *
			 * name (identifier) of socket where is this connection conected
			 *
			 **/
			string socketName;

			/**
			 *
			 * type of functions provied by box which can be connected in this socket
			 *
			 **/
			string functionIceId;
		};
		
		sequence<NeededSocket> NeededSocketSeq;
		struct BoxType {
			string functionIceId;
			NeededSocketSeq neededSockets;
		};
		sequence<BoxType> BoxTypeSeq;
	};
};
#endif
