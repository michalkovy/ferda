// OtherPropertyEditor.cs - editor that is used for any OtherProperty
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
