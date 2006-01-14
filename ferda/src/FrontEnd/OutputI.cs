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

        }

    }
}
