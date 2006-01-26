using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Odbc;
using System.Data;
using System.IO;
using System.Diagnostics;
using Ferda.Modules.Boxes.LISpMinerTasks.AbstractLMTask;
using Ferda.Modules.Boxes.LISpMinerTasks.AbstractQuantifier;
using Ferda.Modules.Boxes.DataMiningCommon.BooleanPartialCedentSetting;
using Ferda.Modules.Boxes.DataMiningCommon.CategorialPartialCedentSetting;
using Ferda.Modules.Boxes;

namespace Ferda.Modules.MetabaseLayer
{
	public abstract class Task
	{
		protected Common common;
		protected Metabase metabase;
		protected abstract TaskTypeEnum taskType
		{
			get;
		}
		protected abstract string exeFileName
		{
			get;
		}
		protected string exeDirectoryName = "lispMinerGens";

		protected string boxIdentity;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="taskStruct">An object for casting to type of taskStruct according to taskType.</param>
		/// <param name="masterTaskID">Metabase DB ID of task.</param>
		/// <param name="boxIdentity">A string identity of box module.</param>
		protected abstract void saveTask(object taskStruct, int masterTaskID, string boxIdentity);

		private int saveTask(object taskStruct, string boxIdentity)
		{
			int masterTaskID = common.SaveTask(boxIdentity, taskType);
			this.saveTask(taskStruct, masterTaskID, boxIdentity);
			return masterTaskID;
		}

		protected abstract void readResults(int taskID, long allObjectsCount, object taskDescription, out GeneratingStruct generation, out HypothesisStruct[] result);

		public void RunTask(object taskDescription, string boxIdentity, out GeneratingStruct generation, out HypothesisStruct[] result)
		{
			this.boxIdentity = boxIdentity;
			this.Prepare();

			int taskID = saveTask(taskDescription, boxIdentity);
			long allObjectsCount;
			remap(taskDescription, boxIdentity, taskID, out allObjectsCount);

			/*
			 * /DSN "<data-source-name>"	... data source name of metabase
			 * /TaskID <TaskID>				... TaskID of selected task
			 * /Quiet						... errors reported to _AppLog.dat instead on screen
			 * /NoProgress					... no progress dialog is displayed
			 * /TimeOut <sec>				... optional: time-out in seconds (approx.) after result 
			 * 								    (exluding initialisation) is automatically interrupted
			 * /ODBCConnectionString="ODBC;<odbc-data-source-connection-string>"
			 * */
			this.metabase.Connection.Close();

			System.Diagnostics.ProcessStartInfo processStartInfo = new ProcessStartInfo();
			processStartInfo.Arguments =
				" /ODBCConnectionString=\"ODBC;" + this.metabase.Connection.ConnectionString + "\""
				+ " /TaskID " + taskID
				+ " /Quiet ";
			processStartInfo.FileName =
				Path.Combine(
					exeDirectoryName, exeFileName
					);
			processStartInfo.WindowStyle = ProcessWindowStyle.Normal;
			Debug.WriteLine("*Gen process start at: " + DateTime.Now.ToString());
			Process genProcess = Process.Start(processStartInfo);

			do
			{
				if (!genProcess.HasExited)
				{
					if (genProcess.Responding)
						Debug.WriteLine("Status = Running");
					else
						Debug.WriteLine("Status = Not Responding");
				}
			}
			while (!genProcess.WaitForExit(250));

			int genProcessExitCode = genProcess.ExitCode;
			if (genProcessExitCode != 0)
			{
				throw Ferda.Modules.Exceptions.BoxRuntimeError(null, boxIdentity, "Process \"" + processStartInfo.FileName + " " + processStartInfo.Arguments + "\" exited with code " + genProcessExitCode.ToString());
			}
			Debug.WriteLine("*Gen process exit at: " + DateTime.Now.ToString() + ", witch exit code: " + genProcessExitCode.ToString());
			this.metabase.Connection.Open();
			Debug.WriteLine("Reading of result start at: " + DateTime.Now.ToString());
			readResults(taskID, allObjectsCount, taskDescription, out generation, out result);
			Debug.WriteLine("Reading of result finished at: " + DateTime.Now.ToString());
			this.metabase.Connection.Close();
			//this.FinalizeMe();
		}

		protected abstract BooleanCedent[] getBooleanCedents(object taskDescription);
		protected void addBooleanCedents(BooleanPartialCedentSettingStruct[] booleanPartialCedents, CedentEnum cedentType, ref List<BooleanCedent> booleanCedents)
		{
			BooleanCedent booleanCedent;
			if (booleanPartialCedents != null && booleanPartialCedents.Length > 0)
				foreach (BooleanPartialCedentSettingStruct booleanPartialCedentSetting in booleanPartialCedents)
				{
					booleanCedent = new BooleanCedent(cedentType, booleanPartialCedentSetting);
					booleanCedents.Add(booleanCedent);
				}
		}

		protected abstract CategorialCedent[] getCategorialCedents(object taskDescription);
		protected void addCategorialCedent(CategorialPartialCedentSettingStruct[] categorialPartialCedents, CedentEnum cedentType, ref List<CategorialCedent> categorialCedents)
		{
			CategorialCedent categorialCedent;
			if (categorialPartialCedents != null && categorialPartialCedents.Length > 0)
				foreach (CategorialPartialCedentSettingStruct categorialPartialCedentSetting in categorialPartialCedents)
				{
					categorialCedent = new CategorialCedent(cedentType, categorialPartialCedentSetting);
					categorialCedents.Add(categorialCedent);
				}
		}

		private void remap(object taskDescription, string boxIdentity, int taskID, out long allObjectsCount)
		{
			 this.common.Remap(
				 getBooleanCedents(taskDescription), 
				 getCategorialCedents(taskDescription), 
				 taskID, 
				 taskType, 
				 boxIdentity, 
				 out allObjectsCount);
		}

		protected void Prepare()
		{
			metabase = new Metabase();
			common = new Common(metabase.Connection);
		}

		protected void FinalizeMe()
		{
			metabase.FinalizeMe();
		}
	}
}
