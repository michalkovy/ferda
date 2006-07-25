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
        public static IBitString GetBitString(BitStringGeneratorPrx bitStringGeneratorPrx, string attributeGuid,
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
                            result = cache[attributeGuid, var];
                        else
                            result = result.And(cache[attributeGuid, var]);
                    }
                    return result;
                case BitwiseOperation.Or:
                    foreach (string var in categoriesIds)
                    {
                        if (result == null)
                            result = cache[attributeGuid, var];
                        else
                            result = result.Or(cache[attributeGuid, var]);
                    }
                    return result;
                default:
                    throw new NotImplementedException();
            }
        }

        public static IBitString[] GetBitStrings(BitStringGeneratorPrx bitStringGeneratorPrx, string attributeGuid)
        {
            IBitStringCache cache = BitStringCache.GetInstance(bitStringGeneratorPrx);
            string[] categoriesIds = cache.GetCategoriesIds(attributeGuid);
            IBitString[] result = new IBitString[categoriesIds.Length];
            for (int i = 0; i < categoriesIds.Length; i++)
            {
                result[i] = cache[attributeGuid, categoriesIds[i]];
            }
            return result;
        }
    }
}