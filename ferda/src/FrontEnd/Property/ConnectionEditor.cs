using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing.Design;

namespace Ferda.FrontEnd.Properties
{
    /// <summary>
    /// Editor that is used when user wants to create connections on the PropertyGrid
    /// </summary>
    class ConnectionEditor : UITypeEditor
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public ConnectionEditor()
        {
        }

        #endregion

        /// <summary>
        /// Indicates whether the UITypeEditor provides a form-based (modal) dialog, 
        /// drop down dialog, or no UI outside of the properties window.
        /// </summary>
        /// <param name="context">Some parameters</param>
        /// <returns>It will be 3 dots (...)</returns>
        [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
        public override System.Drawing.Design.UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }

		// Displays the UI for value selection.
        [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
        public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, System.IServiceProvider provider, object value)
        {
            ConnectionsDialog dialog = new ConnectionsDialog();

            dialog.ShowDialog();

            if (dialog.DialogResult == System.Windows.Forms.DialogResult.OK)
            {
            }

            return null;
        }
    }
}
