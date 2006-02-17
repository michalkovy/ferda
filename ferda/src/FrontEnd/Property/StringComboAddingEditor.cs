using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;

using Ferda.Modules;

namespace Ferda.FrontEnd.Properties
{
    /// <summary>
    /// Editor that is used to display combobox with options and a textbox to add
    /// new options
    /// </summary>
    internal class StringComboAddingEditor : UITypeEditor
    {
        /// <summary>
        /// Service provider for the class
        /// </summary>
        protected IWindowsFormsEditorService edSvc;

        /// <summary>
        /// The editor displays this control
        /// </summary>
        public StringComboAddingControl control;

        /// <summary>
        /// Constructor of the class
        /// </summary>
        public StringComboAddingEditor() : base()
        {
        }

        /// <summary>
        /// Gets the editor style used by the EditValue method.
        /// </summary>
        /// <param name="context">A context that can be used to gain 
        /// additional context information. </param>
        /// <returns>
        /// A UITypeEditorEditStyle value that indicates the style of editor 
        /// used by EditValue. If the UITypeEditor does not support this method, 
        /// then GetEditStyle will return None.
        /// </returns>
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.DropDown;
        }

        /// <summary>
        /// Indicates whether this editor supports painting a representation of an 
        /// object's value.
        /// </summary>
        /// <param name="context">A context that can be used to gain 
        /// additional context information. </param>
        /// <returns>true if PaintValue is implemented; otherwise, false.
        /// </returns>
        public override bool GetPaintValueSupported(ITypeDescriptorContext context)
        {
            return false;
        }

        /// <summary>
        /// Displays a combo box and when the user selects a value, returns this value
        /// </summary>
        /// <param name="context">A context that can be used to gain 
        /// additional context information. </param>
        /// <param name="provider">An IServiceProvider that this editor can use to 
        /// obtain services. 
        /// </param>
        /// <param name="value">The object to edit.</param>
        /// <returns>The new value of the object.</returns>
        public override object EditValue(ITypeDescriptorContext context,
            IServiceProvider provider, object value)
        {
            edSvc =
                (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));

            if (edSvc != null)
            {
                //getting the StringSequence data
                StringSequence seq = value as StringSequence;

                //creating the control
                control = new StringComboAddingControl(seq);
                control.BAddOption.Click += new EventHandler(BAddOption_Click);
                control.LBOptions.DoubleClick += new EventHandler(LBOptions_DoubleClick);

                //displaying the control
                edSvc.DropDownControl(control);

                return seq;
            }

            throw new ApplicationException("IWindowsFormsEditorService not available");
        }

        /// <summary>
        /// Event that should force the control to close and write the selected value
        /// in the list to the property
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event parameters</param>
        void LBOptions_DoubleClick(object sender, EventArgs e)
        {
            if (edSvc != null)
            {
                if (control != null)
                {
                    control.SetListOption();
                    edSvc.CloseDropDown();
                }
            }
        }

        /// <summary>
        /// Event that should force the control to close and write the new value to the
        /// property
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event parameters</param>
        void BAddOption_Click(object sender, EventArgs e)
        {
            if (edSvc != null)
            {
                if (control != null)
                {
                    control.SetOtherOption();
                    edSvc.CloseDropDown();
                }
            }
        }
    }
}
