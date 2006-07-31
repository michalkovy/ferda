using Ferda.Guha.MiningProcessor.Results;

namespace Ferda.Guha.MiningProcessor.QuantifierEvaluator
{
    public interface IEvaluator
    {
        bool VerifyIsComplete(ContingencyTableHelper contingencyTable, Hypothesis hypothesis);
        void Flush();

        double[] SDFirstSetValues(ContingencyTableHelper contingencyTable);
        bool VerifyIsCompleteSDSecondSet(ContingencyTableHelper contingencyTable, double[] sDFirstSetValues, Hypothesis hypothesis);
    }
}