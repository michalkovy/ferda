// IPreferencesManager.cs - Interface for preferences functionality (including
//  localization)
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

namespace Ferda.FrontEnd.Menu
{
    /// <summary>
    /// Interface presents functionality for all the controls to be able
    /// view the preferences of the application. The most important preference
    /// is the localization, but there are others to appear.
    /// </summary>
    public interface IPreferencesManager
    {
        /// <summary>
        /// Method that changes the localication of the whole application
        /// </summary>
        void ChangeGlobalLocalization(string locstring);

        /// <summary>
        /// Current and valid resourceManager that holds the resources
        /// </summary>
        ResourceManager ResManager
        {
            get;
        }

        /// <summary>
        /// Localization string that is selected by the application
        /// </summary>
        string[] LocalePrefs
        {
            get;
            set;
        }

        /// <summary>
        /// User determines whether to show the
        /// <code>Visible sockets</code> group in the property grid.
        /// </summary>
        bool ShowVisibleSockets
        {
            set;
            get;
        }

        /// <summary>
        /// User determines if the progress bars should display a 
        /// dialog showing exact time elapsed for each progress bar
        /// running. This can be useful i.e. when timing hypotheses 
        /// generation.
        /// </summary>
        bool DisplayTiming
        {
            set;
            get;
        }
    }
}
