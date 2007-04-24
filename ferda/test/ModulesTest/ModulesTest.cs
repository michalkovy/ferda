using Ferda.ModulesManager;
using NUnit.Framework;
using System.Diagnostics;

namespace Ferda.Modules
{
	[TestFixture]
	public class ModulesTest
	{
		private Ferda.ProjectManager.ProjectManager projectManager;
		private Ferda.ModulesManager.ModulesManager modulesManager;
		
		[TestFixtureSetUp]
		public void SetUp()
		{
			Debug.Listeners.Clear();
			TextWriterTraceListener t = new TextWriterTraceListener("log.txt");
			Debug.Listeners.Add(t);
			Debug.AutoFlush = true;
			Debug.WriteLine("starting projectManager...");
            Ferda.ProjectManager.ProjectManagerOptions options = new Ferda.ProjectManager.ProjectManagerOptions();
            options.StartIceGridLocaly = true;
            options.StopIceGridLocaly = true;
            options.IceGridAsService = false;
			options.IceGridWorkingDirectory = "/home/michal/studium/mff/ferda/ferda/bin/db";
			options.IceGridApplicationXmlFilePath = "/home/michal/studium/mff/ferda/ferda/bin/db/application.xml";
            options.LocalePrefs = new string[]{"cs-CZ","en-US"};
            options.SentenceForWait = "Server: changed server `0' state to `Inactive' ]";
            projectManager = new Ferda.ProjectManager.ProjectManager(new string[0], options);
			modulesManager = projectManager.ModulesManager;
		}
		
		[TestFixtureTearDown]
		public void TearDown()
		{
            Debug.WriteLine("destroying projectManager...");
			projectManager.DestroyProjectManager();
            Debug.WriteLine("projectManager destroyed");
		}

		private void getBoxModuleProperties(IBoxModule boxModule)
		{
			Debug.WriteLine("enterint test get properties ...");
			PropertyInfo[] properties = boxModule.MadeInCreator.Properties;
			foreach (PropertyInfo property in properties)
			{
				string propertyName = property.name;
				Debug.WriteLine("try to get property " + propertyName);
				switch (property.typeClassIceId)
				{
					case "::Ferda::Modules::BoolT":
						boxModule.GetPropertyBool(propertyName);
						break;
					case "::Ferda::Modules::DateT":
						boxModule.GetPropertyDate(propertyName);
						break;
					case "::Ferda::Modules::DateTimeT":
						boxModule.GetPropertyDateTime(propertyName);
						break;
					case "::Ferda::Modules::DoubleT":
						boxModule.GetPropertyDouble(propertyName);
						break;
					case "::Ferda::Modules::FloatT":
						boxModule.GetPropertyFloat(propertyName);
						break;
					case "::Ferda::Modules::IntT":
						boxModule.GetPropertyInt(propertyName);
						break;
					case "::Ferda::Modules::LongT":
						boxModule.GetPropertyLong(propertyName);
						break;
					case "::Ferda::Modules::ShortT":
						boxModule.GetPropertyShort(propertyName);
						break;
					case "::Ferda::Modules::StringT":
						boxModule.GetPropertyString(propertyName);
						if (property.selectBoxParams != null && property.selectBoxParams.Length > 0)
							boxModule.GetPropertyOptions(propertyName);
						break;
					case "::Ferda::Modules::TimeT":
						boxModule.GetPropertyTime(propertyName);
						break;
					case "::Ferda::Modules::OtherT":
						//TODO
						break;
				}
			}
			Debug.WriteLine("leaving test get properties ...");
		}

		[Test]
		public void Test_CreateAllBoxes()
		{
			Debug.WriteLine("entering create all boxes test ...");
			IBoxModuleFactoryCreator[] boxModuleFactoryCreators = modulesManager.BoxModuleFactoryCreators;
			foreach (IBoxModuleFactoryCreator boxModuleFactoryCreator in boxModuleFactoryCreators)
			{
				string boxModuleIdentifier = boxModuleFactoryCreator.Identifier;
				try
				
				{
					Debug.WriteLine("testing module " + boxModuleIdentifier);
					Debug.WriteLine("  CreateBoxModule ...");
					IBoxModule boxModule = boxModuleFactoryCreator.CreateBoxModule();
					Debug.WriteLine("  ModulesAskingForCreation ...");
					ModulesAskingForCreation[] modulesAskingForCreation = boxModule.ModulesAskingForCreation;
					Debug.WriteLine("  DynamicHelpItems ...");
					DynamicHelpItem[] dynamicHelp = boxModule.DynamicHelpItems;
					Debug.WriteLine("  Sockets, UserHint, UserName, ...");
					SocketInfo[] sockets = boxModule.Sockets;
					string userHint = boxModule.UserHint;
					string userName = boxModule.UserName;
					this.getBoxModuleProperties(boxModule);
					Debug.WriteLine("box module " + boxModuleIdentifier + " tested");
					Debug.WriteLine("user name:" + userName);
					Debug.WriteLine("user hint:" + userHint);
					foreach(SocketInfo socket in sockets)
					{
						Debug.WriteLine("Socket:" + socket.name);
					}
					Debug.WriteLine("dynamic helps:" + dynamicHelp.Length);
					Debug.WriteLine("modulesAskingForCreation:" + modulesAskingForCreation.Length);
				}
				catch(System.Exception)
				{
					NUnit.Framework.Assert.Fail("BoxModule " + boxModuleIdentifier + " failed");
				}
			}
			Debug.WriteLine("leaving create all boxes test ...");
		}

        /*
		[Test]
		public void Test_DatabaseBox()
		{
			IBoxModuleFactoryCreator databaseCreator = modulesManager.GetBoxModuleFactoryCreator("DataMiningCommon.Database");
			IBoxModule databaseBox = databaseCreator.CreateBoxModule();
			databaseBox.SetPropertyString("ConnectionString", "DSN=LM Barbora.mdb");
			//databaseBox.RunAction("TestConnectionString");
		}
         */
	}
}
