using System.Collections.Generic;

namespace Ferda.Guha.MiningProcessor.Formulas
{
    public class ConjunctionFormula : BooleanAttributeFormula
    {
        private readonly List<BooleanAttributeFormula> _operands;

        public List<BooleanAttributeFormula> Operands
        {
            get { return _operands; }
        }
        
        private static void addOperand(List<BooleanAttributeFormula> list, BooleanAttributeFormula operand)
        {
            // flattening
            ConjunctionFormula conjunction = operand as ConjunctionFormula;
            if (conjunction == null)
                list.Add(operand);
            else
                list.AddRange(conjunction.Operands);            
        }

        public ConjunctionFormula(BooleanAttributeFormula[] operands)
        {
            _operands = new List<BooleanAttributeFormula>(2);
            foreach (BooleanAttributeFormula operand in operands)
            {
                addOperand(_operands, operand);
            }
        }

        public ConjunctionFormula(BooleanAttributeFormula operandA, BooleanAttributeFormula operandB)
        {
            _operands = new List<BooleanAttributeFormula>(2);
            addOperand(_operands, operandA);
            addOperand(_operands, operandB);
        }

        public override string ToString()
        {
            List<string> result = new List<string>();
            foreach (BooleanAttributeFormula formula in _operands)
            {
                result.Add(formula.ToString());
            }
            return FormulaHelper.SequenceToString(result, FormulaSeparator.And, true);
        }
    }
}