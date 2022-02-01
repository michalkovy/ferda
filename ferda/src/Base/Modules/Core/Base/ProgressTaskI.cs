// ProgressTaskI.cs - abstract class for progress task
//
// Authors: 
//   Tomáš Kuchař <tomas.kuchar@gmail.com>
//
// Copyright (c) 2006 Tomáš Kuchař 
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
using Ice;

namespace Ferda.Modules
{
    /// <summary>
    /// Abstract class for progress tasks (the tasks such as running a
    /// GUHA procedure which progress can be displayed in the progress
    /// bars component in the FrontEnd
    /// </summary>
    public abstract class ProgressTaskI : ProgressTaskDisp_
    {
        /// <summary>
        /// Creates a proxy of the progress task
        /// </summary>
        /// <param name="adapter">The ICE object adapter</param>
        /// <param name="progressTaskI">Interface implementing the progress task</param>
        /// <returns>A new progres task proxy</returns>
        public static ProgressTaskPrx Create(ObjectAdapter adapter, ProgressTaskDisp_ progressTaskI)
        {
            return ProgressTaskPrxHelper.uncheckedCast(adapter.addWithUUID(progressTaskI));
        }

        /// <summary>
        /// Destroys the progress task in the object adapter
        /// </summary>
        /// <param name="adapter">The object adapter</param>
        /// <param name="prx">Proxy of the task</param>
        public static void Destroy(ObjectAdapter adapter, ProgressTaskPrx prx)
        {
            adapter.remove(prx.ice_getIdentity());
        }

        //public override float getValue(out string message, Current current__)
        //{
        //    throw new System.Exception("The method or operation is not implemented.");
        //}

        //public override void stop(Current current__)
        //{
        //    throw new System.Exception("The method or operation is not implemented.");
        //}
    }
}
