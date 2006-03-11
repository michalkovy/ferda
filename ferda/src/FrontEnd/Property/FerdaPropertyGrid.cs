// FerdaPropertyGrid.cs - property grid for the Ferda application
//
// Author: Martin Ralbovský <martin.ralbovsky@gmail.com>
//
// Copyright (c) 2005 Martin Ralbovský
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
    /// It is used for asynchronous property getting, when there are more boxes 
    /// selected. It determines the state of the property getting.
    /// </summary>
    public enum EMoreBoxesTemporaryValueState
    {
        /// <summary>
        /// No property value has returned from asynchronous calling, the 
        /// value of the property is set to initial value
        /// </summary>
        First,
        /// <summary>
        /// One (or more) properties have returned and they have the same value
        /// </summary>
        Right,
        /// <summary>
        /// The properties returning from asychnronous calling are different, 
        /// therefore the right behaviour is to set the property value back
        /// to initial value
        /// </summary>
        Wrong
    }

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

        delegate void RefreshDelegate(PropertyTable bag);

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
        private Int32 clickID = 0;

        /// <summary>
        /// The property bag where values of the property are stored
        /// </summary>
        protected External.PropertyTable propertyBag;

        /// <summary>
        /// Structure where temporary values of the properties are stored
        /// </summary>
        protected Dictionary<string, object> temporaryValues;
        /// <summary>
        /// A dictionary to get a type of the property from the 
        /// </summary>
        protected Dictionary<string, string> temporaryPropertyTypes;
        /// <summary>
        /// When there are more boxes selected, this dictionary stores
        /// information if there is the initial value written in the
        /// temporaryValues dictionary, or some box already wrote something
        /// there
        /// </summary>
        protected Dictionary<string, EMoreBoxesTemporaryValueState> moreBoxesValuesState;
        /// <summary>
        /// Locker for temporary values
        /// </summary>
        private object tempLocker = new object();

        #endregion

        #region Propetries
        
        /// <summary>
        /// Identifier of the box which properties are beeing shown
        /// at the moment. It serves the asynchronous property getting.
        /// </summary>
        public Int32 ClickID
        {
            set
            {
                lock (tempLocker)
                {
                    clickID = value;
                }
            }
            get
            {
                lock (tempLocker)
                {
                    return clickID;
                }
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
            if (IsDisposed)
            {
                return;
            }

            temporaryValues = new Dictionary<string, object>();
            temporaryPropertyTypes = new Dictionary<string, string>();
            moreBoxesValuesState =
                new Dictionary<string, EMoreBoxesTemporaryValueState>();

            if (IsOneBoxSelected) //creating properties for 1 box only
            {
                PropertyTable prop = CreatePropertiesFromBox(SelectedBox);
                propertyBag = prop;
                CreateAsyncCatchersOneBox();
                AddSocketProperties(propertyBag, SelectedBox);

                this.SelectedObject = propertyBag;
            }
            else //creating it for more selected boxes
            {
                PropertyTable prop = CreatePropertiesFromMoreBoxes(SelectedBoxes);
                propertyBag = prop;
                CreateAsyncCatchersMoreBoxes();
                this.SelectedObject = propertyBag;
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
            if (IsDisposed)
            {
                return;
            }

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
            if (IsDisposed)
            {
                return;
            }
            this.SelectedObject = objectProperties;
        }

        #endregion

        #region IAsyncPropertyManager implementation + related functions

        /// <summary>
        /// Increases the number of clicks in on the desktop
        /// </summary>
        /// <returns>Number of clicks on the desktop</returns>
        public int IncreaseClickID()
        {
            int tmp;
            lock (this)
            {
                tmp = ++clickID;
            }

            return tmp;
        }

        /// <summary>
        /// Add the temporary values into the temporaryValues dictionary and to the
        /// temporaryPropertyType dictionary
        /// </summary>
        /// <param name="normalType">Type of the property</param>
        /// <param name="propertyName">Name of the property (not the localized label)
        /// </param>
        /// <param name="moreBoxes">We are creating for one ore more boxes</param>
        protected void AddAsyncTemporary(string propertyName, string normalType, bool moreBoxes)
        {
            lock (tempLocker)
            {
                if (temporaryValues.ContainsKey(propertyName))
                {
                    if (!temporaryPropertyTypes.ContainsKey(normalType))
                    {
                        temporaryPropertyTypes.Add(propertyName, normalType);
                    }
                    return;
                }
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

                temporaryPropertyTypes.Add(propertyName, normalType);
                if (moreBoxes)
                {
                    moreBoxesValuesState.Add(propertyName,
                        EMoreBoxesTemporaryValueState.First);
                }
            }
        }

        /// <summary>
        /// The property value is changed, so the the propertyGrid should be
        /// refilled with new values
        /// </summary>
        /// <param name="catcher">Catcher of the connection</param>
        /// <param name="value">New value of the property</param>
        /// <param name="moreBoxes">If the refresh of the property grid is 
        /// from one or more boxes</param>
        public void ChangedPropertyValue(AsyncPropertyCatcher catcher, object value, bool moreBoxes)
        {
            //changing one of the temporary values
            //writing to the temporaryValues dictionary
            switch (catcher.PropertyType)
            {
                case "System.Int32":
                    Int32 int32Value = ((IntT)value).intValue;
                    lock (tempLocker)
                    {
                        if (moreBoxes)
                        {
                            switch (moreBoxesValuesState[catcher.PropertyName])
                            {
                                //no box has written to the property
                                case EMoreBoxesTemporaryValueState.First:
                                    temporaryValues[catcher.PropertyName] = int32Value;
                                    moreBoxesValuesState[catcher.PropertyName] =
                                        EMoreBoxesTemporaryValueState.Right;
                                    break;

                                //all the previos boxes wrote the same value
                                case EMoreBoxesTemporaryValueState.Right:
                                    if (int32Value != (Int32)temporaryValues[catcher.PropertyName])
                                    {
                                        temporaryValues[catcher.PropertyName] = (Int32)0;
                                        moreBoxesValuesState[catcher.PropertyName] = 
                                            EMoreBoxesTemporaryValueState.Wrong;
                                    }
                                    break;
                                //values from the boxes are different
                                case EMoreBoxesTemporaryValueState.Wrong:
                                    break;
                            }
                        }
                        else
                        {
                            temporaryValues[catcher.PropertyName] = int32Value;
                        }
                    }
                    break;

                case "Ferda.FrontEnd.Properties.OtherProperty":
                    //here is no moreBoxes here
                    string resultValue = SelectedBox.GetPropertyOtherAboutFromValue(catcher.PropertyName,
                        (PropertyValue)value);
                    OtherProperty prop = new OtherProperty(SelectedBox, catcher.PropertyName,
                        archiveDisplayer, viewDisplayers, this, ResManager);
                    prop.Result = resultValue;
                    lock (tempLocker)
                    {
                        temporaryValues[catcher.PropertyName] = prop;
                    }
                    break;


                case "Ferda.FrontEnd.Properties.StringSequence":
                    string selectedValue = ((StringT)value).stringValue;
                    //creating a new stringSequence
                    StringSequence seq = new StringSequence(catcher.PropertyName,
                        new IBoxModule[] { SelectedBox },
                        ResManager, ArchiveDisplayer, ViewDisplayers,
                        this, selectedValue);
                    lock (tempLocker)
                    {
                        temporaryValues[catcher.PropertyName] = seq;
                    }
                    break;

                case "System.String":
                    string valueString = ((StringT)value).stringValue;
                    lock (tempLocker)
                    {
                        if (moreBoxes)
                        {
                            switch (moreBoxesValuesState[catcher.PropertyName])
                            {
                                //no box has written to the property
                                case EMoreBoxesTemporaryValueState.First:
                                    temporaryValues[catcher.PropertyName] = valueString;
                                    moreBoxesValuesState[catcher.PropertyName] =
                                        EMoreBoxesTemporaryValueState.Right;
                                    break;

                                //all the previos boxes wrote the same value
                                case EMoreBoxesTemporaryValueState.Right:
                                    if (valueString != (string)temporaryValues[catcher.PropertyName])
                                    {
                                        temporaryValues[catcher.PropertyName] = string.Empty;
                                        moreBoxesValuesState[catcher.PropertyName] =
                                            EMoreBoxesTemporaryValueState.Wrong;
                                    }
                                    break;
                                //values from the boxes are different
                                case EMoreBoxesTemporaryValueState.Wrong:
                                    break;
                            }
                        }
                        else
                        {
                            temporaryValues[catcher.PropertyName] = valueString;
                        }
                    }
                    break;

                case "System.Boolean":
                    bool boolValue = ((BoolT)value).boolValue;
                    lock (tempLocker)
                    {
                        if (moreBoxes)
                        {
                            switch (moreBoxesValuesState[catcher.PropertyName])
                            {
                                //no box has written to the property
                                case EMoreBoxesTemporaryValueState.First:
                                    temporaryValues[catcher.PropertyName] = boolValue;
                                    moreBoxesValuesState[catcher.PropertyName] =
                                        EMoreBoxesTemporaryValueState.Right;
                                    break;

                                //all the previos boxes wrote the same value
                                case EMoreBoxesTemporaryValueState.Right:
                                    if (boolValue != (bool)temporaryValues[catcher.PropertyName])
                                    {
                                        temporaryValues[catcher.PropertyName] = false;
                                        moreBoxesValuesState[catcher.PropertyName] =
                                            EMoreBoxesTemporaryValueState.Wrong;
                                    }
                                    break;
                                //values from the boxes are different
                                case EMoreBoxesTemporaryValueState.Wrong:
                                    break;
                            }
                        }
                        else
                        {
                            temporaryValues[catcher.PropertyName] = boolValue;
                        }
                    }
                    break;

                case "System.Int16":
                    Int16 shortValue = ((ShortT)value).shortValue;
                    lock (tempLocker)
                    {
                        if (moreBoxes)
                        {
                            switch (moreBoxesValuesState[catcher.PropertyName])
                            {
                                //no box has written to the property
                                case EMoreBoxesTemporaryValueState.First:
                                    temporaryValues[catcher.PropertyName] = shortValue;
                                    moreBoxesValuesState[catcher.PropertyName] =
                                        EMoreBoxesTemporaryValueState.Right;
                                    break;

                                //all the previos boxes wrote the same value
                                case EMoreBoxesTemporaryValueState.Right:
                                    if (shortValue != (Int16)temporaryValues[catcher.PropertyName])
                                    {
                                        temporaryValues[catcher.PropertyName] = (Int16)0;
                                        moreBoxesValuesState[catcher.PropertyName] =
                                            EMoreBoxesTemporaryValueState.Wrong;
                                    }
                                    break;
                                //values from the boxes are different
                                case EMoreBoxesTemporaryValueState.Wrong:
                                    break;
                            }
                        }
                        else
                        {
                            temporaryValues[catcher.PropertyName] = shortValue;
                        }
                    }
                    break;

                case "System.Int64":
                    Int64 longValue = ((LongT)value).longValue;
                    lock (tempLocker)
                    {
                        if (moreBoxes)
                        {
                            switch (moreBoxesValuesState[catcher.PropertyName])
                            {
                                //no box has written to the property
                                case EMoreBoxesTemporaryValueState.First:
                                    temporaryValues[catcher.PropertyName] = longValue;
                                    moreBoxesValuesState[catcher.PropertyName] =
                                        EMoreBoxesTemporaryValueState.Right;
                                    break;

                                //all the previos boxes wrote the same value
                                case EMoreBoxesTemporaryValueState.Right:
                                    if (longValue != (Int64)temporaryValues[catcher.PropertyName])
                                    {
                                        temporaryValues[catcher.PropertyName] = (Int64)0;
                                        moreBoxesValuesState[catcher.PropertyName] =
                                            EMoreBoxesTemporaryValueState.Wrong;
                                    }
                                    break;
                                //values from the boxes are different
                                case EMoreBoxesTemporaryValueState.Wrong:
                                    break;
                            }
                        }
                        else
                        {
                            temporaryValues[catcher.PropertyName] = longValue;
                        }
                    }
                    break;

                case "System.Double":
                    Double doubleValue = ((DoubleT)value).doubleValue;
                    lock (tempLocker)
                    {
                        if (moreBoxes)
                        {
                            switch (moreBoxesValuesState[catcher.PropertyName])
                            {
                                //no box has written to the property
                                case EMoreBoxesTemporaryValueState.First:
                                    temporaryValues[catcher.PropertyName] = doubleValue;
                                    moreBoxesValuesState[catcher.PropertyName] =
                                        EMoreBoxesTemporaryValueState.Right;
                                    break;

                                //all the previos boxes wrote the same value
                                case EMoreBoxesTemporaryValueState.Right:
                                    if (doubleValue != (Double)temporaryValues[catcher.PropertyName])
                                    {
                                        temporaryValues[catcher.PropertyName] = (Double)0;
                                        moreBoxesValuesState[catcher.PropertyName] =
                                            EMoreBoxesTemporaryValueState.Wrong;
                                    }
                                    break;
                                //values from the boxes are different
                                case EMoreBoxesTemporaryValueState.Wrong:
                                    break;
                            }
                        }
                        else
                        {
                            temporaryValues[catcher.PropertyName] = doubleValue;
                        }
                    }
                    break;

                case "System.Single":
                    Single singleValue = ((FloatT)value).floatValue;
                    lock (tempLocker)
                    {
                        if (moreBoxes)
                        {
                            switch (moreBoxesValuesState[catcher.PropertyName])
                            {
                                //no box has written to the property
                                case EMoreBoxesTemporaryValueState.First:
                                    temporaryValues[catcher.PropertyName] = singleValue;
                                    moreBoxesValuesState[catcher.PropertyName] =
                                        EMoreBoxesTemporaryValueState.Right;
                                    break;

                                //all the previos boxes wrote the same value
                                case EMoreBoxesTemporaryValueState.Right:
                                    if (singleValue != (Single)temporaryValues[catcher.PropertyName])
                                    {
                                        temporaryValues[catcher.PropertyName] = (Single)0;
                                        moreBoxesValuesState[catcher.PropertyName] =
                                            EMoreBoxesTemporaryValueState.Wrong;
                                    }
                                    break;
                                //values from the boxes are different
                                case EMoreBoxesTemporaryValueState.Wrong:
                                    break;
                            }
                        }
                        else
                        {
                            temporaryValues[catcher.PropertyName] = singleValue;
                        }
                    }
                    break;

                case "System.DateTime":
                    //a date property is treated differently due to unexisting conversion
                    if (value is DateT)
                    {
                        DateT v = (DateT)value;
                        DateTime dateTimeValue = new DateTime();
                        try
                        {
                            dateTimeValue = new DateTime(v.year, v.month, v.day);
                        }
                        catch (ArgumentOutOfRangeException)
                        {
                        }

                        lock (tempLocker)
                        {
                            if (moreBoxes)
                            {
                                switch (moreBoxesValuesState[catcher.PropertyName])
                                {
                                    //no box has written to the property
                                    case EMoreBoxesTemporaryValueState.First:
                                        temporaryValues[catcher.PropertyName] = dateTimeValue;
                                        moreBoxesValuesState[catcher.PropertyName] =
                                            EMoreBoxesTemporaryValueState.Right;
                                        break;

                                    //all the previos boxes wrote the same value
                                    case EMoreBoxesTemporaryValueState.Right:
                                        if (dateTimeValue != (DateTime)temporaryValues[catcher.PropertyName])
                                        {
                                            temporaryValues[catcher.PropertyName] = new DateTime();
                                            moreBoxesValuesState[catcher.PropertyName] =
                                                EMoreBoxesTemporaryValueState.Wrong;
                                        }
                                        break;
                                    //values from the boxes are different
                                    case EMoreBoxesTemporaryValueState.Wrong:
                                        break;
                                }
                            }
                            else
                            {
                                temporaryValues[catcher.PropertyName] = dateTimeValue;
                            }
                        }
                    }
                    else //it is a DateTimeT thing
                    {
                        DateTimeT v = (DateTimeT)value;
                        
                        //the value does not have to be correct
                        DateTime dtValue = new DateTime();
                        try
                        {

                            dtValue = new DateTime(v.year,
                                    v.month, v.day, v.hour, v.minute, v.second);
                        }
                        catch (ArgumentOutOfRangeException)
                        {
                        }

                        lock (tempLocker)
                        {
                            if (moreBoxes)
                            {
                                switch (moreBoxesValuesState[catcher.PropertyName])
                                {
                                    //no box has written to the property
                                    case EMoreBoxesTemporaryValueState.First:
                                        temporaryValues[catcher.PropertyName] = dtValue;
                                        moreBoxesValuesState[catcher.PropertyName] =
                                            EMoreBoxesTemporaryValueState.Right;
                                        break;

                                    //all the previos boxes wrote the same value
                                    case EMoreBoxesTemporaryValueState.Right:
                                        if (dtValue != (DateTime)temporaryValues[catcher.PropertyName])
                                        {
                                            temporaryValues[catcher.PropertyName] = new DateTime();
                                            moreBoxesValuesState[catcher.PropertyName] =
                                                EMoreBoxesTemporaryValueState.Wrong;
                                        }
                                        break;
                                    //values from the boxes are different
                                    case EMoreBoxesTemporaryValueState.Wrong:
                                        break;
                                }
                            }
                            else
                            {
                                temporaryValues[catcher.PropertyName] = dtValue;
                            }
                        }
                    }
                    break;

                case "System.TimeSpan":
                    TimeT val = (TimeT)value;
                    TimeSpan timeSpanValue = new TimeSpan(val.hour, val.minute, val.second);
                    lock (tempLocker)
                    {
                        if (moreBoxes)
                        {
                            switch (moreBoxesValuesState[catcher.PropertyName])
                            {
                                //no box has written to the property
                                case EMoreBoxesTemporaryValueState.First:
                                    temporaryValues[catcher.PropertyName] = timeSpanValue;
                                    moreBoxesValuesState[catcher.PropertyName] =
                                        EMoreBoxesTemporaryValueState.Right;
                                    break;

                                //all the previos boxes wrote the same value
                                case EMoreBoxesTemporaryValueState.Right:
                                    if (timeSpanValue != (TimeSpan)temporaryValues[catcher.PropertyName])
                                    {
                                        temporaryValues[catcher.PropertyName] = new TimeSpan();
                                        moreBoxesValuesState[catcher.PropertyName] =
                                            EMoreBoxesTemporaryValueState.Wrong;
                                    }
                                    break;
                                //values from the boxes are different
                                case EMoreBoxesTemporaryValueState.Wrong:
                                    break;
                            }
                        }
                        else
                        {
                            temporaryValues[catcher.PropertyName] = timeSpanValue;
                        }
                    }
                    break;

                default:
                    break;
            }

            //refilling the propertyGrid
            SetPropertyGrid(propertyBag);
        }

        /// <summary>
        /// Creates asynchronous catchers for getting properties out of the ICE layer
        /// </summary>
        protected void CreateAsyncCatchersOneBox()
        {
            lock (tempLocker)
            {
                foreach (string propertyName in temporaryValues.Keys)
                {
                    AsyncPropertyCatcher catcher = new AsyncPropertyCatcher(this,
                        propertyName, temporaryPropertyTypes[propertyName]);
                    SelectedBox.GetProperty_async(catcher, propertyName);
                }
            }
        }

        /// <summary>
        /// Creates asynchronous catchers for getting properties out of the ICE layer
        /// when more than one box is selected on the desktop
        /// </summary>
        protected void CreateAsyncCatchersMoreBoxes()
        {
            lock (tempLocker)
            {
                foreach (string propertyName in temporaryValues.Keys)
                {
                    foreach (IBoxModule box in SelectedBoxes)
                    {
                        //tady byl problem s tim, ze to nemohlo najit jmeno
                        AsyncPropertyCatcher catcher = new AsyncPropertyCatcher(this,
                            propertyName, temporaryPropertyTypes[propertyName], true);
                        box.GetProperty_async(catcher, propertyName);
                    }
                }
            }
        }

        /// <summary>
        /// A special method to refresh the property grid with a new property bag,
        /// that can be called from other threads than the owner thread.
        /// </summary>
        /// <param name="bag">A bag with the right values of the properties</param>
        private void SetPropertyGrid(PropertyTable bag)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.InvokeRequired)
            {
                RefreshDelegate d = new RefreshDelegate(SetPropertyGrid);
                this.Invoke(d, new object[] { bag });
            }
            else
            {
                this.SelectedObject = bag;
            }
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
            //initializing a new bag
            PropertyTable bag = new PropertyTable();
            //increasing the number of clicks of the user on the desktop
            bag.ClickID = IncreaseClickID();
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
                                AddAsyncTemporary(pinfo.name, normalType, false);
                            }
                            else
                            {
                                //throw new ApplicationException("Wierd type that we dont know!!!");
                            }
                        }
                    }
                }
            }
            return bag;
        }

        /// <summary>
        /// Adds the common properties of the selected boxes on the desktop
        /// to the <see cref="T:Ferda.FrontEnd.External.PropertyTable"/> 
        /// object. Common properties mean that all the boxes have the property
        /// of that name and the same type.
        /// </summary>
        /// <param name="boxes">The boxes from which the properties should 
        /// be created</param>
        /// <returns>A <see cref="T:Ferda.FrontEnd.External.PropertyTable"/>
        /// object filled with common properties</returns>
        protected PropertyTable CreatePropertiesFromMoreBoxes(IList<IBoxModule> boxes)
        {
            //structure where the common properties will be stored
            List<PropertyInfo> commonProperties = new List<PropertyInfo>();

            //initializing a new bag
            PropertyTable bag = new PropertyTable();
            bag.GetValue += new PropertySpecEventHandler(propertyBag_GetValue);
            bag.SetValue += new PropertySpecEventHandler(propertyBag_SetValue);
            //increasing the number of clicks of the user on the desktop
            bag.ClickID = IncreaseClickID();

            //first creator - properties from this creator will be 
            //compared to creators from other boxes
            IBoxModuleFactoryCreator firstCreator = boxes[0].MadeInCreator;

            //iterating throutg all the properties of the first selected box
            foreach (PropertyInfo info in firstCreator.Properties)
            {
                bool contains = true;

                //all the boxes should include this property
                for (int i = 1; i < boxes.Count; i++)
                {
                    contains = ContainsPropertyInfo(boxes[i].MadeInCreator, info);
                    if (!contains)
                    {
                        break;
                    }
                }

                if (contains)
                {
                    commonProperties.Add(info);
                }
            }

            //iterating through all the common properties
            foreach (PropertyInfo pinfo in commonProperties)
            {
                FerdaPropertySpec ps;
                if (pinfo.visible)
                {
                    //two known other property types - StringSeqT and CategoriesT
                    if (pinfo.typeClassIceId == "::Ferda::Modules::StringSeqT" ||
                        pinfo.typeClassIceId == "::Ferda::Modules::CategoriesT")
                    {
                        //CreateOtherProperty(pinfo, box, bag);
                        //continue;
                    }

                    //strings are also dealt with separatelly
                    if (pinfo.typeClassIceId == "::Ferda::Modules::StringT")
                    {
                        //deterimining if we can use a module to set the property
                        //bool canBeSetWithModule = true;
                        //foreach (IBoxModule box in boxes)
                        //{
                        //    canBeSetWithModule = 
                        //        box.IsPropertySetWithSettingModule(pinfo.name);
                        //}
                        ////we can set the property with a module (ODBC Connection string)
                        //if (canBeSetWithModule)
                        //{
                        //    string normalType = GetNormalType(pinfo.typeClassIceId);
                        //    if (normalType != "")
                        //    {
                        //        //getting the displayable name of the property
                        //        SocketInfo si = firstCreator.GetSocket(pinfo.name);
                        //        ps = new FerdaPropertySpec(si.label, normalType, false);
                        //        ps.Category = pinfo.categoryName;
                        //        //getting the hint
                        //        ps.Description = si.hint;

                        //        //setting the attributes of the property
                        //        if (IsPropertyReadOnlyMoreBoxes(boxes, pinfo.name) ||
                        //            IsPropertySockedMoreBoxes(boxes, pinfo.name))
                        //        {
                        //            ps.Attributes = new Attribute[]
                        //            {
                        //                ReadOnlyAttribute.Yes,
                        //                new TypeConverterAttribute(typeof(OtherPropertyAddingConverter)), 
                        //                new EditorAttribute(typeof(OtherPropertyEditor), 
                        //                typeof(System.Drawing.Design.UITypeEditor))
                        //            };
                        //        }
                        //        else
                        //        {
                        //            ps.Attributes = new Attribute[]
                        //            {
                        //                new TypeConverterAttribute(typeof(OtherPropertyAddingConverter)),
                        //                new EditorAttribute(typeof(OtherPropertyEditor), 
                        //                typeof(System.Drawing.Design.UITypeEditor))
                        //            };
                        //        }

                        //        //getting the property to the bag
                        //        bag.Properties.Add(ps);
                        //        //adding the asynchronous stuff
                        //        AddAsyncTemporary(pinfo.name, "Ferda.FrontEnd.Properties.OtherProperty", true);
                        //    }
                        //}
                        //else
                        {
                            //it is a normal string property
                            CreateStringProperty(pinfo, boxes[0], bag);
                        }
                    }
                    else
                    {
                        string normalType = GetNormalType(pinfo.typeClassIceId);

                        //This is a normal type, creating a normal property for it
                        if (normalType != "")
                        {
                            //getting the displayable name of the property
                            SocketInfo si = firstCreator.GetSocket(pinfo.name);
                            ps = new FerdaPropertySpec(si.label, normalType, false);
                            ps.Category = pinfo.categoryName;

                            //geting the socket information about the category
                            ps.Description = si.hint;

                            //it is readonly or it is already there as a socket -
                            //cannot edit "socketed" value
                            if (IsPropertyReadOnlyMoreBoxes(boxes, pinfo.name) ||
                                IsPropertySockedMoreBoxes(boxes, pinfo.name))
                            {
                                ps.Attributes = new Attribute[]
                                {
                                   ReadOnlyAttribute.Yes
                                };
                            }

                            bag.Properties.Add(ps);

                            //adding the asynchronous stuff
                            AddAsyncTemporary(pinfo.name, normalType, true);
                        }
                        else
                        {
                            throw new ApplicationException("Wierd type that we dont know!!!");
                        }
                    }
                }
            }
            return bag;
        }

        /// <summary>
        /// Determines if the property is socked in more boxes (it is taken out to the
        /// desktop as a socket). The rule is that if there is one box that has this 
        /// property socked, then it is socked in all the boxes.
        /// </summary>
        /// <param name="boxes">Boxes with the property</param>
        /// <param name="propertyName">Name of the property</param>
        /// <returns>If the property is socked in all the boxes</returns>
        protected static bool IsPropertySockedMoreBoxes(IList<IBoxModule> boxes, string propertyName)
        {
            foreach (IBoxModule box in boxes)
            {
                if (box.GetPropertySocking(propertyName))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Determines if the property is readonly in more boxes. The rule is that
        /// if there is one box that has this property as a readonly property,
        /// it will be readonly in all the boxes.
        /// </summary>
        /// <param name="boxes">Boxes with the property</param>
        /// <param name="propertyName">Name of the property</param>
        /// <returns>If the property is readonly in all the boxes</returns>
        protected static bool IsPropertyReadOnlyMoreBoxes(IList<IBoxModule> boxes, string propertyName)
        {
            foreach (IBoxModule box in boxes)
            {
                if (box.IsPropertyReadOnly(propertyName))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Determines if the creator in the first argument contains the property in
        /// the second argument. It is tested to the same property name and property
        /// type
        /// </summary>
        /// <param name="iBoxModuleFactoryCreator">A creator with properties</param>
        /// <param name="property">A particular property</param>
        /// <returns>True if contains, false otherwise</returns>
        protected static bool ContainsPropertyInfo(IBoxModuleFactoryCreator iBoxModuleFactoryCreator, PropertyInfo property)
        {
            foreach (PropertyInfo info in iBoxModuleFactoryCreator.Properties)
            {
                if ((info.name == property.name) &&
                    (info.typeClassIceId == property.typeClassIceId))
                {
                    return true;
                }
            }

            return false;
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
                AddAsyncTemporary(pinfo.name, "System.String", !IsOneBoxSelected);
            }
            else //a combo-box should be used
            {
                //if there are more boxes selected, the property should not be added
                if (!IsOneBoxSelected)
                {
                    return;
                }

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
                    AddAsyncTemporary(pinfo.name,
                        "Ferda.FrontEnd.Properties.StringSequence", false);
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
            if (IsOneBoxSelected)
            {
                AddAsyncTemporary(pinfo.name, "Ferda.FrontEnd.Properties.OtherProperty", false);
            }
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
                        bool couldWrite = true;
                        foreach (IBoxModule box in SelectedBoxes)
                        {
                            if (box.TryWriteEnter())
                            {
                                box.SetPropertyInt(propertyName, (int)e.Value);
                                box.WriteExit();
                            }
                            else
                            {
                                couldWrite = false;
                            }
                        }
                        if (!couldWrite)
                        {
                            FrontEndCommon.CannotSetPropertyMoreBoxes(ResManager);
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
                        bool couldWrite = true;
                        foreach (IBoxModule box in SelectedBoxes)
                        {
                            if (box.TryWriteEnter())
                            {
                                box.SetPropertyString(propertyName, (string)e.Value);
                                box.WriteExit();
                            }
                            else
                            {
                                couldWrite = false;
                            }
                        }
                        if (!couldWrite)
                        {
                            FrontEndCommon.CannotSetPropertyMoreBoxes(ResManager);
                        }
                    }
                    break;

                case "System.Boolean":
                    if (IsOneBoxSelected)
                    {
                        if (SelectedBox.TryWriteEnter())
                        {
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
                        bool couldWrite = true;
                        foreach (IBoxModule box in SelectedBoxes)
                        {
                            if (box.TryWriteEnter())
                            {
                                box.SetPropertyBool(propertyName, (bool)e.Value);
                                box.WriteExit();
                            }
                            else
                            {
                                couldWrite = false;
                            }
                        }
                        if (!couldWrite)
                        {
                            FrontEndCommon.CannotSetPropertyMoreBoxes(ResManager);
                        }
                    }
                    break;

                case "System.Int16":
                    if (IsOneBoxSelected)
                    {
                        if (SelectedBox.TryWriteEnter())
                        {
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
                        bool couldWrite = true;
                        foreach (IBoxModule box in SelectedBoxes)
                        {
                            if (box.TryWriteEnter())
                            {
                                box.SetPropertyShort(propertyName, (Int16)e.Value);
                                box.WriteExit();
                            }
                            else
                            {
                                couldWrite = false;
                            }
                        }
                        if (!couldWrite)
                        {
                            FrontEndCommon.CannotSetPropertyMoreBoxes(ResManager);
                        }
                    }
                    break;

                case "System.Int64":
                    if (IsOneBoxSelected)
                    {
                        if (SelectedBox.TryWriteEnter())
                        {
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
                        bool couldWrite = true;
                        foreach (IBoxModule box in SelectedBoxes)
                        {
                            if (box.TryWriteEnter())
                            {
                                box.SetPropertyLong(propertyName, (Int64)e.Value);
                                box.WriteExit();
                            }
                            else
                            {
                                couldWrite = false;
                            }
                        }
                        if (!couldWrite)
                        {
                            FrontEndCommon.CannotSetPropertyMoreBoxes(ResManager);
                        }
                    }
                    break;

                case "System.Double":
                    if (IsOneBoxSelected)
                    {
                        if (SelectedBox.TryWriteEnter())
                        {
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
                        bool couldWrite = true;
                        foreach (IBoxModule box in SelectedBoxes)
                        {
                            if (box.TryWriteEnter())
                            {
                                box.SetPropertyDouble(propertyName, (Double)e.Value);
                                box.WriteExit();
                            }
                            else
                            {
                                couldWrite = false;
                            }
                        }
                        if (!couldWrite)
                        {
                            FrontEndCommon.CannotSetPropertyMoreBoxes(ResManager);
                        }
                    }
                    break;

                case "System.Single":
                    if (IsOneBoxSelected)
                    {
                        if (SelectedBox.TryWriteEnter())
                        {
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
                        bool couldWrite = true;
                        foreach (IBoxModule box in SelectedBoxes)
                        {
                            if (box.TryWriteEnter())
                            {
                                box.SetPropertyFloat(propertyName, (float)e.Value);
                                box.WriteExit();
                            }
                            else
                            {
                                couldWrite = false;
                            }
                        }
                        if (!couldWrite)
                        {
                            FrontEndCommon.CannotSetPropertyMoreBoxes(ResManager);
                        }
                    }
                    break;

                case "System.DateTime":
                    if (IsOneBoxSelected)
                    {
                        if (SelectedBox.TryWriteEnter())
                        {
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
                        bool couldWrite = true;
                        foreach (IBoxModule box in SelectedBoxes)
                        {
                            if (box.TryWriteEnter())
                            {
                                SetCorrectDateType(e, box);
                                box.WriteExit();
                            }
                            else
                            {
                                couldWrite = false;
                            }
                        }
                        if (!couldWrite)
                        {
                            FrontEndCommon.CannotSetPropertyMoreBoxes(ResManager);
                        }
                    }
                    break;

                case "System.TimeSpan":
                    if (IsOneBoxSelected)
                    {
                        if (SelectedBox.TryWriteEnter())
                        {
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
                        bool couldWrite = true;
                        foreach (IBoxModule box in SelectedBoxes)
                        {
                            if (box.TryWriteEnter())
                            {
                                box.SetPropertyTime(propertyName, (TimeSpan)e.Value);
                                box.WriteExit();
                            }
                            else
                            {
                                couldWrite = false;
                            }
                        }
                        if (!couldWrite)
                        {
                            FrontEndCommon.CannotSetPropertyMoreBoxes(ResManager);
                        }
                    }
                    break;

                default:
                    break;
            }

            //the whole adapt has to be done, because setting some properties
            //can change others
            Adapt();
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
            lock (tempLocker)
            {
                switch (typeName)
                {
                    case "Ferda.FrontEnd.Properties.OtherProperty":
                        if (IsOneBoxSelected)
                        {
                            e.Value = (OtherProperty)temporaryValues[realPropertyName];
                        }
                        else //setting otherOtherProperty for more boxes does not make sense
                        {
                            throw new ApplicationException("This situation should not happen, other property can be set for one selected box only");
                        }
                        break;

                    case "Ferda.FrontEnd.Properties.StringSequence":
                        e.Value = (StringSequence)temporaryValues[realPropertyName];
                        break;

                    case "System.String":
                        e.Value = (string)temporaryValues[realPropertyName];
                        break;

                    case "System.Int32":
                        e.Value = (Int32)temporaryValues[realPropertyName];
                        break;

                    case "System.Boolean":
                        e.Value = (bool)temporaryValues[realPropertyName];
                        break;

                    case "System.Int16":
                        e.Value = (Int16)temporaryValues[realPropertyName];
                        break;

                    case "System.Int64":
                        e.Value = (Int64)temporaryValues[realPropertyName];
                        break;

                    case "System.Single":
                        e.Value = (Single)temporaryValues[realPropertyName];
                        break;

                    case "System.Double":
                        e.Value = (Double)temporaryValues[realPropertyName];
                        break;

                    case "System.DateTime":
                        e.Value = (DateTime)temporaryValues[realPropertyName];
                        break;

                    case "System.TimeSpan":
                        e.Value = (TimeSpan)temporaryValues[realPropertyName];
                        break;

                    default:
                        //here other types will be treated
                        break;
                }
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
            PropertyTable table = sender as PropertyTable;
            if (table == null)
            {
                throw new ApplicationException("It is not a property bag");
            }

            lock (tempLocker)
            {
                if (table.ClickID != this.ClickID)
                {
                    return;
                }

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
            }
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
