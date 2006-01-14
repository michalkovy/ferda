using System;
using System.Collections;
using System.Text;
using System.Windows.Forms;
using Ferda;
namespace Ferda
{
    namespace FrontEnd.AddIns.EditCategories.NoGUIclasses
    {
        //class for sorting stuff
        class ListViewItemComparer : IComparer
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

                // If the column is string
                if (column < 4)
                {
                    string lvi1String = lvi1.SubItems[column].ToString();
                    string lvi2String = lvi2.SubItems[column].ToString();

                    // Return the normal Compare
                    if (bAscending)
                        return String.Compare(lvi1String, lvi2String);

                    // Return the negated Compare
                    return -String.Compare(lvi1String, lvi2String);
                }

                // The column is integer
                int lvi1Int = Convert.ToInt32(lvi1.SubItems[column].Text.ToString());
                int lvi2Int = Convert.ToInt32(lvi2.SubItems[column].Text.ToString());

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
        }
    }
}