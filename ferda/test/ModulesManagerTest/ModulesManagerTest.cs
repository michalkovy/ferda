using System;
using System.Text;
using NUnit.Framework;
using Ferda;
using Ferda.ModulesManager;
using System.Diagnostics;
using System.IO;
using System.Collections.Generic;

namespace Ferda.ModulesManager
{
	[TestFixture]
	public class ModulesManagerTest
	{
		private ModulesManager manager;
		
		[TestFixtureSetUpAttribute]
		public void set_up()
		{
			Debug.Listeners.Clear();
			TextWriterTraceListener t = new TextWriterTraceListener("log.txt");
			Debug.Listeners.Add(t);
			Debug.AutoFlush = true;
			Debug.WriteLine("0");
			/*PlatformID platformID = Environment.OSVersion.Platform;
			
            if (platformID == PlatformID.Win32NT || platformID == PlatformID.Win32Windows) {
                Process process = new Process();
				process.StartInfo.FileName = "icepacknode";
				process.StartInfo.Arguments = "--start FerdaIcePackNode";
				process.Start();
				process.WaitForExit();
            }
			else
			{*/
            ProcessStartInfo psi = new ProcessStartInfo("icegridnode", "--Ice.Config=config --IceGrid.Registry.Data=registry --IceGrid.Node.Data=node --deploy application.xml --warn");
				psi.CreateNoWindow = true;
                psi.WorkingDirectory = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "../db");
				Process.Start(psi);
				/*ProcessStartInfo psi2 = new ProcessStartInfo("icepackadmin", "--Ice.Config=config -e \"application add 'application.xml'\"");
				psi2.CreateNoWindow = true;
				Process processReg = Process.Start(psi2);
				processReg.WaitForExit();*/
				System.Threading.Thread.Sleep(5000);
			/*}*/
			Debug.WriteLine("1");
			this.manager = new ModulesManager(new string[0],new string[2]{"cs-CZ","en-US"});
			Debug.WriteLine("2");
}
		
		[TestFixtureTearDownAttribute]
		public void tear_down()
		{
			try
			{
				this.manager.DestroyModulesManager();
			}
			finally
			{
				PlatformID platformID = Environment.OSVersion.Platform;
				/*if (!(platformID == PlatformID.Win32NT || platformID == PlatformID.Win32Windows))
				{*/
					ProcessStartInfo psi = new ProcessStartInfo("icegridadmin", "--Ice.Config=config -e shutdown");
					//-e \"server stop FerdaModules\" -e \"application remove FerdaModulesApplication\"
					psi.CreateNoWindow = true;
					Process processReg = Process.Start(psi);
					processReg.WaitForExit();
				/*}*/
			}
			
		}
		
		[Test]
		public void Test_GetLocales()
		{
			Assert.IsNotNull(this.manager,"M001 manager is null");
			Assert.IsNotNull(this.manager.Helper,"M002 manager.Helper is null");
			Assert.AreEqual(new string[2]{"cs-CZ","en-US"},this.manager.Helper.LocalePrefs, "M003");
		}
		
		[Test]
		public void Test_AddingAndRemovingCreators()
		{
			//TODO: give adding
			Assert.IsNotNull(this.manager.BoxModuleFactoryCreators,"M010 manager.BoxModuleFactoryCreators is null");
		}
		
		[Test]
		public void Test_GroupBoxesInCycle()
		{
			//creating creator of group boxes
			IBoxModuleFactoryCreator groupCreator = manager.GetBoxModuleFactoryCreator("group");
			Assert.IsNotNull(groupCreator, "M020");
			
			//creating two group boxes for testing
			IBoxModule boxa, boxb;
			boxa = groupCreator.CreateBoxModule();
			Assert.IsNotNull(boxa, "M021");
			boxb = groupCreator.CreateBoxModule();
			
			//looking if sockets newModule are OK
			Modules.SocketInfo[] groupSockets = boxb.Sockets;
			Assert.IsNotNull(groupSockets, "M022");
			Assert.AreEqual(1, groupSockets.Length, "M023");
			Modules.SocketInfo firstSocket = groupSockets[0];
			Assert.IsNotNull(firstSocket, "M024");
			
			//connecting one box to other and looking if connected correctly
			boxb.SetConnection(firstSocket.name, boxa);
			Assert.AreEqual(new IBoxModule[]{boxa},
                            boxb.GetConnections(firstSocket.name),
							"M025");
			List<IBoxModule> wanted = boxb.ConnectionsFrom();
			Assert.AreEqual(1, wanted.Count, "M026");
			Assert.AreEqual(boxa, wanted[0], "M027");
			
			//trying to connect boxes in cycle
			bool exceptionOK = false;
			try
			{
				boxa.SetConnection(firstSocket.name, boxb);
			}
			catch(Modules.BadTypeError)
			{
				exceptionOK = true;
			}
			Assert.IsTrue(exceptionOK,"M028");
			
			//trying the same with tree boxes
			IBoxModule boxc;
			boxc = groupCreator.CreateBoxModule();
			
			boxc.SetConnection(firstSocket.name, boxb);
			
			exceptionOK = false;
			try
			{
				boxa.SetConnection(firstSocket.name, boxc);
			}
			catch(Modules.BadTypeError)
			{
				exceptionOK = true;
			}
			Assert.IsTrue(exceptionOK,"M029");
			
			//destroying connections
			boxc.RemoveConnection(firstSocket.name, boxb);
			boxb.RemoveConnection(firstSocket.name, boxa);
			boxa.destroy();
			boxb.destroy();
			boxc.destroy();
		}

		[Test]
		public void Test_DatabaseBox()
		{
			//creating creator of database boxes
			IBoxModuleFactoryCreator databaseCreator = manager.GetBoxModuleFactoryCreator("DataMiningCommon.Database");
			Assert.IsNotNull(databaseCreator, "GetBoxModuleFactoryCreator(DataMiningCommon.Database)");
			
			//creating database box
			IBoxModule box = databaseCreator.CreateBoxModule();
			Assert.IsNotNull(box, "M031");
			
			//trying properties correctly
			box.SetPropertyString("ConnectionString", "test");
			Assert.AreEqual("test", box.GetPropertyString("ConnectionString"), "M032");
            /*
             * This is old - SQLStandard was removed from Database box
			box.SetPropertyString("SQLStandard", "MSAccess");
			Assert.AreEqual(box.GetPropertyString("SQLStandard"), "MSAccess", "M032");
			
			//trying to set uncorect value to SQLStandard
			bool exceptionThrown = false;
			Modules.restrictionTypeEnum restr = Modules.restrictionTypeEnum.Maximum;
			try
			{
				box.SetPropertyString("SQLStandard", "test");
			}
			catch(Ferda.Modules.BadValueError ex)
			{
				exceptionThrown = true;
				restr = ex.restrictionType;
			}
			Assert.IsTrue(exceptionThrown,"M033");
			Assert.AreEqual(restr, Modules.restrictionTypeEnum.NotInSelectOptions,"M034");
			Assert.AreEqual(box.GetPropertyString("SQLStandard"), "MSAccess", "M035");
             */
			
			//modulesAskingForCreation
			Assert.AreEqual(0, box.ModulesAskingForCreation.Length,"M033");
			
            /*
			Assert.AreEqual(box.GetPropertyOptions("SQLStandard").Length,3,"M037");*/
			
			//destroying database box
			box.destroy();
		}
		
		[Test]
		public void Test_DatabaseBoxDataMatrixBoxConnection()
		{
			//creating creator of database boxes
			IBoxModuleFactoryCreator databaseCreator = manager.GetBoxModuleFactoryCreator("DataMiningCommon.Database");
			
			//creating database box
			IBoxModule databaseBox = databaseCreator.CreateBoxModule();
			
			//creating creator of dataMatrix boxes
			IBoxModuleFactoryCreator matrixCreator = manager.GetBoxModuleFactoryCreator("DataMiningCommon.DataMatrix");
			Assert.IsNotNull(matrixCreator, "M040");
			
			//creating dataMatrix box
			IBoxModule matrixBox = matrixCreator.CreateBoxModule();
			Assert.IsNotNull(matrixBox, "M041");
			
			matrixBox.SetConnection("Database", databaseBox);
			IBoxModule[] cb = matrixBox.GetConnections("Database");
			Assert.AreEqual(1, cb.Length, "M042");
			Assert.AreEqual(databaseBox, cb[0], "M043");
			
			//modulesAskingForCreation
			Assert.AreEqual(1,matrixBox.ModulesAskingForCreation.Length,"M044");
                         // 1 .. is derived column
			
			matrixBox.RemoveConnection("Database", databaseBox);
			
			cb = matrixBox.GetConnections("Database");
			Assert.AreEqual(0, cb.Length, "M045");
		}
		
		[Test]
		public void Test_DataMatrixCloning()
		{
			IBoxModuleFactoryCreator matrixCreator = manager.GetBoxModuleFactoryCreator("DataMiningCommon.DataMatrix");

			IBoxModule matrixBox = matrixCreator.CreateBoxModule();
			IBoxModule matrixBoxClone = matrixBox.Clone();
			Assert.IsNotNull(matrixBoxClone, "M050");
		}
		
		[Test]
		public void Test_BoolBox()
		{
			IBoxModuleFactoryCreator boolCreator = manager.GetBoxModuleFactoryCreator("BoolT");
			Assert.IsNotNull(boolCreator, "M060");
			
			IBoxModule boolBox = boolCreator.CreateBoxModule();
			Assert.IsNotNull(boolBox, "M061");
			
			boolBox.SetPropertyBool("value", false);
			Assert.AreEqual(false, boolBox.GetPropertyBool("value"), "M062");
			
			boolBox.SetPropertyBool("value", true);
            Assert.AreEqual(true, boolBox.GetPropertyBool("value"), "M063");
		}
	}
}
