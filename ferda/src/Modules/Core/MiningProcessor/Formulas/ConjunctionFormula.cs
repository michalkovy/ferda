using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Ferda.Guha.MiningProcessor.Formulas
{
    public class ConjunctionFormula : BooleanAttributeFormula
    {
        private readonly BooleanAttributeFormula[] _operands;

        public BooleanAttributeFormula[] Operands
        {
            get { return _operands; }
        }
        
        public ConjunctionFormula(BooleanAttributeFormula[] operands)
        {
            // flattening
            List<BooleanAttributeFormula> operandsLists = new List<BooleanAttributeFormula>();
            foreach (BooleanAttributeFormula operand in operands)
            {
                ConjunctionFormula conjunction = operand as ConjunctionFormula;
                if (conjunction == null)
                    operandsLists.Add(operand);
                else
                    operandsLists.AddRange(conjunction.Operands);
            }

            _operands = operandsLists.ToArray();
        }

        public ConjunctionFormula(BooleanAttributeFormula operandA, BooleanAttributeFormula operandB)
            : this(new BooleanAttributeFormula[] { operandA, operandB })
        {
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