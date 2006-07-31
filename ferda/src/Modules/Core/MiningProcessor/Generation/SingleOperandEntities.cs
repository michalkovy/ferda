using System.Collections.Generic;
using Ferda.Guha.Math;
using Ferda.Guha.MiningProcessor.BitStrings;

namespace Ferda.Guha.MiningProcessor.Generation
{
    public class Negation : SingleOperandEntity
    {
        public Negation(NegationSetting setting, ISkipOptimalization skipOptimalization, MarkEnum cedentType)
            : base(setting, skipOptimalization, cedentType)
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

        public override SkipSetting BaseSkipSetting(MarkEnum cedentType)
        {
            SkipSetting parentSkipSetting = ParentSkipOptimalization.BaseSkipSetting(cedentType);
            if (parentSkipSetting == null)
                return null;
            RelationEnum newRelation;
            switch (parentSkipSetting.Relation)
            {
                case RelationEnum.Equal:
                    newRelation = RelationEnum.Equal;
                    break;
                case RelationEnum.Greater:
                    newRelation = RelationEnum.LessOrEqual;
                    break;
                case RelationEnum.GreaterOrEqual:
                    newRelation = RelationEnum.Less;
                    break;
                case RelationEnum.Less:
                    newRelation = RelationEnum.GreaterOrEqual;
                    break;
                case RelationEnum.LessOrEqual:
                    newRelation = RelationEnum.Greater;
                    break;
                default:
                    throw new System.NotImplementedException();
            }
            return new SkipSetting(newRelation, parentSkipSetting.NotTreshold, parentSkipSetting.Treshold);
        }
    }
    
    public class BothSigns : SingleOperandEntity
    {
        public BothSigns(BothSignsSetting setting, ISkipOptimalization skipOptimalization, MarkEnum cedentType)
            : base(setting, skipOptimalization, cedentType)
        {
        }

        public override IEnumerator<IBitString> GetBitStringEnumerator()
        {
            foreach (IBitString bitString in _entity)
            {
                SkipSetting skipSetting = ParentSkipOptimalization.BaseSkipSetting(CedentType);
                if (skipSetting == null)
                {
                    yield return bitString;
                    yield return bitString.Not();
                }
                else
                {
                    if (Ferda.Guha.Math.Common.Compare(skipSetting.Relation, bitString.Sum, skipSetting.Treshold))
                        yield return bitString;
                    IBitString negation = bitString.Not();
                    if (Ferda.Guha.Math.Common.Compare(skipSetting.Relation, negation.Sum, skipSetting.Treshold))
                        yield return negation;
                }
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

        public override SkipSetting BaseSkipSetting(MarkEnum cedentType)
        {
            // UNDONE
            return null;
        }
    }
}