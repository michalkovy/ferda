// BitString.cs - BitString of a fixed length
//
// Authors: Tomáš Kuchař <tomas.kuchar@gmail.com>      
// Commented by: Martin Ralbovský <martin.ralbovsky@gmail.com>
//
// Copyright (c) 2006 Tomáš Kuchař
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

//#define Testing

#define LOOKUP8
#define LOOKUP16
#define UNSAFE
#define UNITTESTING

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using Ferda.Guha.MiningProcessor.Formulas;

namespace Ferda.Guha.MiningProcessor.BitStrings
{
    /// <summary>
    /// This class represents a bit string of a fixed length. The class encapsulates
    /// all the "low-level" bitstring handling such as performing the individual
    /// binary operations on the bitstrings. Class should be accessed only via the
    /// IBitString interface.
    /// </summary>
    public class BitString : IBitString, IBitStringCreate, IBitStringBase
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

        /// <summary>
        /// Identifier of the bit string (each bit string should be
        /// identified by a boolean attribute formula representing the
        /// bit string. 
        /// </summary>
        public Formulas.BooleanAttributeFormula Identifier
        {
            get { return _identifier; }
        }

        private int [] _lengthVector = null;

        public int[] LengthVector
        {
            get { return _lengthVector; }
            set { _lengthVector = value; }
        }

        private const int _blockSize = 64;
        private const ulong _zero = 0ul;
        private const ulong _one = 1ul;
        private const ulong _8bits = 0x00000000000000FFul;
        private const ulong _16bits = 0x000000000000FFFFul;
        private const ulong _allOnes = 0xFFFFFFFFFFFFFFFFul;
        private ulong[] _array;

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

        /// <summary>
        /// Creates a new bit string
        /// </summary>
        /// <param name="identifier">The identifier of the bit string</param>
        /// <param name="length">Length of the bit string</param>
        /// <param name="bits">Array of longs, where each bit is means one bit
        /// of the bit string (64 bits in one long)</param>
        public unsafe BitString(BitStringIdentifier identifier, int length, ReadOnlySpan<long> bits)
            : this(new AtomFormula(identifier))
        {
            if (length <= 0)
                throw Exceptions.BitStringLengthError();

            _size = length;

            int arraySize = (length + _blockSize - 1) / _blockSize; // rounding up...

            if (arraySize != bits.Length)
                throw new ArgumentOutOfRangeException("bits", "The array of bits has bad size (Length).");

            _array = GC.AllocateUninitializedArray<ulong>(arraySize);
            ReadOnlySpan<ulong> ubits = MemoryMarshal.Cast<long, ulong>(bits);
            ubits.CopyTo(_array);
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

            _array = GC.AllocateUninitializedArray<ulong>(source._array.Length);

            copy(source);
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

            _array = GC.AllocateUninitializedArray<ulong>(arraySize);
        }

        private void copy(BitString source)
        {
            source._array.CopyTo((Span<ulong>) _array);
        }

        #endregion

        #region AND

        /// <summary>
        /// Performs the bitwise AND operation on current BitString against the specified BitString.
        /// </summary>
        /// <param name="source">The second BitString operand.</param>
        /// <param name="precomputeSum">Should sum be precomputed during the operation</param>
        /// <returns>The result of the AND operation</returns>
        public IBitString And(IBitString source, bool precomputeSum = false)
        {
            if (source is BitString)
            {
                BitString result = new BitString(this);
                result.and((BitString)source, precomputeSum);
                result._identifier = FormulaHelper.And(Identifier, source.Identifier);
                return result;
            }
            else if (source is FuzzyBitString)
            {
                return source.And(this);
            }
            else if (source is EmptyBitString)
            {
                return new BitString(this);
            }
            else if (source is FalseBitString)
            {
                return source;
            }
            else if (source is TrueBitString)
            {
                return new BitString(this);
            }
            else
                throw new NotImplementedException();
        }

        /// <summary>
        /// Performs the bitwise AND operation on current BitString against the specified BitString. It can modify current and return it or returns the source.
        /// </summary>
        /// <param name="source">The second BitString operand.</param>
        /// <returns>The result of the AND operation based of one of the them</returns>
        public IBitString AndInPlace(IBitString source)
        {
            if (source is BitString)
            {
                this.and((BitString)source);
                this._identifier = FormulaHelper.And(Identifier, source.Identifier);
                return this;
            }
            else if (source is FuzzyBitString)
            {
                return source.AndInPlace(this);
            }
            else if (source is EmptyBitString)
            {
                return this;
            }
            else if (source is FalseBitString)
            {
                return source;
            }
            else if (source is TrueBitString)
            {
                return this;
            }
            else
                throw new NotImplementedException();
        }

        protected void and(BitString source, bool precomputeSum = false)
        {
            if (_size == 0)
                throw new InvalidOperationException("BitString was not initialized (use create method first).");
            if (_size != source._size)
                throw Exceptions.BitStringsLengtsAreNotEqualError();

            Debug.Assert(_array.Length == source._array.Length,
                         "The array sizes don't match, although bit string lengths are the same.");
            Debug.Assert(
                (_size % _blockSize == 0) || ((_array[_array.Length - 1] & (_allOnes << _size % _blockSize)) == 0),
                "The bit string contains non-zero bits in the last block behind the allowed length.");
            Debug.Assert(
                (_size % _blockSize == 0) || ((source._array[_array.Length - 1] & (_allOnes << _size % _blockSize)) == 0),
                "The bit string contains non-zero bits in the last block behind the allowed length.");

            if (precomputeSum)
            {
                andUnsafeWithSumPopCnt(source);
            }
            else
            {
                andUnsafe(source);
                _sum = -1;
            }
        }

        private unsafe void andUnsafe(BitString source)
        {
            fixed (ulong* destPin = _array, sourcePin = source._array)
            {
                ulong* destPtr = destPin, sourcePtr = sourcePin, stopPtr = destPin + _array.Length;
                while (destPtr < stopPtr)
                {
                    *destPtr++ &= *sourcePtr++;
                }
            }
        }

        private unsafe void andUnsafeWithSumPopCnt(BitString source)
        {
            _sum = 0;
            unchecked
            {
                fixed (ulong* destPin = _array, sourcePin = source._array)
                {
                    ulong* destPtr = destPin, sourcePtr = sourcePin, stopPtr = destPin + _array.Length;
                    while (destPtr < stopPtr)
                    {
                        *destPtr &= *sourcePtr++;
                        _sum += BitOperations.PopCount(*destPtr++);
                    }
                }
            }
        }

        #endregion

        #region OR

        /// <summary>
        /// Performs the bitwise OR operation on current BitString against the specified BitString.
        /// </summary>
        /// <param name="source">The second BitString operand.</param>
        /// <param name="precomputeSum">Should sum be precomputed during the operation</param>
        /// <returns>The result of the OR operation</returns>
        public IBitString Or(IBitString source, bool precomputeSum = false)
        {
            if (source is BitString)
            {
                BitString result = new BitString(this);
                result.or((BitString)source, precomputeSum);
                result._identifier = FormulaHelper.Or(Identifier, source.Identifier);
                return result;
            }
            else if (source is FuzzyBitString)
            {
                //fuzzy handling is in fuzzy bit strings
                return source.Or(this);
            }
            else if (source is EmptyBitString)
            {
                return new BitString(this);
            }
            else if (source is FalseBitString)
            {
                return new BitString(this);
            }
            else if (source is TrueBitString)
            {
                return source;
            }
            else
                throw new NotImplementedException();
        }

        /// <summary>
        /// Performs the bitwise OR operation on current BitString against the specified BitString. It modifies current or returns the source.
        /// </summary>
        /// <param name="source">The second BitString operand.</param>
        /// <returns>The result of the OR operation</returns>
        public IBitString OrInPlace(IBitString source)
        {
            if (source is BitString)
            {
                this.or((BitString)source);
                this._identifier = FormulaHelper.Or(Identifier, source.Identifier);
                return this;
            }
            else if (source is FuzzyBitString)
            {
                //fuzzy handling is in fuzzy bit strings
                return source.OrInPlace(this);
            }
            else if (source is EmptyBitString)
            {
                return this;
            }
            else if (source is FalseBitString)
            {
                return this;
            }
            else if (source is TrueBitString)
            {
                return source;
            }
            else
                throw new NotImplementedException();
        }

        protected void or(BitString source, bool precomputeSum = false)
        {
            if (_size == 0)
                throw new InvalidOperationException("BitString was not initialized (use create method first).");
            if (source._size != _size)
                throw Exceptions.BitStringsLengtsAreNotEqualError();

            Debug.Assert(_array.Length == source._array.Length,
                         "The array sizes don't match, although bit string lengths are the same.");
            Debug.Assert(
                (_size % _blockSize == 0) || ((_array[_array.Length - 1] & (_allOnes << _size % _blockSize)) == 0),
                "The bit string contains non-zero bits in the last block behind the allowed length.");
            Debug.Assert(
                (_size % _blockSize == 0) || ((source._array[_array.Length - 1] & (_allOnes << _size % _blockSize)) == 0),
                "The bit string contains non-zero bits in the last block behind the allowed length.");

            if (precomputeSum)
            {
                orUnsafeWithSumPopCnt(source);
            }
            else
            {
                orUnsafe(source);
                _sum = -1;
            }
        }

        private unsafe void orUnsafe(BitString source)
        {
            fixed (ulong* destPin = _array, sourcePin = source._array)
            {
                ulong* destPtr = destPin, sourcePtr = sourcePin, stopPtr = destPin + _array.Length;
                while (destPtr < stopPtr)
                {
                    *destPtr++ |= *sourcePtr++;
                }
            }
        }

        private unsafe void orUnsafeWithSumPopCnt(BitString source)
        {
            _sum = 0;
            unchecked
            {
                fixed (ulong* destPin = _array, sourcePin = source._array)
                {
                    ulong* destPtr = destPin, sourcePtr = sourcePin, stopPtr = destPin + _array.Length;
                    while (destPtr < stopPtr)
                    {
                        *destPtr |= *sourcePtr++;
                        _sum += BitOperations.PopCount(*destPtr++);
                    }
                }
            }
        }

        #endregion

        #region NOT

        /// <summary>
        /// Performs the bitwise NOT on current BitString.
        /// </summary>
        public IBitString Not()
        {
            BitString result = new BitString(this);
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

            notUnsafe();

            if (_sum >= 0)
                _sum = _size - _sum;
        }

        private unsafe void notUnsafe()
        {
            fixed (ulong* pin = _array)
            {
                ulong* ptr = pin, stop = pin + _array.Length;
                while (ptr < stop)
                {
                    *ptr = ~(*ptr);
                    ptr++;
                }
                if (_size % _blockSize > 0)
                {
                    *(ptr - 1) &= _allOnes >> (_blockSize - _size % _blockSize);
                }
            }
        }

        #endregion

        #region SUM

        /// <summary>
        /// Number of bits in the current bit string, that are not equal to zero.
        /// This property came with introduction of fuzzy bit strings. In boolean
        /// bit strings, the Sum operation determines both the number of non-zero
        /// bits and the sum of the bit string. In the fuzzy case these two numbers
        /// are different. The function is needed for determining frequencies in ETrees
        /// and number of all items belonging to a condition in a 4FT.
        /// </summary>
        public long NonZeroBitsCount
        {
            get
            {
                return Convert.ToInt64(Sum);
            }
        }

        /// <summary>
        /// Performs the bitwise SUM operation on current BitString.
        /// </summary>
        /// <returns>The number of bits set to 1 in current BitString.</returns>
        public float Sum
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

                lock (this)
                {
                    if (_sum >= 0)
                        return _sum;

                    // compute the sum using the best available method
                    // (looping, folding, 8-bit lookup, 16-bit lookup or sparse sum)
                    sumPopCnt();
                }

                Debug.Assert(_sum >= 0, "The sum must be non-negative.");
                Debug.Assert(_sum <= _size, "The sum must be less than or equal to the size of the bit string.");

                return Convert.ToSingle(_sum);
            }
            
            set
            {
                _sum = Convert.ToInt32(value);
            }
        }

        private unsafe void sumLoopUnsafe()
        {
            _sum = 0;

            unchecked
            {
                fixed (ulong* arrayPtr = _array)
                {
                    ulong current;
                    ulong* currentPtr = arrayPtr, stopPtr = arrayPtr + _array.Length - 1;

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

        private unsafe void sumFoldUnsafe()
        {
            _sum = 0;

            unchecked
            {
                fixed (ulong* arrayPtr = _array)
                {
                    ulong* currentPtr = arrayPtr, stopPtr = arrayPtr + _array.Length;

                    while (currentPtr < stopPtr)
                    {
                        _sum += sumFoldBlock(*currentPtr++);
                    }
                }
            }
        }

        private static int sumFoldBlock(ulong block)
        {
            block = (block & 0x5555555555555555ul) + ((block >> 1) & 0x5555555555555555ul);
            block = (block & 0x3333333333333333ul) + ((block >> 2) & 0x3333333333333333ul);
            block = (block & 0x0F0F0F0F0F0F0F0Ful) + ((block >> 4) & 0x0F0F0F0F0F0F0F0Ful);
            block = (block & 0x00FF00FF00FF00FFul) + ((block >> 8) & 0x00FF00FF00FF00FFul);
            block = (block & 0x0000FFFF0000FFFFul) + ((block >> 16) & 0x0000FFFF0000FFFFul);
            return (int)((block & 0x00000000FFFFFFFFul) + (block >> 32));
        }

#if LOOKUP8
        private void sumLookup8Safe()
        {
            _sum = 0;
            unchecked
            {
                for (int i = 0; i < _array.Length; i++)
                {
                    ulong current = _array[i];
                    _sum += _lookup8[(int)(current & _8bits)];
                    _sum += _lookup8[(int)((current >> 8) & _8bits)];
                    _sum += _lookup8[(int)((current >> 16) & _8bits)];
                    _sum += _lookup8[(int)((current >> 24) & _8bits)];
                    _sum += _lookup8[(int)((current >> 32) & _8bits)];
                    _sum += _lookup8[(int)((current >> 40) & _8bits)];
                    _sum += _lookup8[(int)((current >> 48) & _8bits)];
                    _sum += _lookup8[(int)(current >> 56)];
                }
            }
        }

        private unsafe void sumLookup8Unsafe()
        {
            _sum = 0;
            unchecked
            {
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
            }
        }
#endif

#if LOOKUP16
        private void sumLookup16Safe()
        {
            _sum = 0;
            unchecked
            {
                for (int i = 0; i < _array.Length; i++)
                {
                    ulong current = _array[i];
                    _sum += _lookup16[(int)(current & _16bits)];
                    _sum += _lookup16[(int)((current >> 16) & _16bits)];
                    _sum += _lookup16[(int)((current >> 32) & _16bits)];
                    _sum += _lookup16[(int)(current >> 48)];
                }
            }
        }

        private unsafe void sumLookup16Unsafe()
        {
            _sum = 0;
            unchecked
            {
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
            }
        }
#endif

        private void sumSparseSafe()
        {
            _sum = 0;
            unchecked
            {
                for (int i = 0; i < _array.Length; i++)
                {
                    ulong current = _array[i];

                    while (current > 0)
                    {
                        _sum++;
                        current &= current - 1;
                    }
                }
            }
        }

        private unsafe void sumSparseUnsafe()
        {
            _sum = 0;
            unchecked
            {
                fixed (ulong* arrayPtr = _array)
                {
                    ulong* currentPtr = arrayPtr, stopPtr = arrayPtr + _array.Length;
                    while (currentPtr < stopPtr)
                    {
                        ulong current = *currentPtr++;

                        while (current > 0)
                        {
                            _sum++;
                            current &= current - 1;
                        }
                    }
                }
            }
        }

        private unsafe void sumPopCnt()
        {
            _sum = 0;
            unchecked
            {
                fixed (ulong* arrayPtr = _array)
                {
                    ulong* currentPtr = arrayPtr, stopPtr = arrayPtr + _array.Length;
                    while (currentPtr < stopPtr)
                    {
                        ulong current = *currentPtr++;
                        _sum += BitOperations.PopCount(current);
                    }
                }
            }
        }

        /// <summary>
        /// sum = (a AND b).Sum
        /// </summary>
        public unsafe static void AndSum(IBitString a, IBitString b, int sumA, int sumB, out int sum)
        {
            ulong[] ap = null;
            ulong[] bp = null;
            sum = 0;

            fillSum(a, sumA, sumB, ref sum, ref ap);
            if (ap == null)
            {
                return;
            }
            else
            {
                fillSum(b, sumB, sumA, ref sum, ref bp);
                if (bp == null)
                {
                    return;
                }
                //else clausule is down
            }

            fixed (ulong* aP = ap, bP = bp)
            {
                fixed (Byte* lookup = _lookup16)
                {
                    ulong* aPt = aP, bPt = bP, stopPt = aP + ap.Length;
                    ulong temp;
                    while (aPt < stopPt)
                    {
                        temp = (*aPt++) & (*bPt++);
                        sum += BitOperations.PopCount(temp);
                    }
                }
            }
        }

        /// <summary>
        /// sum1 = (a AND b).Sum
        /// sum2 = (a AND c).Sum
        /// </summary>
        public unsafe static void TwoAndSum(IBitString a, IBitString b, IBitString c, int sumA, int sumB, int sumC, out int sum1, out int sum2)
        {
            ulong[] ap = null;
            ulong[] bp = null;
            ulong[] cp = null;
            sum1 = 0;
            sum2 = 0;

            fillSums(a, sumA, sumB, sumC, ref sum1, ref sum2, ref ap);
            if (ap == null)
            {
                return;
            }
            else
            {
                fillSum(b, sumB, sumA, ref sum1, ref bp);
                if (bp == null)
                {
                    AndSum(a, c, sumA, sumC, out sum2);
                    return;
                }
                else
                {
                    fillSum(c, sumC, sumA, ref sum2, ref cp);
                    if (cp == null)
                    {
                        AndSum(a, b, sumA, sumB, out sum1);
                        return;
                    }
                    //else clausule is down
                }
            }

            fixed (ulong* aP = ap, bP = bp, cP = cp)
            {
                fixed (Byte* lookup = _lookup16)
                {
                    ulong* aPt = aP, bPt = bP, cPt = cP, stopPt = aP + ap.Length;
                    ulong temp1, temp2;
                    while (aPt < stopPt)
                    {
                        temp1 = *aPt++;
                        temp2 = *bPt++;
                        temp2 &= temp1;
                        sum1 += BitOperations.PopCount(temp2);
                        temp2 = *cPt++;
                        temp2 &= temp1;
                        sum2 += BitOperations.PopCount(temp2);
                    }
                }
            }
        }

        /// <summary>
        /// sum1 = (a AND c).Sum
        /// sum2 = (b AND c).Sum
        /// sum3 = (a AND d).Sum
        /// sum4 = (b AND d).Sum
        /// </summary>
        public unsafe static void CrossAndSum(IBitString a, IBitString b, IBitString c, IBitString d, int sumA, int sumB, int sumC, int sumD, out int sum1, out int sum2, out int sum3, out int sum4)
        {
            ulong[] ap = null;
            ulong[] bp = null;
            ulong[] cp = null;
            ulong[] dp = null;
            sum1 = 0;
            sum2 = 0;
            sum3 = 0;
            sum4 = 0;

            fillSums(a, sumA, sumC, sumD, ref sum1, ref sum3, ref ap);
            if (ap == null)
            {
                TwoAndSum(b, c, d, sumB, sumC, sumD, out sum2, out sum4);
                return;
            }
            else
            {
                fillSums(b, sumB, sumC, sumD, ref sum2, ref sum4, ref bp);
                if (bp == null)
                {
                    TwoAndSum(a, c, d, sumA, sumC, sumD, out sum1, out sum3);
                    return;
                }
                else
                {
                    fillSums(c, sumC, sumA, sumB, ref sum1, ref sum2, ref cp);
                    if (cp == null)
                    {
                        TwoAndSum(d, a, b, sumD, sumA, sumB, out sum3, out sum4);
                        return;
                    }
                    else
                    {
                        fillSums(d, sumD, sumA, sumB, ref sum3, ref sum4, ref dp);
                        if (dp == null)
                        {
                            TwoAndSum(d, a, b, sumD, sumA, sumB, out sum3, out sum4);
                            return;
                        }
                        //else clausule is down
                    }
                }
            }

            fixed (ulong* aP = ap, bP = bp, cP = cp, dP = dp)
            {
                fixed (Byte* lookup = _lookup16)
                {
                    ulong* aPt = aP, bPt = bP, cPt = cP, dPt = dP, stopPt = aP + ap.Length;
                    while (aPt < stopPt)
                    {
                        ulong al = *aPt++;
                        ulong bl = *bPt++;
                        ulong cl = *cPt++;
                        ulong dl = *dPt++;
                        ulong ac = al & cl;
                        ulong ad = al & dl;
                        ulong bc = bl & cl;
                        ulong bd = bl & dl;
                        sum1 += BitOperations.PopCount(ac);
                        sum2 += BitOperations.PopCount(bc);
                        sum3 += BitOperations.PopCount(ad);
                        sum4 += BitOperations.PopCount(bd);
                    }
                }
            }

            /* ukazka pouziti System.Numerics
	     * nicmene myslim si, ze bychom meli retizky predelat z ulong[] na Vector<ulong>[] a pak by to nize bylo komplikovanejsi
            Vector4i sum = new Vector4i(0, 0, 0, 0);
            fixed (ulong* aP = ap, bP = bp, cP = cp, dP = dp)
            {
                fixed (Byte* lookup = _lookup16)
                {
                    ulong* aPt = aP, bPt = bP, cPt = cP, dPt = dP, stopPt = aP + ap.Length;
                    ulong temp1, temp2, temp3;
                    while (aPt < stopPt)
                    {
		        Vector<ulong> ab = new Vector<ulong>(*aPt++, *bPt++);
			Vector<ulong> cd = new Vector<ulong>(*cPt++, *dPt++);
			Vector8us aAcbAd = (Vector8us)(ab & cd);
			Vector8us aAdbAc = (Vector8us)(ab & (Vector<ulong>)Vector4i.Suffle((Vector4i)cd,ShuffleSel.XFromZ | ShuffleSel.YFromW | ShuffleSel.ZFromX | ShuffleSel.WFromY));
			sum += new Vector4i(*(lookup + (int)aAcbAd.V0), *(lookup + (int)aAdbAc.V4), *(lookup + (int)aAdbAc.V0), *(lookup + (int)aAcbAd.V4));
			sum += new Vector4i(*(lookup + (int)aAcbAd.V1), *(lookup + (int)aAdbAc.V5), *(lookup + (int)aAdbAc.V1), *(lookup + (int)aAcbAd.V5));
			sum += new Vector4i(*(lookup + (int)aAcbAd.V2), *(lookup + (int)aAdbAc.V6), *(lookup + (int)aAdbAc.V2), *(lookup + (int)aAcbAd.V6));
			sum += new Vector4i(*(lookup + (int)aAcbAd.V3), *(lookup + (int)aAdbAc.V7), *(lookup + (int)aAdbAc.V3), *(lookup + (int)aAcbAd.V7));
                    }
                }
            } */
        }

        private static void fillSum(IBitString iBS, int actSum, int secondSum, ref int sumSet, ref ulong[] bp)
        {
            if (iBS is EmptyBitString)
            {
                sumSet = secondSum;
            }
            else if (iBS is FalseBitString)
            {
                sumSet = actSum;
            }
            else if (iBS is BitString)
            {
                bp = ((BitString)iBS).Array;
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        private static void fillSums(IBitString iBS, int actSum, int firstSum, int secondSum, ref int firstSumSet, ref int secondSumSet, ref ulong[] bp)
        {
            if (iBS is EmptyBitString)
            {
                firstSumSet = firstSum;
                secondSumSet = secondSum;
            }
            else if (iBS is FalseBitString)
            {
                firstSumSet = actSum;
                secondSumSet = actSum;
            }
            else if (iBS is BitString)
            {
                bp = ((BitString)iBS).Array;
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        #endregion

        /// <summary>
        /// Sets a specified bit in the BitString.
        /// </summary>
        /// <param name="index">Index of the bit to be set.</param>
        /// <param name="value">New value of the bit.</param>
        public void SetBit(int index, float value)
        {
            if (_size == 0)
                throw new InvalidOperationException("BitString was not initialized (use create method first).");
            if ((index < 0) || (index >= _size))
                throw new ArgumentOutOfRangeException("index",
                                                      "Index must be between 0 and the length of BitString minus 1.");
            if (value != 0f && value != 1f)
            {
                throw Exceptions.NotFuzzyBitStringException();
            }
            bool v = System.Convert.ToBoolean(value);

            if (v)
            {
                _array[index / _blockSize] |= _one << (index % _blockSize);
            }
            else
            {
                _array[index / _blockSize] &= ~(_one << (index % _blockSize));
            }

            _sum = -1;
        }


        /// <summary>
        /// Gets a value of the specified bit from the BitString.
        /// </summary>
        /// <param name="index">Index of the bit to be retrieved.</param>
        /// <returns>Value of the specified bit from the BitString.</returns>
        public float GetBit(int index)
        {
            if (_size == 0)
                throw new InvalidOperationException("BitString was not initialized (use create method first).");
            if ((index < 0) || (index >= _size))
                throw new ArgumentOutOfRangeException("index",
                                                      "Index must be between 0 and the length of BitString minus 1.");

            bool result = (_array[index / _blockSize] & (_one << (index % _blockSize))) > 0;
            return Convert.ToSingle(result);
        }

        /// <summary>
        /// Fills the whole BitString with the specified value.
        /// </summary>
        /// <param name="value">Value to be filled into every bit of the BitString.</param>
        /// <remarks>
        /// <para>BitString are filled with zeroes when created, so there is no need to call Fill(false) after create() method.</para>
        /// </remarks>
        public void Fill(float value)
        {
            if (value != 0f && value != 1f)
            {
                throw Exceptions.NotFuzzyBitStringException();
            }

            if (_size == 0)
                throw new InvalidOperationException("BitString was not initialized (use create method first).");

            bool v = System.Convert.ToBoolean(value);
#if UNSAFE
            fillUnsafe(v);
#else
            fillSafe(v);
#endif
        }

#if UNSAFE
        private unsafe void fillUnsafe(bool value)
        {
            fixed (ulong* arrayPtr = _array)
            {
                if (value)
                {
                    ulong* currentPtr = arrayPtr, stopPtr = arrayPtr + _array.Length - 1;
                    while (currentPtr < stopPtr)
                    {
                        *currentPtr++ = _allOnes;
                    }
                    *currentPtr = _allOnes >> (_blockSize - (_size - 1) % _blockSize - 1);
                    _sum = _size;
                }
                else
                {
                    ulong* currentPtr = arrayPtr, stopPtr = arrayPtr + _array.Length;
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

        /// <summary>
        /// Compares two bit strings by value.
        /// </summary>
        /// <param name="obj">Another bit string.</param>
        /// <returns><b>true</b> if the bit strings are the same object or if they contain the same value, <b>false</b> otherwise.</returns>
        public override bool Equals(object obj)
        {
            BitString that = obj as BitString;
            if (that == null)
                return false;

            if (ReferenceEquals(this, that))
                return true;

            if (_size != that._size)
                return false;

            if ((_sum >= 0) && (that._sum >= 0) && (_sum != that._sum))
                return false;

            Debug.Assert(_array.Length == that._array.Length,
                         "The array sizes don't match, although bit string lengths are the same.");
            Debug.Assert(
                (_size % _blockSize == 0) || ((_array[_array.Length - 1] & (_allOnes << _size % _blockSize)) == 0),
                "The bit string contains non-zero bits in the last block behind the allowed length.");
            Debug.Assert(
                (_size % _blockSize == 0) || ((that._array[_array.Length - 1] & (_allOnes << _size % _blockSize)) == 0),
                "The bit string contains non-zero bits in the last block behind the allowed length.");

            for (int i = 0; i < _array.Length; i++)
            {
                if (_array[i] != that._array[i])
                    return false;
            }

            if ((_sum >= 0) && (that._sum < 0))
                that._sum = _sum;
            else if ((that._sum >= 0) && (_sum < 0))
                _sum = that._sum;

            return true;
        }


        /// <summary>
        /// Compares two bit strings by value.
        /// </summary>
        /// <param name="a">A BitString or a null reference.</param>
        /// <param name="b">A BitString or a null reference.</param>
        /// <returns><b>true</b> if the bit strings are the same object or if they contain the same value, <b>false</b> otherwise.</returns>
        public static bool Equals(BitString a, BitString b)
        {
            return Object.Equals(a, b);
        }


        /// <summary>
        /// Compares two bit strings by value.
        /// </summary>
        /// <param name="a">A BitString or a null reference.</param>
        /// <param name="b">A BitString or a null reference.</param>
        /// <returns><b>true</b> if the bit strings are the same object or if they contain the same value, <b>false</b> otherwise.</returns>
        public static bool operator ==(BitString a, BitString b)
        {
            if (ReferenceEquals(a, b))
                return true;
            else if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
                return false;
            else
                return Object.Equals(a, b);
        }


        /// <summary>
        /// Compares two bit strings by value for unequality.
        /// </summary>
        /// <param name="a">A BitString or a null reference.</param>
        /// <param name="b">A BitString or a null reference.</param>
        /// <returns><b>false</b> if the bit strings are the same object or if they contain the same value, <b>true</b> otherwise.</returns>
        public static bool operator !=(BitString a, BitString b)
        {
            return !Object.Equals(a, b);
        }


        /// <summary>
        /// Returns the hash code for this instance of bit string.
        /// </summary>
        /// <returns>A 32-bit signed integer hash code.</returns>
        /// <remarks>
        /// <para>Implemented as a XOR over the array that stores the bit string, plus one XOR for the real bit string size.</para>
        /// </remarks>
        public override int GetHashCode()
        {
            if (_size == 0)
                return 0;

            Debug.Assert(
                (_size % _blockSize == 0) || ((_array[_array.Length - 1] & (_allOnes << _size % _blockSize)) == 0),
                "The bit string contains non-zero bits in the last block behind the allowed length.");

            int hash = _size;
            for (int i = 0; i < _array.Length; i++)
            {
                hash ^= (int)_array[i];
            }
            return hash;
        }

		protected ulong[] Array
		{
			get
			{
				return _array;
			}
		}
    }
}
