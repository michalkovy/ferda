using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Ferda.Guha.MiningProcessor.Formulas;

namespace Ferda.Guha.MiningProcessor.Results
{
    /// <summary>
    /// The effective form of <see cref="T:Ferda.Guha.MiningProcessor.Results.SerializableHypothesis"/>.
    /// Supports methods to gets/sets formulas 
    /// by semantic of given boolean/categorial attribute.
    /// The list of semantics supported by the current task type is given
    /// by method <code>GetSemanticMarks()</code> in the <see cref="T:Ferda.Guha.MiningProcessor.Results.Result"/>.
    /// </summary>
    public class Hypothesis
    {
        /// <summary>
        /// The contingecy table (for first set in SD tasks).
        /// </summary>
        public double[][] ContingencyTableA;

        /// <summary>
        /// The contingency table for second set in SD tasks.
        /// </summary>
        public double[][] ContingencyTableB;

        /// <summary>
        /// Gets the numeric values attribute GUID.
        /// </summary>
        /// <value>The numeric values attribute GUID.</value>
        public string NumericValuesAttributeGuid
        {
            get
            {
                CategorialAttributeFormula f = GetFormula(MarkEnum.Attribute) as CategorialAttributeFormula;
                if (f == null)
                {
                    Debug.Assert(false);
                    return null;
                }
                return f.AttributeGuid;
            }
        }

        #region Storage of formulas
        
        internal const int CountOfFormulas = 8;

        internal Formula[] _formulas = new Formula[CountOfFormulas];

        private int getIndex(MarkEnum semantic)
        {
            switch (semantic)
            {
                case MarkEnum.Antecedent:
                    return 0;
                case MarkEnum.Succedent:
                    return 1;
                case MarkEnum.Condition:
                    return 2;
                case MarkEnum.FirstSet:
                    return 3;
                case MarkEnum.SecondSet:
                    return 4;
                case MarkEnum.Attribute:
                    return 5;
                case MarkEnum.RowAttribute:
                    return 6;
                case MarkEnum.ColumnAttribute:
                    return 7;
                default:
                    throw new NotImplementedException();
            }
        } 
        
        #endregion

        /// <summary>
        /// Gets the formula of specified semantic.
        /// </summary>
        /// <param name="semantic">The semantic.</param>
        /// <returns></returns>
        public Formula GetFormula(MarkEnum semantic)
        {
            return _formulas[getIndex(semantic)];
        }

        /// <summary>
        /// Sets the formula of specified semantic.
        /// </summary>
        /// <param name="semantic">The semantic.</param>
        /// <param name="formula">The formula.</param>
        public void SetFormula(MarkEnum semantic, Formula formula)
        {
            _formulas[getIndex(semantic)] = formula;
        }
    }
}
