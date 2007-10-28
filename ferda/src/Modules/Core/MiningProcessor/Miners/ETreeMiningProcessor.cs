// ETreeMiningProcessor.cs - mining processor for the ETree procedure
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
using Ferda.Guha.MiningProcessor;
using Ferda.Guha.Math.Quantifiers;

namespace Ferda.Guha.MiningProcessor.Miners
{
    /// <summary>
    /// The ETree procedure mining processor
    /// </summary>
    public class ETreeMiningProcessor
    {
        #region Private fields

        /// <summary>
        /// Minimal node impurity (algorithm parameter)
        /// </summary>
        private int minimalNodeImpurity;

        /// <summary>
        /// Minimal node frequency (algorithm parameter)
        /// </summary>
        private int minimalNodeFrequency;

        /// <summary>
        /// Maximal tree depth (algorithm parameter)
        /// </summary>
        private int maximalTreeDepth;

        /// <summary>
        /// Branching attributes (algorithm parameter)
        /// </summary>
        private CategorialAttribute[] branchingAttributes;

        /// <summary>
        /// Target classification attribute (algorithm parameter)
        /// </summary>
        private CategorialAttribute targetClassificationAttribute;

        /// <summary>
        /// Quatifiers to evaluate the tree quality 
        /// (algorithm parameter)
        /// </summary>
        private QuantifierBaseFunctionsPrx[] quantifiers;

        #endregion

        #region Constructor

        public ETreeMiningProcessor(
            CategorialAttribute[] branchingAttributes,
            CategorialAttribute targetClassificationAttribute,
            QuantifierBaseFunctionsPrx[] quantifiers,
            int minimalNodeImpurity,
            int minimalNodeFrequency,
            int maximalTreeDepth,
            int noBranchingCategories)
        {
            this.branchingAttributes = branchingAttributes;
            this.targetClassificationAttribute = targetClassificationAttribute;
            this.quantifiers = quantifiers;

            if (minimalNodeFrequency < 0)
            {
                throw Exceptions.NotMoreThanZeroException("MinimalNodeFrequency");
            }
            else
            {
                this.minimalNodeFrequency = minimalNodeFrequency;
            }

        }


        #endregion
    }
}
