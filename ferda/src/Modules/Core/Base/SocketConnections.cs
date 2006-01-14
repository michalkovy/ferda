using System;
using System.Collections.Generic;
using System.Text;

namespace Ferda.Modules.Boxes
{
	/// <summary>
	/// <para>
	/// Provides some static functions for easier 
	/// work with connections in box module`s sockets.
	/// </para>
	/// <para>
	/// There are two kinds of requested objects. At first you can 
	/// require <c>Functions</c> i.e. proxy to implementation
	/// of <c>Funtions`s</c> interface(s). Secondly you can require 
	/// proxy to <see cref="T:Ferda.Modules.BoxModuleI"/>.
	/// </para>
	/// </summary>
	public static class SocketConnections
	{
		/// <summary>
		/// Iff there is exactly one functions object connected in socket 
		/// named <c>socketName</c> than this object is returned. Iff
		/// there is no functions object connected in the socket than
		/// <see cref="T:Ferda.Modules.NoConnectionInSocketError"/> is thrown.
		/// Otherwise (i.e. there is more than one functions object in the socket)
		/// <see cref="T:System.Exception"/> is thrown.
		/// </summary>
		/// <example>
		/// <para>This examples show basic usage of this function.</para>
		/// <para>This is an example of slice design. Apparently MyBoxModule 
		/// has one interface for implementation MyBoxModuleFunctions.</para>
		/// <code>
		/// module MyBoxModule
		/// {
		/// 	interface MyBoxModuleFunctions
		/// 	{
		/// 		 /* ... */
		/// 	};
		/// };
		/// </code>
		/// <para>This is a sample C# class implementing MyBoxModuleFunctions interface.</para>
		/// <code>
		/// namespace MyBoxModule
		/// {
		///		class MyBoxModuleFunctionsI : MyBoxModuleFunctionsDisp_, IFunctions
		///		{
		///			/* ... */
		///		}
		/// }
		/// </code>
		/// <para>Herein "MyBoxModuleFunctions" are accepted by socket named "MySocket".</para>
		/// <code>
		/// namespace MyNextBoxModule
		/// {
		///		class MyNextBoxModuleFunctionsI :  : MyNextBoxModuleFunctionsDisp_, IFunctions
		///		{
		///			protected BoxModuleI boxModule;
		/// 		
		///			//all functions has to implement this interface
		///			#region IFunctions Members
		/// 		public void setBoxModuleInfo(BoxModuleI boxModule, IBoxInfo boxInfo)
		/// 		{
		/// 			this.boxModule = boxModule;
		/// 		}
		/// 		#endregion
		/// 
		///			/* ... */
		/// 
		///			protected MyBoxModuleFunctionsPrx getMyBoxModuleFunctionsPrx()
		///			{
		///				//gets proxy of functions in socket named "MySocket"
		///				Ice.ObjectPrx functionsPrx = 
		///					Ferda.Modules.Boxes.SocketConnections.GetObjectPrx(this.boxModule, "MySocket");
		/// 
		///				//this makes checked casting from Ice.ObjectPrx to MyBoxModuleFunctionsPrx
		///				return MyBoxModuleFunctionsPrxHelper.checkedCast(functionsPrx);
		///			)
		///		}
		/// }
		/// </code>
		/// </example>
		/// <param name="boxModule">Box Module (having socket named <c>socketName</c>).</param>
		/// <param name="socketName">Name of socket.</param>
		/// <returns><see cref="T:Ice.ObjectPrx"/> i.e. proxy of functions connected in <c>socketName</c>.</returns>
		/// <exception cref="T:System.Exception">Thrown iff there 
		/// is connected more than one BoxModule in <c>socketName</c>.
		/// </exception>
		/// <exception cref="T:Ferda.Modules.NoConnectionInSocketError">
		/// Thrown iff there is no BoxModule connected in <c>socketName</c>.
		/// </exception>
		public static Ice.ObjectPrx GetObjectPrx(BoxModuleI boxModule, string socketName)
		{
			Ice.ObjectPrx[] functions = boxModule.GetFunctions(socketName);
			if (functions.Length == 0)
			{
                throw Ferda.Modules.Exceptions.NoConnectionInSocketError(null, boxModule.StringIceIdentity, "BoxInf11: There is no connection in the socket! (" + socketName + ")", new string[] { socketName });
			}
            else if (functions.Length == 1)
			{
				foreach (Ice.ObjectPrx prx in functions)
				{
					return prx;
				}
			}
            string message = "BoxInf12: There should be connected one box at maximum! in socket: \"" + socketName + "\".";
            System.Diagnostics.Debug.WriteLine(message);
			throw new Exception(message);
		}

		/// <summary>
		/// Similar to <see cref="M:Ferda.Modules.Boxes.SocketConnections.GetObjectPrx(Ferda.Modules.BoxModuleI,System.String)"/>
		/// </summary>
		/// <param name="boxModule">Box Module (having socket named <c>socketName</c>).</param>
		/// <param name="socketName">Name of socket.</param>
		/// <param name="objectPrx"><see cref="T:Ice.ObjectPrx"/> i.e. proxy of functions connected in <c>socketName</c>.</param>
		/// <returns>True is returned iff there is just one functions object connected 
		/// in the socket. If true is returned than proxy of the functions object is given
		/// in out parameter <c>objectPrx</c>.</returns>
		/// <exception cref="T:System.Exception">Thrown iff there 
		/// is connected more than one BoxModule in <c>socketName</c>.
		/// </exception>
		public static bool TryGetObjectPrx(BoxModuleI boxModule, string socketName, out Ice.ObjectPrx objectPrx)
		{
            Ice.ObjectPrx[] functions = boxModule.GetFunctions(socketName);
            if (functions.Length == 0)
			{
				objectPrx = null;
				return false;
			}
            else if (functions.Length == 1)
			{
				foreach (Ice.ObjectPrx prx in functions)
				{
					objectPrx = prx;
					return true;
				}
			}
            string message = "BoxInf13: There should be connected one box at maximum! in socket: \"" + socketName + "\".";
            System.Diagnostics.Debug.WriteLine(message);
            throw new Exception(message);
		}

		/// <summary>
		/// Gets functions objects connected in socked named <c>socketName</c>.
		/// Iff <c>oneAtMinimum</c> is true and there is no functions object 
		/// connected in the socket than <see cref="T:Ferda.Modules.NoConnectionInSocketError"/>
		/// is thrown.
		/// </summary>
		/// <example>
		/// Please see an example for <see cref="M:Ferda.Modules.Boxes.SocketConnections.GetObjectPrx(Ferda.Modules.BoxModuleI,System.String)"/>.
		/// </example>
		/// <param name="boxModule">Box Module (having socket named <c>socketName</c>).</param>
		/// <param name="socketName">Name of socket.</param>
		/// <param name="oneAtMinimum">Iff this is true and there is no 
		/// functions object connected in socket than
		/// <see cref="T:Ferda.Modules.NoConnectionInSocketError"/> is thrown.</param>
		/// <returns>Array of <see cref="T:Ice.ObjectPrx">proxies</see> of functions objects 
		/// connected in <c>socketName</c>.</returns>
		/// <exception cref="T:Ferda.Modules.NoConnectionInSocketError">
		/// Thrown iff there is no BoxModule connected in <c>socketName</c> 
		/// and <c>oneAtMinimum</c> is true.
		/// </exception>
		public static Ice.ObjectPrx[] GetObjectPrxs(BoxModuleI boxModule, string socketName, bool oneAtMinimum)
		{
            Ice.ObjectPrx[] functions = boxModule.GetFunctions(socketName);
            if (oneAtMinimum && functions.Length == 0)
			{
				throw Ferda.Modules.Exceptions.NoConnectionInSocketError(null, boxModule.StringIceIdentity, "BoxInf14: There is no connection in the socket! (" + socketName + ")", new string[] { socketName });
			}
            return functions;
		}

		/// <summary>
		/// Iff there is exactly one <see cref="T:Ferda.Modules.BoxModule"/> 
		/// connected in socket named <c>socketName</c> than this object is 
		/// returned. Iff there is no BoxModule connected in the socket than
		/// <see cref="T:Ferda.Modules.NoConnectionInSocketError"/> is thrown.
		/// Otherwise (i.e. there is more than one BoxModule in the socket)
		/// <see cref="T:System.Exception"/> is thrown.
		/// </summary>
		/// <example>
		/// <para>
		/// This example shows some usage of this function. We are in MyBoxModule instance.
		/// There is defined <see cref="T:Ferda.Modules.ModuleAskingForCreation"/> named "MAFC1", 
		/// which gets proxy of BoxModule (Mark it "InputModule") connected into its socket named 
		/// "MyBoxBoduleInputSocket" and make a connection from "InputModule" to socket named 
		/// "Socket1" in newly created BoxModule.
		/// </para>
		/// <code>
		/// namespace MyBoxModule
		/// {
		///		class MyBoxModuleBoxInfo : Ferda.Modules.Boxes.BoxInfo
		///		{
		/// 		public override ModulesAskingForCreation[] GetModulesAskingForCreation(string[] localePrefs, BoxModuleI boxModule)
		/// 		{
		/// 			Dictionary&lt;string, ModuleAskingForCreation&gt; modulesAFC = this.getModulesAskingForCreationNonDynamic(localePrefs);
		/// 			List&lt;ModuleAskingForCreation&gt; result = new List&lt;ModuleAskingForCreation&gt;();
		/// 			ModulesAskingForCreation moduleAFC;
		/// 			ModulesConnection moduleConnection;
		/// 			foreach (string moduleAFCName in modulesAFC.Keys)
		/// 			{
		/// 				moduleAFC = modulesAFC[moduleAFCName];
		/// 				switch (moduleAFCName)
		/// 				{
		/// 					/* ... */
		/// 					case "MAFC1":
		/// 						moduleConnection = new ModulesConnection();
		/// 						moduleConnection.socketName = "Socket1";
		/// 						moduleConnection.boxModuleParam = GetBoxModulePrx(boxModule, "MyBoxBoduleInputSocket");
		/// 						/* ... */
		/// 						break;
		/// 				}
		/// 				moduleAFC.modulesConnection = new ModulesConnection[] { moduleConnection };
		/// 				result.Add(moduleAFC);
		/// 			}
		/// 			return result.ToArray();
		///			}
		///		}
		/// }
		/// </code>
		/// </example>
		/// <param name="boxModule">Box Module (having socket named <c>socketName</c>).</param>
		/// <param name="socketName">Name of socket.</param>
		/// <returns><see cref="T:Ferda.Modules.BoxModulePrx"/> i.e. proxy of BoxModule connected in <c>socketName</c>.</returns>
		/// <exception cref="T:System.Exception">Thrown iff there 
		/// is connected more than one BoxModule in <c>socketName</c>.
		/// </exception>
		/// <exception cref="T:Ferda.Modules.NoConnectionInSocketError">
		/// Thrown iff there is no BoxModule connected in <c>socketName</c>.
		/// </exception>
		public static BoxModulePrx GetBoxModulePrx(BoxModuleI boxModule, string socketName)
		{
            BoxModulePrx[] connections = boxModule.GetConnections(socketName);
            if (connections.Length == 0)
			{
				throw Ferda.Modules.Exceptions.NoConnectionInSocketError(null, boxModule.StringIceIdentity, "BoxInf15: There is no connection in the socket! (" + socketName + ")", new string[] { socketName });
			}
            else if (connections.Length == 1)
			{
				foreach (BoxModulePrx prx in connections)
				{
					return prx;
				}
			}
            string message = "BoxInf16: There should be connected one box at maximum! in socket: \"" + socketName + "\".";
            System.Diagnostics.Debug.WriteLine(message);
            throw new Exception(message);
		}

		/// <summary>
		/// Similar to <see cref="M:Ferda.Modules.Boxes.SocketConnections.GetBoxModulePrx(Ferda.Modules.BoxModuleI,System.String)"/>
		/// </summary>
		/// <param name="boxModule">Box Module (having socket named <c>socketName</c>).</param>
		/// <param name="socketName">Name of socket.</param>
		/// <param name="boxModulePrx"><see cref="T:Ferda.Modules.BoxModulePrx"/> i.e. proxy 
		/// of box module connected in <c>socketName</c>.</param>
		/// <returns>True is returned iff there is just one box module object connected 
		/// in the socket. If true is returned than proxy of the box module is given
		/// in out parameter <c>boxModulePrx</c>.</returns>
		/// <exception cref="T:System.Exception">Thrown iff there 
		/// is connected more than one BoxModule in <c>socketName</c>.
		/// </exception>
		public static bool TryGetBoxModulePrx(BoxModuleI boxModule, string socketName, out BoxModulePrx boxModulePrx)
		{
            BoxModulePrx[] connections = boxModule.GetConnections(socketName);
            if (connections.Length == 0)
			{
				boxModulePrx = null;
				return false;
			}
            else if (connections.Length == 1)
			{
				foreach (BoxModulePrx prx in connections)
				{
					boxModulePrx = prx;
					return true;
				}
			}
            string message = "BoxInf17: There should be connected one box at maximum! in socket: \"" + socketName + "\".";
            System.Diagnostics.Debug.WriteLine(message);
            throw new Exception(message);
		}

		/// <summary>
		/// Gets array of proxies of <see cref="Ferda.Modules.BoxModule">BoxModules</see> 
		/// connected in socked named <c>socketName</c>.
		/// Iff <c>oneAtMinimum</c> is true and there is no BoxModule
		/// connected in socket than <see cref="T:Ferda.Modules.NoConnectionInSocketError"/>
		/// is thrown.
		/// </summary>
		/// <example>
		/// Please see an example for <see cref="M:Ferda.Modules.Boxes.SocketConnections.GetBoxModulePrx(Ferda.Modules.BoxModuleI,System.String)"/>.
		/// </example>
		/// <param name="boxModule">Box Module (having socket named <c>socketName</c>).</param>
		/// <param name="socketName">Name of socket.</param>
		/// <param name="oneAtMinimum">Iff this is true and there is no 
		/// BoxModule connected in socket than
		/// <see cref="T:Ferda.Modules.NoConnectionInSocketError"/> is thrown.</param>
		/// <returns>Array of <see cref="T:Ferda.Modules.BoxModulePrx">proxies</see> of BoxModules 
		/// connected in <c>socketName</c>.</returns>
		/// <exception cref="T:Ferda.Modules.NoConnectionInSocketError">
		/// Thrown iff there is no BoxModule connected in <c>socketName</c> 
		/// and <c>oneAtMinimum</c> is true.
		/// </exception>
		public static BoxModulePrx[] GetBoxModulePrxs(BoxModuleI boxModule, string socketName, bool oneAtMinimum)
		{
            BoxModulePrx[] connections = boxModule.GetConnections(socketName);
            if (oneAtMinimum && connections.Length == 0)
			{
                throw Ferda.Modules.Exceptions.NoConnectionInSocketError(null, boxModule.StringIceIdentity, "BoxInf18: There is no connection in the socket! (" + socketName + ")", new string[] { socketName });
			}
            return connections;
		}
	}
}
