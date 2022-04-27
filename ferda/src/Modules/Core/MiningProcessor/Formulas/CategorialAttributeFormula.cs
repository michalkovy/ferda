// CategorialAttributeFormula.cs - Formula for categorial attribute
//
// Authors: Tomáš Kuchaø <tomas.kuchar@gmail.com>      
// Commented by: Martin Ralbovský <martin.ralbovsky@gmail.com>
//
// Copyright (c) 2006 Tomáš Kuchaø
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

namespace Ferda.Guha.MiningProcessor.Formulas
{
    /// <summary>
    /// Formula for categorial attribute. The categorial attribute represents
    /// the whole attribute, not just some of its categories. It is used in the
    /// <c>KL</c> and <c>CF</c> procedures. 
    /// </summary>
    public class CategorialAttributeFormula : Formula
    {
        /// <summary>
        /// Identification of attribute that is represented by this formula
        /// </summary>
        private string _attributeGuid;

        /// <summary>
        /// Identification of attribute that is represented by this formula
        /// </summary>
        public string AttributeGuid
        {
            get { return _attributeGuid; }
            set { _attributeGuid = value; }
        }

        /// <summary>
        /// Default constructor of the class
        /// </summary>
        /// <param name="attributeGuid">Attribute identification</param>
        public CategorialAttributeFormula(string attributeGuid)
        {
            _attributeGuid = attributeGuid;
        }

        /// <summary>
        /// Converts the instance of the class to string
        /// </summary>
        /// <returns>String representation of the class</returns>
        public override string ToString()
        {
            return AttributeNameInLiteralsProvider.GetAttributeNameInLiterals(_attributeGuid);
        }
    }
}