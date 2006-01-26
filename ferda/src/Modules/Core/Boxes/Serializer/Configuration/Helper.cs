using System;
using System.Collections.Generic;
using System.Text;

namespace Ferda.Modules.Boxes.Serializer.Configuration
{
    /// <summary>
    /// <para>
    /// This class provides more efficient representation of Box`s XML configuration
    /// file than <see cref="T:Ferda.Modules.Boxes.Serializer.Configuration.Box"/>.
    /// </para>
    /// <para>
    /// Instance of the <see cref="T:Ferda.Modules.Boxes.Serializer.Configuration.Box"/> 
    /// is actually deserealized XML configuration file. The 
    /// <see cref="T:Ferda.Modules.Boxes.Serializer.Configuration.Helper"/> creates 
    /// more efficient structures for futher working with the configuration 
    /// (e. g. <see cref="T:System.Collections.Generic.Dictionary">
    /// Dictionaries</see>).
    /// </para>
    /// </summary>
    /// <remarks>
    /// <para>
    /// It also makes some tests for rightness of the XML configuration file.
    /// </para>
    /// <para>
    /// For futher information about some 
    /// </para>
    /// </remarks>
    /// <seealso cref="T:Ferda.Modules.Boxes.Serializer.Configuration.Box"/>
    /// <seealso cref="T:Ferda.Modules.Boxes.Serializer.Reader"/>
    /// <seealso cref="T:Ferda.Modules.Boxes.BoxInfo"/>
    /// <seealso cref="T:Ferda.Modules.Boxes.Serializer.Localization.BoxLocalization"/>
    [Serializable]
    public class Helper
    {
        private Box box;

        /// <summary>
        /// <para>
        /// Initializes a new instance of the <see cref="Helper"/> class.
        /// </para>
        /// </summary>
        /// <param name="box">Instance of the <see cref="Box"/>.</param>
        /// <exception cref="T:System.ArgumentNullException"><c>box</c> is null reference.</exception>
        public Helper(Box box)
        {
            if (box == null)
            {
                System.Diagnostics.Debug.WriteLine("Ser06");
                throw new ArgumentNullException("box");
            }
            this.box = box;

            //prepare Dictionary of actions and Lists its needed connected sockets
            if (this.box.Actions != null)
                foreach (Action action in this.box.Actions)
                {
                    //add action to Dictionary
                    this.actions.Add(action.Name, action);

                    //prepare List of action`s needed connected sockets
                    List<string[]> neededConnectedSockets = new List<string[]>();
                    if (action.NeededConnectedSocketsOptions != null)
                        foreach (NeededConnectedSocketsOption neededSocketsOption in action.NeededConnectedSocketsOptions)
                        {
                            neededConnectedSockets.Add(neededSocketsOption.NeededConnectedSockets);
                        }
                    //add the List to Dictionary
                    this.actionNeededConnectedSockets.Add(action.Name, neededConnectedSockets.ToArray());
                }

            //prepare Dictionary of sockets
            if (this.box.Sockets != null)
                foreach (Socket socket in this.box.Sockets)
                {
                    this.sockets.Add(socket.Name, socket);
                }

            //prepere socket`s BoxTypes
            this.prepareSocketTypes();

            //prepare Dictionary of properties
            if (this.box.Properties != null)
                foreach (Property property in this.box.Properties)
                {
                    this.properties.Add(property.Name, property);
                }

            //prepare restrictions of the properties
            this.preparePropertyRestrictions();

            //prepare modules asking for creation
            if (this.box.ModulesAskingForCreationSeq != null)
                foreach (ModulesAskingForCreation modulesAskingForCreation in this.box.ModulesAskingForCreationSeq)
                {
                    this.modulesAskingForCreation.Add(modulesAskingForCreation.Name);
                }
#if DEBUG
            //each (visible) property has to be specified also as socket
            {
                // true iff there is defined property in config XML file, but
                // there is not defined the socket of the same name (identifier)
                StringBuilder missingSocketDefinition = new StringBuilder();

                foreach (string propertyName in this.properties.Keys)
                {
                    if (!this.sockets.ContainsKey(propertyName)) // && this.properties[propertyName].Visible == true
                    {
                        missingSocketDefinition.AppendLine("Ser07: There is missing definiction for socket named: " + propertyName + " in the config XML file! (" + this.Identifier + ")");
                    }
                }
                if (missingSocketDefinition.Length > 0)
                {
                    System.Diagnostics.Debug.WriteLine(missingSocketDefinition.ToString());
                    throw new Exception(missingSocketDefinition.ToString());
                }
            }
#endif
        }

        /// <summary>
        /// Gets the box`s identifier.
        /// </summary>
        /// <value>The box`s identifier.</value>
        public string Identifier
        { get { return this.box.Identifier; } }

        /// <summary>
        /// Gets the path to box`s icon design i.e. the "ico" file.
        /// </summary>
        /// <remarks>
        /// For further information about relative pathes please see remars in 
        /// <see cref="T:Ferda.Modules.Boxes.Serializer.Configuration.Box"/>.
        /// </remarks>
        /// <value>The path to the icon.</value>
        public string IconPath
        { get { return this.box.IconPath; } }

        /// <summary>
        /// Gets the path to the <see href="http://www.w3.org/tr/2000/cr-svg-20001102/index.html">
        /// Scalable Vector Graphics (SVG)</see> design file of the box.
        /// </summary>
        /// <remarks>
        /// For further information about relative pathes please see remars in 
        /// <see cref="T:Ferda.Modules.Boxes.Serializer.Configuration.Box"/>.
        /// </remarks>
        /// <value>The path to the SVG design file.</value>
        public string DesignPath
        { get { return this.box.DesignPath; } }

        /// <summary>
        /// Gets the categories i.e. names of categories where the box module belongs to.
        /// </summary>
        /// <value>The categories.</value>
        public string[] Categories
        { get { return this.box.Categories; } }

        private SortedList<string, Boxes.Serializer.Configuration.Socket> sockets = new SortedList<string, Socket>();
        /// <summary>
        /// Gets the sockets.
        /// </summary>
        /// <value>The sockets.
        /// <para><c>Key</c> is the socket`s name.</para>
        /// <para><c>Value</c> is the <see cref="Ferda.Modules.Boxes.Serializer.Configuration.Socket"/>.</para>
        /// </value>
        public SortedList<string, Boxes.Serializer.Configuration.Socket> Sockets
        {
            get { return sockets; }
        }

        private SortedList<string, Boxes.Serializer.Configuration.Property> properties = new SortedList<string, Property>();
        /// <summary>
        /// Gets the properties.
        /// </summary>
        /// <value>The properties.
        /// <para><c>Key</c> is name of the property.</para>
        /// <para><c>Value</c> is the <see cref="Ferda.Modules.Boxes.Serializer.Configuration.Property"/>.</para>
        /// </value>
        public SortedList<string, Boxes.Serializer.Configuration.Property> Properties
        {
            get { return properties; }
        }

        private Dictionary<string, Boxes.Serializer.Configuration.Action> actions = new Dictionary<string, Action>();
        /// <summary>
        /// Gets the actions.
        /// </summary>
        /// <value>The actions.
        /// <para><c>Key</c> is the action`s name.</para>
        /// <para><c>Value</c> is the <see cref="Ferda.Modules.Boxes.Serializer.Configuration.Action"/>.</para>
        /// </value>
        public Dictionary<string, Boxes.Serializer.Configuration.Action> Actions
        {
            get { return actions; }
        }

        private Dictionary<string, string[][]> actionNeededConnectedSockets = new Dictionary<string, string[][]>();
        /// <summary>
        /// Gets the action`s needed connected sockets.
        /// </summary>
        /// <value>The action`s needed connected sockets.
        /// <para><c>Key</c> is the action`s name.</para>
        /// <para><c>Value</c> is the array of conditions on needed connected 
        /// sockets. Box has to satisfy at least one of the conditions before
        /// the action can be executed. The condition is array of socket`s names
        /// in which other box(es) has to be connected.</para>
        /// </value>
        public Dictionary<string, string[][]> ActionNeededConnectedSockets
        {
            get { return actionNeededConnectedSockets; }
        }

        private List<string> modulesAskingForCreation = new List<string>();
        /// <summary>
        /// Gets names the modules asking for creation.
        /// </summary>
        /// <value>Names of the modules asking for creation.</value>
        /// <remarks>
        /// <see cref="T:Ferda.Modules.ModulesAskingForCreation"/> structures are empty because all the
        /// memebers in this structure depends on localization or dynamic (or runtime) factors.
        /// </remarks>
        public List<string> ModulesAskingForCreation
        {
            get { return modulesAskingForCreation; }
        }

        /// <summary>
        /// Gets the socket.
        /// </summary>
        /// <param name="socketName">Name of the socket.</param>
        /// <returns><see cref="T:Ferda.Modules.Boxes.Serializer.Configuration.Socket"/> if 
        /// exists an element of specified <c>socketName</c>; otherwise, throws 
        /// <see cref="T:Ferda.Modules.NameNotExistError"/>.
        /// </returns>
        /// <exception cref="T:Ferda.Modules.NameNotExistError">There is no socket with 
        /// the specified name (<c>socketName</c>) in the box.</exception>
        public Socket GetSocket(string socketName)
        {
            try
            {
                return this.Sockets[socketName];
            }
            catch (Exception ex)
            {
                throw Ferda.Modules.Exceptions.NameNotExistError(ex, "", "Ser08: GetSocket(...): socketName (" + socketName + ") in box " + this.Identifier + " doesn`t exist.", socketName);
            }
        }

        /// <summary>
        /// <para><c>Key</c> is the socket`s name.</para>
        /// <para><c>Value</c> is the array of accepted boxTypes.</para>
        /// </summary>
        private Dictionary<string, Ferda.Modules.BoxType[]> socketsBoxTypes = new Dictionary<string, Ferda.Modules.BoxType[]>();

        /// <summary>
        /// Prepares the socket`s boxTypes i.e. fill in <c>socketsBoxTypes</c>.
        /// </summary>
        private void prepareSocketTypes()
        {
            List<Ferda.Modules.BoxType> boxTypes = new List<Ferda.Modules.BoxType>();
            List<Ferda.Modules.NeededSocket> neededSockets = new List<Ferda.Modules.NeededSocket>();
            foreach (Socket socket in this.sockets.Values)
            {
                if (socket.SocketTypes != null)
                {
                    boxTypes.Clear();
                    //process soket`s boxTypes
                    foreach (BoxType boxType in socket.SocketTypes)
                    {
                        Ferda.Modules.BoxType boxTypeItem = new Ferda.Modules.BoxType();
                        boxTypeItem.functionIceId = boxType.FunctionIceId;

                        //process boxType`s needed sockets
                        neededSockets.Clear();
                        if (boxType.NeededSockets != null)
                            foreach (NeededSocket neededSocket in boxType.NeededSockets)
                            {
                                Ferda.Modules.NeededSocket neededSocketItem = new Ferda.Modules.NeededSocket();
                                neededSocketItem.functionIceId = neededSocket.FunctionIceId;
                                neededSocketItem.socketName = neededSocket.SocketName;
                                neededSockets.Add(neededSocketItem);
                            }

                        //needed sockets of the boxType are done
                        boxTypeItem.neededSockets = neededSockets.ToArray();

                        //the boxType is done
                        boxTypes.Add(boxTypeItem);
                    }
                    this.socketsBoxTypes.Add(socket.Name, boxTypes.ToArray());
                }
                else
                {
                    this.socketsBoxTypes.Add(socket.Name, new Ferda.Modules.BoxType[0]);
                }
            }
        }

        /// <summary>
        /// Gets the socket`s types.
        /// </summary>
        /// <param name="socketName">Name of the socket.</param>
        /// <returns>Array of <see cref="T:Ferda.Modules.BoxType"/>.</returns>
        /// <exception cref="T:Ferda.Modules.NameNotExistError">There is no socket with 
        /// specified <c>socketName</c> in the box.</exception>
        public Ferda.Modules.BoxType[] GetSocketTypes(string socketName)
        {
            try
            {
                return this.socketsBoxTypes[socketName];
            }
            catch (Exception ex)
            {
                throw Ferda.Modules.Exceptions.NameNotExistError(ex, "", "Ser09: GetSocket(...): socketName (" + socketName + ") in box " + this.Identifier + " doesn`t exist.", socketName);
            }
        }

        /// <summary>
        /// Gets the property.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns><see cref="T:Ferda.Modules.Boxes.Serializer.Configuration.Property"/> if 
        /// exists an element of specified <c>propertyName</c>; otherwise, throws 
        /// <see cref="T:Ferda.Modules.NameNotExistError"/>.
        /// </returns>
        /// <exception cref="T:Ferda.Modules.NameNotExistError">
        /// There is no property with specified name (<c>propertyName</c>) in the box.
        /// </exception>
        public Property GetProperty(string propertyName)
        {
            try
            {
                return this.Properties[propertyName];
            }
            catch (Exception ex)
            {
                string message = "Ser10: GetProperty(...): propertyName (" + propertyName + ") in box " + this.Identifier + " doesn`t exist.";
                System.Diagnostics.Debug.WriteLine(message);
                throw Ferda.Modules.Exceptions.NameNotExistError(ex, "", message, propertyName);
            }
        }

        /// <summary>
        /// <para><c>Key</c> is name of the property.</para>
        /// <para><c>Value</c> is the list of restrictions.</para>
        /// </summary>
        private Dictionary<string, List<Ferda.Modules.Restriction>> propertiesRestrictions = new Dictionary<string, List<Ferda.Modules.Restriction>>();

        /// <summary>
        /// Prepares lists of restictions of possible values of the properties.
        /// </summary>
        private void preparePropertyRestrictions()
        {
            List<Ferda.Modules.Restriction> result;
            foreach (Property property in this.properties.Values)
            {
                if (property.NumericalRestrictions != null)
                {
                    result = new List<Ferda.Modules.Restriction>();
                    foreach (Restriction restriction in property.NumericalRestrictions)
                    {
                        Ferda.Modules.Restriction itemOfResult = new Ferda.Modules.Restriction();
                        if (restriction.Floating != 0)
                        {
                            itemOfResult.integral = new long[0];
                            itemOfResult.floating = new double[] { restriction.Floating };
                        }
                        else // if (restriction.Floating == 0)
                        {
                            itemOfResult.integral = new long[] { restriction.Integral };
                            itemOfResult.floating = new double[0];
                        }
                        itemOfResult.min = restriction.Min;
                        itemOfResult.including = restriction.Including;
                        result.Add(itemOfResult);
                    }
                    this.propertiesRestrictions.Add(property.Name, result);
                }
                else
                {
                    this.propertiesRestrictions.Add(property.Name, new List<Ferda.Modules.Restriction>());
                }
            }
        }

        /// <summary>
        /// Gets the restrictions on possible values of the property.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns>
        /// <see cref="T:System.Collections.Generic.List"/> of 
        /// <see cref="T:Ferda.Modules.Restriction">Restriction</see>.
        /// </returns>
        /// <exception cref="T:Ferda.Modules.NameNotExistError">
        /// There is no property with specified name (<c>propertyName</c>) in the box.
        /// </exception>
        public List<Ferda.Modules.Restriction> GetPropertyRestrictions(string propertyName)
        {
            try
            {
                return this.propertiesRestrictions[propertyName];
            }
            catch (Exception ex)
            {
                string message = "Ser11: GetProperty(...): propertyName (" + propertyName + ") in box " + this.Identifier + " doesn`t exist.";
                System.Diagnostics.Debug.WriteLine(message);
                throw Ferda.Modules.Exceptions.NameNotExistError(ex, "", message, propertyName);
            }
        }
    }
}
