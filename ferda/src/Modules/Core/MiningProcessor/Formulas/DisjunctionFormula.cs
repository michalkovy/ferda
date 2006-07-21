using System.Collections.Generic;

namespace Ferda.Guha.MiningProcessor.Formulas
{
    public class DisjunctionFormula : BooleanAttributeFormula
    {
        private readonly List<BooleanAttributeFormula> _operands;

        public List<BooleanAttributeFormula> Operands
        {
            get { return _operands; }
        }

        private static void addOperand(List<BooleanAttributeFormula> list, BooleanAttributeFormula operand)
        {
            // flattening
            DisjunctionFormula disjunction = operand as DisjunctionFormula;
            if (disjunction == null)
                list.Add(operand);
            else
                list.AddRange(disjunction.Operands);
        }
        
        public DisjunctionFormula(BooleanAttributeFormula[] operands)
        {
            _operands = new List<BooleanAttributeFormula>(2);
            foreach (BooleanAttributeFormula operand in operands)
            {
                addOperand(_operands, operand);
            }
        }

        public DisjunctionFormula(BooleanAttributeFormula operandA, BooleanAttributeFormula operandB)
        {
            _operands = new List<BooleanAttributeFormula>(2);
            addOperand(_operands, operandA);
            addOperand(_operands, operandB);
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