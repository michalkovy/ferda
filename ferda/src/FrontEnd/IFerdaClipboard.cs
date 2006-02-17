// IFerdaClipboard.cs - clipboard interface
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

namespace Ferda.FrontEnd
{
    /// <summary>
    /// My implementation of clipboart, it remembers list of nodes. It does not
    /// cooperate with the normal system clipboard.
    /// </summary>
    public interface IFerdaClipboard
    {
        /// <summary>
        /// Nodes contained in the clipboard
        /// </summary>
        List<ModulesManager.IBoxModule> Nodes
        {
            set;
            get;
        }

        /// <summary>
        /// Determines if there is something in the clipboard
        /// </summary>
        bool IsEmpty
        {
            get;
        }
    }
}
