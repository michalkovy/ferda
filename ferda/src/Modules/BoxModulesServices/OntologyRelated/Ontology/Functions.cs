// Functions.cs - Function objects for the Ontology box module
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

namespace Ferda.Modules.Boxes.OntologyRelated.Ontology
{
    /// <summary>
    /// Class is providing ICE functionality of the Ontology
    /// box module
    /// </summary>
    internal class Functions : OntologyFunctionsDisp_, IFunctions
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
        public const string OntologyPath = "OntologyPath";
        
        /// <summary>
        /// The Ontology Path property
        /// </summary>
        /*public string OntologyPath
        {
            get { return _boxModule.GetPropertyString(OntologyPath); }
        }*/

        #endregion

        #region Methods

        
        /* TODO - nahradit Ontology Metodama
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

        /*TOTO nahradit Ontology Functions
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
            System.Windows.Forms.MessageBox.Show("Funkce je volaná s Ontology Path : " + OntologyPath);
            int status = 0;
		    Ice.Communicator ic = null;
		    try {
			    ic = Ice.Util.initialize();
			    Ice.ObjectPrx obj = ic.stringToProxy("OWLParser:default -p 10000");
			    
                /*OWLParserPrx parser = OWLParserPrxHelper.checkedCast(obj);
			    if (parser == null)
				    throw new Error("Invalid proxy");
    			
			    OntologyStructure FerdaOntology = new OntologyStructure();
    						
			    FerdaOntology = parser.parseOntology("file:/D:/Marthin/skola/diplomova_prace/pokusne_ontologie/umls_stulong_2/umls_stulong_2.owl");*/
                status = 0;
            } catch (Ice.Exception e) {
                Console.Error.WriteLine(e);
                status = 1;
            }
            if (ic != null) {
                // Clean up
                //
                try {
                    ic.destroy();
                } catch (Ice.Exception e) {
                    Console.Error.WriteLine(e);
                    status = 1;
                }
            }
            return "end of Hello world";
            //Environment.Exit(status);
        }

        public override void LoadOntology(Ice.Current __current)
        {
            //TODO funkce, ktera ma zajistit spusteni serveru (resp overit, ze bezi) a nacteni a parsovani ontologie
        }


        #endregion
    }
}
