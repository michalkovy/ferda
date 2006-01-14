using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using Ferda.FrontEnd.AddIns.EditCategories.NoGUIclasses;
using Ferda.FrontEnd.AddIns.EditCategories.EditExisting;
using System.Resources;
using System.Reflection;
using System.Windows.Forms;


namespace Ferda
{
    namespace FrontEnd.AddIns.EditCategories.EditExisting
    {
        public class EditExistingCategory
        {
            #region Constructor
            public EditExistingCategory(int index, FerdaSmartDataList dataList, MainListView listView, ResourceManager rm)
            {
                if (dataList.GetMultiSet(index).CatType == Ferda.FrontEnd.AddIns.EditCategories.CategoryType.Interval)
                {
                    EditExistingInterval editedInterval = new EditExistingInterval(dataList, dataList.GetMultiSet(index), rm);

                    listView.SuspendLayout();
                    editedInterval.Name = "EditIntervalWizard";
                    editedInterval.TabIndex = 2;

                    editedInterval.Dock = DockStyle.Right;
                    listView.splitContainer1.Panel2Collapsed = false;
                    listView.splitContainer1.Panel2.Controls.Add(editedInterval);
                    listView.MenuItemNew.Enabled = false;
                    listView.ButtonNew.Enabled = false;
                    listView.ButtonEdit.Enabled = false;
                    listView.MenuItemEdit.Enabled = false;
                    listView.DoubleClick -= new EventHandler(listView.EditItem);

                    editedInterval.Disposed += new EventHandler(listView.ListViewReinitSize);

                    listView.ResumeLayout();
                    editedInterval.BringToFront();
                }
                else
                {
                    EditExistingEnumeration editedEnum = new EditExistingEnumeration(dataList, dataList.GetMultiSet(index), rm);

                    listView.SuspendLayout();
                    editedEnum.Name = "EditSetWizard";
                    editedEnum.TabIndex = 2;

                    editedEnum.Dock = DockStyle.Right;
                    listView.splitContainer1.Panel2Collapsed = false;
                    listView.splitContainer1.Panel2.Controls.Add(editedEnum);
                    listView.MenuItemNew.Enabled = false;
                    listView.ButtonNew.Enabled = false;
                    listView.ButtonEdit.Enabled = false;
                    listView.MenuItemEdit.Enabled = false;
                    
                    editedEnum.Disposed += new EventHandler(listView.ListViewReinitSize);

                    listView.ResumeLayout();
                    editedEnum.BringToFront();
                }
            }
            #endregion
        }
    }
}