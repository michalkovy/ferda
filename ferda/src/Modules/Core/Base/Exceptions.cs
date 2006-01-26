using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace Ferda.Modules
{
    /// <summary>
    /// Provides some functionality which helps you to work with (not only)
    /// Ferda.Modules exceptions.
    /// </summary>
    public static class Exceptions
    {
        /// <summary>
        /// Gets BoxRuntimeError exception.
        /// </summary>
        /// <param name="e">The inner exception.</param>
        /// <param name="boxIdentity">The box identity.</param>
        /// <param name="userMessage">The user message.</param>
        /// <returns>
        /// The <see cref="Ferda.Modules.BoxRuntimeError"/> exception.
        /// </returns>
        public static Ferda.Modules.BoxRuntimeError BoxRuntimeError(Exception e, string boxIdentity, string userMessage)
        {
            Debug.WriteLine(userMessage);
            Ferda.Modules.BoxRuntimeError ex = new BoxRuntimeError(e);
            ex.boxIdentity = boxIdentity;
            ex.userMessage = userMessage;
            return ex;
        }

        /// <summary>
        /// Gets NoConnectionInSocketError exception.
        /// </summary>
        /// <param name="e">The inner exception.</param>
        /// <param name="boxIdentity">The box identity.</param>
        /// <param name="userMessage">The user message.</param>
        /// <param name="socketsNames">Names of the empty sockets.</param>
        /// <returns>
        /// The <see cref="Ferda.Modules.NoConnectionInSocketError"/> exception.
        /// </returns>
        public static Ferda.Modules.NoConnectionInSocketError NoConnectionInSocketError(Exception e, string boxIdentity, string userMessage, string[] socketsNames)
        {
            Debug.WriteLine(userMessage);
            Ferda.Modules.NoConnectionInSocketError ex = new NoConnectionInSocketError(e);
            ex.boxIdentity = boxIdentity;
            ex.userMessage = userMessage;
            ex.socketsNames = socketsNames;
            return ex;
        }

        /// <summary>
        /// Gets BadValueError exception.
        /// </summary>
        /// <param name="e">The inner exception.</param>
        /// <param name="boxIdentity">The box identity.</param>
        /// <param name="userMessage">The user message.</param>
        /// <param name="socketsNames">The names of the sockets where are bad values.</param>
        /// <param name="restrictionType">Type of the restriction.</param>
        /// <returns>
        /// The <see cref="Ferda.Modules.BadValueError"/> exception.
        /// </returns>
        public static Ferda.Modules.BadValueError BadValueError(Exception e, string boxIdentity, string userMessage, string[] socketsNames, restrictionTypeEnum restrictionType)
        {
            Debug.WriteLine(userMessage);
            Ferda.Modules.BadValueError ex = new BadValueError(e);
            ex.boxIdentity = boxIdentity;
            ex.userMessage = userMessage;
            ex.socketsNames = socketsNames;
            ex.restrictionType = restrictionType;
            return ex;
        }

        /// <summary>
        /// Gets NameNotExistError exception.
        /// </summary>
        /// <param name="e">The inner exception.</param>
        /// <param name="boxIdentity">The box identity.</param>
        /// <param name="userMessage">The user message.</param>
        /// <param name="notExistingName">The not existing name.</param>
        /// <returns>
        /// The <see cref="Ferda.Modules.NameNotExistError"/> exception.
        /// </returns>
        public static Ferda.Modules.NameNotExistError NameNotExistError(Exception e, string boxIdentity, string userMessage, string notExistingName)
        {
            Debug.WriteLine(userMessage + " NotExistingName: " + notExistingName);
            Ferda.Modules.NameNotExistError ex = new NameNotExistError(e);
            return ex;
        }

        /// <summary>
        /// Gets BadParamsError exception.
        /// </summary>
        /// <param name="e">The inner exception.</param>
        /// <param name="boxIdentity">The box identity.</param>
        /// <param name="userMessage">The user message.</param>
        /// <param name="restrictionType">Type of the restriction.</param>
        /// <returns>
        /// The <see cref="T:Ferda.Modules.BadParamsError"/> exception.
        /// </returns>
        public static Ferda.Modules.BadParamsError BadParamsError(Exception e, string boxIdentity, string userMessage, restrictionTypeEnum restrictionType)
        {
            Debug.WriteLine(userMessage);
            Ferda.Modules.BadParamsError ex = new BadParamsError(e);
            ex.boxIdentity = boxIdentity;
            ex.userMessage = userMessage;
            ex.restrictionType = restrictionType;
            return ex;
        }

        public static Ferda.Modules.BadParamsError BadParamsUnexpectedReasonError(Exception e, string boxIdentity)
        {
            Debug.WriteLine(e.Message);
            Ferda.Modules.BadParamsError ex = new BadParamsError(e);
            ex.boxIdentity = boxIdentity;
            ex.restrictionType = restrictionTypeEnum.UnexpectedReason;
            return ex;
        }

        /// <summary>
        /// Gets SwitchCaseNotImplementedError exception.
        /// This exception should be thrown in <c>default</c> branch of 
        /// the switch is used and it shouldn`t be.
        /// </summary>
        /// <param name="value">The value which leads to the exception.</param>
        /// <returns>
        /// The <see cref="T:System.ArgumentOutOfRangeException"/> exception.
        /// </returns>
        public static ArgumentOutOfRangeException SwitchCaseNotImplementedError(object value)
        {
            Debug.WriteLine("SwitchCaseNotImplementedError(" + value.ToString() + ")");
            return new ArgumentOutOfRangeException(value.ToString());
        }

        /// <summary>
        /// Gets SwitchCaseNotImplementedError exception.
        /// This exception should be thrown in <c>default</c> branch of
        /// the switch is used and it shouldn`t be.
        /// </summary>
        /// <param name="value">The value which leads to the exception.</param>
        /// <param name="userMessage">The user message.</param>
        /// <returns>
        /// The <see cref="T:System.ArgumentOutOfRangeException"/> exception.
        /// </returns>
        public static ArgumentOutOfRangeException SwitchCaseNotImplementedError(object value, string userMessage)
        {
            Debug.WriteLine(userMessage + " SwitchCaseNotImplementedError(" + value.ToString() + ")");
            return new ArgumentOutOfRangeException(value.ToString(), userMessage);
        }
    }
}