using System.Collections.Generic;

namespace Ferda.Guha.MiningProcessor.Formulas
{
    public class NegationFormula : IFormula
    {
        private IFormula _operand;

        public IFormula Operand
        {
            get { return _operand; }
        }

        public NegationFormula(IFormula operand)
        {
            _operand = operand;
        }

        public override string ToString()
        {
            string result = _operand.ToString();
            if (result.Contains(Formula.SeparatorAnd) || result.Contains(Formula.SeparatorOr))
                return Formula.NegationSign + "(" + result + ")";
            else
                return Formula.NegationSign + result;
        }
    }
}