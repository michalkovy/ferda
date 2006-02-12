// ProjectManagerOptions.cs - Definition of options for Project Manager
//
// Author: Michal Kováč <michal.kovac.develop@centrum.cz>
//
// Copyright (c) 2005 Michal Kováč 
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

namespace Ferda.ProjectManager
{
    /// <summary>
    /// Options of Project Manager, how it has to work
    /// </summary>
    public struct ProjectManagerOptions
    {
        /// <summary>
        /// If Project Manager has to start IceGrid on its init
        /// </summary>
        public bool StartIceGridLocaly;

        /// <summary>
        /// If Project Manager has to stop IceGrid when
        /// <see cref="M:Ferda.ProjectManager.ProjectManager.DestroyProjectManager()"/>
        /// is called
        /// </summary>
        public bool StopIceGridLocaly;

        /// <summary>
        /// If IceGrid is installed as Windows service
        /// </summary>
        public bool IceGridAsService;

        /// <summary>
        /// Sentence on which to wait when IceGrid is starting up on Project Manager init
        /// not as Windows service
        /// </summary>
        public string SentenceForWait;

        /// <summary>
        /// Which IceGrid servers it has to stop when
        /// <see cref="M:Ferda.ProjectManager.ProjectManager.DestroyProjectManager()"/>
        /// is called
        /// </summary>
        public string[] ServersToStop;

        /// <summary>
        /// Path where to find icegridnode.exe executable
        /// </summary>
        public string IceBinPath;

        /// <summary>
        /// An array of locale preferences. <example><code>{"cs-CZ", "en_US"}</code> means
        /// that it will use first czech localization if available,
        /// otherwise american english</example>
        /// </summary>
        public string[] LocalePrefs;
    }
}
