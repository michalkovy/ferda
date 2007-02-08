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
using Ferda.Guha.Attribute;
using Ferda.Guha.Data;

namespace Ferda.FrontEnd.AddIns.EditCategories.CreateNewCategory
{
    /// <summary>
    /// Class for creating interval categories
    /// </summary>
    public partial class CreateIntervalWizard : UserControl
    {

        #region Private variables

        /// <summary>
        /// Edited attribute
        /// </summary>
        protected Attribute<IComparable> attribute;

        /// <summary>
        /// Resource manager
        /// </summary>
        protected ResourceManager resManager;

        /// <summary>
        /// Category being added
        /// </summary>
        protected Category<IComparable> currentCategory;

        #endregion


        #region Constructor

        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="dataList">Datalist</param>
        /// <param name="rm">Resource manager</param>
        public CreateIntervalWizard(Attribute<IComparable> attribute, ResourceManager rm)
        {
            //setting the ResManager resource manager and localization string
            this.resManager = rm;
            this.attribute = attribute;
            InitializeComponent();
            this.ChangeLocale(this.resManager);
            this.AddHandlers();
            this.currentCategory = new Category<IComparable>(attribute);
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
                 /*try
                {
                    Interval<IComparable> tempInterval =
                        this.attribute[index].Intervals[(this.ListBoxIntervals.SelectedIndex)];
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
            */
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
                    this.currentCategory.Intervals.RemoveAt(this.ListBoxIntervals.SelectedIndex);
                    this.ListBoxIntervals.Items.RemoveAt(this.ListBoxIntervals.SelectedIndex);
                }
                catch
                {
                }
            }
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
            try
            {
                TryAddInterval();
                this.ListBoxIntervals.Items.Clear();
                foreach (Interval<IComparable> inter in currentCategory.Intervals)
                {
                    this.ListBoxIntervals.Items.Add(inter.ToString());
                    if (this.ListBoxIntervals.Items.Count > 0)
                    {
                        this.ListBoxIntervals.SelectedIndex = 0;
                    }
                }
            }
            catch (ArgumentOutOfRangeException ex)
            {
                MessageBox.Show(ex.Message, this.resManager.GetString("Error"),
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Generic exception",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

           /* try
            {
                
            }

            catch
            {
                MessageBox.Show(this.resManager.GetString("IntervalIsNotDisjunct"), this.resManager.GetString("InvalidIntervalError"),
                        MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }*/
        }

        /// <summary>
        /// Handler for submitting the changed interval to categories list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SubmitButton_Click(object sender, EventArgs e)
        {
           /* if (currentCategory.GetIntervals().Count > 0)
            {*/
                currentCategory.Reduce();
               // currentCategory.Frequency = 12345;
               // this.dataList.AddNewCategoryDirect(currentCategory);
                this.Dispose();
         /*   }
            else
            {
                MessageBox.Show(this.resManager.GetString("NoEmptyCategoryAllowed"), this.resManager.GetString("InvalidIntervalError"),
                       MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }*/
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
        /*
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
        }*/

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
                switch (attribute.DbDataType)
                {
                    case DbSimpleDataTypeEnum.BooleanSimpleType:
                    case DbSimpleDataTypeEnum.DoubleSimpleType:
                    case DbSimpleDataTypeEnum.FloatSimpleType:
                    case DbSimpleDataTypeEnum.IntegerSimpleType:
                    case DbSimpleDataTypeEnum.LongSimpleType:
                    case DbSimpleDataTypeEnum.ShortSimpleType:
                        {
                            double left = Convert.ToDouble(TextBoxLeftBound.Text);
                            double right = Convert.ToDouble(TextBoxRightBound.Text);
                            if (left >= right)
                                return false;
                        }
                        break;

                    case DbSimpleDataTypeEnum.DateTimeSimpleType:
                        {
                            DateTime left = Convert.ToDateTime(TextBoxLeftBound.Text);
                            DateTime right = Convert.ToDateTime(TextBoxRightBound.Text);
                            if (left >= right)
                                return false;
                        }
                        break;
                    case DbSimpleDataTypeEnum.StringSimpleType:
                        {
                            string left = TextBoxLeftBound.Text;
                            string right = TextBoxRightBound.Text;
                            if (left.CompareTo(right)>1)
                                return false;
                        }
                        break;

                    default:
                        return true;
                }
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
        protected void TryAddInterval()
        {
            BoundaryEnum leftBoundType = new BoundaryEnum();
            BoundaryEnum rightBoundType = new BoundaryEnum();
            IComparable leftBound = 0;
            IComparable rightBound = 0;

            if (!RadioMinusInfinity.Checked)
            {
                if (RadioLeftBoundRound.Checked)
                {
                    leftBoundType = BoundaryEnum.Open;
                }
                else
                {
                    leftBoundType = BoundaryEnum.Closed;
                }
                try
                {
                    switch (attribute.DbDataType)
                    {
                        case DbSimpleDataTypeEnum.BooleanSimpleType:
                            leftBound = (IComparable)Convert.ToBoolean(TextBoxLeftBound.Text);
                            break;

                        case DbSimpleDataTypeEnum.DateTimeSimpleType:
                            leftBound = (IComparable)Convert.ToDateTime(TextBoxLeftBound.Text);
                            break;

                        case DbSimpleDataTypeEnum.DoubleSimpleType:
                        case DbSimpleDataTypeEnum.FloatSimpleType:
                            leftBound = (IComparable)Convert.ToDouble(TextBoxLeftBound.Text);
                            break;

                        case DbSimpleDataTypeEnum.IntegerSimpleType:
                            leftBound = (IComparable)Convert.ToInt32(TextBoxLeftBound.Text);
                            break;

                        case DbSimpleDataTypeEnum.LongSimpleType:
                            leftBound = (IComparable)Convert.ToInt64(TextBoxLeftBound.Text);
                            break;

                        case DbSimpleDataTypeEnum.ShortSimpleType:
                            leftBound = (IComparable)Convert.ToInt16(TextBoxLeftBound.Text);
                            break;

                        case DbSimpleDataTypeEnum.StringSimpleType:
                            leftBound = (IComparable)TextBoxLeftBound.Text;
                            break;

                        default:
                            MessageBox.Show(this.resManager.GetString("InvalidIntervalType"), this.resManager.GetString("InvalidIntervalError"),
                        MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            break;
                    }
                }
                catch
                {
                    throw new ArgumentOutOfRangeException();
                }
            }
            else
            {
                leftBoundType = BoundaryEnum.Infinity;
            }
            if (!RadioPlusInfinity.Checked)
            {
                if (RadioRightBoundRound.Checked)
                {
                    rightBoundType = BoundaryEnum.Open;
                }
                else
                {
                    rightBoundType = BoundaryEnum.Closed;
                }
                try
                {
                    switch (attribute.DbDataType)
                    {
                        case DbSimpleDataTypeEnum.BooleanSimpleType:
                            rightBound = (IComparable)Convert.ToBoolean(TextBoxRightBound.Text);
                            break;

                        case DbSimpleDataTypeEnum.DateTimeSimpleType:
                            rightBound = (IComparable)Convert.ToDateTime(TextBoxRightBound.Text);
                            break;

                        case DbSimpleDataTypeEnum.DoubleSimpleType:
                        case DbSimpleDataTypeEnum.FloatSimpleType:
                            rightBound = (IComparable)Convert.ToDouble(TextBoxRightBound.Text);
                            break;

                        case DbSimpleDataTypeEnum.IntegerSimpleType:
                            rightBound = (IComparable)Convert.ToInt32(TextBoxRightBound.Text);
                            break;

                        case DbSimpleDataTypeEnum.LongSimpleType:
                            rightBound = (IComparable)Convert.ToInt64(TextBoxRightBound.Text);
                            break;

                        case DbSimpleDataTypeEnum.ShortSimpleType:
                            rightBound = (IComparable)Convert.ToInt16(TextBoxRightBound.Text);
                            break;

                        case DbSimpleDataTypeEnum.StringSimpleType:
                            rightBound = (IComparable)TextBoxRightBound.Text;
                            break;

                        default:
                            MessageBox.Show(this.resManager.GetString("InvalidIntervalType"), this.resManager.GetString("InvalidIntervalError"),
                        MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            break;
                    }
                }
                catch
                {
                    throw new ArgumentOutOfRangeException();
                }

            }
            else
            {
                rightBoundType = BoundaryEnum.Infinity;
            }
            this.currentCategory.Intervals.Add(leftBound, leftBoundType,
                rightBound, rightBoundType, false);
        }

       /* /// <summary>
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
        }*/

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
            //this.RadioFloat.Text = rm.GetString("Float");
           // this.RadioLong.Text = rm.GetString("Long");
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