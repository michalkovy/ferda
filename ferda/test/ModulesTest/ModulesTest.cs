using Ferda.ModulesManager;
using NUnit.Framework;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Reflection;

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
			options.IceGridWorkingDirectory = "../../bin/db";
			options.IceGridApplicationXmlFilePath = "application.xml";
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
				try
				{
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
				catch (System.Exception e)
				{
					Assert.Fail("A02, getting property " + propertyName + " from " + boxModule.MadeInCreator.Identifier + " failed: " + e.ToString());
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
				catch(System.Exception e)
				{
					NUnit.Framework.Assert.Fail("A01: BoxModule " + boxModuleIdentifier + " failed with message: " + e.ToString() + "\n\n");
				}
			}
			Debug.WriteLine("leaving create all boxes test ...");
		}
		
		[Test]
		public void Test_LanguageBoxes()
		{
			IBoxModuleFactoryCreator creator = modulesManager.GetBoxModuleFactoryCreator("Language.Lambda");
			IBoxModule lambdaBox = creator.CreateBoxModule();
			lambdaBox.SetPropertyInt("VariablesCount", 3);
			int variablesCount = lambdaBox.GetPropertyInt("VariablesCount");
			Assert.AreEqual(3, variablesCount);
			StringCollection iceIds = lambdaBox.GetFunctionsIceIds();
			Assert.AreEqual(0, iceIds.Count);
		}
		
		[Test]
		public void Test_Lambda()
		{
			IBoxModuleFactoryCreator lambdaCreator = modulesManager.GetBoxModuleFactoryCreator("Language.Lambda");
			IBoxModule lambdaBox = lambdaCreator.CreateBoxModule();
			lambdaBox.SetPropertyInt("VariablesCount", 1);
			
			IBoxModuleFactoryCreator plusCreator = modulesManager.GetBoxModuleFactoryCreator("Language.Math.BinaryOperation");
			IBoxModule plusBox = plusCreator.CreateBoxModule();
			plusBox.SetPropertyString("type", "+");
			plusBox.SetPropertyDouble("value1", 1);
			
			IBoxModuleFactoryCreator doubleCreator = modulesManager.GetBoxModuleFactoryCreator("DoubleT");
			IBoxModule doubleBoxBefore = doubleCreator.CreateBoxModule();
			doubleBoxBefore.SetPropertyDouble("value", 1);
			
			IBoxModule doubleBoxAfter = doubleCreator.CreateBoxModule();
			doubleBoxAfter.SetPropertyDouble("value", 2);
			
			plusBox.SetPropertySocking("value2", true);
			plusBox.SetConnection("value2",doubleBoxBefore);
			lambdaBox.SetConnection("Function", plusBox);
			lambdaBox.SetConnection("Variable0", doubleBoxBefore);
			lambdaBox.SetConnection("VariableValue0", doubleBoxAfter);
			
			IBoxModule doubleBoxResult = doubleCreator.CreateBoxModule();
			doubleBoxResult.SetPropertySocking("value", true);
			doubleBoxResult.SetConnection("value", lambdaBox);
			
			Assert.AreEqual(3.0, doubleBoxResult.GetPropertyDouble("value"), 0.1);
		}
		
		[Test]
		public void Test_Recursion()
		{
			IBoxModuleFactoryCreator lambdaCreator = modulesManager.GetBoxModuleFactoryCreator("Language.Lambda");
			IBoxModule lambdaBox = lambdaCreator.CreateBoxModule();
			lambdaBox.SetPropertyInt("VariablesCount", 1);
			
			IBoxModuleFactoryCreator plusCreator = modulesManager.GetBoxModuleFactoryCreator("Language.Math.BinaryOperation");
			IBoxModule plusBox = plusCreator.CreateBoxModule();
			plusBox.SetPropertyString("type", "+");
			plusBox.SetPropertyDouble("value1", 1);
			
			IBoxModuleFactoryCreator doubleCreator = modulesManager.GetBoxModuleFactoryCreator("DoubleT");
			IBoxModule doubleBox1 = doubleCreator.CreateBoxModule();
			doubleBox1.SetPropertyDouble("value", 0);
			
			plusBox.SetPropertySocking("value2", true);
			plusBox.SetConnection("value2",doubleBox1);
			
			IBoxModuleFactoryCreator compareCreator = modulesManager.GetBoxModuleFactoryCreator("Language.Math.Compare");
			IBoxModule compareBox = compareCreator.CreateBoxModule();
			compareBox.SetPropertyDouble("value2", 5);
			compareBox.SetPropertyString("type", "<");
			compareBox.SetPropertySocking("value1", true);
			compareBox.SetConnection("value1",doubleBox1);
			
			IBoxModuleFactoryCreator ifThenElseCreator = modulesManager.GetBoxModuleFactoryCreator("Language.Math.IfThenElse");
			IBoxModule ifThenElseBox = ifThenElseCreator.CreateBoxModule();
			ifThenElseBox.SetConnection("if",compareBox);
			ifThenElseBox.SetConnection("then",lambdaBox);
			ifThenElseBox.SetConnection("else",doubleBox1);
			
			lambdaBox.SetConnection("Function", ifThenElseBox);
			lambdaBox.SetConnection("Variable0", doubleBox1);
			lambdaBox.SetConnection("VariableValue0", plusBox);
			
			IBoxModule doubleBoxResult = doubleCreator.CreateBoxModule();
			doubleBoxResult.SetPropertySocking("value", true);
			doubleBoxResult.SetConnection("value", ifThenElseBox);
			
			Assert.AreEqual(5.0, doubleBoxResult.GetPropertyDouble("value"), 0.1);
		}

        //[Test]
        //public void Test_OWLOntologyModule()
        //{
        //    IBoxModuleFactoryCreator ontologyCreator = modulesManager.GetBoxModuleFactoryCreator("OntologyRelated.Ontology");
        //    IBoxModule ontologyBox = ontologyCreator.CreateBoxModule();
        //    ontologyBox.RunAction("LoadOntology");
        //}

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
