using System;
using System.Collections.Generic;
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
    public class ProjectManager {
		private List<View> views = new List<View>();
		private Archive archive;
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
			if(options.StartIceGridLocaly) StartIceGrid(options.IceBinPath, options.IceGridAsService);
			modulesManager = new Ferda.ModulesManager.ModulesManager(args, options.LocalePrefs);
			archive = new Archive(views);
		}
		
		/// <summary>
		/// Constructs project manager. You can choose your own OutputPrx
		/// </summary>
		/// <param name="args">A string[] representing arguments from command line.</param>
        /// <param name="options">A <see cref="T:Ferda.ProjectManager.ProjectManagerOptions"/>
        /// representing options for ProjectManaget</param>
		/// <param name="output">A <see cref="T:Ferda.ModulesManager.Output"/> representing
		/// interface for modules for writing informations to user</param>
		/// Modules manager</param>
        public ProjectManager(string[] args, ProjectManagerOptions options, Ferda.ModulesManager.Output output)
		{
            this.options = options;
            if (options.StartIceGridLocaly) StartIceGrid(options.IceBinPath, options.IceGridAsService);
			modulesManager = new Ferda.ModulesManager.ModulesManager(args, options.LocalePrefs, output);
			archive = new Archive(views);
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
                        if (logContents.Equals(options.SentenceForWait))
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
        public void StartIceGrid(string icePath, bool iceGridAsService)
		{
            if (iceGridAsService) {
                process = new Process();
				if(icePath==null) icePath = "";
				process.StartInfo.FileName = System.IO.Path.Combine(icePath,"IceGridnode.exe");
				process.StartInfo.Arguments = "--start FerdaIceGridNode";
				process.StartInfo.RedirectStandardOutput = true;
				process.StartInfo.RedirectStandardError = true;
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
				outputThread.Join();
				errorThread.Join();
				process.WaitForExit();
            }
			else
			{
                waitForStart = true;
				process = new Process();
				process.StartInfo.FileName = "icegridnode";
                process.StartInfo.Arguments = "--Ice.Config=config --IceGrid.Registry.Data=registry --IceGrid.Node.Data=node --deploy application.xml";
				process.StartInfo.RedirectStandardOutput = true;
				process.StartInfo.RedirectStandardError = true;
				process.StartInfo.UseShellExecute = false;
				process.StartInfo.CreateNoWindow = true;
				process.StartInfo.WorkingDirectory = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(),"../db");
				process.Start();
				outputThread = new Thread(new ThreadStart(StreamReaderThread_Output));
				errorThread = new Thread(new ThreadStart(StreamReaderThread_Error));
				_stdOut = process.StandardOutput;
				_stdError = process.StandardError;
				outputThread.Start();
				errorThread.Start();
                lock (waitForStartObject)
                {
                    if (waitForStart)
                        System.Threading.Monitor.Wait(waitForStartObject);
                }
				/*ProcessStartInfo psi = new ProcessStartInfo("IceGridnode", "--Ice.Config=config --warn");
				psi.CreateNoWindow = true;
				Process.Start(psi);
				//TODO toto nedelat a dat deploy do nantu
				System.Threading.Thread.Sleep(1000);
				psi = new ProcessStartInfo("IceGridadmin", "--Ice.Config=config -e \"application add 'application.xml'\"");
				psi.CreateNoWindow = true;
				Process processReg = Process.Start(psi);
				processReg.WaitForExit();*/
			}
			Debug.WriteLine("IceGrid started");
			if(this.modulesManager!=null) this.modulesManager.Refresh();
		}

		/// <summary>
		/// Stops IceGrid but don't do refresh on Modules manager.
		/// </summary>
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
                adminProcess.WaitForExit();
                Debug.WriteLine("admin stoped...");*/
                
                AdminPrx adminPrx = IceGrid.AdminPrxHelper.checkedCast(
                    this.ModulesManager.Helper.ObjectAdapter.getCommunicator().stringToProxy("IceGrid/Admin"));
                Debug.WriteLine("stopping server...");
                //TODO dat jako parametr
                adminPrx.stopServer("0");
                Debug.WriteLine("removing applicationg...");
                adminPrx.removeApplication("FerdaModulesApplication");
                Debug.WriteLine("executing IceGrid shutdown...");
                adminPrx.shutdown();
                Debug.WriteLine("joining thread output...");
                outputThread.Join();
                Debug.WriteLine("joining thread error...");
                errorThread.Join();
                Debug.WriteLine("joining IceGridnode...");
                process.WaitForExit();
                Debug.WriteLine("IceGrid stoped...");
            }
		}

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
            //TODO: localize errors
            string errors = "";
			this.NewProject();
			
			XmlSerializer s = new XmlSerializer( typeof( Project ) );
			TextReader r = new StreamReader(stream);
			Project p = (Project)s.Deserialize( r );
			r.Close();
			
			Dictionary<int,IBoxModule> boxesByProjectIdentifier =
				new Dictionary<int,IBoxModule>();
            List<int> notLoadedBoxes = new List<int>();
			foreach(Project.Box b in p.Boxes)
			{
                IBoxModuleFactoryCreator creator =
                    this.modulesManager.GetBoxModuleFactoryCreator(b.CreatorIdentifier);
                if (creator == null)
                {
                    errors += "Box with identifier " + b.CreatorIdentifier + " does not exist.\n";
                    notLoadedBoxes.Add(b.ProjectIdentifier);
                }
                else
                {
                    IBoxModule box = creator.CreateBoxModule();
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
                    boxesByProjectIdentifier[b.ProjectIdentifier] = box;
                    archive.AddWithIdentifier(box, b.ProjectIdentifier);
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
		/// Savess project from stream.
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
				foreach(Ferda.Modules.PropertyInfo propertyInfo in box.MadeInCreator.Properties)
				{
					if(!propertyInfo.readOnly)
					{
						Project.Box.PropertySet propertySet =
							new Project.Box.PropertySet();
						propertySet.PropertyName = propertyInfo.name;
						Ferda.Modules.PropertyValue value =	 
							box.GetPropertyOther(propertyInfo.name);
						string pomocna = value.GetType().ToString();
						propertySet.Value = ((Ferda.Modules.IValue)value).getValueT();
						propertySets.Add(propertySet);
					}
				}
				b.PropertySets = propertySets.ToArray();
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
    }

}
