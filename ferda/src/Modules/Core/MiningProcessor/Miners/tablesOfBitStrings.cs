using System;
using System.Collections.Generic;
using System.Text;
using Ferda.Guha.MiningProcessor.BitStrings;

namespace Ferda.Guha.MiningProcessor.Miners
{
    internal class nineFoldTableOfBitStrings
    {
        public IBitString pApB;
        public IBitString pAxB;
        public IBitString pAnB;

        public IBitString xApB;
        public IBitString xAxB;
        public IBitString xAnB;

        public IBitString nApB;
        public IBitString nAxB;
        public IBitString nAnB;
    }

    internal class fourFoldTableOfBitStrings
    {
        public IBitString pSpA;
        public IBitString pSnA;

        public IBitString nSpA;
        public IBitString nSnA;
    }
}
