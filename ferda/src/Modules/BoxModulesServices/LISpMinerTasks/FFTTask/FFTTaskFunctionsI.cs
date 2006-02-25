using System;
using System.Collections.Generic;
using Ferda.Modules.Boxes.LISpMinerTasks.AbstractLMTask;

namespace Ferda.Modules.Boxes.LISpMinerTasks.FFTTask
{
    class FFTTaskFunctionsI : FFTTaskFunctionsDisp_, IFunctions, ILISpMinerAbstractTask
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
		protected bool IncludeSymetric
		{
			get
			{
				return this.boxModule.GetPropertyBool("IncludeSymetric");
			}
		}

		protected bool Prolong100A
		{
			get
			{
                return this.boxModule.GetPropertyBool("Prolong100A");
			}
		}

		protected bool Prolong100S
		{
			get
			{
                return this.boxModule.GetPropertyBool("Prolong100S");
			}
		}

		protected MissigTypeEnum MissingType
		{
			get
			{
				return (MissigTypeEnum)Enum.Parse(typeof(MissigTypeEnum),
					this.boxModule.GetPropertyString("MissingsType"),
					true);
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
			result.includeSymetric = IncludeSymetric;
			result.missingType = MissingType;
			result.prolong100A = Prolong100A;
			result.prolong100S = Prolong100S;
			result.quantifiers = getQuantifiers();
			return result;
		}

                private QuantifierSettingStruct[] getQuantifiers()
        {
            return LISpMinerAbstractTask.GetQuantifiersSetting(boxModule, "FFTQuantifier");
        }

        public override QuantifierProvider[] getQuantifierProviders(Ice.Current __current)
        {
            return LISpMinerAbstractTask.GetQuantifierProviders(boxModule, "FFTQuantifier");
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
            LISpMinerAbstractTask.RunTaskOnMetabaseLayer(boxModule, getTask(), new MetabaseLayer.FFTTask());
        }
        #endregion
	}
}