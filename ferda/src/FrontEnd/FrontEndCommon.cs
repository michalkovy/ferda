// FrontEndCommon.cs - Common stuff to all FrontEnd
//
// Author: Martin Ralbovský <martin.ralbovsky@gmail.com>
//
// Copyright (c) 2005 Martin Ralbovský
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
using System.Text;
using Ferda.ModulesManager;
using System.Resources;
using System.Windows.Forms;
using System.IO;
using Ferda.ProjectManager;
using System.Reflection;
using Microsoft.Win32;
using Ferda.Modules;

namespace Ferda.FrontEnd
{
    /// <summary>
    /// Class that provides the functionality that is in common to more classes in
    /// Ferda's FrontEnd
    /// </summary>
    public class FrontEndCommon
    {
        /// <summary>
        /// Shows a messagebox saying that user cannot write to the box
        /// </summary>
        /// <param name="box">Box where we cannot write</param>
        /// <param name="resManager">Resource Manager of the application</param>
        public static void CannotWriteToBox(IBoxModule box, ResourceManager resManager)
        {
            MessageBox.Show(
                resManager.GetString("DesktopCannotWriteText"),
                box.UserName + ": " + resManager.GetString("DesktopCannotWriteCaption"));
        }

        /// <summary>
        /// Shows a messagebox saying that property setting of more boxes has failed
        /// </summary>
        /// <param name="resManager">Resource Manager of the application</param>
        public static void CannotSetPropertyMoreBoxes(ResourceManager resManager)
        {
            MessageBox.Show(resManager.GetString("PropertyMoreBoxesErrorCaption"),
                resManager.GetString("PropertyMoreBoxesErrorText"));
        }

        /// <summary>
        /// Loads the project and writes the information about the project File
        /// </summary>
        /// <param name="fileName">Name of the xpf file from where the project is loaded</param>
        /// <param name="parent">Control which cursor should be changed to wait cursor</param>
        /// <param name="resManager">Resource Manager of the application</param>
        /// <param name="projectManager">Project Manager class</param>
        /// <param name="controlsManager">Controls manager that takes care of the controls</param>
        public static void LoadProject(string fileName, Control parent,
            ResourceManager resManager, ref ProjectManager.ProjectManager projectManager,
            IControlsManager controlsManager)
        {
            string loadErrors = string.Empty;
            if (fileName != "")
            {
                FileStream fs = null;
                try
                {
                    fs = new System.IO.FileStream(fileName, System.IO.FileMode.Open);
                }
                catch (FileNotFoundException)
                {
                    MessageBox.Show(resManager.GetString("ProjectLoadFileNotFound"),
                        resManager.GetString("ProjectLoadErrorCaption"),
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                try
                {
                    Cursor previosCursor = parent.Cursor;
                    parent.Cursor = Cursors.WaitCursor;
                    parent.Refresh();
                    loadErrors = projectManager.LoadProject(fs);
                    controlsManager.ProjectName = fileName;
                    controlsManager.GlobalAdapt();
                    parent.Cursor = previosCursor;
                }
                catch (InvalidOperationException)
                {
                    MessageBox.Show(resManager.GetString("ProjectLoadInvalidFormat"),
                        resManager.GetString("ProjectLoadErrorCaption"),
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                finally
                {
                    fs.Close();
                }
            }

            //displaying what has happened while loading the project
            if (loadErrors != string.Empty)
            {
                MessageBox.Show(loadErrors,
                    resManager.GetString("ProjectLoadErrorCaption"),
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Switches the application to the directory in the parameter
        /// </summary>
        /// <param name="previousDir">A path of the directory where
        /// the application should be returned</param>
        public static void SwitchToPreviousDirectory(string previousDir)
        {
            Directory.SetCurrentDirectory(previousDir);
        }

        /// <summary>
        /// Switches to the directory where all the resources are
        /// placed
        /// </summary>
        /// <returns>A string containing the path of the previous
        /// directory</returns>
        public static string SwitchToBinDirectory()
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            string assemblyDir = GetBinPath();
            Directory.SetCurrentDirectory(assemblyDir);
            return currentDirectory;
        }

        /// <summary>
        /// Function returns the directory where the FerdaFrontEnd.exe file
        /// (current assembly) is located
        /// </summary>
        /// <returns></returns>
        public static string GetBinPath()
        {
            //getting where directory, where the assembly resides (FerdaFrontEnd.exe)    
            //getting the full location fo the .exe file
            string assemblyDir = Assembly.GetExecutingAssembly().Location;
	    System.IO.FileInfo fileInfo = new System.IO.FileInfo(assemblyDir);
	    assemblyDir = fileInfo.Directory.ToString();
            //replacing the .exe by the name of the config file
            //int lastDash = assemblyDir.LastIndexOf('\\');
            //assemblyDir = assemblyDir.Remove(lastDash);

            return assemblyDir;
        }

        /// <summary>
        /// Opens a pdf that is located in the path in the parameter
        /// </summary>
        /// <param name="path">Path to the pdf file</param>
        /// <param name="resManager">Manager that holds resources</param>
        public static void OpenPdf(string path, ResourceManager resManager)
        {
            string AcroPath = FindAcroReaderPath(resManager);

            if (AcroPath != string.Empty)
            {
                System.Diagnostics.Process.Start(AcroPath, path);
            }
        }

        /// <summary>
        /// Opens a pdf that is located in the path in the parameter
        /// and jumps to 
        /// </summary>
        /// <param name="path">Path to the pdf file</param>
        /// <param name="destination">named destination in the pdf where
        /// the Acrobat reader should jump</param>
        /// <param name="resManager">Manager that holds resources</param>
        public static void OpenPdfAndDestination(string path, string destination,
            ResourceManager resManager)
        {
            string AcroPath = FindAcroReaderPath(resManager);
            string parameter = "/A \"namedest=" + destination +
                "\"" + path + "\"";

            if (AcroPath != string.Empty)
            {
                System.Diagnostics.Process.Start(AcroPath, parameter);
            }
        }

        /// <summary>
        /// Finds a path to Acrobat Reader (must be version 6 or higher)
        /// </summary>
        /// <param name="resManager">Manager of the resources</param>
        /// <returns>Path to the Acrobat Reader program</returns>
        protected static string FindAcroReaderPath(ResourceManager resManager)
        {
            //getting the root registry of the local machine
            RegistryKey rk = Registry.LocalMachine;
            //getting the software key
            RegistryKey software = rk.OpenSubKey("Software");
            if (software == null)
            {
                throw new ApplicationException(
                    "Software registry key not provided - something is wrong");
            }

            //opening the Adobe folder
            RegistryKey adobe = software.OpenSubKey("Adobe");
            if (adobe == null)
            {
                MessageBox.Show(resManager.GetString("AcrobatNotFoundText"),
                    resManager.GetString("AcrobatNotFoundCaption"));
                return string.Empty;
            }

            //opening the acrobat
            RegistryKey acrobat = adobe.OpenSubKey("Acrobat Reader");
            if (acrobat == null)
            {
                MessageBox.Show(resManager.GetString("AcrobatNotFoundText"),
                    resManager.GetString("AcrobatNotFoundCaption"));
                return string.Empty;
            }

            //checking the available versions there has to be version at least 6.0
            List<string> versions = new List<string>(acrobat.GetSubKeyNames());
            if (!versions.Contains("6.0") && !versions.Contains("7.0") && !versions.Contains("8.0"))
            {
                MessageBox.Show(resManager.GetString("AcrobatNotFoundText"),
                    resManager.GetString("AcrobatNotFoundCaption"));
                return string.Empty;
            }

            //getting the version
            RegistryKey version = acrobat.OpenSubKey("6.0");
            if (version == null)
            {
                version = acrobat.OpenSubKey("7.0");

                if (version == null)
                {
                    version = acrobat.OpenSubKey("8.0");
                }
            }
            //checking if everything is all right
            if (version == null)
            {
                throw new ApplicationException("The Acrobat version is not right");
            }

            //getting the InstallPath value
            RegistryKey installPath = version.OpenSubKey("InstallPath");
            string acrobatPath = installPath.GetValue(null).ToString();

            acrobatPath += "\\Acrord32.exe";

            return acrobatPath;
        }

        /// <summary>
        /// Gets the name of the box
        /// </summary>
        /// <param name="projManager">Project Manager</param>
        /// <param name="boxIdentity">Ice box identity</param>
        /// <returns>User's name of the box</returns>
        private static string GetFaultBoxName(ProjectManager.ProjectManager projManager, string boxIdentity)
        {
            if (!String.IsNullOrEmpty(boxIdentity))
            {
                IBoxModule faultBox = projManager.ModulesManager.GetIBoxModuleByIdentity(boxIdentity);
                if (faultBox != null)
                    return faultBox.UserName;
            }
            return null;
        }

        /// <summary>
        /// Gets the name of the box that has thrown an exception
        /// </summary>
        /// <param name="faultBoxName">Fault name of the box</param>
        /// <param name="resManager">Resource manager</param>
        /// <returns>User's name of the box</returns>
        public static string GetFaultBoxName(string faultBoxName, ResourceManager resManager)
        {
            if (String.IsNullOrEmpty(faultBoxName))
                return resManager.GetString("BoxExceptionDialogFaultName");
            return faultBoxName;
        }

        /// <summary>
        /// Converts content of an exception into string
        /// </summary>
        /// <param name="e">Exception to be converted</param>
        /// <returns>String representation of the exception</returns>
        private static string exceptionToString(Exception e)
        {
            string result = "\n\nException to string:\n";
            result += e.ToString();
            if (e.InnerException != null)
            {
                result += "\n\nInnerException to string:\n";
                result += e.InnerException.ToString();
            }
            return result;
        }
        
        /// <summary>
        /// Converts array of socket name into one formatted string
        /// </summary>
        /// <param name="socketsNames">Socket names</param>
        /// <returns>Socket names formatted</returns>
        private static string exceptionSocketNamesToString(string[] socketsNames)
        {
            string result = "\n\nSockets:\n";
            foreach (string socket in socketsNames)
            {
                result += socket + '\n';
            }
            return result;
        }
        
        /// <summary>
        /// Converts exception restriction type into string
        /// </summary>
        /// <param name="restrictionType">Restriction type</param>
        /// <returns>Restriction type in string</returns>
        private static string exceptionRestrictionTypeToString(restrictionTypeEnum restrictionType)
        {
            return "\n\n" + restrictionType.ToString() + '\n';
        }
        
        /// <summary>
        /// Validates a box
        /// </summary>
        /// <param name="box">Box to be validated</param>
        /// <param name="resManager">Manager for the localized resources</param>
        /// <param name="projManager">Project manager to identify boxes by identity</param>
        public static void ValidateBox(IBoxModule box, ResourceManager resManager,
            ProjectManager.ProjectManager projManager)
        {
            //trying to validate the box and catching the exceptions
            try
            {
                box.Validate();
            }
            //the Ferda exceptions
            catch (Exception e)
            {
                string faultBoxName;
                string message;
                GetExceptionInfo(e, projManager, out faultBoxName, out message);
                faultBoxName = GetFaultBoxName(faultBoxName, resManager);

                BoxExceptionDialog d = new BoxExceptionDialog(resManager,
                    faultBoxName, message);
                d.ShowDialog();
                return;
            }

            //showing to the user that the validation went right
            MessageBox.Show(resManager.GetString("ValidateOK"),
                resManager.GetString("ValidateCaption"));
        }

        /// <summary>
        /// Gets string exception description from an exception
        /// </summary>
        /// <param name="ex">Exception to be evaluated</param>
        /// <param name="projManager">The Project manager</param>
        /// <param name="faultBoxName">
        /// Name of the box that has thrown the exception
        /// </param>
        /// <param name="message">
        /// Exception message
        /// </param>
        public static void GetExceptionInfo(Exception ex,
            ProjectManager.ProjectManager projManager, out string faultBoxName, out string message)
        {
            faultBoxName = string.Empty;
            message = string.Empty;

            {
                BadValueError e = ex as BadValueError;
                if (e != null)
                {
                    faultBoxName = GetFaultBoxName(projManager, e.boxIdentity);

                    message += e.userMessage;
                    message += exceptionRestrictionTypeToString(e.restrictionType);
                    message += exceptionSocketNamesToString(e.socketsNames);
                    message += exceptionToString(e);
                    return;
                }
            }
            {
                BadParamsError e = ex as BadParamsError;
                if (e != null)
                {
                    faultBoxName = GetFaultBoxName(projManager, e.boxIdentity);

                    message += e.userMessage;
                    message += exceptionRestrictionTypeToString(e.restrictionType);
                    message += exceptionToString(e);

                    return;
                }
            }
            {
                NoConnectionInSocketError e = ex as NoConnectionInSocketError;
                if (e != null)
                {
                    faultBoxName = GetFaultBoxName(projManager, e.boxIdentity);

                    message += e.userMessage;
                    message += exceptionSocketNamesToString(e.socketsNames);
                    message += exceptionToString(e);

                    return;
                }
            }
            {
                BoxRuntimeError e = ex as BoxRuntimeError;
                if (e != null)
                {
                    faultBoxName = GetFaultBoxName(projManager, e.boxIdentity);

                    message += e.userMessage;
                    message += exceptionToString(e);

                    return;
                }
            }
            // other unwanted exceptions (Ice.UnknownUserException or Exception)
            {
                //message += resManager.GetString("ValidateICEException");

                message += exceptionToString(ex);

                return;
            }
        }
    }

    /// <summary>
    /// The controls (displayers) implementing this interface
    /// have ability to fill the edit part of the main menu.
    /// </summary>
    public interface IEditMenuAbility
    {
        /// <summary>
        /// Context menu for the edit part of the main menu
        /// </summary>
        ContextMenuStrip EditMenu
        {
            get;
        }
    }
}
