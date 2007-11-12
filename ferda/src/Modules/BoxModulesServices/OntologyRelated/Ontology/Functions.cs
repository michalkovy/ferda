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
using Ferda.OntologyRelated.generated.OntologyData;

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
        
        private Ferda.OntologyRelated.generated.OntologyData.OntologyStructure ontology = new OntologyStructure();

        //names of the properties
        public const string PropOntologyPath = "OntologyPath";

        /// <summary>
        /// The Ontology Path property
        /// </summary>
        public string OntologyPath
        {
            get { return _boxModule.GetPropertyString(PropOntologyPath); }
        }

        public BoxModuleI BoxModule
        {
            get { return _boxModule; }
        }

        #endregion

        #region Methods

        public OntologyStructure getOntology(bool fallOnError)
        {
            return ExceptionsHandler.GetResult<OntologyStructure>(
                fallOnError,
                delegate
                {
                    return ontology;
                },
                delegate
                {
                    return null;
                },
                _boxModule.StringIceIdentity
                );
        }

        #endregion

        #region Ice Functions

        public override void LoadOntology(Ice.Current __current)
        {
            LoadOntologyWithParameter(OntologyPath.ToString(), __current);
            return;
        }

        public override void LoadOntologyWithParameter(string innerOntologyPath, Ice.Current __current)
        {
            
            Ice.Communicator ic = null;
            ic = Ice.Util.initialize();

            Ferda.OntologyRelated.generated.OWLParserPrx prx =
                Ferda.OntologyRelated.generated.OWLParserPrxHelper.checkedCast(
                    BoxModule.Manager.getManagersLocator().findAllObjectsWithType(
                        "::Ferda::OntologyRelated::OWLParser"
                    )[0]
                );

            if (prx == null)
            {
                throw Ferda.Modules.Exceptions.BoxRuntimeError(null, _boxModule.StringIceIdentity,
                        "Ice object ::Ferda::OntologyRelated::OWLParser can't be found.");
            }

            this.ontology = prx.parseOntology(innerOntologyPath.Replace("\\", "/").ToString());
            //examples
            //this.ontology = prx.parseOntology("file:/D:/Marthin/skola/diplomova_prace/pokusne_ontologie/umls_stulong_2/umls_stulong_2.owl");
            //this.ontology = prx.parseOntology("http://www.co-ode.org/ontologies/pizza/2007/02/12/pizza.owl"

            if (this.ontology.OntologyClassMap.Keys.Count == 0)
            {
                throw Ferda.Modules.Exceptions.BoxRuntimeError(null, _boxModule.StringIceIdentity,
                        "Ontology specified by the ontology path: " + innerOntologyPath.ToString() + " is incorrect. Either the path is wrong or the ontology is in incorrect format.");
            }
            
            return;
        }

        public override OntologyStructure getOntology(Current current__)
        {
            return getOntology(true);
        }

        #endregion
    }
}
