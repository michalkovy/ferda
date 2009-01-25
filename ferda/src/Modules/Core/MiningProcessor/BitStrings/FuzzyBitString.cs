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
    public class FuzzyBitString : IBitStringBase
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
                throw new Exception();
                //throw Exceptions.BitStringLengthError();
            }

            _size = floats.Length;

            // if size mod 4 = 0, the the vectorF size = size/4 else size/4 + 1
            int flength = (_size % 4 == 0) ? _size / 4 : (_size / 4) + 1;
            _array = new Vector4f[flength];

            for (int i = 0; i < flength; i++)
            {
                //so it is sure, that there are 4 floats still to be filled
                if (4 * (i + 1) <= floats.Length)
                {
                    _array[i] = new Vector4f(floats[4 * i],
                        floats[4 * i + 1], floats[4 * i + 2], floats[4 * i + 3]);
                }
                else
                {
                    //there is no other float to be filled
                    if (4 * i == floats.Length + 1)
                    {
                        break;
                    }
                    //there is at least one float to be filled
                    else
                    {
                        _array[i] = new Vector4f();
                        _array[i].X = floats[4 * i];
                        //there is only one
                        if (floats.Length % 4 == 1)
                        {
                            _array[i].Y = -1f;
                            _array[i].Z = -1f;
                            _array[i].W = -1f;
                            break;
                        }
                        //there are at least 2 floats to be filled
                        if (floats.Length % 4 == 2)
                        {
                            _array[i].Y = floats[4 * i + 1];
                            _array[i].Z = -1f;
                            _array[i].W = -1f;
                        }
                        else
                        {
                            _array[i].Y = floats[4 * i + 1];
                            _array[i].Z = floats[4 * i + 2];
                            _array[i].W = -1f;
                        }
                    }
                }
            }
        }

        #endregion

        #region And

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

                if (_size % 4 == 2)
                {
                    retValue.AppendFormat("{0:F3}|", _array[_array.Length - 1].Y);
                }
                if (_size % 4 == 3)
                {
                    retValue.AppendFormat("{0:F3}|{1:F3}|",
                        _array[_array.Length - 1].Y, _array[_array.Length - 1].Z);
                }
                if (_size % 4 == 0)
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
