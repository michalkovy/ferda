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
			options.IceGridWorkingDirectory = "/home/michal/studium/mff/ferda/ferda/bin/db";
			options.IceGridApplicationXmlFilePath = "/home/michal/studium/mff/ferda/ferda/bin/db/application.xml";
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
	}
}
