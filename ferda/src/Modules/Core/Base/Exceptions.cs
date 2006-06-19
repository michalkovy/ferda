using System;
using System.Diagnostics;

namespace Ferda.Modules
{
    /// <summary>
    /// Provides some functionality which helps you to work with (not only)
    /// Ferda.Modules exceptions.
    /// </summary>
    public static class Exceptions
    {
        private static void prepare(BoxRuntimeError ex, string boxIdentity, string userMessage)
        {
            Debug.Assert(!String.IsNullOrEmpty(userMessage));
            ex.boxIdentity = boxIdentity;
            ex.userMessage = userMessage;
        }

        private static void prepare(BadParamsError ex, string boxIdentity, string userMessage,
                                    restrictionTypeEnum restrictionType)
        {
            prepare(ex, boxIdentity, userMessage);
            ex.restrictionType = restrictionType;
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
        public static BadValueError BadValueError(Exception e, string boxIdentity, string userMessage,
                                                  string[] socketsNames, restrictionTypeEnum restrictionType)
        {
            BadValueError ex = new BadValueError(e);
            prepare(ex, boxIdentity, userMessage, restrictionType);
            ex.socketsNames = socketsNames;
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
        public static BadParamsError BadParamsError(Exception e, string boxIdentity, string userMessage,
                                                    restrictionTypeEnum restrictionType)
        {
            BadParamsError ex = new BadParamsError(e);
            prepare(ex, boxIdentity, userMessage, restrictionType);
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
        public static NoConnectionInSocketError NoConnectionInSocketError(Exception e, string boxIdentity, string[] socketsNames)
        {
            Debug.Assert(socketsNames == null || socketsNames.Length == 0);

            NoConnectionInSocketError ex = new NoConnectionInSocketError(e);
            prepare(ex, boxIdentity, "There is no connection in the socket!");
            ex.socketsNames = socketsNames;
            return ex;
        }

        /// <summary>
        /// Gets BoxRuntimeError exception.
        /// </summary>
        /// <param name="e">The inner exception.</param>
        /// <param name="boxIdentity">The box identity.</param>
        /// <param name="userMessage">The user message.</param>
        /// <returns>
        /// The <see cref="Ferda.Modules.BoxRuntimeError"/> exception.
        /// </returns>
        public static BoxRuntimeError BoxRuntimeError(Exception e, string boxIdentity, string userMessage)
        {
            BoxRuntimeError ex = new BoxRuntimeError(e);
            prepare(ex, boxIdentity, userMessage);
            return ex;
        }

        /// <summary>
        /// Gets NameNotExistError exception.
        /// </summary>
        /// <param name="e">The inner exception.</param>
        /// <param name="notExistingName">The not existing name.</param>
        /// <returns>
        /// The <see cref="Ferda.Modules.NameNotExistError"/> exception.
        /// </returns>
        public static NameNotExistError NameNotExistError(Exception e, string notExistingName)
        {
            return new NameNotExistError("NotExistingName: " + notExistingName, e);
        }
    }
}