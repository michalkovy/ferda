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
        int BitStringsCount { get; }
        bool ContainsBitString(BitStringIdentifier bitStringId);
        IBitString GetBitString(BitStringIdentifier bitStringId);
        IBitString GetMissingInformationBitString(Guid attributeId);
        int MaxSize { get; }
        string[] GetCategoriesIds(Guid attributeId);
    }
}