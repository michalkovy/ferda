using System;
using Object=Ice.Object;

namespace Ferda.Modules.Boxes.GuhaMining.Tasks.FourFold
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

        public const string typeIdentifier = "GuhaMining.Tasks.FourFold";

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
				case Common.PropTotalTime:
                    TimeSpan _ts = (TimeSpan)((Common.EndTime(Func) - Common.StartTime(Func)));
                    return new StringTI(_ts.ToString());
                default:
                    throw new NotImplementedException();
            }
        }

        public override void RunAction(string actionName, BoxModuleI boxModule)
        {
            Functions Func = (Functions) boxModule.FunctionsIObj;

              IntT TNumberRuns = (IntT)boxModule.GetPropertyOther("NumberRuns");
              int IntNumberRuns = TNumberRuns.getIntValue();
              IntNumberRuns++;
              TNumberRuns.intValue = IntNumberRuns;
              PropertyValue NRValue = TNumberRuns;
                
              boxModule.setProperty("NumberRuns", NRValue);

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
