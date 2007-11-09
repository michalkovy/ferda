// NodeClassification.cs - structure for classification of a node
// category
//
// Authors: Martin Ralbovský <martin.ralbovsky@gmail.com>
//
// Copyright (c) 2007 Martin Ralbovský 
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
using Ferda.Guha.MiningProcessor.BitStrings;

namespace Ferda.Guha.MiningProcessor.DecisionTrees
{
    /// <summary>
    /// Structure for classification of a node category. It does not classify
    /// the node as a whole, but its individual categories.
    /// </summary>
    public class NodeClassification 
    {
        /// <summary>
        /// Number of true positive examples
        /// (examples that are true and positively classified)
        /// </summary>
        public long truePositive = -1;

        /// <summary>
        /// Number of true negative examples
        /// (examples that are true and negatively classified)
        /// </summary>
        public long trueNegative = -1;

        /// <summary>
        /// Number of false positive examples
        /// (examples that are false and positively classified)
        /// </summary>
        public long falsePositive = -1;

        /// <summary>
        /// Number of false negative examples
        /// (examples that are true and positively classified)
        /// </summary>
        public long falseNegative = -1;

        /// <summary>
        /// Classification category - which classification category belongs
        /// to this (normal) category
        /// </summary>
        public string classificationCategory = string.Empty;

        /// <summary>
        /// Initializes the integer variables of this structure - 
        /// true positive etc... 
        /// </summary>
        /// <param name="categoryBitString">Bit string of the normal category</param>
        /// <param name="classificationBitString">Bit string of the classification category</param>
        public void Init(IBitString categoryBitString, 
            IBitString classificationBitString)
        {
            truePositive = 
                classificationBitString.And(categoryBitString).Sum;
            trueNegative = 
                classificationBitString.And(categoryBitString.Not()).Sum;
            falsePositive = 
                classificationBitString.Not().And(categoryBitString).Sum;
            falseNegative = 
                classificationBitString.Not().And(categoryBitString.Not()).Sum;
        }
    }
}
