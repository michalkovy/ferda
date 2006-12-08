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
    /// Cache for relational DM is separate, as getting bitstrings from sub-miners
    /// is different: sub-miner provides no particular bitstring with specified
    /// attribute and category id, but "next" bitstring instead
    /// This cache will hold first N bitstrings from one subminer run
    /// </summary>
    public class BitStringBuffer
    {
        private const int maxSize = 65536;
        private static readonly BitStringBuffer _instance = new BitStringBuffer();
        private static List<BitStringIceWithCategoryId> _buffer =
            new List<BitStringIceWithCategoryId>();

        private List<string> _guids =
            new List<string>();

        public int Count
        {
            get
            {
                return _buffer.Count;
            }
        }

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

        public bool AddBitString(BitStringIceWithCategoryId bitString)
        {
            if (_instance.Count < maxSize)
            {
                _buffer.Add(bitString);
                return true;
            }
            return false;
        }

        public BitStringIceWithCategoryId GetBitString(int index)
        {
            return _buffer[index];
        }

        public void Reset()
        {
            _guids.Clear();
            _buffer.Clear();
        }
    }
}
