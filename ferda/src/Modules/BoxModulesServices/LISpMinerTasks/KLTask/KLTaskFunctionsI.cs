using System;
using System.Collections.Generic;
using Ferda.Modules.Boxes.AbstractLMTask;
using Ferda.Modules.Boxes.KLTask.Quantifiers.AbstractKLQuantifier;
using Ferda.Modules.Boxes.AbstractQuantifier;
using Ferda.Modules.Boxes.DataMiningCommon.CategorialPartialCedentSetting;
using Ferda.Modules.Boxes.DataMiningCommon.BooleanPartialCedentSetting;

namespace Ferda.Modules.Boxes.KLTask
{
	class KLTaskFunctionsI : KLTaskFunctionsDisp_, IFunctions
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

		#region Properties
		protected GeneratingStruct GenerationInfo
		{
			get
			{
                GeneratingStruct result = ((GenerationInfoTI)this.boxModule.GetPropertyOther("GenerationInfo")).getGenerationInfo();
				if (result.generationStartTime == null)
					result.generationStartTime = new DateTimeTI(0, 0, 0, 0, 0, 0);
				if (result.generationTotalTime == null)
					result.generationTotalTime = new TimeTI(0, 0, 0);
				return result;
			}
			set
			{
				GenerationInfoTI newValue = new GenerationInfoTI(value);
				this.boxModule.setProperty("GenerationInfo", newValue);
			}
		}

		protected HypothesisStruct[] Hypotheses
		{
			get
			{
				HypothesisStruct[] result = ((HypothesesTI)this.boxModule.GetPropertyOther("Hypotheses")).getHypothesesValue();
				if (result == null)
					return new HypothesisStruct[0];
				else
					return result;
			}
			set
			{
				HypothesesTI newValue = new HypothesesTI(value);
				this.boxModule.setProperty("Hypotheses", newValue);
			}
		}
		#endregion

		#region Functions
		public override TaskStruct getTask(Ice.Current __current)
		{
			TaskStruct result = new TaskStruct();
			result.succedentSetting = this.getCategorialPartialCedentSettingSeq("SuccedentSetting");
			result.antecedentSetting = this.getCategorialPartialCedentSettingSeq("AntecedentSetting");
			result.conditionSetting = this.getBooleanPartialCedentSettingSeq("ConditionSetting");
			result.quantifiers = getQuantifiers();
			return result;
		}

		private QuantifierSettingStruct[] getQuantifiers()
		{
			List<QuantifierSettingStruct> result = new List<QuantifierSettingStruct>();
			List<PropertySetting> propertySetting = new List<PropertySetting>();
			PropertySetting property;
			QuantifierSettingStruct item;
			foreach (BoxModulePrx prx in getAbstractKLQuantifierBoxModulePrx())
			{
				propertySetting.Clear();
				foreach (PropertyInfo propertyInfo in prx.getMyFactory().getProperties())
				{
					property = new PropertySetting();
					property.propertyName = propertyInfo.name;
					property.value = prx.getProperty(property.propertyName);
					propertySetting.Add(property);
				}
				item = new QuantifierSettingStruct();
				item.setting = propertySetting.ToArray();
				item.typeIdentifier = prx.getMyFactory().getMyFactoryCreator().getIdentifier();
				result.Add(item);
			}
			return result.ToArray();
		}

		public override QuantifierProvider[] getQuantifierProviders(Ice.Current __current)
		{
			List<QuantifierProvider> result = new List<QuantifierProvider>();
			QuantifierProvider item;
			foreach (BoxModulePrx prx in getAbstractKLQuantifierBoxModulePrx())
			{
				item = new QuantifierProvider();
				item.localizedBoxLabel = prx.getMyFactory().getMyFactoryCreator().getLabel(boxModule.LocalePrefs);
				item.userBoxLabel = boxModule.Manager.getProjectInformation().getUserLabel(Ice.Util.identityToString(prx.ice_getIdentity()));
				item.functions = AbstractKLQuantifierFunctionsPrxHelper.checkedCast(prx.getFunctions());
				result.Add(item);
			}
			return result.ToArray();
		}

		public override GeneratingStruct getGeneratingInfo(Ice.Current __current)
		{
			return GenerationInfo;
		}

		public override HypothesisStruct[] getResult(Ice.Current __current)
		{
			return Hypotheses;
		}
		#endregion

		#region Sockets
		private CategorialPartialCedentSettingStruct[] getCategorialPartialCedentSettingSeq(string socketName)
		{
			List<CategorialPartialCedentSettingStruct> result = new List<CategorialPartialCedentSettingStruct>();
			bool oneAtMinimum = (socketName == "SuccedentSetting" || socketName == "AntecedentSetting");
			Ice.ObjectPrx[] prxs = SocketConnections.GetObjectPrxs(boxModule, socketName, oneAtMinimum);
			foreach (Ice.ObjectPrx prx in prxs)
			{
				result.Add((CategorialPartialCedentSettingFunctionsPrxHelper.checkedCast(prx)).getCategorialPartialCedentSetting());
			}
			return result.ToArray();
		}
		private BooleanPartialCedentSettingStruct[] getBooleanPartialCedentSettingSeq(string socketName)
		{
			List<BooleanPartialCedentSettingStruct> result = new List<BooleanPartialCedentSettingStruct>();
			bool oneAtMinimum = false; //(socketName == "SuccedentSetting");
			Ice.ObjectPrx[] prxs = SocketConnections.GetObjectPrxs(boxModule, socketName, oneAtMinimum);
			foreach (Ice.ObjectPrx prx in prxs)
			{
				result.Add((BooleanPartialCedentSettingFunctionsPrxHelper.checkedCast(prx)).getBooleanPartialCedentSetting());
			}
			return result.ToArray();
		}
		private BoxModulePrx[] getAbstractKLQuantifierBoxModulePrx()
		{
			return SocketConnections.GetBoxModulePrxs(boxModule, "KLQuantifier", false);
		}
		#endregion

		#region Actions
		public void RunActionRun()
		{
			//switch generation state to running
            GeneratingStruct generationInfo = GenerationInfo;
			generationInfo.generationState = GenerationStateEnum.Running;
			GenerationInfo = generationInfo;

			//get task description (all task parameters)
			TaskStruct input = getTask();
#if !DEBUG
			try
			{
#endif
			//Better if value of the input TaskStruct is compared with last (cached) value of TaskStruct. Iff it is the same finish.

			//run task
			MetabaseLayer.KLTask metabaseLayer = new MetabaseLayer.KLTask();
			HypothesisStruct[] hypotheses;
            metabaseLayer.RunTask(input, boxModule.StringIceIdentity, out generationInfo, out hypotheses);

			//save generation info and result (hypotheses)
			GenerationInfo = generationInfo;
			Hypotheses = hypotheses;
#if !DEBUG
			}
			catch (Exception e)
			{
				string exText = e.Message;
				exText += (e.InnerException == null) ? "" : e.InnerException.Message;
				throw Ferda.Modules.Exceptions.BoxRuntimeError(e, myIdentity, "Error occured! " + e.Message);
			}
#endif
			if (generationInfo.generationState == GenerationStateEnum.Running)
			{
				generationInfo.generationState = GenerationStateEnum.Interrupted;
				GenerationInfo = generationInfo;
			}
		}
		#endregion

		#region BoxInfo
		public GeneratingStruct GetGeneratingInfo()
		{
			return GenerationInfo;
		}
		public HypothesisStruct[] GetResult()
		{
			return Hypotheses;
		}
		#endregion
	}
}
