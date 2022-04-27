// MappedPair.cs - Mapped pair of ontology entity - database column
//
// Author: Martin Zeman <martinzeman@email.cz>
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
using System.Windows.Forms;

namespace Ferda.FrontEnd.AddIns.SetOntologyMapping
{
    /// <summary>
    /// Mapped pair of ontology entity - database column
    /// </summary>
    public class MappedPair
    {
        /// <summary>
        /// Name of the data table
        /// </summary>
        public string DataTableName;
        /// <summary>
        /// Name of the data table column
        /// </summary>
        public string DataTableColumnName;
        /// <summary>
        /// Name of the ontology entity
        /// </summary>
        public string OntologyEntityName;
        /// <summary>
        /// Position in the tree view of the columns
        /// </summary>
        public TreeNode ColumnPositionInTreeView;


        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="dataTableName">Name of the data table</param>
        /// <param name="dataTableColumnName">Name of the data table column</param>
        /// <param name="columnPositionInTreeView">Position in the tree view of the columns</param>
        /// <param name="ontologyEntityName">Name of the ontology entity</param>
        public MappedPair(string dataTableName, string dataTableColumnName, TreeNode columnPositionInTreeView, string ontologyEntityName)
        {
            DataTableName = dataTableName;
            DataTableColumnName = dataTableColumnName;
            ColumnPositionInTreeView = columnPositionInTreeView;
            OntologyEntityName = ontologyEntityName;
        }

        /// <summary>
        /// String representation
        /// </summary>
        /// <returns>String representation</returns>
        public override string ToString()
        {
            return DataTableName + "." + DataTableColumnName + " - " + OntologyEntityName;
        }
    }
}
