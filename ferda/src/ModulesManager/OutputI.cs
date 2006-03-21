// OutputI.cs - basic implementation of output interface for modules
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
using Ice;

namespace Ferda {
    namespace ModulesManager {

        /// <summary>
        /// Basic implementation of output interface for modules
        /// </summary>
        public class OutputI : OutputDisp_ {

            /// <summary>
            /// Method writeMsg writes message to console
            /// </summary>
            /// <param name="type">A Ferda.ModulesManager.MsgType saying type of message</param>
            /// <param name="name">Name of message</param>
            /// <param name="message">Text of message</param>
            /// <param name="__current">An Ice.Current</param>
            public override void writeMsg(MsgType type, String name, String message, Current __current) {
                string msg = name + ": " + message;
				switch(type)
				{
					case MsgType.Error:
						Console.Error.WriteLine("Error: "+msg);
						break;
					case MsgType.Warning:
						Console.Error.WriteLine("Warning: "+msg);
						break;
					case MsgType.Debug:
						Console.WriteLine("Debug: "+msg);
						break;
					case MsgType.Info:
						Console.WriteLine("Info: "+msg);
						break;
					default:
						Console.WriteLine(msg);
						break;
				}
            }

        }

    }
}
