// Helpers.cs - Helper classes for the SemanticWeb boxes
//
// Author: Martin Ralbovský <martin.ralbovsky@gmail.cz>
//
// Copyright (c) 2009 Martin Ralbovský
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
using System.Xml;
using Ferda.Guha.MiningProcessor;
using Ferda.Guha.MiningProcessor.Results;

namespace Ferda.Modules.Boxes.SemanticWeb.Helpers
{
    /// <summary>
    /// A helping structure for storing information about Boolean attrbutes in PMML
    /// </summary>
    public struct PMMLBooleanAttribute
    {
        /// <summary>
        /// The Entity setting
        /// </summary>
        public IEntitySetting EntitySetting;

        /// <summary>
        /// Unique identifier of the Boolean attribute used for XML referencing
        /// </summary>
        public int Number;

        /// <summary>
        /// The text representatio of the Boolean attribute, which is the
        /// same as the default identifiers of the Boolean attribute boxes.
        /// </summary>
        public string TextRepresentation;
    }

    /// <summary>
    /// Class for providing helping information for the PMMLBuilder box for
    /// providing Boolean attributes
    /// </summary>
    public class PMMLBooleanAttributeHelper
    {
        /// <summary>
        /// List of Boolean attribute helping structures.
        /// </summary>
        private List<PMMLBooleanAttribute> BAs;

        /// <summary>
        /// Default constructor of the class
        /// </summary>
        /// <param name="functions">The PMML builder functions object</param>
        /// <param name="taskPrx">The 4FT task proxy</param>
        public PMMLBooleanAttributeHelper(PMMLBuilder.Functions functions, 
            MiningTaskFunctionsPrx taskPrx)
        {

        }

        public XmlTextWriter WriteBasicBooleanAttributes(XmlTextWriter output)
        {
            return null;
        }

        public XmlTextWriter WriteDerivedBooleanAttributes(XmlTextWriter output)
        {
            return null;
        }

        public int GetAntecedentNumber()
        {
            return 0;
        }

        public int GetSuccedentNumber()
        {
            return 0;
        }

        public int GetConditionNumber()
        {
            return 0;
        }
    }
}
