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
    /// Editor that is used for any OtherProperty
    /// </summary>
    internal class OtherPropertyEditor : UITypeEditor
    {
        /// <summary>
        /// Constructor of the class
        /// </summary>
        public OtherPropertyEditor() : base()
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
            return UITypeEditorEditStyle.Modal;
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
            OtherProperty op = value as OtherProperty;
            if (op != null)
            {
                op.SetProperty();
            }
            return op;
        }
    }
}
