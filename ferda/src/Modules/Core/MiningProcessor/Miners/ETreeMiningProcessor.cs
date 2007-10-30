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
using Ferda.ModulesManager;

namespace Ferda.Guha.MiningProcessor.Miners
{
    /// <summary>
    /// The ETree procedure mining processor
    /// </summary>
    public class ETreeMiningProcessor : ProgressBarHandler
    {
        #region Private fields

        /// <summary>
        /// Minimal node impurity (algorithm parameter). Minimal node impurity is
        /// a condition for stopping growth of a tree. When sufficient amount 
        /// (determined by this parameter) of cases (items) belongs to one classification
        /// class in one node, the three is returned in output and stops growing. 
        /// </summary>
        private int minimalNodeImpurity;

        /// <summary>
        /// Minimal node frequency (algorithm parameter). Minimal node frequency is
        /// a condition for stopping growth of a tree. When a node does not contain
        /// minimal number of items (determined by this parameter), the three is returned
        /// in output and stops growing. 
        /// </summary>
        private int minimalNodeFrequency;

        /// <summary>
        /// Maximal tree depth (algorithm parameter). The total depth of the tree
        /// cannot exceed this value. 
        /// </summary>
        private int maximalTreeDepth;

        /// <summary>
        /// Number of attributes for branching (algorithm parameter). When determining
        /// the most suitable for tree branching, the remaining attributes are sorted
        /// by a criterion (here, chi squared) and the best N(determined by this 
        /// parameter) are used for branching.
        /// </summary>
        private int noAttributesForBranching;

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

        /// <summary>
        /// Default constructor of the class.
        /// </summary>
        /// <param name="branchingAttributes">
        /// Branching attributes (algorithm parameter)
        /// </param>
        /// <param name="targetClassificationAttribute">
        /// Target classification attribute (algorithm parameter)
        /// </param>
        /// <param name="quantifiers">
        /// Quatifiers to evaluate the tree quality 
        /// (algorithm parameter)
        /// </param>
        /// <param name="minimalNodeImpurity">
        /// Minimal node impurity (algorithm parameter). Minimal node impurity is
        /// a condition for stopping growth of a tree. When sufficient amount 
        /// (determined by this parameter) of cases (items) belongs to one classification
        /// class in one node, the three is returned in output and stops growing. 
        /// </param>
        /// <param name="minimalNodeFrequency">
        /// Minimal node frequency (algorithm parameter). Minimal node frequency is
        /// a condition for stopping growth of a tree. When a node does not contain
        /// minimal number of items (determined by this parameter), the three is returned
        /// in output and stops growing. 
        /// </param>
        /// <param name="maximalTreeDepth">
        /// Maximal tree depth (algorithm parameter). The total depth of the tree
        /// cannot exceed this value. 
        /// </param>
        /// <param name="noAttributesForBranching">
        /// Number of attributes for branching (algorithm parameter). When determining
        /// the most suitable for tree branching, the remaining attributes are sorted
        /// by a criterion (here, chi squared) and the best N(determined by this 
        /// parameter) are used for branching.
        /// </param>
        /// <param name="progressListener">The progress listener.</param>
        /// <param name="progressBarPrx">The progress bar PRX.</param>
        public ETreeMiningProcessor(
            CategorialAttribute[] branchingAttributes,
            CategorialAttribute targetClassificationAttribute,
            QuantifierBaseFunctionsPrx[] quantifiers,
            int minimalNodeImpurity,
            int minimalNodeFrequency,
            int maximalTreeDepth,
            int noAttributesForBranching,
            ProgressTaskListener progressListener,
            ProgressBarPrx progressBarPrx) : base(progressListener, progressBarPrx)
        {
            this.branchingAttributes = branchingAttributes;
            this.targetClassificationAttribute = targetClassificationAttribute;
            this.quantifiers = quantifiers;

            //checking corectness of the input parameters
            if (minimalNodeFrequency < 1)
            {
                throw Exceptions.NotMoreThanZeroException("MinimalNodeFrequency");
            }
            else
            {
                this.minimalNodeFrequency = minimalNodeFrequency;
            }

            if (minimalNodeImpurity < 1)
            {
                throw Exceptions.NotMoreThanZeroException("MinimalNodeImpurity");
            }
            else
            {
                this.minimalNodeImpurity = minimalNodeImpurity;

            }

            if (maximalTreeDepth < 1)
            {
                throw Exceptions.NotMoreThanZeroException("MaximalTreeDepth");
            }
            else
            {
                this.maximalTreeDepth = maximalTreeDepth;
            }

            if (noAttributesForBranching < 1)
            {
                throw Exceptions.NotMoreThanZeroException("MaximalTreeDepth");
            }
            else
            {
                this.noAttributesForBranching = noAttributesForBranching;
            }
        }

        #endregion
    }
}
