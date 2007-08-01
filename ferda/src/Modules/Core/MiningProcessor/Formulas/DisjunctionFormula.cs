// DisjunctionFormula.cs - formula for disjunction
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

using System.Collections.Generic;

namespace Ferda.Guha.MiningProcessor.Formulas
{
    /// <summary>
    /// The class represents a disjunction formula
    /// </summary>
    public class DisjunctionFormula : BooleanAttributeFormula
    {
        /// <summary>
        /// List of operands of disjunction
        /// </summary>
        private readonly List<BooleanAttributeFormula> _operands;

        /// <summary>
        /// List of operands of disjunction
        /// </summary>
        public List<BooleanAttributeFormula> Operands
        {
            get { return _operands; }
        }

        /// <summary>
        /// The static function adds a new operand to a list of operands.
        /// </summary>
        /// <param name="list">List of operands (where to add)</param>
        /// <param name="operand">New operand (what to add)</param>
        private static void addOperand(List<BooleanAttributeFormula> list, BooleanAttributeFormula operand)
        {
            // flattening
            DisjunctionFormula disjunction = operand as DisjunctionFormula;
            if (disjunction == null)
                list.Add(operand);
            else
                list.AddRange(disjunction.Operands);
        }

        /// <summary>
        /// Constructor of the class for multiple operands
        /// </summary>
        /// <param name="operands">Multiple operands</param>
        public DisjunctionFormula(BooleanAttributeFormula[] operands)
        {
            _operands = new List<BooleanAttributeFormula>(2);
            foreach (BooleanAttributeFormula operand in operands)
            {
                addOperand(_operands, operand);
            }
        }

        /// <summary>
        /// Constructor of the class for two operands
        /// </summary>
        /// <param name="operandA">First operand</param>
        /// <param name="operandB">Second operand</param>
        public DisjunctionFormula(BooleanAttributeFormula operandA, BooleanAttributeFormula operandB)
        {
            _operands = new List<BooleanAttributeFormula>(2);
            addOperand(_operands, operandA);
            addOperand(_operands, operandB);
        }

        /// <summary>
        /// Converts the instance of the formula to string. 
        /// </summary>
        /// <returns>String representation of the formula</returns>
        public override string ToString()
        {
            List<string> result = new List<string>();
            Dictionary<string, List<string>> atoms = new Dictionary<string, List<string>>();
            foreach (BooleanAttributeFormula formula in _operands)
            {
                AtomFormula atomFormula = formula as AtomFormula;
                if (atomFormula == null)
                    result.Add(formula.ToString());
                else
                {
                    //group atoms
                    if (!(atoms.ContainsKey(atomFormula.BitStringIdentifier.AttributeGuid)))
                    {
                        atoms[atomFormula.BitStringIdentifier.AttributeGuid] = new List<string>();
                    }
                    atoms[atomFormula.BitStringIdentifier.AttributeGuid].Add(atomFormula.BitStringIdentifier.CategoryId);
                }
            }
            // print atoms
            foreach (KeyValuePair<string, List<string>> pair in atoms)
            {
                result.Add(AtomFormula.WriteAtom(pair.Key, pair.Value));
            }
            return FormulaHelper.SequenceToString(result, FormulaSeparator.Or, true);
        }
    }
}