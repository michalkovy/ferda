// Functions.cs - Function objects for the Command box module
//
// Author: Michal Kováč
//
// Copyright (c) 2007 Michal Kováč <michal.kovac.develop@centrum.cz>
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
using System.IO;
using System.Threading;
using Ice;
using Ferda.Modules.Boxes.Language;

namespace Ferda.Modules.Boxes.Language.Command
{
	/// <summary>
	/// Class is providing ICE functionality of the Command function
	/// </summary>
	public class Functions : Ferda.Modules.Boxes.Language.CommandFunctionsDisp_, IFunctions
	{
		/// <summary>
		/// The box module.
		/// </summary>
		protected BoxModuleI _boxModule;

		//protected IBoxInfo _boxInfo;

		#region IFunctions Members

		/// <summary>
		/// Sets the <see cref="T:Ferda.Modules.BoxModuleI">box module</see>
		/// and the <see cref="T:Ferda.Modules.Boxes.IBoxInfo">box info</see>.
		/// </summary>
		/// <param name="boxModule">The box module.</param>
		/// <param name="boxInfo">The box info.</param>
		public void setBoxModuleInfo(BoxModuleI boxModule, IBoxInfo boxInfo)
		{
	 	   _boxModule = boxModule;
	 	   //_boxInfo = boxInfo;
	    }

        #endregion
        
        #region CommandFunctions Members
        
        
        public override int ExecuteCommand (CommandTextStreamPrx standardOutput, CommandTextStreamPrx errorOutput, Current current__)
        {
        	//only one execution is supported
        	lock(this)
        	{
        		BoxModulePrx[] mainBoxModulePrxs = _boxModule.getConnections("Command",null);
        		BoxModulePrx mainBoxModulePrx = (mainBoxModulePrxs.Length != 0) ? mainBoxModulePrxs[0] : null;
        		ObjectPrx objectPrx = (mainBoxModulePrx != null) ? mainBoxModulePrx.getFunctions() : null;
        		this.inputCommand = (objectPrx != null) ? CommandFunctionsPrxHelper.checkedCast(objectPrx) : null;
	        	System.Diagnostics.Process process = new System.Diagnostics.Process();
	            process.StartInfo.FileName = this._boxModule.GetPropertyString("Executable");
	            process.StartInfo.Arguments = this._boxModule.GetPropertyString("Arguments");
	            process.StartInfo.RedirectStandardOutput = standardOutput != null;
	            process.StartInfo.RedirectStandardError = errorOutput != null;
	            process.StartInfo.RedirectStandardInput = this.inputCommand != null;
	        	this.standardOutput = standardOutput;
	        	this.errorOutput = errorOutput;
	            process.StartInfo.UseShellExecute = false;
	            process.StartInfo.CreateNoWindow = true;
	            process.StartInfo.WorkingDirectory = System.IO.Directory.GetCurrentDirectory();
	            try
	            {
	                process.Start();
	            }
	            catch (System.Exception e)
	            {
	                this._boxModule.Manager.getOutputInterface().writeMsg(Ferda.ModulesManager.MsgType.Error, e.GetType().ToString(), e.Message);
	                return process.ExitCode;
	            }
	            outputThread = new Thread(new ThreadStart(AdminStreamReaderThread_Output));
	            errorThread = new Thread(new ThreadStart(AdminStreamReaderThread_Error));
	            inputThread = new Thread(new ThreadStart(AdminStreamReaderThread_Input));
	            if (this.inputCommand != null)
	            {
	            	_stdIn = process.StandardInput;
	            	inputThread.Start();
	            }
	            if (standardOutput != null)
	            {
	            	_stdOut = process.StandardOutput;
	            	outputThread.Start();
	            }
	            if (errorOutput != null)
	            {
	            	_stdError = process.StandardError;
	            	errorThread.Start();
	            	errorThread.Join();
	            }
	            if (this.inputCommand != null)
	            {
	            	inputThread.Join();
	            }
	            _stdError.Close();
	            if (standardOutput != null)
	            {
	            	outputThread.Join();
	            }
	            process.WaitForExit();
	            return process.ExitCode;
	        }
        }
        
        private void AdminStreamReaderThread_Output() {
            StreamReader reader = _stdOut;
        	
            while (true) {
                string logContents = reader.ReadLine();
                if (logContents == null)
                {
                	this.standardOutput.closeStream();
                    break;
                }
	            this.standardOutput.write(logContents);
            }
        }

        private void AdminStreamReaderThread_Error() {
            StreamReader reader = _stdError;
        	
            while (true) {
                string logContents = reader.ReadLine();
                if (logContents == null)
                {
                    break;
                }
                lock (this.errorOutput)
                {
		        	this.errorOutput.write(logContents);
	            }
            }
        }
        
        private void AdminStreamReaderThread_Input() {
        	CommandTextStreamPrx standardStream = CommandTextStreamPrxHelper.checkedCast( this._boxModule.Adapter.addWithUUID(new CommandTextStreamI(_stdIn, null, true)));
        	CommandTextStreamPrx errorStream = CommandTextStreamPrxHelper.checkedCast(this._boxModule.Adapter.addWithUUID(new CommandTextStreamI(null, errorOutput, false)));
            inputCommand.ExecuteCommand(
	            		standardStream,
	            		errorStream);
        }
        
        private StreamReader _stdError;

        private StreamReader _stdOut;
        
        private StreamWriter _stdIn;

        private Thread outputThread;

        private Thread errorThread;
        
        private Thread inputThread;
        
        private CommandTextStreamPrx standardOutput;
        private CommandTextStreamPrx errorOutput;
        private CommandFunctionsPrx inputCommand;
        
 		#endregion       
    }
}