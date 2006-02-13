using System;
using System.Resources;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Threading;

using Ferda.FrontEnd.Menu;
using Ferda.FrontEnd.External;
using Ferda.ModulesManager;
using Ferda.Modules;
using Ferda.FrontEnd.Archive;
using Ferda.FrontEnd.Desktop;

namespace Ferda.FrontEnd.Properties
{
    /// <summary>
    /// Property Grid for the Ferda application
    /// </summary>
    ///<stereotype>control</stereotype>
    public class FerdaPropertyGrid : System.Windows.Forms.PropertyGrid, IPropertiesDisplayer,
        IOtherObjectDisplayer, IAsyncPropertyManager
    {
        #region Class fields

        private List<IViewDisplayer> viewDisplayers;
        private IArchiveDisplayer archiveDisplayer;

        //Resource manager from the FerdaForm
        private ResourceManager resManager;
        /// <summary>
        /// Localization manager of the application
        /// </summary>
        protected Menu.ILocalizationManager localizationManager;

        /// <summary>
        /// Menu of the application
        /// </summary>
        protected IMenuDisplayer menuDisplayer;
        /// <summary>
        /// Toolbar of the application
        /// </summary>
        protected IMenuDisplayer toolBar;

        private bool isOneBoxSelected;
        private List<IBoxModule> selectedBoxes;
        private IBoxModule selectedBox;
        private int boxIdentifier;

        /// <summary>
        /// The property bag where values of the property are stored
        /// </summary>
        protected External.PropertyTable propertyBag;

        /// <summary>
        /// Structure where temporary values of the properties are stored
        /// </summary>
        protected Dictionary<string, object> temporaryValues;

        #endregion

        #region Propetries
        
        /// <summary>
        /// Identifier of the box which properties are beeing shown
        /// at the moment. It serves the asynchronous property getting.
        /// </summary>
        public int BoxIdentifier
        {
            set
            {
                boxIdentifier = value;
            }
            get
            {
                return boxIdentifier;
            }
        }

        /// <summary>
        /// View displayer
        /// </summary>
        public List<IViewDisplayer> ViewDisplayers
        {
            set
            {
                viewDisplayers = value;
            }
            get
            {
                return viewDisplayers;
            }
        }

        /// <summary>
        /// ArchiveDisplayer representing the archive control of the application
        /// </summary>
        public IArchiveDisplayer ArchiveDisplayer
        {
            set
            {
                archiveDisplayer = value;
            }
            get
            {
                return archiveDisplayer;
            }
        }

        /// <summary>
        /// Resource manager of the application, it is filled according to the
        /// current localization
        /// </summary>
        public ResourceManager ResManager
        {
            set
            {
                resManager = value;
            }
            get
            {
                if (resManager == null)
                {
                    throw new ApplicationException(
                        "PropertyGrid.ResManager cannot be null");
                }
                return resManager;
            }
        }

        ///<summary>
        ///Determines which boxes are selected in the control
        ///</summary>
        ///<remarks>
        ///Tady nekde by mel oznamit vsem ostatnim kontrolum, ze doslo ke
        ///zmene...
        ///</remarks>
        public List<IBoxModule> SelectedBoxes
        {
            set
            {
                selectedBoxes = value;
            }
            get
            {
                return selectedBoxes;
            }
        }

        ///<summary>
        ///Determines which box is selected in the control
        ///</summary>
        ///<remarks>
        ///Tady nekde by mel oznamit vsem ostatnim kontrolum, ze doslo ke
        ///zmene...
        ///</remarks>
        public IBoxModule SelectedBox
        {
            set
            {
                selectedBox = value;
            }
            get
            {
                return selectedBox;
            }
        }

        ///<summary>
        ///Determines, wheather only one box is selected in the control
        ///(and thus all the context menu of box module actions ... can
        ///be displayed)
        ///</summary>
        public bool IsOneBoxSelected
        {
            set
            {
                isOneBoxSelected = value;
            }
            get
            {
                return isOneBoxSelected;
            }
        }

        #endregion

        #region Constructor

        ///<summary>
        /// Default constructor for FerdaPropertyGrid class.
        ///</summary>
        ///<param name="locManager">Localization manager of the application</param>
        ///<param name="menuDisp">Menu of the application</param>
        ///<param name="toolBar">Toolbar of the application</param>
        public FerdaPropertyGrid(Menu.ILocalizationManager locManager,
            IMenuDisplayer menuDisp, IMenuDisplayer toolBar)
            : base()
        {
            viewDisplayers = new List<IViewDisplayer>();

            localizationManager = locManager;
            ResManager = localizationManager.ResManager;

            //setting the menu displayer
            menuDisplayer = menuDisp;
            this.toolBar = toolBar;

            //Setting the size
            Size = new System.Drawing.Size(170, 500);

            //setting the focus
            Enter += new EventHandler(FerdaPropertyGrid_Enter);
        }

        #endregion

        #region Methods

        #region IPropertiesDisplayer & implementation

        ///<summary>
        ///Retrieves the selected object from the desktop or archive and
        ///displays its properties in the property bar
        ///</summary>
        public void Adapt()
        {
            temporaryValues = new Dictionary<string, object>();

            if (IsOneBoxSelected) //creating properties for 1 box only
            {
                boxIdentifier = SelectedBox.ProjectIdentifier;
                propertyBag = CreatePropertiesFromBox(SelectedBox);
                AddSocketProperties(propertyBag, SelectedBox);

                this.SelectedObject = propertyBag;
            }
            else
            {
                //PropertyTable[] tables = new PropertyTable[SelectedBoxes.Count];
                //for (int i = 0; i < SelectedBoxes.Count; i++)
                //{
                //    tables[i] = CreatePropertiesFromBox(SelectedBoxes[i], true);
                //}
                //this.SelectedObjects = tables;
            }
        }

        ///<summary>
        ///This function is called when the localization
        ///of the application is changed - the whole menu needs to be redrawn
        ///</summary>
        public void ChangeLocalization()
        {
            //updating the resource manager
            ResManager = localizationManager.ResManager;
        }

        /// <summary>
        /// Resets the propetry grid to be without properties
        /// </summary>
        public void Reset()
        {
            propertyBag = new PropertyTable();
            this.SelectedObject = propertyBag;
        }

        /// <summary>
        /// Adapts the PropertyGrid when another object than a box is selected.
        /// Uses the SelectedObject as its object.
        /// </summary>
        /// <param name="objectProperties">The 
        /// <see cref="T:Ferda.FrontEnd.External.PropertyTable"/> object
        /// that contains information about the object.
        /// </param>
        public void OtherObjectAdapt(PropertyTable objectProperties)
        {
            this.SelectedObject = objectProperties;
        }

        #endregion

        #region IAsyncPropertyManager implementation + related functions

        /// <summary>
        /// Add the temporary values into the temporaryValues dictionary
        /// </summary>
        /// <param name="normalType">Type of the property</param>
        /// <param name="propertyName">Name of the property (not the localized label)
        /// </param>
        protected void AddAsyncStuff(string propertyName, string normalType)
        {
            //writing to the temporaryValues dictionary
            switch (normalType)
            {
                case "System.Int32":
                    temporaryValues.Add(propertyName, 0);
                    break;

                case "Ferda.FrontEnd.Properties.OtherProperty":
                    temporaryValues.Add(propertyName, null);
                    break;

                case "System.String":
                    temporaryValues.Add(propertyName, string.Empty);
                    break;

                case "System.Boolean":
                    temporaryValues.Add(propertyName, false);
                    break;

                case "System.Int16":
                    temporaryValues.Add(propertyName, (Int16)0);
                    break;

                case "System.Int64":
                    temporaryValues.Add(propertyName, (Int64)0);
                    break;

                case "System.Double":
                    temporaryValues.Add(propertyName, (Double)0);
                    break;

                case "System.Single":
                    temporaryValues.Add(propertyName, (Single)0);
                    break;

                case "System.DateTime":
                    temporaryValues.Add(propertyName, new DateTime());
                    break;

                case "System.TimeSpan":
                    temporaryValues.Add(propertyName, new TimeSpan());
                    break;

                case "Ferda.FrontEnd.Properties.StringSequence":
                    temporaryValues.Add(propertyName, null);
                    break;

                default:
                    break;
            }

            //writing to the asyncprotpertyCatchers 
            AsyncPropertyCatcher catcher = 
                new AsyncPropertyCatcher(this, propertyName, normalType, 
                SelectedBox.ProjectIdentifier);
            //Console.WriteLine("Box: " + SelectedBox.ProjectIdentifier.ToString() + " property: " + propertyName);
            SelectedBox.GetProperty_async(catcher, propertyName);
        }

        /// <summary>
        /// The property value is changed, so the the propertyGrid should be
        /// refilled with new values
        /// </summary>
        /// <param name="catcher">Catcher of the connection</param>
        /// <param name="value">New value of the property</param>
        public void ChangedPropertyValue(AsyncPropertyCatcher catcher, object value)
        {
            //changing one of the temporary values
            //writing to the temporaryValues dictionary
            switch (catcher.PropertyType)
            {
                case "System.Int32":
                    temporaryValues[catcher.PropertyName] = ((IntT)value).intValue;
                    break;

                case "Ferda.FrontEnd.Properties.OtherProperty":
                    string resultValue = SelectedBox.GetPropertyOtherAboutFromValue(catcher.PropertyName, 
                        (PropertyValue)value);
                    OtherProperty prop = new OtherProperty(SelectedBox, catcher.PropertyName, 
                        archiveDisplayer, viewDisplayers, this, ResManager);
                    prop.Result = resultValue;
                    temporaryValues[catcher.PropertyName] = prop;
                    break;

                case "Ferda.FrontEnd.Properties.StringSequence":
                    string selectedValue = ((StringT)value).stringValue;
                    //creating a new stringSequence
                    StringSequence seq = new StringSequence(catcher.PropertyName,
                        new IBoxModule[] { SelectedBox }, 
                        ResManager, ArchiveDisplayer, ViewDisplayers,
                        this, selectedValue);
                    temporaryValues[catcher.PropertyName] = seq;
                    break;

                case "System.String":
                    temporaryValues[catcher.PropertyName] = ((StringT)value).stringValue;
                    break;

                case "System.Boolean":
                    temporaryValues[catcher.PropertyName] = ((BoolT)value).boolValue;
                    break;

                case "System.Int16":
                    temporaryValues[catcher.PropertyName] = ((ShortT)value).shortValue;
                    break;

                case "System.Int64":
                    temporaryValues[catcher.PropertyName] = ((LongT)value).longValue;
                    break;

                case "System.Double":
                    temporaryValues[catcher.PropertyName] = ((DoubleT)value).doubleValue;
                    break;

                case "System.Single":
                    temporaryValues[catcher.PropertyName] = ((FloatT)value).floatValue;                    
                    break;

                case "System.DateTime":
                    //a date property is treated differently due to unexisting conversion
                    if (value is DateT)
                    {
                        DateT v = (DateT)value;
                        temporaryValues[catcher.PropertyName] = new DateTime(v.year,
                            v.month, v.day);
                    }
                    else //it is a DateTimeT thing
                    {
                        DateTimeT v = (DateTimeT)value;
                        temporaryValues[catcher.PropertyName] = new DateTime(v.year,
                            v.month, v.day, v.hour, v.minute, v.second);
                    }
                    break;

                case "System.TimeSpan":
                    TimeT val = (TimeT)value;
                    temporaryValues[catcher.PropertyName] = new TimeSpan(val.hour,
                        val.minute, val.second);
                    break;

                default:
                    break;
            }

            //refilling the propertyGrid
            SelectedObject = propertyBag;
        }

        #endregion

        #region Creating properties

        /// <summary>
        /// Adds the properties of the selectedBox to the bag
        /// </summary>
        /// <param name="box">Box from where the properties should be
        /// loaded</param>
        /// <returns>A bag full of properties from the box</returns>
        protected PropertyTable CreatePropertiesFromBox(IBoxModule box)
        {
            PropertyTable bag = new PropertyTable();
            IBoxModuleFactoryCreator creator = box.MadeInCreator;

            FerdaPropertySpec ps;
            bag.GetValue += new PropertySpecEventHandler(propertyBag_GetValue);
            bag.SetValue += new PropertySpecEventHandler(propertyBag_SetValue);

            //iterating through all the properties
            foreach (PropertyInfo pinfo in creator.Properties)
            {
                if (pinfo.visible)
                {
                    if (box.IsPropertySetWithSettingModule(pinfo.name))
                    { //creating the "other" property
                        if (IsOneBoxSelected)
                        {
                            CreateOtherProperty(pinfo, box, bag);
                        }
                    }
                    else
                    { //creating normal property
                        //two known other property types - StringSeqT and CategoriesT
                        if (pinfo.typeClassIceId == "::Ferda::Modules::StringSeqT" ||
                            pinfo.typeClassIceId == "::Ferda::Modules::CategoriesT")
                        {
                            CreateOtherProperty(pinfo, box, bag);
                            continue;
                        }

                        //strings are also dealt with separatelly
                        if (pinfo.typeClassIceId == "::Ferda::Modules::StringT")
                        {
                            CreateStringProperty(pinfo, box, bag);
                        }
                        else
                        {
                            string normalType = GetNormalType(pinfo.typeClassIceId);

                            //This is a normal type, creating a normal property for it
                            if (normalType != "")
                            {
                                //getting the displayable name of the property
                                SocketInfo si = creator.GetSocket(pinfo.name);
                                ps = new FerdaPropertySpec(si.label, normalType, false);
                                ps.Category = pinfo.categoryName;

                                //geting the socket information about the category
                                ps.Description = si.hint;

                                //it is readonly or it is already there as a socket -
                                //cannot edit "socketed" value
                                if (box.IsPropertyReadOnly(pinfo.name) || 
                                    box.GetPropertySocking(pinfo.name))
                                {
                                    ps.Attributes = new Attribute[]
                                    {
                                       ReadOnlyAttribute.Yes
                                    };
                                }

                                bag.Properties.Add(ps);

                                //adding the asynchronous stuff
                                if (IsOneBoxSelected)
                                {
                                    AddAsyncStuff(pinfo.name, normalType);
                                }
                            }
                            else
                            {
                                //throw new ApplicationException("Wierd type that we dont know!!!");
                            }
                        }
                    }

                }
            }

            //AddConnectionProperties();
            return bag;
        }

        /// <summary>
        /// Adds to the <see cref="T:Ferda.FrontEnd.External.PropertyTable"/> table
        /// information about sockets of the box
        /// </summary>
        /// <param name="propertyTable">Table to put the properties</param>
        /// <param name="box">Box from where to get the properties</param>
        protected PropertyTable AddSocketProperties(PropertyTable propertyTable, IBoxModule box)
        {
            IBoxModuleFactoryCreator creator = box.MadeInCreator;
            FerdaPropertySpec ps;

            foreach (PropertyInfo pinfo in creator.Properties)
            {
                //we are not adding the visible and readonly properties
                if (pinfo.visible && !pinfo.readOnly)
                {
                    //getting the displayable name of the property
                    SocketInfo si = creator.GetSocket(pinfo.name);

                    //zatim je to udelane takhle
                    if (si.label != "")
                    {
                        ps = new FerdaPropertySpec(si.label, "System.Boolean", true);
                        ps.Category = ResManager.GetString("PropertySocketName");
                        ps.Description = ResManager.GetString("PropertySocketHint");
                        propertyTable.Properties.Add(ps);
                    }
                    else
                    {
                        throw new ApplicationException("A not read-only, visible property does not have a label.");
                    }
                }
            }

            return propertyTable;
        }

        /// <summary>
        /// Creates a property for a "::Ferda::Modules::StringT". There can be not 
        /// only string, but also types of ComboBoxes and this method decides, which
        /// FerdaPropertySpec will be created
        /// </summary>
        /// <param name="pinfo">Information about the property</param>
        /// <param name="box">Box where it finds the other property</param>
        /// <param name="bag">Bag where to put the FerdaPropertySpec</param>
        protected void CreateStringProperty(PropertyInfo pinfo, IBoxModule box, PropertyTable bag)
        {
            FerdaPropertySpec ps;
            SelectString[] array;

            //determining the type of the property
            array = box.GetPropertyOptions(pinfo.name);

            if (array.Length == 0) //it is a normal string property
            {
                SocketInfo si = box.MadeInCreator.GetSocket(pinfo.name);

                ps = new FerdaPropertySpec(si.label, typeof(string), false);
                ps.Category = pinfo.categoryName;
                ps.Description = si.hint;
                if (box.IsPropertyReadOnly(pinfo.name) || box.GetPropertySocking(pinfo.name))
                {
                    ps.Attributes = new Attribute[]
                        {
                            ReadOnlyAttribute.Yes
                        };
                }

                bag.Properties.Add(ps);

                //adding the asynch stuff
                if (IsOneBoxSelected)
                {
                    AddAsyncStuff(pinfo.name, "System.String");
                }
            }
            else //a combo-box should be used
            {
                //if there are more boxes selected, the property should not be added
                if (!IsOneBoxSelected)
                {
                    return;
                }
                //TODO az se vyresi problem s jinymi typy u selectedObjects, tak se tohle vypusti

                if (box.ArePropertyOptionsObligatory(pinfo.name))
                //this means user cannot edit values
                {
                    SocketInfo si = box.MadeInCreator.GetSocket(pinfo.name);

                    ps = new FerdaPropertySpec(si.label, typeof(StringSequence), false);
                    ps.Category = pinfo.categoryName;
                    ps.Description = si.hint;

                    //atributes
                    if (box.IsPropertyReadOnly(pinfo.name) || box.GetPropertySocking(pinfo.name))
                    {
                        ps.Attributes = new Attribute[]
                            {
                            ReadOnlyAttribute.Yes,
                            new EditorAttribute(typeof(StringComboEditor), typeof(System.Drawing.Design.UITypeEditor)),
                            new TypeConverterAttribute(typeof(StringSequenceConverter))
                            };
                    }
                    else
                    {
                        ps.Attributes = new Attribute[]
                            {
                            new EditorAttribute(typeof(StringComboEditor), typeof(System.Drawing.Design.UITypeEditor)),
                            new TypeConverterAttribute(typeof(StringSequenceConverter))
                            };
                    }

                    //adding to the bag
                    bag.Properties.Add(ps);
                }
                else
                //user can add his own option to the string selection
                {
                    SocketInfo si = box.MadeInCreator.GetSocket(pinfo.name);

                    ps = new FerdaPropertySpec(si.label, typeof(StringSequence), false);
                    ps.Description = si.hint;
                    ps.Category = pinfo.categoryName;

                    //atributes
                    if (box.IsPropertyReadOnly(pinfo.name) || box.GetPropertySocking(pinfo.name))
                    {
                        ps.Attributes = new Attribute[]
                            {
                            ReadOnlyAttribute.Yes,
                            new EditorAttribute(typeof(StringComboAddingEditor), typeof(System.Drawing.Design.UITypeEditor)),
                            new TypeConverterAttribute(typeof(StringSequenceConverter))
                            };
                    }
                    else
                    {
                        ps.Attributes = new Attribute[]
                            {
                            new EditorAttribute(typeof(StringComboAddingEditor), typeof(System.Drawing.Design.UITypeEditor)),
                            new TypeConverterAttribute(typeof(StringSequenceConverter))
                            };
                    }

                    //adding to the bag
                    bag.Properties.Add(ps);
                }

                //adding the asynch stuff
                if (IsOneBoxSelected)
                {
                    AddAsyncStuff(pinfo.name, "Ferda.FrontEnd.Properties.StringSequence");
                }
            }
        }

        /// <summary>
        /// Creates a property for all other properties
        /// </summary>
        /// <param name="pinfo">Information about the property</param>
        /// <param name="box">Box where it finds the other property</param>
        /// <param name="bag">Bag where to put the propertyspec</param>
        protected void CreateOtherProperty(PropertyInfo pinfo, IBoxModule box, PropertyTable bag)
        {
            FerdaPropertySpec ps;

            SocketInfo si = box.MadeInCreator.GetSocket(pinfo.name);

            ps = new FerdaPropertySpec(si.label, typeof(OtherProperty), false);
            ps.Description = si.hint;
            ps.Category = pinfo.categoryName;

            //properties that user can set without executing the module (ODBC Connection string)
            if (box.IsPossibleToSetWithAbout(pinfo.name))
            //using a OtherPropertyAddingConverter
            {
                if (box.IsPropertyReadOnly(pinfo.name) || box.GetPropertySocking(pinfo.name))
                {
                    ps.Attributes = new Attribute[]
                    {
                        ReadOnlyAttribute.Yes,
                        new TypeConverterAttribute(typeof(OtherPropertyAddingConverter)), 
                        new EditorAttribute(typeof(OtherPropertyEditor), typeof(System.Drawing.Design.UITypeEditor))
                    };
                }
                else
                {
                    ps.Attributes = new Attribute[]
                    {
                        new TypeConverterAttribute(typeof(OtherPropertyAddingConverter)),
                        new EditorAttribute(typeof(OtherPropertyEditor), typeof(System.Drawing.Design.UITypeEditor))
                    };
                }
            }
            else //using a OtherPropertyConverter
            {
                if (box.IsPropertyReadOnly(pinfo.name) || box.GetPropertySocking(pinfo.name))
                {
                    ps.Attributes = new Attribute[]
                    {
                        ReadOnlyAttribute.Yes,
                        new TypeConverterAttribute(typeof(OtherPropertyConverter)),
                        new EditorAttribute(typeof(OtherPropertyEditor), typeof(System.Drawing.Design.UITypeEditor))
                    };
                }
                else
                {
                    ps.Attributes = new Attribute[]
                    {
                        new TypeConverterAttribute(typeof(OtherPropertyConverter)), 
                        new EditorAttribute(typeof(OtherPropertyEditor), typeof(System.Drawing.Design.UITypeEditor))
                    };
                }
            }

            AddAsyncStuff(pinfo.name, "Ferda.FrontEnd.Properties.OtherProperty");
            bag.Properties.Add(ps);
        }

        #endregion

        #region Getting and setting methods

        /// <summary>
        /// Sets a normal property of the box (not the socket one)
        /// </summary>
        /// <param name="e">Parameters of the property to set</param>
        /// <param name="propertyName">Name of the property</param>
        /// <param name="typeName">Type of the property</param>
        protected void SetNormalProperty(PropertySpecEventArgs e,
            string propertyName, string typeName)
        {
            switch (typeName)
            {
                //the string sequence is dealt separatelly in the string combo editor
                //and stringcombo adding editor
                case "System.Int32":
                    if (IsOneBoxSelected)
                    {
                        if (SelectedBox.TryWriteEnter())
                        {
                            //first setting the value into the temporary structures
                            temporaryValues[propertyName] = (int)e.Value;
                            SelectedObject = propertyBag;

                            //then setting it to the box
                            SelectedBox.SetPropertyInt(propertyName, (int)e.Value);
                            SelectedBox.WriteExit();
                        }
                        else
                        {
                            FrontEndCommon.CannotWriteToBox(SelectedBox, ResManager);
                        }
                    }
                    else //setting the property for all the selected boxes
                    {
                        foreach (IBoxModule box in SelectedBoxes)
                        {
                            if (box.TryWriteEnter())
                            {
                                box.SetPropertyInt(propertyName, (int)e.Value);
                                box.WriteExit();
                            }
                            else
                            {
                                FrontEndCommon.CannotWriteToBox(box, ResManager);
                            }
                        }
                    }
                    break;

                case "Ferda.FrontEnd.Properties.OtherProperty":
                    if (IsOneBoxSelected)
                    {
                        if (SelectedBox.TryWriteEnter())
                        {
                            if (SelectedBox.IsPossibleToSetWithAbout(propertyName))
                            {
                                OtherProperty op = e.Value as OtherProperty;
                                SelectedBox.SetPropertyOtherAbout(propertyName, op.Result);
                            }
                            SelectedBox.WriteExit();
                        }
                        else
                        {
                            FrontEndCommon.CannotWriteToBox(SelectedBox, ResManager);
                        }
                    }
                    else
                    {
                        //every box in the selected boxes should have this property
                        if (SelectedBoxes[0].IsPossibleToSetWithAbout(propertyName))
                        {
                            foreach (IBoxModule box in SelectedBoxes)
                            {
                                if (box.TryWriteEnter())
                                {
                                    OtherProperty op = e.Value as OtherProperty;
                                    box.SetPropertyOtherAbout(propertyName, op.Result);
                                    box.WriteExit();
                                }
                                else
                                {
                                    FrontEndCommon.CannotWriteToBox(box, ResManager);
                                }
                            }
                        }
                    }

                    break;

                case "System.String":
                    if (IsOneBoxSelected)
                    {
                        if (SelectedBox.TryWriteEnter())
                        {
                            //first setting the value into the temporary structures
                            temporaryValues[propertyName] = (string)e.Value;
                            SelectedObject = propertyBag;

                            //then setting it to the box

                            SelectedBox.SetPropertyString(propertyName, (string)e.Value);
                            SelectedBox.WriteExit();
                        }
                        else
                        {
                            FrontEndCommon.CannotWriteToBox(SelectedBox, ResManager);
                        }
                    }
                    else //setting the property for all the selected boxes
                    {
                        foreach (IBoxModule box in SelectedBoxes)
                        {
                            if (box.TryWriteEnter())
                            {
                                box.SetPropertyString(propertyName, (string)e.Value);
                                box.WriteExit();
                            }
                            else
                            {
                                FrontEndCommon.CannotWriteToBox(box, ResManager);
                            }
                        }
                    }
                    break;

                case "System.Boolean":
                    if (IsOneBoxSelected)
                    {
                        if (SelectedBox.TryWriteEnter())
                        {
                            //first setting the value into the temporary structures
                            temporaryValues[propertyName] = (bool)e.Value;
                            SelectedObject = propertyBag;

                            //then setting it to the box
                            SelectedBox.SetPropertyBool(propertyName, (bool)e.Value);
                            SelectedBox.WriteExit();
                        }
                        else
                        {
                            FrontEndCommon.CannotWriteToBox(SelectedBox, ResManager);
                        }
                    }
                    else //setting the property for all the selected boxes
                    {
                        foreach (IBoxModule box in SelectedBoxes)
                        {
                            if (box.TryWriteEnter())
                            {
                                box.SetPropertyBool(propertyName, (bool)e.Value);
                                box.WriteExit();
                            }
                            else
                            {
                                FrontEndCommon.CannotWriteToBox(box, ResManager);
                            }
                        }
                    }
                    break;

                case "System.Int16":
                    if (IsOneBoxSelected)
                    {
                        if (SelectedBox.TryWriteEnter())
                        {
                            //first setting the value into the temporary structures
                            temporaryValues[propertyName] = (short)e.Value;
                            SelectedObject = propertyBag;

                            //then setting it to the box
                            SelectedBox.SetPropertyShort(propertyName, (short)e.Value);
                            SelectedBox.WriteExit();
                        }
                        else
                        {
                            FrontEndCommon.CannotWriteToBox(SelectedBox, ResManager);
                        }
                    }
                    else //setting the property for all the selected boxes
                    {
                        foreach (IBoxModule box in SelectedBoxes)
                        {
                            if (box.TryWriteEnter())
                            {
                                box.SetPropertyShort(propertyName, (short)e.Value);
                                box.WriteExit();
                            }
                            else
                            {
                                FrontEndCommon.CannotWriteToBox(box, ResManager);
                            }
                        }
                    }
                    break;

                case "System.Int64":
                    if (IsOneBoxSelected)
                    {
                        if (SelectedBox.TryWriteEnter())
                        {
                            //first setting the value into the temporary structures
                            temporaryValues[propertyName] = (long)e.Value;
                            SelectedObject = propertyBag;

                            //then setting it to the box
                            SelectedBox.SetPropertyLong(propertyName, (long)e.Value);
                            SelectedBox.WriteExit();
                        }
                        else
                        {
                            FrontEndCommon.CannotWriteToBox(SelectedBox, ResManager);
                        }
                    }
                    else //setting the property for all the selected boxes
                    {
                        foreach (IBoxModule box in SelectedBoxes)
                        {
                            if (box.TryWriteEnter())
                            {
                                box.SetPropertyLong(propertyName, (long)e.Value);
                                box.WriteExit();
                            }
                            else
                            {
                                FrontEndCommon.CannotWriteToBox(box, ResManager);
                            }
                        }
                    }
                    break;

                case "System.Double":
                    if (IsOneBoxSelected)
                    {
                        if (SelectedBox.TryWriteEnter())
                        {
                            //first setting the value into the temporary structures
                            temporaryValues[propertyName] = (double)e.Value;
                            SelectedObject = propertyBag;

                            //then setting it to the box
                            SelectedBox.SetPropertyDouble(propertyName, (double)e.Value);
                            SelectedBox.WriteExit();
                        }
                        else
                        {
                            FrontEndCommon.CannotWriteToBox(SelectedBox, ResManager);
                        }
                    }
                    else //setting the property for all the selected boxes
                    {
                        foreach (IBoxModule box in SelectedBoxes)
                        {
                            if (box.TryWriteEnter())
                            {
                                box.SetPropertyDouble(propertyName, (double)e.Value);
                                box.WriteExit();
                            }
                            else
                            {
                                FrontEndCommon.CannotWriteToBox(box, ResManager);
                            }
                        }
                    }
                    break;

                case "System.Single":
                    if (IsOneBoxSelected)
                    {
                        if (SelectedBox.TryWriteEnter())
                        {
                            //first setting the value into the temporary structures
                            temporaryValues[propertyName] = (float)e.Value;
                            SelectedObject = propertyBag;

                            //then setting it to the box
                            SelectedBox.SetPropertyFloat(propertyName, (float)e.Value);
                            SelectedBox.WriteExit();
                        }
                        else
                        {
                            FrontEndCommon.CannotWriteToBox(SelectedBox, ResManager);
                        }
                    }
                    else //setting the property for all the selected boxes
                    {
                        foreach (IBoxModule box in SelectedBoxes)
                        {
                            if (box.TryWriteEnter())
                            {
                                box.SetPropertyFloat(propertyName, (float)e.Value);
                                box.WriteExit();
                            }
                            else
                            {
                                FrontEndCommon.CannotWriteToBox(box, ResManager);
                            }
                        }
                    }
                    break;

                case "System.DateTime":
                    if (IsOneBoxSelected)
                    {
                        if (SelectedBox.TryWriteEnter())
                        {
                            //first setting the value into the temporary structures
                            temporaryValues[propertyName] = (DateTime)e.Value;
                            SelectedObject = propertyBag;

                            //then setting it to the box
                            SetCorrectDateType(e, SelectedBox);
                            SelectedBox.WriteExit();
                        }
                        else
                        {
                            FrontEndCommon.CannotWriteToBox(SelectedBox, ResManager);
                        }
                    }
                    else //setting the property for all the selected boxes
                    {
                        foreach (IBoxModule box in SelectedBoxes)
                        {
                            if (box.TryWriteEnter())
                            {
                                SetCorrectDateType(e, box);
                                box.WriteExit();
                            }
                            else
                            {
                                FrontEndCommon.CannotWriteToBox(box, ResManager);
                            }
                        }
                    }
                    break;

                case "System.TimeSpan":
                    if (IsOneBoxSelected)
                    {
                        if (SelectedBox.TryWriteEnter())
                        {
                            //first setting the value into the temporary structures
                            temporaryValues[propertyName] = (TimeSpan)e.Value;
                            SelectedObject = propertyBag;

                            //then setting it to the box
                            SelectedBox.SetPropertyTime(propertyName, (TimeSpan)e.Value);
                            SelectedBox.WriteExit();
                        }
                        else
                        {
                            FrontEndCommon.CannotWriteToBox(SelectedBox, ResManager);
                        }
                    }
                    else //setting the property for all the selected boxes
                    {
                        foreach (IBoxModule box in SelectedBoxes)
                        {
                            if (box.TryWriteEnter())
                            {
                                box.SetPropertyTime(propertyName, (TimeSpan)e.Value);
                                box.WriteExit();
                            }
                            else
                            {
                                FrontEndCommon.CannotWriteToBox(box, ResManager);
                            }
                        }
                    }
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// Sets a socket property of the box (not the normal one)
        /// </summary>
        /// <param name="e">Parameters of the property to set</param>
        /// <param name="propertyName">Name of the property (socket)</param>
        /// <returns>The indication if a socking of the property has been
        /// changed.</returns>
        protected bool SetSocketProperty(PropertySpecEventArgs e, string
            propertyName)
        {
            bool previousValue = SelectedBox.GetPropertySocking(propertyName);
            bool thisValue = (bool)e.Value;

            if (thisValue == previousValue)
            {
                //nothing has changed
                return false;
            }
            else
            {
                //something has changed
                //trying to access the selected box
                if (SelectedBox.TryWriteEnter())
                {
                    SelectedBox.SetPropertySocking(propertyName, (bool)e.Value);
                }
                else
                {
                    FrontEndCommon.CannotWriteToBox(SelectedBox, ResManager);
                }

                return true;
            }
        }

        /// <summary>
        /// Gets a normal property of the box (not the socket one)
        /// </summary>
        /// <param name="e">Parameters of the property to set</param>
        /// <param name="realPropertyName">Name of the property(identifier)</param>
        /// <param name="typeName">Type of the property</param>
        protected void GetNormalProperty(PropertySpecEventArgs e, string
            realPropertyName, string typeName)
        {
            //the object when we get values from more selected boxes
            object value;
            switch (typeName)
            {
                //TODO doresit jeste nejak i tohle - kdyz se ptaji na OtherProperty,
                //hlavne nejak rozumne odladit
                case "Ferda.FrontEnd.Properties.OtherProperty":
                    if (IsOneBoxSelected)
                    {
                        //e.Value = GetOtherProperty(SelectedBox, realPropertyName);
                        e.Value = (OtherProperty)temporaryValues[realPropertyName];
                    }
                    else
                    {
                        throw new ApplicationException("This situation should not happen, other property can be set for one selected box only");
                    }
                    break;
                //tohle taky doresit
                case "Ferda.FrontEnd.Properties.StringSequence":
                    if (IsOneBoxSelected)
                    {
                        e.Value = (StringSequence)temporaryValues[realPropertyName];
                    }
                    else
                    {
                        //value = GetStringSequenceMoreBoxes(realPropertyName);
                        //if (value != null)
                        //{
                        //    e.Value = (StringSequence)value;
                        //}
                    }
                    break;

                case "System.String":
                    if (IsOneBoxSelected)
                    {
                        e.Value = (string)temporaryValues[realPropertyName];
                    }
                    else
                    {
                        value = GetMoreBoxesProperty(typeName, realPropertyName);
                        if (value != null)
                        {
                            e.Value = (string)value;
                        }
                    }
                    break;

                case "System.Int32":
                    if (IsOneBoxSelected)
                    {
                        e.Value = (Int32)temporaryValues[realPropertyName];
                    }
                    else
                    {
                        value = GetMoreBoxesProperty(typeName, realPropertyName);
                        if (value != null)
                        {
                            e.Value = (Int32)value;
                        }
                    }
                    break;

                case "System.Boolean":
                    if (IsOneBoxSelected)
                    {
                        e.Value = (bool)temporaryValues[realPropertyName];
                    }
                    else
                    {
                        value = GetMoreBoxesProperty(typeName, realPropertyName);
                        if (value != null)
                        {
                            e.Value = (bool)value;
                        }
                    }
                    break;

                case "System.Int16":
                    if (IsOneBoxSelected)
                    {
                        e.Value = (Int16)temporaryValues[realPropertyName];
                    }
                    else
                    {
                        value = GetMoreBoxesProperty(typeName, realPropertyName);
                        if (value != null)
                        {
                            e.Value = (Int16)value;
                        }
                    }
                    break;

                case "System.Int64":
                    if (IsOneBoxSelected)
                    {
                        e.Value = (Int64)temporaryValues[realPropertyName];
                    }
                    else
                    {
                        value = GetMoreBoxesProperty(typeName, realPropertyName);
                        if (value != null)
                        {
                            e.Value = (Int64)value;
                        }
                    }
                    break;

                case "System.Single":
                    if (IsOneBoxSelected)
                    {
                        e.Value = (Single)temporaryValues[realPropertyName];
                    }
                    else
                    {
                        value = GetMoreBoxesProperty(typeName, realPropertyName);
                        if (value != null)
                        {
                            e.Value = (Single)value;
                        }
                    }
                    break;

                case "System.Double":
                    if (IsOneBoxSelected)
                    {
                        e.Value = (Double)temporaryValues[realPropertyName];
                    }
                    else
                    {
                        value = GetMoreBoxesProperty(typeName, realPropertyName);
                        if (value != null)
                        {
                            e.Value = (Double)value;
                        }
                    }
                    break;

                case "System.DateTime":
                    if (IsOneBoxSelected)
                    {
                        e.Value = (DateTime)temporaryValues[realPropertyName];
                        //GetCorrectDateType(e);
                    }
                    else
                    {
                        GetCorrectDateTypeMoreBoxes(e);
                    }
                    break;

                case "System.TimeSpan":
                    if (IsOneBoxSelected)
                    {
                        e.Value = (TimeSpan)temporaryValues[realPropertyName];
                    }
                    else
                    {
                        value = GetMoreBoxesProperty(typeName, realPropertyName);
                        if (value != null)
                        {
                            e.Value = (TimeSpan)value;
                        }
                    }
                    break;

                default:
                    //here other types will be treated
                    break;
            }
        }

        /// <summary>
        /// Gets a socket property of the box (not normal property)
        /// </summary>
        /// <param name="e">Parameters of the property to set</param>
        /// <param name="propertyName">Name of the property (socket)</param>
        protected void GetSocketProperty(PropertySpecEventArgs e, string propertyName)
        {
            e.Value = SelectedBox.GetPropertySocking(propertyName);
        }

        /// <summary>
        /// Retrieves normal box type from the ICE type
        /// </summary>
        /// <param name="typeClassIceId">ICE Ferda type</param>
        /// <returns>normal system type identifier</returns>
        protected string GetNormalType(string typeClassIceId)
        {
            switch (typeClassIceId)
            {
                case "::Ferda::Modules::StringT":
                    return "System.String";

                case "::Ferda::Modules::IntT":
                    return "System.Int32";

                case "::Ferda::Modules::BoolT":
                    return "System.Boolean";

                case "::Ferda::Modules::ShortT":
                    return "System.Int16";

                case "::Ferda::Modules::LongT":
                    return "System.Int64";

                case "::Ferda::Modules::FloatT":
                    return "System.Single";

                case "::Ferda::Modules::DoubleT":
                    return "System.Double";

                case "::Ferda::Modules::DateT":
                    return "System.DateTime";

                case "::Ferda::Modules::DateTimeT":
                    return "System.DateTime";

                case "::Ferda::Modules::TimeT":
                    return "System.TimeSpan";

                default:
                    return "";
            }
        }

        /// <summary>
        /// The method sets the correct date (or datetime) property according
        /// to the property name. The DateT and DateTimeT structures of the
        /// ModulesManager are both converted to the DateTime structure. Thus
        /// we must know which function to call (SetPropertyDate or SetPropertyDateTime)
        /// </summary>
        /// <param name="e">Arguments of the propertyBag_GetValue event</param>
        /// <param name="box">Box where to set the property</param>
        protected void SetCorrectDateType(PropertySpecEventArgs e, IBoxModule box)
        {
            string propertyName = GetPropertyName(e.Property.Name, SelectedBox);

            //box.SetPropertyDate(e.Property.Name, (DateTime)e.Value);
            foreach (PropertyInfo pinfo in box.MadeInCreator.Properties)
            {
                if (pinfo.name == propertyName)
                {
                    if (pinfo.typeClassIceId == "::Ferda::Modules::DateTimeT")
                    {
                        box.SetPropertyDateTime(e.Property.Name, (DateTime)e.Value);
                    }
                    if (pinfo.typeClassIceId == "::Ferda::Modules::DateT")
                    {
                        box.SetPropertyDate(e.Property.Name, (DateTime)e.Value);
                    }
                }
            }
        }

        /*
        /// <summary>
        /// The method gets the correct date (or datetime) property according
        /// to the property name. The DateT and DateTimeT structures of the 
        /// ModulesManager are both converted to the DateTime structure. Thus
        /// we must know which function to call (GetPropertyDate or GetPropertyDateTime)
        /// </summary>
        /// <param name="e">Arguments of the propertyBag_GetValue event</param>
        protected void GetCorrectDateType(PropertySpecEventArgs e)
        {
            string propertyName = GetPropertyName(e.Property.Name, SelectedBox);

            foreach (PropertyInfo pinfo in SelectedBox.MadeInCreator.Properties)
            {
                if (pinfo.name == propertyName)
                {
                    if (pinfo.typeClassIceId == "::Ferda::Modules::DateTimeT")
                    {
                        e.Value = SelectedBox.GetPropertyDateTime(propertyName);
                    }
                    if (pinfo.typeClassIceId == "::Ferda::Modules::DateT")
                    {
                        e.Value = SelectedBox.GetPropertyDate(propertyName);
                    }
                    break;
                }
            }
        }
        */

        /// <summary>
        /// The method gets the correct date (or datetime) property according
        /// to the property name for more boxes. The DateT and DateTimeT structures of the 
        /// ModulesManager are both converted to the DateTime structure. Thus
        /// we must know which function to call (GetPropertyDate or GetPropertyDateTime)
        /// </summary>
        /// <param name="e">Arguments of the propertyBag_GetValue event</param>
        protected void GetCorrectDateTypeMoreBoxes(PropertySpecEventArgs e)
        {
            //value that shows the type of the property
            bool dateT = false;
            //determines if there was a property of that name
            bool cycleCheck = false;
            DateTime tempValue;

            string propertyName = GetPropertyName(e.Property.Name, SelectedBox);

            //determining the type of the property in the first box, if it is
            //a DateTimeT, or a DateT structure
            foreach (PropertyInfo pinfo in SelectedBoxes[0].MadeInCreator.Properties)
            {
                if (pinfo.name == propertyName)
                {
                    if (pinfo.typeClassIceId == "::Ferda::Modules::DateTimeT")
                    {
                        dateT = false;
                        cycleCheck = true;
                    }
                    if (pinfo.typeClassIceId == "::Ferda::Modules::DateT")
                    {
                        dateT = true;
                        cycleCheck = true;
                    }
                    break;
                }
            }

            //veryfying with the cycleCheck
            if (!cycleCheck)
            {
                throw new ApplicationException("Wrong properties");
            }

            //getting the first value
            if (dateT)
            {
                tempValue = SelectedBoxes[0].GetPropertyDate(propertyName);
            }
            else
            {
                tempValue = SelectedBoxes[0].GetPropertyDateTime(propertyName);
            }

            //getting the other values and comparing them 
            for (int i = 1; i < SelectedBoxes.Count; i++)
            {
                //it is a DateT value
                if (dateT)
                {
                    if (tempValue == SelectedBoxes[i].GetPropertyDate(propertyName))
                    {
                        continue;
                    }
                    else
                    {
                        return;
                    }
                }
                //it is a DateTimeT value
                else
                {
                    if (tempValue == SelectedBoxes[i].GetPropertyDateTime(propertyName))
                    {
                        continue;
                    }
                    else
                    {
                        return;
                    }
                }
            }

            e.Value = tempValue;
        }

        /*
        /// <summary>
        /// Function returns a <see cref="T:Ferda.FrontEnd.Properties.StringSequence"/>
        /// object generated from the all the selected boxes and the property name
        /// </summary>
        /// <param name="realPropertyName">Name of the property</param>
        /// <returns>A <see cref="T:Ferda.FrontEnd.Properties.StringSequence"/> 
        /// object that contains the
        /// properties options</returns>
        */
        //protected StringSequence GetStringSequenceMoreBoxes(string realPropertyName)
        //{
        //    StringSequence firstSeq = GetStringSequence(SelectedBoxes[0], realPropertyName);
        //    StringSequence otherSeq;

        //    for (int i = 1; i < SelectedBoxes.Count; i++)
        //    {
        //        otherSeq = GetStringSequence(SelectedBoxes[i], realPropertyName);

        //        //if the sequences have different values inside, return null;
        //        //if (!StringSequence.EqualArrays(firstSeq, otherSeq))
        //        //{
        //        //    return null;
        //        //}

        //        //if the sequences have differnent selected string, return null
        //        if (!StringSequence.EqualSelections(firstSeq, otherSeq))
        //        {
        //            return null;
        //        }
        //    }

        //    return firstSeq;
        //}

        /// <summary>
        /// Function returns a OtherProperty object generated from the box
        /// and property
        /// </summary>
        /// <param name="SelectedBox">Box that contains the property</param>
        /// <param name="propertyName">Name of the property</param>
        /// <returns>An OtherProperty object that contains the
        /// properties options</returns>
        protected OtherProperty GetOtherProperty(IBoxModule SelectedBox, string propertyName)
        {
            return new OtherProperty(SelectedBox, propertyName, archiveDisplayer, viewDisplayers,
                this, ResManager);
        }

        /// <summary>
        /// Gets the correct property name from a box and a label of the property (socket)
        /// </summary>
        /// <param name="propertyLabel">Label of the property (actually socket)</param>
        /// <param name="box">Box where this property is located</param>
        /// <returns>Real name of the property</returns>
        protected string GetPropertyName(string propertyLabel, IBoxModule box)
        {
            foreach (PropertyInfo pinfo in box.MadeInCreator.Properties)
            {
                //Tomas Kuchar wrote
                if (!pinfo.visible)
                    continue;

                SocketInfo si = box.MadeInCreator.GetSocket(pinfo.name);
                if (si.label == propertyLabel)
                {
                    return si.name;
                }
            }

            //the property is a connection property, it has the same label as name
            return propertyLabel;
        }

        /// <summary>
        /// This function gets one value from more boxes, that are selected (on the 
        /// desktop)
        /// </summary>
        /// <param name="typeName">name of the type of the propertry, example: 
        /// <example>System.Int16</example>
        /// </param>
        /// <param name="realPropertyName">The name of the property in the Module 
        /// (not the label)
        /// </param>
        /// <returns>object containing the value, null if the values are different
        /// </returns>
        protected object GetMoreBoxesProperty(string typeName, string realPropertyName)
        {
            int i;
            switch (typeName)
            {
                case "System.String":
                    //a variable that holds the value of the first item
                    string tempString =
                        SelectedBoxes[0].GetPropertyString(realPropertyName);

                    //iterating through the other items to see if there is another value
                    for (i = 1; i < SelectedBoxes.Count; i++)
                    {
                        if (tempString == SelectedBoxes[i].GetPropertyString(realPropertyName))
                        {
                            //the value is the same, we can continue
                            continue;
                        }
                        else
                        {
                            //the value is not the same, null should be returned
                            return null;
                        }
                    }
                    return tempString;

                case "System.Int32":
                    Int32 tempInt32 =
                        SelectedBoxes[0].GetPropertyInt(realPropertyName);
                    for (i = 1; i < SelectedBoxes.Count; i++)
                    {
                        if (tempInt32 == SelectedBoxes[i].GetPropertyInt(realPropertyName))
                        {
                            continue;
                        }
                        else
                        {
                            return null;
                        }
                    }
                    return tempInt32;

                case "System.Boolean":
                    bool tempBool =
                        SelectedBoxes[0].GetPropertyBool(realPropertyName);
                    for (i = 1; i < SelectedBoxes.Count; i++)
                    {
                        if (tempBool == SelectedBoxes[i].GetPropertyBool(realPropertyName))
                        {
                            continue;
                        }
                        else
                        {
                            return null;
                        }
                    }
                    return tempBool;

                case "System.Int16":
                    Int16 tempInt16 =
                        SelectedBoxes[0].GetPropertyShort(realPropertyName);
                    for (i = 1; i < SelectedBoxes.Count; i++)
                    {
                        if (tempInt16 == SelectedBoxes[i].GetPropertyShort(realPropertyName))
                        {
                            continue;
                        }
                        else
                        {
                            return null;
                        }
                    }
                    return tempInt16;

                case "System.Int64":
                    Int64 tempInt64 =
                        SelectedBoxes[0].GetPropertyLong(realPropertyName);
                    for (i = 1; i < SelectedBoxes.Count; i++)
                    {
                        if (tempInt64 == SelectedBoxes[i].GetPropertyLong(realPropertyName))
                        {
                            continue;
                        }
                        else
                        {
                            return null;
                        }
                    }
                    return tempInt64;

                case "System.Single":
                    Single tempSingle =
                        SelectedBoxes[0].GetPropertyFloat(realPropertyName);
                    for (i = 1; i < SelectedBoxes.Count; i++)
                    {
                        if (tempSingle == SelectedBoxes[i].GetPropertyFloat(realPropertyName))
                        {
                            continue;
                        }
                        else
                        {
                            return null;
                        }
                    }
                    return tempSingle;

                case "System.Double":
                    Double tempDouble =
                        SelectedBoxes[0].GetPropertyDouble(realPropertyName);
                    for (i = 1; i < SelectedBoxes.Count; i++)
                    {
                        if (tempDouble == SelectedBoxes[i].GetPropertyDouble(realPropertyName))
                        {
                            continue;
                        }
                        else
                        {
                            return null;
                        }
                    }
                    return tempDouble;

                case "System.TimeSpan":
                    TimeSpan tempTimeSpan =
                        SelectedBoxes[0].GetPropertyTime(realPropertyName);
                    for (i = 1; i < SelectedBoxes.Count; i++)
                    {
                        if (tempTimeSpan == SelectedBoxes[i].GetPropertyTime(realPropertyName))
                        {
                            continue;
                        }
                        else
                        {
                            return null;
                        }
                    }
                    return tempTimeSpan;

                case "System.DateTime":
                    //it is treated by separate function
                    break;
            }
            return null;
        }

        #endregion

        #region Other methods

        /// <summary>
        /// Changes the size of the child controls. Archive has to do it 
        /// itself, because DockDotNET doesnt support these kinds of events
        /// </summary>
        public void ChangeSize()
        {
            if (Parent != null)
            {
                this.Size = new System.Drawing.Size(
                    Parent.Size.Width - 5, Parent.Size.Height - 20);
            }
        }

        /// <summary>
        /// Returns real type name of the property 
        /// (without the assembly information)
        /// </summary>
        /// <param name="assemblyTypeName">type name from the assembly</param>
        /// <returns>real type name</returns>
        public string RealTypeName(string assemblyTypeName)
        {
            //parsing the string to get the type name without assembly information
            int firstComma = assemblyTypeName.IndexOf(',');
            string typeName;

            if (firstComma == -1)
            {
                typeName = assemblyTypeName;
            }
            else
            {
                typeName = assemblyTypeName.Substring(0, firstComma);
            }

            return typeName;
        }

        #endregion

        #endregion

        #region Events

        /// <summary>
        /// Event when the property grid receives focus. It forces the menu
        /// to adapt
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event parameters</param>
        void FerdaPropertyGrid_Enter(object sender, EventArgs e)
        {
            menuDisplayer.ControlHasFocus = this;
            menuDisplayer.Adapt();
            toolBar.ControlHasFocus = this;
            toolBar.Adapt();
        }

        /// <summary>
        /// Gets the property value for one or more selected boxes
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event parameters</param>
        /// <remarks>
        /// Tady se to musi jeste doplnit o vsechny typy, ktere tam ma definovane Michal
        /// + pridelat neco pro custom typy (zatim nevim jak presne se to bude delat)
        /// </remarks>
        void propertyBag_GetValue(object sender, PropertySpecEventArgs e)
        {
            //Cursor c = Cursor;
            //Parent.Cursor = Cursors.WaitCursor;

            //getting the real name of the property, not the label
            string realPropertyName = "";

            if (IsOneBoxSelected)
            {
                realPropertyName = GetPropertyName(e.Property.Name, SelectedBox);
            }
            else
            {
                realPropertyName = GetPropertyName(e.Property.Name, SelectedBoxes[0]);
            }

            string typeName = RealTypeName(e.Property.TypeName);

            FerdaPropertySpec pfs = e.Property as FerdaPropertySpec;

            if (pfs.SocketProperty)
            {
                GetSocketProperty(e, realPropertyName);
            }
            else
            {
                GetNormalProperty(e, realPropertyName, typeName);
            }
            //Parent.Cursor = c;
        }

        /// <summary>
        /// Sets the property value for one or more selected boxes
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event parameters</param>
        void propertyBag_SetValue(object sender, PropertySpecEventArgs e)
        {
            //getting the real name of the property, not the label
            string propertyName = String.Empty;

            if (IsOneBoxSelected)
            {
                propertyName = GetPropertyName(e.Property.Name, SelectedBox);
            }
            else
            {
                propertyName = GetPropertyName(e.Property.Name, SelectedBoxes[0]);
            }

            //parsing the string to get the type name without assembly information
            int firstComma = e.Property.TypeName.IndexOf(',');
            string typeName;

            if (firstComma == -1)
            {
                typeName = e.Property.TypeName;
            }
            else
            {
                typeName = e.Property.TypeName.Substring(0, firstComma);
            }

            FerdaPropertySpec pfs = e.Property as FerdaPropertySpec;
            //this determines, if a socket was changed
            bool changedSocket = false;

            if (pfs.SocketProperty)
            {
                changedSocket = SetSocketProperty(e, propertyName);
            }
            else
            {
                SetNormalProperty(e, propertyName, typeName);
            }

            archiveDisplayer.RefreshBoxNames();
            foreach (IViewDisplayer view in ViewDisplayers)
            {
                view.RefreshBoxNames();
                //for optimalization purposes we redraw the view only if
                //there was a socket changed
                if (changedSocket)
                {
                    view.Adapt();
                }
            }
        }

        #endregion
    }
}
