using System;
using System.Collections.Generic;

namespace Ferda.Guha.MiningProcessor.Formulas
{
    public class DisjunctionFormula : IFormula
    {
        private readonly IFormula[] _operands;

        public IFormula[] Operands
        {
            get { return _operands; }
        }

        public DisjunctionFormula(IFormula[] operands)
        {
            // flattening
            List<IFormula> operandsLists = new List<IFormula>();
            foreach (IFormula operand in operands)
            {
                DisjunctionFormula disjunction = operand as DisjunctionFormula;
                if (disjunction == null)
                    operandsLists.Add(operand);
                else
                    operandsLists.AddRange(disjunction.Operands);
            }

            _operands = operandsLists.ToArray();
        }

        public DisjunctionFormula(IFormula operandA, IFormula operandB)
            : this(new IFormula[] { operandA, operandB })
        {
        }

        public override string ToString()
        {
            List<string> result = new List<string>();
            Dictionary<Guid, List<string>> atoms = new Dictionary<Guid, List<string>>();
            foreach (IFormula formula in _operands)
            {
                Atom atom = formula as Atom;
                if (atom == null)
                    result.Add(formula.ToString());
                else
                {
                    //group atoms
                    if (!(atoms.ContainsKey(atom.BitStringIdentifier.AttributeId)))
                        atoms[atom.BitStringIdentifier.AttributeId] = new List<string>();
                    atoms[atom.BitStringIdentifier.AttributeId].Add(atom.BitStringIdentifier.CategoryId);
                }
            }
            // print atoms
            foreach (KeyValuePair<Guid, List<string>> pair in atoms)
            {
                result.Add(Atom.WriteAtom(pair.Key, pair.Value));
            }
            return Formula.SequenceToString(result, FormulaSeparator.Or, true);
        }
    }
}