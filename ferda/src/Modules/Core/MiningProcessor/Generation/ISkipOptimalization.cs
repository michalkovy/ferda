using System;
using System.Collections.Generic;
using System.Text;
using Ferda.Guha.Math.Quantifiers;

namespace Ferda.Guha.MiningProcessor.Generation
{
    public class SkipSetting
    {
        public SkipSetting(Math.RelationEnum relation, double treshold, double notTreshold)
        {
            _relation = relation;
            _treshold = treshold;
            _notTreshold = notTreshold;
        }
        
        private Math.RelationEnum _relation;
        public Math.RelationEnum Relation
        {
            get { return _relation; }
        }
        
        private double _treshold;
        public double Treshold
        {
            get { return _treshold; }
        }

        private double _notTreshold;
        /// <summary>
        /// Gets the not treshold. Equals to <c>all objects count - Treshold</c>
        /// </summary>
        /// <value>The not treshold.</value>
        public double NotTreshold
        {
            get { return _notTreshold; }
        }
    }
    
    public interface ISkipOptimalization
    {
        SkipSetting BaseSkipSetting(MarkEnum cedentType);
    }
}
