using System;
using System.Collections.Generic;
using Ferda.Guha.Math.Quantifiers;
using Ferda.Guha.MiningProcessor.BitStrings;
using Ferda.Guha.MiningProcessor.QuantifierEvaluator;
using Ferda.Guha.MiningProcessor.Results;
using Ferda.Modules;
using Ferda.ModulesManager;
using Ice;
using Exception = System.Exception;

namespace Ferda.Guha.MiningProcessor.Miners
{
    public class MiningProcessorFunctionsI : MiningProcessorFunctionsDisp_
    {
        public override BitStringIceWithCategoryId GetNextBitString(Current current__)
        {
            if (_booleanTraceEnumerator.MoveNext())
            {
                BitStringIceWithCategoryId _tmpTuple = new BitStringIceWithCategoryId();
                _tmpTuple.categoryId = _booleanTraceEnumerator.Current.Key;
                _tmpTuple.bitString = _booleanTraceEnumerator.Current.Value;
                return _tmpTuple;
            }
            else
                return null;
        }

       // private static bool firstRun = true;

        private static GuidStruct _attributeId;

        private static int[] _countVector;

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

        private IEnumerator<KeyValuePair<string,BitStringIce>> _booleanTraceEnumerator;

        public override string Run(
            BoxModulePrx boxModule,
            BooleanAttribute[] booleanAttributes,
            CategorialAttribute[] categorialAttributes,
            QuantifierBaseFunctionsPrx[] quantifiers,
            TaskRunParams taskParams,
            BitStringGeneratorProviderPrx bitStringGenerator,
            OutputPrx output,
            GuidStruct attributeId,
            int[] countVector,
            out string resultInfo,
            Current current__
            )
        {
            // Ice peformance
            BitStringCache.IceCalls = 0;
            BitStringCache.IceTicks = 0;
            Quantifier.IceCalls = 0;
            Quantifier.IceTicks = 0;
            long before = DateTime.Now.Ticks;

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

                switch (taskParams.resultType)
                {
                    case ResultTypeEnum.Trace:
                        miningProcessor.Trace();
                        miningProcessor.ProgressSetValue(1, "Completing result.");
                        resultInfo = SerializableResultInfo.Serialize(miningProcessor.ResultInfo);
                        break;

                    case ResultTypeEnum.TraceBoolean:
                        resultInfo = String.Empty;
                        _attributeId = attributeId;
                        _countVector = countVector;
                        //bitStringGenerator.
                        _booleanTraceEnumerator = miningProcessor.TraceBoolean(_countVector, _attributeId).GetEnumerator();
                        break;

                    case ResultTypeEnum.TraceReal:
                        throw new NotImplementedException();

                    default:
                        throw new NotImplementedException();
                }
                // performance ICE
                miningProcessor.ResultInfo.OtherInfo = string.Format("BitStringCache.IceCalls: {0}; BitStringCache.IceTicks: {1}; Quantifier.IceCalls: {2}; Quantifier.IceTicks: {3}; All.Ticks: {4}", BitStringCache.IceCalls, BitStringCache.IceTicks, Quantifier.IceCalls, Quantifier.IceTicks, DateTime.Now.Ticks - before);
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