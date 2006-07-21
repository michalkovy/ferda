//#define Testing

#define USE64BIT
#define LOOKUP8
#define LOOKUP16
#define UNSAFE
#define UNITTESTING

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Text;
using Ferda.Guha.MiningProcessor.Formulas;

namespace Ferda.Guha.MiningProcessor.BitStrings
{
    /// <summary>
    /// This class represents a bit string of a fixed length.
    /// </summary>
    public partial class BitString : IBitString, IBitStringCreate, IBitStringBase
    {
        #region Fields

#if LOOKUP8
        private static Byte[] _lookup8;
#endif
#if LOOKUP16
        private static Byte[] _lookup16;
#endif

        static BitString()
        {
#if LOOKUP8
            _lookup8 = new Byte[256];
            for (int i = 0; i < 256; i++)
            {
                int sum = i;
                sum = (sum & 0x55) + ((sum >> 1) & 0x55);
                sum = (sum & 0x33) + ((sum >> 2) & 0x33);
                sum = (sum & 0x0F) + ((sum >> 4) & 0x0F);
                _lookup8[i] = (Byte)sum;
            }
#endif

#if LOOKUP16
            _lookup16 = new Byte[65536];
            for (int i = 0; i < 65536; i++)
            {
                int sum = i;
                sum = (sum & 0x5555) + ((sum >> 1) & 0x5555);
                sum = (sum & 0x3333) + ((sum >> 2) & 0x3333);
                sum = (sum & 0x0F0F) + ((sum >> 4) & 0x0F0F);
                sum = (sum & 0x00FF) + ((sum >> 8) & 0x00FF);
                _lookup16[i] = (Byte)sum;
            }
#endif
        }


        private int _size;
        private int _sum = -1;

        private Formulas.BooleanAttributeFormula _identifier;

        public Formulas.BooleanAttributeFormula Identifier
        {
            get { return _identifier; }
        }

#if USE64BIT
        private const int _blockSize = 64;
        private const ulong _zero = 0ul;
        private const ulong _one = 1ul;
        private const ulong _8bits = 0x00000000000000FFul;
        private const ulong _16bits = 0x000000000000FFFFul;
        private const ulong _allOnes = 0xFFFFFFFFFFFFFFFFul;
        private ulong[] _array;
#else
        const int _blockSize = 32;
        const uint _zero = 0u;
        const uint _one = 1u;
        const uint _8bits = 0x000000FFu;
        const uint _16bits = 0x0000FFFFu;
        const uint _allOnes = 0xFFFFFFFFu;
        private uint [] _array;
#endif

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor that allocates the memory.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        private BitString(BooleanAttributeFormula identifier)
        {
            _identifier = identifier;
        }

        public BitString(int length, BitStringIdentifier identifier, long[] bits)
            : this(new AtomFormula(identifier))
        {
            if (length <= 0)
                throw Exceptions.BitStringLengthError();

            _size = length;

            int arraySize = (length + _blockSize - 1) / _blockSize; // rounding up...

#if USE64BIT
            if (arraySize != bits.Length)
                throw new ArgumentOutOfRangeException("bits", "The array of bits has bad size (Length).");

            _array = new ulong[arraySize];
            for (int i = 0; i < bits.Length; i++)
            {
                unchecked
                {
                    _array[i] = (ulong)bits[i];
                }
            }

#else
            throw new NotImplementedException();
#endif
        }

        /// <summary>
        /// Copy constructor.
        /// </summary>
        /// <param name="source">Source BitString that will be copied.</param>
        public BitString(BitString source)
            : this(source.Identifier)
        {
            if (source._size == 0)
                throw new InvalidOperationException("Cannot copy-construct a BitString from an uninitialized one.");

            _size = source._size;
            _sum = source._sum;

#if USE64BIT
            _array = new ulong[source._array.Length];
#else
            _array = new uint[source._array.Length];
#endif

#if UNSAFE
            copyUnsafe(source);
#else
            copySafe(source);
#endif
        }

#if Testing
        /// <summary>
        /// Initialize bit string from a string of characters '0' and '1'.
        /// </summary>
        /// <param name="source">Initialization string, which can contain only characters '0' and '1'.</param>
        /// <param name="identifier">The identifier.</param>
        /// <exception cref="NullReferenceException">Input string cannot be a null reference..</exception>
        /// <exception cref="ArgumentException">Input string can contain only characters '0' and '1'.</exception>
        public BitString(string source, AtomFormula identifier)
            : this(identifier)
        {
            create(source.Length);
            for (int i = 0; i < source.Length; i++)
            {
                switch (source[i])
                {
                    case '0':
                        // nothing...
                        break;

                    case '1':
                        _array[i / _blockSize] |= (_one << (i % _blockSize));
                        _sum++;
                        break;

                    default:
                        throw new ArgumentException("Source string can contain only characters '0' and '1'.");
                }
            }
        }
#endif

        /// <summary>
        /// This method allocates the memory of BitString.
        /// Call this method only after the default constructor was used.
        /// Cannot be called more than once.
        /// </summary>
        /// <param name="length">The length of BitString (the number of bits).</param>
        private void create(int length)
        {
            if (length <= 0)
                throw Exceptions.BitStringLengthError();
            if (_size > 0)
                throw new InvalidOperationException("BitString cannot be initialized more than once.");

            _size = length;
            _sum = 0;
            int arraySize = (length + _blockSize - 1) / _blockSize; // rounding up...

#if USE64BIT
            _array = new ulong[arraySize];
#else
            _array = new uint[arraySize];
#endif
        }

#if UNSAFE
        private unsafe void copyUnsafe(BitString source)
        {
#if USE64BIT
            fixed (ulong* destPin = _array, sourcePin = source._array)
            {
                ulong* destPtr = destPin, sourcePtr = sourcePin, stopPtr = destPin + _array.Length;
                while (destPtr < stopPtr)
                {
                    *destPtr++ = *sourcePtr++;
                }
            }
#else
            fixed (uint *destPin = _array, sourcePin = source._array)
            {
                uint *destPtr = destPin, sourcePtr = sourcePin, stopPtr = destPin + _array.Length;
                while (destPtr < stopPtr)
                {
                    *destPtr++ = *sourcePtr++;
                }
            }
#endif
        }
#else
        private void copySafe(BitString source)
        {
            for (int i = 0; i < _array.Length; i++)
            {
                _array[i] = source._array[i];
            }
        }
#endif

        #endregion

        private static void testSizes(BitString source1, BitString source2)
        {
            if (source1._size == 0)
                throw new InvalidOperationException("BitString was not initialized (use create method first).");
            if (source1._size != source2._size)
                throw Exceptions.BitStringsLengtsAreNotEqualError();

            Debug.Assert(source1._array.Length == source2._array.Length,
                         "The array sizes don't match, although bit string lengths are the same.");
            Debug.Assert(
                (source1._size % _blockSize == 0) || ((source1._array[source1._array.Length - 1] & (_allOnes << source1._size % _blockSize)) == 0),
                "The bit string contains non-zero bits in the last block behind the allowed length.");
            Debug.Assert(
                (source1._size % _blockSize == 0) || ((source2._array[source1._array.Length - 1] & (_allOnes << source1._size % _blockSize)) == 0),
                "The bit string contains non-zero bits in the last block behind the allowed length.");
        }
        
        #region AND

        public IBitString And(IBitString source)
        {
            BitString bsSource = source as BitString;
            if (bsSource != null)
            {
                testSizes(this, bsSource);
#if UNSAFE
                andUnsafe(bsSource);
#else
                andSafe(bsSource);
#endif
                _sum = -1;
                _identifier = FormulaHelper.And(Identifier, source.Identifier);
                return this;
            }
            else if (source is EmptyBitString)
            {
                return this;
            }
            else if (source is FalseBitString)
            {
                return source;
            }
            else
                throw new NotImplementedException();
        }
        
        public IBitString AndCloned(IBitString source)
        {
            BitString bsSource = source as BitString;
            if (bsSource != null)
            {
                BitString result = new BitString(FormulaHelper.And(Identifier, bsSource.Identifier));
                if (_size == 0)
                    throw new InvalidOperationException("Cannot copy-construct a BitString from an uninitialized one.");
                result._size = _size;
                result._sum = -1;

                testSizes(this, bsSource);
#if UNSAFE
                result._array = andUnsafe(_array, bsSource._array);
#else
                result._array = andSafe(_array, bsSource._array);
#endif
                return result;
            }
            else if (source is EmptyBitString)
            {
                return new BitString(this);
            }
            else if (source is FalseBitString)
            {
                return source;
            }
            else
                throw new NotImplementedException();
        }

        #endregion

        #region OR

        public IBitString Or(IBitString source)
        {
            BitString bsSource = source as BitString;
            if (bsSource != null)
            {
                testSizes(this, bsSource);
#if UNSAFE
                orUnsafe(bsSource);
#else
                orSafe(bsSource);
#endif
                _sum = -1;
                _identifier = FormulaHelper.Or(Identifier, source.Identifier);
                return this;
            }
            else if (source is EmptyBitString)
            {
                return this;
            }
            else if (source is FalseBitString)
            {
                return this;
            }
            else
                throw new NotImplementedException();
        }

        public IBitString OrCloned(IBitString source)
        {
            BitString bsSource = source as BitString;
            if (bsSource != null)
            {
                BitString result = new BitString(FormulaHelper.Or(Identifier, bsSource.Identifier));
                if (_size == 0)
                    throw new InvalidOperationException("Cannot copy-construct a BitString from an uninitialized one.");
                result._size = _size;
                result._sum = -1;

                testSizes(this, bsSource);
#if UNSAFE
                result._array = orUnsafe(_array, bsSource._array);
#else
                result._array = orSafe(_array, bsSource._array);
#endif
                return result;
            }
            else if (source is EmptyBitString)
            {
                return new BitString(this);
            }
            else if (source is FalseBitString)
            {
                return new BitString(this);
            }
            else
                throw new NotImplementedException();
        }
        
        #endregion

        #region NOT

        public IBitString Not()
        {
#if UNSAFE
            notUnsafe();
#else
            notSafe();
#endif
            if (_sum >= 0)
                _sum = _size - _sum;
            _identifier = FormulaHelper.Not(Identifier);
            return this;
        }

        public IBitString NotCloned()
        {
            BitString result = new BitString(FormulaHelper.Or(Identifier, Identifier));
            if (_size == 0)
                throw new InvalidOperationException("Cannot copy-construct a BitString from an uninitialized one.");
#if UNSAFE
            result._array = notUnsafe(_array, _size);
#else
            result._array = notSafe(_array, _size);
#endif
            result.not();
            result._identifier = FormulaHelper.Not(Identifier);
            return result;
        }

        protected void not()
        {
            if (_size == 0)
                throw new InvalidOperationException("BitString was not initialized (use create method first).");

            Debug.Assert(
                (_size % _blockSize == 0) || ((_array[_array.Length - 1] & (_allOnes << _size % _blockSize)) == 0),
                "The bit string contains non-zero bits in the last block behind the allowed length.");

#if UNSAFE
            notUnsafe();
#else
            notSafe();
#endif

            if (_sum >= 0)
                _sum = _size - _sum;
        }



        #endregion

        #region SUM

        public int Sum
        {
            get
            {
                if (_size == 0)
                    throw new InvalidOperationException("BitString was not initialized (use create method first).");

                Debug.Assert(_sum >= -1, "The sum must be -1 (i.e. \"unknown\") or non-negative.");
                Debug.Assert(_sum <= _size, "The sum must be less than or equal to the size of the bit string.");
                Debug.Assert(
                    (_size % _blockSize == 0) || ((_array[_array.Length - 1] & (_allOnes << _size % _blockSize)) == 0),
                    "The bit string contains non-zero bits in the last block behind the allowed length.");

                if (_sum >= 0)
                    return _sum;

                // compute the sum using the best available method
                // (looping, folding, 8-bit lookup, 16-bit lookup or sparse sum)
#if UNSAFE
                sumLookup16Unsafe();
#else
                sumLookup16Safe();
#endif

                Debug.Assert(_sum >= 0, "The sum must be non-negative.");
                Debug.Assert(_sum <= _size, "The sum must be less than or equal to the size of the bit string.");

                return _sum;
            }
        }

#if UNSAFE
        private unsafe void sumLoopUnsafe()
        {
            _sum = 0;

            unchecked
            {
#if USE64BIT
                fixed (ulong* arrayPtr = _array)
                {
                    ulong current;
                    ulong* currentPtr = arrayPtr, stopPtr = arrayPtr + _array.Length - 1;
#else
                fixed (uint *arrayPtr = _array)
                {
                    uint current;
                    uint *currentPtr = arrayPtr, stopPtr = arrayPtr + _array.Length - 1;
#endif


                    while (currentPtr < stopPtr)
                    {
                        current = *currentPtr++;
                        for (int j = _blockSize - 1; j > 0; j--)
                        {
                            _sum += (int)(current & _one);
                            current >>= 1;
                        }
                        _sum += (int)current;
                    }

                    // sum the last block
                    current = *currentPtr;
                    for (int j = (_size - 1) % _blockSize; j > 0; j--)
                    {
                        _sum += (int)(current & _one);
                        current >>= 1;
                    }
                    _sum += (int)(current & _one);
                }
            }
        }
#else
        private void sumLoopSafe()
        {
#if USE64BIT
            ulong current;
#else
            uint current;
#endif

            _sum = 0;
            int i;
            unchecked
            {
                // compute all but the last block
                for (i = 0; i < _array.Length - 1; i++)
                {
                    current = _array[i];
                    for (int j = _blockSize - 1; j > 0; j--)
                    {
                        _sum += (int)(current & _one);
                        current >>= 1;
                    }
                    _sum += (int)current;
                }
                // add the last block
                current = _array[i];
                for (int j = (_size - 1) % _blockSize; j > 0; j--)
                {
                    _sum += (int)(current & _one);
                    current >>= 1;
                }
                _sum += (int)(current & _one);
            }
        }
#endif

#if UNSAFE
        private unsafe void sumFoldUnsafe()
        {
            _sum = 0;

            unchecked
            {
#if USE64BIT
                fixed (ulong* arrayPtr = _array)
                {
                    ulong* currentPtr = arrayPtr, stopPtr = arrayPtr + _array.Length;
#else
                fixed (uint *arrayPtr = _array)
                {
                    uint *currentPtr = arrayPtr, stopPtr = arrayPtr + _array.Length;
#endif

                    while (currentPtr < stopPtr)
                    {
                        _sum += sumFoldBlock(*currentPtr++);
                    }
                }
            }
        }
#else
        private void sumFoldSafe()
        {
            _sum = 0;
            unchecked
            {
                for (int i = 0; i < _array.Length; i++)
                {
                    _sum += sumFoldBlock(_array[i]);
                }
            }
        }
#endif

#if USE64BIT
        private static int sumFoldBlock(ulong block)
        {
            block = (block & 0x5555555555555555ul) + ((block >> 1) & 0x5555555555555555ul);
            block = (block & 0x3333333333333333ul) + ((block >> 2) & 0x3333333333333333ul);
            block = (block & 0x0F0F0F0F0F0F0F0Ful) + ((block >> 4) & 0x0F0F0F0F0F0F0F0Ful);
            block = (block & 0x00FF00FF00FF00FFul) + ((block >> 8) & 0x00FF00FF00FF00FFul);
            block = (block & 0x0000FFFF0000FFFFul) + ((block >> 16) & 0x0000FFFF0000FFFFul);
            return (int)((block & 0x00000000FFFFFFFFul) + (block >> 32));
        }
#else
        private static int sumFoldBlock(uint block)
        {
            block = (block & 0x55555555u) + ((block >> 1) & 0x55555555u);
            block = (block & 0x33333333u) + ((block >> 2) & 0x33333333u);
            block = (block & 0x0F0F0F0Fu) + ((block >> 4) & 0x0F0F0F0Fu);
            block = (block & 0x00FF00FFu) + ((block >> 8) & 0x00FF00FFu);
            return (int) ((block & 0x0000FFFFu) + (block >> 16));
        }
#endif

#if LOOKUP8
        private void sumLookup8Safe()
        {
            _sum = 0;
            unchecked
            {
                for (int i = 0; i < _array.Length; i++)
                {
#if USE64BIT
                    ulong current = _array[i];
                    _sum += _lookup8[(int)(current & _8bits)];
                    _sum += _lookup8[(int)((current >> 8) & _8bits)];
                    _sum += _lookup8[(int)((current >> 16) & _8bits)];
                    _sum += _lookup8[(int)((current >> 24) & _8bits)];
                    _sum += _lookup8[(int)((current >> 32) & _8bits)];
                    _sum += _lookup8[(int)((current >> 40) & _8bits)];
                    _sum += _lookup8[(int)((current >> 48) & _8bits)];
                    _sum += _lookup8[(int)(current >> 56)];
#else
                    uint current = _array[i];
                    _sum += _lookup8[(int) (current & _8bits)];
                    _sum += _lookup8[(int) ((current >> 8) & _8bits)];
                    _sum += _lookup8[(int) ((current >> 16) & _8bits)];
                    _sum += _lookup8[(int) (current >> 24)];
#endif
                }
            }
        }

#if UNSAFE
        private unsafe void sumLookup8Unsafe()
        {
            _sum = 0;
            unchecked
            {
#if USE64BIT
                fixed (ulong* arrayPtr = _array)
                {
                    fixed (Byte* lookup = _lookup8)
                    {
                        ulong* currentPtr = arrayPtr, stopPtr = arrayPtr + _array.Length;
                        while (currentPtr < stopPtr)
                        {
                            ulong current = *currentPtr++;

                            _sum += *(lookup + (int)(current & _8bits));
                            _sum += *(lookup + (int)((current >> 8) & _8bits));
                            _sum += *(lookup + (int)((current >> 16) & _8bits));
                            _sum += *(lookup + (int)((current >> 24) & _8bits));
                            _sum += *(lookup + (int)((current >> 32) & _8bits));
                            _sum += *(lookup + (int)((current >> 40) & _8bits));
                            _sum += *(lookup + (int)((current >> 48) & _8bits));
                            _sum += *(lookup + (int)(current >> 56));
                        }
                    }
                }
#else
                fixed (uint *arrayPtr = _array)
                {
                    fixed (Byte *lookup = _lookup8)
                    {
                        uint *currentPtr = arrayPtr, stopPtr = arrayPtr + _array.Length;
                        while (currentPtr < stopPtr)
                        {
                            uint current = *currentPtr++;

                            _sum += *(lookup + (int) (current & _8bits));
                            _sum += *(lookup + (int) ((current >> 8) & _8bits));
                            _sum += *(lookup + (int) ((current >> 16) & _8bits));
                            _sum += *(lookup + (int) (current >> 24));
                        }
                    }
                }
#endif
            }
        }
#endif
#endif

#if LOOKUP16
        private void sumLookup16Safe()
        {
            _sum = 0;
            unchecked
            {
                for (int i = 0; i < _array.Length; i++)
                {
#if USE64BIT
                    ulong current = _array[i];
                    _sum += _lookup16[(int)(current & _16bits)];
                    _sum += _lookup16[(int)((current >> 16) & _16bits)];
                    _sum += _lookup16[(int)((current >> 32) & _16bits)];
                    _sum += _lookup16[(int)(current >> 48)];
#else
                    uint current = _array[i];
                    _sum += _lookup16[(int) (current & _16bits)];
                    _sum += _lookup16[(int) (current >> 16)];
#endif
                }
            }
        }

#if UNSAFE
        private unsafe void sumLookup16Unsafe()
        {
            _sum = 0;
            unchecked
            {
#if USE64BIT
                fixed (ulong* arrayPtr = _array)
                {
                    fixed (Byte* lookup = _lookup16)
                    {
                        ulong* currentPtr = arrayPtr, stopPtr = arrayPtr + _array.Length;
                        while (currentPtr < stopPtr)
                        {
                            ulong current = *currentPtr++;

                            _sum += *(lookup + (int)(current & _16bits));
                            _sum += *(lookup + (int)((current >> 16) & _16bits));
                            _sum += *(lookup + (int)((current >> 32) & _16bits));
                            _sum += *(lookup + (int)(current >> 48));
                        }
                    }
                }
#else
                fixed (uint *arrayPtr = _array)
                {
                    fixed (Byte *lookup = _lookup16)
                    {
                        uint *currentPtr = arrayPtr, stopPtr = arrayPtr + _array.Length;
                        while (currentPtr < stopPtr)
                        {
                            uint current = *currentPtr++;

                            _sum += *(lookup + (int) (current & _16bits));
                            _sum += *(lookup + (int) (current >> 16));
                        }
                    }
                }
#endif
            }
        }
#endif
#endif

        private void sumSparseSafe()
        {
            _sum = 0;
            unchecked
            {
                for (int i = 0; i < _array.Length; i++)
                {
#if USE64BIT
                    ulong current = _array[i];
#else
                    uint current = _array[i];
#endif

                    while (current > 0)
                    {
                        _sum++;
                        current &= current - 1;
                    }
                }
            }
        }

#if UNSAFE
        private unsafe void sumSparseUnsafe()
        {
            _sum = 0;
            unchecked
            {
#if USE64BIT
                fixed (ulong* arrayPtr = _array)
                {
                    ulong* currentPtr = arrayPtr, stopPtr = arrayPtr + _array.Length;
                    while (currentPtr < stopPtr)
                    {
                        ulong current = *currentPtr++;
#else
                fixed (uint *arrayPtr = _array)
                {
                    uint *currentPtr = arrayPtr, stopPtr = arrayPtr + _array.Length;
                    while (currentPtr < stopPtr)
                    {
                        uint current = *currentPtr++;
#endif

                        while (current > 0)
                        {
                            _sum++;
                            current &= current - 1;
                        }
                    }
                }
            }
        }
#endif

        #endregion

        public void SetBit(int index, bool value)
        {
            if (_size == 0)
                throw new InvalidOperationException("BitString was not initialized (use create method first).");
            if ((index < 0) || (index >= _size))
                throw new ArgumentOutOfRangeException("index",
                                                      "Index must be between 0 and the length of BitString minus 1.");

            if (value)
            {
                _array[index / _blockSize] |= _one << (index % _blockSize);
            }
            else
            {
                _array[index / _blockSize] &= ~(_one << (index % _blockSize));
            }

            _sum = -1;
        }

        public bool GetBit(int index)
        {
            if (_size == 0)
                throw new InvalidOperationException("BitString was not initialized (use create method first).");
            if ((index < 0) || (index >= _size))
                throw new ArgumentOutOfRangeException("index",
                                                      "Index must be between 0 and the length of BitString minus 1.");

            return (_array[index / _blockSize] & (_one << (index % _blockSize))) > 0;
        }

        public void Fill(bool value)
        {
            if (_size == 0)
                throw new InvalidOperationException("BitString was not initialized (use create method first).");

#if UNSAFE
            fillUnsafe(value);
#else
            fillSafe(value);
#endif
        }

#if UNSAFE
        private unsafe void fillUnsafe(bool value)
        {
#if USE64BIT
            fixed (ulong* arrayPtr = _array)
#else
            fixed (uint *arrayPtr = _array)
#endif
            {
                if (value)
                {
#if USE64BIT
                    ulong* currentPtr = arrayPtr, stopPtr = arrayPtr + _array.Length - 1;
#else
                    uint *currentPtr = arrayPtr, stopPtr = arrayPtr + _array.Length - 1;
#endif
                    while (currentPtr < stopPtr)
                    {
                        *currentPtr++ = _allOnes;
                    }
                    *currentPtr = _allOnes >> (_blockSize - (_size - 1) % _blockSize - 1);
                    _sum = _size;
                }
                else
                {
#if USE64BIT
                    ulong* currentPtr = arrayPtr, stopPtr = arrayPtr + _array.Length;
#else
                    uint *currentPtr = arrayPtr, stopPtr = arrayPtr + _array.Length;
#endif
                    while (currentPtr < stopPtr)
                    {
                        *currentPtr++ = 0;
                    }
                    _sum = 0;
                }
            }
        }
#else
        private void fillSafe(bool value)
        {
            if (value)
            {
                int i;
                for (i = 0; i < _array.Length - 1; i++)
                {
                    _array[i] = _allOnes;
                }
                _array[i] = _allOnes >> (_blockSize - (_size - 1) % _blockSize - 1);
                _sum = _size;
            }
            else
            {
                for (int i = 0; i < _array.Length; i++)
                {
                    _array[i] = 0;
                }
                _sum = 0;
            }
        }
#endif

        /// <summary>
        /// Returns the human-readable string of the BitString contents.
        /// </summary>
        /// <returns>Human-readable string of the BitString contents.</returns>
        public override string ToString()
        {
            if (_size > 0)
            {
                StringBuilder retValue = new StringBuilder(_size);

                int i, j;
                for (i = 0; i < _array.Length - 1; i++)
                {
                    for (j = 0; j < _blockSize; j++)
                    {
                        retValue.Append((_array[i] & (_one << j)) > 0 ? '1' : '0');
                    }
                }
                for (j = 0; j < _size % _blockSize; j++)
                {
                    retValue.Append((_array[i] & (_one << j)) > 0 ? '1' : '0');
                }

                return retValue.ToString();
            }
            else
            {
                return "uninitialized BitString";
            }
        }


        /// <summary>
        /// Gets the length of the BitString.
        /// </summary>
        public int Length
        {
            get { return _size; }
        }

        ///// <summary>
        ///// Compares two bit strings by value.
        ///// </summary>
        ///// <param name="obj">Another bit string.</param>
        ///// <returns><b>true</b> if the bit strings are the same object or if they contain the same value, <b>false</b> otherwise.</returns>
        //public override bool Equals(object obj)
        //{
        //    BitString that = obj as BitString;
        //    if (that == null)
        //        return false;

        //    if (ReferenceEquals(this, that))
        //        return true;

        //    if (_size != that._size)
        //        return false;

        //    if ((_sum >= 0) && (that._sum >= 0) && (_sum != that._sum))
        //        return false;

        //    Debug.Assert(_array.Length == that._array.Length,
        //                 "The array sizes don't match, although bit string lengths are the same.");
        //    Debug.Assert(
        //        (_size % _blockSize == 0) || ((_array[_array.Length - 1] & (_allOnes << _size % _blockSize)) == 0),
        //        "The bit string contains non-zero bits in the last block behind the allowed length.");
        //    Debug.Assert(
        //        (_size % _blockSize == 0) || ((that._array[_array.Length - 1] & (_allOnes << _size % _blockSize)) == 0),
        //        "The bit string contains non-zero bits in the last block behind the allowed length.");

        //    for (int i = 0; i < _array.Length; i++)
        //    {
        //        if (_array[i] != that._array[i])
        //            return false;
        //    }

        //    if ((_sum >= 0) && (that._sum < 0))
        //        that._sum = _sum;
        //    else if ((that._sum >= 0) && (_sum < 0))
        //        _sum = that._sum;

        //    return true;
        //}
    }
}