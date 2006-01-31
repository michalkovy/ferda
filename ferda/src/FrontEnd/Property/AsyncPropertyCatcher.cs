using System;
using System.Collections.Generic;
using System.Text;
using Ferda.Modules;

namespace Ferda.FrontEnd.Properties
{
    internal class AsyncPropertyCatcher : AMI_BoxModule_getProperty
    {
        #region Fields

        private IAsyncPropertyManager myManager;
        private string propertyName;
        private string propertyType;

        #endregion

        #region Properties

        /// <summary>
        /// Name (not the label) of the property that is beeing changed
        /// </summary>
        public string PropertyName
        {
            get { return propertyName; }
            set { propertyName = value; }
        }

        /// <summary>
        /// Type of the property (for good type conversion in the 
        /// propertyGrid.Temporary values
        /// </summary>
        public string PropertyType
        {
            get { return propertyType; }
            set { propertyType = value; }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="man">Manager of this property</param>
        /// <param name="propName">Name of the property</param>
        public AsyncPropertyCatcher(IAsyncPropertyManager man, string propName,
            string propertyType)
        {
            myManager = man;
            propertyName = propName;
            this.propertyType = propertyType;
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
