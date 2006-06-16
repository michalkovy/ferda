using System;
using Ferda.Guha.MiningProcessor.BitStrings;

namespace Ferda.Guha.MiningProcessor.Generation
{
    public class Conjunction : MutliOperandEntity
    {
        public Conjunction(IMultipleOperandEntitySetting setting)
            : base(setting)
        {
        }

        protected override IBitString operation(IBitString operand1, IBitString operand2)
        {
            if (operand1 == null)
                throw new ArgumentNullException("operand1");
            if (operand2 == null)
                throw new ArgumentNullException("operand2");
            return operand1.And(operand2);
        }
    }

    public class Disjunction : MutliOperandEntity
    {
        public Disjunction(IMultipleOperandEntitySetting setting)
            : base(setting)
        {
        }

        protected override IBitString operation(IBitString operand1, IBitString operand2)
        {
            if (operand1 == null)
                throw new ArgumentNullException("operand1");
            if (operand2 == null)
                throw new ArgumentNullException("operand2");
            return operand1.Or(operand2);
        }
    }
}