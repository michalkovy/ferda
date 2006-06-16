using System.Collections.Generic;
using Ferda.Guha.MiningProcessor.BitStrings;

namespace Ferda.Guha.MiningProcessor.Generation
{
    public class Negation : SingleOperandEntity
    {
        public Negation(NegationSetting setting)
            : base(setting)
        {
        }

        public override IEnumerator<IBitString> GetBitStringEnumerator()
        {
            foreach (IBitString bitString in _entity)
            {
                yield return bitString.Not();
            }
        }
    }
    
    public class BothSigns : SingleOperandEntity
    {
        public BothSigns(BothSignsSetting setting)
        : base(setting)
        {
        }

        public override IEnumerator<IBitString> GetBitStringEnumerator()
        {
            foreach (IBitString bitString in _entity)
            {
                yield return bitString;
                yield return bitString.Not();
            }
        }
    }
}