using System;
using System.Collections.Generic;
using Ferda.Guha.MiningProcessor.BitStrings;

namespace Ferda.Guha.MiningProcessor.Generation
{
    public class Conjunction : MutliOperandEntity
    {
        public Conjunction(IMultipleOperandEntitySetting setting, ISkipOptimalization skipOptimalization, MarkEnum cedentType)
            : base(setting, skipOptimalization, cedentType)
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

        public override string ToString()
        {
            List<string> entities = new List<string>();
            foreach (IEntityEnumerator entity in _sourceEntities)
                entities.Add(entity.ToString());
            return "Conjunction of {" 
                   + Formulas.FormulaHelper.SequenceToString(entities, Formulas.FormulaSeparator.AtomMembers, true) 
                   + "}";
        }

        public override SkipSetting BaseSkipSetting(MarkEnum cedentType)
        {
            SkipSetting parentSkipSetting = ParentSkipOptimalization.BaseSkipSetting(cedentType);
            if (parentSkipSetting == null)
                return null;
            // UNDONE (this is OK for >  or >= relations)
            if (Ferda.Guha.Math.Common.GetRelationOrientation(parentSkipSetting.Relation) > 0)
                return ParentSkipOptimalization.BaseSkipSetting(cedentType);
            else
                return null;
        }
    }

    public class Disjunction : MutliOperandEntity
    {
        public Disjunction(IMultipleOperandEntitySetting setting, ISkipOptimalization skipOptimalization, MarkEnum cedentType)
            : base(setting, skipOptimalization, cedentType)
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

        public override string ToString()
        {
            List<string> entities = new List<string>();
            foreach (IEntityEnumerator entity in _sourceEntities)
                entities.Add(entity.ToString());
            return "Disjunction of {"
                   + Formulas.FormulaHelper.SequenceToString(entities, Formulas.FormulaSeparator.AtomMembers, true)
                   + "}";
        }

        public override SkipSetting BaseSkipSetting(MarkEnum cedentType)
        {
            // UNDONE
            // no base skip optimalization for disjunctions
            return null;
        }
    }
}