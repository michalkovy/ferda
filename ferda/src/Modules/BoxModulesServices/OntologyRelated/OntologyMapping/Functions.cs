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
using Ferda.OntologyRelated.generated.OntologyData;

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
        public const string Mapping = "Mapping";
        public const string PropNumberOfMappedPairs = "NumberOfMappedPairs";

        public IntTI NumberOfMappedPairs
        {
            get
            {
                //OntologyStructure tmpOntology = getOntology(false);
                //return (tmpOntology != null) ? tmpOntology.OntologyClassMap.Values.Count.ToString() : "0";
                return 777;
            }
        }

        #endregion

        #region Methods

        public StrSeqMap getOntologyEntityProperties(string dataTableColumnName, bool fallOnError)
        {
            return ExceptionsHandler.GetResult<StrSeqMap>(
                fallOnError,
                delegate
                    {
                        /*GenericDatabase tmp = GetGenericDatabase(fallOnError);
                        if (tmp != null)
                        {
                            List<DataTableExplain> result = new List<DataTableExplain>();
                            foreach (GenericDataTable table in tmp)
                            {
                                if (table.IsAcceptable(accepts))
                                    result.Add(table.Explain);
                            }
                            return result.ToArray();
                        }
                        return new DataTableExplain[0];*/
                        StrSeqMap dataPropertiesMap = new StrSeqMap();

                        /*TODO - upravit odpovidajici vyhledani informaci z ontologie na zaklade parametru dataTableColumnName a mapovani*/

                        string[] values = new string[3] { "15", "20", "30" };
                        dataPropertiesMap.Add("DomainDividingValues", values);
                        return dataPropertiesMap;
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

        /// <summary>
        /// Gets the properties (dataproperties) of ontology entity - for individuals it is empty
        /// </summary>
        /// <param name="dataTableColumnName">Name of table column which properties (based on mapping) I want to get</param>
        /// <param name="current__">Ice stuff</param>
        /// <returns>Data properties of ontology entity</returns>

        public override StrSeqMap getOntologyEntityProperties(string dataTableColumnName, Ice.Current __current)
        {
            return getOntologyEntityProperties(dataTableColumnName, true);
        }


        public override Ferda.Modules.BoxModule getBoxModule(Ice.Current __current)
        {
            return (BoxModule)_boxModule;
        }
        /*
        public BoxModuleI getBoxModule()
        {
            return _boxModule;
        }
        */
        #endregion
    }
}
