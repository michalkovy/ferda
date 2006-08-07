using System;
using Ferda.Guha.Math.Quantifiers;
using Ferda.Guha.MiningProcessor.Results;
using Ferda.Modules;
using Ferda.ModulesManager;
using Ice;
using Exception = System.Exception;

namespace Ferda.Guha.MiningProcessor.Miners
{
    internal class MiningProcessorFunctionsI : MiningProcessorFunctionsDisp_
    {
        private static string taskTypeToString(TaskTypeEnum taskType)
        {
            switch (taskType)
            {
                case TaskTypeEnum.CF:
                    return "CF";
                case TaskTypeEnum.FourFold:
                    return "4ft";
                case TaskTypeEnum.KL:
                    return "KL";
                case TaskTypeEnum.SDCF:
                    return "SDCF";
                case TaskTypeEnum.SDFourFold:
                    return "SD4ft";
                case TaskTypeEnum.SDKL:
                    return "SDKL";
                default:
                    throw new NotImplementedException();
            }
        }
        
        
        public override string Run(
            BoxModulePrx boxModule,
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
            ProgressTaskListener progressListener = new ProgressTaskListener();
            ProgressTaskPrx progressPrx =
                ProgressTaskI.Create(current__.adapter, progressListener);
            string label = taskTypeToString(taskParams.taskType) + "-Task";
            ProgressBarPrx progressBarPrx = 
                output.startProgress(progressPrx, label, label + " running...");
            progressBarPrx.setValue(-1, "Loading ...");

            MiningProcessorBase miningProcessor = null;
            try
            {
                switch (taskParams.taskType)
                {
                    case TaskTypeEnum.FourFold:
                        miningProcessor = new FourFoldMiningProcessor(
                            booleanAttributes,
                            categorialAttributes,
                            quantifiers,
                            taskParams,
                            bitStringGenerator,
                            progressListener,
                            progressBarPrx);
                        break;
                    case TaskTypeEnum.CF:
                        miningProcessor = new CFMiningProcessor(
                            booleanAttributes,
                            categorialAttributes,
                            quantifiers,
                            taskParams,
                            bitStringGenerator,
                            progressListener,
                            progressBarPrx);
                        break;
                    case TaskTypeEnum.KL:
                        miningProcessor = new KLMiningProcessor(
                            booleanAttributes,
                            categorialAttributes,
                            quantifiers,
                            taskParams,
                            bitStringGenerator,
                            progressListener,
                            progressBarPrx);
                        break;
                    case TaskTypeEnum.SDCF:
                        miningProcessor = new SDCFMiningProcessor(
                            booleanAttributes,
                            categorialAttributes,
                            quantifiers,
                            taskParams,
                            bitStringGenerator,
                            progressListener,
                            progressBarPrx);
                        break;
                    case TaskTypeEnum.SDFourFold:
                        miningProcessor = new SDFourFoldMiningProcessor(
                            booleanAttributes,
                            categorialAttributes,
                            quantifiers,
                            taskParams,
                            bitStringGenerator,
                            progressListener,
                            progressBarPrx);
                        break;
                    case TaskTypeEnum.SDKL:
                        miningProcessor = new SDKLMiningProcessor(
                            booleanAttributes,
                            categorialAttributes,
                            quantifiers,
                            taskParams,
                            bitStringGenerator,
                            progressListener,
                            progressBarPrx);
                        break;
                    default:
                        throw new NotImplementedException();
                }
                miningProcessor.Trace();
                miningProcessor.ProgressSetValue(1, "Completing result.");
                resultInfo = SerializableResultInfo.Serialize(miningProcessor.ResultInfo);
                return SerializableResult.Serialize(miningProcessor.Result);
            }
            catch (BoxRuntimeError)
            {
                if (miningProcessor != null)
                    miningProcessor.ProgressSetValue(-1, "Generation failed");
                else 
                    progressBarPrx.setValue(-1, "Generation failed");
                System.Threading.Thread.Sleep(1500);
                throw;
            }
            catch (Exception e)
            {
                if (miningProcessor != null)
                    miningProcessor.ProgressSetValue(-1, "Generation failed");
                else
                    progressBarPrx.setValue(-1, "Generation failed");
                System.Threading.Thread.Sleep(1500);
                throw Modules.Exceptions.BoxRuntimeError(e, null, "An error ocured in mining processor: " + e.Message + "\n" + e.ToString());
            }
            finally
            {
                progressBarPrx.done();
                System.Threading.Thread.Sleep(100);
                ProgressTaskI.Destroy(current__.adapter, progressPrx);
            }
        }
    }
}