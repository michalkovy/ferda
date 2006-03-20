// Sorter.cs - class for listview items sorting
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
using System.Collections;
using System.Text;
using System.Windows.Forms;

namespace Ferda.FrontEnd.AddIns.ResultBrowser.NonGUIClasses
{
    /// <summary>
    /// Class for sorting listview
    /// </summary>
    public class Sorter : IComparer
    {
        // Initialize the variables to default
        public int column = 0;
        public bool bAscending = true;

        // Using the Compare function of IComparer
        public int Compare(object x, object y)
        {
            // Cast the objects to ListViewItems
            ListViewItem lvi1 = (ListViewItem)x;
            ListViewItem lvi2 = (ListViewItem)y;

            // The column is integer

            double lvi1Int = 0;
            double lvi2Int = 0;

            if (column == -1)
            {
                return 0;
            }

            if (column > 3)
            {
                lvi1Int = Convert.ToDouble(lvi1.SubItems[column].Text.ToString());
                lvi2Int = Convert.ToDouble(lvi2.SubItems[column].Text.ToString());

                // Return the normal compare.. if x < y then return -1
                if (bAscending)
                {
                    if (lvi1Int < lvi2Int)
                        return -1;
                    else if (lvi1Int == lvi2Int)
                        return 0;

                    return 1;
                }

                // Return the opposites for descending
                if (lvi1Int > lvi2Int)
                    return -1;
                else if (lvi1Int == lvi2Int)
                    return 0;

                return 1;
            }
            else
            {
                string lvi1String = lvi1.SubItems[column].ToString();
                string lvi2String = lvi2.SubItems[column].ToString();
                // Return the normal Compare
                if (bAscending)
                    return String.Compare(lvi1String, lvi2String);

                // Return the negated Compare
                return -String.Compare(lvi1String, lvi2String);
            }
        }
    }
}
