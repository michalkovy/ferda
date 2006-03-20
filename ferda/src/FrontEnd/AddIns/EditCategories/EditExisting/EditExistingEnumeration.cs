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
using Ferda.FrontEnd.AddIns.EditCategories.NoGUIclasses;
using System.Resources;
using System.Reflection;

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
        FerdaSmartDataList datalist;

        /// <summary>
        /// Edited enumeration.
        /// </summary>
        Category enumeration;

        /// <summary>
        /// Edited enumeration index
        /// </summary>
        int index = 0;

        #endregion


        #region Constructor

        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="dataList">Datalist to work with</param>
        /// <param name="Enumeration">Enumeration to edit</param>
        /// <param name="rm"></param>
        public EditExistingEnumeration(FerdaSmartDataList dataList, Category Enumeration, ResourceManager rm)
            : base(dataList, rm)
        {
            this.datalist = dataList;
            this.enumeration = Enumeration;
            this.ButtonSubmit.Click -= new EventHandler(Submit_Click);
            this.ButtonSubmit.Click += new EventHandler(Submit_Click_New);
            index = this.datalist.GetIndex(this.enumeration);
            this.TextBoxNewName.Text = this.enumeration.Name;
            foreach (object value in this.enumeration.Set.Values)
            {
                this.ListBoxExistingValues.Items.Add(value);
            }

            // This call is required by the Windows Form Designer.
            InitializeComponent();
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
            this.enumeration.RemoveSetValues();
            ArrayList tempList = new ArrayList();
            foreach (object item in ListBoxExistingValues.Items)
            {
                tempList.Add(item);
            }
            SingleSet newSet = new SingleSet(tempList);
            this.enumeration.AddSingleSet(newSet);
            this.datalist.RemoveCategory(this.index);
            this.enumeration.Name = this.TextBoxNewName.Text;
            this.datalist.AddNewCategoryDirect(this.enumeration);
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