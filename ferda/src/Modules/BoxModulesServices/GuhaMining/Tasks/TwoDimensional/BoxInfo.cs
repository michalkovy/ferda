using System;
using Object=Ice.Object;

namespace Ferda.Modules.Boxes.GuhaMining.Tasks.TwoDimensional
{
    internal class BoxInfo : Boxes.BoxInfo
    {
        public override void CreateFunctions(BoxModuleI boxModule, out Object iceObject, out IFunctions functions)
        {
            Functions result = new Functions();
            iceObject = result;
            functions = result;
        }

        public override string[] GetBoxModuleFunctionsIceIds()
        {
            return Functions.ids__;
        }

        public override string GetDefaultUserLabel(BoxModuleI boxModule)
        {
            return null;
        }

        public override ModulesAskingForCreation[] GetModulesAskingForCreation(string[] localePrefs,
                                                                               BoxModuleI boxModule)
        {
            return new ModulesAskingForCreation[0];
        }

        public override SelectString[] GetPropertyOptions(string propertyName, BoxModuleI boxModule)
        {
            return null;
        }

        public const string typeIdentifier = "GuhaMining.Tasks.TwoDimensional";

        protected override string identifier
        {
            get { return typeIdentifier; }
        }

        public override PropertyValue GetReadOnlyPropertyValue(string propertyName, BoxModuleI boxModule)
        {
            Functions Func = (Functions) boxModule.FunctionsIObj;
            switch (propertyName)
            {
                case Common.PropTotalNumberOfRelevantQuestions:
                    return new DoubleTI(Common.TotalNumberOfRelevantQuestions(Func));
                case Common.PropNumberOfVerifications:
                    return new LongTI(Common.NumberOfVerifications(Func));
                case Common.PropNumberOfHypotheses:
                    return new LongTI(Common.NumberOfHypotheses(Func));
                case Common.PropStartTime:
                    return new DateTimeTI(Common.StartTime(Func));
                case Common.PropEndTime:
                    return new DateTimeTI(Common.EndTime(Func));
                default:
                    throw new NotImplementedException();
            }
        }

        public override void RunAction(string actionName, BoxModuleI boxModule)
        {
            Functions Func = (Functions) boxModule.FunctionsIObj;
            switch (actionName)
            {
                case "Run":
                    Func.Run();
                    break;
                default:
                    throw Exceptions.NameNotExistError(null, actionName);
            }
        }

        public override void Validate(BoxModuleI boxModule)
        {
            // all used attributes are from the same data table

            Functions Func = (Functions) boxModule.FunctionsIObj;
            Func.GetSourceDataTableId();
        }
    }
}