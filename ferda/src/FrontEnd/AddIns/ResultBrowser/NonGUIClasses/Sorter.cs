using System;
using System.Collections;
using System.Text;
using System.Windows.Forms;

namespace FrontEnd.AddIns.ResultBrowser.NonGUIClasses
{
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

            int lvi1Int = 0;
            int lvi2Int = 0;

            try
            {
                lvi1Int = Convert.ToInt32(lvi1.SubItems[column].Text.ToString());
                lvi2Int = Convert.ToInt32(lvi2.SubItems[column].Text.ToString());

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

            catch
            {
                try
                {
                    string lvi1String = lvi1.SubItems[column].ToString();
                    string lvi2String = lvi2.SubItems[column].ToString();

                    // Return the normal Compare
                    if (bAscending)
                        return String.Compare(lvi1String, lvi2String);

                    // Return the negated Compare
                    return -String.Compare(lvi1String, lvi2String);
                }

                catch
                {
                    return 0;
                }
            }
 
        }

    }
}
