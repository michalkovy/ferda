using System;
using System.Collections.Generic;
using Ferda.Modules.Helpers.Common;

namespace Ferda.Guha.MiningProcessor.Formulas
{
    public enum FormulaSeparator
    {
        And,
        Or,
        AtomMembers
    }

    public static class FormulaHelper
    {
        public static BooleanAttributeFormula And(BooleanAttributeFormula operandA, BooleanAttributeFormula operandB)
        {
            return new ConjunctionFormula(operandA, operandB);
        }

        public static BooleanAttributeFormula Or(BooleanAttributeFormula operandA, BooleanAttributeFormula operandB)
        {
            return new DisjunctionFormula(operandA, operandB);
        }

        public static BooleanAttributeFormula Not(BooleanAttributeFormula operand)
        {
            return new NegationFormula(operand);
        }

        /// <summary>
        /// Should be the unicode Character 'LOGICAL AND' (U+2227)
        /// http://www.fileformat.info/info/unicode/char/2227/index.htm
        /// but it is the normal & instead. The reason is, that the fonts
        /// that support unicode are in normal windows installation
        /// </summary>
        public const string SeparatorAnd = " & ";

        //public const string SeparatorAnd = "&";

        /// <summary>
        /// Should be the unicode Character 'LOGICAL OR' (U+2228)
        /// http://www.fileformat.info/info/unicode/char/2228/index.htm
        /// but it is the normal | instead. The reason is, that the fonts
        /// that support unicode are in normal windows installation
        /// </summary>
        public const string SeparatorOr = " | ";

        //public const string SeparatorOr = "|";

        public const string SeparatorAtomMembers = ", ";

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
                    throw new NotImplementedException();
            }

            if (sorted)
            {
                List<string> sortedItems = new List<string>(items);
                items = sortedItems;
            }

            return Print.SequenceToString(items, separator);
        }
    }
}