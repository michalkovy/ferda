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
using Ferda.Guha.Data;
using Ferda.FrontEnd.AddIns.EditCategories.CreateNewCategory;
using System.Resources;
using System.Reflection;
using Ferda.Guha.Attribute;

namespace Ferda.FrontEnd.AddIns.EditCategories.CreateNewCategory
{
    /// <summary>
    /// Class for creating enum categories
    /// </summary>
    public partial class CreateSetWizard : UserControl
    {
        #region Private variables

        /// <summary>
        /// Edited attribute
        /// </summary>
        private Attribute<IComparable> attribute;

        /// <summary>
        /// Resource manager
        /// </summary>
        private ResourceManager resManager;

        /// <summary>
        /// Temporary category name
        /// </summary>
        protected string tempName;

        /// <summary>
        /// Datatable on which the attribute is applied
        /// </summary>
        DataTable table;

        #endregion


        #region Constructor

        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="dataList">Datalist to work with</param>
        /// <param name="rm">Resource manager</param>
        public CreateSetWizard(Attribute<IComparable> attribute,
            string tempname, DataTable table, ResourceManager rm, EventHandler closeHandler)
        {
            //setting the ResManager resource manager and localization string
            this.resManager = rm;
            this.attribute = attribute;
            this.table = table;
            InitializeComponent();
            if (tempname != String.Empty)
            {
                tempName = tempname;
            }
            else
            {
                tempName = RandomString.CreateKey(64);
                this.attribute.Add(tempName);
            }
            
            this.FillAvailableValues(ListBoxUncoveredValues, this.attribute);
            this.ChangeLocale(this.resManager);
            this.Disposed += closeHandler;
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
            IComparable itemToAdd;
            ArrayList arrayList = new ArrayList();

            if (ListBoxUncoveredValues.SelectedIndices.Count > 0)
            {
                object item = ListBoxUncoveredValues.SelectedItem;
                switch (attribute.DbDataType)
                {
                    case DbSimpleDataTypeEnum.BooleanSimpleType:
                        itemToAdd = (IComparable)Convert.ToBoolean(item);
                        break;

                    case DbSimpleDataTypeEnum.DateTimeSimpleType:
                        itemToAdd = (IComparable)Convert.ToDateTime(item);
                        break;

                    case DbSimpleDataTypeEnum.DoubleSimpleType:
                    case DbSimpleDataTypeEnum.FloatSimpleType:
                        itemToAdd = (IComparable)Convert.ToDouble(item);
                        break;

                    case DbSimpleDataTypeEnum.IntegerSimpleType:
                        itemToAdd = (IComparable)Convert.ToInt32(item);
                        break;

                    case DbSimpleDataTypeEnum.LongSimpleType:
                        itemToAdd = (IComparable)Convert.ToInt64(item);
                        break;

                    case DbSimpleDataTypeEnum.ShortSimpleType:
                        itemToAdd = (IComparable)Convert.ToInt16(item);
                        break;

                    case DbSimpleDataTypeEnum.StringSimpleType:
                        itemToAdd = (IComparable)item.ToString();
                        break;

                    default:
                        MessageBox.Show(this.resManager.GetString("InvalidEnumerationType"),
                            this.resManager.GetString("Error"),
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                }
                try
                {
                    this.attribute[tempName].Enumeration.Add(itemToAdd, false);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Generic exception",
                        MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                arrayList.Add(item);
                arrayList.AddRange(ListBoxExistingValues.Items);
                ListBoxUncoveredValues.Items.RemoveAt(ListBoxUncoveredValues.SelectedIndex);
                arrayList.Sort();
                ListBoxExistingValues.Items.Clear();
                ListBoxExistingValues.Items.AddRange(arrayList.ToArray());
            }
        }

        /// <summary>
        /// Create a new multiset with the given values.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Submit_Click(object sender, EventArgs e)
        {
            attribute[tempName].Reduce();
            try
            {
                this.attribute.RenameCategory(tempName, this.TextBoxNewName.Text);
            }
            catch
            {
                MessageBox.Show(this.resManager.GetString("SameCategoryNames"), 
                    this.resManager.GetString("Error"),
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
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
                IComparable itemToRemove;

                object item = ListBoxExistingValues.SelectedItem;
                switch (attribute.DbDataType)
                {
                    case DbSimpleDataTypeEnum.BooleanSimpleType:
                        itemToRemove = (IComparable)Convert.ToBoolean(item);
                        break;

                    case DbSimpleDataTypeEnum.DateTimeSimpleType:
                        itemToRemove = (IComparable)Convert.ToDateTime(item);
                        break;

                    case DbSimpleDataTypeEnum.DoubleSimpleType:
                    case DbSimpleDataTypeEnum.FloatSimpleType:
                        itemToRemove = (IComparable)Convert.ToDouble(item);
                        break;

                    case DbSimpleDataTypeEnum.IntegerSimpleType:
                        itemToRemove = (IComparable)Convert.ToInt32(item);
                        break;

                    case DbSimpleDataTypeEnum.LongSimpleType:
                        itemToRemove = (IComparable)Convert.ToInt64(item);
                        break;

                    case DbSimpleDataTypeEnum.ShortSimpleType:
                        itemToRemove = (IComparable)Convert.ToInt16(item);
                        break;

                    case DbSimpleDataTypeEnum.StringSimpleType:
                        itemToRemove = (IComparable)item.ToString();
                        break;

                    default:
                        MessageBox.Show(this.resManager.GetString("InvalidEnumerationType"),
                            this.resManager.GetString("Error"),
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                }
                try
                {
                    this.attribute[tempName].Enumeration.Remove(itemToRemove);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Generic exception",
                        MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                ArrayList arrayList = new ArrayList();
                arrayList.AddRange(ListBoxUncoveredValues.Items);
                arrayList.Add(item);
                ListBoxExistingValues.Items.RemoveAt(ListBoxExistingValues.SelectedIndex);
                arrayList.Sort();
                ListBoxUncoveredValues.Items.Clear();
                ListBoxUncoveredValues.Items.AddRange(arrayList.ToArray());
            }
        }

        /// <summary>
        /// Cancel handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Cancel_Click(object sender, EventArgs e)
        {
            this.attribute.Remove(tempName);
            this.Dispose();
        }

        #endregion


        #region Other private methods

        /// <summary>
        /// Fill the listbox with available (not used) values
        /// </summary>
        /// <param name="listBox">Listbox to fill</param>
        /// <param name="dataList">Used datalist</param>
        private void FillAvailableValues(ListBox listBox, Attribute<IComparable> attribute)
        {
            foreach (object value in attribute.GetUncoveredValues(this.table))
            {
                ListBoxUncoveredValues.Items.Add(value);
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
            this.LabelUncoveredValues.Text = rm.GetString("LabelAvailableValues");
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