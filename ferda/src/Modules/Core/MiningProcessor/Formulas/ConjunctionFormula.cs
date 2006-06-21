using System;
using System.Collections.Generic;

namespace Ferda.Guha.MiningProcessor.Formulas
{
    public class ConjunctionFormula : IFormula
    {
        private readonly IFormula[] _operands;

        public IFormula[] Operands
        {
            get { return _operands; }
        }

        public ConjunctionFormula(IFormula[] operands)
        {
            // flattening
            List<IFormula> operandsLists = new List<IFormula>();
            foreach (IFormula operand in operands)
            {
                ConjunctionFormula conjunction = operand as ConjunctionFormula;
                if (conjunction == null)
                    operandsLists.Add(operand);
                else
                    operandsLists.AddRange(conjunction.Operands);
            }

            _operands = operandsLists.ToArray();
        }

        public ConjunctionFormula(IFormula operandA, IFormula operandB)
            : this(new IFormula[] { operandA, operandB })
        {
        }

        public override string ToString()
        {
            List<string> result = new List<string>();
            foreach (IFormula formula in _operands)
            {
                result.Add(formula.ToString());
            }
            return Formula.SequenceToString(result, FormulaSeparator.And, true);
        }
    }
}