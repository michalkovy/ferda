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
//using Ferda.FrontEnd.AddIns.EditCategories.NoGUIclasses;
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
        /// Temporary category name
        /// </summary>
        protected string tempName;

        /// <summary>
        /// Flag whether an interval is being edited
        /// </summary>
        protected bool editingInterval = false;

        #endregion


        #region Constructor

        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="dataList">Datalist</param>
        /// <param name="rm">Resource manager</param>
        public CreateIntervalWizard(Attribute<IComparable> attribute,
            string tempname, ResourceManager rm, EventHandler closeHandler)
        {
            //setting the ResManager resource manager and localization string
            this.resManager = rm;
            this.attribute = attribute;
            InitializeComponent();
            this.ChangeLocale(this.resManager);
            this.TextBoxLeftBound.LostFocus += new EventHandler(TextBoxLeftBound_TextEntered);
            this.TextBoxRightBound.LostFocus += new EventHandler(TextBoxRightBound_TextEntered);
            this.Disposed += closeHandler;

            if (tempname != String.Empty)
            {
                tempName = tempname;
            }
            else
            {
                tempName = RandomString.CreateKey(64);
                this.attribute.Add(tempName);
            }
        }

        #endregion


        #region Initialization


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

        private void ListBoxIntervals_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.ListBoxIntervals.SelectedIndices.Count > 0)
            {
                RadioEdit.Enabled = true;
            }
            else
            {
                RadioEdit.Enabled = false;
            }
        }

        private void RadioAddEdit_CheckedChanged(object sender, EventArgs e)
        {
            if (RadioAddNew.Checked)
            {
                editingInterval = false;
                ResetIntervalUI();
            }
            else
            {
                if (this.ListBoxIntervals.SelectedIndices.Count > 0)
                {
                    editingInterval = true;
                    Interval<IComparable> tempInterval;
                    try
                    {
                        tempInterval =
                            this.attribute[tempName].Intervals[(this.ListBoxIntervals.SelectedIndex)];
                    }

                    catch
                    {
                        return;
                    }
                    this.TextBoxLeftBound.Text = tempInterval.LeftValue.ToString();
                    this.TextBoxRightBound.Text = tempInterval.RightValue.ToString();

                    if (tempInterval.LeftBoundary == BoundaryEnum.Closed)
                    {
                        this.RadioLeftBoundSharp.Checked = true;
                    }
                    else if (tempInterval.LeftBoundary == BoundaryEnum.Infinity)
                    {
                        this.RadioMinusInfinity.Checked = true;
                    }
                    else
                    {
                        this.RadioLeftBoundRound.Checked = true;
                    }

                    if (tempInterval.RightBoundary == BoundaryEnum.Closed)
                    {
                        this.RadioRightBoundSharp.Checked = true;
                    }
                    else if (tempInterval.RightBoundary == BoundaryEnum.Infinity)
                    {
                        this.RadioPlusInfinity.Checked = true;
                    }
                    else
                    {
                        this.RadioRightBoundRound.Checked = true;
                    }
                    this.ButtonAddInterval.Text = resManager.GetString("ButtonEditSelectedInterval");
                }
                else
                {
                    editingInterval = false;
                    this.ButtonAddInterval.Text = resManager.GetString("ButtonCheck");
                    RadioEdit.Enabled = false;
                }
            }
        }


        /// <summary>
        /// Deletes the selected intervals from the category
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonDeleteInterval_Click(object sender, EventArgs e)
        {
            if (this.ListBoxIntervals.SelectedIndices.Count > 0)
            {
                try
                {
                    this.attribute[tempName].Intervals.RemoveAt(this.ListBoxIntervals.SelectedIndex);
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

            if (editingInterval)
            {
                Interval<IComparable> tempInterval;
                tempInterval =
                      this.attribute[tempName].Intervals[(this.ListBoxIntervals.SelectedIndex)];
                this.attribute[tempName].Intervals.RemoveAt(this.ListBoxIntervals.SelectedIndex);

                try
                {
                    //try to add an edited interval
                    TryAddInterval();
                    editingInterval = false;
                    ResetIntervalUI();
                }
                catch
                {
                    //if not successful, return the old interval back
                    this.attribute[tempName].Intervals.Add(tempInterval.LeftValue,
                        tempInterval.LeftBoundary,
                tempInterval.RightValue, tempInterval.RightBoundary, false);
                }
                finally
                {
                    this.ListBoxIntervals.Items.Clear();
                    foreach (Interval<IComparable> inter in attribute[tempName].Intervals)
                    {
                        this.ListBoxIntervals.Items.Add(inter.ToString());
                        if (this.ListBoxIntervals.Items.Count > 0)
                        {
                            this.ListBoxIntervals.SelectedIndex = 0;
                        }
                    }
                }
            }
            else
            {
                try
                {
                    TryAddInterval();
                    ResetIntervalUI();
                    this.ListBoxIntervals.Items.Clear();
                    foreach (Interval<IComparable> inter in attribute[tempName].Intervals)
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
            }
#if MONO
#else
            this.ListBoxIntervals.SelectedItems.Clear();
#endif
        }

        /// <summary>
        /// Resets interval ui to default values
        /// </summary>
        void ResetIntervalUI()
        {
            this.TextBoxLeftBound.Text = String.Empty;
            this.TextBoxRightBound.Text = String.Empty;
            this.RadioLeftBoundRound.Checked = true;
            this.RadioRightBoundRound.Checked = true;
            this.ButtonAddInterval.Text = resManager.GetString("ButtonCheck");
            this.RadioAddNew.Checked = true;
        }


        /// <summary>
        /// Handler for submitting the changed interval to categories list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            attribute[tempName].Reduce();
            if ((attribute[tempName].Enumeration.Count == 0) && (attribute[tempName].Intervals.Count == 0))
            {
                MessageBox.Show(this.resManager.GetString("EmptyCategory"),
                   this.resManager.GetString("Error"),
                   MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            try
            {
                this.attribute.RenameCategory(tempName, this.TextBoxCategoryName.Text);
            }
            catch
            {
                MessageBox.Show(this.resManager.GetString("SameCategoryNames"), this.resManager.GetString("Error"),
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            this.Dispose();
        }

        /// <summary>
        /// Handler for cancel button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CancelButton_Click(object sender, EventArgs e)
        {
            this.attribute.Remove(tempName);
            this.Dispose();
        }

        #endregion


        #region Other private methods

        /// <summary>
        /// Method to check whether all interval parameters were entered.
        /// </summary>
        /// <returns></returns>
        protected bool DataIsValid()
        {
            if ((RadioMinusInfinity.Checked) && (RadioPlusInfinity.Checked))
                return true;
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
                            double left = 0;
                            double right = 0;
                            if (!RadioMinusInfinity.Checked)
                                left = Convert.ToDouble(TextBoxLeftBound.Text);

                            if (!RadioPlusInfinity.Checked)
                                right = Convert.ToDouble(TextBoxRightBound.Text);

                            if ((!RadioMinusInfinity.Checked) && (!RadioPlusInfinity.Checked))
                            {
                                if (left >= right)
                                    return false;
                            }
                            else
                            {
                                return true;
                            }
                        }
                        break;

                    case DbSimpleDataTypeEnum.DateTimeSimpleType:
                        {
                            DateTime left = new DateTime();
                            DateTime right = new DateTime();
                            if (!RadioMinusInfinity.Checked)
                                left = Convert.ToDateTime(TextBoxLeftBound.Text);
                            if (!RadioPlusInfinity.Checked)
                                right = Convert.ToDateTime(TextBoxRightBound.Text);

                            if ((!RadioMinusInfinity.Checked) && (!RadioPlusInfinity.Checked))
                            {
                                if (left >= right)
                                    return false;
                            }
                            else
                            {
                                return true;
                            }
                        }
                        break;
                    case DbSimpleDataTypeEnum.StringSimpleType:
                        {
                            string left = String.Empty;
                            string right = String.Empty;
                            if (!RadioMinusInfinity.Checked)
                                left = TextBoxLeftBound.Text;

                            if (!RadioPlusInfinity.Checked)
                                right = TextBoxRightBound.Text;
                            if ((!RadioMinusInfinity.Checked) && (!RadioPlusInfinity.Checked))
                            {
                                if (left.CompareTo(right) > 1)
                                    return false;

                            }
                            else
                            {
                                return true;
                            }
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
                leftBound = (IComparable)1;
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
            this.attribute[tempName].Intervals.Add(leftBound, leftBoundType,
                rightBound, rightBoundType, false);
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
            //this.RadioFloat.Text = rm.GetString("Float");
            // this.RadioLong.Text = rm.GetString("Long");
            this.ButtonCancel.Text = rm.GetString("ButtonCancel");
            this.ButtonAddInterval.Text = rm.GetString("ButtonCheck");
            this.ButtonDeleteInterval.Text = rm.GetString("ButtonDeleteInterval");
            this.ButtonSubmit.Text = rm.GetString("ButtonSubmit");
            this.LabelLeftBoundType.Text = rm.GetString("LabelLeftBoundType");
            this.LabelLeftBoundValue.Text = rm.GetString("LabelLeftBoundValue");
            this.LabelRightBoundType.Text = rm.GetString("LabelRightBoundType");
            this.LabelRightBoundValue.Text = rm.GetString("LabelRightBoundValue");
            this.RadioAddNew.Text = rm.GetString("RadioAdd");
            this.RadioEdit.Text = rm.GetString("RadioEdit");

            this.LabelNewName.Text = rm.GetString("LabelNewName");
            this.TextBoxCategoryName.Text = rm.GetString("NewName");
            this.LabelIntervals.Text = rm.GetString("LabelIntervals");
        }

        #endregion
    }
}
