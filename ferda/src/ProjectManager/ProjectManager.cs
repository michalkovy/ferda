// ProjectManager.cs - Main file of Project Manager
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
using System.Collections.Generic;
using System.Collections.Specialized;
using Ferda.ModulesManager;
using System.Diagnostics;
using System.Threading;
using System.IO;
using IceGrid;
using System.Xml.Serialization;

namespace Ferda.ProjectManager {

    /// <summary>
    /// Main class for working with Project manager.
    /// </summary>
    public class ProjectManager : IProjectLoader, Ferda.ModulesManager.IProjectHelper {
        private List<View> views = new List<View>();
        private Archive archive;
        private Ferda.ProjectManager.NetworkArchive networkArchive;
        private Ferda.ModulesManager.ModulesManager modulesManager;

        private StreamReader _stdError;

        private StreamReader _stdOut;

        private Object waitForStartObject = new Object();
        private bool waitForStart = false;

        private Thread outputThread;

        private Thread errorThread;

        private Process process;

        private StreamReader _adminStdError;

        private StreamReader _adminStdOut;

        private static Object consoleLock = new Object();

        private ProjectManagerOptions options;

        /// <summary>
        /// Constructs project manager with OutputPrx writing to console.
        /// </summary>
        /// <param name="args">A string[] representing arguments from command line.</param>
        /// <param name="options">A <see cref="T:Ferda.ProjectManager.ProjectManagerOptions"/>
        /// representing options for ProjectManaget</param>
        public ProjectManager(string[] args, ProjectManagerOptions options)
        {
            this.options = options;
            if(options.StartIceGridLocaly) StartIceGrid(options.IceBinPath, options.IceGridAsService, options.IceGridWorkingDirectory, options.IceGridApplicationXmlFilePath);
            modulesManager = new Ferda.ModulesManager.ModulesManager(args, options.LocalePrefs, this);
            archive = new Archive(views);
            networkArchive = new Ferda.ProjectManager.NetworkArchive(modulesManager,this);
        }

        /// <summary>
        /// Constructs project manager. You can choose your own OutputPrx
        /// </summary>
        /// <param name="args">A string[] representing arguments from command line.</param>
        /// <param name="options">A <see cref="T:Ferda.ProjectManager.ProjectManagerOptions"/>
        /// representing options for ProjectManaget</param>
        /// <param name="output">A <see cref="T:Ferda.ModulesManager.Output"/> representing
        /// interface for modules for writing informations to user</param>
        public ProjectManager(string[] args, ProjectManagerOptions options, Ferda.ModulesManager.Output output)
        {
            this.options = options;
            if (options.StartIceGridLocaly) StartIceGrid(options.IceBinPath, options.IceGridAsService, options.IceGridWorkingDirectory, options.IceGridApplicationXmlFilePath);
            modulesManager = new Ferda.ModulesManager.ModulesManager(args, options.LocalePrefs, output, this);
            archive = new Archive(views);
            networkArchive = new Ferda.ProjectManager.NetworkArchive(modulesManager,this);
        }

        private void StreamReaderThread_Output() {
            StreamReader reader = _stdOut;
        	
            while (true) {
                string logContents = reader.ReadLine();
                if (logContents == null) {
                    break;
                }
	            lock(consoleLock)
	            {
		            Console.WriteLine(logContents);
	            }
            }
        }

        private void StreamReaderThread_Error() {
            StreamReader reader = _stdError;
        	
            while (true) {
                string logContents = reader.ReadLine();
                if (logContents == null) {
                    break;
                }
	            lock(consoleLock)
	            {
                    if (waitForStart)
                    {
                        if (logContents.EndsWith(options.SentenceForWait))
                        {
                            lock (waitForStartObject)
                            {
                                System.Threading.Monitor.Pulse(waitForStartObject);
                                waitForStart = false;
                            }
                        }
                    }
		            Console.Error.WriteLine(logContents);
	            }
            }
            if (waitForStart)
            {
                lock (waitForStartObject)
                {
                    System.Threading.Monitor.Pulse(waitForStartObject);
                    waitForStart = false;
                }
            }
        }

        private void AdminStreamReaderThread_Output() {
            StreamReader reader = _adminStdOut;
        	
            while (true) {
                string logContents = reader.ReadLine();
                if (logContents == null) {
                    break;
                }
	            lock(consoleLock)
	            {
		            Console.WriteLine(logContents);
	            }
            }
        }

        private void AdminStreamReaderThread_Error() {
            StreamReader reader = _adminStdError;
        	
            while (true) {
                string logContents = reader.ReadLine();
                if (logContents == null) {
                    break;
                }
	            lock(consoleLock)
	            {
		            Console.Error.WriteLine(logContents);
	            }
            }
        }

        /// <summary>
        /// Starts IceGrid and refreshes Modules manager
        /// </summary>
        /// <param name="icePath">A <see cref="T:System.String"/> representing
        /// path where IceGridnode.exe is</param>
        /// <param name="iceGridAsService">If true, will try to start IceGrid as Windows service</param>
        /// <param name="iceGridWorkingDirectory">The ice grid working directory.</param>
        /// <param name="iceGridApplicationXmlFilePath">The ice grid application XML file path.</param>
        public void StartIceGrid(string icePath, bool iceGridAsService, string iceGridWorkingDirectory, string iceGridApplicationXmlFilePath)
        {
            if (icePath == null) icePath = "";
            if (iceGridAsService) {
                process = new Process();
	            process.StartInfo.FileName = System.IO.Path.Combine(icePath,"icegridnode.exe");
	            process.StartInfo.Arguments = "--start FerdaIceGridNode";
	            process.StartInfo.RedirectStandardOutput = true;
	            process.StartInfo.RedirectStandardError = true;
	            process.StartInfo.RedirectStandardInput = true;
	            process.StartInfo.UseShellExecute = false;
	            process.StartInfo.CreateNoWindow = true;
	            process.StartInfo.WorkingDirectory = System.IO.Directory.GetCurrentDirectory();
	            try{
		            process.Start();
	            }
	            catch(System.ComponentModel.Win32Exception)
	            {
		            Debug.WriteLine("It was not possible to start IceGridnode");
	            }
	            outputThread = new Thread(new ThreadStart(StreamReaderThread_Output));
	            errorThread = new Thread(new ThreadStart(StreamReaderThread_Error));
	            _stdOut = process.StandardOutput;
	            _stdError = process.StandardError;
	            outputThread.Start();
	            errorThread.Start();
	            process.StandardInput.WriteLine("ferda");
	            process.StandardInput.WriteLine("ferda");
	            outputThread.Join();
	            errorThread.Join();
	            process.WaitForExit();
            }
            else
            {
                waitForStart = true;
	            process = new Process();
                process.StartInfo.FileName = System.IO.Path.Combine(icePath, "icegridnode");
                process.StartInfo.Arguments = "--Ice.Config=config --IceGrid.Registry.LMDB.Path=registry --IceGrid.Node.Data=node --deploy \"" + iceGridApplicationXmlFilePath + "\"";
	            process.StartInfo.RedirectStandardOutput = true;
	            process.StartInfo.RedirectStandardError = true;
							process.StartInfo.RedirectStandardInput = true;
	            process.StartInfo.UseShellExecute = false;
	            process.StartInfo.CreateNoWindow = true;
	            process.StartInfo.WorkingDirectory = iceGridWorkingDirectory;
	            process.Start();
	            outputThread = new Thread(new ThreadStart(StreamReaderThread_Output));
	            errorThread = new Thread(new ThreadStart(StreamReaderThread_Error));
	            _stdOut = process.StandardOutput;
	            _stdError = process.StandardError;
	            outputThread.Start();
	            errorThread.Start();
	            process.StandardInput.WriteLine("ferda");
	            process.StandardInput.WriteLine("ferda");
                lock (waitForStartObject)
                {
                    if (waitForStart)
                        System.Threading.Monitor.Wait(waitForStartObject);
                }
	            /*ProcessStartInfo psi = new ProcessStartInfo("IceGridnode", "--Ice.Config=config --warn");
	            psi.CreateNoWindow = true;
	            Process.Start(psi);
	            
	            System.Threading.Thread.Sleep(1000);
	            psi = new ProcessStartInfo("IceGridadmin", "--Ice.Config=config -e \"application add 'application.xml'\"");
	            psi.CreateNoWindow = true;
	            Process processReg = Process.Start(psi);
	            processReg.WaitForExit();*/
            }
            Debug.WriteLine("IceGrid started");
            if(this.modulesManager!=null) this.modulesManager.Refresh(null);
        }

        /// <summary>
        /// Stops IceGrid but don't do refresh on Modules manager.
        /// </summary>
        /// <param name="icePath">A <see cref="T:System.String"/> representing
        /// path where IceGridnode executable is</param>
        /// <param name="iceGridAsService">If true, will try to stop IceGrid as Windows service</param>
        public void StopIceGrid(string icePath, bool iceGridAsService)
        {
            Debug.WriteLine("stopping IceGrid...");
            if (icePath == null) icePath = "";
            if (iceGridAsService)
            {
                Process process = new Process();
                process.StartInfo.FileName = System.IO.Path.Combine(icePath, "icegridnode.exe");
                process.StartInfo.Arguments = "--stop FerdaIceGridNode";
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.WorkingDirectory = System.IO.Directory.GetCurrentDirectory();
                try
                {
                    process.Start();
                }
                catch (System.ComponentModel.Win32Exception)
                {
                    Debug.WriteLine("It was not possible to stop IceGridnode");
                }
                outputThread = new Thread(new ThreadStart(AdminStreamReaderThread_Output));
                errorThread = new Thread(new ThreadStart(AdminStreamReaderThread_Error));
                _adminStdOut = process.StandardOutput;
                _adminStdError = process.StandardError;
                outputThread.Start();
                errorThread.Start();
                outputThread.Join();
                errorThread.Join();
                process.WaitForExit();
            }
            else
            {
                /*Process adminProcess = new Process();
                adminProcess.StartInfo.FileName = System.IO.Path.Combine(icePath, "icegridadmin.exe");
                adminProcess.StartInfo.Arguments = "--Ice.Config=config -e shutdown";
                adminProcess.StartInfo.RedirectStandardOutput = true;
                adminProcess.StartInfo.RedirectStandardError = true;
                adminProcess.StartInfo.UseShellExecute = false;
                adminProcess.StartInfo.CreateNoWindow = true;
                adminProcess.StartInfo.WorkingDirectory = System.IO.Directory.GetCurrentDirectory();
                Debug.WriteLine("starting admin...");
                adminProcess.Start();
                Thread adminOutputThread = new Thread(new ThreadStart(AdminStreamReaderThread_Output));
                Thread adminErrorThread = new Thread(new ThreadStart(AdminStreamReaderThread_Error));
                _adminStdOut = adminProcess.StandardOutput;
                _adminStdError = adminProcess.StandardError;
                adminOutputThread.Start();
                adminErrorThread.Start();
                adminOutputThread.Join();
                adminErrorThread.Join();
                Debug.WriteLine("waiting for exit admin...");
     a/ferda/src/ProjectManager/bin/Debug           adminProcess.WaitForExit();
                Debug.WriteLine("admin stoped...");*/
                
                RegistryPrx registryPrx = IceGrid.RegistryPrxHelper.checkedCast(
                    this.ModulesManager.Helper.ObjectAdapter.getCommunicator().stringToProxy("IceGrid/Registry"));
				AdminSessionPrx adminSessionPrx = registryPrx.createAdminSession( "ferda", "ferda");
				AdminPrx adminPrx = adminSessionPrx.getAdmin();
                /*
                AdminPrx adminPrx = IceGrid.AdminPrxHelper.checkedCast(
                    this.ModulesManager.Helper.ObjectAdapter.getCommunicator().stringToProxy("IceGrid/Admin"));*/
                Debug.WriteLine("stopping server...");
                //TODO this is better to make as parameter
                adminPrx.stopServer("0");
                Debug.WriteLine("removing applicationg...");
                adminPrx.removeApplication("FerdaModulesApplication");
                Debug.WriteLine("executing IceGrid shutdown...");
                adminPrx.shutdown();
                //Debug.WriteLine("Killing IceGridNode process...");
                //process.Kill();
                Debug.WriteLine("joining thread output...");
                outputThread.Join();
                Debug.WriteLine("joining thread error...");
                errorThread.Join();
                Debug.WriteLine("joining IceGridnode...");
                process.WaitForExit();
                Debug.WriteLine("IceGrid stoped...");
            }
        }

        /// <summary>
        /// Stop IceGrid server with identifier <paramref name="serverId"/>
        /// </summary>
        /// <param name="serverId">A <see cref="T:System.String"/> representing
        /// identifier of IceGrid server</param>
        /// <param name="icePath">Path to icegridadmin executable</param>
        public void StopIceGridServer(string serverId, string icePath)
        {
            Debug.WriteLine("stopping IceGrid server " + serverId + "...");
            Process process = new Process();
            if (icePath == null) icePath = "";
            process.StartInfo.FileName = System.IO.Path.Combine(icePath, "icegridadmin.exe");
            process.StartInfo.Arguments = "--Ice.Config=config -e \"server stop " + serverId + "\"";
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.WorkingDirectory = System.IO.Directory.GetCurrentDirectory();
            try
            {
                process.Start();
            }
            catch (System.ComponentModel.Win32Exception)
            {
                Debug.WriteLine("It was not possible to stop icegrid server " + serverId);
            }
            outputThread = new Thread(new ThreadStart(AdminStreamReaderThread_Output));
            errorThread = new Thread(new ThreadStart(AdminStreamReaderThread_Error));
            _adminStdOut = process.StandardOutput;
            _adminStdError = process.StandardError;
            outputThread.Start();
            errorThread.Start();
            outputThread.Join();
            errorThread.Join();
            process.WaitForExit();
        }

        /// <summary>
        /// Destroys project manager.
        /// </summary>
        /// <remarks>
        /// This method is needed to be executed on end of working with Project manager.
        /// It destroys Modules manager and stops IceGrid if it was executed by Project
        /// Manager.
        /// </remarks>
        public void DestroyProjectManager()
        {
            this.modulesManager.DestroyModulesManagersObjects();
            if (options.ServersToStop != null)
            {
                foreach (string server in options.ServersToStop)
                    StopIceGridServer(server, options.IceBinPath);
            }
            if (options.StopIceGridLocaly) this.StopIceGrid(options.IceBinPath, options.IceGridAsService);
            this.modulesManager.DestroyModulesManagersCommunicator();
        }

        /// <summary>
        /// Loads project from stream.
        /// </summary>
        /// <param name="stream">A System.IO.Stream representing stream with XML
        /// file with project information</param>
        /// <returns>A string that contains loading errors</returns>
        /// <seealso cref="M:Ferda.ProjectManager.ProjectManager.SaveProject(System.IO.Stream)"/>
        /// <seealso cref="M:Ferda.ProjectManager.ProjectManager.NewProject()"/>
        public string LoadProject(System.IO.Stream stream)
        {
            this.NewProject();
        	return ImportProject(stream);
        }
        
		/// <summary>
		/// Import Project from a stream
		/// </summary>
		/// <param name="stream">A System.IO.Stream representing stream with XML
        /// file with project information</param>
        /// <returns>A string that contains loading errors</returns>
        public string ImportProject(System.IO.Stream stream)
        {
        	XmlSerializer s = new XmlSerializer( typeof( Project ) );
            TextReader r = new StreamReader(stream);
            Project p = (Project)s.Deserialize( r );
            r.Close();
            
            Ferda.ModulesManager.IBoxModule mainIBoxModule;
            
            return ImportProject(p, null, true, out mainIBoxModule);
        }
        
        public string ImportProject(Project p, Project.Box mainProjectBox, bool addToProject, out Ferda.ModulesManager.IBoxModule mainIBoxModule)
        {
        	//TODO localize errors
        	string errors = "";
        	Dictionary<int,IBoxModule> boxesByProjectIdentifier =
	            new Dictionary<int,IBoxModule>();
            List<int> notLoadedBoxes = new List<int>();
            mainIBoxModule = null;
            int lastBoxModuleProjectIdentifier = archive.LastBoxModuleProjectIdentifier;
            foreach(Project.Box b in p.Boxes)
            {
                IBoxModuleFactoryCreator creator =
                    this.modulesManager.GetBoxModuleFactoryCreator(b.CreatorIdentifier);
                if (boxesByProjectIdentifier.ContainsKey(b.ProjectIdentifier))
				{
                    errors += "There are more boxes with project identifier " + b.ProjectIdentifier + ".\n";
                    notLoadedBoxes.Add(b.ProjectIdentifier);
                }
				else if (creator == null)
                {
                    errors += "Box with identifier " + b.CreatorIdentifier + " does not exist.\n";
                    notLoadedBoxes.Add(b.ProjectIdentifier);
                }
                else
                {
                    IBoxModule box = creator.CreateBoxModule();
                    if (b == mainProjectBox)
                    	mainIBoxModule = box;
                    if (b.UserName != null)
                        box.UserName = b.UserName;
                    box.UserHint = b.UserHint;
                    foreach (Project.Box.PropertySet propertySet in b.PropertySets)
                    {
                        try
                        {
                            box.SetPropertyOther(propertySet.PropertyName,
                                             propertySet.Value.GetPropertyValue());
                        }
                        catch (Ferda.Modules.NameNotExistError)
                        {
                            errors += "Your box with identifier " + b.CreatorIdentifier +
                                " does not have property with name " +
                                propertySet.PropertyName + ".\n";
                        }
                        catch (Ferda.Modules.BadTypeError)
                        {
                            errors += "Your box with identifier " + b.CreatorIdentifier +
                                " have property with name " +
                                propertySet.PropertyName + " in other type than needed.\n";
                        }
                        catch (Ferda.Modules.BadValueError)
                        {
                            errors += "Your box with identifier " + b.CreatorIdentifier +
                                " have property with name " +
                                propertySet.PropertyName + " which does not allow value in needed range.\n";
                        }
                        catch (Ferda.Modules.ReadOnlyError)
                        {
                            errors += "Your box with identifier " + b.CreatorIdentifier +
                                " have property with name " +
                                propertySet.PropertyName + " readonly but I have to write something inside.\n";
                        }
                    }
                    
                    StringCollection propertyNames = new StringCollection();
	               	foreach(Ferda.Modules.PropertyInfo propertyInfo in box.MadeInCreator.Properties)
	               	{
	               		propertyNames.Add(propertyInfo.name);
	               	}
	               	if(b.SockedProperties != null)
	               	{
	                    foreach(string propertyName in b.SockedProperties)
	                    {
	                    	if(propertyNames.Contains(propertyName))
	                    	{
	                    		box.SetPropertySocking(propertyName, true);
	                    	}
	                    	else
	                    	{
	                    		errors += "Your box with identifier " + b.CreatorIdentifier +
	                                " does not have property with name " +
	                                propertyName + ".\n";
	                    	}
	                    }
	                }
                    boxesByProjectIdentifier[b.ProjectIdentifier] = box;
                    if (addToProject)
                    {
                    	archive.AddWithIdentifier(box, b.ProjectIdentifier + lastBoxModuleProjectIdentifier);
                    }
                }
            }
            foreach(Project.Box b in p.Boxes)
            {
                if (!notLoadedBoxes.Contains(b.ProjectIdentifier))
                {
                    foreach (Project.Box.Connection connection in b.Connections)
                    {
                        if (!notLoadedBoxes.Contains(connection.BoxProjectIdentifier))
                        {
                            try
                            {
                                boxesByProjectIdentifier[b.ProjectIdentifier].SetConnection(
                                    connection.SocketName,
                                    boxesByProjectIdentifier[connection.BoxProjectIdentifier]);
                            }
                            catch (Ferda.Modules.NameNotExistError)
                            {
                                errors += "Box with project identifier " + b.ProjectIdentifier +
                                        " does not have socket with name " +
                                        connection.SocketName + ".\n";
                            }
                            catch (Ferda.Modules.BadTypeError)
                            {
                                errors += "Box with project identifier " + connection.BoxProjectIdentifier +
                                        " can not be connected to box with project identifier " +
                                        b.ProjectIdentifier + " to socket with name " +
                                        connection.SocketName + " because it is of bad type.\n";
                            }
                            catch (Ferda.Modules.ConnectionExistsError)
                            {
                                errors += "It is not possible to connect more boxes to box with project identifier" +
                                    b.ProjectIdentifier + " to socket " +
                                    connection.SocketName + ".\n";
                            }
                        }
                    }
                }
            }
            foreach(Project.View viewInfo in p.Views)
            {
	            View view = this.NewView(viewInfo.Name);
	            foreach(Project.View.PositionSet positionSet in viewInfo.PositionSets)
	            {
                    if (!notLoadedBoxes.Contains(positionSet.BoxProjectIdentifier))
                    {
		                view.Add(boxesByProjectIdentifier[positionSet.BoxProjectIdentifier],
				             positionSet.Position);
                    }
	            }
            }
            return errors;
        }

        /// <summary>
        /// Saves project from stream.
        /// </summary>
        /// <param name="stream">A System.IO.Stream representing stream with XML
        /// file with project information</param>
        /// <seealso cref="M:Ferda.ProjectManager.ProjectManager.LoadProject(System.IO.Stream)"/>
        /// <seealso cref="M:Ferda.ProjectManager.ProjectManager.NewProject()"/>
        public void SaveProject(System.IO.Stream stream)
        {
            XmlSerializer s = new XmlSerializer( typeof( Project ) );
            TextWriter w = new StreamWriter( stream );
            Project p = new Project();
        	
            List<Project.Box> boxes = new List<Project.Box>();
        	
            foreach(IBoxModule box in this.Archive.Boxes)
            {
	            Project.Box b = new Project.Box();
	            b.ProjectIdentifier = box.ProjectIdentifier;
	            b.CreatorIdentifier = box.MadeInCreator.Identifier;
	            b.UserHint = box.UserHint;
	            if(box.UserNameSet)
	            {
		            b.UserName = box.UserName;
	            }
	            else
	            {
		            b.UserName = null;
	            }
	            List<Project.Box.Connection> connections =
		            new List<Project.Box.Connection>();
	            foreach(Ferda.Modules.SocketInfo socketInfo in box.Sockets)
	            {
		            foreach(IBoxModule otherBox in box.GetConnections(socketInfo.name))
		            {
			            Project.Box.Connection connection = new Project.Box.Connection();
			            connection.SocketName = socketInfo.name;
			            connection.BoxProjectIdentifier = otherBox.ProjectIdentifier;
			            connections.Add(connection);
		            }
	            }
	            b.Connections = connections.ToArray();
	            List<Project.Box.PropertySet> propertySets =
		            new List<Project.Box.PropertySet>();
		        List<string> sockedProperties = new List<string>();
	            foreach(Ferda.Modules.PropertyInfo propertyInfo in box.MadeInCreator.Properties)
	            {
		            if(!propertyInfo.readOnly)
		            {
		            	if (box.GetPropertySocking(propertyInfo.name))
		            	{
		            		sockedProperties.Add(propertyInfo.name);
		            	}
		            	else
		            	{
				            Project.Box.PropertySet propertySet =
					            new Project.Box.PropertySet();
				            propertySet.PropertyName = propertyInfo.name;
				            Ferda.Modules.PropertyValue value =
					            box.GetPropertyOther(propertyInfo.name);
				            propertySet.Value = ((Ferda.Modules.IValue)value).getValueT();
				            propertySets.Add(propertySet);
				        }
		            }
	            }
	            b.PropertySets = propertySets.ToArray();
	            b.SockedProperties = sockedProperties.ToArray();
	            boxes.Add(b);
            }
            p.Boxes = boxes.ToArray();
        	
            List<Project.View> views = new List<Project.View>();
            foreach(View viewInfo in this.Views)
            {
	            Project.View view = new Project.View();
	            view.Name = viewInfo.Name;
	            List<Project.View.PositionSet> positionSets = new List<Project.View.PositionSet>();
	            foreach(IBoxModule box in viewInfo.Boxes)
	            {
		            Project.View.PositionSet positionSet =
			            new Project.View.PositionSet();
		            positionSet.BoxProjectIdentifier = box.ProjectIdentifier;
		            positionSet.Position = viewInfo.GetPosition(box);
		            positionSets.Add(positionSet);
	            }
	            view.PositionSets = positionSets.ToArray();
	            views.Add(view);
            }
            p.Views = views.ToArray();
        	
            s.Serialize( w, p );
            w.Close();
        }

		/// <summary>
		/// Creates Project object from box modules
		/// </summary>
		public Project SaveBoxModulesToProject(IBoxModule[] boxModules, View view, IBoxModule[][] variables, IBoxModule[] variableValues)
		{
			if(view == null)
				view = new View(archive, modulesManager, null);
			
			Dictionary<IBoxModule, IBoxModule> changing = new Dictionary<IBoxModule,IBoxModule>();
			if(variables != null)
			{
				for(int i = 0; i < variables.Length; i++)
				{
					for(int j = 0; j < variables[i].Length; j++)
					{
						if(variables[i][j] != null && variableValues[i] != null)
						{
							changing[variables[i][j]] = variableValues[i];
						}
					}
				}
			}

			List<IBoxModule> usedBoxModules = new List<IBoxModule>();
			Queue<IBoxModule> modulesToGoTrought = new Queue<IBoxModule>();
			foreach(IBoxModule box in boxModules)
			{
				IBoxModule boxNew = null;
				if(!changing.TryGetValue(box, out boxNew))
				{
					boxNew = box;
				}
				modulesToGoTrought.Enqueue(boxNew);
			}
			List<Project.Box> resultBoxes = new List<Project.Box>();
			Project resultProject = new Project();
			
			int leastProjectIdentifier = 0;
			
			foreach(IBoxModule boxModule in modulesToGoTrought)
			{
				if (boxModule.ProjectIdentifier <= 0)
				{
					boxModule.ProjectIdentifier = --leastProjectIdentifier;
				}
			}
			
			while(modulesToGoTrought.Count > 0)
			{
				IBoxModule boxModule = modulesToGoTrought.Peek();
				
				resultBoxes.Add(SaveBoxModule(boxModule, view, modulesToGoTrought, usedBoxModules, ref leastProjectIdentifier, changing));
				usedBoxModules.Add(boxModule);
				modulesToGoTrought.Dequeue();
			}
			
			
			resultProject.Boxes = resultBoxes.ToArray();
			resultProject.Views = new Project.View[0];
			return resultProject;
		}
		
		private Project.Box SaveBoxModule(IBoxModule box, View view, Queue<IBoxModule> modulesToGoTrought, List<IBoxModule> usedBoxModules, ref int leastProjectIdentifier, Dictionary<IBoxModule, IBoxModule> changing)
		{
			Project.Box b = new Project.Box();
			b.ProjectIdentifier = box.ProjectIdentifier;
			b.CreatorIdentifier = box.MadeInCreator.Identifier;
			b.UserHint = box.UserHint;
			if(box.UserNameSet)
			{
				b.UserName = box.UserName;
			}
			else
			{
				b.UserName = null;
			}
			List<Project.Box.Connection> connections =
				new List<Project.Box.Connection>();
			foreach(Ferda.Modules.SocketInfo socketInfo in box.Sockets)
			{
				foreach(IBoxModule otherBoxBefore in box.GetConnections(socketInfo.name))
				{
					IBoxModule otherBox = null;
					if(!changing.TryGetValue(otherBoxBefore, out otherBox))
					{
						otherBox = otherBoxBefore;
					}
					
					if(!(modulesToGoTrought.Contains(otherBox) ||
					   usedBoxModules.Contains(otherBox)))
					{
						if(view.ContainsBox(otherBox))
						{
							break;
						}
						else
						{
							modulesToGoTrought.Enqueue(otherBox);
							if (otherBox.ProjectIdentifier <= 0)
							{
								otherBox.ProjectIdentifier = --leastProjectIdentifier;
							}
						}
					}

					Project.Box.Connection connection = new Project.Box.Connection();
					connection.SocketName = socketInfo.name;
					connection.BoxProjectIdentifier = otherBox.ProjectIdentifier;
					connections.Add(connection);
				}
			}
			b.Connections = connections.ToArray();
			List<Project.Box.PropertySet> propertySets =
				new List<Project.Box.PropertySet>();
			List<string> sockedProperties = new List<string>();
			foreach(Ferda.Modules.PropertyInfo propertyInfo in box.MadeInCreator.Properties)
			{
				if(!propertyInfo.readOnly)
				{
					if (box.GetPropertySocking(propertyInfo.name))
	            	{
	            		sockedProperties.Add(propertyInfo.name);
	            	}
	            	else
	            	{
						Project.Box.PropertySet propertySet =
							new Project.Box.PropertySet();
						propertySet.PropertyName = propertyInfo.name;
						Ferda.Modules.PropertyValue value =
							box.GetPropertyOther(propertyInfo.name);
						propertySet.Value = ((Ferda.Modules.IValue)value).getValueT();
						propertySets.Add(propertySet);
					}
				}
			}
			b.PropertySets = propertySets.ToArray();
			b.SockedProperties = sockedProperties.ToArray();
			return b;
		}
        
        

        /// <summary>
        /// Creates new project.
        /// </summary>
        /// <remarks>
        /// It will lose old project.
        /// </remarks>
        /// <seealso cref="M:Ferda.ProjectManager.ProjectManager.LoadProject(System.IO.Stream)"/>
        /// <seealso cref="M:Ferda.ProjectManager.ProjectManager.SaveProject(System.IO.Stream)"/>
        public void NewProject()
        {
            archive.Destroy();
            views = new List<View>();
            archive = new Archive(views);
        }
        
        public IBoxModule CloneBoxModuleWithChilds(IBoxModule boxModule, bool addToProject, IBoxModule[][] variables, IBoxModule[] variableValues)
        {
            //XmlSerializer s = new XmlSerializer( typeof( Project ) );
            //TextWriter w = new StreamWriter("test.p1");
            
        	Project projectOld = SaveBoxModulesToProject(new IBoxModule[]{boxModule}, null, variables, variableValues);
        	
            //s.Serialize( w, projectOld );
            //w.Close();
            
			Ferda.NetworkArchive.Box box = Ferda.NetworkArchive.ProjectConverter.CreateBoxFromProject(projectOld, projectOld.Boxes[0].ProjectIdentifier);
			
			Project.Box mainProjectBox;
			Ferda.ModulesManager.IBoxModule clonedBox;
			
            //TextWriter w2 = new StreamWriter("test.p2");
			Project project = Ferda.NetworkArchive.ProjectConverter.CreateProjectFromBox(box, out mainProjectBox);
            //s.Serialize( w2, project );
            //w2.Close();
            
			ImportProject(project, mainProjectBox, addToProject, out clonedBox);
			return clonedBox;
        }

        /// <summary>
        /// Gets <see cref="T:Ferda.ProjectManager.View"/> with name <paramref name="name"/>.
        /// </summary>
        /// <returns>A <see cref="T:Ferda.ProjectManager.View"/> representing
        /// view with name <paramref name="name"/></returns>
        /// <param name="name">A string representing name of
        /// <see cref="T:Ferda.ProjectManager.View"/></param>
        /// <seealso cref="P:Ferda.ProjectManager.ProjectManager.Views"/>
        /// <seealso cref="M:Ferda.ProjectManager.ProjectManager.NewView(System.String)"/>
        /// <seealso cref="M:Ferda.ProjectManager.ProjectManager.RemoveView(System.String)"/>
        public View GetView(string name) {
            foreach(View v in views)
            {
	            if(v.Name == name)
		            return v;
            }
            throw new KeyNotFoundException();
        }

        /// <summary>
        /// Creates new <see cref="T:Ferda.ProjectManager.View"/> with name <paramref name="name"/>.
        /// </summary>
        /// <returns>A <see cref="T:Ferda.ProjectManager.View"/> representing
        /// view with name <paramref name="name"/></returns>
        /// <param name="name">A string representing name of
        /// <see cref="T:Ferda.ProjectManager.View"/></param>
        /// <seealso cref="P:Ferda.ProjectManager.ProjectManager.Views"/>
        /// <seealso cref="M:Ferda.ProjectManager.ProjectManager.GetView(System.String)"/>
        /// <seealso cref="M:Ferda.ProjectManager.ProjectManager.RemoveView(System.String)"/>
        public View NewView(string name)
        {
            View result = new View(archive,modulesManager,name);
            views.Add(result);
            return result;
        }

        /// <summary>
        /// Removes <see cref="T:Ferda.ProjectManager.View"/> with name
        /// <paramref name="name"/> from project.
        /// </summary>
        /// <param name="name">A string representing name of
        /// <see cref="T:Ferda.ProjectManager.View"/></param>
        /// <seealso cref="P:Ferda.ProjectManager.ProjectManager.Views"/>
        /// <seealso cref="M:Ferda.ProjectManager.ProjectManager.GetView(System.String)"/>
        /// <seealso cref="M:Ferda.ProjectManager.ProjectManager.NewView(System.String)"/>
        public void RemoveView(string name)
        {
            View v = this.GetView(name);
            views.Remove(v);
        }

        /// <summary>
        /// Views in project.
        /// </summary>
        /// <value>
        /// An array of <see cref="T:Ferda.ProjectManager.View"/> in project.
        /// </value>
        /// <seealso cref="M:Ferda.ProjectManager.ProjectManager.GetView(System.String)"/>
        /// <seealso cref="M:Ferda.ProjectManager.ProjectManager.NewView(System.String)"/>
        /// <seealso cref="M:Ferda.ProjectManager.ProjectManager.RemoveView(System.String)"/>
        public View[] Views
        {
            get {
	            return views.ToArray();
            }
        }

        /// <summary>
        /// Archive of boxes in project.
        /// </summary>
        /// <value>
        /// An <see cref="T:Ferda.ProjectManager.Archive"/>
        /// </value>
        public Archive Archive
        {
            get {
	            return archive;
            }
        }

        /// <summary>
        /// Modules manager with which works this project manager.
        /// </summary>
        /// <remarks>
        /// Use this Modules manager for getting
        /// <see cref="T:Ferda.ModulesManager.IBoxModuleFactoryCreator"/> classes
        /// which represents creators of boxes.
        /// </remarks>
        /// <value>
        /// An <see cref="T:Ferda.ModulesManager.ModulesManager"/>
        /// </value>
        public Ferda.ModulesManager.ModulesManager ModulesManager
        {
            get {
	            return modulesManager;
            }
        }
        
		/// <summary>
		/// Network archive
		/// </summary>
        public Ferda.ProjectManager.NetworkArchive NetworkArchive
        {
        	get {
        		return networkArchive;
        	}
        }
    }
}
