using System;
using Ferda.Modules;

namespace Ferda {
    namespace ModulesManager {
		public interface IBoxModuleFactoryCreator {
			/// <summary>
			/// Gets ice identifiers of functions interface of boxes created by this creator.
			/// </summary>
			/// <remarks>
			/// <para>
			/// These ice identifiers are used in
			/// <see cref="T:Ferda.Modules.BoxType">BoxType</see>.
			/// </para>
			/// <para>
			/// Use
			/// <see cref="M:Ferda.ModulesManager.IBoxModule.GetFunctionsIceIds()"/>
			/// for conrete box instead of this function whenever possible.
			/// </para></remarks>
			/// <returns>A System.Collections.Specialized.StringCollection of ice identifiers of functions interface</returns>
			System.Collections.Specialized.StringCollection
				GetBoxModuleFunctionsIceIds();

			/// <summary>
			/// Returns bool indicating whether functions interface has the specified ice identifier
			/// </summary>
			/// <remarks>
			/// Use
			/// <see cref="M:Ferda.ModulesManager.IBoxModule.IsWithIceId(System.String)"/>
			/// for conrete box instead of this function whenever possible.
			/// </remarks>
			/// <returns>True if functions interface of boxes created by this creator is of type <paramref name="iceId"/>, otherwise false</returns>
			/// <param name="iceId">A string representation of ice identifier of functions interface</param>
            bool IsWithIceId(string iceId);
	
			/// <summary>
			/// Returns a Boolean value indicating whether boxes created by this creator
			/// has type <paramref name="boxType"/>
			/// </summary>
			/// <remarks>
			/// <para>
			/// <see cref="T:Ferda.Modules.BoxType"/> is used in
			/// <see cref="P:Ferda.ModulesManager.IBoxModuleFactoryCreator.Sockets"/>
			/// for indicating whether some box can be connected into thet socket.
			/// </para>
			/// <para>
			/// Use <see cref="M:Ferda.ModulesManager.IBoxModule.HasBoxType(Ferda.Modules.BoxType)"/>
			/// for conrete box instead of this function whenever possible.
			/// </para>
			/// </remarks>
			/// <returns>true if boxes created by this creator are
			/// of type <paramref name="boxType"/></returns>
			/// <param name="boxType">A <see cref="T:Ferda.Modules.BoxType"/> type of
			/// box</param>
			bool HasBoxType(BoxType boxType);

			/// <summary>
			/// Creates new box and returns in.
			/// </summary>
			/// <returns>A <see cref="T:Ferda.ModulesManager.IBoxModule"/>
			/// representing box</returns>
			Ferda.ModulesManager.IBoxModule CreateBoxModule();

			/// <summary>
			/// Gets path of help file with identifier <paramref name="identifier"/>
			/// </summary>
			/// <returns>A string representing path</returns>
			/// <param name="identifier">A string representing identifier
			/// of help</param>
			string GetHelpFilePath(string identifier);

			/// <summary>
			/// Actions of boxes created by this creator
			/// </summary>
			/// <value>
			/// An array of <see cref="T:Ferda.Modules.ActionInfo"/>
			/// which represents information about actions of boxes
			/// </value>
			ActionInfo[] Actions
			{
                get;
            }
			
			/// <summary>
			/// Icon of boxes created by this creator
			/// </summary>
			/// <value>
			/// Byte[] representation of .ico file
			/// </value>
			byte[] Icon
			{
                get;
            }
			
			/// <summary>
			/// Information about sockets in this boxes created by this creator.
			/// </summary>
			/// <value>An array of <see cref="T:Ferda.Modules.SocketInfo"/>
			/// where every <see cref="T:Ferda.Modules.SocketInfo"/> is for
			/// one socket</value>
			/// <remarks>
			/// Use <see cref="P:Ferda.ModulesManager.IBoxModule.Sockets"/>
			/// for conrete box instead of this function whenever possible.
			/// </remarks>
			Ferda.Modules.SocketInfo[] Sockets
			{
                get;
            }
			
			/// <summary>
			/// Gets information about socket in this boxes created by this creator.
			/// </summary>
			/// <value>A <see cref="T:Ferda.Modules.SocketInfo"/>
			/// representing information about socket</value>
			Ferda.Modules.SocketInfo GetSocket(string socketName);
			
			/// <summary>
			/// Design of box
			/// </summary>
			/// <value>String representation of .svg file</value>
            string Design
			{
                get;
            }
			
			/// <summary>
			/// Categories in which are boxes created by this creator.
			/// </summary>
			/// <value>An array of string representing categories</value>
			string[] BoxCategories
			{
				get;
            }
			
			/// <summary>
			/// Informations about help files for boxes created by this creator
			/// </summary>
			/// <value>
			/// A dictionary of <see cref="T:Ferda.Modules.HelpFileInfo"/>
			/// </value>
			System.Collections.Generic.Dictionary<string,HelpFileInfo> HelpFileInfoSeq
			{
                get;
            }
			/*
			/// <summary>
			/// Property name which drives
			/// <see cref="T:Ferda.ModulesManager.IBoxModuleFactoryCreator.Label"/>
			/// of boxes
			/// </summary>
			/// <value>
			/// Empty array of strings if there is no property which drives
			/// <see cref="T:Ferda.ModulesManager.IBoxModuleFactoryCreator.Label"/>
			/// or array with one string representing name of property which drives
			/// that
			/// <see cref="T:Ferda.ModulesManager.IBoxModuleFactoryCreator.Label"/>
			/// </value>
			String[] PropertyDrivingLabel
			{
                get;
            }
			 */
			
			/// <summary>
			/// Identifier of creator
			/// </summary>
			/// <value>
			/// string representing unique identifier of creator
			/// </value>
            String Identifier
			{
                get;
            }
			
			/// <summary>
			/// Information about properties in this boxes created by this creator.
			/// </summary>
			/// <value>An array of <see cref="T:Ferda.Modules.PropertyInfo"/>
			/// where every <see cref="T:Ferda.Modules.PropertyInfo"/> is for
			/// one property</value>
			Ferda.Modules.PropertyInfo[] Properties
			{
                get;
            }
			
			/// <summary>
			/// Gets property with specified name
			/// </summary>
			/// <returns>A <see cref="T:Ferda.Modules.PropertyInfo"/>
			/// with name <paramref name="name"/></returns>
			/// <param name="name">A string representing name
			/// of property</param>
			Ferda.Modules.PropertyInfo GetProperty(string name);
			
			/// <summary>
			/// Label of boxes created by this creator
			/// </summary>
			/// <value>
			/// string representing localized label of boxes
			/// </value>
			String Label
			{
                get;
            }
			
			/// <summary>
			/// Hint of boxes created by this creator
			/// </summary>
			/// <value>
			/// string representing localized hint about boxes
			/// created by this creator
			/// </value>
            String Hint
			{
                get;
            }
		}
	}
}
