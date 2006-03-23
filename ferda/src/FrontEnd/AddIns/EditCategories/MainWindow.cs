// MainWindow.cs - Windows.Form class
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

#region using...

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Ferda.FrontEnd.AddIns.EditCategories.NoGUIclasses;
using Ferda.FrontEnd.AddIns.EditCategories.CreateNewCategory;
using Ferda;
using Ferda.Modules;
using Ferda.FrontEnd.AddIns;
using Ferda.FrontEnd.AddIns.EditCategories.EditExisting;
using System.Resources;
using System.Reflection;

#endregion

namespace Ferda.FrontEnd.AddIns.EditCategories
{
    /// <summary>
    /// Class which displays categories and tools to work with them
    /// </summary>
    public partial class MainListView : System.Windows.Forms.Form, Ferda.FrontEnd.IIconProvider
    {
        #region Private variables

        /// <summary>
        /// Datalist which is displayed in the EditCategoriesListView
        /// </summary>
        FerdaSmartDataList bigDataList;

        /// <summary>
        /// Categories to put in the datalist
        /// </summary>
        CategoriesStruct returnCategories = new CategoriesStruct();

        /// <summary>
        /// Resource manager
        /// </summary>
        private ResourceManager resManager;

        /// <summary>
        /// Localization string, en-US or cs-CZ for now.
        /// </summary>
        private string localizationString;

        /// <summary>
        /// All the possible values for the datalist
        /// </summary>
        private string[] distinctValues;

        /// <summary>
        /// Private variable indicating that a category is being edited
        /// </summary>
        bool EditInProgress = false;

        /// <summary>
        /// Current path (for icon loading)
        /// </summary>
        private string path;

        /// <summary>
        /// Dictionary that contains all the icons for the application, ]
        /// that are keyed by string values. See 
        /// <see cref="F:Ferda.FrontEnd.FerdaForm.LoadIcons"/> for their names
        /// </summary>
        private Dictionary<string, Icon> iconProvider;

        /// <summary>
        /// Owner of addin
        /// </summary>
        private IOwnerOfAddIn ownerOfAdd;


        #endregion


        #region Constructor

        /// <summary>
        /// Contructor which creates editcategories instance and fills in the needed values
        /// </summary>
        /// <param name="localePrefs">Current locale</param>
        /// <param name="categories">Categories to edit</param>
        /// <param name="distinctValues">Distinct values for enum categories</param>
        public MainListView(string[] localePrefs, CategoriesStruct categories, string[] distinctValues, IOwnerOfAddIn ownerOfAddin)
        {
            //setting the ResManager resource manager and localization string
            string locale;
            try
            {
                locale = localePrefs[0];
                localizationString = locale;
                locale = "Ferda.FrontEnd.AddIns.EditCategories.Localization_" + locale;
                resManager = new ResourceManager(locale, Assembly.GetExecutingAssembly());
            }
            catch
            {
                this.resManager = new ResourceManager("Ferda.FrontEnd.AddIns.EditCategories.Localization_en-US",
            Assembly.GetExecutingAssembly());
                localizationString = "en-US";
            }
            this.distinctValues = distinctValues;
            this.ownerOfAdd = ownerOfAddin;
            this.path = Assembly.GetExecutingAssembly().Location;
            InitializeComponent();
            this.ChangeLocale(this.resManager);
            bigDataList = this.MyIceRun(categories);
            FillEditCategoriesListView(CategoriesListView);
            //adding a handling method for column sorting
            this.CategoriesListView.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.listView1_ColumnClick);
            this.LoadIcons();
            this.InitIcons();
        }

        #endregion


        #region Initialization

        /// <summary>
        /// Method for setting handlers for context menu and toolstrip buttons
        /// </summary>
        private void ContextMenuToolStripHandlers()
        {
            //context menu handlers initialization
            this.MenuItemNewEnum.Click += new EventHandler(AddNewEnumeration_Click);
            this.MenuItemNewInterval.Click += new EventHandler(AddNewInterval_Click);
            this.MenuItemJoin.Click += new EventHandler(Join_Click);
            this.MenuItemDelete.Click += new EventHandler(Delete_Click);
            this.MenuItemSaveAndQuit.Click += new EventHandler(SaveAndQuit_Click);
            this.MenuItemQuitWithoutSave.Click += new EventHandler(QuitWithoutSave_Click);
            this.ButtonDelete.Click += new EventHandler(Delete_Click);
            this.ButtonJoin.Click += new EventHandler(Join_Click);
            this.ButtonQuitWithoutSave.Click += new EventHandler(QuitWithoutSave_Click);
            this.ButtonSaveAndQuit.Click += new EventHandler(SaveAndQuit_Click);
            this.ToolMenuItemNewEnumeration.Click += new EventHandler(AddNewEnumeration_Click);
            this.ToolMenuItemNewInterval.Click += new EventHandler(AddNewInterval_Click);
            this.CategoriesListView.AfterLabelEdit += new LabelEditEventHandler(EditLabelHandler);
            this.MenuItemEdit.Click += new EventHandler(EditItem);
            this.ButtonEdit.Click += new EventHandler(EditItem);
            this.CategoriesListView.SelectedIndexChanged += new EventHandler(OnSelectedIndexChanged);
            this.MenuItemRename.Click += new EventHandler(Rename_Click);
        }

        /// <summary>
        /// Method for loading icons
        /// </summary>
        private void InitIcons()
        {
            this.Icon = iconProvider["FerdaIcon"];
            this.ButtonDelete.Image = iconProvider["DeleteIcon"].ToBitmap();
            this.MenuItemDelete.Image = iconProvider["DeleteIcon"].ToBitmap();
            this.ButtonEdit.Image = iconProvider["EditIcon"].ToBitmap();
            this.MenuItemEdit.Image = iconProvider["EditIcon"].ToBitmap();
            this.ButtonJoin.Image = iconProvider["JoinIcon"].ToBitmap();
            this.MenuItemJoin.Image = iconProvider["JoinIcon"].ToBitmap();
            this.ButtonNew.Image = iconProvider["NewIcon"].ToBitmap();
            this.MenuItemNew.Image = iconProvider["NewIcon"].ToBitmap();
            this.ButtonQuitWithoutSave.Image = iconProvider["QuitWithoutSaveIcon"].ToBitmap();
            this.MenuItemQuitWithoutSave.Image = iconProvider["QuitWithoutSaveIcon"].ToBitmap();
            this.ButtonSaveAndQuit.Image = iconProvider["SaveAndQuitIcon"].ToBitmap();
            this.MenuItemSaveAndQuit.Image = iconProvider["SaveAndQuitIcon"].ToBitmap();
            this.MenuItemRename.Image = iconProvider["RenameIcon"].ToBitmap();
        }

        /// <summary>
        /// Method for filling EditCategoriesListView with data and setting some handlers
        /// </summary>
        /// <param name="listView"></param>
        private void FillEditCategoriesListView(ListView listView)
        {
            ContextMenuToolStripHandlers();
            FerdaSmartDataList dataList = bigDataList;
            // this.splitContainer1.Panel2MinSize = 0;
            dataList.StructureChange += new FerdaEvent(RefreshList);
            listView.MouseDoubleClick += new MouseEventHandler(EditItem);
            SmartDataListToListView(dataList, CategoriesListView);
        }

        #endregion


        #region Datalist to ListView methods

        /// <summary>
        /// Method to display FerdaSmartDataList using a ListView component
        /// </summary>
        /// <param name="smartList">SmartDataList to display</param>
        /// <param name="form">Form to display the SmartDataList in</param>
        public void SmartDataListToListView(FerdaSmartDataList smartList, ListView listView)
        {
            foreach (Category multiSet in smartList.Categories)
            {
                ListViewItem item = new ListViewItem(multiSet.Name, 1);
                switch (multiSet.CatType)
                {
                    case CategoryType.Interval:
                        item.SubItems.Add(this.resManager.GetString("TypeInterval"));
                        break;
                    case CategoryType.Enumeration:
                        item.SubItems.Add(this.resManager.GetString("TypeSet"));
                        break;
                    default:
                        throw new Exception("Switch branch not implemented");
                }
                item.SubItems.Add(multiSet.ToString());
                //tag contains index of the value in the array of multisets
                item.Tag = smartList.GetIndex(multiSet);
                listView.Items.Add(item);
            }
        }

        /// <summary>
        /// Method to refresh items in the EditCategoriesListView
        /// </summary>
        private void RefreshList()
        {
            CategoriesListView.Items.Clear();
            SmartDataListToListView(bigDataList, CategoriesListView);
        }

        #endregion


        #region Button and menu handlers

        /// <summary>
        /// Rename handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Rename_Click(object sender, EventArgs e)
        {
            if (this.CategoriesListView.SelectedItems.Count > 0)
            {
                this.CategoriesListView.SelectedItems[0].BeginEdit();
            }
        }

        /// <summary>
        /// Method to handle listview items activation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSelectedIndexChanged(object sender, EventArgs e)
        {
            ListView view = (ListView)sender;
            if (view.SelectedItems.Count > 0)
            {
                if (view.SelectedItems.Count > 1)
                {
                    //two and more items are selected, thus nothing can be splitted, only joined,
                    //if the type is the same
                    this.ButtonDelete.Enabled = false;
                    this.ButtonEdit.Enabled = false;
                    this.MenuItemRename.Enabled = false;
                    this.MenuItemDelete.Enabled = false;
                    this.MenuItemEdit.Enabled = false;
                    int[] indexes = new int[view.SelectedItems.Count];
                    for (int i = 0; i < view.SelectedItems.Count; i++)
                    {
                        try
                        {
                            indexes[i] = (int)view.SelectedItems[i].Tag;
                        }
                        catch
                        {
                            return;
                        }
                    }
                    if (bigDataList.SameTypeCategories(indexes))
                    {
                        this.ButtonJoin.Enabled = true;
                        this.MenuItemJoin.Enabled = true;
                    }
                }
                else
                {
                    //only one item is selected, check if it can be splitted
                    this.ButtonDelete.Enabled = true;
                    this.ButtonEdit.Enabled = true;
                    this.ButtonJoin.Enabled = false;
                    this.MenuItemDelete.Enabled = true;
                    this.MenuItemEdit.Enabled = true;
                    this.MenuItemJoin.Enabled = false;
                    this.MenuItemRename.Enabled = true;
                }
            }
            else
            {
                //if nothing is selected, only save&quit, nosave&quit, new and flatten actions have sense
                this.ButtonDelete.Enabled = false;
                this.ButtonEdit.Enabled = false;
                this.ButtonJoin.Enabled = false;
                this.MenuItemRename.Enabled = false;
                this.MenuItemDelete.Enabled = false;
                this.MenuItemEdit.Enabled = false;
                this.MenuItemJoin.Enabled = false;
            }
        }

        /// <summary>
        /// Method which closes the window and returns the modified categories.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveAndQuit_Click(object sender, EventArgs e)
        {
            CategoriesStruct categories = new CategoriesStruct();
            try
            {
                this.returnCategories = this.MyIceRunOut(this.bigDataList);
            }
            catch (System.ApplicationException)
            {
                MessageBox.Show(resManager.GetString("SameCategoryNames"), resManager.GetString("Error"),

                          MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            categories = this.returnCategories;
            DialogResult = DialogResult.OK;
            this.Dispose();
        }

        /// <summary>
        /// Method which closes the window without categories modification.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void QuitWithoutSave_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            this.Dispose();
        }

        /// <summary>
        /// Handler for join.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Join_Click(object sender, EventArgs e)
        {
            ArrayList selected = new ArrayList();
            if (this.CategoriesListView.SelectedItems.Count > 1)
            {
                foreach (int index in this.CategoriesListView.SelectedIndices)
                {
                    selected.Add((int)this.CategoriesListView.Items[index].Tag);
                }
                bigDataList.JoinCategories(selected);
            }
        }

        /// <summary>
        /// Refreshes the listview.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Refr_Click(object sender, EventArgs e)
        {
            this.RefreshList();
        }

        /// <summary>
        /// Method will handle a request to edit the selected category.
        /// </summary>
        internal void EditItem(object sender, EventArgs e)
        {
            if (!EditInProgress)
            {
                EditInProgress = true;
                int index = 0;
                try
                {
                    index = (int)this.CategoriesListView.SelectedItems[0].Tag;
                    this.ButtonDelete.Enabled = false;
                    this.ButtonJoin.Enabled = false;
                    this.ButtonNew.Enabled = false;
                    this.ButtonEdit.Enabled = false;
                    this.MenuItemDelete.Enabled = false;
                    this.MenuItemJoin.Enabled = false;
                    this.MenuItemNew.Enabled = false;
                    this.MenuItemEdit.Enabled = false;
                    EditExistingCategory editCategory = new EditExistingCategory(index, bigDataList, this, this.resManager);
                }
                catch
                {
                    EditInProgress = false;
                    return;
                }
            }
        }


        /// <summary>
        /// Handler for label editing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EditLabelHandler(object sender, LabelEditEventArgs e)
        {
            int index = 0;
            try
            {
                index = (int)this.CategoriesListView.Items[e.Item].Tag;
                e.CancelEdit = true;
                this.bigDataList.SetName(index, e.Label);
            }
            catch
            {
                return;
            }
        }

        /// <summary>
        /// Handler for delete.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Delete_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in CategoriesListView.SelectedItems)
            {
                this.bigDataList.RemoveCategory((int)item.Tag);
                item.Remove();
            }
            CategoriesListView.SelectedItems.Clear();
        }

        /// <summary>
        /// Adds a control for creating a new enum to the form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddNewEnumeration_Click(object sender, EventArgs e)
        {
            CreateNewCategory.CreateSetWizard newEnum = new CreateSetWizard(bigDataList, this.resManager);
            this.SuspendLayout();
            newEnum.Name = "CreateSetWizard";
            newEnum.TabIndex = 2;
            newEnum.Disposed += new EventHandler(ListViewReinitSize);
            newEnum.Dock = DockStyle.Right;
            this.splitContainer1.Panel2Collapsed = false;
            this.splitContainer1.Panel2.Controls.Add(newEnum);
            this.MenuItemNew.Enabled = false;
            this.ButtonNew.Enabled = false;
            this.ButtonEdit.Enabled = false;
            this.MenuItemEdit.Enabled = false;
            this.DoubleClick -= new EventHandler(EditItem);
            this.ResumeLayout();
            newEnum.BringToFront();
        }

        /// <summary>
        /// Adds a control for creating a new interval to the form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddNewInterval_Click(object sender, EventArgs e)
        {
            CreateIntervalWizard newInterval1 = new CreateIntervalWizard(bigDataList, this.resManager);
            this.SuspendLayout();
            newInterval1.Name = "CreateIntervalWizard";
            newInterval1.TabIndex = 2;
            newInterval1.Disposed += new EventHandler(ListViewReinitSize);
            newInterval1.Dock = System.Windows.Forms.DockStyle.Right;
            this.splitContainer1.Panel2Collapsed = false;
            this.splitContainer1.Panel2.Controls.Add(newInterval1);
            this.MenuItemNew.Enabled = false;
            this.ButtonNew.Enabled = false;
            this.ButtonEdit.Enabled = false;
            this.MenuItemEdit.Enabled = false;
            this.DoubleClick -= new EventHandler(EditItem);
            this.ResumeLayout();
            newInterval1.BringToFront();
        }


        #endregion


        #region Other handlers and ListView methods

        /// <summary>
        /// ColumnClick handler - sorting
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listView1_ColumnClick(object sender, System.Windows.Forms.ColumnClickEventArgs e)
        {
            ListViewItemComparer columnSorter = new ListViewItemComparer();
            columnSorter.column = e.Column;

            if ((columnSorter.bAscending = (CategoriesListView.Sorting == SortOrder.Ascending)))
                CategoriesListView.Sorting = SortOrder.Descending;
            else
                CategoriesListView.Sorting = SortOrder.Ascending;

            CategoriesListView.ListViewItemSorter = columnSorter;
        }

        /// <summary>
        /// Method for re-instating the EditCategoriesListView size and menus status
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        internal void ListViewReinitSize(object sender, EventArgs e)
        {
            this.ButtonDelete.Enabled = false;
            this.ButtonNew.Enabled = true;
            this.MenuItemDelete.Enabled = false;
            this.MenuItemNew.Enabled = true;
            EditInProgress = false;
            this.SuspendLayout();
            this.CategoriesListView.Size = new System.Drawing.Size(830, 427);
            this.splitContainer1.Panel2Collapsed = true;
            this.ResumeLayout();
        }

        private void ToolStripButtonHelp_Click(object sender, EventArgs e)
        {
            ownerOfAdd.OpenPdf(ownerOfAdd.GetBinPath() + "\\AddIns\\Help\\EditCategories.pdf");
        }

        #endregion


        #region Conversion methods

        /// <summary>
        /// Method to initialize listview using ice structures
        /// </summary>
        /// <param name="existingCategories">Categories to fill in FerdaSmartDataList</param>
        /// <returns>FerdaSmartDataList with categories</returns>
        private FerdaSmartDataList MyIceRun(Modules.CategoriesStruct existingCategories)
        {
            //here we need to fill our data structure with data from ice
            ArrayList allValues = new ArrayList();
            if (this.distinctValues != null)
            {
                foreach (string value in this.distinctValues)
                {
                    allValues.Add(value);
                }
            }

            FerdaSmartDataList returnList = new FerdaSmartDataList(allValues, new ArrayList());
            foreach (DictionaryEntry myEnumCategorySeq in existingCategories.enums)
            {
                Category newMultiset = new Category();
                newMultiset.CatType = CategoryType.Enumeration;
                newMultiset.Name = myEnumCategorySeq.Key.ToString();
                ArrayList tempArray = new ArrayList();
                String[] StringSeq = (String[])myEnumCategorySeq.Value;
                foreach (string entry in StringSeq)
                {
                    tempArray.Add(entry);
                }
                SingleSet newSet = new SingleSet(tempArray);
                newMultiset.AddSingleSet(newSet);
                returnList.AddNewCategoryDirect(newMultiset);
            }

            foreach (DictionaryEntry myLongIntervalCategorySeq in existingCategories.longIntervals)
            {
                Category newMultiset = new Category();
                newMultiset.CatType = CategoryType.Interval;
                newMultiset.Name = myLongIntervalCategorySeq.Key.ToString();
                LongIntervalStruct[] myLongIntervalStructSeq = (LongIntervalStruct[])myLongIntervalCategorySeq.Value;
                foreach (LongIntervalStruct myLongIntervalStruct in myLongIntervalStructSeq)
                {
                    IntervalBoundType ubt, lbt;
                    if (myLongIntervalStruct.leftBoundType == BoundaryEnum.Infinity)
                    {
                        lbt = IntervalBoundType.Infinity;
                    }
                    else
                    {
                        if (myLongIntervalStruct.leftBoundType == BoundaryEnum.Round)
                        {
                            lbt = IntervalBoundType.Round;
                        }
                        else
                        {
                            lbt = IntervalBoundType.Sharp;
                        }
                    }
                    if (myLongIntervalStruct.rightBoundType == BoundaryEnum.Infinity)
                    {
                        ubt = IntervalBoundType.Infinity;
                    }
                    else
                    {
                        if (myLongIntervalStruct.rightBoundType == BoundaryEnum.Round)
                        {
                            ubt = IntervalBoundType.Round;
                        }
                        else
                        {
                            ubt = IntervalBoundType.Sharp;
                        }
                        Interval newInterval = new Interval(IntervalType.Long);
                        newInterval.lowerBound = myLongIntervalStruct.leftBound;
                        newInterval.upperBound = myLongIntervalStruct.rightBound;
                        newInterval.lowerBoundType = lbt;
                        newInterval.upperBoundType = ubt;
                        //Interval newInterval = new Interval((int)myLongIntervalStruct.leftBound, (int)myLongIntervalStruct.rightBound, lbt, ubt);
                        //  newInterval.intervalType = IntervalType.Long
                        newMultiset.AddInterval(newInterval);
                    }
                    returnList.AddNewCategoryDirect(newMultiset);
                }
            }

            foreach (DictionaryEntry myFloatIntervalCategorySeq in existingCategories.floatIntervals)
            {
                Category newMultiset = new Category();
                newMultiset.CatType = CategoryType.Interval;
                newMultiset.Name = myFloatIntervalCategorySeq.Key.ToString();
                FloatIntervalStruct[] myFloatIntervalStructSeq = (FloatIntervalStruct[])myFloatIntervalCategorySeq.Value;
                foreach (FloatIntervalStruct myFloatIntervalStruct in myFloatIntervalStructSeq)
                {
                    IntervalBoundType ubt, lbt;
                    if (myFloatIntervalStruct.leftBoundType == BoundaryEnum.Infinity)
                    {
                        lbt = IntervalBoundType.Infinity;
                    }
                    else
                    {
                        if (myFloatIntervalStruct.leftBoundType == BoundaryEnum.Round)
                        {
                            lbt = IntervalBoundType.Round;
                        }
                        else
                        {
                            lbt = IntervalBoundType.Sharp;
                        }
                    }
                    if (myFloatIntervalStruct.rightBoundType == BoundaryEnum.Infinity)
                    {
                        ubt = IntervalBoundType.Infinity;
                    }
                    else
                    {
                        if (myFloatIntervalStruct.rightBoundType == BoundaryEnum.Round)
                        {
                            ubt = IntervalBoundType.Round;
                        }
                        else
                        {
                            ubt = IntervalBoundType.Sharp;
                        }
                        Interval newInterval = new Interval(IntervalType.Float);
                        newInterval.lowerBoundFl = myFloatIntervalStruct.leftBound;
                        newInterval.upperBoundFl = myFloatIntervalStruct.rightBound;
                        newInterval.lowerBoundType = lbt;
                        newInterval.upperBoundType = ubt;
                        //     Interval newInterval = new Interval(myFloatIntervalStruct.leftBound, myFloatIntervalStruct.rightBound, lbt, ubt);
                        newMultiset.AddInterval(newInterval);
                    }
                    returnList.AddNewCategoryDirect(newMultiset);
                }
            }
            return returnList;
        }

        /// <summary>
        /// Method to convert SmartDataList to CategoriesStruct structure.
        /// </summary>
        /// <param name="dataList"></param>
        /// <returns></returns>
        private CategoriesStruct MyIceRunOut(FerdaSmartDataList dataList)
        {
            CategoriesStruct myCategoriesStruct = new CategoriesStruct();
            myCategoriesStruct.dateTimeIntervals = new DateTimeIntervalCategorySeq();
            myCategoriesStruct.enums = new EnumCategorySeq();
            myCategoriesStruct.floatIntervals = new FloatIntervalCategorySeq();
            myCategoriesStruct.longIntervals = new LongIntervalCategorySeq();
            ArrayList tempArray = new ArrayList();
            foreach (Category multiSet in dataList.Categories)
            {
                switch (multiSet.CatType)
                {
                    case CategoryType.Interval:
                        foreach (Interval interval in multiSet.GetIntervals())
                        {
                            if (interval.intervalType == IntervalType.Long)
                            {
                                LongIntervalStruct newLong = new LongIntervalStruct();
                                newLong.leftBound = interval.lowerBound;
                                newLong.rightBound = interval.upperBound;
                                if (interval.lowerBoundType == IntervalBoundType.Round)
                                {
                                    newLong.leftBoundType = BoundaryEnum.Round;
                                }
                                else
                                {
                                    if (interval.lowerBoundType == IntervalBoundType.Sharp)
                                    {
                                        newLong.leftBoundType = BoundaryEnum.Sharp;
                                    }
                                    else
                                    {
                                        newLong.leftBoundType = BoundaryEnum.Infinity;
                                    }
                                }
                                if (interval.upperBoundType == IntervalBoundType.Round)
                                {
                                    newLong.rightBoundType = BoundaryEnum.Round;
                                }
                                else
                                {
                                    if (interval.upperBoundType == IntervalBoundType.Sharp)
                                    {
                                        newLong.rightBoundType = BoundaryEnum.Sharp;
                                    }
                                    else
                                    {
                                        newLong.rightBoundType = BoundaryEnum.Infinity;
                                    }
                                }
                                tempArray.Add(newLong);
                            }
                            else
                            {
                                if (interval.intervalType == IntervalType.Float)
                                {
                                    FloatIntervalStruct newFloat = new FloatIntervalStruct();
                                    newFloat.leftBound = interval.lowerBoundFl;
                                    newFloat.rightBound = interval.upperBoundFl;
                                    if (interval.lowerBoundType == IntervalBoundType.Round)
                                    {
                                        newFloat.leftBoundType = BoundaryEnum.Round;
                                    }
                                    else
                                    {
                                        if (interval.lowerBoundType == IntervalBoundType.Sharp)
                                        {
                                            newFloat.leftBoundType = BoundaryEnum.Sharp;
                                        }
                                        else
                                        {
                                            newFloat.leftBoundType = BoundaryEnum.Infinity;
                                        }
                                    }
                                    if (interval.upperBoundType == IntervalBoundType.Round)
                                    {
                                        newFloat.rightBoundType = BoundaryEnum.Round;
                                    }
                                    else
                                    {
                                        if (interval.upperBoundType == IntervalBoundType.Sharp)
                                        {
                                            newFloat.rightBoundType = BoundaryEnum.Sharp;
                                        }
                                        else
                                        {
                                            newFloat.rightBoundType = BoundaryEnum.Infinity;
                                        }
                                    }
                                    tempArray.Add(newFloat);
                                }
                            }
                        }
                        if (multiSet.GetIntervalType() == IntervalType.Long)
                        {
                            myCategoriesStruct.longIntervals.Add(multiSet.Name, (LongIntervalStruct[])tempArray.ToArray(typeof(LongIntervalStruct)));
                            tempArray.Clear();
                        }
                        else
                        {
                            if (multiSet.GetIntervalType() == IntervalType.Float)
                            {
                                myCategoriesStruct.floatIntervals.Add(multiSet.Name, (FloatIntervalStruct[])tempArray.ToArray(typeof(FloatIntervalStruct)));
                                tempArray.Clear();
                            }
                        }
                        break;
                    case CategoryType.Enumeration:
                        tempArray = multiSet.Set.Values;
                        String[] tempString = new String[tempArray.Count];
                        for (int i = 0; i < tempArray.Count; i++)
                        {
                            tempString[i] = tempArray[i].ToString();
                        }
                        try
                        {
                            myCategoriesStruct.enums.Add(multiSet.Name, tempString);
                        }
                        catch (System.ArgumentException)
                        {
                            throw (new System.ApplicationException());
                        }
                        tempArray.Clear();
                        break;
                    default:
                        break;
                }
            }
            return myCategoriesStruct;
        }

        /// <summary>
        /// Method which returns updated categories
        /// </summary>
        /// <returns></returns>
        public CategoriesStruct GetUpdatedCategories()
        {
            return this.returnCategories;
        }

        #endregion


        #region Localization
        /// <summary>
        /// Resource manager of the application, it is filled according to the
        /// current localization
        /// </summary>
        public ResourceManager ResManager
        {
            get
            {
                return resManager;
            }
        }

        /// <summary>
        /// Localization string of the application, possible values are "en-US" and "cs-CZ"
        /// </summary>
        public string LocalizationString
        {
            get
            {
                return localizationString;
            }
        }

        /// <summary>
        /// Method to change l10n.
        /// </summary>
        /// <param name="rm">Resource manager to handle new l10n resource</param>
        private void ChangeLocale(ResourceManager rm)
        {
            this.ColumnCategoryName.Text = rm.GetString("ColumnCategoryName");
            this.ColumnCategoryType.Text = rm.GetString("ColumnCategoryType");
            this.ColumnCategoryValue.Text = rm.GetString("ColumnCategoryValue");
            this.ColumnFrequency.Text = rm.GetString("ColumnFrequency");
            this.ButtonDelete.Text = rm.GetString("ButtonDelete");
            this.ButtonJoin.Text = rm.GetString("ButtonJoin");
            this.ButtonNew.Text = rm.GetString("ButtonNew");
            this.ButtonQuitWithoutSave.Text = rm.GetString("ButtonQuitWithoutSave");
            this.ButtonSaveAndQuit.Text = rm.GetString("ButtonSaveAndQuit");
            this.ToolMenuItemNewEnumeration.Text = rm.GetString("NewEnumeration");
            this.ToolMenuItemNewInterval.Text = rm.GetString("NewInterval");
            this.ButtonEdit.Text = rm.GetString("ButtonEdit");
            this.Text = rm.GetString("EditCategoriesAbout");
            this.MenuItemJoin.Text = rm.GetString("ButtonJoin");
            this.MenuItemNew.Text = rm.GetString("ButtonNew");
            this.MenuItemNewEnum.Text = rm.GetString("NewEnumeration");
            this.MenuItemNewInterval.Text = rm.GetString("NewInterval");
            this.MenuItemQuitWithoutSave.Text = rm.GetString("ButtonQuitWithoutSave");
            this.MenuItemSaveAndQuit.Text = rm.GetString("ButtonSaveAndQuit");
            this.MenuItemDelete.Text = rm.GetString("ButtonDelete");
            this.MenuItemEdit.Text = rm.GetString("ButtonEdit");
            this.MenuItemRename.Text = rm.GetString("ButtonRename");
            this.ToolStripButtonHelp.Text = rm.GetString("Help");
        }

        #endregion


        #region Other private methods

        /// <summary>
        /// Loads the icons for the application
        /// </summary>
        /// <remarks>
        /// Sometimes, the program path can change and at this time, no icons
        /// are present
        /// </remarks>
        private void LoadIcons()
        {
            System.Text.RegularExpressions.Regex r = new System.Text.RegularExpressions.Regex("(\\\\)");
            string[] s = r.Split(this.path);
            string newPath = "";

            for (int j = 0; j < s.GetLength(0) - 3; j++)
            {
                newPath = newPath + s[j];
            }

            Icon i;
            iconProvider = new Dictionary<string, Icon>();

            //loading the program icon
            i = new Icon(newPath + "FerdaFrontEnd.ico");
            iconProvider.Add("FerdaIcon", i);

            i = new Icon(newPath + "\\Icons\\Save project.ico");
            iconProvider.Add("SaveAndQuitIcon", i);

            i = new Icon(newPath + "\\Icons\\Exit.ico");
            iconProvider.Add("QuitWithoutSaveIcon", i);

            i = new Icon(newPath + "\\Icons\\New project.ico");
            iconProvider.Add("NewIcon", i);

            i = new Icon(newPath + "\\Icons\\Delete from Desktop.ico");
            iconProvider.Add("DeleteIcon", i);

            i = new Icon(newPath + "\\Icons\\Layout.ico");
            iconProvider.Add("JoinIcon", i);

            i = new Icon(newPath + "\\Icons\\Rename Icon.ico");
            iconProvider.Add("RenameIcon", i);

            i = new Icon(newPath + "\\Icons\\USerNote.ico");
            iconProvider.Add("EditIcon", i);
        }

        #endregion


        #region IIconProvider Members

        /// <summary>
        /// Gets the icon specified by icons string identifier
        /// </summary>
        /// <param name="IconName">Name of the icon</param>
        /// <returns>Icon that is connected to this name</returns>
        public Icon GetIcon(string IconName)
        {
            return iconProvider[IconName];
        }

        #endregion
    }
}