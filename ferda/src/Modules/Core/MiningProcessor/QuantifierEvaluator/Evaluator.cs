using System;
using System.Collections.Generic;
using System.Text;
using Ferda.Guha.Math.Quantifiers;

namespace Ferda.Guha.MiningProcessor.QuantifierEvaluator
{
    public class Evaluator
    {
        private long _allObjectsCount;
        public Evaluator(long allObjectsCount)
        {
            _allObjectsCount = allObjectsCount;
        }
        
        public bool Evaluate(Quantifier quantifier, out double value)
        {
            //TODO jako parametry kontingecni tabulka -> units, podtabulka, ...
            throw new NotImplementedException();
        }
    }
}
