using System;
using System.Collections.Generic;
using Ferda.Guha.MiningProcessor;
using Ice;

namespace Ferda.Modules.Boxes.GuhaMining.DisjunctionSetting
{
    internal class Functions : BooleanAttributeSettingFunctionsDisp_, IFunctions
    {
        /// <summary>
        /// The box module.
        /// </summary>
        protected BoxModuleI _boxModule;

        //protected IBoxInfo _boxInfo;

        #region IFunctions Members

        /// <summary>
        /// Sets the <see cref="T:Ferda.Modules.BoxModuleI">box module</see>
        /// and the <see cref="T:Ferda.Modules.Boxes.IBoxInfo">box info</see>.
        /// </summary>
        /// <param name="boxModule">The box module.</param>
        /// <param name="boxInfo">The box info.</param>
        public void setBoxModuleInfo(BoxModuleI boxModule, IBoxInfo boxInfo)
        {
            _boxModule = boxModule;
            //_boxInfo = boxInfo;
        }

        #endregion

        #region Properties

        public const string PropImportance = "Importance";
        public const string PropMinimalLength = "MinimalLength";
        public const string PropMaximalLength = "MaximalLength";
        public const string SockBooleanAttributeSetting = "BooleanAttributeSetting";
        public const string SockEquivalenceClasses = "EquivalenceClasses";

        public GuidStruct Guid
        {
            get { return BoxInfoHelper.GetGuidStructFromProperty("Guid", _boxModule); }
        }

        public ImportanceEnum Importance
        {
            get
            {
                return (ImportanceEnum) Enum.Parse(
                                            typeof (ImportanceEnum),
                                            _boxModule.GetPropertyString(PropImportance)
                                            );
            }
        }

        public int MinimalLength
        {
            get { return _boxModule.GetPropertyInt(PropMinimalLength); }
        }

        public int MaximalLength
        {
            get { return _boxModule.GetPropertyInt(PropMaximalLength); }
        }

        #endregion

        #region Methods

        public List<BooleanAttributeSettingFunctionsPrx> GetBooleanAttributeSettingFunctionsPrxs(bool fallOnError)
        {
            return SocketConnections.GetPrxs<BooleanAttributeSettingFunctionsPrx>(
                _boxModule,
                SockBooleanAttributeSetting,
                BooleanAttributeSettingFunctionsPrxHelper.checkedCast,
                true,
                fallOnError);
        }

        public List<EquivalenceClassFunctionsPrx> GetEquivalenceClassFunctionsPrxs(bool fallOnError)
        {
            return SocketConnections.GetPrxs<EquivalenceClassFunctionsPrx>(
                _boxModule,
                SockEquivalenceClasses,
                EquivalenceClassFunctionsPrxHelper.checkedCast,
                false,
                fallOnError);
        }

        public string[] GetInputBoxesLabels()
        {
            return SocketConnections.GetInputBoxesLabels(_boxModule, SockBooleanAttributeSetting);
        }

        public IEntitySetting GetEntitySetting(bool fallOnError)
        {
            return ExceptionsHandler.GetResult<IEntitySetting>(
                fallOnError,
                delegate
                    {
                        List<BooleanAttributeSettingFunctionsPrx> subFormulas =
                            GetBooleanAttributeSettingFunctionsPrxs(fallOnError);
                        List<EquivalenceClassFunctionsPrx> equivalenceClasses =
                            GetEquivalenceClassFunctionsPrxs(fallOnError);
                        if (subFormulas == null || equivalenceClasses == null)
                            return null;
                        else
                        {
                            DisjunctionSettingI result =
                                new DisjunctionSettingI();
                            result.id = Guid;
                            result.importance = Importance;
                            result.maxLength = MaximalLength;
                            result.minLength = MinimalLength;
                            List<IEntitySetting> operands = new List<IEntitySetting>();
                            foreach (BooleanAttributeSettingFunctionsPrx prx in subFormulas)
                            {
                                operands.Add(prx.GetEntitySetting());
                            }
                            result.operands = operands.ToArray();
                            List<GuidStruct[]> classesOfEquivalence = new List<GuidStruct[]>();
                            foreach (EquivalenceClassFunctionsPrx prx in equivalenceClasses)
                            {
                                classesOfEquivalence.Add(prx.GetEquivalenceClass());
                            }
                            result.classesOfEquivalence = classesOfEquivalence.ToArray();
                            return result;
                        }
                    },
                delegate
                    {
                        return null;
                    },
                _boxModule.StringIceIdentity
                );
        }

        #endregion

        #region Ice Functions

        public override IEntitySetting GetEntitySetting(Current current__)
        {
            return GetEntitySetting(true);
        }

        public override GuidAttributeNamePair[] GetAttributeNames(Current current__)
        {
            List<GuidAttributeNamePair> result = new List<GuidAttributeNamePair>();
            List<BooleanAttributeSettingFunctionsPrx> subFormulas =
                GetBooleanAttributeSettingFunctionsPrxs(true);
            foreach (BooleanAttributeSettingFunctionsPrx prx in subFormulas)
            {
                result.AddRange(prx.GetAttributeNames());
            }
            return result.ToArray();
        }

        public override BitStringGeneratorPrx GetBitStringGenerator(GuidStruct attributeGuid, Current current__)
        {
            BitStringGeneratorPrx result;
            List<BooleanAttributeSettingFunctionsPrx> subFormulas =
                GetBooleanAttributeSettingFunctionsPrxs(true);
            foreach (BooleanAttributeSettingFunctionsPrx prx in subFormulas)
            {
                result = prx.GetBitStringGenerator(attributeGuid);
                if (result != null)
                    return result;
            }
            return null;
        }

        public override string GetSourceDataTableId(Current current__)
        {
            List<BooleanAttributeSettingFunctionsPrx> prxs = GetBooleanAttributeSettingFunctionsPrxs(true);
            string last = null;
            if (prxs != null)
                foreach (BooleanAttributeSettingFunctionsPrx prx in prxs)
                {
                    string newer = prx.GetSourceDataTableId();
                    if (String.IsNullOrEmpty(last))
                        last = newer;
                    else if (last != newer)
                        throw Exceptions.BadValueError(null, _boxModule.StringIceIdentity,
                                                       "Mining over only source data table is supported.",
                                                       new string[] {SockBooleanAttributeSetting},
                                                       restrictionTypeEnum.OtherReason);
                }
            return last;
        }

        #endregion
    }
}