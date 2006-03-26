// listviewitemcomparer.cs - comparer
//
// Author: Alexander Kuzmin <alexander.kuzmin@gmail.com>
//
// Copyright (c) 2006 Alexander Kuzmin
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
using Ferda;
namespace Ferda.FrontEnd.AddIns.Common.ListView
{
    /// <summary>
    /// Class for comparing listview items
    /// </summary>
    public class ListViewItemComparer : IComparer
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

            //try to convert to intervals first
            double first;
            double second;
            System.Text.RegularExpressions.Regex r = new System.Text.RegularExpressions.Regex("(^[<(]\\d+[;]\\d+[>)]$)");
            System.Text.RegularExpressions.Regex r1 = new System.Text.RegularExpressions.Regex("[<>();]");
            if ((r.IsMatch(lvi1.SubItems[column].Text)) && (r.IsMatch(lvi2.SubItems[column].Text)))
            {
                //hooray, an interval
                string[] numbers = r1.Split(lvi1.SubItems[column].Text);
                string[] numbers1 = r1.Split(lvi2.SubItems[column].Text);
                if ((numbers.Length > 1) && (numbers1.Length > 1) && (Double.TryParse(numbers[1], out first)) && (Double.TryParse(numbers1[1], out second)))
                {
                    if (bAscending)
                        return first > second ? 1 : (first < second ? -1 : 0);

                    // Return the negated Compare
                    return first > second ? -1 : (first < second ? 1 : 0);
                }
                else
                {
                    //some strange string, this should not happen
                    //if nothing works, it is a string
                    string lvi1String = lvi1.SubItems[column].ToString();
                    string lvi2String = lvi2.SubItems[column].ToString();

                    // Return the normal Compare
                    if (bAscending)
                        return String.Compare(lvi1String, lvi2String);

                    // Return the negated Compare
                    return -String.Compare(lvi1String, lvi2String);
                }
            }
            else if ((Double.TryParse(lvi1.SubItems[column].Text, out first)) && (Double.TryParse(lvi2.SubItems[column].Text, out second)))
            {
                //it is double
                if (bAscending)
                    return first > second ? 1 : (first < second ? -1 : 0);

                // Return the negated Compare
                return first > second ? -1 : (first < second ? 1 : 0);                
            }
            else
            {
                //if nothing works, it is a string
                string lvi1String = lvi1.SubItems[column].ToString();
                string lvi2String = lvi2.SubItems[column].ToString();

                // Return the normal Compare
                if (bAscending)
                    return String.Compare(lvi1String, lvi2String);

                // Return the negated Compare
                return -String.Compare(lvi1String, lvi2String);
            }
          //  return 1;
        }
    }
}