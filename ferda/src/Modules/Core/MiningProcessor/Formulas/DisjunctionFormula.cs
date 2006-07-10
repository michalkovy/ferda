using System.Collections.Generic;

namespace Ferda.Guha.MiningProcessor.Formulas
{
    public class DisjunctionFormula : BooleanAttributeFormula
    {
        private readonly BooleanAttributeFormula[] _operands;

        public BooleanAttributeFormula[] Operands
        {
            get { return _operands; }
        }

        public DisjunctionFormula(BooleanAttributeFormula[] operands)
        {
            // flattening
            List<BooleanAttributeFormula> operandsLists = new List<BooleanAttributeFormula>();
            foreach (BooleanAttributeFormula operand in operands)
            {
                DisjunctionFormula disjunction = operand as DisjunctionFormula;
                if (disjunction == null)
                    operandsLists.Add(operand);
                else
                    operandsLists.AddRange(disjunction.Operands);
            }

            _operands = operandsLists.ToArray();
        }

        public DisjunctionFormula(BooleanAttributeFormula operandA, BooleanAttributeFormula operandB)
            : this(new BooleanAttributeFormula[] {operandA, operandB})
        {
        }

        public override string ToString()
        {
            List<string> result = new List<string>();
            Dictionary<string, List<string>> atoms = new Dictionary<string, List<string>>();
            foreach (BooleanAttributeFormula formula in _operands)
            {
                AtomFormula atomFormula = formula as AtomFormula;
                if (atomFormula == null)
                    result.Add(formula.ToString());
                else
                {
                    //group atoms
                    if (!(atoms.ContainsKey(atomFormula.BitStringIdentifier.AttributeGuid)))
                        atoms[atomFormula.BitStringIdentifier.AttributeGuid] = new List<string>();
                    atoms[atomFormula.BitStringIdentifier.AttributeGuid].Add(atomFormula.BitStringIdentifier.CategoryId);
                }
            }
            // print atoms
            foreach (KeyValuePair<string, List<string>> pair in atoms)
            {
                result.Add(AtomFormula.WriteAtom(pair.Key, pair.Value));
            }
            return FormulaHelper.SequenceToString(result, FormulaSeparator.Or, true);
        }
    }
}