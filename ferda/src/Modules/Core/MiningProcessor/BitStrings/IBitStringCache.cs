using System;

namespace Ferda.Guha.MiningProcessor.BitStrings
{
    /// <summary>
    /// Gets bit strings by the identifier form the cache (cache i.e. bitStringCollection)
    /// or from corresponding generator (BitStringGeneratorPrxSeq)
    /// </summary>
    public interface IBitStringCache
    {
        //static IBitStringCache GetInstance(BitStringGeneratorPrx prx);

        /// <summary>
        /// Gets or the count of bit strings in the cache.
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Gets or the actual cache size.
        /// </summary>
        int ActSize { get; }

        /// <summary>
        /// Gets or sets the maximum cache size.
        /// </summary>
        int MaxSize { get; set; }

        /// <summary>
        /// Allows to check whether the cache contains some concrete bit string (identified by guid)
        /// </summary>
        /// <param name="bitStringId">Bit string identification</param>
        /// <returns>True if cache contains specified bit string, otherwise false</returns>
        bool Contains(BitStringIdentifier bitStringId);

        /// <summary>
        /// Allows to obtain bit string identified by its guid.
        /// Internally distinguish between two possible cases.
        /// First case - bit string is stored in cache, leads to simple handling. Stored bit string is returned to the caller.
        /// Second case - bit string is not found in cache, leads to request to data preprocesor. Obtained string is first stored in cache and then returned to the caller.
        /// Cache is transparent for any exceptions from data preprocesor.
        /// </summary>
        /// <value></value>
        /// <returns>Bit string responding to given guid that is stored in cache or obtained from data preprocesor.</returns>
        IBitString this[BitStringIdentifier bitStringId] { get; }
        IBitString this[Guid attributeId, string categoryId] { get; }
        
        IBitString GetMissingInformationBitString(Guid attributeId);
        
        string[] GetCategoriesIds(Guid attributeId);
    }
}