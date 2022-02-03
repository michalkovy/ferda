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
//using Ferda.FrontEnd.AddIns.EditCategories.NoGUIclasses;
using Ferda.FrontEnd.AddIns.EditCategories.CreateNewCategory;
using Ferda;
using Ferda.Modules;
using Ferda.FrontEnd.AddIns;
using Ferda.FrontEnd.AddIns.EditCategories.EditExisting;
using System.Resources;
using System.Reflection;
using Ferda.Guha.Data;
using Ferda.Guha.Attribute;
using System.Security.Cryptography;
using Ferda.FrontEnd.AddIns.Common.ListView;

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
   //     private ValuesAndFrequencies distinctValues;

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

        /// <summary>
        /// Edited attribute
        /// </summary>
        private Attribute<IComparable> attribute;

        /// <summary>
        /// Unchanged serialized attribute
        /// </summary>
        private string oldAttribute;

        /// <summary>
        /// Datatable on which the attribute is applied
        /// </summary>
        private DataTable table;


        /// <summary>
        /// Comparer for the listview items
        /// </summary>
        private ListViewItemComparer comparer = new ListViewItemComparer();

        #endregion


        #region Constructor

        /// <summary>
        /// Contructor which creates editcategories instance and fills in the needed values
        /// </summary>
        /// <param name="localePrefs">Current locale</param>
        /// <param name="categories">Categories to edit</param>
        /// <param name="distinctValues">Distinct values for enum categories</param>
        public MainListView(string[] localePrefs, string categories, DataTable table,
            CardinalityEnum cardinality, DbDataTypeEnum columnDataType, IOwnerOfAddIn ownerOfAddin)
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
                this.resManager = new ResourceManager(
                    "Ferda.FrontEnd.AddIns.EditCategories.Localization_en-US",
            Assembly.GetExecutingAssembly());
                localizationString = "en-US";
            }

            bool intervalsAllowed = false;
            if (cardinality == CardinalityEnum.Cardinal)
            {
                intervalsAllowed = true;
            }

            if (categories == String.Empty)
            {
                switch(columnDataType)
                {
                    case DbDataTypeEnum.BooleanType:
                        this.attribute = new Attribute<IComparable>(
                            DbSimpleDataTypeEnum.BooleanSimpleType, intervalsAllowed);
                        break;

                    case DbDataTypeEnum.DateTimeType:
                        this.attribute = new Attribute<IComparable>(
                            DbSimpleDataTypeEnum.DateTimeSimpleType, intervalsAllowed);
                        break;
            
                    case DbDataTypeEnum.DoubleType:
                        this.attribute = new Attribute<IComparable>(
                            DbSimpleDataTypeEnum.DoubleSimpleType, intervalsAllowed);
                        break;

                    case DbDataTypeEnum.DecimalType:
                    case DbDataTypeEnum.FloatType:
                        this.attribute = new Attribute<IComparable>(
                            DbSimpleDataTypeEnum.FloatSimpleType, intervalsAllowed);
                        break;

                    case DbDataTypeEnum.IntegerType:
                    case DbDataTypeEnum.UnsignedIntegerType:
                        this.attribute = new Attribute<IComparable>(
                            DbSimpleDataTypeEnum.IntegerSimpleType, intervalsAllowed);
                        break;

                    case DbDataTypeEnum.LongIntegerType:
                    case DbDataTypeEnum.UnsignedLongIntegerType:
                        this.attribute = new Attribute<IComparable>(
                            DbSimpleDataTypeEnum.LongSimpleType, intervalsAllowed);
                        break;

                    case DbDataTypeEnum.ShortIntegerType:
                    case DbDataTypeEnum.UnsignedShortIntegerType:
                        this.attribute = new Attribute<IComparable>(
                            DbSimpleDataTypeEnum.ShortSimpleType, intervalsAllowed);
                        break;

                    case DbDataTypeEnum.StringType:
                        this.attribute = new Attribute<IComparable>(
                            DbSimpleDataTypeEnum.StringSimpleType, false);
                        break;

                    case DbDataTypeEnum.TimeType:
                        this.attribute = new Attribute<IComparable>(
                            DbSimpleDataTypeEnum.TimeSimpleType, intervalsAllowed);
                        break;

                    default:
                        MessageBox.Show(this.resManager.GetString("TypeNotSupported"),
                           this.resManager.GetString("Error"),
                   MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        this.Dispose();
                        return;
                
                }
            }
            else
            {

                #region Switch
                switch (columnDataType)
                {
                    case DbDataTypeEnum.BooleanType:
                        attribute =
                            Retyper<IComparable, Boolean>.ToIComparable(
                            Guha.Attribute.Serializer.Deserialize<Boolean>(categories));
                        break;

                    case DbDataTypeEnum.DateTimeType:
                        attribute =
                            Retyper<IComparable, DateTime>.ToIComparable(
                            Guha.Attribute.Serializer.Deserialize<DateTime>(categories));
                        break;

                    case DbDataTypeEnum.DoubleType:
                        attribute =
                            Retyper<IComparable, Double>.ToIComparable(
                            Guha.Attribute.Serializer.Deserialize<Double>(categories));
                        break;

                    case DbDataTypeEnum.FloatType:
                    case DbDataTypeEnum.DecimalType:
                        attribute =
                            Retyper<IComparable, Single>.ToIComparable(
                            Guha.Attribute.Serializer.Deserialize<Single>(categories));
                        break;


                    case DbDataTypeEnum.IntegerType:
                    case DbDataTypeEnum.UnsignedIntegerType:
                        attribute =
                           Retyper<IComparable, Int32>.ToIComparable(
                            Guha.Attribute.Serializer.Deserialize<Int32>(categories));
                        break;

                    case DbDataTypeEnum.ShortIntegerType:
                    case DbDataTypeEnum.UnsignedShortIntegerType:
                        attribute =
                           Retyper<IComparable, Int16>.ToIComparable(
                            Guha.Attribute.Serializer.Deserialize<Int16>(categories));
                        break;

                    case DbDataTypeEnum.LongIntegerType:
                    case DbDataTypeEnum.UnsignedLongIntegerType:
                        attribute =
                           Retyper<IComparable, Int64>.ToIComparable(
                            Guha.Attribute.Serializer.Deserialize<Int64>(categories));
                        break;

                    case DbDataTypeEnum.StringType:
                        attribute =
                           Retyper<IComparable, String>.ToIComparable(
                            Guha.Attribute.Serializer.Deserialize<String>(categories));
                        break;

                    default:
                        throw new ArgumentOutOfRangeException();
                }



                #endregion
            }

            this.table = table;
            this.oldAttribute = categories;
            this.ownerOfAdd = ownerOfAddin;
            this.path = Assembly.GetExecutingAssembly().Location;
            InitializeComponent();
            this.ChangeLocale(this.resManager);
            FillEditCategoriesListView(CategoriesListView);
            //adding a handling method for column sorting
            this.CategoriesListView.ColumnClick += 
                new System.Windows.Forms.ColumnClickEventHandler(this.ColumnListView_ColumnClick);
            this.LoadIcons();
            this.InitIcons();
            this.MenuItemNewInterval.Enabled = intervalsAllowed;
            this.ToolMenuItemNewInterval.Enabled = intervalsAllowed;
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
            this.MenuItemEdit.Click += new EventHandler(EditItemEnum);
            this.MenuItemEditInterval.Click += new EventHandler(EditItemInterval);
            this.ButtonEditEnum.Click += new EventHandler(EditItemEnum);
            this.ButtonEditInterval.Click += new EventHandler(EditItemInterval);
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
            this.ButtonEditEnum.Image = iconProvider["EditIcon"].ToBitmap();
            this.ButtonEditInterval.Image = iconProvider["EditIcon"].ToBitmap();
            this.MenuItemEdit.Image = iconProvider["EditIcon"].ToBitmap();
            this.MenuItemEditInterval.Image = iconProvider["EditIcon"].ToBitmap();
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
            //FerdaSmartDataList dataList = bigDataList;
            // this.splitContainer1.Panel2MinSize = 0;
           // dataList.StructureChange += new FerdaEvent(RefreshList);
            listView.MouseDoubleClick += new MouseEventHandler(EditItemEnum);
            AttributeToListView(this.attribute, CategoriesListView);
        }

        #endregion


        #region Attribute to ListView methods

        /// <summary>
        /// Method to display FerdaSmartDataList using a ListView component
        /// </summary>
        /// <param name="smartList">SmartDataList to display</param>
        /// <param name="form">Form to display the SmartDataList in</param>
        public void AttributeToListView(Attribute<IComparable> _attribute, ListView listView)
        {
            Dictionary<string, int> frequencies = _attribute.GetFrequencies(table);
            foreach (string categoryName in attribute.Keys)
            {
                ListViewItem item = new ListViewItem(categoryName, 1);
                item.SubItems.Add(attribute[categoryName].ToString());
                item.SubItems.Add(frequencies[categoryName].ToString());
                item.Tag = categoryName;
                listView.Items.Add(item);
            }  
        }

        /// <summary>
        /// Method to refresh items in the EditCategoriesListView
        /// </summary>
        private void RefreshList()
        {
            CategoriesListView.Items.Clear();
            AttributeToListView(this.attribute, CategoriesListView);
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
                    this.ButtonDelete.Enabled = false;
                    this.ButtonJoin.Enabled = true;
                    this.ButtonEditEnum.Enabled = false;
                    this.ButtonEditInterval.Enabled = false;
                    this.MenuItemRename.Enabled = false;
                    this.MenuItemDelete.Enabled = false;
                    this.MenuItemEdit.Enabled = false;
                    this.MenuItemEditInterval.Enabled = false;
                }
                else
                {
                    //only one item is selected, check if it can be splitted
                    this.ButtonDelete.Enabled = true;
                    this.ButtonEditEnum.Enabled = true;
                    this.ButtonEditInterval.Enabled = this.attribute.IntervalsAllowed;
                    this.ButtonJoin.Enabled = false;
                    this.MenuItemDelete.Enabled = true;
                    this.MenuItemEdit.Enabled = true;
                    this.MenuItemEditInterval.Enabled = this.attribute.IntervalsAllowed;
                    this.MenuItemJoin.Enabled = false;
                    this.MenuItemRename.Enabled = true;
                }
            }
            else
            {
                //if nothing is selected, only save&quit, nosave&quit, new actions have sense
                this.ButtonDelete.Enabled = false;
                this.ButtonEditEnum.Enabled = false;
                this.ButtonEditInterval.Enabled = false;
                this.ButtonJoin.Enabled = false;
                this.MenuItemRename.Enabled = false;
                this.MenuItemDelete.Enabled = false;
                this.MenuItemEdit.Enabled = false;
                this.MenuItemEditInterval.Enabled = false;
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
            this.DialogResult = DialogResult.OK;
            this.Dispose();
            return;
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
            List<string>categoriesToJoin = new List<string>();

            if (this.CategoriesListView.SelectedItems.Count > 1)
            {
                foreach (int index in this.CategoriesListView.SelectedIndices)
                {
                    categoriesToJoin.Add((string)this.CategoriesListView.Items[index].Tag);
                }
                string tmp = String.Empty;
                attribute.JoinCategories(categoriesToJoin.ToArray(), NewCategoryName.JoinPreviousNames,
                    String.Empty, out tmp);
                RefreshList();
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
        /// Method will handle a request to edit the selected category's enumeration.
        /// </summary>
        internal void EditItemEnum(object sender, EventArgs e)
        {
            if (!EditInProgress)
            {
                EditInProgress = true;
                string index = String.Empty;
                try
                {
                    index = (string)this.CategoriesListView.SelectedItems[0].Tag;
                    this.ButtonDelete.Enabled = false;
                    this.ButtonJoin.Enabled = false;
                    this.ButtonNew.Enabled = false;
                    this.ButtonEditEnum.Enabled = false;
                    this.ButtonEditInterval.Enabled = false;
                    this.MenuItemDelete.Enabled = false;
                    this.MenuItemJoin.Enabled = false;
                    this.MenuItemNew.Enabled = false;
                    this.MenuItemEdit.Enabled = false;
                    this.MenuItemEditInterval.Enabled = false;
                    EditExistingCategory editCategory = new EditExistingCategory(false,
                        index, attribute, this.table, this, this.resManager, new EventHandler(Refr_Click));
                }
                catch
                {
                    EditInProgress = false;
                    return;
                }
            }
        }

        /// <summary>
        /// Method will handle a request to edit the selected category's enumeration.
        /// </summary>
        internal void EditItemInterval(object sender, EventArgs e)
        {
            if (!EditInProgress)
            {
                EditInProgress = true;
                string index = String.Empty;
                try
                {
                    index = (string)this.CategoriesListView.SelectedItems[0].Tag;
                    this.ButtonDelete.Enabled = false;
                    this.ButtonJoin.Enabled = false;
                    this.ButtonNew.Enabled = false;
                    this.ButtonEditEnum.Enabled = false;
                    this.ButtonEditInterval.Enabled = false;
                    this.MenuItemDelete.Enabled = false;
                    this.MenuItemJoin.Enabled = false;
                    this.MenuItemNew.Enabled = false;
                    this.MenuItemEdit.Enabled = false;
                    this.MenuItemEditInterval.Enabled = false;
                    EditExistingCategory editCategory = new EditExistingCategory(true,
                        index, attribute, this.table, this, this.resManager, new EventHandler(Refr_Click));
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
            string index = String.Empty;
            index = (string)this.CategoriesListView.Items[e.Item].Tag;
            e.CancelEdit = true;

            //name has not been changed yet
            if ((index == e.Label) || (e.Label == String.Empty))
            {
                return;
            }

            try
            {   
                this.attribute.RenameCategory(index, e.Label);
                this.RefreshList();
            }
            catch
            {
                MessageBox.Show(this.resManager.GetString("SameCategoryNames"), this.resManager.GetString("Error"),
                        MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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
                this.attribute.Remove((string)item.Tag);
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
            CreateNewCategory.CreateSetWizard newEnum 
                = new CreateSetWizard(attribute, String.Empty, this.table,
                this.resManager, new EventHandler(Refr_Click));
            this.SuspendLayout();
            newEnum.Name = "CreateSetWizard";
            newEnum.TabIndex = 2;
            newEnum.Disposed += new EventHandler(ListViewReinitSize);
            newEnum.Dock = DockStyle.Right;
            this.splitContainer1.Panel2Collapsed = false;
            this.splitContainer1.Panel2.Controls.Add(newEnum);
            this.MenuItemNew.Enabled = false;
            this.ButtonNew.Enabled = false;
            this.ButtonEditEnum.Enabled = false;
            this.MenuItemEdit.Enabled = false;
            this.DoubleClick -= new EventHandler(EditItemEnum);
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
            if (attribute.IntervalsAllowed)
            {
                CreateIntervalWizard newInterval1 = new CreateIntervalWizard(
                    attribute, String.Empty, this.resManager, new EventHandler(Refr_Click));
                this.SuspendLayout();
                newInterval1.Name = "CreateIntervalWizard";
                newInterval1.TabIndex = 2;
                newInterval1.Disposed += new EventHandler(ListViewReinitSize);
                newInterval1.Dock = System.Windows.Forms.DockStyle.Right;
                this.splitContainer1.Panel2Collapsed = false;
                this.splitContainer1.Panel2.Controls.Add(newInterval1);
                this.MenuItemNew.Enabled = false;
                this.ButtonNew.Enabled = false;
                this.ButtonEditEnum.Enabled = false;
                this.MenuItemEdit.Enabled = false;
                this.DoubleClick -= new EventHandler(EditItemEnum);
                this.ResumeLayout();
                newInterval1.BringToFront();
            }
            else
            {
                MessageBox.Show(this.resManager.GetString("IntervalsNotAllowed"), this.resManager.GetString("Error"),
                        MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }


        #endregion


        #region Other handlers and ListView methods

        /// <summary>
        /// Handler for column click - sorts a listview.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ColumnListView_ColumnClick(object sender, System.Windows.Forms.ColumnClickEventArgs e)
        {
            comparer.column = e.Column;
            if (CategoriesListView.Sorting == SortOrder.Ascending)
            {
                comparer.bAscending = false;
                CategoriesListView.Sorting = SortOrder.Descending;
            }
            else
            {
                comparer.bAscending = true;
                CategoriesListView.Sorting = SortOrder.Ascending;
            }
            CategoriesListView.ListViewItemSorter = comparer;
        }

        /*
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
        */
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
        /// Method which returns updated categories
        /// </summary>
        /// <returns></returns>
        public string GetUpdatedCategories()
        {
            return Guha.Attribute.Serializer.Serialize<IComparable>(this.attribute.Export());
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
         //   this.ColumnCategoryType.Text = rm.GetString("ColumnCategoryType");
            this.ColumnCategoryValue.Text = rm.GetString("ColumnCategoryValue");
            this.ColumnFrequency.Text = rm.GetString("ColumnFrequency");
            this.ButtonDelete.Text = rm.GetString("ButtonDelete");
            this.ButtonJoin.Text = rm.GetString("ButtonJoin");
            this.ButtonNew.Text = rm.GetString("ButtonNew");
            this.ButtonEditEnum.Text = rm.GetString("ButtonEdit");
            this.ButtonEditInterval.Text = rm.GetString("ButtonEditInterval");
            this.ButtonQuitWithoutSave.Text = rm.GetString("ButtonQuitWithoutSave");
            this.ButtonSaveAndQuit.Text = rm.GetString("ButtonSaveAndQuit");
            this.ToolMenuItemNewEnumeration.Text = rm.GetString("NewEnumeration");
            this.ToolMenuItemNewInterval.Text = rm.GetString("NewInterval");
            this.ButtonEditEnum.Text = rm.GetString("ButtonEdit");
            this.Text = rm.GetString("EditCategoriesAbout");
            this.MenuItemJoin.Text = rm.GetString("ButtonJoin");
            this.MenuItemNew.Text = rm.GetString("ButtonNew");
            this.MenuItemNewEnum.Text = rm.GetString("NewEnumeration");
            this.MenuItemNewInterval.Text = rm.GetString("NewInterval");
            this.MenuItemQuitWithoutSave.Text = rm.GetString("ButtonQuitWithoutSave");
            this.MenuItemSaveAndQuit.Text = rm.GetString("ButtonSaveAndQuit");
            this.MenuItemDelete.Text = rm.GetString("ButtonDelete");
            this.MenuItemEdit.Text = rm.GetString("ButtonEdit");
            this.MenuItemEditInterval.Text = rm.GetString("ButtonEditInterval");
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

    /// <summary>
    /// Classs for random string generation
    /// </summary>
    public static class RandomString
    {
        /// <summary>
        /// Method to create a random string of the specified length
        /// </summary>
        /// <param name="numBytes">String length</param>
        /// <returns>Created random string</returns>
        public static String CreateKey(int numBytes)
        {
            byte[] buff = RandomNumberGenerator.GetBytes(numBytes);

            return BytesToHexString(buff);
        }

        /// <summary>
        /// Converts bytes to hexstring
        /// </summary>
        /// <param name="bytes">Byte array to convert</param>
        /// <returns>Converted string</returns>
        static String BytesToHexString(byte[] bytes)
        {
            StringBuilder hexString = new StringBuilder(64);

            for (int counter = 0; counter < bytes.Length; counter++)
            {
                hexString.Append(String.Format("{0:X2}", bytes[counter]));
            }
            return hexString.ToString();
        }
    }
}