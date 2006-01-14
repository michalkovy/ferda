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
    /// Editor that is used to display a combobox of string options
    /// </summary>
    class StringComboEditor : UITypeEditor
    {
        protected ListBox listBox;
        protected IWindowsFormsEditorService edSvc;

        /// <summary>
        /// Constructor of the class
        /// </summary>
        public StringComboEditor() : base()
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
                //creating a listbox
                listBox = new ListBox();
                listBox.DoubleClick += new EventHandler(lb_DoubleClick);

                StringSequence seq = value as StringSequence;

                //filling the listbox
                foreach (SelectString ss in seq.array)
                {
                    listBox.Items.Add(ss.label);
                }

                edSvc.DropDownControl(listBox);

                if (listBox.SelectedIndex!= -1)
                {
                    //setting the property into the ProjectManager
                    string name = string.Empty;
                    foreach (SelectString ss in seq.array)
                    {
                        if (ss.label == listBox.SelectedItem.ToString())
                        {
                            name = ss.name;
                            break;
                        }
                    }

                    //checking some name was found
                    if (name == string.Empty)
                    {
                        throw new ApplicationException("Inconsistent label and name in SelectString");
                    }

                    seq.selectedName = name;
                    seq.selectedLabel = listBox.SelectedItem.ToString();
                    seq.SetSelectedOption();
                }

                return seq;
            }

            throw new ApplicationException("IWindowsFormsEditorService not available");
        }

        /// <summary>
        /// Event that should force the control to close
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event parameters</param>
        void lb_DoubleClick(object sender, EventArgs e)
        {
            if (listBox != null)
            {
                if (edSvc != null)
                {
                    edSvc.CloseDropDown();
                }
            }
        }
    }
}
