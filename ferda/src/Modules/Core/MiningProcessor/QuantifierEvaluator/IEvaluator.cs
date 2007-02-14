using Ferda.Guha.MiningProcessor.Results;

namespace Ferda.Guha.MiningProcessor.QuantifierEvaluator
{
    public interface IEvaluator
    {
        //tady pujde bufferovat zadani a volat Valid na pole CT
        bool VerifyIsComplete(ContingencyTableHelper contingencyTable, Hypothesis hypothesis);

        /// <summary>
        /// Returns boolean vector of verification results on contingency tables
        /// </summary>
        /// <returns>Boolean vector</returns>
        bool[] GetEvaluationVector();

        /// <summary>
        /// Returns boolean vector of verification results on contingency tables for SD miners
        /// </summary>
        /// <returns>Boolean vector</returns>
        bool[] GetEvaluationVectorSD();
        
        //tohle bude validovat zbytek bufferu
        void Flush();

        double[] SDFirstSetValues(ContingencyTableHelper contingencyTable);
        bool VerifyIsCompleteSDSecondSet(ContingencyTableHelper contingencyTable, double[] sDFirstSetValues, Hypothesis hypothesis);
    }
}