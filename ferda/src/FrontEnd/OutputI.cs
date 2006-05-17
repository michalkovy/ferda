// OutputI.cs - A class that writes different messages to the user
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
using Ice;
using Ferda.ModulesManager;
using System.Windows.Forms;

namespace Ferda {
    namespace FrontEnd {

        /// <summary>
        /// A class that writes different messages to the user
        /// </summary>
        public class OutputI : OutputDisp_ {

            /// <summary>
            /// Method writeMsg
            /// </summary>
            /// <param name="type">A  Ferda.ModulesManager.MsgType</param>
            /// <param name="name">A  string</param>
            /// <param name="message">A  string</param>
            /// <param name="__current">An Ice.Current</param>
            public override void writeMsg(MsgType type, String name, String message, Current __current) {
				switch(type)
				{
					case MsgType.Error:
						MessageBox.Show(
							message,
							name,
							MessageBoxButtons.OK,
							MessageBoxIcon.Error);
						break;
					case MsgType.Warning:
						MessageBox.Show(
							message,
							name,
							MessageBoxButtons.OK,
							MessageBoxIcon.Warning);
						break;
					case MsgType.Debug:
						MessageBox.Show(
							message,
							name,
							MessageBoxButtons.OK,
							MessageBoxIcon.None);
						break;
					case MsgType.Info:
						MessageBox.Show(
							message,
							name,
							MessageBoxButtons.OK,
							MessageBoxIcon.Information);
						break;
					default:
						MessageBox.Show(
							message,
							name,
							MessageBoxButtons.OK,
							MessageBoxIcon.None);
						break;
				}
            }

            public override ProgressBarPrx startProgress(ProgressTaskPrx task, string name, string hint, Current current__)
            {
                return null;
            }

        }

    }
}
