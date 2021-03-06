// EditExistingEnumeration.cs - class for editing an enumeration
//
// Author: Alexander Kuzmin <alexander.kuzmin@gmail.com>
//
// Copyright (c) 2005 Alexander Kuzmin
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
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
//using Ferda.FrontEnd.AddIns.EditCategories.NoGUIclasses;
using System.Resources;
using System.Reflection;
using Ferda.Guha.Attribute;
using System.Data;

namespace Ferda.FrontEnd.AddIns.EditCategories.EditExisting
{
    /// <summary>
    /// Class for editing existing enumeration
    /// </summary>
    public class EditExistingEnumeration : Ferda.FrontEnd.AddIns.EditCategories.CreateNewCategory.CreateSetWizard
    {

        #region Private variables

        /// <summary>
        /// Required variable
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// DataList to work with
        /// </summary>
        Attribute<IComparable> attribute;


        #endregion


        #region Constructor

    /// <summary>
    /// Classs constructor
    /// </summary>
    /// <param name="attribute">Edited attribute</param>
    /// <param name="index">Index of edited category</param>
    /// <param name="rm">Resource manager</param>
        public EditExistingEnumeration(Attribute<IComparable> attribute, string index, DataTable table,
            ResourceManager rm, EventHandler closeHandler)
            : base(attribute, index, table, rm, closeHandler)
        {
            this.attribute = attribute;
            this.tempName = index;
            this.ButtonSubmit.Click -= new EventHandler(Submit_Click);
            this.ButtonSubmit.Click += new EventHandler(Submit_Click_New);

            this.ButtonCancel.Click -= new EventHandler(Cancel_Click);
            this.ButtonCancel.Click += new EventHandler(Cancel_Click_New);

            this.TextBoxNewName.Text = index;
            foreach (object value in this.attribute[index].Enumeration)
            {
                this.ListBoxExistingValues.Items.Add(value);
            }

            // This call is required by the Windows Form Designer.
            InitializeComponent();
            this.TextBoxNewName.Enabled = false;
        }

        #endregion


        #region Button handlers

        /// <summary>
        /// Submits edited enumeration
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Submit_Click_New(object sender, EventArgs e)
        {
            this.attribute[tempName].Reduce();
            if ((attribute[tempName].Enumeration.Count == 0) && (attribute[tempName].Intervals.Count == 0))
            {
                MessageBox.Show(this.resManager.GetString("EmptyCategory"),
                   this.resManager.GetString("Error"),
                   MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            this.Dispose();
        }

        /// <summary>
        /// Cancel handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Cancel_Click_New(object sender, EventArgs e)
        {
            this.Dispose();
        }

        #endregion


        #region VS generated code
        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #endregion


        #region Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
        }

        #endregion
    }
}