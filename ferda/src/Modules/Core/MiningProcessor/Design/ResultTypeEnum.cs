// ResultTypeEnum.cs - 
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

namespace Ferda.Guha.MiningProcessor.Design
{
    /// <summary>
    /// The type of result that is returned from the GUHA procedure
    /// (miner).
    /// </summary>
    public enum ResultTypeEnum
    {
        /// <summary>
        /// Only valid hypotheses are returned
        /// </summary>
	    Trace,
        /// <summary>
        /// All the verifications, validity of the verification on
        /// individual objects can be true or false
        /// </summary>
	    TraceBoolean,
        /// <summary>
        /// All the verifications, validity of the verificatoin
        /// on individual objects can be a value from closed interval
        /// 0 to 1.
        /// </summary>
	    TraceReal
    }
}
