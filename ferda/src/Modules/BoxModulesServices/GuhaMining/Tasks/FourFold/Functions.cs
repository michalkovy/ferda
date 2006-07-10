#define ProgressBarDebug

using System;
using System.Collections.Generic;
using System.Text;
using Ferda.Guha.MiningProcessor;
using Ferda.Guha.Math.Quantifiers;
using Ferda.ModulesManager;

namespace Ferda.Modules.Boxes.GuhaMining.Tasks.FourFold
{
    public class Functions : MiningTaskFunctionsDisp_, IFunctions, ITask
    {
        /// <summary>
        /// The box module.
        /// </summary>
        protected BoxModuleI _boxModule;

        //protected IBoxInfo _boxInfo;

        #region IFunctions Members

        /// <summary>
        /// Sets the <see cref="T:Ferda.Modules.BoxModuleI">box module</see>
        /// and the <see cref="T:Ferda.Modules.Boxes.IBoxInfo">box info</see>.
        /// </summary>
        /// <param name="boxModule">The box module.</param>
        /// <param name="boxInfo">The box info.</param>
        public void setBoxModuleInfo(BoxModuleI boxModule, IBoxInfo boxInfo)
        {
            _boxModule = boxModule;
            //_boxInfo = boxInfo;
        }

        #endregion

        #region Properties

        #endregion
        
        private static string[] getSocketNames()
        {
            return new string[]
                {
                    Common.SockSuccedent,
                    Common.SockAntecedent,
                    Common.SockCondition
                };
        }

        public override BitStringGeneratorPrx GetBitStringGenerator(GuidStruct attributeId, Ice.Current current__)
        {
            return Common.GetBitStringGenerator(
                _boxModule,
                attributeId,
                getSocketNames()
                );
        }


        public void Run()
        {
#if ProgressBarDebug
            // Progress Bar
            ProgressBarPrx pB = _boxModule.Output.startProgress(null, "Testovací PB", "Hintik pri startu");
            for (int i = 0; i < 10; i++)
            {
                pB.setValue(10 / (i + 1), "Zvysuji na " + ((i + 1) * 10) + "%");
            }
            pB.done();
            return;
#else
            #region Cedents
            List<BooleanAttribute> cedents = new List<BooleanAttribute>();

            BooleanAttribute cedent;

            cedent = Common.GetBooleanAttribute(_boxModule, MarkEnum.Succedent, Common.SockSuccedent, true);
            if (cedent != null)
                cedents.Add(cedent);

            cedent = Common.GetBooleanAttribute(_boxModule, MarkEnum.Antecedent, Common.SockAntecedent, false);
            if (cedent != null)
                cedents.Add(cedent);

            cedent = Common.GetBooleanAttribute(_boxModule, MarkEnum.Condition, Common.SockCondition, false);
            if (cedent != null)
                cedents.Add(cedent); 
            #endregion

            MiningProcessorFunctionsPrx miningProcessor = Common.GetMiningProcessorFunctionsPrx(_boxModule);

            BitStringGeneratorProviderPrx bsProvider = Common.GetBitStringGeneratorProviderPrx(_boxModule);

            //TODO PropMaxNumberOfHypotheses

            TaskRunParams taskRunParams = new TaskRunParams(
                    TaskTypeEnum.FourFold,
                    Common.ExecutionType(_boxModule),
                    Common.MaxNumberOfHypotheses(_boxModule),
                    WorkingWithSecondSetModeEnum.None
                );
            
            string statistics;
            string result =
                miningProcessor.Run(
                        cedents.ToArray(),
                        new CategorialAttribute[0],
                        GetQuantifiers(),
                        taskRunParams,
                        bsProvider,
                        _boxModule.Output,
                        out statistics
                    );
            
            Common.SetResult(_boxModule, result);
            Common.SetResultInfo(_boxModule, statistics);
            
            // reset cache
            _cachedSerializableResultInfo = null;
#endif
        }

        public override QuantifierBaseFunctionsPrx[] GetQuantifiers(Ice.Current current__)
        {
            return Common.GetQuantifierBaseFunctions(_boxModule, true).ToArray();
        }

        public override string GetResult(out string statistics, Ice.Current current__)
        {
            statistics = Common.GetResultInfo(_boxModule);
            return Common.GetResult(_boxModule);
        }

        #region ITask Members

        SerializableResultInfo _cachedSerializableResultInfo = null;
        public SerializableResultInfo GetResultInfo()
        {
            if (_cachedSerializableResultInfo == null)
                _cachedSerializableResultInfo = Common.GetResultInfoDeserealized(_boxModule);
            return _cachedSerializableResultInfo;
        }

        #endregion

        public override GuidAttributeNamePair[] GetAttributeNames(Ice.Current current__)
        {
            return Common.GetAttributeNames(_boxModule, getSocketNames());
        }
    }
}
