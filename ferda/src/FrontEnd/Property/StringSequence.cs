// StringSequence.cs - class for more strings 
//
// Author: Martin Ralbovský <martin.ralbovsky@gmail.com>
//
// Copyright (c) 2005 Martin Ralbovský
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
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Resources;
using System.Collections.Generic;

using Ferda.Modules;
using Ferda.ModulesManager;
using Ferda.FrontEnd;

namespace Ferda.FrontEnd.Properties
{
    /// <summary>
    /// Class to represent sequence of strings. It is used in the
    /// property grid to display a StringT property that allows selecting
    /// a string and editing it
    /// </summary>
    /// <remarks>
    /// I could not find any better solution to the problem of passing the data
    /// to the editor than to save it all into the StringSequence object.
    /// </remarks>
    internal class StringSequence
    {
        #region Class fields

        /// <summary>
        /// Name of the property that is connected with this string
        /// </summary>
        protected string propertyName;

        /// <summary>
        /// Box containing the property
        /// </summary>
        public IBoxModule[] boxes;

        /// <summary>
        /// Resource Manager to write the error message
        /// </summary>
        protected ResourceManager resManager;

        /// <summary>
        /// Properties displayer to refresh the properties after some
        /// changes have been made
        /// </summary>
        protected IPropertiesDisplayer propertiesDisplayer;

        /// <summary>
        /// Label for the converter to be displayed in the grid (different from 
        /// name of the string)
        /// </summary>
        public string selectedLabel;

        /// <summary>
        /// Name of the property that has been selected
        /// </summary>
        public string selectedName;

        //Archive and View displayers to be updated when the class changes a property
        private Archive.IArchiveDisplayer archiveDisplayer;
        private List<Desktop.IViewDisplayer> viewDisplayers;

        #endregion

        #region Properties

        /// <summary>
        /// Resource Manager of the whole application
        /// </summary>
        public ResourceManager ResManager
        {
            get
            {
                return resManager;
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor for the class
        /// </summary>
        /// <param name="arch">archive of the application</param>
        /// <param name="b">The property belongs to this boxes</param>
        /// <param name="name">Name of the property</param>
        /// <param name="res">Default resource manager</param>
        /// <param name="views">Views to be refreshed</param>
        /// <param name="prop">To reset the propertygrid afterwards</param>
        /// <param name="label">Label, that is selected in the sequence</param>
        public StringSequence(string name, IBoxModule [] b, ResourceManager res, 
            Archive.IArchiveDisplayer arch, List<Desktop.IViewDisplayer> views,
            IPropertiesDisplayer prop, string label)
        {
            boxes = b;
            propertyName = name;
            resManager = res;

            this.selectedLabel = label;

            archiveDisplayer = arch;
            viewDisplayers = views;
            propertiesDisplayer = prop;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Sets the selected option to the box
        /// </summary>
        /// <remarks>
        /// I think this is the only way to let the ProjectManager know that 
        /// a selection has been changed. The name of the property should be set
        /// as selectedString, not the label
        /// </remarks>
        public void SetSelectedOption()
        {
            foreach (IBoxModule box in boxes)
            {
                if (box.TryWriteEnter())
                {
                    box.SetPropertyString(propertyName, selectedName);
                    box.WriteExit();
                    archiveDisplayer.RefreshBoxNames();
                    foreach (Desktop.IViewDisplayer view in viewDisplayers)
                    {
                        view.RefreshBoxNames();
                    }
                    //propertiesDisplayer.Adapt();
                }
                else
                {
                    MessageBox.Show(
                        resManager.GetString("PropertiesCannotWriteText"),
                        box.UserName + ": " + resManager.GetString("PropertiesCannotWriteCaption"));
                }
            }
        }

        /*
        /// <summary>
        /// Function returns, if 2 sequences have identical option arrays
        /// </summary>
        /// <param name="sequence1">First sequence to compare</param>
        /// <param name="sequence2">Second sequence to compare</param>
        /// <returns>True if identical, false otherwise</returns>
        */
        //public static bool EqualArrays(StringSequence sequence1, 
        //    StringSequence sequence2)
        //{
        //    if (sequence1.array.Length != sequence2.array.Length)
        //    {
        //        return false;
        //    }

        //    for (int i = 0; i < sequence1.array.Length; i++)
        //    {
        //        if (sequence1.array[i] != sequence2.array[i])
        //        {
        //            return false;
        //        }
        //    }

        //    return true;
        //}

        /// <summary>
        /// Function returns, if 2 sequences have identical selected strings
        /// </summary>
        /// <param name="sequence1">First sequence to compare</param>
        /// <param name="sequence2">Second sequence to compare</param>
        /// <returns>True if identical, false otherwise</returns>
        public static bool EqualSelections(StringSequence sequence1,
            StringSequence sequence2)
        {
            return String.Equals(sequence1.selectedLabel, sequence2.selectedLabel);
        }

        /// <summary>
        /// Gets the array of options for this property
        /// </summary>
        /// <returns>array of options for this property</returns>
        public SelectString[] GetArray()
        {
            SelectString[] array = boxes[0].GetPropertyOptions(propertyName);
            return array;
        }

        #endregion
    }
}
