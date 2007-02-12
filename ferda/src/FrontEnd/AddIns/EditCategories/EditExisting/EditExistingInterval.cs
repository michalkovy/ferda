// EditExistingInterval.cs - class for editing an interval
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
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Ferda.FrontEnd.AddIns.EditCategories.NoGUIclasses;
using System.Resources;
using System.Reflection;
using Ferda.Guha.Attribute;

namespace Ferda.FrontEnd.AddIns.EditCategories.EditExisting
{
    /// <summary>
    /// Class for editing interval
    /// </summary>
    public partial class EditExistingInterval : Ferda.FrontEnd.AddIns.EditCategories.CreateNewCategory.CreateIntervalWizard
    {
        #region Private variables



        #endregion


        #region Constructor

        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="dataList">Datalist to work with</param>
        /// <param name="editedInterval">Edited interval category</param>
        /// <param name="rm">resource manager</param>
        public EditExistingInterval(Attribute<IComparable> attribute, string index, ResourceManager rm, EventHandler closeHandler)
            : base(attribute, index, rm, closeHandler)
        {
            this.attribute = attribute;
            this.tempName = index;

            Interval<IComparable> tempInterval = attribute[index].Intervals[0];
            foreach (Interval<IComparable> inter in attribute[index].Intervals)
            {
                this.ListBoxIntervals.Items.Add(inter.ToString());
                if (this.ListBoxIntervals.Items.Count > 0)
                {
                    this.ListBoxIntervals.SelectedIndex = 0;
                }
            }
            this.ButtonCancel.Click -= new System.EventHandler(this.CancelButton_Click);
            this.ButtonSubmit.Click -= new System.EventHandler(this.SubmitButton_Click);
            this.ButtonCancel.Click += new System.EventHandler(this.ButtonCancel_Click_New);
            this.ButtonSubmit.Click += new System.EventHandler(this.ButtonSubmit_Click_New);
            InitializeComponent();
            this.TextBoxCategoryName.Text =
                index;
            this.TextBoxCategoryName.Enabled = false;
        }

        #endregion


        #region Button handlers


        /// <summary>
        /// Handler for submitting the changed interval to categories list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ButtonSubmit_Click_New(object sender, EventArgs e)
        {
            this.attribute[tempName].Reduce();
            this.Dispose();
        }

        /// <summary>
        /// Handler for cancel button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ButtonCancel_Click_New(object sender, EventArgs e)
        {
            this.Dispose();
        }

        #endregion
    }
}