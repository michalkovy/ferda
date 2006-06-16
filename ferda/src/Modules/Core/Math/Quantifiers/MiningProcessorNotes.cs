using System;
using System.Collections;
using System.Collections.Generic;
//TODO rozmyslet taky "UF"-Filter

namespace Ferda.Guha.Math.Quantifiers
{

    #region BitStrings

    /// <summary>
    /// Interface pro bitové/fuzzy retizky
    /// </summary>
    interface IBitString
    {
    }

    class BoolString : IBitString
    {
    }

    class FuzzyString : IBitString
    {
    }

    #endregion

    /// <summary>
    /// Struktura navrzena Michalem .. obecna hypoteza
    /// </summary>
    public class Hypothesis
    {
    }

    public class QuantifierValueQueue
    {
        private QuantifierSemanticEnum _quantifierSemantic;
        double value;
        Hypothesis hypothesis;
    }

    public class Quantifier
    {
        /// <summary>
        /// Proxy func. obj. konkretniho kvantifikatoru
        /// </summary>
        public AbstractQuantifierFunctions functionsPrx;

        /// <summary>
        /// Nastaveni kvantifikatoru
        /// </summary>
        public QuantifierSetting setting;

        /// <summary>
        /// Fronta hypotéz razena dle hodnoty kvantifikatoru a jeho semantiky.
        /// </summary>
        public QuantifierValueQueue Values;
    }

    /// <summary>
    /// Fronta kvanitfikatorù - pøetižit Sort()
    /// - "neuspesnosti kvantifikatoru" 
    /// - performance narocnosti 
    /// - preferovat ty, ktere nepotrebuji numeric values
    /// </summary>
    public class QuantifiersQueue : IEnumerable<Quantifier>
    {
        public List<Quantifier> Quantifiers;

        #region IEnumerable<Quantifier> Members

        public IEnumerator<Quantifier> GetEnumerator()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion
    }

    /// <summary>
    /// Base for each miner (4ft, SD4ft, KL, SDKL, CF, SDCF)
    /// </summary>
    public abstract class MiningProcesor
    {
        private QuantifiersQueue _quantifiers;

        /// <summary>
        /// Get quantifiers and some describe info (for performance purposes)
        /// </summary>
        public MiningProcesor()
        {
            //Fill in _quantifiers
        }

        /// <summary>
        /// Generates the hypotheses.
        ///  - make new hypothesis
        ///  - make contingency table(s)
        ///  - evaluate OperationMode of quantifiers
        ///  - call ProcessHypothesis(Hypothesis*, ContingencyTable*)
        /// </summary>
        public abstract void GenerateHypotheses();

        public void ProcessQuantifiers(Hypothesis hypothesis, ContingencyTable contingecyTable)
        {
            // save TopN (mozna i FirstN) hypothesis
            foreach (Quantifier var in _quantifiers)
            {
                //TODO
            }
        }
    }

    public class XMiner : MiningProcesor
    {
        public override void GenerateHypotheses()
        {
            throw new Exception("The method or operation is not implemented.");
        }
    }

    /* TopN algoritmus
     * 
     * Task Results
     * -	The valid hypotheses are kept sorted by the natural values given by 
     *      the quantifiers. For every quantifier there is a queue of top hypotheses.
     * -	The user can specify the maximum number of hypotheses in queues. 
     *      Valid hypotheses are dropped if do not fit to any of the queues in favor 
     *      of stronger hypotheses. Note that this approach slightly diverges from the 
     *      original GUHA method, which put all prime hypotheses to output.
     * */

    /* Filtr
     * 
     * musi do nej jit zapojit "ruzne" kvantifikatory, a ty budou mit ruzne potreby 
     * (FromRow, ..., )
     * 
     * musi se tvarit jako bezny kvantifikator
     * 
     * bizardni priklad muze byt 
     * (MinValue/MaxValue - FI/(Base*AA)) > 5
     * 
     * */
}