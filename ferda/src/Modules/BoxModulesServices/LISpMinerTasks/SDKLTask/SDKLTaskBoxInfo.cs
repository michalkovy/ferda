using System;
using System.Collections.Generic;

namespace Ferda.Modules.Boxes.SDKLTask
{
	class SDKLTaskBoxInfo : Ferda.Modules.Boxes.BoxInfo
	{
		public const string typeIdentifier =
            "LISpMinerTasks.SDKLTask";

		protected override string identifier
		{
			get { return typeIdentifier; }
		}

		public override void CreateFunctions(BoxModuleI boxModule, out Ice.Object iceObject, out IFunctions functions)
		{
			SDKLTaskFunctionsI result = new SDKLTaskFunctionsI();
			iceObject = (Ice.Object)result;
			functions = (IFunctions)result;
		}

		public override string[] GetBoxModuleFunctionsIceIds()
		{
			return SDKLTaskFunctionsI.ids__;
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
			SDKLTaskFunctionsI Func = (SDKLTaskFunctionsI)boxModule.FunctionsIObj;
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
				default:
					throw Ferda.Modules.Exceptions.SwitchCaseNotImplementedError(propertyName);
			}
		}

		public override void RunAction(string actionName, BoxModuleI boxModule)
		{
			switch (actionName)
			{
				case "Run":
					((SDKLTaskFunctionsI)boxModule.FunctionsIObj).RunActionRun();
					break;
				default:
					throw Ferda.Modules.Exceptions.SwitchCaseNotImplementedError(actionName);
			}
		}
	}
}