// OtherProperty.cs - a special kind of property for the Ferda application
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
using System.Collections.Generic;
using System.Text;
using System.Resources;
using System.Windows.Forms;

using Ferda.ModulesManager;

namespace Ferda.FrontEnd.Properties
{
    /// <summary>
    /// This class is used to set and get other properties.
    /// It contains the property name and the box which is changing the property
    /// and a result for type converting. It is used to display setting modules
    /// in the property grid.
    /// </summary>
    public class OtherProperty
    {
        #region Class fields

        /// <summary>
        /// Box that is the owner of the property
        /// </summary>
        protected IBoxModule box;
        /// <summary>
        /// Name of the property
        /// </summary>
        protected string propertyName;

        /// <summary>
        /// Result of the type conversion from a string
        /// </summary>
        protected string result;

        /// <summary>
        /// Archive and View displayers to be updated when the class changes a property
        /// </summary>
        protected Archive.IArchiveDisplayer archiveDisplayer;
        private List<Desktop.IViewDisplayer> viewDisplayers;

        /// <summary>
        /// Properties displayer to refresh the properties after some
        /// changes have been made
        /// </summary>
        protected IPropertiesDisplayer propertiesDisplayer;

        /// <summary>
        /// Resource Manager to write the error message
        /// </summary>
        protected ResourceManager resManager;

        #endregion

        #region Properties

        /// <summary>
        /// Box that is the owner of the property
        /// </summary>
        public IBoxModule Box
        {
            set
            {
                box = value;
            }
            get
            {
                return box;
            }
        }

        /// <summary>
        /// Name of the property
        /// </summary>
        public string PropertyName
        {
            set
            {
                propertyName = value;
            }
            get
            {
                return propertyName;
            }
        }

        /// <summary>
        /// Result of the type conversion from a string
        /// </summary>
        public string Result
        {
            set
            {
                result = value;
            }
            get
            {
                return result;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor of the class when we want to get a property
        /// </summary>
        /// <param name="box">Box</param>
        /// <param name="res">Default resource manager</param>
        /// <param name="name">name of the property</param>
        /// <param name="views">Views to be refreshed</param>
        /// <param name="prop">To reset the propertygrid afterwards</param>
        /// <param name="arch">archive of the application</param>
        public OtherProperty(IBoxModule box, string name, Archive.IArchiveDisplayer arch,
            List<Desktop.IViewDisplayer> views, IPropertiesDisplayer prop, ResourceManager res)
        {
            this.box = box;
            this.propertyName = name;
            this.result = String.Empty;

            archiveDisplayer = arch;
            viewDisplayers = views;
            propertiesDisplayer = prop;

            resManager = res;
        }

        /// <summary>
        /// Constructor of the class when we want to set the property
        /// </summary>
        /// <param name="result">Result string</param>
        public OtherProperty(string result)
        {
            this.box = null;
            this.propertyName = String.Empty;
            this.result = result;
        }

        #endregion

        #region Methods

        /// <summary>
        /// The method runs other module to set the property
        /// </summary>
        public void SetProperty()
        {
            if (Box.TryWriteEnter())
            {
                Box.RunSetPropertyOther(PropertyName);
                Box.WriteExit();
                archiveDisplayer.RefreshBoxNames();
                foreach (Desktop.IViewDisplayer view in viewDisplayers)
                {
                    view.RefreshBoxNames();
                }
                propertiesDisplayer.Adapt();
            }
            else
            {
                MessageBox.Show(
                    resManager.GetString("PropertiesCannotWriteText"),
                    box.UserName + ": " + resManager.GetString("PropertiesCannotWriteCaption"));
            }
        }

        #endregion
    }
}
