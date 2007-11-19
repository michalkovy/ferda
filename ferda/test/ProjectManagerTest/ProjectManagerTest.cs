// ProjectManagerTest.cs - NUnit tests for Project Manager
//
// Author: Michal Kováč <michal.kovac.develop@centrum.cz>
//
// Copyright (c) 2005 Michal Kováč
//
// This program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA

using System;
using System.Text;
using NUnit.Framework;
using Ferda;
using Ferda.ModulesManager;
using System.Diagnostics;
using System.IO;
using System.Collections.Generic;
using System.Collections.Specialized;

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
			Ferda.ProjectManager.ProjectManagerOptions options = new Ferda.ProjectManager.ProjectManagerOptions();
			options.StartIceGridLocaly = true;
			options.StopIceGridLocaly = true;
			options.IceGridAsService = false;
			options.IceGridWorkingDirectory = "../../bin/db";
			options.IceGridApplicationXmlFilePath = "application.xml";
			options.LocalePrefs = new string[]{"cs-CZ","en-US"};
			options.SentenceForWait = "Server: changed server `0' state to `Inactive' ]";
			projectManager = new Ferda.ProjectManager.ProjectManager(new string[0], options);
		}
		
		[TestFixtureTearDown]
		public void TearDown()
		{
			Debug.WriteLine("destroying projectManager...");
			projectManager.DestroyProjectManager();
			Debug.WriteLine("projectManager destroyed");
		}
		
        /*
        /// <summary>
        /// Will test working with sample setting module - historical
        /// </summary>
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
		
		/// <summary>
		/// Will try to create sample project, save it and load it
		/// </summary>
		[Test]
		public void Test_SavingAndLoadingProject()
		{
			Ferda.ModulesManager.ModulesManager modulesManager = projectManager.ModulesManager;
			Ferda.ModulesManager.IBoxModule b = null;
			Ferda.ModulesManager.IBoxModule bDatabase = null;
			
			try
			{
			Ferda.ModulesManager.IBoxModuleFactoryCreator creator =
					modulesManager.GetBoxModuleFactoryCreator("DataPreparation.DataSource.DataTable");

			b = creator.CreateBoxModule();
			
			Ferda.ModulesManager.IBoxModuleFactoryCreator creatorDatabase =
					modulesManager.GetBoxModuleFactoryCreator("DataPreparation.DataSource.Database");
			bDatabase = creatorDatabase.CreateBoxModule();
			}
			catch (Exception)
			{
				Assert.Fail("PM00: Can not connect database to datatable");
			}
			
			try
			{
				b.SetConnection("Database", bDatabase);
			}
			catch (Exception)
			{
				Assert.Fail("PM01: Can not connect database to datatable");
			}
			
			try
			{
			projectManager.Archive.Add(b);
			projectManager.Archive.Add(bDatabase);
			View viv = projectManager.NewView("testView");
			viv.Add(b,new System.Drawing.PointF(4,5));
			}
			catch (Exception)
			{
				Assert.Fail("PM02: Can not connect database to datatable");
			}
			
			try
			{
			System.IO.FileStream fs = new System.IO.FileStream("test.xml",System.IO.FileMode.Create);
			projectManager.SaveProject(fs);
			fs.Close();
			}
			catch (Exception)
			{
				Assert.Fail("PM03: Can not connect database to datatable");
			}
			
			try
			{
			System.IO.FileStream fs = new System.IO.FileStream("test.xml",System.IO.FileMode.Open);
			projectManager.LoadProject(fs);
			fs.Close();
			}
			catch (Exception)
			{
				Assert.Fail("PM04: Can not connect database to datatable");
			}
		}
		
		/// <summary>
		/// Will try to do many things with network archive
		/// </summary>
		[Test]
		public void Test_NetworkArchive()
		{
			Ferda.ModulesManager.ModulesManager modulesManager = projectManager.ModulesManager;
			
			Ferda.ModulesManager.IBoxModuleFactoryCreator creator =
				modulesManager.GetBoxModuleFactoryCreator("DataPreparation.DataSource.DataTable");
			Ferda.ModulesManager.IBoxModule b = creator.CreateBoxModule();
			
			Ferda.ModulesManager.IBoxModuleFactoryCreator creatorDatabase =
				modulesManager.GetBoxModuleFactoryCreator("DataPreparation.DataSource.Database");
			Ferda.ModulesManager.IBoxModule bDatabase = creatorDatabase.CreateBoxModule();
			
			b.SetConnection("Database", bDatabase);
			projectManager.Archive.Add(b);
			projectManager.Archive.Add(bDatabase);
			
			Ferda.ProjectManager.NetworkArchive networkArchive = projectManager.NetworkArchive;
			
			StringCollection labels = new StringCollection();
			labels.AddRange(networkArchive.Labels);
			if (labels.Contains("Test Box Database"))
				networkArchive.RemoveBox("Test Box Database");
			
			if (labels.Contains("Test Box Datatable"))
				networkArchive.RemoveBox("Test Box Datatable");
			
			labels = new StringCollection();
			labels.AddRange(networkArchive.Labels);
			Assert.IsFalse(labels.Contains("Test Box Database") || labels.Contains("Test Box Datatable"),
						  "PM100.1: Labels in network archive still contains testing boxes");
			
			try
			{
				networkArchive.AddBox(bDatabase, "Test Box Database");
			}
			catch (Exception e)
			{
				Assert.Fail("PM100: Could not add Database box to archive: {0}", e.ToString());
			}
			
			try
			{
				networkArchive.AddBox(b, "Test Box Datatable");
			}
			catch (Exception e)
			{
				Assert.Fail("PM101: Could not add DataTable box to archive: {0}", e.ToString());
			}
			
			labels = new StringCollection();
			labels.AddRange(networkArchive.Labels);
			Assert.IsTrue(labels.Contains("Test Box Database") && labels.Contains("Test Box Datatable"),
						  "PM102: Labels in network archive does not contains added boxes");
			
			Assert.AreSame(
				networkArchive.GetBoxModuleFactoryCreatorOfBox("Test Box Datatable"),
				b.MadeInCreator,
				"PM103: Creator of old box is not same as new one"
			);
			
			string errors = null;
			try
			{
				networkArchive.GetBoxToProject("Test Box Database", out errors);
			}
			catch (Exception e)
			{
				Assert.Fail("PM104: Could not load DataBase box from archive to project: {0}", e.ToString());
			}
			
			Assert.IsTrue(String.IsNullOrEmpty(errors),
						  "PM105: There are errors when loading stored database box: {0}",
						  errors);
			
			IBoxModule newB = null;
			try
			{
				newB = networkArchive.GetBoxToProject("Test Box Datatable", out errors);
			}
			catch (Exception e)
			{
				Assert.Fail("PM106: Could not load Datatable box from archive to project: {0}", e.ToString());
			}
			
			Assert.IsTrue(String.IsNullOrEmpty(errors),
						  "PM107: There are errors when loading stored datatable box: {0}",
						  errors);
			
			Assert.AreSame(newB.MadeInCreator, b.MadeInCreator, "PM108");
			Assert.AreEqual(newB.UserName, b.UserName, "PM109");
			Assert.AreEqual(newB.ConnectionsFrom().Count, b.ConnectionsFrom().Count, "PM110");
			Assert.AreEqual(newB.ConnectionsFrom()[0].MadeInCreator, b.ConnectionsFrom()[0].MadeInCreator, "PM111");
			
			try
			{
				networkArchive.RemoveBox("Test Box Database");
			}
			catch (Exception e)
			{
				Assert.Fail("PM121: Could not add Database box to archive: {0}", e.ToString());
			}
			
			try
			{
				networkArchive.RemoveBox("Test Box Datatable");
			}
			catch (Exception e)
			{
				Assert.Fail("PM122: Could not add DataTable box to archive: {0}", e.ToString());
			}
			
			//a little bit problematic connection - only for test
			/*Ferda.ModulesManager.IBoxModuleFactoryCreator creatorString =
				modulesManager.GetBoxModuleFactoryCreator("StringT");
			Ferda.ModulesManager.IBoxModule bString = creatorString.CreateBoxModule();
			
			Assert.IsNotNull(bString, "PM123");
			
			bString.SetConnection("value", bString);
			
			networkArchive.AddBox(bString, "Test String Box");*/
			
		}
		
		/// <summary>
		/// Will try to do many things with cloning
		/// </summary>
		[Test]
		public void Test_Cloning()
		{
			Ferda.ModulesManager.ModulesManager modulesManager = projectManager.ModulesManager;
			
			Ferda.ModulesManager.IBoxModuleFactoryCreator creator =
				modulesManager.GetBoxModuleFactoryCreator("DataPreparation.DataSource.DataTable");
			Ferda.ModulesManager.IBoxModule b = creator.CreateBoxModule();
			
			Ferda.ModulesManager.IBoxModuleFactoryCreator creatorDatabase =
				modulesManager.GetBoxModuleFactoryCreator("DataPreparation.DataSource.Database");
			Ferda.ModulesManager.IBoxModule bDatabase = creatorDatabase.CreateBoxModule();
			
			b.SetConnection("Database", bDatabase);
			projectManager.Archive.Add(b);
			projectManager.Archive.Add(bDatabase);

			IBoxModule newB = projectManager.CloneBoxModuleWithChilds(b, false, null, null);
			
			Assert.AreSame(newB.MadeInCreator, b.MadeInCreator, "PM111");
			Assert.AreEqual(newB.UserName, b.UserName, "PM112");
			Assert.AreEqual(newB.ConnectionsFrom().Count, b.ConnectionsFrom().Count, "PM113");
			Assert.AreEqual(newB.ConnectionsFrom()[0].MadeInCreator, b.ConnectionsFrom()[0].MadeInCreator, "PM114");

			
			//a little bit problematic connection - only for test
			/*Ferda.ModulesManager.IBoxModuleFactoryCreator creatorString =
				modulesManager.GetBoxModuleFactoryCreator("StringT");
			Ferda.ModulesManager.IBoxModule bString = creatorString.CreateBoxModule();
			
			Assert.IsNotNull(bString, "PM123");
			
			bString.SetConnection("value", bString);
			
			networkArchive.AddBox(bString, "Test String Box");*/
			
		}
		
		/// <summary>
		/// Will try to do many things with cloning
		/// </summary>
		[Test]
		public void Test_Cloning2()
		{
			Ferda.ModulesManager.ModulesManager modulesManager = projectManager.ModulesManager;
			
			IBoxModuleFactoryCreator plusCreator = modulesManager.GetBoxModuleFactoryCreator("Language.Math.BinaryOperation");
			IBoxModule plusBox = plusCreator.CreateBoxModule();
			plusBox.SetPropertyString("type", "+");
			plusBox.SetPropertyDouble("value1", 1);
			
			IBoxModuleFactoryCreator doubleCreator = modulesManager.GetBoxModuleFactoryCreator("DoubleT");
			IBoxModule doubleBoxBefore = doubleCreator.CreateBoxModule();
			doubleBoxBefore.SetPropertyDouble("value", 1);
			
			projectManager.Archive.Add(plusBox);
			projectManager.Archive.Add(doubleBoxBefore);
			
			plusBox.SetPropertySocking("value2", true);
			plusBox.SetConnection("value2",doubleBoxBefore);
			
			IBoxModule newB = projectManager.CloneBoxModuleWithChilds(plusBox, false, null, null);
			
			Assert.AreSame(newB.MadeInCreator, plusBox.MadeInCreator, "PM121");
			Assert.AreEqual(newB.UserName, plusBox.UserName, "PM122");
			
			IBoxModule[] connectedToSucc2 = newB.GetConnections("value2");
			//Assert.AreEqual(newB.ConnectionsFrom().Count, plusBox.ConnectionsFrom().Count, "PM123");
			Assert.AreEqual(1, connectedToSucc2.Length, "PM123");
			Assert.AreEqual("DoubleT", connectedToSucc2[0].MadeInCreator.Identifier, "PM124");
			Assert.AreEqual(1, connectedToSucc2[0].GetPropertyDouble("value"), 0.01, "PM125");
			
			//Assert.AreEqual(newB.ConnectionsFrom()[0].MadeInCreator, plusBox.ConnectionsFrom()[0].MadeInCreator, "PM124");
		}	
	}
}
