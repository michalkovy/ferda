using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Ferda.Guha.MiningProcessor.Formulas
{
    public class NegationFormula : BooleanAttributeFormula
    {
        private readonly BooleanAttributeFormula _operand;

        public BooleanAttributeFormula Operand
        {
            get { return _operand; }
        }

        public NegationFormula(BooleanAttributeFormula operand)
        {
            _operand = operand;
        }

        public override string ToString()
        {
            string result = _operand.ToString();
            if (result.Contains(FormulaHelper.SeparatorAnd) || result.Contains(FormulaHelper.SeparatorOr))
                return FormulaHelper.NegationSign + "(" + result + ")";
            else
                return FormulaHelper.NegationSign + result;
        }
    }
}