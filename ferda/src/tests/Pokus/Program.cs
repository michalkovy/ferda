using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Ferda.Modules;
using Ferda.ModulesManager;
using Ferda.ProjectManager;

namespace Pokus
{
    class Program
    {
        static void Main(string[] args)
        {
            //IceGridnode --Ice.Config=config --warn
            //IceGridadmin --Ice.Config=config -e "application add 'application.xml'"

            Debug.Listeners.Clear();
            TextWriterTraceListener t = new TextWriterTraceListener("log.txt");
            Debug.Listeners.Add(t);
            Debug.AutoFlush = true;
            Debug.WriteLine("0");
            Debug.WriteLine("1");

            //bez modules manageru
            /**
            Ice.Communicator ic = null;
            ic = Ice.Util.initialize(ref args);
            Ice.ObjectPrx obj = ic.stringToProxy("DatabaseFactoryCreator:default -p 4000");
            BoxModuleFactoryCreatorPrx creatorPrx =
                BoxModuleFactoryCreatorPrxHelper.checkedCast(obj);
            creatorPrx.createBoxModuleFactory(new string[] { "cs-CZ", "en-US" }, null).getProperties();
            ic.destroy();
            /**/

            //S modules managerem
            /**/
            //ModulesManager manager = new ModulesManager(new string[0], new string[2] { "cs-CZ", "en-US" });

            //IBoxModule box = manager.GetBoxModuleFactoryCreator("Database").CreateBoxModule();
            //IBoxModule otherBox = manager.GetBoxModuleFactoryCreator("Database").CreateBoxModule();
            //box.SetConnection("sdfd", otherBox);
            //manager.DestroyModulesManager();
            /**/

			ProjectManager projectManager = new ProjectManager(new string[0], new string[] { "cs-CZ", "en-US" }, true);

			Debug.WriteLine("getting modules manager...");
			/*Ice.ObjectAdapter adapter = projectManager.ModulesManager.Helper.ObjectAdapter;*/
			Ferda.ModulesManager.ModulesManager modulesManager = projectManager.ModulesManager;
			/*modulesManager.AddModuleService(
				adapter.addWithUUID(new Ferda.Modules.SampleStringSeqSettingModule()).ice_collocationOptimization(false));*/
			Debug.WriteLine("getting dataMatrix creator...");
			Ferda.ModulesManager.IBoxModuleFactoryCreator creator =
				modulesManager.GetBoxModuleFactoryCreator("DataMatrix");
			Debug.WriteLine("creating boxModule...");
			Ferda.ModulesManager.IBoxModule b = creator.CreateBoxModule();
			Debug.WriteLine("destroying boxModule...");
			
			projectManager.DestroyProjectManager();

            Debug.WriteLine("2");
            Environment.Exit(0);
        }
    }
}
