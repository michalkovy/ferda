using System;
using System.Collections.Generic;
using Ferda.Guha.MiningProcessor.BitStrings;
using Ferda.Modules.Helpers.Common;

namespace Ferda.Guha.MiningProcessor
{
    /// <summary>
    /// Iterates through IBitStrings provider. Please note that no
    /// inheritor of this interface can iterates through 
    /// missing information IBitString i.e. the BitString of 
    /// missing information can not get to output!
    /// </summary>
    public interface IEntityEnumerator : IEnumerable<IBitString>
    {
        long TotalCount { get;}
        Set<Guid> UsedAttributes { get; }
        Set<Guid> UsedEntities { get; }
    }
}