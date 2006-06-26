using System;
using System.Collections.Generic;
using System.Text;
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
            yield return EmptyBitStringSingleton.EmptyBitString;
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public Set<Guid> UsedAttributes
        {
            get { return new Set<Guid>(); }
        }

        public Set<Guid> UsedEntities
        {
            get { return new Set<Guid>(); }
        }

        #endregion
    }
}
