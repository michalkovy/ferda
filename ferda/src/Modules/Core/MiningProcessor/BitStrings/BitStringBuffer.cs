// BitStringBuffer.cs - Buffer of bit strings for the relational DM tasks
//
// Author: Alexander Kuzmin <alexander.kuzmin@gmail.com>
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
using System.Collections;
using System.Text;
using Ferda.Guha.MiningProcessor;
using System.Runtime.InteropServices;
//using Ferda.Guha.MiningProcessor.BitStrings;

namespace Ferda.Guha.MiningProcessor.BitStrings
{
    /// <summary>
    /// This is buffer for getting bit string from the Ice layer 
    /// <see cref="Ferda.Guha.MiningProcessor.BitStringIceWithCategoryId"/>
    /// (not to be confused with the <see cref="BitStringCache"/>, that 
    /// caches bit strings <see cref="IBitString"/> inside the mining processor.
    /// It is used also in relational DM.
    /// </summary>
    /// <remarks>
    /// Cache for relational DM is separate, as getting bitstrings from sub-miners
    /// is different: sub-miner provides no particular bitstring with specified
    /// attribute and category id, but "next" bitstring instead
    /// This cache will hold first N bitstrings from one subminer run
    /// </remarks>
    public class BitStringBuffer
    {
        private const int maxSize = 1000;
        //private const int maxSize = 0;
        private static readonly BitStringBuffer _instance = new BitStringBuffer();
        private static List<BitStringIceWithCategoryId> _buffer =
            new List<BitStringIceWithCategoryId>();

        private List<string> _guids =
            new List<string>();

        /// <summary>
        /// Count of currently buffered strings
        /// </summary>
        public int Count
        {
            get
            {
                return _buffer.Count;
            }
        }

        /// <summary>
        /// Checks for cached attribute using its guid
        /// </summary>
        /// <param name="guid">Guid of checked attribute</param>
        /// <returns>True if guid present</returns>
        public bool GuidPresent(string guid)
        {
            return _guids.Contains(guid);
        }

        public void AddGuid(string guid)
        {
            if (!_guids.Contains(guid))
            {
                _guids.Add(guid);
            }
        }
            
        
        private BitStringBuffer()
        { }

        public static BitStringBuffer GetInstance()
        {
            return _instance;
        }

        /// <summary>
        /// Adds bitstring to buffer
        /// </summary>
        /// <param name="bitString">bitstring to add</param>
        /// <returns>True if successful</returns>
        public bool AddBitString(BitStringIceWithCategoryId bitString)
        {
            if (_instance.Count < maxSize)
            {
                _buffer.Add(bitString);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Gets the bitstring at the specified index
        /// </summary>
        /// <param name="index">Index to get bitstring at</param>
        /// <returns>Bitstring</returns>
        public BitStringIceWithCategoryId GetBitString(int index)
        {
            return _buffer[index];
        }

        /// <summary>
        /// Resets the buffer
        /// </summary>
        public void Reset()
        {
            _guids.Clear();
            _buffer.Clear();
        }
    }
}
