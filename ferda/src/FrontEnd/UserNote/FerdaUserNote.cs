// FerdaUserNote.cs - control that displays a user note
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
using System.Windows.Forms;
using Ferda.ModulesManager;

namespace Ferda.FrontEnd.UserNote
{
    /// <summary>
    /// This control enables the user to edit the user note of
    /// the box that is currently beeing selected(edited) either in the
    /// archive or on the desktop
    /// </summary>
    public class FerdaUserNote : RichTextBox, IUserNoteDisplayer
    {
        #region Fields

        private IBoxModule selectedBox;

        #endregion

        #region Properties

        /// <summary>
        /// The box that is beeing edited by the control
        /// </summary>
        public IBoxModule SelectedBox
        {
            get { return selectedBox; }
            set { selectedBox = value; }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor for the class
        /// </summary>
        public FerdaUserNote()
        {
            this.LostFocus += new EventHandler(FerdaUserNote_LostFocus);
            selectedBox = null;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Adapts the control (loads the user note of the selected box)
        /// </summary>
        public void Adapt()
        {
            if (IsDisposed)
            {
                return;
            }
            this.Enabled = true;
            this.Text = SelectedBox.UserHint;
        }

        /// <summary>
        /// Resets the user note to be without any information
        /// </summary>
        public void Reset()
        {
            if (IsDisposed)
            {
                return;
            }
            this.Enabled = false;
            this.Text = string.Empty;
        }

        /// <summary>
        /// Resizes the control according to the size of the parent content
        /// </summary>
        public void ChangeSize()
        {
            if (Parent != null)
            {
                this.Size = Parent.Size;
            }
        }

        #endregion

        #region Events

        /// <summary>
        /// Occurs when the text of the box changes - the new usernote
        /// should be written back to the box
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event parameters</param>
        void FerdaUserNote_LostFocus(object sender, EventArgs e)
        {
            if (selectedBox != null)
            {
                SelectedBox.UserHint = this.Text;   
            }
        }

        #endregion
    }
}
