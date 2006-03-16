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

namespace Ferda
{
    namespace FrontEnd.AddIns.EditCategories.EditExisting
    {
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

                this.currentMultiSet = editedInterval;
                this.index = datalist.GetIndex(this.currentMultiSet);
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
                this.datalist.RemoveMultiSet(index);
                InitializeComponent();
            }

            #endregion


            #region Button handlers

            protected void ButtonNewCancel_Click(object sender, EventArgs e)
            {
                this.datalist.AddNewMultiSetDirect(this.interval);
                this.Dispose();
            }

            #endregion
        }
    }
}