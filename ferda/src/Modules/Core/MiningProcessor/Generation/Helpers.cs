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
                            result = cache[attributeId, var];
                        else
                            result = result.And(cache[attributeId, var]);
                    }
                    return result;
                case BitwiseOperation.Or:
                    foreach (string var in categoriesIds)
                    {
                        if (result == null)
                            result = cache[attributeId, var];
                        else
                            result = result.Or(cache[attributeId, var]);
                    }
                    return result;
                default:
                    throw new NotImplementedException();
            }
        }

        public static IBitString[] GetBitStrings(BitStringGeneratorPrx bitStringGeneratorPrx, Guid attributeId)
        {
            IBitStringCache cache = BitStringCache.GetInstance(bitStringGeneratorPrx);
            string[] categoriesIds = cache.GetCategoriesIds(attributeId);
            IBitString[] result = new IBitString[categoriesIds.Length];
            for (int i = 0; i < categoriesIds.Length; i++)
			{
			    result[i] = cache[attributeId, categoriesIds[i]];
			}
            return result;
        }
    }
}