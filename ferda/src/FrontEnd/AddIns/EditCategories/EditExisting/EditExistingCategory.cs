// EditExistingCategory.cs - class for initializing editing category
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
using System.Text;
using System.Drawing;
using Ferda.FrontEnd.AddIns.EditCategories.NoGUIclasses;
using Ferda.FrontEnd.AddIns.EditCategories.EditExisting;
using System.Resources;
using System.Reflection;
using System.Windows.Forms;

namespace Ferda.FrontEnd.AddIns.EditCategories.EditExisting
{
    /// <summary>
    /// Class for initializing editing categories
    /// </summary>
    public class EditExistingCategory
    {
        #region Constructor

        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="index">Index of category to edit</param>
        /// <param name="dataList">Datalist to work with</param>
        /// <param name="listView">Listview to display categories</param>
        /// <param name="rm">Resource manager</param>
        public EditExistingCategory(int index, FerdaSmartDataList dataList, MainListView listView, ResourceManager rm)
        {
            if (dataList.GetCategory(index).CatType == Ferda.FrontEnd.AddIns.EditCategories.CategoryType.Interval)
            {
                EditExistingInterval editedInterval = new EditExistingInterval(dataList, dataList.GetCategory(index), rm);
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
                EditExistingEnumeration editedEnum = new EditExistingEnumeration(dataList, dataList.GetCategory(index), rm);
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