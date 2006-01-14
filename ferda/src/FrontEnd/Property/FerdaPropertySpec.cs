using System;
using System.Collections.Generic;
using System.Text;
using Ferda.FrontEnd.External;

namespace Ferda.FrontEnd.Properties
{
    /// <summary>
    /// A standard <see cref="T:Ferda.FrontEnd.External.PropertySpec"/> class
    /// with information about the type of the property - if it is a socket
    /// property or a normal property
    /// </summary>
    public class FerdaPropertySpec : PropertySpec
    {
        private bool socketProperty;

        /// <summary>
        /// Deterimes if this property belongs to a socket or if it is
        /// a normal property
        /// </summary>
        public bool SocketProperty
        {
            set
            {
                socketProperty = value;
            }
            get
            {
                return socketProperty;
            }
        }

        /// <summary>
        /// Constructor that should be used for the class
        /// </summary>
        /// <param name="propName">Name of the property</param>
        /// <param name="propType">Type of the property</param>
        /// <param name="sockProp">if it is a socket property</param>
        public FerdaPropertySpec(string propName, string propType, bool sockProp)
            : base(propName, propType)
        {
            this.SocketProperty = sockProp;
        }

        /// <summary>
        /// Other constructor that should be used
        /// </summary>
        /// <param name="propName">Name of the property</param>
        /// <param name="type">Type of the property</param>
        /// <param name="sockProp">if it is a socket property</param>
        public FerdaPropertySpec(string propName, Type type, bool sockProp)
            : base(propName, type)
        {
            this.SocketProperty = sockProp;
        }
    }
}
