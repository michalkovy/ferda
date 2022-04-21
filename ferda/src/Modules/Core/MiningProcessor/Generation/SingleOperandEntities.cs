// SingleOperandEntities.cs - single operand entities enumerators
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

using System.Collections.Generic;
using Ferda.Guha.Math;
using Ferda.Guha.MiningProcessor.BitStrings;

namespace Ferda.Guha.MiningProcessor.Generation
{
    /// <summary>
    /// Negation entity enumerator
    /// </summary>
    public class Negation : SingleOperandEntity
    {
        /// <summary>
        /// Default constructor of the class
        /// </summary>
        /// <param name="setting">Negation setting</param>
        /// <param name="skipOptimalization">Skip step optimalization</param>
        /// <param name="cedentType">Type of the cedent</param>
        public Negation(NegationSetting setting, ISkipOptimalization skipOptimalization, MarkEnum cedentType)
            : base(setting, skipOptimalization, cedentType)
        {
        }

        /// <summary>
        /// Retrieves the entity enumerator. Returns negations of original bit strings
        /// </summary>
        /// <returns>Entity enumerator</returns>
        public override async IAsyncEnumerator<IBitString> GetBitStringEnumerator()
        {
            await foreach (IBitString bitString in _entity)
            {
                yield return bitString.Not();
            }
        }

        /// <summary>
        /// Total number of bit strings in this enumerator - for negation setting
        /// it is the number of bit strings of the operand enumerator
        /// </summary>
        public override long TotalCount
        {
            get { return _entity.TotalCount; }
        }

        /// <summary>
        /// Returns text representation of the negation enumerator, which is in
        /// form <c>Negation of operand</c>
        /// </summary>
        /// <returns>Text representation of the negation</returns>
        public override string ToString()
        {
            return "Negation of <" + _entity.ToString() + ">";
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
            RelationEnum newRelation;
            switch (parentSkipSetting.Relation)
            {
                case RelationEnum.Equal:
                    newRelation = RelationEnum.Equal;
                    break;
                case RelationEnum.Greater:
                    newRelation = RelationEnum.LessOrEqual;
                    break;
                case RelationEnum.GreaterOrEqual:
                    newRelation = RelationEnum.Less;
                    break;
                case RelationEnum.Less:
                    newRelation = RelationEnum.GreaterOrEqual;
                    break;
                case RelationEnum.LessOrEqual:
                    newRelation = RelationEnum.Greater;
                    break;
                default:
                    throw new System.NotImplementedException();
            }
            return new SkipSetting(newRelation, parentSkipSetting.NotTreshold, parentSkipSetting.Treshold);
        }
    }
    
    /// <summary>
    /// Both signs entity enumerator (positive and neagtive)
    /// </summary>
    public class BothSigns : SingleOperandEntity
    {
        /// <summary>
        /// Default constructor of the class
        /// </summary>
        /// <param name="setting">Both signs setting setting</param>
        /// <param name="skipOptimalization">Skip step optimalization</param>
        /// <param name="cedentType">Type of the cedent</param>
        public BothSigns(BothSignsSetting setting, ISkipOptimalization skipOptimalization, MarkEnum cedentType)
            : base(setting, skipOptimalization, cedentType)
        {
        }

        /// <summary>
        /// Retrieves the entity enumerator. Returns original bit strings from the operand
        /// and new bit strings that are negation of original bit strings.
        /// </summary>
        /// <returns>Entity enumerator</returns>
        public override async IAsyncEnumerator<IBitString> GetBitStringEnumerator()
        {
            await foreach (IBitString bitString in _entity)
            {
                SkipSetting skipSetting = ParentSkipOptimalization.BaseSkipSetting(CedentType);
                if (skipSetting == null)
                {
                    yield return bitString;
                    yield return bitString.Not();
                }
                else
                {
                    if (Ferda.Guha.Math.Common.Compare(skipSetting.Relation, bitString.Sum, skipSetting.Treshold))
                        yield return bitString;
                    IBitString negation = bitString.Not();
                    if (Ferda.Guha.Math.Common.Compare(skipSetting.Relation, negation.Sum, skipSetting.Treshold))
                        yield return negation;
                }
            }
        }

        /// <summary>
        /// Total number of bit strings in this enumerator - for both signs setting
        /// it is the number of bit strings of the operand enumerator * 2
        /// </summary>
        public override long TotalCount
        {
            get { return _entity.TotalCount * 2; }
        }

        /// <summary>
        /// Returns text representation of the negation enumerator, which is in
        /// form <c>Both signs of operand</c>
        /// </summary>
        /// <returns>Text representation of the both signs</returns>
        public override string ToString()
        {
            return "Both signs of <" + _entity.ToString() + ">";
        }

        /// <summary>
        /// The base skip otrimalization setting
        /// </summary>
        /// <param name="cedentType">Type of the cedent</param>
        /// <returns>The base skip optimalization setting for the cedent</returns>
        public override SkipSetting BaseSkipSetting(MarkEnum cedentType)
        {
            // UNDONE
            return null;
        }
    }
}