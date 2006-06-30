namespace Ferda.Guha.MiningProcessor.QuantifierEvaluator
{
    public interface IEvaluator
    {
        bool VerifyIsComplete(ContingencyTableHelper contingencyTable, Hypothesis hypothesis);
        void Flush();
    }
}