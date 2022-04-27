// BitString.cs - fuzzy BitString of a fixed length
//
// Authors: Martin Ralbovský <martin.ralbovsky@gmail.com>
//
// Copyright (c) 2009 Martin Ralbovský 
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
using System.Numerics;
using System.Text;
using Ferda.Guha.MiningProcessor.Formulas;

namespace Ferda.Guha.MiningProcessor.BitStrings
{
    public class FuzzyBitString : IBitString, IBitStringCreate
    {
        /// <summary>
        /// Internal array where fuzzy bit strings are stored
        /// </summary>
        private Vector4[] _array;

        /// <summary>
        /// The size of the bit string
        /// </summary>
        private int _size;

        /// <summary>
        /// Identifier of the bit string (each bit string should be
        /// identified by a boolean attribute formula representing the
        /// bit string. 
        /// </summary>
        private BooleanAttributeFormula _identifier;

        /// <summary>
        /// The size of one float vector - 4 floats
        /// </summary>
        private const int _blocksize = 4;

        /// <summary>
        /// Number of bits in the current bit string, that are not equal to zero.
        /// This property came with introduction of fuzzy bit strings. In boolean
        /// bit strings, the Sum operation determines both the number of non-zero
        /// bits and the sum of the bit string. In the fuzzy case these two numbers
        /// are different. The function is needed for determining frequencies in ETrees
        /// and number of all items belonging to a condition in a 4FT.
        /// </summary>
        private long _nonZeroBitsCount = -1;

        /// <summary>
        /// A bitwise SUM cache
        /// </summary>
        private float _sum = -1f;

        #region Properties

        /// <summary>
        /// Gets the length of the BitString.
        /// </summary>
        public int Length
        {
            get { return _size; }
        }

        /// <summary>
        /// Identifier of the bit string (each bit string should be
        /// identified by a boolean attribute formula representing the
        /// bit string. 
        /// </summary>
        public BooleanAttributeFormula Identifier
        {
            get { return _identifier; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Adds the identifier
        /// </summary>
        /// <param name="identifier">Identifier</param>
        private FuzzyBitString(BooleanAttributeFormula identifier)
        {
            _identifier = identifier;
        }

        /// <summary>
        /// Copy constructor
        /// </summary>
        /// <param name="source">The source bit string (from which it is copied)</param>
        public FuzzyBitString(FuzzyBitString source)
            : this(source._identifier)
        {
            if (source._size == 0)
                throw new InvalidOperationException("Cannot copy-construct a BitString from an uninitialized one.");

            _size = source._size;
            _array = new Vector4[source._array.Length];

            for (int i = 0; i < _array.Length; i++)
            {
                _array[i] = new Vector4(source._array[i].X, source._array[i].Y, source._array[i].Z, source._array[i].W);
            }
        }

        /// <summary>
        /// Constructor of the class.
        /// </summary>
        /// <param name="identifier">Identifier of the bit string</param>
        /// <param name="floats">Array of floats, which are transformed into
        /// the <see cref="System.Numerics.Vector4"/>structures.</param>
        /// <param name="checking">
        /// Determines, if the individual fuzzy values, that is floats in the 
        /// <paramref name="float"/> parameter should be checked if they are 
        /// in [0,1]
        /// </param>
        /// <remarks>
        /// In contrary to the constructor of <see cref="BitString"/> class, this constructor
        /// is safe and it does not need the length parameter. The length of the bit string
        /// is determined by the lenght of the array of floats. 
        /// </remarks>
        public FuzzyBitString(BitStringIdentifier identifier, float[] floats, bool checking)
            : this(new AtomFormula(identifier))
        {
            if (floats.Length == 0)
            {
                throw Exceptions.BitStringLengthError();
            }
            if (checking)
            {
                foreach (float f in floats)
                {
                    if (f < 0 || f > 1)
                    {
                        throw Exceptions.ValueNotFuzzyException();
                    }
                }
            }

            _size = floats.Length;

            // if size mod 4 = 0, the the vectorF size = size/4 else size/4 + 1
            int flength = (_size % _blocksize == 0) ? _size / _blocksize : (_size / _blocksize) + 1;
            _array = new Vector4[flength];

            for (int i = 0; i < flength; i++)
            {
                //so it is sure, that there are 4 floats still to be filled
                if (_blocksize * (i + 1) <= floats.Length)
                {
                    _array[i] = new Vector4(floats[_blocksize * i],
                        floats[_blocksize * i + 1], floats[_blocksize * i + 2], 
                        floats[_blocksize * i + 3]);
                }
                else
                {
                    //there is no other float to be filled
                    if (_blocksize * i == floats.Length + 1)
                    {
                        break;
                    }
                    //there is at least one float to be filled
                    else
                    {
                        _array[i] = new Vector4();
                        _array[i].X = floats[_blocksize * i];
                        //there is only one
                        if (floats.Length % _blocksize == 1)
                        {
                            break;
                        }
                        //there are at least 2 floats to be filled
                        if (floats.Length % _blocksize == 2)
                        {
                            _array[i].Y = floats[_blocksize * i + 1];
                        }
                        else
                        {
                            _array[i].Y = floats[_blocksize * i + 1];
                            _array[i].Z = floats[_blocksize * i + 2];
                        }
                    }
                }
            }
        }

        #endregion

        #region And

        /// <summary>
        /// Performs the bitwise AND operation on current BitString against the specified BitString.
        /// The supported t-norm is algebraic product, that is T(x,y) = xy. 
        /// </summary>
        /// <param name="source">The second BitString operand.</param>
        /// <returns>Result of the AND operation</returns>
        public IBitString And(IBitString source, bool precomputeSum = false)
        {
            if (source is FuzzyBitString)
            {
                FuzzyBitString result = new FuzzyBitString(this);
                result.and((FuzzyBitString)source);
                result._identifier = FormulaHelper.And(_identifier, source.Identifier);
                return result;
            }
            else if (source is BitString)
            {
                FuzzyBitString result = new FuzzyBitString(this);
                result.andNonFuzzy((BitString)source);
                result._identifier = FormulaHelper.And(_identifier, source.Identifier);
                return result;
            }
            else if (source is EmptyBitString)
            {
                return new FuzzyBitString(this);
            }
            else if (source is FalseBitString)
            {
                return source;
            }
            else if (source is TrueBitString)
            {
                return new FuzzyBitString(this);
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public IBitString AndInPlace(IBitString source)
        {
            if (source is FuzzyBitString)
            {
                this.and((FuzzyBitString)source);
                this._identifier = FormulaHelper.And(_identifier, source.Identifier);
                return this;
            }
            else if (source is BitString)
            {
                this.andNonFuzzy((BitString)source);
                this._identifier = FormulaHelper.And(_identifier, source.Identifier);
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
            else if (source is TrueBitString)
            {
                return this;
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// The conjunction of fuzzy and crisp bit string
        /// </summary>
        /// <param name="source">the other operand</param>
        private void andNonFuzzy(BitString source)
        {
            if (_size == 0)
                throw new InvalidOperationException("BitString was not initialized (use create method first).");
            if (_size != source.Length)
                throw Exceptions.BitStringsLengtsAreNotEqualError();

            //all but the last one
            for (int i = 0; i < _array.Length - 1; i++)
            {
                Vector4 tmp = new Vector4(
                    source.GetBit(_blocksize * i),
                    source.GetBit(_blocksize * i + 1),
                    source.GetBit(_blocksize * i + 2),
                    source.GetBit(_blocksize * i + 3));
                _array[i] *= tmp;
            }

            //there has to be at least one item in the last vector
            Vector4 last = new Vector4();
            last.X = source.GetBit(_blocksize * (_array.Length - 1));
            if (_size % _blocksize == 2)
            {
                last.Y = source.GetBit(_blocksize * (_array.Length - 1) + 1);
            }
            if (_size % _blocksize == 3)
            {
                last.Y = source.GetBit(_blocksize * (_array.Length - 1) + 1);
                last.Z = source.GetBit(_blocksize * (_array.Length - 1) + 2);
            }
            if (_size % _blocksize == 0)
            {
                last.Y = source.GetBit(_blocksize * (_array.Length - 1) + 1);
                last.Z = source.GetBit(_blocksize * (_array.Length - 1) + 2);
                last.W = source.GetBit(_blocksize * (_array.Length - 1) + 3);
            }
            _array[_array.Length - 1] *= last;
        }

        /// <summary>
        /// Safe (managed) computation of the fuzzy conjuction -
        /// algebraic product
        /// </summary>
        /// <param name="source">the other operand</param>
        private void andSafe(FuzzyBitString source)
        {
            for (int i = 0; i < _array.Length; i++)
            {
                _array[i] *= source._array[i];
            }
        }

        /// <summary>
        /// Unsafe conjunction.
        /// The method uses the algebraic product T(a,b) = a*b as
        /// a conjunction.
        /// </summary>
        /// <param name="source">the other operand</param>
        private unsafe void and(FuzzyBitString source)
        {
            if (_size == 0)
                throw new InvalidOperationException("BitString was not initialized (use create method first).");
            if (_size != source._size)
                throw Exceptions.BitStringsLengtsAreNotEqualError();

            fixed (Vector4* thisPin = _array, sourcePin = source._array)
            {
                Vector4* currentPtr = thisPin, sourcePtr = sourcePin,
                    stopPtr = thisPin + _array.Length;
                while (currentPtr < stopPtr)
                {
                    *currentPtr++ *= *sourcePtr++;
                }
            }
        }

        #endregion

        #region Or

        /// <summary>
        /// Performs the bitwise OR operation on current BitString against the specified BitString.
        /// </summary>
        /// <param name="source">The second BitString operand.</param>
        /// <returns>Result of the OR operation</returns>
        public IBitString Or(IBitString source, bool precomputeSum = false)
        {
            if (source is FuzzyBitString)
            {
                FuzzyBitString result = new FuzzyBitString(this);
                result.or((FuzzyBitString)source);
                result._identifier = FormulaHelper.Or(_identifier, source.Identifier);
                return result;
            }
            else if (source is BitString)
            {
                FuzzyBitString result = new FuzzyBitString(this);
                result.orNonFuzzy((BitString)source);
                result._identifier = FormulaHelper.Or(_identifier, source.Identifier);
                return result;
            }
            else if (source is EmptyBitString)
            {
                return new FuzzyBitString(this);
            }
            else if (source is FalseBitString)
            {
                return new FuzzyBitString(this);
            }
            else if (source is TrueBitString)
            {
                return source;
            }
            else
                throw new NotImplementedException();
        }

        public IBitString OrInPlace(IBitString source)
        {
            if (source is FuzzyBitString)
            {
                this.or((FuzzyBitString)source);
                this._identifier = FormulaHelper.Or(_identifier, source.Identifier);
                return this;
            }
            else if (source is BitString)
            {
                this.orNonFuzzy((BitString)source);
                this._identifier = FormulaHelper.Or(_identifier, source.Identifier);
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
            else if (source is TrueBitString)
            {
                return source;
            }
            else
                throw new NotImplementedException();
        }

        /// <summary>
        /// Unsafe disjunction.
        /// The method uses the algebraic sum S(a,b) = a + b - ab
        /// as a disjunction.
        /// </summary>
        /// <param name="source">the other operand</param>
        private unsafe void or(FuzzyBitString source)
        {
            if (_size == 0)
                throw new InvalidOperationException("BitString was not initialized (use create method first).");
            if (_size != source._size)
                throw Exceptions.BitStringsLengtsAreNotEqualError();

            fixed (Vector4* thisPin = _array, sourcePin = source._array)
            {
                Vector4* currentPtr = thisPin, sourcePtr = sourcePin,
                    stopPtr = thisPin + _array.Length;
                while (currentPtr < stopPtr)
                {
                    *currentPtr= *sourcePtr + *currentPtr - (*sourcePtr)*(*currentPtr);
                    currentPtr++;
                    sourcePtr++;
                }
            }
        }

        /// <summary>
        /// The disjunction of fuzzy and crisp bit string.
        /// The method uses the algebraic sum S(a,b) = a + b - ab
        /// as a disjunction.
        /// </summary>
        /// <param name="source">the other operand</param>
        private void orNonFuzzy(BitString source)
        {
            if (_size == 0)
                throw new InvalidOperationException("BitString was not initialized (use create method first).");
            if (_size != source.Length)
                throw Exceptions.BitStringsLengtsAreNotEqualError();

            //all but the last one
            for (int i = 0; i < _array.Length - 1; i++)
            {
                Vector4 tmp = new Vector4(
                    source.GetBit(_blocksize * i),
                    source.GetBit(_blocksize * i + 1),
                    source.GetBit(_blocksize * i + 2),
                    source.GetBit(_blocksize * i + 3));
                _array[i] = tmp + _array[i] - tmp*_array[i];
            }

            //there has to be at least one item in the last vector
            Vector4 last = new Vector4();
            last.X = source.GetBit(_blocksize * (_array.Length - 1));
            if (_size % _blocksize == 2)
            {
                last.Y = source.GetBit(_blocksize * (_array.Length - 1) + 1);
            }
            if (_size % _blocksize == 3)
            {
                last.Y = source.GetBit(_blocksize * (_array.Length - 1) + 1);
                last.Z = source.GetBit(_blocksize * (_array.Length - 1) + 2);
            }
            if (_size % _blocksize == 0)
            {
                last.Y = source.GetBit(_blocksize * (_array.Length - 1) + 1);
                last.Z = source.GetBit(_blocksize * (_array.Length - 1) + 2);
                last.W = source.GetBit(_blocksize * (_array.Length - 1) + 3);
            }
            _array[_array.Length - 1] = last + _array[_array.Length - 1] - last * _array[_array.Length - 1];
        }

        #endregion

        #region Not

        /// <summary>
        /// Performs the bitwise NOT on current BitString. The used fuzzy negation
        /// is the Lukasiewicz negation (standard complement), that is N(x) = 1 - x. It the future, we
        /// also consider other negations.
        /// </summary>
        public IBitString Not()
        {
            if (_size == 0)
                throw new InvalidOperationException("BitString was not initialized (use create method first).");

            FuzzyBitString result = new FuzzyBitString(this);
            result.not();
            result._identifier = FormulaHelper.Not(_identifier);
            return result;
        }

        /// <summary>
        /// Unsafe (unmanaged) negation.
        /// Method that switches a fuzzy bit string for its negation. 
        /// The used fuzzy negation
        /// is the Lukasiewicz negation (standard complement), that is N(x) = 1 - x.
        /// </summary>
        private unsafe void not()
        {
            Vector4 tmp = new Vector4(1f, 1f, 1f, 1f);

            fixed (Vector4* arrayPtr = _array)
            {
                Vector4* currentPtr = arrayPtr, stopPtr = arrayPtr + _array.Length;
                while (currentPtr < stopPtr)
                {
                    *currentPtr = tmp - *currentPtr;
                    currentPtr++;
                }
            }
        }

        /// <summary>
        /// Safe (managed) negation.
        /// Method that switches a fuzzy bit string for its negation. 
        /// The used fuzzy negation
        /// is the Lukasiewicz negation (standard complement), that is N(x) = 1 - x.
        /// </summary>
        private void notSafe()
        {
            Vector4 tmp = new Vector4(1f, 1f, 1f, 1f);
            for (int i = 0; i < _array.Length; i++)
            {
                _array[i] = tmp - _array[i];
            }
        }

        #endregion

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
                if (_size == 0)
                    throw new InvalidOperationException("BitString was not initialized (use create method first).");

                lock (this)
                {
                    //using a cache, computing only once
                    if (_nonZeroBitsCount >= 0)
                    {
                        return _nonZeroBitsCount;
                    }

                    long result = 0;
                    for (int i = 0; i < _array.Length - 1; i++)
                    {
                        result += _array[i].X > 0 ? 1 : 0;
                        result += _array[i].Y > 0 ? 1 : 0;
                        result += _array[i].Z > 0 ? 1 : 0;
                        result += _array[i].W > 0 ? 1 : 0;
                    }

                    //there has to be at least one item in the last vector
                    result += _array[_array.Length - 1].X > 0 ? 1 : 0;
                    if (_size % _blocksize == 2)
                    {
                        result += _array[_array.Length - 1].Y > 0 ? 1 : 0;
                    }
                    if (_size % _blocksize == 3)
                    {
                        result += _array[_array.Length - 1].Y > 0 ? 1 : 0;
                        result += _array[_array.Length - 1].Z > 0 ? 1 : 0;
                    }
                    if (_size % _blocksize == 0)
                    {
                        result += _array[_array.Length - 1].Y > 0 ? 1 : 0;
                        result += _array[_array.Length - 1].Z > 0 ? 1 : 0;
                        result += _array[_array.Length - 1].W > 0 ? 1 : 0;
                    }

                    if (result < 0)
                    {
                        throw Exceptions.BitStringLengthError();
                    }
                    if (result > _size)
                    {
                        throw Exceptions.BitStringLengthError2();
                    }

                    _nonZeroBitsCount = result;
                    return _nonZeroBitsCount;
                }
            }
        }

        /// <summary>
        /// Performs the bitwise SUM operation on current BitString.
        /// </summary>
        public float Sum
        {
            set { }
            get
            {
                if (_size == 0)
                    throw new InvalidOperationException("BitString was not initialized (use create method first).");

                lock (this)
                {
                    //using a cache, computing only once
                    if (_sum >= 0)
                    {
                        return _sum;
                    }

                    //sumSafe();
                    sumUnsafe();

                    return _sum;
                }
            }
        }

        /// <summary>
        /// Unsafe (unmanaged) computation of the fuzzy bit string sum
        /// </summary>
        private unsafe void sumUnsafe()
        {
            Vector4 res = new Vector4(0f, 0f, 0f, 0f);

            fixed (Vector4* arrayPtr = _array)
            {
                Vector4* currentPtr = arrayPtr, stopPtr = arrayPtr + _array.Length - 1;
                while (currentPtr < stopPtr)
                {
                    res += *currentPtr++;
                }
            }

            //there has to be at least one item in the last vector
            float result = _array[_array.Length - 1].X;
            if (_size % _blocksize == 2)
            {
                result += _array[_array.Length - 1].Y;
            }
            if (_size % _blocksize == 3)
            {
                result += _array[_array.Length - 1].Y;
                result += _array[_array.Length - 1].Z;
            }
            if (_size % _blocksize == 0)
            {
                result += _array[_array.Length - 1].Y;
                result += _array[_array.Length - 1].Z;
                result += _array[_array.Length - 1].W;
            }
            result += res.X + res.Y + res.Z + res.W;

            _sum = result;
        }

        /// <summary>
        /// Safe (managed) computation of the fuzzy bit string sum
        /// </summary>
        private void sumSafe()
        {
            Vector4 res = new Vector4(0f, 0f, 0f, 0f);
            //counting all but last vector
            for (int i = 0; i < _array.Length - 1; i++)
            {
                res += _array[i];
            }

            //there has to be at least one item in the last vector
            float result = _array[_array.Length - 1].X;
            if (_size % _blocksize == 2)
            {
                result += _array[_array.Length - 1].Y;
            }
            if (_size % _blocksize == 3)
            {
                result += _array[_array.Length - 1].Y;
                result += _array[_array.Length - 1].Z;
            }
            if (_size % _blocksize == 0)
            {
                result += _array[_array.Length - 1].Y;
                result += _array[_array.Length - 1].Z;
                result += _array[_array.Length - 1].W;
            }
            result += res.X + res.Y + res.Z + res.W;

            _sum = result;
        }

        #region IBitStringCreate

        /// <summary>
        /// Fills the whole BitString with the specified value. In case of 
        /// crisp bit strings, the value is 1 or 0, in case of fuzzy bit strings,
        /// the value is a float [0,1].
        /// </summary>
        /// <param name="value">Value to be filled into every "bit" of the BitString.</param>
        public void Fill(float value)
        {
            if (value < 0 || value > 1)
            {
                throw new ArgumentException("Value of the fuzzy bit string was either < 0 or > 1.");
            }
            if (_size == 0)
            {
                throw new InvalidOperationException("BitString was not initialized (use create method first).");
            }

            //Adding all the Vector4's that are sure to be full
            for (int i = 0; i < _array.Length - 1; i++)
            {
                _array[i].X = value;
                _array[i].Y = value;
                _array[i].Z = value;
                _array[i].W = value;
            }

            //Adding the last Vector4 - there has to be at least one item
            _array[_array.Length - 1].X = value;

            if (_size % _blocksize == 2)
            {
                _array[_array.Length - 1].Y = value;
            }
            if (_size % _blocksize == 3)
            {
                _array[_array.Length - 1].Y = value;
                _array[_array.Length - 1].Z = value;
            }
            if (_size % _blocksize == 0)
            {
                _array[_array.Length - 1].Y = value;
                _array[_array.Length - 1].Z = value;
                _array[_array.Length - 1].W = value;
            }
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
            switch (index % _blocksize)
            {
                case 0:
                    return _array[index / _blocksize].X;
                case 1:
                    return _array[index / _blocksize].Y;
                case 2:
                    return _array[index / _blocksize].Z;
                case 3:
                    return _array[index / _blocksize].W;
                default:
                    throw new ArgumentOutOfRangeException("index",
                                                      "the block size is not equal to 4");
            }
        }

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
            if (value < 0 || value > 1)
            {
                throw new ArgumentException("Value of the fuzzy bit string was either < 0 or > 1.");
            }

            switch (index % _blocksize)
            {
                case 0:
                    _array[index / _blocksize].X = value;
                    break;
                case 1:
                    _array[index / _blocksize].Y = value;
                    break;
                case 2:
                    _array[index / _blocksize].Z = value;
                    break;
                case 3:
                    _array[index / _blocksize].W = value;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("index",
                                                      "the block size is not equal to 4");
            }
        }

        #endregion

        /// <summary>
        /// Returns the human-readable string of the BitString contents.
        /// </summary>
        /// <returns>Human-readable string of the BitString contents.</returns>
        public override string ToString()
        {
            if (_size > 0)
            {
                StringBuilder retValue = new StringBuilder();
                //Adding all the Vector4's that are sure to be full
                for (int i = 0; i < _array.Length - 1; i++)
                {
                    retValue.AppendFormat("|{0:F3}|{1:F3}|{2:F3}|{3:F3}|",
                        _array[i].X, _array[i].Y, _array[i].Z, _array[i].W);
                }
                //Adding the last Vector4 - there has to be at least one item
                retValue.AppendFormat("{0:F3}|", _array[_array.Length - 1].X);

                if (_size % _blocksize == 2)
                {
                    retValue.AppendFormat("{0:F3}|", _array[_array.Length - 1].Y);
                }
                if (_size % _blocksize == 3)
                {
                    retValue.AppendFormat("{0:F3}|{1:F3}|",
                        _array[_array.Length - 1].Y, _array[_array.Length - 1].Z);
                }
                if (_size % _blocksize == 0)
                {
                    retValue.AppendFormat("{0:F3}|{1:F3}|{2:F3}|",
                        _array[_array.Length - 1].Y, _array[_array.Length - 1].Z, _array[_array.Length - 1].W);
                }
                return retValue.ToString();
            }
            else
            {
                return "uninitialized fuzzy BitString";
            }
        }
    }
}
