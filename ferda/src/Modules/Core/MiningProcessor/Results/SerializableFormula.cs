using System;
using System.Collections.Generic;
using System.Text;
using Ferda.Guha.MiningProcessor.BitStrings;
using Ferda.Guha.MiningProcessor.Formulas;

namespace Ferda.Guha.MiningProcessor.Results
{
    /// <summary>
    /// Serializable formula.
    /// Deserealized form is some class implementing the 
    /// <see cref="T:Ferda.Guha.MiningProcessor.Formulas.Formula"/> 
    /// interface.
    /// (Design Pattern <c>Builder</c> is used).
    /// </summary>
    [Serializable()]
    public class SerializableFormula
    {
        /// <summary>
        /// The type of formula ... indicates the type of 
        /// corresponding serialized class.
        /// </summary>
        public enum FormulaType
        {
            AtomFormula,
            NegationFormula,
            ConjunctionFormula,
            DisjunctionFormula,
            CategorialFormula,
        }
        
        /// <summary>
        /// Indicates the type of serialized formula.
        /// </summary>
        public FormulaType Type;
        
        /// <summary>
        /// Serizalized operands (subformulas) (if any).
        /// </summary>
        public SerializableFormula[] Operands;
        
        /// <summary>
        /// GUID of attribute (in serialized atoms).
        /// </summary>
        public string AttributeGuid;
        
        /// <summary>
        /// Name of category (in serialized atoms).
        /// </summary>
        public string CategoryId;

        /// <summary>
        /// Builds the specified input.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static Formula Build(SerializableFormula input)
        {
            if (input == null)
                return null;
            BooleanAttributeFormula[] inner = null;
            if (input.Operands != null && input.Operands.Length > 0)
            {
                inner = new BooleanAttributeFormula[input.Operands.Length];
                for (int i = 0; i < input.Operands.Length; i++)
                {
                    inner[i] = (BooleanAttributeFormula)Build(input.Operands[i]);
                }
            }

            switch (input.Type)
            {
                case FormulaType.AtomFormula:
                    return new AtomFormula(new BitStringIdentifier(input.AttributeGuid, input.CategoryId));
                case FormulaType.NegationFormula:
                    return new NegationFormula(inner[0]);
                case FormulaType.ConjunctionFormula:
                    return new ConjunctionFormula(inner);
                case FormulaType.DisjunctionFormula:
                    return new DisjunctionFormula(inner);
                case FormulaType.CategorialFormula:
                    return new CategorialAttributeFormula(input.AttributeGuid);
                default:
                    throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Builds the specified input.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static SerializableFormula Build(Formula input)
        {
            if (input == null)
                return null;
            SerializableFormula result = new SerializableFormula();
            {
                AtomFormula af = input as AtomFormula;
                if (af != null)
                {
                    result.AttributeGuid = af.BitStringIdentifier.AttributeGuid;
                    result.CategoryId = af.BitStringIdentifier.CategoryId;
                    result.Type = FormulaType.AtomFormula;
                    return result;
                }
            }
            {
                ConjunctionFormula cf = input as ConjunctionFormula;
                if (cf != null)
                {
                    SerializableFormula[] inners = new SerializableFormula[cf.Operands.Count];
                    for (int i = 0; i < cf.Operands.Count; i++)
                    {
                        inners[i] = Build(cf.Operands[i]);
                    }
                    result.Operands = inners;
                    result.Type = FormulaType.ConjunctionFormula;
                    return result;
                }
            }
            {
                DisjunctionFormula df = input as DisjunctionFormula;
                if (df != null)
                {
                    SerializableFormula[] inners = new SerializableFormula[df.Operands.Count];
                    for (int i = 0; i < df.Operands.Count; i++)
                    {
                        inners[i] = Build(df.Operands[i]);
                    }
                    result.Operands = inners;
                    result.Type = FormulaType.DisjunctionFormula;
                    return result;
                }
            }
            {
                NegationFormula nf = input as NegationFormula;
                if (nf != null)
                {
                    result.Operands = new SerializableFormula[] { Build(nf.Operand) };
                    result.Type = FormulaType.NegationFormula;
                    return result;
                }
            }
            {
                CategorialAttributeFormula caf = input as CategorialAttributeFormula;
                if (caf != null)
                {
                    result.AttributeGuid = caf.AttributeGuid;
                    result.Type = FormulaType.CategorialFormula;
                    return result;
                }
            }
            throw new NotImplementedException();
        }
    }
}
