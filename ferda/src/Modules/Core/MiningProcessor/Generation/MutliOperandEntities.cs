// MultiOperandEntities.cs - Multi operand entities enumerators
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

using System;
using System.Collections.Generic;
using Ferda.Guha.MiningProcessor.BitStrings;

namespace Ferda.Guha.MiningProcessor.Generation
{
    /// <summary>
    /// Conjunction entity enumerator
    /// </summary>
    public class Conjunction : MutliOperandEntity
    {
        /// <summary>
        /// Default constructor for the class
        /// </summary>
        /// <param name="setting">The setting of this enumerator</param>
        /// <param name="skipOptimalization">The skip optimalization.</param>
        /// <param name="cedentType">Type of the cedent.</param>
        public Conjunction(IMultipleOperandEntitySetting setting, ISkipOptimalization skipOptimalization, MarkEnum cedentType)
            : base(setting, skipOptimalization, cedentType)
        {
        }

        /// <summary>
        /// The operation performed with entities - conjunction
        /// </summary>
        /// <param name="operand1">First operand</param>
        /// <param name="operand2">Second operand</param>
        /// <returns>Conjunction of two bit strings</returns>
        protected override IBitString operation(IBitString operand1, IBitString operand2)
        {
            if (operand1 == null)
                throw new ArgumentNullException("operand1");
            if (operand2 == null)
                throw new ArgumentNullException("operand2");
            return operand1.And(operand2);
        }

        /// <summary>
        /// Returns text representation of the subsets coefficient enumerator, 
        /// which is in form <c>Conjunction of {operand1, operand2...}</c>
        /// </summary>
        /// <returns>Text representation of the conjunction</returns>
        public override string ToString()
        {
            List<string> entities = new List<string>();
            foreach (IEntityEnumerator entity in _sourceEntities)
                entities.Add(entity.ToString());
            return "Conjunction of {" 
                   + Formulas.FormulaHelper.SequenceToString(entities, Formulas.FormulaSeparator.AtomMembers, true) 
                   + "}";
        }

        /// <summary>
        /// The base skip otrimalization setting
        /// </summary>
        /// <param name="cedentType">Type of the cedent</param>
        /// <returns>The base skip optimalization setting for the cedent</returns>
        public override SkipSetting BaseSkipSetting(MarkEnum cedentType)
        {
            SkipSetting parentSkipSetting = ParentSkipOptimalization.BaseSkipSetting(cedentType);
            if (parentSkipSetting == null)
                return null;
            // UNDONE (this is OK for >  or >= relations)
            if (Ferda.Guha.Math.Common.GetRelationOrientation(parentSkipSetting.Relation) > 0)
                return ParentSkipOptimalization.BaseSkipSetting(cedentType);
            else
                return null;
        }
    }

    /// <summary>
    /// Disjunction entity enumerator
    /// </summary>
    public class Disjunction : MutliOperandEntity
    {
        /// <summary>
        /// Default constructor for the class
        /// </summary>
        /// <param name="setting">The setting of this enumerator</param>
        /// <param name="skipOptimalization">The skip optimalization.</param>
        /// <param name="cedentType">Type of the cedent.</param>
        public Disjunction(IMultipleOperandEntitySetting setting, ISkipOptimalization skipOptimalization, MarkEnum cedentType)
            : base(setting, skipOptimalization, cedentType)
        {
        }

        /// <summary>
        /// The operation performed with entities - disjunction
        /// </summary>
        /// <param name="operand1">First operand</param>
        /// <param name="operand2">Second operand</param>
        /// <returns>Disjunction of two bit strings</returns>
        protected override IBitString operation(IBitString operand1, IBitString operand2)
        {
            if (operand1 == null)
                throw new ArgumentNullException("operand1");
            if (operand2 == null)
                throw new ArgumentNullException("operand2");
            return operand1.Or(operand2);
        }

        /// <summary>
        /// Returns text representation of the subsets coefficient enumerator, 
        /// which is in form <c>Disjunction of {operand1, operand2...}</c>
        /// </summary>
        /// <returns>Text representation of the disjunction</returns>
        public override string ToString()
        {
            List<string> entities = new List<string>();
            foreach (IEntityEnumerator entity in _sourceEntities)
                entities.Add(entity.ToString());
            return "Disjunction of {"
                   + Formulas.FormulaHelper.SequenceToString(entities, Formulas.FormulaSeparator.AtomMembers, true)
                   + "}";
        }

        /// <summary>
        /// The base skip otrimalization setting - nothing for disjunctions
        /// </summary>
        /// <param name="cedentType">Type of the cedent</param>
        /// <returns>The base skip optimalization setting for the cedent</returns>
        public override SkipSetting BaseSkipSetting(MarkEnum cedentType)
        {
            // UNDONE
            // no base skip optimalization for disjunctions
            return null;
        }
    }
}