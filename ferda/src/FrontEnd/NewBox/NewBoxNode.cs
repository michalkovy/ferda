// NewBoxNode.cs - extension to the normal TreeNode to display new boxes
// 
// Author: Martin Ralbovský <martin.ralbovsky@gmail.com>
//
// Copyright (c) 2005 Martin Ralbovský
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

namespace Ferda.FrontEnd.NewBox
{
    /// <summary>
    /// Determines the type of the node (if it is a box or a category
    /// of boxes)
    /// </summary>
    public enum ENodeType
    {
        /// <summary>
        /// It is a box
        /// </summary>
        Box, 
        /// <summary>
        /// It is a category of boxes
        /// </summary>
        Category
    }

    /// <summary>
    /// Class for the tree nodes in the NewBox control. It has additional
    /// information about the type of the node (if it is a box type or a 
    /// category) for drag&amp;drop operations
    /// </summary>
    public class NewBoxNode : TreeNode
    {
        #region Private fields

        private ENodeType nodeType;
        private string identifier;

        #endregion

        #region Properties

        /// <summary>
        /// Type of the node
        /// </summary>
        public ENodeType NodeType
        {
            get
            {
                return nodeType; 
            }
        }

        /// <summary>
        /// String identifier of the creator that is represented by this node
        /// </summary>
        public string Identifier
        {
            get
            {
                return identifier;
            }
        }
        #endregion

        #region Constructor

        /// <summary>
        /// Constructor of the class, that should be used
        /// </summary>
        /// <param name="name">name of the node</param>
        /// <param name="type">type of the node</param>
        /// <param name="ident">ICE identifier of the box factory</param>
        public NewBoxNode(string name, ENodeType type, string ident) : base(name)
        {
            nodeType = type;
            identifier = ident;
        }
        #endregion
    }
}
