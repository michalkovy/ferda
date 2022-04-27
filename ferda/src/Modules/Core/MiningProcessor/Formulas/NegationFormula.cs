// NegationFormula.cs - formula for negation
//
// Author: Tomáš Kuchaø <tomas.kuchar@gmail.com>
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

using Ferda.Modules.Helpers.Common;

namespace Ferda.Guha.MiningProcessor.Formulas
{
    /// <summary>
    /// The class represents a negation formula
    /// </summary>
    public class NegationFormula : BooleanAttributeFormula
    {
        /// <summary>
        /// Operand of the negation 
        /// </summary>
        private readonly BooleanAttributeFormula _operand;

        /// <summary>
        /// Operand of the negation 
        /// </summary>
        public BooleanAttributeFormula Operand
        {
            get { return _operand; }
        }

        public override Set<string> UsedAttributes => _operand.UsedAttributes;

        /// <summary>
        /// Default constructor of the class
        /// </summary>
        /// <param name="operand">Operand of negation</param>
        public NegationFormula(BooleanAttributeFormula operand)
        {
            _operand = operand;
        }

        /// <summary>
        /// Converts the instance of the formula to string. 
        /// </summary>
        /// <returns>String representation of the formula</returns>
        public override string ToString()
        {
            string result = _operand.ToString();
            if (result.Contains(FormulaHelper.SeparatorAnd) || result.Contains(FormulaHelper.SeparatorOr))
                return FormulaHelper.NegationSign + "(" + result + ")";
            else
                return FormulaHelper.NegationSign + result;
        }
    }
}