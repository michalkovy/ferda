// Hypothesis.cs - one GUHA hypothesis (effective form)
//
// Authors: Tomáš Kuchař <tomas.kuchar@gmail.com>      
// Commented by: Martin Ralbovský <martin.ralbovsky@gmail.com>
//
// Copyright (c) 2006 Tomáš Kuchař
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
using System.Diagnostics;
using System.Text;
using Ferda.Guha.MiningProcessor.Formulas;

namespace Ferda.Guha.MiningProcessor.Results
{
    /// <summary>
    /// The effective form of <see cref="T:Ferda.Guha.MiningProcessor.Results.SerializableHypothesis"/>.
    /// Supports methods to gets/sets formulas 
    /// by semantic of given boolean/categorial attribute.
    /// The list of semantics supported by the current task type is given
    /// by method <code>GetSemanticMarks()</code> in the <see cref="T:Ferda.Guha.MiningProcessor.Results.Result"/>.
    /// </summary>
    public class Hypothesis
    {
        /// <summary>
        /// The contingecy table (for first set in SD tasks).
        /// </summary>
        public double[][] ContingencyTableA;

        /// <summary>
        /// The contingency table for second set in SD tasks.
        /// </summary>
        public double[][] ContingencyTableB;

        /// <summary>
        /// Gets the numeric values attribute GUID.
        /// </summary>
        /// <value>The numeric values attribute GUID.</value>
        public string NumericValuesAttributeGuid
        {
            get
            {
                CategorialAttributeFormula f = GetFormula(MarkEnum.Attribute) as CategorialAttributeFormula;
                if (f == null)
                {
                    //Debug.Assert(false);
                    return null;
                }
                return f.AttributeGuid;
            }
        }

        #region Storage of formulas
        
        /// <summary>
        /// Count of possible formulas
        /// </summary>
        internal const int CountOfFormulas = 8;

        /// <summary>
        /// Internal storage for formulas
        /// </summary>
        internal Formula[] _formulas = new Formula[CountOfFormulas];

        /// <summary>
        /// Gets index for a specific mark enum (antecedent, succedent..)
        /// for internal representations of formulas (in an array)
        /// </summary>
        /// <param name="semantic">The specified semantics
        /// (antecedent, succedent...)</param>
        /// <returns>Index of the mark enum for internal array representation
        /// of the</returns>
        private int getIndex(MarkEnum semantic)
        {
            switch (semantic)
            {
                case MarkEnum.Antecedent:
                    return 0;
                case MarkEnum.Succedent:
                    return 1;
                case MarkEnum.Condition:
                    return 2;
                case MarkEnum.FirstSet:
                    return 3;
                case MarkEnum.SecondSet:
                    return 4;
                case MarkEnum.Attribute:
                    return 5;
                case MarkEnum.RowAttribute:
                    return 6;
                case MarkEnum.ColumnAttribute:
                    return 7;
                default:
                    throw new NotImplementedException();
            }
        } 
        
        #endregion

        /// <summary>
        /// Gets the formula of specified semantic.
        /// </summary>
        /// <param name="semantic">The semantic.</param>
        /// <returns></returns>
        public Formula GetFormula(MarkEnum semantic)
        {
            return _formulas[getIndex(semantic)];
        }

        /// <summary>
        /// Sets the formula of specified semantic.
        /// </summary>
        /// <param name="semantic">The semantic.</param>
        /// <param name="formula">The formula.</param>
        public void SetFormula(MarkEnum semantic, Formula formula)
        {
            _formulas[getIndex(semantic)] = formula;
        }
    }
}
