using System;
using System.Collections.Generic;
using System.Text;
using Ferda.Modules.Boxes.LISpMinerTasks.AbstractLMTask;

namespace Ferda.Modules.Boxes.LISpMinerTasks
{
    public abstract class LISpMinerAbstractTaskBoxInfo : Ferda.Modules.Boxes.BoxInfo
    {
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
            AbstractLMTaskFunctions Func = (AbstractLMTaskFunctions)boxModule.FunctionsIObj;
            switch (propertyName)
            {
                case "GenerationState":
                    return new Ferda.Modules.StringTI(Func.getGeneratingInfo().generationState.ToString());
                case "GenerationNrOfTests":
                    return new Ferda.Modules.LongTI(Func.getGeneratingInfo().generationNrOfTests);
                case "GenerationNrOfHypotheses":
                    return new Ferda.Modules.LongTI(Func.getGeneratingInfo().generationNrOfHypotheses);
                case "GenerationStartTime":
                    DateTimeT dateTime = Func.getGeneratingInfo().generationStartTime;
                    return new Ferda.Modules.DateTimeTI(dateTime.year, dateTime.month, dateTime.day, dateTime.hour, dateTime.minute, dateTime.second);
                case "GenerationTotalTime":
                    TimeT time = Func.getGeneratingInfo().generationTotalTime;
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
                    ((ILISpMinerAbstractTask)boxModule.FunctionsIObj).RunActionRun();
                    break;
                default:
                    throw Ferda.Modules.Exceptions.NameNotExistError(null, null, null, actionName);
            }
        }
    }
}
