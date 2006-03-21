namespace Ferda.FrontEnd.AddIns
{
	public interface IOwnerOfAddIn
	{
        /// <summary>
        /// Method ShowForm, shows a form 
        /// </summary>
        /// <param name="form">A  System.Windows.Forms.Form</param>
		void ShowForm(System.Windows.Forms.Form form);

        /// <summary>
        /// Method ShowDialog, shows a dialog
        /// </summary>
        /// <param name="form">A  System.Windows.Forms.Form</param>
		System.Windows.Forms.DialogResult ShowDialog(System.Windows.Forms.Form form);

        /// <summary>
        /// The method shows a control and docks it into the FrontEnd environment
        /// </summary>
        /// <param name="userControl">A  System.Windows.Forms.UserControl</param>
        /// <param name="name">Text of the control</param>
		void ShowDockableControl(System.Windows.Forms.UserControl userControl, string name);

        /// <summary>
        /// Forces to adapt the property grid from another thread
        /// </summary>
        void AsyncAdapt();

        /// <summary>
        /// Shows the exception with the box to the user
        /// </summary>
        /// <param name="boxUserName">Name of the box that has thrown the exception</param>
        /// <param name="userMessage">User message to be displayed</param>
        void ShowBoxException(string boxUserName, string userMessage);

        /// <summary>
        /// Project manager of the Ferda system
        /// </summary>
        Ferda.ProjectManager.ProjectManager ProjectManager
        {
            get;
        }

        /// <summary>
        /// Opens a pdf that is located in the path in the parameter
        /// and jumps to 
        /// </summary>
        /// <param name="path">Path to the pdf file</param>
        /// <param name="destination">named destination in the pdf where
        /// the Acrobat reader should jump</param>
        void OpenPdfAndDestination(string path, string destination);

        /// <summary>
        /// Opens a pdf that is located in the path in the parameter
        /// </summary>
        /// <param name="path">Path to the pdf file</param>
        void OpenPdf(string path);

        /// <summary>
        /// Function returns the directory where the FerdaFrontEnd.exe file
        /// (current assembly) is located
        /// </summary>
        /// <returns>The path where the assembly is located</returns>
        string GetBinPath();
	}
}
