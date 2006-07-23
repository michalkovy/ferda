using System;
using System.Collections.Generic;
using System.Text;
using Ferda.Guha.MiningProcessor.BitStrings;

namespace Ferda.Guha.MiningProcessor.Miners
{
    internal class nineFoldTableOfBitStrings
    {
        public IBitString pSpA;
        public IBitString pSxA;
        public IBitString pSnA;

        public IBitString xSpA;
        public IBitString xSxA;
        public IBitString xSnA;

        public IBitString nSpA;
        public IBitString nSxA;
        public IBitString nSnA;
    }

    internal class fourFoldTableOfBitStrings
    {
        public IBitString pSpA;
        public IBitString pSnA;

        public IBitString nSpA;
        public IBitString nSnA;
    }
}
