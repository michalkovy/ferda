using System;
using System.Collections.Generic;
using System.Text;
using Ferda.Modules.Boxes.LISpMinerTasks.AbstractLMTask;

namespace Ferda.Modules.Boxes.LISpMinerTasks.CFTask
{
    class CFTaskFunctionsI : CFTaskFunctionsDisp_, IFunctions, ILISpMinerAbstractTask
    {
        /// <summary>
        /// The box module.
        /// </summary>
        protected BoxModuleI boxModule;
        //protected IBoxInfo boxInfo;

        #region IFunctions Members
        /// <summary>
        /// Sets the <see cref="T:Ferda.Modules.BoxModuleI">box module</see>
        /// and the <see cref="T:Ferda.Modules.Boxes.IBoxInfo">box info</see>.
        /// </summary>
        /// <param name="boxModule">The box module.</param>
        /// <param name="boxInfo">The box info.</param>
        public void setBoxModuleInfo(BoxModuleI boxModule, IBoxInfo boxInfo)
        {
            this.boxModule = boxModule;
            //this.boxInfo = boxInfo;
        }
        #endregion

        #region Functions
        public override TaskStruct getTask(Ice.Current __current)
        {
            TaskStruct result = new TaskStruct();
            result.antecedentSetting = LISpMinerAbstractTask.GetCategorialPartialCedentSettingSeq(boxModule, "AntecedentSetting", true);
            result.conditionSetting = LISpMinerAbstractTask.GetBooleanPartialCedentSettingSeq(boxModule, "ConditionSetting", false);
            result.quantifiers = getQuantifiers();
            return result;
        }

        private QuantifierSettingStruct[] getQuantifiers()
        {
            return LISpMinerAbstractTask.GetQuantifiersSetting(boxModule, "CFQuantifier");
        }

        public override QuantifierProvider[] getQuantifierProviders(Ice.Current __current)
        {
            return LISpMinerAbstractTask.GetQuantifierProviders(boxModule, "CFQuantifier");
        }

        public override GeneratingStruct getGeneratingInfo(Ice.Current __current)
        {
            return LISpMinerAbstractTask.GetGenerationInfo(boxModule);
        }

        public override HypothesisStruct[] getResult(Ice.Current __current)
        {
            return LISpMinerAbstractTask.GetHypotheses(boxModule);
        }
        #endregion

        #region Actions
        public void RunActionRun()
        {
            LISpMinerAbstractTask.RunTaskOnMetabaseLayer(boxModule, getTask(), new MetabaseLayer.CFTask());
        }
        #endregion
        public override void runAction(Ice.Current current__)
        {
            RunActionRun();
        }
    }
}
