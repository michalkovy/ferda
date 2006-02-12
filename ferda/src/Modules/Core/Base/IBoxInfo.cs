using System;
namespace Ferda.Modules.Boxes
{
	/// <summary>
	/// <para>
	/// This interface provides fundamental funcionality and lot of 
	/// basic features (including localizaton) of the box module. This 
    /// interface is required by <see cref="T:Ferda.Modules.BoxModuleI"/>,
	/// <see cref="T:Ferda.Modules.BoxModuleFactoryI"/> and 
	/// <see cref="T:Ferda.Modules.BoxModuleFactoryCreatorI"/> which
    /// are used by <b>ModulesManager</b> and <b>ProjectManager</b>.
    /// Therefore if you wants to implement your own box module
    /// you only needs to implement this interface and your own functions
    /// specified by your slice design of the box module`s functions.
	/// </para>
	/// </summary>
    /// <remarks>
    /// <para>
    /// Lot of methods has parameter <c>localePrefs</c> 
    /// (Localization preferences). Value of <c>localePrefs</c> is array
    /// of <see cref="T:System.String">Strings</see> defining 
    /// <see cref="N:System.Globalization">culture names</see>.
    /// </para>
    /// <para>
    /// The culture names follow the RFC 1766 standard in the format 
    /// "&lt;languagecode2&gt;-&lt;country/regioncode2&gt;", where &lt;languagecode2&gt;
    /// is a lowercase two-letter code derived from ISO 639-1 and &lt;country/regioncode2&gt;
    /// is an uppercase two-letter code derived from ISO 3166. For example, U.S. English 
    /// is "en-US". In cases where a two-letter language code is not available, 
    /// the three-letter code derived from ISO 639-2 is used; for example, 
    /// the three-letter code "div" is used for cultures that use the Dhivehi language. 
    /// Some culture names have suffixes that specify the script; for example, 
    /// "-Cyrl" specifies the Cyrillic script, "-Latn" specifies the Latin script.
    /// </para>
    /// <para>
    /// The most culture name from <c>localePrefs</c> is used. If no one can 
    /// be used than default culture name (basicly "en-US") is used.
    /// </para>
    /// </remarks>
	/// <seealso cref="T:Ferda.Modules.Boxes.BoxInfo"/>
	/// <seealso cref="T:Ferda.Modules.BoxModuleI"/>
	/// <seealso cref="T:Ferda.Modules.BoxModuleFactoryI"/>
	/// <seealso cref="T:Ferda.Modules.BoxModuleFactoryCreatorI"/>
	public interface IBoxInfo
	{
		/// <summary>
		/// Array of <see cref="T:System.String">Strings</see> 
		/// as list of names of categories, in which this 
		/// box module belongs to.
		/// </summary>
        /// <value>
        /// Names of categories, in which the box module belongs to.
        /// </value>
		/// <remarks>
        /// <para>
        /// Box module can be in any number of categories.
        /// </para>
        /// <para>
        /// These names are not localized  i.e. the name of 
        /// the category is an identifier of the category.
        /// </para>
        /// <para>
        /// For localization of this identifiers use 
        /// <see cref="M:Ferda.Modules.Boxes.IBoxInfo.GetBoxCategoryLocalizedName(System.String,System.String)"/>.
        /// </para>
        /// </remarks>
        /// <seealso cref="M:Ferda.Modules.Boxes.IBoxInfo.GetBoxCategoryLocalizedName(System.String,System.String)"/>
		string[] Categories { get; }

		/// <summary>
		/// Creates <see cref="T:Ice.Object"/> implementing Ice interface of 
		/// the box module i.e. box`s functions declared in slice design.
		/// </summary>
		/// <example>
		/// <para>Following examples shows how to implement this function.</para>
        /// <para>This is an example of slice design. As you can see <c>MyBoxModule</c> 
        /// has one interface for implementation named <c>MyBoxModuleFunctions</c>.</para>
		/// <code>
		/// module MyBoxModule
		/// {
		/// 	interface MyBoxModuleFunctions
		/// 	{
        ///         nonmutating string HelloWorld();
		/// 		/* ... */
		/// 	};
		/// };
		/// </code>
        /// <para>This is a sample C# class implementing <c>MyBoxModuleFunctions</c> interface.
        /// Please notice that class implementing the slice design of any box module has 
        /// to implement <see cref="T:Ferda.Modules.IFunctions"/>.</para>
		/// <code>
		/// namespace MyBoxModule
		/// {
		///		class MyBoxModuleFunctionsI : MyBoxModuleFunctionsDisp_, IFunctions
		///		{
        ///		    protected Ferda.Modules.BoxModuleI boxModule;
        ///		    protected Ferda.Modules.Boxes.IBoxInfo boxInfo;
        /// 
        ///		    #region IFunctions Members
        ///		    void Ferda.Modules.IFunctions.setBoxModuleInfo(Ferda.Modules.BoxModuleI boxModule, Ferda.Modules.Boxes.IBoxInfo boxInfo)
        ///		    {
        ///	    	    this.boxModule = boxModule;
        ///	    	    this.boxInfo = boxInfo;
        ///	    	}
        ///		    #endregion
        /// 
        /// 		//this implements HelloWorld() method specified in slice design
        ///         public override string HelloWorld(Ice.Current __current)
		/// 		{
		/// 			return "Hello World!";
        /// 		}
        /// 
		///			/* ... */
		///		}
		/// }
		/// </code>
		/// <para>Finally, this is sample implementation of this function.</para>
		/// <code>
		/// namespace MyBoxModule
		/// {
		///		class MyBoxModuleBoxInfo : Ferda.Modules.Boxes.BoxInfo
		///		{
		///			/* ... */
        /// 
		///			public override void CreateFunctions(BoxModuleI boxModule, out Ice.Object iceObject, out IFunctions functions)
		///			{
		///				MyBoxModuleFunctionsI result = new MyBoxModuleFunctionsI();
		///				iceObject = (Ice.Object)result;
		/// 			functions = (IFunctions)result;
		///			}
        /// 
        ///         /* ... */
		///		}
		/// }
		/// </code>
		/// </example>
		/// <param name="boxModule">Box module, to which created functions 
		/// will belong to.</param>
        /// <param name="iceObject">An out parameter returning <see cref="T:Ice.Object"/> 
		/// implementing box`s "ice" functions. This value is same as value 
		/// of <c>functions</c>.</param>
        /// <param name="functions">An out parameter returning <see cref="T:Ice.Object"/> 
		/// implementing box`s "ice" functions. This value is same as value 
		/// of <c>iceObject</c>.</param>
        /// <remarks>
        /// Each instance of the box module has its own functions object but 
        /// class implementing <see cref="T:Ferda.Modules.Boxes.IBoxInfo">
        /// this interface</see> is shared by all instances of the box modules
        /// of the same type <see cref="P:Ferda.Modules.Boxes.IBoxInfo.Identifier"/>
        /// </remarks>
		void CreateFunctions(BoxModuleI boxModule, out Ice.Object iceObject, out IFunctions functions);

        /// <summary>
        /// Gets the <see href="http://www.w3.org/tr/2000/cr-svg-20001102/index.html">
        /// Scalable Vector Graphics (SVG)</see> design.
        /// </summary>
        /// <value>The string representation of SVG design file.</value>
        string Design { get; }

        /// <summary>
        /// Gets needed connected sockets of the action specified by <c>actionName</c>.
        /// </summary>
        /// <param name="actionName">Name of the action.</param>
        /// <returns>Array of conditions on needed conected sockets.</returns>
        /// <remarks>Returned value is array of conditions on connections in
        /// sockets. At least one of returned conditions has to be realized before
        /// execution of the action is allowed.</remarks>
        /// <exception cref="T:Ferda.Modules.NameNotExistsError">Iff <c>actionName</c> is bad.</exception>
		string[][] GetActionInfoNeededConnectedSockets(string actionName);

        /// <summary>
        /// Gets actions of the box module.
        /// </summary>
        /// <param name="localePrefs">The localization preferences.</param>
        /// <returns>Array of <see cref="T:Ferda.Modules.ActionInfo"/>.</returns>
		ActionInfo[] GetActions(string[] localePrefs);

        /// <summary>
        /// Gets the localized name of the box`s category.
        /// </summary>
        /// <param name="cultureName">Name of the culture i.e. (one) localization prefrence.</param>
        /// <param name="categoryName">Name of the category.</param>
        /// <returns>
        /// Localized name of the category named <c>categoryName</c>.
        /// </returns>
		string GetBoxCategoryLocalizedName(string cultureName, string categoryName);

        /// <summary>
        /// Gets function`s Ice identifiers of the box module.
        /// </summary>
        /// <example>
        /// <para>Following examples shows how to implement this function.</para>
        /// <para>This is an example of slice design. As you can see <c>MyBoxModule</c> 
        /// has one interface for implementation named <c>MyBoxModuleFunctions</c>.</para>
        /// <code>
        /// module MyBoxModule
        /// {
        /// 	interface MyBoxModuleFunctions
        /// 	{
        /// 		/* ... */
        /// 	};
        /// };
        /// </code>
        /// <para>This is a sample C# class implementing <c>MyBoxModuleFunctions</c> interface.
        /// Please notice that class implementing the slice design of any box module has 
        /// to implement <see cref="T:Ferda.Modules.IFunctions"/>.</para>
        /// <code>
        /// namespace MyBoxModule
        /// {
        ///		class MyBoxModuleFunctionsI : MyBoxModuleFunctionsDisp_, IFunctions
        ///		{
        ///			/* ... */
        ///		}
        /// }
        /// </code>
        /// <para>Finally, this is sample implementation of this function.</para>
        /// <code>
        /// namespace MyBoxModule
        /// {
        ///		class MyBoxModuleBoxInfo : Ferda.Modules.Boxes.BoxInfo
        ///		{
        ///			/* ... */
        ///			public override string[] GetBoxModuleFunctionsIceIds()
        ///			{
        ///				return MyBoxModule.MyBoxModuleFunctionsI.ids__;
        ///			}
        ///		}
        /// }
        /// </code>
        /// </example>
        /// <returns>
        /// An array of strings representing Ice identifiers 
        /// of the box module`s functions.
        /// </returns>
		string[] GetBoxModuleFunctionsIceIds();

        /// <summary>
        /// Gets items of the dynamic help.
        /// </summary>
        /// <param name="localePrefs">The localization preferences.</param>
        /// <param name="boxModule">The box module.</param>
        /// <returns>
        /// Array of <seealso cref="T:Ferda.Modules.DynamicHelpItem">DynamicHelpItems</seealso>.
        /// </returns>
		DynamicHelpItem[] GetDynamicHelpItems(string[] localePrefs, BoxModuleI boxModule);

        /// <summary>
        /// Gets help file as aray of <see cref="T:System.Byte">Bytes</see>.
        /// </summary>
        /// <param name="identifier">The identifier of the help file.</param>
        /// <returns>Content of the help file as array of <see cref="T:System.Byte">Bytes</see>.</returns>
		byte[] GetHelpFile(string identifier);

        /// <summary>
        /// Gets information about the help files.
        /// </summary>
        /// <param name="localePrefs">The localization preferences.</param>
        /// <returns>
        /// Array of <seealso cref="T:Ferda.Modules.HelpFileInfo">HelpFileInfos</seealso>.
        /// </returns>
        HelpFileInfo[] GetHelpFileInfoSeq(string[] localePrefs);

        /// <summary>
        /// Gets localized hint (short suggestion) of the box module.
        /// </summary>
        /// <param name="localePrefs">The localization preferences.</param>
        /// <returns>Localized hint (short suggestion) of the box module.</returns>
		string GetHint(string[] localePrefs);

        /// <summary>
        /// Gets localized label of the box module.
        /// </summary>
        /// <param name="localePrefs">The localization preferences.</param>
        /// <returns>Localized label of the box module.</returns>
		string GetLabel(string[] localePrefs);

        /// <summary>
        /// Gets the box modules asking for creation.
        /// </summary>
        /// <param name="localePrefs">The localization preferences.</param>
        /// <param name="boxModule">The box module.</param>
        /// <returns>
        /// Array of <see cref="T:Ferda.Modules.ModuleAskingForCreation">
        /// Modules Asking For Creation</see>.
        /// </returns>
        /// <remarks>
        /// Modules asking for creation dynamically depends on actual
        /// inner state of the box module.
        /// </remarks>
		ModulesAskingForCreation[] GetModulesAskingForCreation(string[] localePrefs, BoxModuleI boxModule);

        /// <summary>
        /// Gets the properties of the box module.
        /// </summary>
        /// <param name="localePrefs">The localization preferences.</param>
        /// <returns>
        /// Array of <seealso cref="T:Ferda.Modules.PropertyInfo">PropertyInfos</seealso>.
        /// </returns>
		PropertyInfo[] GetProperties(string[] localePrefs);

        /// <summary>
        /// Gets names of the properties.
        /// </summary>
        /// <returns>
        /// Array of <seealso cref="T:System.String">Strings</seealso> as 
        /// names of the properties in the box module.
        /// </returns>
        /// <remarks>
        /// The name of the property is its unique identifier.
        /// </remarks>
		string[] GetPropertiesNames();

        /// <summary>
        /// Gets default value of the property.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns>
        /// Returns default <see cref="T:Ferda.Modules.PropertyValue">value</see>
        /// of the property named <c>propertyName</c>.
        /// </returns>
		PropertyValue GetPropertyDefaultValue(string propertyName);

        /// <summary>
        /// Gets TypeClassIceId of the data type of the property.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns>
        /// Returns Ice id of type of property value class i.e. 
        /// <see cref="F:Ferda.Modules.PropertyInfo.typeClassIceId">TypeClassIceId</see>.
        /// </returns>
        string GetPropertyDataType(string propertyName);

        /// <summary>
        /// <para>
        /// Gets <see cref="T:Ferda.Modules.PropertyValue"/> from 
        /// <see cref="T:Ice.ObjectPrx">objectPrx</see> parameter.
        /// </para>
        /// <para>
        /// Useful for "OtherT" types of property. This will return 
        /// instance of class implementing appropriate interface of 
        /// <see cref="T:Ferda.Modules.PropertyValue"/> or throws
        /// <see cref="T:Ferda.Modules.NameNotExistError"/> iff there
        /// is no property named as <c>propertyName</c>.
        /// </para>
        /// </summary>
        /// <example>
        /// <para>Following examples shows how to implement this function.</para>
        /// <para>
        /// This shows a slice design for new PropertyValue named <c>MyPoint</c>.
        /// </para>
        /// <code>
        /// module Ferda {
        ///		module Modules { 
        ///			//e.g.; this can be any type (class, struct, ...)
        ///			struct MyPoint
        ///			{
        ///				int x;
        ///				int y;
        ///			};
        /// 
        /// 		interface MyPointTInterface
        ///			{
        ///				nonmutating MyPoint getMyPointValue();
        ///			};
        /// 
        ///			class MyPointT extends Ferda::Modules::PropertyValue implements MyPointTInterface {
        /// 			MyPoint myPointValue;
        ///			};
        ///		};
        /// };
        /// </code>
        /// Exemplary implementation of new type <c>MyPointT</c>.
        /// <code>
        /// namespace Ferda.Modules
        /// {
        /// 	public class MyPointTI : MyPointT, IValue
        /// 	{
        ///			public MyPointTI()
        ///			{
        ///				this.myPointValue = new MyPoint();
        ///				this.myPointValue.x = 0;
        ///				this.myPointValue.y = 0;
        ///			}
        ///			public MyPointTI(MyPointTInterfacePrx iface)
        ///			{
        ///				this.myPointValue = iface.getMyPointValue();
        ///			}
        ///			public MyPointTI(MyPoint myPoint)
        ///			{
        ///				this.myPointValue = myPoint;
        ///			}
        ///			public override MyPoint getMyPointValue(Current __current)
        ///			{
        ///				return this.generationInfoValue;
        ///			}
        ///			ValueT IValue.getValueT()
        ///			{
        ///				MyPointT result = new MyPointT();
        ///				result.Value = this.myPointValue;
        ///				return result;
        ///			}
        ///		}
        /// }
        /// </code>
        /// <para>Finally, this is an implementation of this function. We presume 
        /// that there is property named "Point" in <c>MyBoxModule</c>`s properties of 
        /// our newly created PropertyValue type named <c>MyPoint</c>.</para>
        /// <code>
        /// using Ferda.Modules;
        /// 
        /// namespace MyBoxModule
        /// {
        ///		class MyBoxModuleBoxInfo : Ferda.Modules.Boxes.BoxInfo
        ///		{
        ///			/* ... */
        /// 		public override PropertyValue GetPropertyObjectFromInterface(string propertyName, Ice.ObjectPrx objectPrx)
        /// 		{
        /// 			if (this.PropertyNameExists(propertyName))
        /// 			{
        /// 				if (propertyName == "Point")
        /// 					return new MyPointTI(MyPointTInterfacePrxHelper.checkedCast(objectPrx));
        ///					return null;
        ///				}
        /// 			throw Ferda.Modules.Exceptions.NameNotExistError(null, null, null, propertyName);
        ///			}
        ///		}
        /// }
        /// </code>
        /// </example>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="objectPrx"><see cref="T:Ice.ObjectPrx"/> covering 
        /// <see cref="T:Ferda.Modules.PropertyValue"/> of the property.</param>
        /// <returns>The <see cref="T:Ferda.Modules.PropertyValue"/>.</returns>
        /// <exception cref="T:Ferda.Modules.NameNotExistError">Iff 
        /// there is no property named <c>propertyName</c></exception>
		PropertyValue GetPropertyObjectFromInterface(string propertyName, Ice.ObjectPrx objectPrx);

        /// <summary>
        /// Gets array of <see cref="T:Ferda.Modules.SelectString"/> as 
        /// options for property, whose options are dynamically variable.
        /// </summary>
        /// <example>
        /// <para>Following examples shows how to implement this function.</para>
        /// <para>First example show how to implement this function while there
        /// is no property with dynamically variable options in box module`s properties.</para>
        /// <code>
        /// namespace MyBoxModule
        /// {
        ///		class MyBoxModuleBoxInfo : Ferda.Modules.Boxes.BoxInfo
        ///		{
        ///			/* ... */
        ///			public override Ferda.Modules.SelectString[] GetPropertyOptions(string propertyName, BoxModuleI boxModule)
        ///			{
        ///				return null;
        ///			}
        ///		}
        /// }
        /// </code>
        /// <para>Next examples show how to implement this function 
        /// when there is property <c>DynamicNames</c> whose options are given 
        /// by GetDynamicNames() implemented in box module`s functions 
        /// (i.e. MyBoxModuleFunctionsI).</para>
        /// <code>
        /// namespace MyBoxModule
        /// {
        ///     class MyBoxModuleFunctionsI : MyBoxModuleFunctionsDisp_, IFunctions
        ///		{
        ///			/* ... */
        ///			public string[] GetDynamicNames()
        ///			{
        ///				/* ... */
        ///			}
        ///		}
        ///	}
        /// </code>
        /// <para>Finally, this is sample implementation of this function.</para>
        /// <code>
        /// namespace MyBoxModule
        /// {
        ///		class MyBoxModuleBoxInfo : Ferda.Modules.Boxes.BoxInfo
        ///		{
        ///			/* ... */
        ///			public override Ferda.Modules.SelectString[] GetPropertyOptions(string propertyName, BoxModuleI boxModule)
        ///			{
        ///					if (propertyName == "DynamicNames")
        ///					{
        ///						// gets options by calling box`s functions implementation class
        ///						string[] result = ((MyBoxModuleFunctionsI)boxModule.FunctionsIObj).GetDynamicNames();
        /// 
        ///						// makes a conversion from string[] to SelectString[]
        ///						return Ferda.Modules.Boxes.BoxInfo.StringArrayToSelectStringArray(
        ///							result
        ///							);
        ///					}
        ///                 else
        ///                     throw new Exception("GetPropertyOptions is not implemented in box module: " + this.identifier + " for property named " + propertyName + ".");
        ///			}
        ///		}
        /// }
        /// </code>
        /// </example>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="boxModule">The box module.</param>
        /// <returns>
        /// An array of <see cref="T:Ferda.Modules.SelectString"/>
        /// as list of options for property named <c>propertyName</c>.
        /// </returns>
        /// <remarks>
        /// This function doesn`t make any test iff the property of 
        /// specified name <c>propertyName</c> exists.
        /// </remarks>
		SelectString[] GetPropertyOptions(string propertyName, BoxModuleI boxModule);

        /// <summary>
        /// Gets array of <see cref="T:Ferda.Modules.SelectString"/> as list
        /// of options for <c>propertyName</c>.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns>
        /// An array of <see cref="T:Ferda.Modules.SelectString"/>
        /// as list of options for property named <c>propertyName</c>.
        /// </returns>
        /// <remarks>
        /// Only for static selectboxes i.e. options doesn`t depend 
        /// on box module`s inner state.
        /// </remarks>
        SelectString[] GetPropertyFixedOptions(string propertyName);

        /// <summary>
        /// Gets value of readonly property. Readonly properties may not be
        /// modified by user. Basically values of readonly properties depends on 
        /// actual inner state of Box module
        /// </summary>
        /// <param name="propertyName">Name of readonly property.</param>.
        /// <param name="boxModule">Box module.</param>
        /// <returns>A <see cref="T:Ferda.Modules.PropertyValue"/> of 
        /// readonly property named <c>propertyName</c>.</returns>
        /// <remarks>
        /// This function doesn`t make any test if specified property 
        /// (by <c>propertyName</c>) really exists and is really readonly.
        /// </remarks>
        /// <example>
        /// <para>Implementation of box module`s functions.</para>
        /// <code>
        /// namespace MyBoxModule
        /// {
        ///     class MyBoxModuleFunctionsI : MyBoxModuleFunctionsDisp_, IFunctions
        ///		{
        ///			/* ... */
        ///			public string HelloWorld()
        ///			{
        ///				return "Hello world!";
        ///			}
        ///		}
        ///	}
        /// </code>
        /// <para>Finally, this is sample implementation of this function.</para>
        /// <code>
        /// namespace MyBoxModule
        /// {
        ///		class MyBoxModuleBoxInfo : Ferda.Modules.Boxes.IBoxInfo
        ///		{
        ///			/* ... */
        ///			public override PropertyValue GetReadOnlyPropertyValue(String propertyName, BoxModuleI boxModule)
        ///			{
        ///				if (propertyName == "HelloWold")
        ///				{
        ///                 // getting readonly values by box module`s functions
        ///                 string helloWoldResult = ((MyBoxModuleFunctionsI)boxModule.FunctionsIObj).HelloWorld();
        ///                 return new Ferda.Modules.StringTI(helloWoldResult);
        ///				}
        ///             else if (propertyName == "SumFirstSecond")
        ///             {
        ///                 // computing readonly values by some execution over box module`s properties
        ///                 int sum = boxModule.GetPropertyInt("First") + boxModule.GetPropertyInt("Second");
        ///                 return new Ferda.Modules.IntTI(sum);
        ///             }
        ///             else
        ///             {
        ///                 // implementation for other properties is missing
        ///                 throw new Exception("GetReadOnlyPropertyValue is not implemented in box module: " + this.identifier + " for property named " + propertyName + ".");
        ///             }
        ///			}
        ///		}
        /// }
        /// </code>
        /// </example>
        PropertyValue GetReadOnlyPropertyValue(string propertyName, BoxModuleI boxModule);

        /// <summary>
        /// Gets localized short label for option specified by <c>optionName</c> 
        /// of the property specified by <c>propertyName</c>.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="optionName">Name of the option from <see cref="T:Ferda.Modules.SelectString"/> array.</param>
        /// <param name="localePrefs">The localization preferences.</param>
        /// <returns>
        /// Returns <c>short label</c> or <c>label</c> or <c>optionName</c> of the option.
        /// </returns>
		string GetPropertyOptionShortLocalizedLabel(string propertyName, string optionName, string[] localePrefs);

        /// <summary>
        /// Gets array of <see cref="T:Ferda.Modules.BoxType"/> as list 
        /// of types of boxes, which can be connected to <c>socketName</c>.
        /// </summary>
        /// <param name="socketName">Unique name (identifer) of socket.</param>
        /// <returns>
        /// Returns types of boxes, which can be connected to <c>socketName</c>.
        /// </returns>
        /// <remarks>
        /// <b>Box type</b> is given by the functions, which the box is providing, and array of its
        /// <see cref="T:Ferda.Modules.Serializer.BoxSerializer.NeededSocket">needed sockets</see>.
        /// Needed socket is given by its name and the functions accepted by this socket.
        /// </remarks>
		BoxType[] GetSocketTypes(string socketName);

        /// <summary>
        /// Gets names of the sockets.
        /// </summary>
        /// <returns>
        /// Array of <seealso cref="T:System.String">Strings</seealso> as 
        /// names of the sockets in the box module.
        /// </returns>
        /// <remarks>
        /// The name of the socket is its unique identifier.
        /// </remarks>
		string[] GetSocketNames();

        /// <summary>
        /// Gets the sockets of the box module.
        /// </summary>
        /// <param name="localePrefs">The localization preferences.</param>
        /// <returns>
        /// Array of <seealso cref="T:Ferda.Modules.SocketInfo">SocketInfos</seealso>.
        /// </returns>
		SocketInfo[] GetSockets(string[] localePrefs);

        /// <summary>
        /// Gets the box module`s icon.
        /// </summary>
        /// <value>
        /// The box module`s icon i.e. content of the "*.ico" file 
        /// as array of <see cref="T:System.Byte">Bytes</see>.
        /// </value>
        byte[] Icon { get; }


        /// <summary>
        /// The identifier of the box module`s type. It has to be unique!
        /// </summary>
        /// <value>
        /// The identifier of the box module`s type. Please remember that the 
        /// identifier is used for identification of the box module type/kind 
        /// so that if new instance of some box module`s type wants be created 
        /// the <see cref="T:Ferda.Modules.BoxModuleFactoryCreatorI"/>
        /// of <see cref="T:Ferda.Modules.BoxModuleFactoryI"/> with the specified 
        /// <see cref="M:Ferda.Modules.BoxModuleFactoryCreatorI.getIdentifier(Ice.Current)">identifier</see> 
        /// i.e. type is used.
        /// </value>
		string Identifier { get; }

        /// <summary>
        /// Returns boolean value that indicates wheter the property 
        /// specified by <c>propertyName</c> is readonly.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns>Returns true iff the property is readonly.</returns>
        /// <exception cref="T:Ferda.Modules.Exceptions.NameNotExistError">There 
        /// is no property named <c>propertyName</c> in the box module</exception>
		bool IsPropertyReadOnly(string propertyName);

        /// <summary>
        /// Useful for property of "OtherT" type of the property. 
        /// Returns true iff the property specified by <c>propertyName</c> is set.
        /// This information is useful for neededProperty test.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="propertyValue">Value of the property.</param>
        /// <returns>True iff property value is purposeful/entered.</returns>
		bool IsPropertySet(string propertyName, PropertyValue propertyValue);

        /// <summary>
        /// Returns boolean value that indicates wheter more than 
        /// one box can be connected to the socket specified by <c>socketName</c>.
        /// </summary>
        /// <param name="socketName">Name of the socket.</param>
        /// <returns>Returns true iff more than one box can be connected in 
        /// the socket named <c>socketName</c>; otherwise, returns false.</returns>
        /// <exception cref="T:Ferda.Modules.Exceptions.NameNotExistError">There 
        /// is no <c>socketName</c> in Box module</exception>
		bool IsSocketMoreThanOne(string socketName);

        /// <summary>
        /// Gets default value for box module`s user label. This label
        /// should predicate box module`s settings (values of its properties)
        /// for easier working with many boxes on user desktop (in FrontEnd).
        /// </summary>
        /// <param name="boxModule">Box Module.</param>
        /// <returns>
        /// String representing default user label of the box module.
        /// This label can change in time ... it can reflect inner 
        /// box module`s state e.g. up-to-date values of its properties.
        /// </returns>
        /// <remarks>
        /// Please don`t foreget that localization preferences are specified by 
        /// <see cref="P:Ferda.Modules.BoxModuleI.LocalePrefs"/>.
        /// </remarks>
        string GetDefaultUserLabel(BoxModuleI boxModule);

        /// <summary>
        /// <para>
        /// Tests existence of the property named <c>propertyName</c>.
        /// </para>
        /// <para>
        /// Returns boolean value that indicates wheather there is some
        /// property named <c>propertyName</c> in the box module.</para>
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns>Returns true iff the box module has property 
        /// named <c>propertyName</c>.</returns>
		bool TestPropertyNameExistence(string propertyName);

        /// <summary>
        /// <para>
        /// Tests existence of the socket named <c>socketName</c>.
        /// </para>
        /// <para>
        /// Returns boolean value that indicates wheather there is some
        /// socket named <c>socketName</c> in the box module.</para>
        /// </summary>
        /// <param name="socketName">Name of the socket.</param>
        /// <returns>Returns true iff the box module has socket 
        /// named <c>socketName</c>.</returns>
		bool TestSocketNameExistence(string socketName);

        /// <summary>
        /// Executes (runs) action specified by <c>actionName</c>.
        /// </summary>
        /// <param name="actionName">Name of the action.</param>
        /// <param name="boxModule">The Box module.</param>
        /// <exception cref="T:Ferda.Modules.NameNotExistError">Thrown if action named <c>actionName</c> doesn`t exist.</exception>
        /// <exception cref="T:Ferda.Modules.BoxRuntimeError">Thrown if any runtime error occured while executing the action.</exception>
        void RunAction(string actionName, BoxModuleI boxModule);

        /// <summary>
        /// Gets regular expression restricting possible values of 
        /// the property specified by <c>propertyName</c>.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns>
        /// Returns regular expression restriction for 
        /// possible values of the property named <c>propertyName</c>.
        /// </returns>
        /// <exception cref="T:Ferda.Modules.Exceptions.NameNotExistError">There 
        /// is no property named <c>propertyName</c> in the box module</exception>
        string GetPropertyRegexp(string propertyName);

        /// <summary>
        /// Gets list of numeric restrictions of possible values of 
        /// the property specified by <c>propertyName</c>.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns>
        /// Lists of numeric restrictions fo possible values of the 
        /// property named <c>propertyName</c>.
        /// </returns>
        /// <exception cref="T:Ferda.Modules.Exceptions.NameNotExistError">There 
        /// is no property named <c>propertyName</c> in the box module</exception>
        System.Collections.Generic.List<Restriction> GetPropertyRestrictions(string propertyName);


        /// <summary>
        /// Tries to the get specified phrase (<c>phraseIdentifier</c>).
        /// </summary>
        /// <param name="phraseIdentifier">The phrase`s identifier.</param>
        /// <param name="phraseLocalizedText">The phrase`s localized text.</param>
        /// <param name="localePrefs">Localization preferences</param>
        /// <returns>
        /// <c>true</c> if localization of specified phrase (<c>phraseIdentifier</c>)
        /// exists; otherwise, <c>false</c>.
        /// </returns>
        bool TryGetPhrase(string phraseIdentifier, out string phraseLocalizedText, string[] localePrefs);

        /// <summary>
        /// Validates the specified box module. (e.g. setting of some properties
        /// is right (satisfies its restrictions) but box module can not work with
        /// this setting e.g. property "OdbcConnectionString" is valid ODBC connection
        /// string but the box module can not connect with given value to the 
        /// specified data source.)
        /// </summary>
        /// <param name="boxModule">The box module.</param>
        void Validate(BoxModuleI boxModule);
	}
}