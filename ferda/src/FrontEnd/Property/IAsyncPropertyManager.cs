using System;
using System.Collections.Generic;
using System.Text;

namespace Ferda.FrontEnd.Properties
{
    /// <summary>
    /// The interface is implemented when a control wants asynchronous
    /// changing of properties
    /// </summary>
    public interface IAsyncPropertyManager
    {
        /// <summary>
        /// Identifier of the box which properties are beeing shown
        /// at the moment. It serves the asynchronous property getting.
        /// </summary>
        int BoxIdentifier
        {
            set;
            get;
        }

        /// <summary>
        /// The property value is changed, so the the propertyGrid should be
        /// refilled with new values
        /// </summary>
        /// <param name="catcher">Catcher of the connection</param>
        /// <param name="value">New value of the property</param>
        void ChangedPropertyValue(AsyncPropertyCatcher catcher, object value);
    }
}
