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

namespace Ferda.FrontEnd.AddIns.EditCategories.EditExisting
{
    /// <summary>
    /// Class for editing interval
    /// </summary>
    public partial class EditExistingInterval : Ferda.FrontEnd.AddIns.EditCategories.CreateNewCategory.CreateIntervalWizard
    {
        #region Private variables

        /// <summary>
        /// Datalist to work with
        /// </summary>
        FerdaSmartDataList datalist;

        /// <summary>
        /// Edited interval
        /// </summary>
        Category interval;

        /// <summary>
        /// Edited category index
        /// </summary>
        int index = 0;

        #endregion


        #region Constructor

        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="dataList">Datalist to work with</param>
        /// <param name="editedInterval">Edited interval category</param>
        /// <param name="rm">resource manager</param>
        public EditExistingInterval(FerdaSmartDataList dataList, Category editedInterval, ResourceManager rm)
            : base(dataList, rm)
        {
            this.datalist = dataList;
            this.interval = new Category();
            this.interval.CatType = CategoryType.Interval;
            this.interval.Name = editedInterval.Name;
            this.interval.Frequency = editedInterval.Frequency;
            foreach (Interval inter in editedInterval.GetIntervals())
            {
                this.interval.AddInterval(inter);
            }
            this.currentCategory = editedInterval;
            this.index = datalist.GetIndex(this.currentCategory);
            this.TextBoxCategoryName.Text = editedInterval.Name;
            Interval tempInterval = (Interval)editedInterval.GetIntervals()[0];
            switch (tempInterval.intervalType)
            {
                case IntervalType.Long:
                    this.TextBoxLeftBound.Text = tempInterval.lowerBound.ToString();
                    this.TextBoxRightBound.Text = tempInterval.upperBound.ToString();
                    break;

                case IntervalType.Float:
                    this.TextBoxLeftBound.Text = tempInterval.lowerBoundFl.ToString();
                    this.TextBoxRightBound.Text = tempInterval.upperBoundFl.ToString();
                    break;

                default:
                    throw new Exception("Switch branch not implemented");
            }

            foreach (Interval inter in editedInterval.GetIntervals())
            {
                this.ListBoxIntervals.Items.Add(inter.ToString());
                if (this.ListBoxIntervals.Items.Count > 0)
                {
                    this.ListBoxIntervals.SelectedIndex = 0;
                }
            }
            this.CheckIntervalTypesConsistency();
            this.ButtonCancel.Click -= new System.EventHandler(this.CancelButton_Click);
            this.ButtonCancel.Click += new System.EventHandler(this.ButtonNewCancel_Click);

            //initialiazing controls according to the interval
            if (tempInterval.lowerBoundType == IntervalBoundType.Infinity)
            {
                this.RadioMinusInfinity.Checked = true;
            }
            else
            {
                if (tempInterval.lowerBoundType == IntervalBoundType.Round)
                {
                    this.RadioLeftBoundRound.Checked = true;
                }
                else
                {
                    this.RadioLeftBoundSharp.Checked = true;
                }
            }

            if (tempInterval.upperBoundType == IntervalBoundType.Infinity)
            {
                this.RadioPlusInfinity.Checked = true;
            }
            else
            {
                if (tempInterval.upperBoundType == IntervalBoundType.Round)
                {
                    this.RadioRightBoundRound.Checked = true;
                }
                else
                {
                    this.RadioRightBoundSharp.Checked = true;
                }
            }
            this.datalist.RemoveCategory(index);
            InitializeComponent();
        }

        #endregion


        #region Button handlers

        /// <summary>
        /// Cancel click handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ButtonNewCancel_Click(object sender, EventArgs e)
        {
            this.datalist.AddNewCategoryDirect(this.interval);
            this.Dispose();
        }

        #endregion
    }
}