// FormulaHelper.cs - methods for help to build formulas
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

using System;
using System.Collections.Generic;
using Ferda.Modules.Helpers.Common;

namespace Ferda.Guha.MiningProcessor.Formulas
{
    /// <summary>
    /// Possible separators for a formula
    /// </summary>
    public enum FormulaSeparator
    {
        And,
        Or,
        AtomMembers
    }

    /// <summary>
    /// Provides static methods for helping of formula building.
    /// </summary>
    public static class FormulaHelper
    {
        /// <summary>
        /// Constructs a new conjunction formula out of two operands
        /// </summary>
        /// <param name="operandA">First operand</param>
        /// <param name="operandB">Second operand</param>
        /// <returns>Resulting conjunction formula</returns>
        public static BooleanAttributeFormula And(BooleanAttributeFormula operandA, BooleanAttributeFormula operandB)
        {
            return new ConjunctionFormula(operandA, operandB);
        }

        /// <summary>
        /// Constructs a new disjunction formula out of two operands
        /// </summary>
        /// <param name="operandA">First operand</param>
        /// <param name="operandB">Second operand</param>
        /// <returns>Resulting disjunction formula</returns>
        public static BooleanAttributeFormula Or(BooleanAttributeFormula operandA, BooleanAttributeFormula operandB)
        {
            return new DisjunctionFormula(operandA, operandB);
        }

        /// <summary>
        /// Constructs a new negation formula out of two operands
        /// </summary>
        /// <param name="operandA">First operand</param>
        /// <param name="operandB">Second operand</param>
        /// <returns>Resulting negation formula</returns>
        public static BooleanAttributeFormula Not(BooleanAttributeFormula operand)
        {
            return new NegationFormula(operand);
        }

        /// <summary>
        /// Should be the unicode Character 'LOGICAL AND' (U+2227)
        /// http://www.fileformat.info/info/unicode/char/2227/index.htm
        /// but it is the normal & instead. The reason is, that the fonts
        /// that support unicode are in normal windows installation
        /// </summary>
        public const string SeparatorAnd = " & ";

        //public const string SeparatorAnd = "&";

        /// <summary>
        /// Should be the unicode Character 'LOGICAL OR' (U+2228)
        /// http://www.fileformat.info/info/unicode/char/2228/index.htm
        /// but it is the normal | instead. The reason is, that the fonts
        /// that support unicode are in normal windows installation
        /// </summary>
        public const string SeparatorOr = " | ";

        /// <summary>
        /// Separator of atoms
        /// </summary>
        public const string SeparatorAtomMembers = ", ";

        /// <summary>
        /// Unicode Character 'NOT SIGN' (U+00AC)
        /// http://www.fileformat.info/info/unicode/char/00ac/index.htm
        /// </summary>
        public const string NegationSign = "\u00AC";

        /// <summary>
        /// Converts a sequence of items (operands of a formula) into string
        /// and binds them with the desired separator.
        /// </summary>
        /// <param name="items">The items</param>
        /// <param name="formulaSeparator">Formula separator</param>
        /// <param name="sorted">If the items are sorted</param>
        /// <returns>Sequence converted to string</returns>
        public static string SequenceToString(IEnumerable<string> items, FormulaSeparator formulaSeparator, bool sorted)
        {
            string separator;
            switch (formulaSeparator)
            {
                case FormulaSeparator.And:
                    separator = SeparatorAnd;
                    break;
                case FormulaSeparator.Or:
                    separator = SeparatorOr;
                    break;
                case FormulaSeparator.AtomMembers:
                    separator = SeparatorAtomMembers;
                    break;
                default:
                    throw new NotImplementedException();
            }

            if (sorted)
            {
                throw new NotImplementedException();
                //List<string> sortedItems = new List<string>(items);
                //items = sortedItems;
            }

            return Print.SequenceToString(items, separator);
        }
    }
}