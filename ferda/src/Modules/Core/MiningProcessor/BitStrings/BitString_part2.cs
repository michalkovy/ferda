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
        #region AND
#if UNSAFE
        private unsafe void andUnsafe(BitString source)
        {
            _andUnsafe(ref _array, _array, source._array);
        }
#if USE64BIT
        private static unsafe void _andUnsafe(ref ulong[] result, ulong[] array1, ulong[] array2)
        {
            fixed (ulong* pinArray1 = array1, pinArray2 = array2, destPin = result)
            {
                ulong* destPtr = destPin, sourcePtr1 = pinArray1, sourcePtr2 = pinArray2, stopPtr = pinArray1 + array1.Length;
                while (destPtr < stopPtr)
                {
                    *destPtr++ = *sourcePtr1++ & *sourcePtr2++;
                }
            }
        }
        private static unsafe ulong[] andUnsafe(ulong[] array1, ulong[] array2)
        {
            ulong[] result = new ulong[array1.Length];
            _andUnsafe(ref result, array1, array2);
            return result;
        }
#else
        private static unsafe void _andUnsafe(ref uint[] result, uint[] array1, uint[] array2)
        {
            fixed (uint* pinArray1 = array1, pinArray2 = array2, destPin = result)
            {
                uint* destPtr = destPin, sourcePtr1 = pinArray1, sourcePtr2 = pinArray2, stopPtr = pinArray1 + array1.Length;
                while (destPtr < stopPtr)
                {
                    *destPtr++ = *sourcePtr1++ & *sourcePtr2++;
                }
            }
        }
        private static unsafe uint[] andUnsafe(uint[] array1, uint[] array2)
        {
            uint[] result = new uint[array1.Length];
            _andUnsafe(ref result, array1, array2);
            return result;
        }
#endif
#else
        private void andSafe(BitString source)
        {
            for (int i = 0; i < _array.Length; i++)
            {
                _array[i] &= source._array[i];
            }
        }
#if USE64BIT
        private static ulong[] andSafe(ulong[] array1, ulong[] array2)
        {
            ulong[] result = new ulong[array1.Length];
            for (int i = 0; i < array1.Length; i++)
            {
                result[i] = array1[i] & array1[2];
            }
            return result;
        }
#else
        private static unsafe uint[] andSafe(uint[] array1, uint[] array2)
        {
            uint[] result = new uint[array1.Length];
            for (int i = 0; i < array1.Length; i++)
            {
                result[i] = array1[i] & array1[2];
            }
            return result;
        }
#endif
#endif
        #endregion

        #region OR
#if UNSAFE
        private unsafe void orUnsafe(BitString source)
        {
            _orUnsafe(ref _array, _array, source._array);
        }
#if USE64BIT
        private static unsafe void _orUnsafe(ref ulong[] result, ulong[] array1, ulong[] array2)
        {
            fixed (ulong* pinArray1 = array1, pinArray2 = array2, destPin = result)
            {
                ulong* destPtr = destPin, sourcePtr1 = pinArray1, sourcePtr2 = pinArray2, stopPtr = pinArray1 + array1.Length;
                while (destPtr < stopPtr)
                {
                    *destPtr++ = *sourcePtr1++ | *sourcePtr2++;
                }
            }
        }
        private static unsafe ulong[] orUnsafe(ulong[] array1, ulong[] array2)
        {
            ulong[] result = new ulong[array1.Length];
            _orUnsafe(ref result, array1, array2);
            return result;
        }
#else
        private static unsafe void _orUnsafe(ref uint[] result, uint[] array1, uint[] array2)
        {
            fixed (uint* pinArray1 = array1, pinArray2 = array2, destPin = result)
            {
                uint* destPtr = destPin, sourcePtr1 = pinArray1, sourcePtr2 = pinArray2, stopPtr = pinArray1 + array1.Length;
                while (destPtr < stopPtr)
                {
                    *destPtr++ = *sourcePtr1++ | *sourcePtr2++;
                }
            }
        }
        private static unsafe uint[] orUnsafe(uint[] array1, uint[] array2)
        {
            uint[] result = new uint[array1.Length];
            _orUnsafe(ref result, array1, array2);
            return result;
        }
#endif

#else
        private void orSafe(BitString source)
        {
            for (int i = 0; i < _array.Length; i++)
            {
                _array[i] |= source._array[i];
            }
        }
#if USE64BIT
        private static ulong[] orSafe(ulong[] array1, ulong[] array2)
        {
            ulong[] result = new ulong[array1.Length];
            for (int i = 0; i < array1.Length; i++)
            {
                result[i] = array1[i] | array1[2];
            }
            return result;
        }
#else
        private static unsafe uint[] orSafe(uint[] array1, uint[] array2)
        {
            uint[] result = new uint[array1.Length];
            for (int i = 0; i < array1.Length; i++)
            {
                result[i] = array1[i] | array1[2];
            }
            return result;
        }
#endif
#endif
        #endregion

        #region NOT

#if UNSAFE
        private unsafe void notUnsafe()
        {
            _notUnsafe(ref _array, _array, _size);
        }
#if USE64BIT
        private static unsafe void _notUnsafe(ref ulong[] result, ulong[] array, int size)
        {
            fixed (ulong* pin = array, destPin = result)
            {
                ulong* destPtr = destPin, ptr = pin, stop = pin + array.Length;
                while (destPtr < stop)
                {
                    *destPtr = ~(*ptr);
                    destPtr++;
                    ptr++;
                }
                if (size % _blockSize > 0)
                {
                    *(destPtr - 1) &= _allOnes >> (_blockSize - size % _blockSize);
                }
            }
        }
        private static unsafe ulong[] notUnsafe(ulong[] array, int size)
        {
            ulong[] result = new ulong[array.Length];
            _notUnsafe(ref result, array, size);
            return result;
        }
#else
        private static unsafe void _notUnsafe(ref uint[] result, uint[] array, int size)
        {
            fixed (uint* pin = array, destPin = result)
            {
                uint* destPtr = destPin, ptr = pin, stop = pin + array.Length;
                while (destPtr < stop)
                {
                    *destPtr = ~(*ptr);
                    destPtr++;
                    ptr++;
                }
                if (size % _blockSize > 0)
                {
                    *(destPtr - 1) &= _allOnes >> (_blockSize - size % _blockSize);
                }
            }
        }
        private static unsafe uint[] notUnsafe(uint[] array, int size)
        {
            uint[] result = new uint[array.Length];
            _notUnsafe(ref result, array, size);
            return result;
        }
#endif
#else
        private void notSafe()
        {
            for (int i = 0; i < _array.Length; i++)
            {
                _array[i] = ~_array[i];
            }
            if (_size % _blockSize > 0)
            {
                _array[_array.Length - 1] &= _allOnes >> (_blockSize - _size % _blockSize);
            }
        }
#if USE64BIT
        private static unsafe ulong[] notSafe(ulong[] array1, int size)
        {
            ulong[] result = new ulong[array1.Length];
            for (int i = 0; i < array1.Length; i++)
            {
                result[i] = ~array1[i];
            }
            if (size % _blockSize > 0)
            {
                result[array1.Length - 1] &= _allOnes >> (_blockSize - size % _blockSize);
            }
            return result;
        }
#else
        private static unsafe uint[] notSafe(uint[] array1, int size)
        {
            uint[] result = new uint[array1.Length];
            for (int i = 0; i < array1.Length; i++)
            {
                result[i] = ~array1[i];
            }
            if (size % _blockSize > 0)
            {
                result[array1.Length - 1] &= _allOnes >> (_blockSize - size % _blockSize);
            }
            return result;
        }
#endif
#endif
        #endregion
    }
}
