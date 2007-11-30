// BranchingStoppingCriterionEnum.cs - criterion for stopping of branching of a node
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

namespace Ferda.Guha.MiningProcessor.Design
{
    /// <summary>
    /// Defines criterion for stopping of branching of a node
    /// in the ETree mining procedure.
    /// </summary>
    public enum BranchingStoppingCriterionEnum
    {
        /// <summary>
        /// Only minimal node purity is taken into account
        /// </summary>
        MinimalNodePurity,

        /// <summary>
        /// Only minimal node frequency is taken into accoung
        /// </summary>
        MinimalNodeFrequency,

        /// <summary>
        /// The node should fulfill at least one of the criterions:
        /// minimal node frequency or minimal node purity
        /// </summary>
        MinimalNodePurityORMinimalNodeFrequency
    }
}
