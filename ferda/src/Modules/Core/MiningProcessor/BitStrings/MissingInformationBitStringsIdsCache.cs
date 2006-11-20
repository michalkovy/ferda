using Ferda.Modules.Helpers.Caching;

namespace Ferda.Guha.MiningProcessor.BitStrings
{
    internal class MissingInformationBitStringsIdsCache : MostRecentlyUsed<string, string>
    {
        private readonly BitStringCache _bitStringCache;

        private const int cacheDefaultSize = 32;

        public MissingInformationBitStringsIdsCache(BitStringCache bitStringCache)
            : base(cacheDefaultSize)
        {
            _bitStringCache = bitStringCache;
        }

        public override string GetValue(string key)
        {
            BitStringGeneratorPrx proxy =
                _bitStringCache.GetBitStringGeneratorPrx(key);

            if (proxy != null)
            {
                string[] missingInformationCategoryId =
                    _bitStringCache.GetBitStringGeneratorPrx(key).GetMissingInformationCategoryId();
                if (missingInformationCategoryId.Length == 1)
                {
                    return missingInformationCategoryId[0];
                }
            }
            return null;
        }

        public override int GetSize(string itemToMeasure)
        {
            return 1;
        }
    }
}
