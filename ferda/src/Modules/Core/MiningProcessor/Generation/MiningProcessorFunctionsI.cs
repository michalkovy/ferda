using System;
using System.Collections.Generic;
using System.Text;

namespace Ferda.Guha.MiningProcessor.Generation
{
    class MiningProcessorFunctionsI : MiningProcessorFunctionsDisp_
    {
        public override string Run(BooleanAttribute[] booleanAttributes, CategorialAttribute[] categorialAttributes, Ferda.Guha.Math.Quantifiers.QuantifierBaseFunctionsPrx[] quantifiers, TaskTypeEnum taskType, BitStringGeneratorProviderPrx bitStringGenerator, Ferda.ModulesManager.OutputPrx output, out string statistics, Ice.Current current__)
        {
            MiningProcessorBase miningProcessor;
            switch (taskType)
            {
                case TaskTypeEnum.FourFold:
                    miningProcessor = new FourFoldMiningProcessor(
                        booleanAttributes,
                        categorialAttributes,
                        quantifiers,
                        bitStringGenerator);
                    miningProcessor.Trace();
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
            throw new Exception("The method or operation is not implemented.");
        }
    }
}
