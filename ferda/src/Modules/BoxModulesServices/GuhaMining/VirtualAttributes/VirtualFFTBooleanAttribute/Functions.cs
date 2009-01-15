
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

        public override string ETreeRun(BoxModulePrx taskBoxModule, ETreeTaskRunParams taskParams, ModulesManager.OutputPrx output, out string resultInfo, Current current__)
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

     //   public const string PropMasterIdColumn = "MasterIdColumn";
    //    public const string PropDetailIdColumn = "DetailIdColumn";
        public const string PropDataType = "DataType";
        public const string PropCardinality = "Cardinality";
        public const string SockMasterDataTable = "MasterDataTable";
       // public const string SockDetailDataTable = "DetailDataTable";
        public const string PropImportance = "Importance";
        public const string PropMaxNumberOfHypotheses = "MaxNumberOfHypotheses";

        /// <summary>
        /// Maximum of generated relevant questions
        /// </summary>
        private long MaxNumberOfHypotheses
        {
            get { return _boxModule.GetPropertyLong(PropMaxNumberOfHypotheses); }
        }

        private bool _minerInitialized = false;
        private IEnumerator<BitStringIceWithCategoryId> _bitStringEnumerator;
        private int[] _countVector = null;
        private int _skipFirstN = -1;
        private int bitStringsYielded = 0;

        /// <summary>
        /// Enumerator for bitstrings yielded by virtual attribute
        /// </summary>
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
                        Guid, miningFunctions, _skipFirstN, _current).GetEnumerator();

                    _minerInitialized = true;
                    return _bitStringEnumerator;
                }
                else
                {
                    return _bitStringEnumerator;
                }
            }
        }

        /// <summary>
        /// Countvector
        /// </summary>
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

                    //proxy of master data table
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
                            _countVector = __prx.GetCountVector(
                                _primaryKeyColumns[0], _dataTableName, String.Empty);
                            return _countVector;
                        }
                        else
                        {
                            throw Exceptions.BoxRuntimeError(null, _boxModule.StringIceIdentity, "No primary key selected");
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

        /// <summary>
        /// Attribute cardinality
        /// </summary>
        public CardinalityEnum Cardinality
        {
            get
            {
                return CardinalityEnum.Cardinal;
            }
        }

        /// <summary>
        /// Guid
        /// </summary>
        public GuidStruct Guid
        {
            get { return BoxInfoHelper.GetGuidStructFromProperty("Guid", _boxModule); }
        }

        /*
  /// <summary>
  /// Master datatable id column (for CountVector)
  /// </summary>
  public string MasterTableIdColumn
  {
      get { return _boxModule.GetPropertyString(PropMasterIdColumn); }
  }

  public string MasterDataTable
  {
      get { return _boxModule.GetPropertyString(PropMasterIdColumn); }
  }*/

        /// <summary>
        /// Literal importance
        /// </summary>
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

        /*
        /// <summary>
        /// Gets column names for master datatable
        /// </summary>
        /// <param name="fallOnError"></param>
        /// <returns>Array with column names</returns>
        public string[] GetMasterColumnsNames(bool fallOnError)
        {
            return ExceptionsHandler.GetResult<string[]>(
                fallOnError,
                delegate
                {
                    DataTableFunctionsPrx tmp1 = GetMasterDataTableFunctionsPrx(fallOnError);

                    if (tmp1 != null)
                        return tmp1.getColumnsNames();
                    return null;
                },
                delegate
                {
                    return null;
                },
                _boxModule.StringIceIdentity
                );
        }

        /// <summary>
        /// Gets column names for detail datatable
        /// </summary>
        /// <param name="fallOnError"></param>
        /// <returns>Array with column names</returns>
        public string[] GetDetailColumnsNames(bool fallOnError)
        {
            return ExceptionsHandler.GetResult<string[]>(
                fallOnError,
                delegate
                {
                    DataTableFunctionsPrx tmp1 = GetDetailDataTableFunctionsPrx(fallOnError);

                    if (tmp1 != null)
                        return tmp1.getColumnsNames();
                    return null;
                },
                delegate
                {
                    return null;
                },
                _boxModule.StringIceIdentity
                );
        }
        */

        /// <summary>
        /// Gets proxy of master datatable
        /// </summary>
        /// <param name="fallOnError"></param>
        /// <returns>Datatable proxy</returns>
        public DataTableFunctionsPrx GetMasterDataTableFunctionsPrx(bool fallOnError)
        {
            return SocketConnections.GetPrx<DataTableFunctionsPrx>(
                _boxModule,
                SockMasterDataTable,
                DataTableFunctionsPrxHelper.checkedCast,
                fallOnError);
        }
        /*
        /// <summary>
        /// Gets proxy of detail datatable
        /// </summary>
        /// <param name="fallOnError"></param>
        /// <returns>Datatable proxy</returns>
        public DataTableFunctionsPrx GetDetailDataTableFunctionsPrx(bool fallOnError)
        {
            return SocketConnections.GetPrx<DataTableFunctionsPrx>(
                _boxModule,
                SockDetailDataTable,
                DataTableFunctionsPrxHelper.checkedCast,
                fallOnError);
        }*/


        private CacheFlag _cacheFlagColumn = new CacheFlag();

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

        /// <summary>
        /// Gets the connected bitstring generator
        /// </summary>
        /// <param name="attributeId">Attribute id</param>
        /// <param name="current__"></param>
        /// <returns>Bitstring generator proxy</returns>
        public override BitStringGeneratorPrx GetBitStringGenerator(GuidStruct attributeId, Current current__)
        {
            if (attributeId.value == Guid.value)
            {
                return BitStringGeneratorPrxHelper.checkedCast(_boxModule.getFunctions());
            }
            return null;
        }

        /// <summary>
        /// Returns the proxy of all the bit string generators of this box. In case
        /// of Virtual attribute, it is just itself. 
        /// </summary>
        /// <param name="current__">ICE stuff</param>
        /// <returns>Proxy of all the bit string generators</returns>
        public override BitStringGeneratorPrx[] GetBitStringGenerators(Current current__)
        {
            BitStringGeneratorPrx[] result = new BitStringGeneratorPrx[1];
            result[0] = BitStringGeneratorPrxHelper.checkedCast(_boxModule.getFunctions());
            return result;
        }

        /// <summary>
        /// Gets entity setting
        /// </summary>
        /// <param name="fallOnError">Whether to fall on error</param>
        /// <returns>Entity setting</returns>
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

        /// <summary>
        /// Gets entity setting
        /// </summary>
        /// <param name="current__"></param>
        /// <returns>Entity setting</returns>
        public override IEntitySetting GetEntitySetting(Current current__)
        {
            return GetEntitySetting(true);
        }

        /// <summary>
        /// Gets attribute cardinality
        /// </summary>
        /// <param name="current__"></param>
        /// <returns>Attribute cardinality</returns>
        public override CardinalityEnum GetAttributeCardinality(Current current__)
        {
            return Cardinality;
        }

        /// <summary>
        /// Gets attribute id
        /// </summary>
        /// <param name="current__"></param>
        /// <returns>Attribute id</returns>
        public override GuidStruct GetAttributeId(Current current__)
        {
            return Guid;
        }

        /// <summary>
        /// Gets missing information categoryid
        /// </summary>
        /// <param name="current__"></param>
        /// <returns>Missing information categoryid</returns>
        public override string[] GetMissingInformationCategoryId(Current current__)
        {
            return new string[0];
        }

        private Ice.Current _current = null;

        /// <summary>
        /// Gets next bitstring for the virtual columns
        /// </summary>
        /// <param name="skipFirstN">Skips first bistrings</param>
        /// <param name="bitString">Returned bitstring</param>
        /// <param name="current__"></param>
        /// <returns>True if more bitstrings can be returned</returns>
        public override bool GetNextBitString(int skipFirstN, out BitStringIceWithCategoryId bitString, Current current__)
        {
            if (bitStringsYielded < MaxNumberOfHypotheses)
            {
                _skipFirstN = skipFirstN;

                _current = current__;
                bitString =
                    GetNextBitStringFromBuffer();
                if (bitString == null)
                {
                    bitString = new
                        BitStringIceWithCategoryId();
                    _minerInitialized = false;
                    bitStringsYielded = 0;
                    return false;
                }
                return true;
            }
            else
            {
                bitString = new
                        BitStringIceWithCategoryId();
                _minerInitialized = false;
                bitStringsYielded = 0;
                return false;
            }
        }

        /// <summary>
        /// Gets category numeric values - for future implementations
        /// </summary>
        /// <param name="current__"></param>
        /// <returns>Category numeric values</returns>
        public override double[] GetCategoriesNumericValues(Current current__)
        {
            return new double[0];
        }

        /// <summary>
        /// Gets attribute name
        /// </summary>
        /// <param name="current__"></param>
        /// <returns></returns>
        public override GuidAttributeNamePair[] GetAttributeNames(Current current__)
        {
            List<GuidAttributeNamePair> _result = new List<GuidAttributeNamePair>();
            _result.AddRange(Common.GetAttributeNames(_boxModule, this));

            _result.Add(new GuidAttributeNamePair(Guid, "V-FFT-Bool"));

            return _result.ToArray();
        }

        /// <summary>
        /// Gets source datatable id
        /// </summary>
        /// <param name="current__"></param>
        /// <returns></returns>
        public override string GetSourceDataTableId(Current current__)
        {
            return Common.GetSourceDataTableIdMR(_boxModule, this);
        }

        /// <summary>
        /// Gets categories ids
        /// </summary>
        /// <param name="current__"></param>
        /// <returns></returns>
        public override string[] GetCategoriesIds(Current current__)
        {
            return new string[0];
            //throw new Exception("The method or operation is not implemented.");
        }

        /// <summary>
        /// Returns information from the column about the values and frequencies
        /// of the column. This fucntion was added to the Slice desing for
        /// the PMML support.
        /// </summary>
        /// <param name="current__"></param>
        /// <returns>ValuesAndFrequencies structure</returns>
        public override ValuesAndFrequencies GetColumnValuesAndFrequencies(Current current__)
        {
            throw new BoxRuntimeError("Virtual 4FT attribute", "The function GetColumnValuesAndFrequencies is not supported.");
        }

        /// <summary>
        /// Returns serialized attribute <see cref="Ferda.Guha.Attribute"/>.
        /// </summary>
        /// <param name="current__">ICE stuff</param>
        /// <returns>Serialized attribute</returns>
        public override string getAttribute(Current current__)
        {
            throw new BoxRuntimeError("Virtual 4FT attribute", "The function getAttribute is not supported.");
        }

        /// <summary>
        /// Gets the name of the column from which the attribute and bit string generator
        /// is created. Added for PMML purposes. 
        /// </summary>
        /// <param name="current__">ICE stuff</param>
        /// <returns></returns>
        public override string GetColumnName(Current current__)
        {
            return "Virtual 4FT attribute";
        }

        #region Not implemented

        public override BitStringIce GetBitString(string categoryId, Current current__)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override int[] GetCountVector(string masterIdColumn, string masterDatatableName, string detailIdColumn, Current current__)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        /// <summary>
        /// Gets categories and frequencies of the underlying attribute. The function
        /// was taken from AttributeFunctions for purposes of PMML generation. 
        /// </summary>
        /// <param name="current__">ICE stuff</param>
        /// <returns>Values and frequencies of the attribute categories</returns>
        public override ValuesAndFrequencies getCategoriesAndFrequencies(Current current__)
        {
            throw new BoxRuntimeError("Virtual 4FT attribute", "The function GetColumnValuesAndFrequencies is not supported.");
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

        /// <summary>
        /// Gets maximal count of generated bitstrings for virtual columns
        /// </summary>
        /// <param name="current__"></param>
        /// <returns>Maximal count of generated bistrings</returns>
        public override long GetMaxBitStringCount(Current current__)
        {
            return MaxNumberOfHypotheses;
        }
    }
}