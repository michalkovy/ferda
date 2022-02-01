
#ifndef FERDA_NETWORK_ARCHIVE
#define FERDA_NETWORK_ARCHIVE

#include <Modules/BuiltinSequences.ice>
#include <Modules/BasicPropertyTypes.ice>
#include <Modules/Modules.ice>

module Ferda {
	module NetworkArchive {
		/* forward declarations */
		class Box;
	
		/**
		 *
		 * Connection between boxes
		 *
		 **/
		["clr:class"]
		struct Connection {
			/**
			 *
			 * Name of socket
			 *
			 **/
			string socketName;

			/**
			 *
			 * Box
			 *
			 **/
			Box boxValue; 
		};
		sequence<Connection> ConnectionSeq;
		
		/**
		 *
		 * Class representing a box in serialized structure
		 *
		 **/
		class Box {
			
			/**
			 *
			 * Identifier of creator of factory for box
			 *
			 **/
			string creatorIdentifier;

			/**
			 *
			 * Name of box instance defined by user
			 *
			 **/
			string userName;

			/**
			 *
			 * User notes for box instance
			 *
			 **/
			string userHint;

			/**
			 *
			 * What is connected in sockets
			 *
			 **/
			ConnectionSeq Connections;

			/**
			 *
			 * How are properties set
			 *
			 **/
			Ferda::Modules::PropertySettingSeq PropertySets;

			/**
			 *
			 * Socked properties
			 *
			 **/
			Ferda::Modules::StringSeq SockedProperties;
		};
		
		exception NullParamError{};
		exception NameExistsError{};
		exception NameNotExistsError{};
	
		interface Archive {
			/**
			 *
			 * Adds a box with connected subboxes to the archive
			 *
			 **/
			void addBox(Box boxValue, string label)
				throws NullParamError, NameExistsError;
			
			/**
			 *
			 * Removes the box from archive
			 *
			 **/
			void removeBox(string label)
				throws NameNotExistsError;
			
			/**
			 *
			 * Gets box which is in the archive
			 *
			 **/
			Box getBox(string label)
				throws NameNotExistsError;
			
			/**
			 *
			 * Gets labels of boxes in the archive
			 *
			 **/
			idempotent Ferda::Modules::StringSeq listLabels();
		};
	};
};

#endif
