// CommandTextStreamI.cs created with MonoDevelop
// User: michal at 16:23 24.11.2007
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//
// created on 24.11.2007 at 16:23
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
	public class CommandTextStreamI : Ferda.Modules.Boxes.Language.CommandTextStreamDisp_
	{
		public CommandTextStreamI(StreamWriter standardInput, CommandTextStreamPrx errorInput, bool closeStreams)
		{
			this.standardInput = standardInput;
			this.errorInput = errorInput;
			this.closeStreams = closeStreams;
		}
		
		public override void write(string text, Current current__)
		{
			if (standardInput != null)
			{
				lock (standardInput)
				{
					standardInput.Write(text);
				}
			}
			if (errorInput != null)
			{
				lock (errorInput)
				{
					errorInput.write(text);
				}
			}
			resultString = resultString + text;
		}
		
		public override void closeStream (Current current__)
		{
			if (closeStreams)
			{
				if (standardInput != null)
				{
					lock (standardInput)
					{
						standardInput.Close();
					}
				}
				if (errorInput != null)
				{
					lock (errorInput)
					{
						errorInput.closeStream();
					}
				}
			}
		}
		
		public string ResultString
		{
			get
			{
				return resultString;
			}
		}
		
		private StreamWriter standardInput;
		private CommandTextStreamPrx errorInput;
		private bool closeStreams;
		private string resultString = "";
	}
}