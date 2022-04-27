// ISkipOptimalization.cs - skipping steps optimalization interface
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
using System.Text;
using Ferda.Guha.Math.Quantifiers;

namespace Ferda.Guha.MiningProcessor.Generation
{
    /// <summary>
    /// Skipping steps optimalization setting. This setting shows,
    /// when to skip optimalization (when value is in relation with
    /// given treshold).
    /// </summary>
    public class SkipSetting
    {
        /// <summary>
        /// Default constructor of the class
        /// </summary>
        /// <param name="relation">Relation setting</param>
        /// <param name="treshold">Treshold setting</param>
        /// <param name="notTreshold">Not treshold setting</param>
        public SkipSetting(Math.RelationEnum relation, double treshold, double notTreshold)
        {
            _relation = relation;
            _treshold = treshold;
            _notTreshold = notTreshold;
        }
        
        /// <summary>
        /// Relation for given threshold
        /// </summary>
        private Math.RelationEnum _relation;
        /// <summary>
        /// Relation for given threshold
        /// </summary>
        public Math.RelationEnum Relation
        {
            get { return _relation; }
        }

        /// <summary>
        /// Threshold
        /// </summary>        
        private double _treshold;
        /// <summary>
        /// Threshold
        /// </summary>
        public double Treshold
        {
            get { return _treshold; }
        }

        /// <summary>
        /// Gets the not treshold. Equals to <c>all objects count - Treshold</c>
        /// </summary>
        /// <value>The not treshold.</value>
        private double _notTreshold;
        /// <summary>
        /// Gets the not treshold. Equals to <c>all objects count - Treshold</c>
        /// </summary>
        /// <value>The not treshold.</value>
        public double NotTreshold
        {
            get { return _notTreshold; }
        }
    }
    
    /// <summary>
    /// Skipping steps optimalization interface. Currently only the base skipping
    /// optimalizations (some branches of the computational trees are not
    /// searched when the base (support) of the root of the branch is
    /// below certain limit.
    /// </summary>
    public interface ISkipOptimalization
    {
        /// <summary>
        /// The base skip otrimalization settng
        /// </summary>
        /// <param name="cedentType">Type of the cedent</param>
        /// <returns>The base skip optimalization setting for the cedent</returns>
        SkipSetting BaseSkipSetting(MarkEnum cedentType);
    }
}
