using System;
using System.Collections.Generic;
using System.Text;
using Ferda.ModulesManager;
using System.Resources;
using System.Windows.Forms;
using System.IO;
using Ferda.ProjectManager;

namespace Ferda.FrontEnd
{
    /// <summary>
    /// Class that provides the functionality that is in common to more classes
    /// </summary>
    public class FrontEndCommon
    {
        /// <summary>
        /// Shows a messagebox saying that user cannot write to the box
        /// </summary>
        public static void CannotWriteToBox(IBoxModule box, ResourceManager resManager)
        {
            MessageBox.Show(
                resManager.GetString("DesktopCannotWriteText"),
                box.UserName + ": " + resManager.GetString("DesktopCannotWriteCaption"));
        }

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
    }
}
