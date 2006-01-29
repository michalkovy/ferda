using System;
using System.Collections.Generic;
using System.Text;
using Ferda.Modules;

namespace Ferda.FrontEnd.Properties
{
    internal class AsyncPropertyCatcher : AMI_BoxModule_getProperty
    {
        #region Fields

        /// <summary>
        /// Value of the property that is beeing changed
        /// </summary>
        protected object propertyValue;

        /// <summary>
        /// The manager of all the properties of the box
        /// </summary>
        protected IAsyncPropertyManager myManager;

        #endregion

        #region Properties

        /// <summary>
        /// Value of the property that is beeing changed
        /// </summary>
        public object PropertyValue
        {
            get { return propertyValue; }
            set { propertyValue = value; }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="man">Manager of this property</param>
        public AsyncPropertyCatcher(IAsyncPropertyManager man)
        {
            myManager = man;
        }

        #endregion

        /// <summary>
        /// Ice has thrown a message, that a value of a property has been changed
        /// </summary>
        /// <param name="value">value of the property</param>
        public override void ice_response(Ferda.Modules.PropertyValue value)
        {
            myManager.ChangedPropertyValue(this, value);
        }

        /// <summary>
        /// Ice has thrown an exception
        /// </summary>
        /// <param name="ex">ICE exception</param>
        public override void ice_exception(Ice.Exception ex)
        {
        }
    }
}
