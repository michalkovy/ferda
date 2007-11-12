// Functions.cs - Function objects for the Ontology Mapping box
//
// Author: Martin Zeman <martin.zeman@email.cz>
//
// Copyright (c) 2007 Martin Zeman
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
using System.Collections.Generic;
using System.Text;
using Ice;

namespace Ferda.Modules.Boxes.OntologyRelated.OntologyMapping
{
    /// <summary>
    /// Class is providing ICE functionality of the Ontology Mapping
    /// box module
    /// </summary>
    internal class Functions : OntologyMappingFunctionsDisp_, IFunctions
    {
        /// <summary>
        /// The current box module
        /// </summary>
        protected BoxModuleI _boxModule;

        #region IFunctions Members

        /// <summary>
        /// Sets the <see cref="T:Ferda.Modules.BoxModuleI">box module</see>
        /// and the <see cref="T:Ferda.Modules.Boxes.IBoxInfo">box info</see>.
        /// </summary>
        /// <param name="boxModule">The box module.</param>
        /// <param name="boxInfo">The box info.</param>
        public void setBoxModuleInfo(BoxModuleI boxModule, IBoxInfo boxInfo)
        {
            _boxModule = boxModule;
        }

        #endregion

        #region Properties

        //names of the properties
        public const string SockOntology = "Ontology";
        public const string OntologyMapping = "onto mapping";
        
        #endregion

        #region Methods

        /* TODO - nahradit OntologyMapping Metodama
        /// <summary>
        /// Gets the names of data tables in the database
        /// </summary>
        /// <param name="fallOnError">If to fail on error</param>
        /// <returns>The names of data tables in the database</returns>
        public string[] GetDataTablesNames(bool fallOnError)
        {
            return ExceptionsHandler.GetResult<string[]>(
                fallOnError,
                delegate
                    {
                        GenericDatabase tmp = GetGenericDatabase(fallOnError);
                        if (tmp != null)
                            return tmp.GetAcceptableDataTablesNames(AcceptableDataTableTypes);
                        return new string[0];
                    },
                delegate
                    {
                        return new string[0];
                    },
                _boxModule.StringIceIdentity
                );
        }
         */

        #endregion

        #region Ice Functions

        /*TOTO nahradit OntologyMapping Functions
        /// <summary>
        /// Gets names of tables in the database
        /// </summary>
        /// <param name="current__">Ice stuff</param>
        /// <returns>Names of tables</returns>
        public override string[] getDataTablesNames(Current current__)
        {
            return GetDataTablesNames(true);
        }
         */

        public override string HelloWorld(Ice.Current __current)
        {
            return "Hello World!";
        }


        #endregion
    }
}
