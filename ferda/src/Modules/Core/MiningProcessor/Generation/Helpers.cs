using System;
using Ferda.Guha.MiningProcessor.BitStrings;

namespace Ferda.Guha.MiningProcessor.Generation
{
    public enum BitwiseOperation
    {
        And,
        Or
    }

    public static class Helpers
    {
        public static IBitString GetBitString(BitStringGeneratorPrx bitStringGeneratorPrx, Guid attributeId,
                                              string[] categoriesIds, BitwiseOperation operation)
        {
            IBitString result = null;
            IBitStringCache cache = BitStringCache.GetInstance(bitStringGeneratorPrx);
            switch (operation)
            {
                case BitwiseOperation.And:
                    foreach (string var in categoriesIds)
                    {
                        if (result == null)
                            result = cache.GetBitString(new BitStringIdentifier(attributeId, var));
                        else
                            result = result.And(cache.GetBitString(new BitStringIdentifier(attributeId, var)));
                    }
                    return result;
                case BitwiseOperation.Or:
                    foreach (string var in categoriesIds)
                    {
                        if (result == null)
                            result = cache.GetBitString(new BitStringIdentifier(attributeId, var));
                        else
                            result = result.Or(cache.GetBitString(new BitStringIdentifier(attributeId, var)));
                    }
                    return result;
                default:
                    throw new NotImplementedException();
            }
        }
    }
}