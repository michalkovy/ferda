// NetworkArchiveNode.cs - a tree node Ferda Network Archive
//
// Author: Martin Ralbovský <martin.ralbovsky@gmail.com>
//
// Copyright (c) 2007 Martin Ralbovský
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
using System.Windows.Forms;

namespace Ferda.FrontEnd.NetworkArchive
{
    /// <summary>
    /// The tree node structure for the Ferda network archive. It contains
    /// information about the identifier of the box type (to retrieve the
    /// box icon).
    /// </summary>
    public class NetworkArchiveNode : TreeNode
    {
        /// <summary>
        /// Identifier of the box type that is represented by this node
        /// </summary>

        protected string identifier;

        /// <summary>
        /// Identifier of the box type that is represented by this node
        /// </summary>
        public string Identifier
        {
            get { return identifier; }
            set { identifier = value; }
        }
    }
}
