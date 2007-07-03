// INetworkArchiveDisplayer.cs - Interface to implement the Ferda Network Archive
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
using Ferda.ModulesManager;

namespace Ferda.FrontEnd.NetworkArchive
{
    /// <summary>
    /// Each control implementing Ferda Network Archive displayer should
    /// implement this interface
    /// </summary>
    public interface INetworkArchiveDisplayer
    {
        /// <summary>
        /// Adds Box to a network archive
        /// </summary>
        /// <param name="box">Box module to be added</param>
        /// <param name="label">Label of the box in the network archive</param>
        void AddBox(IBoxModule box, string label);
    }
}
