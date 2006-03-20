// CreateSetWizard.cs - class for creating an enumeration category
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
using System.Data;
using System.Text;
using System.Windows.Forms;
using Ferda.FrontEnd.AddIns.EditCategories.NoGUIclasses;
using Ferda;
using Ferda.FrontEnd.AddIns.EditCategories.CreateNewCategory;
using System.Resources;
using System.Reflection;

namespace Ferda.FrontEnd.AddIns.EditCategories.CreateNewCategory
{
    /// <summary>
    /// Class for creating enum categories
    /// </summary>
    public partial class CreateSetWizard : UserControl
    {
        #region Private variables

        /// <summary>
        /// Datalist to work with
        /// </summary>
        private FerdaSmartDataList dataList;

        /// <summary>
        /// Resource manager
        /// </summary>
        private ResourceManager resManager;

        #endregion


        #region Constructor

        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="dataList">Datalist to work with</param>
        /// <param name="rm">Resource manager</param>
        public CreateSetWizard(FerdaSmartDataList dataList, ResourceManager rm)
        {
            //setting the ResManager resource manager and localization string
            this.resManager = rm;
            this.dataList = dataList;
            InitializeComponent();
            this.FillAvailableValues(ListBoxAvailableValues, this.dataList);
            this.ChangeLocale(this.resManager);
        }

        #endregion


        #region Button handlers
        /// <summary>
        /// Adds a value to the values of a new set.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Add_Click(object sender, EventArgs e)
        {
            ArrayList arrayList = new ArrayList();
            foreach (object item in ListBoxExistingValues.Items)
            {
                arrayList.Add(item);
            }
            if (ListBoxAvailableValues.SelectedIndices.Count > 0)
            {
                arrayList.Add(ListBoxAvailableValues.SelectedItem);
                ListBoxAvailableValues.Items.RemoveAt(ListBoxAvailableValues.SelectedIndex);
                arrayList.Sort();
                ListBoxExistingValues.Items.Clear();
                foreach (object item in arrayList)
                {
                    ListBoxExistingValues.Items.Add(item);
                }
            }
        }

        /// <summary>
        /// Create a new multiset with the given values.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Submit_Click(object sender, EventArgs e)
        {
            ArrayList arrayList = new ArrayList();
            foreach (object item in ListBoxExistingValues.Items)
            {
                arrayList.Add(item);
            }
            Category tempSet = new Category();
            tempSet.CatType = CategoryType.Enumeration;
            tempSet.Name = this.TextBoxNewName.Text;
            SingleSet tempSingle = new SingleSet(arrayList);
            tempSet.AddSingleSet(tempSingle);
            dataList.AddNewCategoryDirect(tempSet);
            this.Dispose();
        }

        /// <summary>
        /// Removes selected values from the enumeration.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Remove_Click(object sender, EventArgs e)
        {
            if (ListBoxExistingValues.SelectedIndices.Count > 0)
            {
                ArrayList arrayList = new ArrayList();
                foreach (object item in ListBoxAvailableValues.Items)
                {
                    arrayList.Add(item);
                }
                arrayList.Add(ListBoxExistingValues.SelectedItem);
                ListBoxExistingValues.Items.RemoveAt(ListBoxExistingValues.SelectedIndex);
                arrayList.Sort();
                ListBoxAvailableValues.Items.Clear();
                foreach (object item in arrayList)
                {
                    ListBoxAvailableValues.Items.Add(item);
                }
            }
        }

        /// <summary>
        /// Cancel handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Cancel_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        #endregion


        #region Other private methods

        /// <summary>
        /// Fill the listbox with available (not used) values
        /// </summary>
        /// <param name="listBox">Listbox to fill</param>
        /// <param name="dataList">Used datalist</param>
        private void FillAvailableValues(ListBox listBox, FerdaSmartDataList dataList)
        {
            foreach (object value in dataList.GetAvailableValues())
            {
                ListBoxAvailableValues.Items.Add(value);
            }
        }

        #endregion


        #region Localization

        /// <summary>
        /// Change locale method
        /// </summary>
        /// <param name="rm">Resource manager</param>
        private void ChangeLocale(ResourceManager rm)
        {
            this.LabelAvailableValues.Text = rm.GetString("LabelAvailableValues");
            this.LabelExistingValues.Text = rm.GetString("LabelExistingValues");
            this.LabelNewName.Text = rm.GetString("LabelNewName");
            this.ButtonAdd.Text = rm.GetString("ButtonAddText");
            this.ButtonRemove.Text = rm.GetString("ButtonRemoveText");
            this.ButtonCancel.Text = rm.GetString("ButtonCancel");
            this.ButtonSubmit.Text = rm.GetString("ButtonSubmit");
        }

        #endregion
    }
}