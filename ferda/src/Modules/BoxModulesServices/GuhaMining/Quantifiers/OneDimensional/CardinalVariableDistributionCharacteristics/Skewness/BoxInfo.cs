using System;
using Object=Ice.Object;

namespace Ferda.Modules.Boxes.GuhaMining.Quantifiers.OneDimensional.CardinalVariableDistributionCharacteristics.Skewness
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

        public const string typeIdentifier =
            "GuhaMining.Quantifiers.OneDimensional.CardinalVariableDistributionCharacteristics.Skewness";

        protected override string identifier
        {
            get { return typeIdentifier; }
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

        public override void Validate(BoxModuleI boxModule)
        {
            //Functions Func = (Functions) boxModule.FunctionsIObj;
        }

        public override PropertyValue GetReadOnlyPropertyValue(string propertyName, BoxModuleI boxModule)
        {
            Functions Func = (Functions) boxModule.FunctionsIObj;
            switch (propertyName)
            {
                case Common.PropFromRowBoundary:
                    return new StringTI(Func.FromRowBoundary.ToString());
                case Common.PropFromRowBoundaryIndex:
                    return new IntTI(Func.FromRowBoundaryIndex);
                case Common.PropToRowBoundary:
                    return new StringTI(Func.ToRowBoundary.ToString());
                case Common.PropToRowBoundaryIndex:
                    return new IntTI(Func.ToRowBoundaryIndex);

                case Common.PropUnits:
                    return new StringTI(Func.Units.ToString());
                case Common.PropMissingInformationHandling:
                    return new StringTI(Func.MissingInformationHandling.ToString());

                case Common.PropNeedsNumericValues:
                    return new BoolTI(Func.NeedsNumericValues);
                case Common.PropSupportedData:
                    return new StringTI(Func.SupportedData.ToString());
                default:
                    throw new NotImplementedException();
            }
        }
    }
}