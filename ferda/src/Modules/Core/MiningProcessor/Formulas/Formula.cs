using System.Collections.Generic;

namespace Ferda.Guha.MiningProcessor.Formulas
{
    public enum FormulaSeparator
    {
        And,
        Or,
        AtomMembers
    }
    
    public static class Formula
    {
        public static IFormula And(IFormula operandA, IFormula operandB)
        {
            return new ConjunctionFormula(operandA, operandB);
        }

        public static IFormula Or(IFormula operandA, IFormula operandB)
        {
            return new DisjunctionFormula(operandA, operandB);
        }

        public static IFormula Not(IFormula operand)
        {
            return new NegationFormula(operand);
        }

        /// <summary>
        /// Unicode Character 'LOGICAL AND' (U+2227)
        /// http://www.fileformat.info/info/unicode/char/2227/index.htm
        /// </summary>
        //public const string SeparatorAnd = "\u2227"; TODO
        public const string SeparatorAnd = "&";
        
        /// <summary>
        /// Unicode Character 'LOGICAL OR' (U+2228)
        /// http://www.fileformat.info/info/unicode/char/2228/index.htm
        /// </summary>
        //public const string SeparatorOr = "\u2228"; TODO
        public const string SeparatorOr = "|";
        
        public const string SeparatorAtomMembers = ",";
        
        /// <summary>
        /// Unicode Character 'NOT SIGN' (U+00AC)
        /// http://www.fileformat.info/info/unicode/char/00ac/index.htm
        /// </summary>
        public const string NegationSign = "\u00AC";

        public static string SequenceToString(IEnumerable<string> items, FormulaSeparator formulaSeparator, bool sorted)
        {
            string separator;
            switch (formulaSeparator)
            {
                case FormulaSeparator.And:
                    separator = SeparatorAnd;
                    break;
                case FormulaSeparator.Or:
                    separator = SeparatorOr;
                    break;
                case FormulaSeparator.AtomMembers:
                    separator = SeparatorAtomMembers;
                    break;
                default:
                    throw new System.NotImplementedException();
            }
            
            if (sorted)
            {
                List<string> sortedItems = new List<string>(items);
                items = sortedItems;
            }

            string result = null;
            foreach (string s in items)
            {
                if (result != null)
                    result += separator + s;
                else
                    result = s;
            }
            return result;
        }
    }
}