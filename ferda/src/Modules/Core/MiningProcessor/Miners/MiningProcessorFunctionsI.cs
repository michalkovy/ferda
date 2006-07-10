using Ferda.Guha.Math.Quantifiers;
using Ferda.Modules;
using Ferda.ModulesManager;
using Ice;
using Exception=System.Exception;

namespace Ferda.Guha.MiningProcessor.Miners
{
    internal class MiningProcessorFunctionsI : MiningProcessorFunctionsDisp_
    {
        public override string Run(
            BooleanAttribute[] booleanAttributes,
            CategorialAttribute[] categorialAttributes,
            QuantifierBaseFunctionsPrx[] quantifiers,
            TaskRunParams taskParams,
            BitStringGeneratorProviderPrx bitStringGenerator,
            OutputPrx output,
            out string resultInfo,
            Current current__
            )
        {
            try
            {
                MiningProcessorBase miningProcessor;
                Result result = null;
                SerializableResultInfo rInfo = null;
                switch (taskParams.taskType)
                {
                    case TaskTypeEnum.FourFold:
                        miningProcessor = new FourFoldMiningProcessor(
                            booleanAttributes,
                            categorialAttributes,
                            quantifiers,
                            taskParams,
                            bitStringGenerator);
                        result = miningProcessor.Trace(out rInfo);
                        break;
                    case TaskTypeEnum.CF:
                        break;
                    case TaskTypeEnum.KL:
                        break;
                    case TaskTypeEnum.SDCF:
                        break;
                    case TaskTypeEnum.SDFourFold:
                        break;
                    case TaskTypeEnum.SDKL:
                        break;
                    default:
                        break;
                }
                resultInfo = SerializableResultInfo.Serialize(rInfo);
                return SerializableResult.Serialize(result);
            }
            catch (BoxRuntimeError)
            {
                throw;
            }
            catch (Exception e)
            {
                throw Modules.Exceptions.BoxRuntimeError(e, null, "An error ocured in mining processor: " + e.Message);
            }
        }
    }
}