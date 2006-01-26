using System;
using System.Collections.Generic;
using Ferda.Modules.Boxes.LISpMinerTasks.AbstractLMTask;

namespace Ferda.Modules.Boxes.LISpMinerTasks.KLTask
{
    class KLTaskFunctionsI : KLTaskFunctionsDisp_, IFunctions, ILISpMinerAbstractTask
	{
        protected BoxModuleI boxModule;
        //protected IBoxInfo boxInfo;

        #region IFunctions Members
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
			result.succedentSetting = LISpMinerAbstractTask.GetCategorialPartialCedentSettingSeq(boxModule, "SuccedentSetting", true);
			result.antecedentSetting = LISpMinerAbstractTask.GetCategorialPartialCedentSettingSeq(boxModule, "AntecedentSetting", true);
			result.conditionSetting = LISpMinerAbstractTask.GetBooleanPartialCedentSettingSeq(boxModule, "ConditionSetting", false);
			result.quantifiers = getQuantifiers();
			return result;
		}

                private QuantifierSettingStruct[] getQuantifiers()
        {
            return LISpMinerAbstractTask.GetQuantifiersSetting(boxModule, "KLQuantifier");
        }

        public override QuantifierProvider[] getQuantifierProviders(Ice.Current __current)
        {
            return LISpMinerAbstractTask.GetQuantifierProviders(boxModule, "KLQuantifier");
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
            LISpMinerAbstractTask.RunTaskOnMetabaseLayer(boxModule, getTask(), new MetabaseLayer.KLTask());
        }
        #endregion
	}
}
