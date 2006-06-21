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

        public override long TotalCount
        {
            get { return _entity.TotalCount; }
        }

        public override string ToString()
        {
            return "Negation of <" + _entity.ToString() + ">";
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

        public override long TotalCount
        {
            get { return _entity.TotalCount * 2; }
        }
        
        public override string ToString()
        {
            return "Both signs of <" + _entity.ToString() + ">";
        }
    }
}