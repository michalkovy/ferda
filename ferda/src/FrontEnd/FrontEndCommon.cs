using System;
using System.Collections.Generic;
using System.Text;
using Ferda.ModulesManager;
using System.Resources;
using System.Windows.Forms;
using System.IO;
using Ferda.ProjectManager;
using System.Reflection;

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
                catch(FileNotFoundException)
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
            //replacing the .exe by the name of the config file
            int lastDash = assemblyDir.LastIndexOf('\\');
            assemblyDir = assemblyDir.Remove(lastDash);

            return assemblyDir;
        }
    }
}
