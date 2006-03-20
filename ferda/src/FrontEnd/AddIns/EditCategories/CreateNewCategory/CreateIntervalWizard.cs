// CreateIntervalWizard.cs - class for creating interval category
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

namespace Ferda.FrontEnd.AddIns.EditCategories.CreateNewCategory
{
    /// <summary>
    /// Class for creating interval categories
    /// </summary>
    public partial class CreateIntervalWizard : UserControl
    {

        #region Private variables

        /// <summary>
        /// Datalist to work with
        /// </summary>
        protected FerdaSmartDataList dataList;

        /// <summary>
        /// Resource manager
        /// </summary>
        protected ResourceManager resManager;

        /// <summary>
        /// Temp variable for the multiset being created
        /// </summary>
        protected Category currentCategory;

        #endregion


        #region Constructor

        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="dataList">Datalist</param>
        /// <param name="rm">Resource manager</param>
        public CreateIntervalWizard(FerdaSmartDataList dataList, ResourceManager rm)
        {
            //setting the ResManager resource manager and localization string
            this.resManager = rm;
            this.dataList = dataList;
            InitializeComponent();
            this.ChangeLocale(this.resManager);
            this.AddHandlers();
            //Initializing temp category
            this.currentCategory = new Category();
            currentCategory.CatType = CategoryType.Interval;
        }

        #endregion


        #region Initialization

        /// <summary>
        /// Method for adding handlers to controls
        /// </summary>
        private void AddHandlers()
        {
            this.TextBoxLeftBound.LostFocus += new EventHandler(TextBoxLeftBound_TextEntered);
            this.TextBoxRightBound.LostFocus += new EventHandler(TextBoxRightBound_TextEntered);
        }

        #endregion


        #region Radiobuttons handlers

        /// <summary>
        /// Handler for minus infinity checkbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void MinusInfinity_CheckedChanged(object sender, EventArgs e)
        {
            if (this.RadioMinusInfinity.Checked == true)
            {
                this.TextBoxLeftBound.Enabled = false;
            }

            else
            {
                this.TextBoxLeftBound.Enabled = true;
            }
        }


        /// <summary>
        /// Handler for plus infinity checkbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void PlusInfinity_CheckedChanged(object sender, EventArgs e)
        {
            if (this.RadioPlusInfinity.Checked == true)
            {
                this.TextBoxRightBound.Enabled = false;
            }
            else
            {
                this.TextBoxRightBound.Enabled = true;
            }
        }

        #endregion


        #region TextBox handlers

        /// <summary>
        /// Handler which sets the left bound of the interval
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void TextBoxLeftBound_TextEntered(object sender, EventArgs e)
        {
            try
            {
                Convert.ToDouble(TextBoxLeftBound.Text);
            }
            catch
            {
                TextBoxLeftBound.Text = "";
            }
        }

        /// <summary>
        /// Handler which sets the right bound of the interval
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void TextBoxRightBound_TextEntered(object sender, EventArgs e)
        {
            try
            {
                Convert.ToDouble(TextBoxRightBound.Text);
            }
            catch
            {
                TextBoxRightBound.Text = "";
            }
        }

        #endregion


        #region ListBox handlers

        /// <summary>
        /// Method which fills the UI elements accroding to the selected interval
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ListBoxIntervals_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.ListBoxIntervals.SelectedIndices.Count > 0)
            {
                try
                {
                    Interval tempInterval = this.currentCategory.GetInterval(this.ListBoxIntervals.SelectedIndex);
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

                }
                catch
                {
                }
            }
        }

        /// <summary>
        /// Handler for double-click which deletes the interval clicked on
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ListBoxIntervals_DoubleClick(object sender, System.EventArgs e)
        {
            if (this.ListBoxIntervals.SelectedIndices.Count > 0)
            {
                try
                {
                    this.currentCategory.RemoveInterval(this.ListBoxIntervals.SelectedIndex);
                    this.ListBoxIntervals.Items.RemoveAt(this.ListBoxIntervals.SelectedIndex);
                }
                catch
                {
                }
            }
            this.CheckIntervalTypesConsistency();
        }


        #endregion


        #region Buttons handlers

        /// <summary>
        /// Handler for adding the interval to category
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void AddToCategoryButton_Click(object sender, EventArgs e)
        {
            if (!this.DataIsValid())
            {
                MessageBox.Show(this.resManager.GetString("InsufficientIntervalParameters"), this.resManager.GetString("InvalidIntervalError"),
                        MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            CheckIntervalTypesConsistency();
            Interval interval;
            if (this.RadioFloat.Checked)
            {
                try
                {
                    interval = this.MakeInterval(IntervalType.Float);
                }
                catch
                {
                    MessageBox.Show(this.resManager.GetString("BadIntervalType"), this.resManager.GetString("InvalidIntervalError"),
                        MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
            }
            else
            {
                try
                {
                    interval = this.MakeInterval(IntervalType.Long);
                }
                catch
                {
                    MessageBox.Show(this.resManager.GetString("BadIntervalType"), this.resManager.GetString("InvalidIntervalError"),
                       MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
            }

            if ((this.dataList.IntervalIsDisjunct(interval)) && (this.dataList.IntervalDisjunctWithCurrentEnums(interval)) && (this.IsIntervalDisjuctWithCurrent(interval)))
            {
                this.ListBoxIntervals.Items.Add(interval.ToString());
                this.currentCategory.AddInterval(interval);
            }
            else
            {
                MessageBox.Show(this.resManager.GetString("IntervalIsNotDisjunct"), this.resManager.GetString("InvalidIntervalError"),
                        MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        /// <summary>
        /// Handler for submitting the changed interval to categories list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            if (currentCategory.GetIntervals().Count > 0)
            {
                currentCategory.Name = TextBoxCategoryName.Text.ToString();
                currentCategory.Frequency = 12345;
                this.dataList.AddNewCategoryDirect(currentCategory);
                this.Dispose();
            }
            else
            {
                MessageBox.Show(this.resManager.GetString("NoEmptyCategoryAllowed"), this.resManager.GetString("InvalidIntervalError"),
                       MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        /// <summary>
        /// Handler for cancel button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CancelButton_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        #endregion


        #region Other private methods

        /// <summary>
        /// Method which ensures that all intervals in the category are of the same type
        /// </summary>
        protected void CheckIntervalTypesConsistency()
        {
            if (currentCategory.GetIntervals().Count > 0)
            {
                Interval temp = (Interval)currentCategory.GetIntervals()[0];
                switch (temp.intervalType)
                {
                    case IntervalType.Long:
                        this.RadioLong.Checked = true;
                        this.RadioFloat.Enabled = false;
                        this.RadioLong.Enabled = false;
                        break;

                    case IntervalType.Float:
                        this.RadioFloat.Checked = true;
                        this.RadioFloat.Enabled = false;
                        this.RadioLong.Enabled = false;
                        break;

                    default:
                        throw new Exception("Switch branch not implemented");
                }
            }
            else
            {
                this.RadioFloat.Enabled = true;
                this.RadioLong.Enabled = true;
            }
        }

        /// <summary>
        /// Method to check whether all interval parameters were entered.
        /// </summary>
        /// <returns></returns>
        protected bool DataIsValid()
        {
            //we expect the value and the type of the left bound, if it is not infinity
            if (!RadioMinusInfinity.Checked)
            {
                if ((TextBoxLeftBound.Text == "") || ((!RadioLeftBoundRound.Checked) && (!RadioLeftBoundSharp.Checked)))
                {
                    return false;
                }
            }
            if (!RadioPlusInfinity.Checked)
            {
                if ((TextBoxRightBound.Text == "") || ((!RadioRightBoundRound.Checked) && (!RadioRightBoundSharp.Checked)))
                {
                    return false;
                }
            }

            //we expect lower bound to be smaller than upper bound
            try
            {
                double left = Convert.ToDouble(TextBoxLeftBound.Text);
                double right = Convert.ToDouble(TextBoxRightBound.Text);
                if (left >= right)
                    return false;
            }
            catch
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Method for creating an interval based on inputted values
        /// </summary>
        /// <param name="intervalType"></param>
        /// <returns></returns>
        protected Interval MakeInterval(IntervalType intervalType)
        {
            IntervalBoundType leftBoundType = new IntervalBoundType();
            IntervalBoundType rightBoundType = new IntervalBoundType();
            int leftBound = 0;
            int rightBound = 0;
            bool isint = true;
            float leftDoubleBound = 0;
            float rightDoubleBound = 0;
            if (!RadioMinusInfinity.Checked)
            {
                if (RadioLeftBoundRound.Checked)
                {
                    leftBoundType = IntervalBoundType.Round;
                }
                else
                {
                    leftBoundType = IntervalBoundType.Sharp;
                }
                switch (intervalType)
                {
                    case IntervalType.Long:
                        try
                        {
                            leftBound = Convert.ToInt32(TextBoxLeftBound.Text);
                        }
                        catch
                        {
                            throw new ArgumentOutOfRangeException();
                        }
                        break;
                    case IntervalType.Float:
                        try
                        {
                            isint = false;
                            leftDoubleBound = (float)Convert.ToDouble(TextBoxLeftBound.Text);
                        }
                        catch
                        {
                            throw new ArgumentOutOfRangeException();
                        }
                        break;
                    default:
                        throw new Exception("Switch branch not implemented");
                }
            }
            else
            {
                leftBoundType = IntervalBoundType.Infinity;
            }
            if (!RadioPlusInfinity.Checked)
            {
                if (RadioRightBoundRound.Checked)
                {
                    rightBoundType = IntervalBoundType.Round;
                }
                else
                {
                    rightBoundType = IntervalBoundType.Sharp;
                }

                switch (intervalType)
                {
                    case IntervalType.Long:
                        try
                        {
                            rightBound = Convert.ToInt32(TextBoxRightBound.Text);
                        }
                        catch
                        {
                            throw new ArgumentOutOfRangeException();
                        }
                        break;

                    case IntervalType.Float:
                        try
                        {
                            isint = false;
                            rightDoubleBound = (float)Convert.ToDouble(TextBoxRightBound.Text);
                        }
                        catch
                        {
                            throw new ArgumentOutOfRangeException();
                        }
                        break;

                    default:
                        throw new Exception("Switch branch not implemented");
                }
            }
            else
            {
                rightBoundType = IntervalBoundType.Infinity;
            }

            Interval interval;
            if (isint)
            {
                interval = new Interval(IntervalType.Long);
                interval.lowerBound = leftBound;
                interval.upperBound = rightBound;
                interval.lowerBoundType = leftBoundType;
                interval.upperBoundType = rightBoundType;
                //interval = new Interval(leftBound, rightBound, leftBoundType, rightBoundType);
            }
            else
            {
                interval = new Interval(IntervalType.Float);
                interval.lowerBoundFl = leftDoubleBound;
                interval.upperBoundFl = rightDoubleBound;
                interval.lowerBoundType = leftBoundType;
                interval.upperBoundType = rightBoundType;
                //interval = new Interval(leftDoubleBound, rightDoubleBound, leftBoundType, rightBoundType);
            }

            return interval;
        }

        /// <summary>
        /// Method to check whether the interval is disjunct with currently available intervals in the category.
        /// </summary>
        /// <param name="interval">Interval to check for disjunctivity</param>
        /// <returns>True if the interval is disjunct</returns>
        protected bool IsIntervalDisjuctWithCurrent(Interval interval)
        {
            foreach (Interval inter in this.currentCategory.GetIntervals())
            {
                if (!inter.IntervalIsDisjunct(interval))

                    return false;
            }
            return true;
        }

        #endregion


        #region Localization

        /// <summary>
        /// Changes localization
        /// </summary>
        /// <param name="rm">Resource manager</param>
        private void ChangeLocale(ResourceManager rm)
        {
            this.RadioMinusInfinity.Text = rm.GetString("RadioMinusInfinity");
            this.RadioPlusInfinity.Text = rm.GetString("RadioPlusInfinity");
            this.RadioFloat.Text = rm.GetString("Float");
            this.RadioLong.Text = rm.GetString("Long");
            this.ButtonCancel.Text = rm.GetString("ButtonCancel");
            this.ButtonAddInterval.Text = rm.GetString("ButtonCheck");
            this.ButtonSubmit.Text = rm.GetString("ButtonSubmit");
            this.LabelLeftBoundType.Text = rm.GetString("LabelLeftBoundType");
            this.LabelLeftBoundValue.Text = rm.GetString("LabelLeftBoundValue");
            this.LabelRightBoundType.Text = rm.GetString("LabelRightBoundType");
            this.LabelRightBoundValue.Text = rm.GetString("LabelRightBoundValue");
            this.LabelNewName.Text = rm.GetString("LabelNewName");
            this.TextBoxCategoryName.Text = rm.GetString("NewName");
            this.LabelIntervals.Text = rm.GetString("LabelIntervals");
        }

        #endregion
    }
}