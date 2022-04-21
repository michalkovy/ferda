// SubsetsGenerationHelper.cs - Helper classes to generate subsets
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
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace Ferda.Guha.MiningProcessor.Generation
{
    internal class LongMultiplicationArraySubsetsInstance : SubsetsInstance<long, long>
    {
        private long[] _items;

        public LongMultiplicationArraySubsetsInstance(long[] items)
        {
            _items = items;
        }

        #region SubsetsInstance<int,int[]> Members

        public long operation(long previous, long current)
        {
            return previous * current;
        }

        public long operation(long current)
        {
            return current;
        }

        public long getItem(int index)
        {
            return _items[index];
        }

        public long getDefaultInit()
        {
            return 0;
        }

        public bool skipOptimize(long current)
        {
            return true;
        }

        #endregion
    }

    public interface SubsetsInstance<T, M>
    {
        M operation(M previous, T current);
        M operation(T current);
        T getItem(int index);
        M getDefaultInit();
        bool skipOptimize(M current);
    }

    public class Subsets<T, M> : IEnumerable<M>
    {
        private int _effectiveMinLength;
        private int _effectiveMaxLength;
        private int _itemsCount;
        private SubsetsInstance<T, M> _instance;

        public Subsets(int effectiveMinLength, int effectiveMaxLength, int itemsCount, SubsetsInstance<T, M> instance)
        {
            _effectiveMinLength = effectiveMinLength;
            _effectiveMaxLength = effectiveMaxLength;
            _itemsCount = itemsCount;
            _instance = instance;
        }

        private Stack<M> sB = new Stack<M>();
        private Stack<int> sI = new Stack<int>();

        private void sBPush(T adding)
        {
            if (sB.Count > 0)
            {
                M previous = sB.Peek();
                sB.Push(_instance.operation(previous, adding));
            }
            else
            {
                sB.Push(_instance.operation(adding));
            }
        }

        private bool returnCurrent(out M result)
        {
            Debug.Assert(sB.Count <= _effectiveMaxLength);
            if (sB.Count >= _effectiveMinLength)
            {
                result = sB.Peek();
                return true;
            }
            result = _instance.getDefaultInit();
            return false;
        }

        private void getEntity(int index)
        {
            sBPush(_instance.getItem(index));
            sI.Push(index);
        }

        private bool prolong(bool afterRemove)
        {
            if (sB.Count == _effectiveMaxLength) // not after remove
                return false;
            int newIndex;
            if (afterRemove)
                newIndex = sI.Pop() + 1;
            else
                newIndex = sI.Peek() + 1;
            if (newIndex >= _itemsCount)
                return false;
            getEntity(newIndex);
            return true;
        }

        private bool removeLastItem()
        {
            if (sB.Count > 0)
            {
                sB.Pop();
                return true;
            }
            return false;
        }

        #region IEnumerable<M> Members

        public IEnumerator<M> GetEnumerator()
        //public override IEnumerator<IBitString> GetBitStringEnumerator()
        {
            M result;
            bool afterRemove;
            afterRemove = false;

            #region initialize

            sB.Clear();
            sI.Clear();
            getEntity(0);

        #endregion

        returnCurrent:
            if (returnCurrent(out result))
            {
                if (_instance.skipOptimize(result))
                {
                    yield return result;
                }
            }
        prolong:
            if (prolong(afterRemove))
            {
                afterRemove = false;
                goto returnCurrent;
            }
            if (removeLastItem())
            {
                afterRemove = true;
                goto prolong;
            }
        }

        #endregion

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }
}