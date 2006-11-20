
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

        private int[] CountVector
        {
            get
            {
                if (_countVector == null)
                {
                    BooleanAttributeSettingFunctionsPrx _prx = null;
                    
                    foreach (string s in new string [] {Common.SockAntecedent, Common.SockSuccedent, Common.SockCondition})
                    {
                        _prx = Common.GetBooleanAttributePrx(_boxModule, s, false);
                        if (_prx != null)
                            break;
                    }
                    DataTableFunctionsPrx _dtPrx = GetMasterDataTableFunctionsPrx(true);
                    if (_dtPrx != null)
                    {
                        GuidStruct _guid = _prx.GetEntitySetting().id;
                        
                        if (_guid != null)
                        {
                            BitStringGeneratorPrx __prx = _prx.GetBitStringGenerator(_guid);
                            string [] _primaryKeyColumns = _dtPrx.getDataTableInfo().primaryKeyColumns;
                            if(_primaryKeyColumns.Length > 0)
                            {
                                string _dataTableName = _dtPrx.getDataTableInfo().dataTableName;
                                return __prx.GetCountVector(_primaryKeyColumns[0],_dataTableName);
                            }
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

        private DataTableFunctionsPrx GetMasterDataTableFunctionsPrx(bool fallOnError)
        {
            return SocketConnections.GetPrx<DataTableFunctionsPrx>(
                _boxModule,
                SockMasterDataTable,
                DataTableFunctionsPrxHelper.checkedCast,
                fallOnError);
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

        public override BitStringIceWithCategoryId GetNextBitString(Current current__)
        {
            if (BitStringEnumerator.MoveNext())
            {
                return BitStringEnumerator.Current;
            }
            else
            {
                _minerInitialized = false;
                return null;
            }
        }

        public override double[] GetCategoriesNumericValues(Current current__)
        {
            return new double[0];
        }

        public override GuidAttributeNamePair[] GetAttributeNames(Current current__)
        {
            return Common.GetAttributeNames(_boxModule, this);
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