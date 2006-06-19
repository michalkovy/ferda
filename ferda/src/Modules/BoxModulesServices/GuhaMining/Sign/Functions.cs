using System;
using Ferda.Guha.MiningProcessor;
using Ice;

namespace Ferda.Modules.Boxes.GuhaMining.Sign
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
        public const string PropSignType = "SignType";
        public const string SockBooleanAttributeSetting = "BooleanAttributeSetting";

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

        public SignTypeEnum SignType
        {
            get
            {
                return (SignTypeEnum) Enum.Parse(
                                          typeof (SignTypeEnum),
                                          _boxModule.GetPropertyString(PropSignType)
                                          );
            }
        }

        #endregion

        #region Methods

        public BooleanAttributeSettingFunctionsPrx GetBitStringGeneratorPrx(bool fallOnError)
        {
            return SocketConnections.GetPrx<BooleanAttributeSettingFunctionsPrx>(
                _boxModule,
                SockBooleanAttributeSetting,
                BooleanAttributeSettingFunctionsPrxHelper.checkedCast,
                fallOnError);
        }

        public string GetInputBoxLabel()
        {
            return SocketConnections.GetInputBoxLabel(_boxModule, SockBooleanAttributeSetting);
        }

        public IEntitySetting GetEntitySetting(bool fallOnError)
        {
            return ExceptionsHandler.GetResult<IEntitySetting>(
                fallOnError,
                delegate
                    {
                        BooleanAttributeSettingFunctionsPrx booleanAttribute = GetBitStringGeneratorPrx(true);
                        if (booleanAttribute == null)
                            return null;
                        else
                        {
                            switch (SignType)
                            {
                                case SignTypeEnum.Both:
                                    {
                                        BothSignsSetting result =
                                            new BothSignsSetting();
                                        result.id = new GuidStruct((new Guid()).ToString()); //TODO Guid
                                        result.importance = Importance;
                                        return result;
                                    }
                                case SignTypeEnum.Negative:
                                    {
                                        NegationSetting result =
                                            new NegationSetting();
                                        result.id = new GuidStruct((new Guid()).ToString()); //TODO Guid
                                        result.importance = Importance;
                                        return result;
                                    }
                                case SignTypeEnum.Positive:
                                    return booleanAttribute.GetEntitySetting();
                                default:
                                    throw new NotImplementedException();
                            }
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
            return GetBitStringGeneratorPrx(true).GetAttributeNames();
        }

        #endregion
    }
}