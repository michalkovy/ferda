#ifndef FERDA_MODULES_MANAGER_MANAGERS_ENGINE
#define FERDA_MODULES_MANAGER_MANAGERS_ENGINE

#include <Modules/BoxType.ice>
#include <Modules/Exceptions.ice>


module Ferda {
	module Modules {
		interface BoxModule;
		interface BoxModuleFactoryCreator;
		interface ModuleForInteraction;		
		interface SettingModule;
		interface ProgressTask;
	};
	
	module ModulesManager {

		enum MsgType { Debug, Info, Warning, Error };
		
		exception BoxModuleNotExistError{};
		
		interface ProgressBar {
		  // ten progress bar by mel ukazovat ze bezi, ale ne konretni hotovou cast (v %),
      // pokud tohle dlouho nezavolam, tak by se mel zastavit - hodnota zaporna
			idempotent void setValue(float value, string message);
			
			// Call this on end. After calling this, its proxy will not be usable
			void done();
		};

		interface Output {
			void writeMsg(MsgType type, string name, string message);
			ProgressBar* startProgress(Ferda::Modules::ProgressTask* task, string name, string hint);
		};
		
		interface BoxModuleProjectInformation {
			idempotent string getUserLabel(string boxModuleIceIdentity)
				throws BoxModuleNotExistError;
			idempotent string getUserHint(string boxModuleIceIdentity)
				throws BoxModuleNotExistError;
			idempotent int getProjectIdentifier(string boxModuleIceIdentity)
				throws BoxModuleNotExistError;
		};

		interface BoxModuleLocker {
			idempotent void lockBoxModule(string boxModuleIceIdentity)
				throws BoxModuleNotExistError;
			idempotent void unlockBoxModule(string boxModuleIceIdentity)
				throws BoxModuleNotExistError;
		};
		
		interface BoxModuleValidator {
			idempotent void validate(string boxModuleIceIdentity)
				throws
						Ferda::Modules::BoxRuntimeError,
						Ferda::Modules::BadValueError,
						Ferda::Modules::BadParamsError,
						Ferda::Modules::NoConnectionInSocketError,
						BoxModuleNotExistError;
		};

		sequence<Ferda::Modules::BoxModule*> BoxModuleSeq;
		sequence<BoxModuleSeq> BoxModuleSeqSeq;

		interface BoxModuleManager {
			/**
			 * Clone box boxModule with all boxes connected to it.
			 * 
			 * @param boxModule The parent box module which we want to clone with all child box modules
			 * 
			 * @return The cloned version of box module boxModule
			 **/
			Ferda::Modules::BoxModule* CloneBoxModuleWithChilds(Ferda::Modules::BoxModule* boxModule,
				bool addToProject,
				BoxModuleSeqSeq variables,
				BoxModuleSeq variableValues);
		};
		
		
		sequence<Ferda::Modules::BoxModuleFactoryCreator*> BoxModuleFactoryCreatorSeq;
		sequence<Ferda::Modules::ModuleForInteraction*> ModuleForInteractionSeq;
		sequence<Object*> ObjectSeq;
		
		/**
		 *
		 * Searches for ice objects avariable for work with
		 *
		 **/
		interface ManagersLocator {
			idempotent Ferda::Modules::BoxModuleFactoryCreator* findBoxModuleCreatorByIdentifier(string identifier);
			idempotent Ferda::Modules::BoxModuleFactoryCreator* findBoxModuleCreatorByBoxType(Ferda::Modules::BoxType moduleType);
			idempotent BoxModuleFactoryCreatorSeq findAllBoxModuleCreatorsWithBoxType(Ferda::Modules::BoxType moduleType);
			idempotent Ferda::Modules::SettingModule* findSettingModule(string propertyIceId);
			idempotent Ferda::Modules::ModuleForInteraction* findModuleForInteraction(Ferda::Modules::BoxModuleFactoryCreator* creator);
			idempotent ModuleForInteractionSeq findAllModulesForInteraction(Ferda::Modules::BoxModuleFactoryCreator* creator);
			idempotent ObjectSeq findAllObjectsWithType(string type);
			idempotent Object* findObjectByType(string type);
		};

		interface ManagersEngine {
			idempotent Output* getOutputInterface();
			idempotent BoxModuleProjectInformation* getProjectInformation();
			idempotent BoxModuleLocker* getBoxModuleLocker();
			idempotent BoxModuleValidator* getBoxModuleValidator();
			idempotent BoxModuleManager* getBoxModuleManager();
			idempotent ManagersLocator* getManagersLocator();
		};
	};
};
#endif
