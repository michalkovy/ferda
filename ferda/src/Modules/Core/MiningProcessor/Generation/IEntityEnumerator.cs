using System.Collections.Generic;
using Ferda.Guha.MiningProcessor.BitStrings;

namespace Ferda.Guha.MiningProcessor
{
    public interface IEntityEnumerator : IEnumerable<IBitString>
    {
        long TotalCount { get;}
    }
}