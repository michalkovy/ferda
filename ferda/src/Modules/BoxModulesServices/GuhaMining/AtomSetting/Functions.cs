using System;
using Ferda.Guha.Data;
using Ferda.Guha.MiningProcessor;
using Ice;

namespace Ferda.Modules.Boxes.GuhaMining.AtomSetting
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
        public const string PropCoefficientType = "CoefficientType";
        public const string PropMinimalLength = "MinimalLength";
        public const string PropMaximalLength = "MaximalLength";
        public const string SockBitStringGenerator = "BitStringGenerator";

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

        public CoefficientTypeEnum CoefficientType
        {
            get
            {
                return (CoefficientTypeEnum) Enum.Parse(
                                                 typeof (CoefficientTypeEnum),
                                                 _boxModule.GetPropertyString(PropCoefficientType)
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

        public BitStringGeneratorPrx GetBitStringGeneratorPrx(bool fallOnError)
        {
            return SocketConnections.GetPrx<BitStringGeneratorPrx>(
                _boxModule,
                SockBitStringGenerator,
                BitStringGeneratorPrxHelper.checkedCast,
                fallOnError);
        }

        public string[] GetCategoriesIds(bool fallOnError)
        {
            return ExceptionsHandler.GetResult<string[]>(
                fallOnError,
                delegate
                    {
                        BitStringGeneratorPrx bitStringGenerator = GetBitStringGeneratorPrx(fallOnError);
                        if (bitStringGenerator == null)
                            return null;
                        else
                        {
                            return bitStringGenerator.GetCategoriesIds();
                        }
                    },
                delegate
                    {
                        return null;
                    },
                _boxModule.StringIceIdentity
                );
        }

        public CardinalityEnum GetAttributeCardinality(bool fallOnError)
        {
            return ExceptionsHandler.GetResult<CardinalityEnum>(
                fallOnError,
                delegate
                    {
                        BitStringGeneratorPrx bitStringGenerator = GetBitStringGeneratorPrx(fallOnError);
                        if (bitStringGenerator == null)
                            return CardinalityEnum.Nominal;
                        else
                        {
                            return bitStringGenerator.GetAttributeCardinality();
                        }
                    },
                delegate
                    {
                        return CardinalityEnum.Nominal;
                    },
                _boxModule.StringIceIdentity
                );
        }

        public string GetAttributeName(bool fallOnError)
        {
            return ExceptionsHandler.GetResult<string>(
                fallOnError,
                delegate
                    {
                        BitStringGeneratorPrx bitStringGenerator = GetBitStringGeneratorPrx(fallOnError);
                        if (bitStringGenerator == null)
                            return null;
                        else
                        {
                            GuidAttributeNamePair[] attribNames = bitStringGenerator.GetAttributeNames();
                            string attribId = bitStringGenerator.GetAttributeId().value;
                            if (attribNames.Length == 1 && attribNames[0].id.value == attribId)
                                return attribNames[0].attributeName;
                            else if (fallOnError)
                                throw new BoxRuntimeError();
                            else return null;
                        }
                    },
                delegate
                    {
                        return null;
                    },
                _boxModule.StringIceIdentity
                );
        }

        public IEntitySetting GetEntitySetting(bool fallOnError)
        {
            return ExceptionsHandler.GetResult<IEntitySetting>(
                fallOnError,
                delegate
                    {
                        BitStringGeneratorPrx bitStringGenerator = GetBitStringGeneratorPrx(fallOnError);
                        if (bitStringGenerator == null)
                            return null;
                        else
                        {
                            CoefficientSetting result =
                                new CoefficientSetting();
                            result.id = new GuidStruct((new Guid()).ToString()); //TODO Guid
                            result.importance = Importance;
                            result.generator = bitStringGenerator;
                            result.maxLenght = MaximalLength;
                            result.minLenght = MinimalLength;
                            result.coefficientType = CoefficientType;
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
            return GetBitStringGeneratorPrx(true).GetAttributeNames();
        }

        #endregion
    }
}