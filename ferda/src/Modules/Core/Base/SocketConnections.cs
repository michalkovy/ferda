using System;
using System.Collections.Generic;
using System.Diagnostics;
using Ice;
using Exception=System.Exception;

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
        #region Functions Prx(s)
        
        #region Private methods
        
        /// <summary>
        /// Method delegate for checked casting of proxies.
        /// </summary>
        /// <typeparam name="ResultPrx">Result type of the proxy.</typeparam>
        /// <param name="objectProxy">The proxy</param>
        /// <returns></returns>
        public delegate ResultPrx MethodDelegate<ResultPrx>(ObjectPrx objectProxy);

        private static ObjectPrx getObjectPrx(BoxModuleI boxModule, string socketName)
        {
            Debug.Assert(!String.IsNullOrEmpty(socketName));
            Debug.Assert(boxModule != null);

            ObjectPrx[] functions = boxModule.GetFunctions(socketName);

            Debug.Assert(functions.Length == 1);

            if (functions.Length == 1)
                return functions[0];
            else if (functions.Length == 0)
                throw Exceptions.NoConnectionInSocketError(null, boxModule.StringIceIdentity, new string[] { socketName });
            else
                throw Exceptions.BoxRuntimeError(null, boxModule.StringIceIdentity,
                                                 "There should be connected one box at maximum in socket: \"" +
                                                 socketName +
                                                 "\".");
        }

        private static bool tryGetObjectPrx(BoxModuleI boxModule, string socketName, out ObjectPrx objectPrx)
        {
            Debug.Assert(!String.IsNullOrEmpty(socketName));
            Debug.Assert(boxModule != null);

            ObjectPrx[] functions = boxModule.GetFunctions(socketName);

            Debug.Assert(functions.Length == 1);

            if (functions.Length == 1)
            {
                objectPrx = functions[0];
                return true;
            }
            else if (functions.Length == 0)
            {
                objectPrx = null;
                return false;
            }
            else
                throw Exceptions.BoxRuntimeError(null, boxModule.StringIceIdentity,
                                                 "There should be connected one box at maximum in socket: \"" +
                                                 socketName +
                                                 "\".");
        } 
        
        #endregion

        /// <summary>
        /// Iff there is exactly one functions object connected in socket
        /// named <c>socketName</c> than this object is returned. Iff
        /// there is no functions object connected in the socket and fallOnError is set to true than
        /// <see cref="T:Ferda.Modules.NoConnectionInSocketError"/> is thrown.
        /// Otherwise (i.e. there is more than one functions object in the socket)
        /// <see cref="T:Ferda.Modules.BoxRuntimeError"/> is thrown.
        /// </summary>
        /// <param name="boxModule">Box Module (having socket named <c>socketName</c>).</param>
        /// <param name="socketName">Name of socket.</param>
        /// <param name="checkedCast">The checked cast.</param>
        /// <param name="fallOnError">if set to <c>true</c> the method will throw an exception on error; otherwise, returns null.</param>
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
        ///			protected MyBoxModuleFunctionsPrx getMyBoxModuleFunctionsPrx(bool fallOnError)
        ///			{
        ///				//gets proxy of functions in socket named "MySocket"
        ///             return Ferda.Modules.Boxes.SocketConnections.GetPrx&lt;MyBoxModuleFunctionsPrx&gt;(
        ///                     this.boxModule, 
        ///                     "MySocket",
        ///                     MyBoxModuleFunctionsPrxHelper.checkedCast,
        ///                     fallOnError
        ///                 );
        ///			)
        ///		}
        /// }
        /// </code>
        /// </example>
        /// <returns>
        /// 	<see cref="T:Ice.ObjectPrx"/> i.e. proxy of functions connected in <c>socketName</c>.
        /// </returns>
        /// <exception cref="T:Ferda.Modules.BoxRuntimeError">Thrown iff there
        /// is connected more than one BoxModule in <c>socketName</c>.
        /// </exception>
        /// <exception cref="T:Ferda.Modules.NoConnectionInSocketError">
        /// Thrown iff there is no BoxModule connected in <c>socketName</c>.
        /// </exception>
        public static ResultPrx GetPrx<ResultPrx>(BoxModuleI boxModule, string socketName,
                                                  MethodDelegate<ResultPrx> checkedCast, bool fallOnError)
            where ResultPrx : class
        {
            Debug.Assert(!String.IsNullOrEmpty(socketName));
            Debug.Assert(boxModule != null);

            if (fallOnError)
            {
                return checkedCast(
                    getObjectPrx(boxModule, socketName)
                    );
            }
            else
            {
                ObjectPrx prx;
                if (tryGetObjectPrx(boxModule, socketName, out prx))
                    return checkedCast(prx);
                else
                    return null;
            }
        }

        /// <summary>
        /// Gets functions objects connected in socked named <c>socketName</c>.
        /// Iff <c>oneAtMinimum</c> is true and there is no functions object
        /// connected in the socket than <see cref="T:Ferda.Modules.NoConnectionInSocketError"/>
        /// is thrown.
        /// </summary>
        /// <param name="boxModule">Box Module (having socket named <c>socketName</c>).</param>
        /// <param name="socketName">Name of socket.</param>
        /// <param name="checkedCast">The checked cast delegate.</param>
        /// <param name="oneAtMinimum">Iff this is true and there is no
        /// functions object connected in socket than
        /// <see cref="T:Ferda.Modules.NoConnectionInSocketError"/> is thrown (if fallOnError is true).</param>
        /// <param name="fallOnError">if set to <c>true</c> them error falls on error; otherwise, returns null.</param>
        /// <returns>
        /// Array of <see cref="T:Ice.ObjectPrx">proxies</see> of functions objects
        /// connected in <c>socketName</c>.
        /// </returns>
        /// <example>
        /// Please see an example for <see cref="M:Ferda.Modules.Boxes.SocketConnections.GetPrx(Ferda.Modules.BoxModuleI,System.String,MethodDelegate,System.Boolean)"/>.
        /// </example>
        /// <exception cref="T:Ferda.Modules.NoConnectionInSocketError">
        /// Thrown iff there is no BoxModule connected in <c>socketName</c>
        /// and <c>oneAtMinimum</c> and <c>fallOnError</c> is true.
        /// </exception>
        public static List<ResultPrx> GetPrxs<ResultPrx>(BoxModuleI boxModule, string socketName,
                                                  MethodDelegate<ResultPrx> checkedCast, bool oneAtMinimum, bool fallOnError)
            where ResultPrx : class
        {
            Debug.Assert(!String.IsNullOrEmpty(socketName));
            Debug.Assert(boxModule != null);

            ObjectPrx[] functions = boxModule.GetFunctions(socketName);
            if (oneAtMinimum && functions.Length == 0)
            {
                if (fallOnError)
                    throw Exceptions.NoConnectionInSocketError(null, boxModule.StringIceIdentity, new string[] { socketName });
                else
                    return null;
            }
            List<ResultPrx> result = new List<ResultPrx>();
            foreach (ObjectPrx prx in functions)
            {
                result.Add(checkedCast(prx));
            }
            return result;
        } 
        
        #endregion

        #region Box Module Prx(s)
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
        /// 					// ...
        /// 					case "MAFC1":
        /// 						moduleConnection = new ModulesConnection();
        /// 						moduleConnection.socketName = "Socket1";
        /// 						moduleConnection.boxModuleParam = getBoxModulePrx(boxModule, "MyBoxBoduleInputSocket");
        /// 						// ...
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
        private static BoxModulePrx getBoxModulePrx(BoxModuleI boxModule, string socketName)
        {
            Debug.Assert(!String.IsNullOrEmpty(socketName));
            BoxModulePrx[] connections = boxModule.GetConnections(socketName);
            if (connections.Length == 0)
            {
                throw Exceptions.NoConnectionInSocketError(null, boxModule.StringIceIdentity, new string[] { socketName });
            }
            else if (connections.Length == 1)
            {
                foreach (BoxModulePrx prx in connections)
                {
                    return prx;
                }
            }
            string message = "BoxInf16: There should be connected one box at maximum! in socket: \"" + socketName +
                             "\".";
            Debug.WriteLine(message);
            throw new Exception(message);
        }

        /// <summary>
        /// Similar to <see cref="M:Ferda.Modules.Boxes.SocketConnections.getBoxModulePrx(Ferda.Modules.BoxModuleI,System.String)"/>
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
        private static bool tryGetBoxModulePrx(BoxModuleI boxModule, string socketName, out BoxModulePrx boxModulePrx)
        {
            Debug.Assert(!String.IsNullOrEmpty(socketName));
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
            string message = "BoxInf17: There should be connected one box at maximum! in socket: \"" + socketName +
                             "\".";
            Debug.WriteLine(message);
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
        /// Please see an example for <see cref="M:Ferda.Modules.Boxes.SocketConnections.getBoxModulePrx(Ferda.Modules.BoxModuleI,System.String)"/>.
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
        private static BoxModulePrx[] getBoxModulePrxs(BoxModuleI boxModule, string socketName, bool oneAtMinimum)
        {
            Debug.Assert(!String.IsNullOrEmpty(socketName));
            BoxModulePrx[] connections = boxModule.GetConnections(socketName);
            if (oneAtMinimum && connections.Length == 0)
            {
                throw Exceptions.NoConnectionInSocketError(null, boxModule.StringIceIdentity, new string[] { socketName });
            }
            return connections;
        } 
        #endregion

        #region Box Module(s) default user labels

        /// <summary>
        /// Gets the box label connected in the specified <c>socketName</c>.
        /// </summary>
        /// <param name="boxModule">The current box module.</param>
        /// <param name="socketName">Name of the socket.</param>
        /// <returns></returns>
        public static string GetInputBoxLabel(BoxModuleI boxModule, string socketName)
        {
            Debug.Assert(!String.IsNullOrEmpty(socketName));
            Debug.Assert(boxModule != null);

            BoxModulePrx inputBoxModule;
            if (tryGetBoxModulePrx(boxModule, socketName, out inputBoxModule))
            {
                string[] attributeDefaultUserLabel = inputBoxModule.getDefaultUserLabel();
                if (attributeDefaultUserLabel.Length > 0)
                    return attributeDefaultUserLabel[0];
            }
            return null;
        }
        /// <summary>
        /// Gets the labels of boxes connected in the specified <c>socketName</c>.
        /// </summary>
        /// <param name="boxModule">The current box module.</param>
        /// <param name="socketName">Name of the socket.</param>
        /// <returns></returns>
        public static string[] GetInputBoxesLabels(BoxModuleI boxModule, string socketName)
        {
            Debug.Assert(!String.IsNullOrEmpty(socketName));
            Debug.Assert(boxModule != null);

            BoxModulePrx[] prxs = getBoxModulePrxs(boxModule, socketName, false);
            if (prxs == null)
                return null;
            List<string> result = new List<string>();
            foreach (BoxModulePrx prx in prxs)
            {
                string[] attributeDefaultUserLabel = prx.getDefaultUserLabel();
                if (attributeDefaultUserLabel.Length > 0)
                    result.Add(attributeDefaultUserLabel[0]);
            }
            return result.ToArray();
        } 
        #endregion
    }
}