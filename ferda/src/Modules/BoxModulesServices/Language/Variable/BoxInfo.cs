using System;
using Object=Ice.Object;

namespace Ferda.Modules.Boxes.Language.Variable
{
	internal class BoxInfo : Boxes.BoxInfo
	{
		public override void CreateFunctions(BoxModuleI boxModule, out Object iceObject, out IFunctions functions)
		{
	 	   iceObject = null;
	 	   functions = null;
	    }

        public override string[] GetBoxModuleFunctionsIceIds()
        {
            return new string[0];
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

        public const string typeIdentifier = "Language.Variable";

        protected override string identifier
        {
            get { return typeIdentifier; }
        }

        public override PropertyValue GetReadOnlyPropertyValue(string propertyName, BoxModuleI boxModule)
        {
            // Functions Func = (Functions) boxModule.FunctionsIObj;
            switch (propertyName)
            {
                /*case Common.PropTotalNumberOfRelevantQuestions:
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
					return new DateTimeTI(Common.EndTime(Func) - Common.StartTime(Func));*/
                default:
                    throw new NotImplementedException();
            }
        }

        public override void RunAction(string actionName, BoxModuleI boxModule)
        {
            switch (actionName)
            {
                default:
                    throw Exceptions.NameNotExistError(null, actionName);
            }
        }

        public override void Validate(BoxModuleI boxModule)
        {
        }
    }
}
