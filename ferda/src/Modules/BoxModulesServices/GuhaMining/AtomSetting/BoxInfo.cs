using Ferda.Guha.Data;
using Ferda.Guha.MiningProcessor;
using Ice;

namespace Ferda.Modules.Boxes.GuhaMining.AtomSetting
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
            Functions Func = (Functions) boxModule.FunctionsIObj;
            string attributeName = Func.GetAttributeName(false);
            if (attributeName == null)
                return null;
            else
            {
                string shortCoefficientType =
                    GetPropertyOptionShortLocalizedLabel(Functions.PropCoefficientType, Func.CoefficientType.ToString(),
                                                         boxModule.LocalePrefs);
                return
                    attributeName + "(" + shortCoefficientType + "[" + Func.MinimalLength + "-" + Func.MaximalLength +
                    "])";
            }
        }

        public const string typeIdentifier = "GuhaMining.AtomSetting";

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
            Functions Func = (Functions) boxModule.FunctionsIObj;

            if (Func.MinimalLength > Func.MaximalLength)
                throw Exceptions.BadValueError(null, boxModule.StringIceIdentity,
                                               "Minimal length is not less or equal to maximal length.",
                                               new string[] {Functions.PropMinimalLength, Functions.PropMaximalLength},
                                               restrictionTypeEnum.Minimum);

            if (Func.MinimalLength < 1)
                throw Exceptions.BadValueError(null, boxModule.StringIceIdentity,
                                               "Minimal length must be greather than 1.",
                                               new string[] {Functions.PropMinimalLength},
                                               restrictionTypeEnum.Minimum);

            string[] categories = Func.GetCategoriesIds(true);
            if (categories != null && categories.Length < Func.MinimalLength)
                throw Exceptions.BadValueError(null, boxModule.StringIceIdentity,
                                               "Count of categories in input attribute must be greater than Minimal length.",
                                               new string[]
                                                   {Functions.SockBitStringGenerator, Functions.PropMinimalLength},
                                               restrictionTypeEnum.OtherReason);

            //conform coefficient type a attribute cardinality type
            CardinalityEnum attributeCardinalityType = Func.GetAttributeCardinality(true);
            CoefficientTypeEnum coefficientType = Func.CoefficientType;
            if (coefficientType == CoefficientTypeEnum.Subsets)
            {
                //OK
            }
            else if (coefficientType == CoefficientTypeEnum.LeftCuts
                     || coefficientType == CoefficientTypeEnum.RightCuts
                     || coefficientType == CoefficientTypeEnum.Cuts
                     || coefficientType == CoefficientTypeEnum.Intervals)
            {
                if (attributeCardinalityType == CardinalityEnum.Ordinal
                    || attributeCardinalityType == CardinalityEnum.Cardinal)
                {
                    //OK
                }
                else
                {
                    // cyclic ordinal / nominal
                    throw Exceptions.BadValueError(null, boxModule.StringIceIdentity,
                                                   "For current cardinality type of source attribute is not allowed to make cuts or intervals.",
                                                   new string[]
                                                       {Functions.SockBitStringGenerator, Functions.PropCoefficientType},
                                                   restrictionTypeEnum.OtherReason);
                }
            }
            else if (coefficientType == CoefficientTypeEnum.CyclicIntervals)
            {
                if (attributeCardinalityType == CardinalityEnum.OrdinalCyclic)
                {
                    //OK
                }
                else
                {
                    // nominal / ordinal / cardinal / numeric cardinal
                    throw Exceptions.BadValueError(null, boxModule.StringIceIdentity,
                                                   "For current cardinality type of source attribute is not allowed to make cyclic intervals.",
                                                   new string[]
                                                       {Functions.SockBitStringGenerator, Functions.PropCoefficientType},
                                                   restrictionTypeEnum.OtherReason);
                }
            }

            // try to invoke methods
            object dummy = Func.GetEntitySetting(true);
            dummy = Func.GetAttributeName(true);
        }
    }
}