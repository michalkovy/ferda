using System;
using System.Collections.Generic;
using System.Text;
using Ferda.Modules.Boxes.DataMiningCommon.BooleanPartialCedentSetting;
using Ferda.Modules.Boxes.DataMiningCommon.CategorialPartialCedentSetting;
using Ferda.Modules.Boxes.LISpMinerTasks.AbstractLMTask;

namespace Ferda.Modules.Boxes.LISpMinerTasks
{
    public static class LISpMinerAbstractTask
    {
        public static QuantifierSettingStruct[] GetQuantifiersSetting(BoxModuleI boxModule, string socketName)
        {
            List<QuantifierSettingStruct> result = new List<QuantifierSettingStruct>();
            List<PropertySetting> propertySetting = new List<PropertySetting>();
            PropertySetting property;
            QuantifierSettingStruct item;
            foreach (BoxModulePrx prx in SocketConnections.GetBoxModulePrxs(boxModule, socketName, false))
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

        public static QuantifierProvider[] GetQuantifierProviders(BoxModuleI boxModule, string socketName)
        {
            List<QuantifierProvider> result = new List<QuantifierProvider>();
            QuantifierProvider item;
            foreach (BoxModulePrx prx in SocketConnections.GetBoxModulePrxs(boxModule, socketName, false))
            {
                item = new QuantifierProvider();
                item.localizedBoxLabel = prx.getMyFactory().getMyFactoryCreator().getLabel(boxModule.LocalePrefs);
                item.userBoxLabel = boxModule.Manager.getProjectInformation().getUserLabel(Ice.Util.identityToString(prx.ice_getIdentity()));
                item.functions = AbstractQuantifier.AbstractQuantifierFunctionsPrxHelper.checkedCast(prx.getFunctions());
                result.Add(item);
            }
            return result.ToArray();
        }

        public static GeneratingStruct GetGenerationInfo(BoxModuleI boxModule)
        {
            GeneratingStruct result = ((GenerationInfoTI)boxModule.GetPropertyOther("GenerationInfo")).getGenerationInfo();
            if (result.generationStartTime == null)
                result.generationStartTime = new DateTimeTI(0, 0, 0, 0, 0, 0);
            if (result.generationTotalTime == null)
                result.generationTotalTime = new TimeTI(0, 0, 0);
            return result;
        }

        public static void SetGenerationInfo(BoxModuleI boxModule, GeneratingStruct value)
        {
            GenerationInfoTI newValue = new GenerationInfoTI(value);
            boxModule.setProperty("GenerationInfo", newValue);
        }

        public static HypothesisStruct[] GetHypotheses(BoxModuleI boxModule)
        {
            HypothesisStruct[] result = ((HypothesesTI)boxModule.GetPropertyOther("Hypotheses")).getHypothesesValue();
            if (result == null)
                return new HypothesisStruct[0];
            else
                return result;
        }

        public static void SetHypotheses(BoxModuleI boxModule, HypothesisStruct[] value)
        {
            HypothesesTI newValue = new HypothesesTI(value);
            boxModule.setProperty("Hypotheses", newValue);
        }

        public static BooleanPartialCedentSettingStruct[] GetBooleanPartialCedentSettingSeq(BoxModuleI boxModule, string socketName, bool oneAtMinimum)
        {
            List<BooleanPartialCedentSettingStruct> result = new List<BooleanPartialCedentSettingStruct>();
            Ice.ObjectPrx[] prxs = SocketConnections.GetObjectPrxs(boxModule, socketName, oneAtMinimum);
            foreach (Ice.ObjectPrx prx in prxs)
            {
                result.Add(BooleanPartialCedentSettingFunctionsPrxHelper.checkedCast(prx).getBooleanPartialCedentSetting());
            }
            return result.ToArray();
        }

        public static CategorialPartialCedentSettingStruct[] GetCategorialPartialCedentSettingSeq(BoxModuleI boxModule, string socketName, bool oneAtMinimum)
        {
            List<CategorialPartialCedentSettingStruct> result = new List<CategorialPartialCedentSettingStruct>();
            Ice.ObjectPrx[] prxs = SocketConnections.GetObjectPrxs(boxModule, socketName, oneAtMinimum);
            foreach (Ice.ObjectPrx prx in prxs)
            {
                result.Add((CategorialPartialCedentSettingFunctionsPrxHelper.checkedCast(prx)).getCategorialPartialCedentSetting());
            }
            return result.ToArray();
        }

        public static void SetGenerationInfoStateToRunning(BoxModuleI boxModule)
        {
            GeneratingStruct generationInfo = GetGenerationInfo(boxModule);
            generationInfo.generationState = GenerationStateEnum.Running;
            SetGenerationInfo(boxModule, generationInfo);
        }

        public static void SetGenerationInfoStateToFailed(BoxModuleI boxModule)
        {
            GeneratingStruct generationInfo = GetGenerationInfo(boxModule);
            generationInfo.generationState = GenerationStateEnum.Failed;
            SetGenerationInfo(boxModule, generationInfo);
        }

        public static void RunTaskOnMetabaseLayer(BoxModuleI boxModule, object taskStruct, Ferda.Modules.MetabaseLayer.Task metabaseLayer)
        {
            //switch generation state to running
            SetGenerationInfoStateToRunning(boxModule);

            try
            {
                //Better if value of the input TaskStruct is compared with last (cached) value of TaskStruct. Iff it is the same finish.

                //run task
                HypothesisStruct[] hypotheses;
                GeneratingStruct generationInfo;
                metabaseLayer.RunTask(taskStruct, boxModule.StringIceIdentity, out generationInfo, out hypotheses);

                //save generation info and result (hypotheses)
                SetGenerationInfo(boxModule, generationInfo);
                SetHypotheses(boxModule, hypotheses);
            }
            catch (Exception e)
            {
                SetGenerationInfoStateToFailed(boxModule);
                string exText = e.Message;
                exText += (e.InnerException == null) ? "" : e.InnerException.Message;
                throw Ferda.Modules.Exceptions.BoxRuntimeError(e, boxModule.StringIceIdentity, "Error occured: " + e.Message);
            }

            GeneratingStruct generationInfoFinalState = GetGenerationInfo(boxModule);
            if (generationInfoFinalState.generationState == GenerationStateEnum.Running)
            {
                generationInfoFinalState.generationState = GenerationStateEnum.Interrupted;
                SetGenerationInfo(boxModule, generationInfoFinalState);
            }
        }
    }
}
