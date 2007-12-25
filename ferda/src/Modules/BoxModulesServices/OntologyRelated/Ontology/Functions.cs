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
// Foundation, Inc., 59 Temple Place, Suite 330,  Boston, MA  02111-1307  USA

using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        
        /// <summary>
        /// Structure for storing parsed ontology
        /// </summary>
        private OntologyStructure ontology;
        
        //names of the properties
        public const string PropOntologyPath = "OntologyPath";
        public const string PropOntologyURI = "OntologyURI";
        public const string PropNumberOfClasses = "NumberOfClasses";
        public const string PropLastReloadRequest = "LastReloadRequest";

        /// <summary>
        /// The Ontology Path property
        /// </summary>
        public string OntologyPath
        {
            get { return _boxModule.GetPropertyString(PropOntologyPath); }
        }

        /// <summary>
        /// Ontology URI
        /// </summary>
        public StringTI OntologyURI
        {
            get
            {
                OntologyStructure tmpOntology = getOntology(false);
                return (tmpOntology != null) ? tmpOntology.ontologyURI.ToString() : null;
            }
        }

        /// <summary>
        /// Number of classes in the ontology
        /// </summary>
        public StringTI NumberOfClasses
        {
            get
            {
                OntologyStructure tmpOntology = getOntology(false);
                return (tmpOntology != null) ? tmpOntology.OntologyClassMap.Values.Count.ToString() : "0";
            }
        }

        private DateTimeTI _lastReloadRequest = null;

        /// <summary>
        /// The Last Reload Request property
        /// </summary>
        public DateTimeTI LastReloadRequest
        {
            get
            {
                if (_lastReloadRequest == null)
                    _lastReloadRequest = DateTime.MinValue;
                return _lastReloadRequest;
            }
            set
            {
                Debug.Assert((DateTime)_lastReloadRequest <= (DateTime)value);
                _lastReloadRequest = value;
            }
        }

        /// <summary>
        /// The current box module
        /// </summary>
        public BoxModuleI BoxModule
        {
            get { return _boxModule; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the ontology
        /// </summary>
        /// <param name="fallOnError">If to fall on error</param>
        /// <returns>Parsed ontology</returns>
        public OntologyStructure getOntology(bool fallOnError)
        {
            if (OntologyPath != null)    //for loading the ontology after loading a saved project
            {
                LoadOntologyWithParameter(OntologyPath.ToString(), fallOnError);
            }

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

        /// <summary>
        /// Loads and parses the ontology. Connects to ICE module FerdaOWLParser (currently in java).
        /// </summary>
        /// <param name="innerOntologyPath">The path to the ontology (full path with file:/ prefix if the ontology is network based)</param>
        public void LoadOntologyDelegate(string innerOntologyPath)
        {
            /// a path to an ontology is not set
            if (innerOntologyPath.ToString() == "")
            {
                throw Ferda.Modules.Exceptions.BoxRuntimeError(null, _boxModule.StringIceIdentity,
                        "Parameter Path to Ontology is empty.");
            }
            /// a path to an ontology is set
            else
            {
                /// getting the proxy to FerdaOWLParser module
                Ferda.OntologyRelated.generated.OWLParserPrx prx =
                    Ferda.OntologyRelated.generated.OWLParserPrxHelper.checkedCast(
                        BoxModule.Manager.getManagersLocator().findAllObjectsWithType(
                            "::Ferda::OntologyRelated::OWLParser"
                        )[0]
                    );

                /// connection to FerdaOWLParser modules failed
                if (prx == null)
                {
                    throw Ferda.Modules.Exceptions.BoxRuntimeError(null, _boxModule.StringIceIdentity,
                            "Ice object ::Ferda::OntologyRelated::OWLParser can't be found.");
                }

                /// parsing the ontology
                /// 
                /// examples for eventual testing purposes
                /// this.ontology = prx.parseOntology("file:/D:/My ontologies/umls_stulong_2.owl");
                /// this.ontology = prx.parseOntology("http://www.co-ode.org/ontologies/pizza/2007/02/12/pizza.owl");
                this.ontology = prx.parseOntology(innerOntologyPath.ToString());

                /// the ontology is empty or in incorrect format
                if (this.ontology.OntologyClassMap.Keys.Count == 0)
                {
                    throw Ferda.Modules.Exceptions.BoxRuntimeError(null, _boxModule.StringIceIdentity,
                            "Ontology specified by the ontology path: " + innerOntologyPath.ToString() + " is incorrect. Either the path is wrong or the ontology is in incorrect format.");
                }
            }
            
            return;
        }

        /// <summary>
        /// Loads and parses the ontology
        /// </summary>
        /// <param name="innerOntologyPath">The path to the ontology (full path with file:/ prefix if the ontology is network based)</param>
        /// <param name="fallOnError">If to fall on error</param>
        /// <returns>True if the ontology was successfully loaded</returns>
        public bool LoadOntologyWithParameter(string innerOntologyPath, bool fallOnError)
        {
            if (fallOnError)
            {
                return ExceptionsHandler.TryCatchMethodThrow<bool>(
                    delegate
                        {
                            LoadOntologyDelegate(innerOntologyPath);
                            return true;
                        },
                    _boxModule.StringIceIdentity
                );
            }
            else 
            {
                return ExceptionsHandler.TryCatchMethodNoThrow<bool>(
                    delegate
                    {
                        LoadOntologyDelegate(innerOntologyPath);
                        return true;
                    },
                    delegate
                    {
                        return false;
                    }
                );
            }
        }

        #endregion

        #region Ice Functions

        /// <summary>
        /// Loads and parses the ontology
        /// </summary>
        /// <param name="current__">Ice stuff</param>
        public override void LoadOntology(Ice.Current __current)
        {
            if ((OntologyPath == null))
            {
                LoadOntologyWithParameter("", __current);
            }
            else
            {
                LoadOntologyWithParameter(OntologyPath.ToString(), __current);
            }
            return;
        }

        /// <summary>
        /// Loads and parses the ontology with specified path to ontology
        /// </summary>
        /// <param name="innerOntologyPath">The path to the ontology (full path with file:/ prefix if the ontology is network based)</param>
        /// <param name="current__">Ice stuff</param>
        public override void LoadOntologyWithParameter(string innerOntologyPath, Ice.Current __current)
        {
            LoadOntologyWithParameter(innerOntologyPath, true);
        }

        /// <summary>
        /// Gets the ontology
        /// </summary>
        /// <param name="current__">Ice stuff</param>
        /// <returns>Parsed ontology</returns>
        public override OntologyStructure getOntology(Current current__)
        {
            return getOntology(true);
        }

        /// <summary>
        /// Gets the data properties of the ontology entity
        /// </summary>
        /// <param name="entityName">The name of the ontology entity</param>
        /// <param name="current__">Ice stuff</param>
        /// <returns>Data properties of the ontology entity</returns>
        public override StrSeqMap getOntologyEntityProperties(string entityName, Current current__)
        {
            OntologyStructure ontology = getOntology(true);
            try
            {
                return ontology.OntologyClassMap[entityName].DataPropertiesMap;
            }
            catch
            {
                return new StrSeqMap();
            }
        }

        /// <summary>
        /// Gets the annotations of the ontology entity
        /// </summary>
        /// <param name="entityName">The name of the ontology entity</param>
        /// <param name="current__">Ice stuff</param>
        /// <returns>Annotations of the ontology entity</returns>
        public override string[] getOntologyEntityAnnotations(string entityName, Current current__)
        {
            OntologyStructure ontology = getOntology(true);

            try
            {
                //ontology entity is a class
                return ontology.OntologyClassMap[entityName].Annotations;
            }
            catch
            {
                //ontology entity is an individual
                try
                {
                    foreach (OntologyClass tmpOntologyClass in ontology.OntologyClassMap.Values) {
                        foreach (string tmpIndividual in tmpOntologyClass.InstancesAnnotations.Keys) {
                            if (tmpIndividual == entityName)
                                return tmpOntologyClass.InstancesAnnotations[tmpIndividual];
                        }
                    }
                }
                catch
                {
                    return new string[0];
                }
                return new string[0];
            }
        }

        /// <summary>
        /// Gets the superClass of the ontology entity from ontology, for individuals it returns the class from which the instance is instantiated
        /// </summary>
        /// <param name="ontologyEntityName">Name of the ontology entity</param>
        /// <param name="current__">Ice stuff</param>
        /// <returns>SuperClasses of the ontology entity</returns>
        public override string[] getOntologyEntitySuperClasses(string ontologyEntityName, Current current__)
        {
            OntologyStructure ontology = getOntology(true);
            try
            {
                return ontology.OntologyClassMap[ontologyEntityName].SuperClasses;
            }
            catch
            {
                return null;
            }
        }

        #endregion
    }
}
