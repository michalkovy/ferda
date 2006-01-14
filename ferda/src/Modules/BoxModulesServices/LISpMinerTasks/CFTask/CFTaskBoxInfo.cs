using System;
using System.Collections.Generic;
using System.Text;
using Ferda.Modules.Boxes;
using Ferda.Modules;
using Ferda.Modules.MetabaseLayer;
//using Ferda.Modules.DataMiningCommon;
using Ferda.Modules.Boxes.DataMiningCommon;
using Ferda.Modules.Helpers.Data;

namespace Ferda.Modules.Boxes.CFTask
{
    class CFTaskBoxInfo : Ferda.Modules.Boxes.BoxInfo
    {
        public const string typeIdentifier =
            "LISpMinerTasks.CFTask";

        public override void CreateFunctions(BoxModuleI boxModule, out Ice.Object iceObject, out IFunctions functions)
        {
            CFTaskFunctionsI result = new CFTaskFunctionsI();
            iceObject = (Ice.Object)result;
            functions = (IFunctions)result;
        }

        public override string[] GetBoxModuleFunctionsIceIds()
        {
            return CFTaskFunctionsI.ids__;
        }

		public override string GetDefaultUserLabel(BoxModuleI boxModule)
		{
			return null;
		}

        public override PropertyValue GetPropertyObjectFromInterface(string propertyName, Ice.ObjectPrx objectPrx)
        {
            
            if (propertyName == "GenerationInfo")
                return new GenerationInfoTI(GenerationInfoTInterfacePrxHelper.checkedCast(objectPrx));
            if (propertyName == "Hypotheses")
                return new HypothesesTI(HypothesesTInterfacePrxHelper.checkedCast(objectPrx));
            throw Ferda.Modules.Exceptions.NameNotExistError(null, null, null, propertyName);
        }

        public override SelectString[] GetPropertyOptions(string propertyName, BoxModuleI boxModule)
        {
            return null;
        }

        public override ModulesAskingForCreation[] GetModulesAskingForCreation(string[] localePrefs, BoxModuleI boxModule)
        {
            return new ModulesAskingForCreation[0] { };
        }


        public override PropertyValue GetReadOnlyPropertyValue(String propertyName, BoxModuleI boxModule)
        {
            CFTaskFunctionsI Func = (CFTaskFunctionsI)boxModule.FunctionsIObj;
            switch (propertyName)
            {
                case "GenerationState":
                    return new Ferda.Modules.StringTI(Func.GetGeneratingInfo().generationState.ToString());
                case "GenerationNrOfTests":
                    return new Ferda.Modules.LongTI(Func.GetGeneratingInfo().generationNrOfTests);
                case "GenerationNrOfHypotheses":
                    return new Ferda.Modules.LongTI(Func.GetGeneratingInfo().generationNrOfHypotheses);
                case "GenerationStartTime":
                    DateTimeT dateTime = Func.GetGeneratingInfo().generationStartTime;
                    return new Ferda.Modules.DateTimeTI(dateTime.year, dateTime.month, dateTime.day, dateTime.hour, dateTime.minute, dateTime.second);
                case "GenerationTotalTime":
                    TimeT time = Func.GetGeneratingInfo().generationTotalTime;
                    return new Ferda.Modules.TimeTI(time.hour, time.minute, time.second);
                /*
            case "GenerationInfo":
                return new Ferda.Modules.GenerationInfoTI(Func.GetGeneratingInfo());
            case "Hypotheses":
                return new Ferda.Modules.HypothesesTI(Func.GetResult());
                 */
                default:
                    throw Ferda.Modules.Exceptions.SwitchCaseNotImplementedError(propertyName);
            }
        }

        public override void RunAction(string actionName, BoxModuleI boxModule)
        {
            switch (actionName)
            {
                case "Run":
                    ((CFTaskFunctionsI)boxModule.FunctionsIObj).RunActionRun();
                    break;
                default:
                    throw Ferda.Modules.Exceptions.NameNotExistError(null, null, null, actionName);
            }
        }
        

        protected override string identifier
        {
            get { return typeIdentifier; }
        }

       
}
}
