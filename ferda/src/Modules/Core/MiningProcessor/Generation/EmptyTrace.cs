using System;
using System.Collections;
using System.Collections.Generic;
using Ferda.Guha.MiningProcessor.BitStrings;
using Ferda.Modules.Helpers.Common;

namespace Ferda.Guha.MiningProcessor.Generation
{
    public class EmptyTrace : IEntityEnumerator
    {
        #region IEntityEnumerator Members

        public long TotalCount
        {
            get { return 1; }
        }

        #endregion

        #region IEnumerable<IBitString> Members

        public IEnumerator<IBitString> GetEnumerator()
        {
            yield return EmptyBitString.GetInstance();
        }

        #endregion

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public Set<string> UsedAttributes
        {
            get { return new Set<string>(); }
        }

        public Set<string> UsedEntities
        {
            get { return new Set<string>(); }
        }

        #endregion

        #region IEntityEnumerator Members


        public MarkEnum CedentType
        {
            get { throw new NotImplementedException("This should be never invoked."); }
        }

        #endregion
    }
}