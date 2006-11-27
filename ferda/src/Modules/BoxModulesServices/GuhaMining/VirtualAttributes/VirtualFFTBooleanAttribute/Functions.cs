
using System;
using System.Collections.Generic;
using Ferda.Modules.Helpers.Caching;
using Ferda.Guha.Data;
using Ferda.Guha.MiningProcessor;
using Ferda.Guha.Attribute;
using Ice;
using Exception = System.Exception;
using Common = Ferda.Modules.Boxes.GuhaMining.Tasks.Common;
using Ferda.Modules.Boxes.DataPreparation;
using Ferda.Modules.Boxes.GuhaMining.Tasks;

//using Ferda.Modules.Boxes.DataPreparation.DataSource;

namespace Ferda.Modules.Boxes.GuhaMining.VirtualAttributes.VirtualFFTBooleanAttribute
{
    internal class MiningFunctions : MiningProcessorFunctionsDisp_
    {
        public override BitStringIceWithCategoryId GetNextBitString(Current current__)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override string Run(BoxModulePrx taskBoxModule, BooleanAttribute[] booleanAttributes, CategorialAttribute[] categorialAttributes, Ferda.Guha.Math.Quantifiers.QuantifierBaseFunctionsPrx[] quantifiers, TaskRunParams taskParams, BitStringGeneratorProviderPrx bitStringGenerator, Ferda.ModulesManager.OutputPrx output, GuidStruct attributeId, int[] countVector, out string resultInfo, Current current__)
        {
            throw new Exception("The method or operation is not implemented.");
        }
    }

    internal class Functions : BooleanAttributeSettingWithBSGenerationAbilityFunctionsDisp_, IFunctions, ITask
    {

        private MiningFunctions miningFunctions = new MiningFunctions();

        /// <summary>
        /// The box module.
        /// </summary>
        protected BoxModuleI _boxModule;

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
        }

        #endregion


        #region Properties

        public const string PropMasterIdColumn = "MasterIdColumn";
        public const string PropDataType = "DataType";
        public const string PropCardinality = "Cardinality";
        public const string SockMasterDataTable = "MasterDataTable";
        public const string PropImportance = "Importance";

        private bool _minerInitialized = false;
        private IEnumerator<BitStringIceWithCategoryId> _bitStringEnumerator;
        private int[] _countVector = null;

        private IEnumerator<BitStringIceWithCategoryId> BitStringEnumerator
        {
            get
            {
                if (!_minerInitialized)
                {
                    _bitStringEnumerator = Common.RunTaskNoResult(
                        _boxModule, this,
                        TaskTypeEnum.FourFold,
                        ResultTypeEnum.TraceBoolean,
                        CountVector,
                        Guid, miningFunctions).GetEnumerator();

                    _minerInitialized = true;
                    return _bitStringEnumerator;
                }
                else
                {
                    return _bitStringEnumerator;
                }
            }
        }

        internal int[] CountVector
        {
            get
            {
                if (_countVector == null)
                {
                    BooleanAttributeSettingFunctionsPrx[] _prxs =
                        new BooleanAttributeSettingFunctionsPrx[3];

                    int i = 0;
                    foreach (string s in new string[] { Common.SockAntecedent, Common.SockSuccedent, Common.SockCondition })
                    {
                        _prxs[i] = Common.GetBooleanAttributePrx(_boxModule, s, false);
                        i++;
                    }
                    DataTableFunctionsPrx _dtPrx = GetMasterDataTableFunctionsPrx(true);
                    if (_dtPrx != null)
                    {
                        BitStringGeneratorPrx __prx = null;
                        GuidStruct _guid = null;
                        foreach (BooleanAttributeSettingFunctionsPrx _prx in _prxs)
                        {
                            if ((_guid = _prx.GetEntitySetting().id) != null)
                            {
                                if ((__prx = _prx.GetBitStringGenerator(_guid)) != null)
                                    break;
                            }
                        }
                        if (__prx == null)
                            throw Exceptions.BoxRuntimeError(
                                new ArgumentNullException(), _boxModule.BoxInfo.Identifier,
                                "BitStringGeneratorProxy is null");

                        string[] _primaryKeyColumns = _dtPrx.getDataTableInfo().primaryKeyColumns;
                        if (_primaryKeyColumns.Length > 0)
                        {
                            string _dataTableName = _dtPrx.getDataTableInfo().dataTableName;
                            _countVector = __prx.GetCountVector(_primaryKeyColumns[0], _dataTableName);
                            return _countVector;
                        }
                        else
                        {
                            throw Exceptions.BoxRuntimeError(null, _boxModule.StringIceIdentity, "No unique key selected");
                        }

                    }
                    return null;
                }
                else
                {
                    return _countVector;
                }
            }
        }

        public CardinalityEnum Cardinality
        {
            get
            {
                return CardinalityEnum.Cardinal;
            }
        }

        public GuidStruct Guid
        {
            get { return BoxInfoHelper.GetGuidStructFromProperty("Guid", _boxModule); }
        }

        public string MasterTableIdColumn
        {
            get { return _boxModule.GetPropertyString(PropMasterIdColumn); }
        }

        public string MasterDataTable
        {
            get { return _boxModule.GetPropertyString(PropMasterIdColumn); }
        }

        public ImportanceEnum Importance
        {
            get
            {
                return (ImportanceEnum)Enum.Parse(
                                            typeof(ImportanceEnum),
                                            _boxModule.GetPropertyString(PropImportance)
                                            );
            }
        }

        #endregion


        #region Private methods

        private CacheFlag _cacheFlagColumn = new CacheFlag();
        private GenericColumn _cachedValueColumn = null;

        internal DataTableFunctionsPrx GetMasterDataTableFunctionsPrx(bool fallOnError)
        {
            return SocketConnections.GetPrx<DataTableFunctionsPrx>(
                _boxModule,
                SockMasterDataTable,
                DataTableFunctionsPrxHelper.checkedCast,
                fallOnError);
        }

        private const int _bufferSize = 200;
        private BitStringIceWithCategoryId[] bitStringCache =
            new BitStringIceWithCategoryId[_bufferSize];
        private int _bufferFlag = 0;
        private bool _bufferInitialized = false;

        private Queue<BitStringIceWithCategoryId> _buffer =
            new Queue<BitStringIceWithCategoryId>(_bufferSize);

        /// <summary>
        /// Method which fills the bitstring buffer
        /// </summary>
        private void FillBitStringCache()
        {
            for (int i = 0; i < _bufferSize; i++)
            {
                if (BitStringEnumerator.MoveNext())
                {
                    _buffer.Enqueue(BitStringEnumerator.Current);
                    _bufferFlag = i;
                }
                else
                {
                    return;
                }
            }
        }

        /// <summary>
        /// Method which returns bitstring from buffer
        /// </summary>
        /// <returns></returns>
        private BitStringIceWithCategoryId GetNextBitStringFromBuffer()
        {
            
            if ((!_bufferInitialized) || (_bufferFlag == 0))
            {
                FillBitStringCache();
                if (_bufferFlag == 0)
                    return null;
                _bufferInitialized = true;
            }
            BitStringIceWithCategoryId _returnBs =
                _buffer.Dequeue();
            _bufferFlag--;
            return _returnBs;
            /*
            if (BitStringEnumerator.MoveNext())
            {
                return BitStringEnumerator.Current;
            }
            else
            {
                return null;
            }*/
        }

        #endregion


        #region BooleanAttributeSettingWithBSGenerationAbilityFunctionsDisp_ members

        public override BitStringGeneratorPrx GetBitStringGenerator(GuidStruct attributeId, Current current__)
        {
            if (attributeId.value == Guid.value)
            {
                return BitStringGeneratorPrxHelper.checkedCast(_boxModule.getFunctions());
            }
            return null;
        }

        public IEntitySetting GetEntitySetting(bool fallOnError)
        {
            return ExceptionsHandler.GetResult<IEntitySetting>(
                fallOnError,
                delegate
                {
                    CoefficientSettingI result =
                        new CoefficientSettingI();
                    result.id = Guid;
                    result.importance = Importance;
                    result.generator = BitStringGeneratorPrxHelper.checkedCast(_boxModule.getFunctions());
                    result.maxLength = 1;
                    result.minLength = 1;
                    result.coefficientType = CoefficientTypeEnum.SubsetsOneOne;
                    return result;
                },
                delegate
                {
                    return null;
                },
                _boxModule.StringIceIdentity
                );
        }

        public override IEntitySetting GetEntitySetting(Current current__)
        {
            return GetEntitySetting(true);
        }


        public override CardinalityEnum GetAttributeCardinality(Current current__)
        {
            return Cardinality;
        }

        public override GuidStruct GetAttributeId(Current current__)
        {
            return Guid;
        }

        public override string[] GetMissingInformationCategoryId(Current current__)
        {
            return new string[0];
        }

        public override bool GetNextBitString(out BitStringIceWithCategoryId bitString, Current current__)
        {
            bitString =
                GetNextBitStringFromBuffer();
            if (bitString == null)
            {
                bitString = new
                    BitStringIceWithCategoryId();
                _minerInitialized = false;
                return false;
            }
            return true;
        }

        public override double[] GetCategoriesNumericValues(Current current__)
        {
            return new double[0];
        }

        public override GuidAttributeNamePair[] GetAttributeNames(Current current__)
        {
            List<GuidAttributeNamePair>_result = new List<GuidAttributeNamePair>();
                _result.AddRange(Common.GetAttributeNames(_boxModule, this));

            _result.Add(new GuidAttributeNamePair(Guid,"VirtualFFTBooleanAttribute"));

            return _result.ToArray();
        }

        public override string GetSourceDataTableId(Current current__)
        {
            return Common.GetSourceDataTableId(_boxModule, this);
        }


        #region Not implemented

        public override BitStringIce GetBitString(string categoryId, Current current__)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override string[] GetCategoriesIds(Current current__)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override int[] GetCountVector(string masterIdColumn, string masterDatatableName, Current current__)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion

        #endregion


        #region ITask Members

        public string[] GetCategorialAttributesSocketNames()
        {
            return new string[0];
        }

        public string[] GetBooleanAttributesSocketNames()
        {
            return new string[]
                {
                    Common.SockSuccedent,
                    Common.SockAntecedent,
                    Common.SockCondition
                };
        }

        public bool IsRequiredOneAtMinimumAttributeInSocket(string socketName)
        {
            if (socketName == Common.SockSuccedent)
                return true;
            return false;
        }

        #region Not implemented

        Ferda.Guha.MiningProcessor.Results.SerializableResultInfo ITask.GetResultInfo()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion

        #endregion
    }
}