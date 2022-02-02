using System;
using Object=Ice.Object;

namespace Ferda.Modules.Boxes.GuhaMining.Quantifiers.FourFold.Equivalence.FoundedEquivalence
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
            rvar f = new Functions();
            return f.ice_ids();
        }

        public override string GetDefaultUserLabel(BoxModuleI boxModule)
        {
            return null;
        }

        public const string typeIdentifier = "GuhaMining.Quantifiers.FourFold.Equivalence.FoundedEquivalence";

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

                case Common.PropFromColumnBoundary:
                    return new StringTI(Func.FromColumnBoundary.ToString());
                case Common.PropFromColumnBoundaryIndex:
                    return new IntTI(Func.FromColumnBoundaryIndex);
                case Common.PropToColumnBoundary:
                    return new StringTI(Func.ToColumnBoundary.ToString());
                case Common.PropToColumnBoundaryIndex:
                    return new IntTI(Func.ToColumnBoundaryIndex);

                case Common.PropUnits:
                    return new StringTI(Func.Units.ToString());

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