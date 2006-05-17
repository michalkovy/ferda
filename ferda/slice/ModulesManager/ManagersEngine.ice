#ifndef FERDA_MODULES_MANAGER_MANAGERS_ENGINE
#define FERDA_MODULES_MANAGER_MANAGERS_ENGINE

#include <Modules/BoxType.ice>
#include <Modules/Modules.ice>
#include <Modules/Exceptions.ice>


module Ferda {
	module Modules {
		interface BoxModule;
		interface BoxModuleFactoryCreator;
		interface ModuleForInteraction;		
		interface SettingModule;		
	};
	
	module ModulesManager {

		enum MsgType { Debug, Info, Warning, Error };
		
		exception BoxModuleNotExistError{};
		
		interface ProgressTask {
		  nonmutating float getValue();
		  void stop();
    };
    
    interface ProgressBar {
      idempotent void setValue(float value);
      void done();
    };

		interface Output {
			void writeMsg(MsgType type, string name, string message);
			ProgressBar* startProgress(ProgressTask* task, string name, string hint);
		};
		
		interface BoxModuleProjectInformation {
			nonmutating string getUserLabel(string boxModuleIceIdentity)
				throws BoxModuleNotExistError;
			nonmutating string getUserHint(string boxModuleIceIdentity)
				throws BoxModuleNotExistError;
			nonmutating int getProjectIdentifier(string boxModuleIceIdentity)
				throws BoxModuleNotExistError;
		};

		interface BoxModuleLocker {
			idempotent void lockBoxModule(string boxModuleIceIdentity)
				throws BoxModuleNotExistError;
			idempotent void unlockBoxModule(string boxModuleIceIdentity)
				throws BoxModuleNotExistError;
		};
		
		interface BoxModuleValidator {
		  nonmutating void validate(string boxModuleIceIdentity)
  		  throws
  					Ferda::Modules::BoxRuntimeError,
  					Ferda::Modules::BadValueError,
  					Ferda::Modules::BadParamsError,
  					Ferda::Modules::NoConnectionInSocketError,
            BoxModuleNotExistError;
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
			nonmutating Ferda::Modules::BoxModuleFactoryCreator* findBoxModuleCreatorByIdentifier(string identifier);
			nonmutating Ferda::Modules::BoxModuleFactoryCreator* findBoxModuleCreatorByBoxType(Ferda::Modules::BoxType moduleType);
			nonmutating BoxModuleFactoryCreatorSeq findAllBoxModuleCreatorsWithBoxType(Ferda::Modules::BoxType moduleType);
			nonmutating Ferda::Modules::SettingModule* findSettingModule(string propertyIceId);
			nonmutating Ferda::Modules::ModuleForInteraction* findModuleForInteraction(Ferda::Modules::BoxModuleFactoryCreator* creator);
			nonmutating ModuleForInteractionSeq findAllModulesForInteraction(Ferda::Modules::BoxModuleFactoryCreator* creator);
			nonmutating ObjectSeq findAllObjectsWithType(string type);
			nonmutating Object* findObjectByType(string type);
		};

		interface ManagersEngine {
			nonmutating Output* getOutputInterface();
			nonmutating BoxModuleProjectInformation* getProjectInformation();
			nonmutating BoxModuleLocker* getBoxModuleLocker();
			nonmutating BoxModuleValidator* getBoxModuleValidator();
			nonmutating ManagersLocator* getManagersLocator();
		};
	};
};
#endif
