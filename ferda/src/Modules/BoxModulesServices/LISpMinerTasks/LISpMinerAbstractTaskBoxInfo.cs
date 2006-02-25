using System;
using System.Collections.Generic;
using System.Text;
using Ferda.Modules.Boxes.LISpMinerTasks.AbstractLMTask;

namespace Ferda.Modules.Boxes.LISpMinerTasks
{
    /// <summary>
    /// Base for LISp-Miner task BoxInfo.
    /// </summary>
    public abstract class LISpMinerAbstractTaskBoxInfo : Ferda.Modules.Boxes.BoxInfo
    {
        /// <summary>
        /// Gets default value for box module user label.
        /// </summary>
        public override string GetDefaultUserLabel(BoxModuleI boxModule)
        {
            return null;
        }

        /// <summary>
        /// Gets <see cref="T:Ferda.Modules.PropertyValue"/> from
        /// <see cref="T:Ice.ObjectPrx">objectPrx</see> parameter.
        /// </summary>
        public override PropertyValue GetPropertyObjectFromInterface(string propertyName, Ice.ObjectPrx objectPrx)
        {

            if (propertyName == "GenerationInfo")
                return new GenerationInfoTI(GenerationInfoTInterfacePrxHelper.checkedCast(objectPrx));
            if (propertyName == "Hypotheses")
                return new HypothesesTI(HypothesesTInterfacePrxHelper.checkedCast(objectPrx));
            throw Ferda.Modules.Exceptions.NameNotExistError(null, null, null, propertyName);
        }

        /// <summary>
        /// Gets array of <see cref="T:Ferda.Modules.SelectString"/> as
        /// options for property, whose options are dynamically variable.
        /// </summary>
        public override SelectString[] GetPropertyOptions(string propertyName, BoxModuleI boxModule)
        {
            return null;
        }

        /// <summary>
        /// Gets the box modules asking for creation.
        /// </summary>
        /// <param name="localePrefs">The localization preferences.</param>
        /// <param name="boxModule">The box module.</param>
        /// <returns>
        /// Array of <see cref="T:Ferda.Modules.ModuleAskingForCreation">
        /// Modules Asking For Creation</see>.
        /// </returns>
        public override ModulesAskingForCreation[] GetModulesAskingForCreation(string[] localePrefs, BoxModuleI boxModule)
        {
            return new ModulesAskingForCreation[0] { };
        }


        /// <summary>
        /// Gets value of readonly property value.
        /// </summary>
        /// <param name="propertyName">Name of readonly property.</param>
        /// <param name="boxModule">Box module.</param>
        /// <returns>
        /// A <see cref="T:Ferda.Modules.PropertyValue"/> of
        /// readonly property named <c>propertyName</c>.
        /// </returns>
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

        /// <summary>
        /// Executes (runs) action specified by <c>actionName</c>.
        /// </summary>
        /// <param name="actionName">Name of the action.</param>
        /// <param name="boxModule">The Box module.</param>
        /// <exception cref="T:Ferda.Modules.NameNotExistError">Thrown if action named <c>actionName</c> doesn`t exist.</exception>
        /// <exception cref="T:Ferda.Modules.BoxRuntimeError">Thrown if any runtime error occured while executing the action.</exception>
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
