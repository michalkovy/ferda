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
using System.Text;
using Ferda.Guha.MiningProcessor.Formulas;
using Mono.Simd;

namespace Ferda.Guha.MiningProcessor.BitStrings
{
    public class FuzzyBitString : IBitStringBase, IBitStringCreate
    {
        /// <summary>
        /// Internal array where fuzzy bit strings are stored
        /// </summary>
        private Vector4f[] _array;

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
            _array = new Vector4f[source._array.Length];

            for (int i = 0; i < _array.Length; i++)
            {
                _array[i] = new Vector4f(source._array[i].X, source._array[i].Y, source._array[i].Z, source._array[i].W);
            }
        }

        /// <summary>
        /// Constructor of the class.
        /// </summary>
        /// <param name="identifier">Identifier of the bit string</param>
        /// <param name="floats">Array of floats, which are transformed into
        /// the <see cref="Mono.Simd.Vector4f"/>structures.</param>
        /// <remarks>
        /// In contrary to the constructor of <see cref="BitString"/> class, this constructor
        /// is safe and it does not need the length parameter. The length of the bit string
        /// is determined by the lenght of the array of floats. 
        /// </remarks>
        public FuzzyBitString(BitStringIdentifier identifier, float[] floats)
            : this(new AtomFormula(identifier))
        {
            if (floats.Length == 0)
            {
                throw Exceptions.BitStringLengthError();
            }

            _size = floats.Length;

            // if size mod 4 = 0, the the vectorF size = size/4 else size/4 + 1
            int flength = (_size % _blocksize == 0) ? _size / _blocksize : (_size / _blocksize) + 1;
            _array = new Vector4f[flength];

            for (int i = 0; i < flength; i++)
            {
                //so it is sure, that there are 4 floats still to be filled
                if (_blocksize * (i + 1) <= floats.Length)
                {
                    _array[i] = new Vector4f(floats[_blocksize * i],
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
                        _array[i] = new Vector4f();
                        _array[i].X = floats[_blocksize * i];
                        //there is only one
                        if (floats.Length % _blocksize == 1)
                        {
                            _array[i].Y = -1f;
                            _array[i].Z = -1f;
                            _array[i].W = -1f;
                            break;
                        }
                        //there are at least 2 floats to be filled
                        if (floats.Length % _blocksize == 2)
                        {
                            _array[i].Y = floats[_blocksize * i + 1];
                            _array[i].Z = -1f;
                            _array[i].W = -1f;
                        }
                        else
                        {
                            _array[i].Y = floats[_blocksize * i + 1];
                            _array[i].Z = floats[_blocksize * i + 2];
                            _array[i].W = -1f;
                        }
                    }
                }
            }
        }

        #endregion

        #region And

        #endregion

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

            //Adding all the Vector4f's that are sure to be full
            for (int i = 0; i < _array.Length - 1; i++)
            {
                _array[i].X = value;
                _array[i].Y = value;
                _array[i].Z = value;
                _array[i].W = value;
            }

            //Adding the last Vector4f - there has to be at least one item
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
                //Adding all the Vector4f's that are sure to be full
                for (int i = 0; i < _array.Length - 1; i++)
                {
                    retValue.AppendFormat("|{0:F3}|{1:F3}|{2:F3}|{3:F3}|",
                        _array[i].X, _array[i].Y, _array[i].Z, _array[i].W);
                }
                //Adding the last Vector4f - there has to be at least one item
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
