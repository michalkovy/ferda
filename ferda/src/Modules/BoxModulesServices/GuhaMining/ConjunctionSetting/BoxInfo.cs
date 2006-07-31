using System.Collections.Generic;
using Ferda.Guha.MiningProcessor;
using Ferda.Guha.MiningProcessor.Formulas;
using Ferda.Modules.Helpers.Common;
using Ice;

namespace Ferda.Modules.Boxes.GuhaMining.ConjunctionSetting
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

            string[] inputBoxesLabels = Func.GetInputBoxesLabels();

            if (inputBoxesLabels.Length == 0)
                return null;
            else
            {
                List<string> result = new List<string>();
                foreach (string s in inputBoxesLabels)
                {
                    if (s.Contains(FormulaHelper.SeparatorOr))
                        result.Add("(" + s + ")");
                    else
                        result.Add(s);
                }
                return Print.SequenceToString(result, FormulaHelper.SeparatorAnd);
            }
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

        public const string typeIdentifier = "GuhaMining.ConjunctionSetting";

        protected override string identifier
        {
            get { return typeIdentifier; }
        }

        public override void Validate(BoxModuleI boxModule)
        {
            Functions Func = (Functions) boxModule.FunctionsIObj;

            // try to invoke methods
            IEntitySetting myES = Func.GetEntitySetting(true);
            object dummy = Func.GetAttributeNames();
            dummy = Func.GetSourceDataTableId();

            List<BooleanAttributeSettingFunctionsPrx> booleanAttributes = Func.GetBooleanAttributeSettingFunctionsPrxs(true);
            if (booleanAttributes != null)
            {
                bool notOnlyAuxiliary = false;
                bool containsForced = false;
                foreach (BooleanAttributeSettingFunctionsPrx prx in booleanAttributes)
                {
                    IEntitySetting eS = prx.GetEntitySetting();
                    if (eS.importance != ImportanceEnum.Auxiliary)
                        notOnlyAuxiliary = true;
                    if (eS.importance == ImportanceEnum.Forced)
                        containsForced = true;
                }
                
                // inner entity can not be auxiliary
                if (!notOnlyAuxiliary)
                    throw Exceptions.BadValueError(
                        null,
                        boxModule.StringIceIdentity,
                        "Inner boolean attribute settings can not be only auxiliary.",
                        new string[] { Functions.SockBooleanAttributeSetting },
                        restrictionTypeEnum.OtherReason
                        );
                if (containsForced &&
                    myES.importance != ImportanceEnum.Forced)
                    throw Exceptions.BadValueError(
                        null,
                        boxModule.StringIceIdentity,
                        "Some inner boolean attribute setting is forced, therefore current has to be also forced.",
                        new string[] { Functions.SockBooleanAttributeSetting },
                        restrictionTypeEnum.OtherReason
                        );
            }
        }
    }
}