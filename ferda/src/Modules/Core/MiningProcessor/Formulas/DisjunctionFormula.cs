using System;
using System.Collections.Generic;
using System.Xml.Serialization;

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
            : this(new BooleanAttributeFormula[] { operandA, operandB })
        {
        }

        public override string ToString()
        {
            List<string> result = new List<string>();
            Dictionary<Guid, List<string>> atoms = new Dictionary<Guid, List<string>>();
            foreach (BooleanAttributeFormula formula in _operands)
            {
                AtomFormula atomFormula = formula as AtomFormula;
                if (atomFormula == null)
                    result.Add(formula.ToString());
                else
                {
                    //group atoms
                    if (!(atoms.ContainsKey(atomFormula.BitStringIdentifier.AttributeId)))
                        atoms[atomFormula.BitStringIdentifier.AttributeId] = new List<string>();
                    atoms[atomFormula.BitStringIdentifier.AttributeId].Add(atomFormula.BitStringIdentifier.CategoryId);
                }
            }
            // print atoms
            foreach (KeyValuePair<Guid, List<string>> pair in atoms)
            {
                result.Add(AtomFormula.WriteAtom(pair.Key, pair.Value));
            }
            return FormulaHelper.SequenceToString(result, FormulaSeparator.Or, true);
        }
    }
}