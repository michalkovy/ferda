// Functions.cs - Function objects for the CommandOutput box module
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
using Ice;
using System.IO;
using System.Threading;
using Ferda.Modules.Boxes.Language;
using Ferda.Modules.Boxes.Language.Command;

namespace Ferda.Modules.Boxes.Language.CommandOutput
{
	/// <summary>
	/// Class is providing ICE functionality of the CommandOutput function
	/// </summary>
	public class Functions : Ferda.Modules.StringTI, IFunctions
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
		
		#region StringTInterfacePrx Members

		public override String getStringValue(Current __current)
		{
			BoxModulePrx[] mainBoxModulePrxs = _boxModule.getConnections("Command",null);
	   		BoxModulePrx mainBoxModulePrx = (mainBoxModulePrxs.Length != 0) ? mainBoxModulePrxs[0] : null;
	   		ObjectPrx objectPrx = (mainBoxModulePrx != null) ? mainBoxModulePrx.getFunctions() : null;
	   		CommandFunctionsPrx inputCommand = (objectPrx != null) ? CommandFunctionsPrxHelper.checkedCast(objectPrx) : null;
	   		
	   		if (inputCommand == null) return "";
	   		
	   		string standardOutputFile = this._boxModule.GetPropertyString("StandardOutputFile");
	   		string errorOutputFile = this._boxModule.GetPropertyString("ErrorOutputFile");
	   		bool standardOutputAppendExistingFile = this._boxModule.GetPropertyBool("StandardOutputAppendExistingFile");
	   		bool errorOutputAppendExistingFile = this._boxModule.GetPropertyBool("ErrorOutputAppendExistingFile");
	   		bool resultStandardOutput = this._boxModule.GetPropertyString("ResultValue") == "StandardOutput";
	   		bool showErrors = this._boxModule.GetPropertyBool("ShowErrors");
	   		
	   		StreamWriter writer = null;
	   		if (!String.IsNullOrEmpty(standardOutputFile))
	   		{
	   			writer = new StreamWriter(standardOutputFile, standardOutputAppendExistingFile);	
	   		}
	   		CommandTextStreamI standardOutputTextStream = new CommandTextStreamI(writer, null, true);
	   		CommandTextStreamPrx standardOutputTextStreamPrx = CommandTextStreamPrxHelper.checkedCast( this._boxModule.Adapter.addWithUUID(standardOutputTextStream));
	   		
	   		writer = null;
	   		if (!String.IsNullOrEmpty(errorOutputFile))
	   		{
	   			writer = new StreamWriter(errorOutputFile, errorOutputAppendExistingFile);	
	   		}
	   		CommandTextStreamI errorOutputTextStream = new CommandTextStreamI(writer, null, true);
	   		CommandTextStreamPrx errorOutputTextStreamPrx = CommandTextStreamPrxHelper.checkedCast( this._boxModule.Adapter.addWithUUID(errorOutputTextStream));
	   		
			inputCommand.ExecuteCommand(
						standardOutputTextStreamPrx,
						errorOutputTextStreamPrx);
			
			if (showErrors && !String.IsNullOrEmpty(errorOutputTextStream.ResultString))
			{
				this._boxModule.Manager.getOutputInterface().writeMsg(Ferda.ModulesManager.MsgType.Error, "Errors", errorOutputTextStream.ResultString);
			}
			
			if (resultStandardOutput)
			{
				return standardOutputTextStream.ResultString;
			}
			else
			{
				return errorOutputTextStream.ResultString;
			}
		}
		
 		#endregion	   
	}
}