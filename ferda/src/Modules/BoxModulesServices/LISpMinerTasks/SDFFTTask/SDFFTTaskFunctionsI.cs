using System;
using System.Collections.Generic;
using Ferda.Modules;
using Ferda.Modules.Boxes.LISpMinerTasks.AbstractLMTask;

namespace Ferda.Modules.Boxes.LISpMinerTasks.SDFFTTask
{
    class SDFFTTaskFunctionsI : SDFFTTaskFunctionsDisp_, IFunctions, ILISpMinerAbstractTask
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

        #region Properties
        protected MissigTypeEnum MissingType
        {
            get
            {
                return MissigTypeEnum.Deleting;
                /*
                return (MissigTypeEnum)Enum.Parse(typeof(MissigTypeEnum),
                    this.boxModule.GetPropertyString("MissingsType"),
                    true);
                 */
            }
        }
        #endregion

        #region Functions
        public override TaskStruct getTask(Ice.Current __current)
        {
            TaskStruct result = new TaskStruct();
            result.succedentSetting = LISpMinerAbstractTask.GetBooleanPartialCedentSettingSeq(boxModule, "SuccedentSetting", true);
            result.antecedentSetting = LISpMinerAbstractTask.GetBooleanPartialCedentSettingSeq(boxModule, "AntecedentSetting", false);
            result.conditionSetting = LISpMinerAbstractTask.GetBooleanPartialCedentSettingSeq(boxModule, "ConditionSetting", false);
            result.firstCedentSetting = LISpMinerAbstractTask.GetBooleanPartialCedentSettingSeq(boxModule, "Cedent1", true);
            result.secondCedentSetting = LISpMinerAbstractTask.GetBooleanPartialCedentSettingSeq(boxModule, "Cedent2", false);
            result.firstSet = LISpMinerAbstractSDTask.GetFirstSet(boxModule);
            result.secondSet = LISpMinerAbstractSDTask.GetSecondSet(boxModule);
            result.missingType = MissingType;
            result.quantifiers = getQuantifiers();
            return result;
        }

        private QuantifierSettingStruct[] getQuantifiers()
        {
            return LISpMinerAbstractTask.GetQuantifiersSetting(boxModule, "SDFFTQuantifier");
        }

        public override QuantifierProvider[] getQuantifierProviders(Ice.Current __current)
        {
            return LISpMinerAbstractTask.GetQuantifierProviders(boxModule, "SDFFTQuantifier");
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
            LISpMinerAbstractTask.RunTaskOnMetabaseLayer(boxModule, getTask(), new MetabaseLayer.SDFFTTask());
        }
        #endregion
        public override void runAction(Ice.Current current__)
        {
            RunActionRun();
        }
    }
}
