using System;
using System.Text;
using NUnit.Framework;
using System.Diagnostics;

namespace Ferda.ProjectManager
{
	[TestFixture]
	public class ProjectManagerTest
	{
		private ProjectManager projectManager;
		
		[TestFixtureSetUp]
		public void SetUp()
		{
			Debug.Listeners.Clear();
			TextWriterTraceListener t = new TextWriterTraceListener("log.txt");
			Debug.Listeners.Add(t);
			Debug.AutoFlush = true;
			Debug.WriteLine("starting projectManager...");
            ProjectManagerOptions options = new ProjectManagerOptions();
            options.StartIceGridLocaly = true;
            options.StopIceGridLocaly = true;
            options.IceGridAsService = false;
            options.LocalePrefs = new string[]{"en-US","cs-CZ"};
            options.SentenceForWait = "[ icegridnode: Server: changed server `0' state to `Inactive' ]";
            projectManager = new ProjectManager(new string[0], options);
		}
		
		[TestFixtureTearDown]
		public void TearDown()
		{
            Debug.WriteLine("destroying projectManager...");
			projectManager.DestroyProjectManager();
            Debug.WriteLine("projectManager destroyed");
		}
		
        /*
		[Test]
		public void Test_StringSeqSettingModule()
		{
			Debug.WriteLine("getting modules manager...");
			//Ice.ObjectAdapter adapter = projectManager.ModulesManager.Helper.ObjectAdapter;
			Ferda.ModulesManager.ModulesManager modulesManager = projectManager.ModulesManager;
			//modulesManager.AddModuleService(
			//	adapter.addWithUUID(new Ferda.Modules.SampleStringSeqSettingModule()).ice_collocationOptimization(false));
			Debug.WriteLine("getting dataMatrix creator...");
			Ferda.ModulesManager.IBoxModuleFactoryCreator creator =
				modulesManager.GetBoxModuleFactoryCreator("DataMiningCommon.DataMatrix");
			Debug.WriteLine("creating boxModule...");
			Ferda.ModulesManager.IBoxModule b = creator.CreateBoxModule();
			Debug.WriteLine("destroying boxModule...");
			b.destroy();
            //Ferda.ModulesManager.IBoxModule dataMatrix = 
            //string about;
            //dataMatrix.RunSetPropertyOther("PrimaryKeyColumns",out about);
            //Assert.AreEqual(about, "ttt");
		}*/
		
		[Test]
		public void Test_SavingAndLoadingProject()
		{
			Ferda.ModulesManager.ModulesManager modulesManager = projectManager.ModulesManager;
			Ferda.ModulesManager.IBoxModuleFactoryCreator creator =
				modulesManager.GetBoxModuleFactoryCreator("DataMiningCommon.DataMatrix");

			Ferda.ModulesManager.IBoxModule b = creator.CreateBoxModule();
			
			Ferda.ModulesManager.IBoxModuleFactoryCreator creatorDatabase =
				modulesManager.GetBoxModuleFactoryCreator("DataMiningCommon.Database");
			
			Ferda.ModulesManager.IBoxModule bDatabase = creatorDatabase.CreateBoxModule();
			
			b.SetConnection("Database", bDatabase);
			
			projectManager.Archive.Add(b);
			projectManager.Archive.Add(bDatabase);
			View viv = projectManager.NewView("testView");
			viv.Add(b,new System.Drawing.PointF(4,5));
			
			System.IO.FileStream fs = new System.IO.FileStream("test.xml",System.IO.FileMode.Create);
			projectManager.SaveProject(fs);
			fs.Close();
			
			fs = new System.IO.FileStream("test.xml",System.IO.FileMode.Open);
			projectManager.LoadProject(fs);
			fs.Close();
		}
	}
}
