using System;
using System.Collections.Generic;
using Ferda.Guha.Math.Quantifiers;
using Ferda.Guha.MiningProcessor;
using Ferda.Guha.MiningProcessor.Results;
using Ferda.ModulesManager;

namespace Ferda.Modules.Boxes.GuhaMining.Tasks
{
    public interface ITask
    {
        SerializableResultInfo GetResultInfo();
        string[] GetCategorialAttributesSocketNames();
        string[] GetBooleanAttributesSocketNames();
        bool IsRequiredOneAtMinimumAttributeInSocket(string socketName);
    }

    public static class Common
    {
        #region Other Properties

        public const string PropMaxNumberOfHypotheses = "MaxNumberOfHypotheses";
        public const string PropExecutionType = "ExecutionType";
        public const string PropWorkingWithSecondSetMode = "WorkingWithSecondSetMode";

        public static long MaxNumberOfHypotheses(BoxModuleI boxModule)
        {
            return boxModule.GetPropertyLong(PropMaxNumberOfHypotheses);
        }

        public static TaskEvaluationTypeEnum ExecutionType(BoxModuleI boxModule)
        {
            return (TaskEvaluationTypeEnum) Enum.Parse(
                                                typeof (TaskEvaluationTypeEnum),
                                                boxModule.GetPropertyString(PropExecutionType)
                                                );
        }

        public static WorkingWithSecondSetModeEnum WorkingWithSecondSetMode(BoxModuleI boxModule)
        {
            return
                (WorkingWithSecondSetModeEnum)
                Enum.Parse(typeof (WorkingWithSecondSetModeEnum),
                           boxModule.GetPropertyString(PropWorkingWithSecondSetMode));
        }

        #endregion

        #region Boolean and Categorial attributes

        public const string SockAntecedent = "Antecedent";
        public const string SockSuccedent = "Succedent";
        public const string SockCondition = "Condition";
        public const string SockRowAttribute = "RowAttribute";
        public const string SockColumnAttribute = "ColumnAttribute";
        public const string SockAttribute = "Attribute";
        public const string SockSDCedent1 = "SDCedent1";
        public const string SockSDCedent2 = "SDCedent2";

        private static MarkEnum socketName2MarkEnum(string socketName)
        {
            switch (socketName)
            {
                case SockAntecedent:
                    return MarkEnum.Antecedent;
                case SockSuccedent:
                    return MarkEnum.Succedent;
                case SockCondition:
                    return MarkEnum.Condition;
                case SockRowAttribute:
                    return MarkEnum.RowAttribute;
                case SockColumnAttribute:
                    return MarkEnum.ColumnAttribute;
                case SockAttribute:
                    return MarkEnum.Attribute;
                case SockSDCedent1:
                    return MarkEnum.FirstSet;
                case SockSDCedent2:
                    return MarkEnum.SecondSet;
                default:
                    throw new NotImplementedException();
            }
        }

        public static BooleanAttributeSettingFunctionsPrx GetBooleanAttributePrx(BoxModuleI boxModule, string sockName,
                                                                                 bool fallOnError)
        {
            return SocketConnections.GetPrx<BooleanAttributeSettingFunctionsPrx>(
                boxModule,
                sockName,
                BooleanAttributeSettingFunctionsPrxHelper.checkedCast,
                fallOnError);
        }

        public static List<BitStringGeneratorPrx> GetCategorialAttributePrxs(BoxModuleI boxModule, string sockName,
                                                                             bool oneAtMininum, bool fallOnError)
        {
            return SocketConnections.GetPrxs<BitStringGeneratorPrx>(
                boxModule,
                sockName,
                BitStringGeneratorPrxHelper.checkedCast,
                oneAtMininum,
                fallOnError);
        }

        public static BooleanAttribute GetBooleanAttribute(BoxModuleI boxModule, MarkEnum semantic, string sockName,
                                                           bool fallOnError)
        {
            BooleanAttributeSettingFunctionsPrx prx = GetBooleanAttributePrx(boxModule, sockName, fallOnError);
            if (prx != null)
            {
                return new BooleanAttribute(semantic, prx.GetEntitySetting());
            }
            return null;
        }

        public static List<CategorialAttribute> GetCategorialAttributes(BoxModuleI boxModule, MarkEnum semantic,
                                                                        string sockName, bool oneAtMinimum,
                                                                        bool fallOnError)
        {
            List<BitStringGeneratorPrx> prxs =
                GetCategorialAttributePrxs(boxModule, sockName, oneAtMinimum, fallOnError);
            if (prxs != null)
            {
                List<CategorialAttribute> result = new List<CategorialAttribute>(prxs.Count);
                foreach (BitStringGeneratorPrx prx in prxs)
                {
                    result.Add(new CategorialAttribute(semantic, prx));
                }
                return result;
            }
            return null;
        }

        public static BooleanAttribute[] GetBooleanAttributes(BoxModuleI boxModule, ITask taskFunctions)
        {
            string[] socketNames = taskFunctions.GetBooleanAttributesSocketNames();
            if (socketNames == null || socketNames.Length == 0)
                return new BooleanAttribute[0];

            List<BooleanAttribute> result = new List<BooleanAttribute>();
            BooleanAttribute item;
            foreach (string s in socketNames)
            {
                if (String.IsNullOrEmpty(s))
                    continue;
                item = GetBooleanAttribute(
                    boxModule,
                    socketName2MarkEnum(s),
                    s,
                    taskFunctions.IsRequiredOneAtMinimumAttributeInSocket(s)
                    );
                if (item != null)
                    result.Add(item);
            }
            return result.ToArray();
        }

        public static CategorialAttribute[] GetCategorialAttributes(BoxModuleI boxModule, ITask taskFunctions)
        {
            string[] socketNames = taskFunctions.GetCategorialAttributesSocketNames();
            if (socketNames == null || socketNames.Length == 0)
                return new CategorialAttribute[0];

            List<CategorialAttribute> result = new List<CategorialAttribute>();
            foreach (string s in socketNames)
            {
                if (String.IsNullOrEmpty(s))
                    continue;

                List<CategorialAttribute> items = GetCategorialAttributes(
                    boxModule,
                    socketName2MarkEnum(s),
                    s,
                    taskFunctions.IsRequiredOneAtMinimumAttributeInSocket(s),
                    true
                    );
                if (items != null && items.Count > 0)
                    result.AddRange(items);
            }
            return result.ToArray();
        }

        #endregion

        #region Quantifiers

        public const string SockQuantifiers = "Quantifiers";

        public static List<QuantifierBaseFunctionsPrx> GetQuantifierBaseFunctions(BoxModuleI boxModule, bool fallOnError)
        {
            return SocketConnections.GetPrxs<QuantifierBaseFunctionsPrx>(
                boxModule,
                SockQuantifiers,
                QuantifierBaseFunctionsPrxHelper.checkedCast,
                true,
                fallOnError);
        }

        #endregion

        #region Result Info Properties

        public const string PropTotalNumberOfRelevantQuestions = "TotalNumberOfRelevantQuestions";
        public const string PropNumberOfVerifications = "NumberOfVerifications";
        public const string PropNumberOfHypotheses = "NumberOfHypotheses";
        public const string PropStartTime = "StartTime";
        public const string PropEndTime = "EndTime";

        public static double TotalNumberOfRelevantQuestions(ITask task)
        {
            SerializableResultInfo rInfo = task.GetResultInfo();
            if (rInfo == null)
                return 0;
            else
                return rInfo.TotalNumberOfRelevantQuestions;
        }

        public static long NumberOfVerifications(ITask task)
        {
            SerializableResultInfo rInfo = task.GetResultInfo();
            if (rInfo == null)
                return 0;
            else
                return rInfo.NumberOfVerifications;
        }

        public static long NumberOfHypotheses(ITask task)
        {
            SerializableResultInfo rInfo = task.GetResultInfo();
            if (rInfo == null)
                return 0;
            else
                return rInfo.NumberOfHypotheses;
        }

        public static DateTime StartTime(ITask task)
        {
            SerializableResultInfo rInfo = task.GetResultInfo();
            if (rInfo == null)
                return new DateTime();
            else
                return rInfo.StartTime;
        }

        public static DateTime EndTime(ITask task)
        {
            SerializableResultInfo rInfo = task.GetResultInfo();
            if (rInfo == null)
                return new DateTime();
            else
                return rInfo.EndTime;
        }

        #endregion

        #region Serialized result

        public const string PropResult = "Result";
        public const string PropResultInfo = "ResultInfo";

        public static SerializableResultInfo GetResultInfoDeserealized(BoxModuleI boxModule)
        {
            string serializedRI = boxModule.GetPropertyString(PropResultInfo);
            if (String.IsNullOrEmpty(serializedRI))
                return null;
            else
                return SerializableResultInfo.Deserialize(serializedRI);
        }

        public static string GetResultInfo(BoxModuleI boxModule)
        {
            return boxModule.GetPropertyString(PropResultInfo);
        }

        public static string GetResult(BoxModuleI boxModule)
        {
            return boxModule.GetPropertyString(PropResult);
        }

        public static void SetResultInfo(BoxModuleI boxModule, string value)
        {
            boxModule.setProperty(PropResultInfo, new StringTI(value));
        }

        public static void SetResult(BoxModuleI boxModule, string value)
        {
            boxModule.setProperty(PropResult, new StringTI(value));
        }

        #endregion

        #region Working with boolean/categorial attributes sockets

        public static string GetSourceDataTableId(BoxModuleI boxModule, ITask taskFunctions)
        {
            BooleanAttributeSettingFunctionsPrx prx;
            string last = null;
            string newer;

            string[] socketsNames;

            // boolean attributes
            socketsNames = taskFunctions.GetBooleanAttributesSocketNames();
            if (socketsNames != null && socketsNames.Length > 0)
                foreach (string s in socketsNames)
                {
                    if (String.IsNullOrEmpty(s))
                        continue;

                    prx = GetBooleanAttributePrx(boxModule, s, false);

                    if (prx == null)
                        continue;

                    newer = prx.GetSourceDataTableId();
                    if (String.IsNullOrEmpty(last))
                        last = newer;
                    else if (last != newer)
                        throw Exceptions.BadValueError(null, boxModule.StringIceIdentity,
                                                       "Mining over only source data table is supported.",
                                                       new string[] {s},
                                                       restrictionTypeEnum.OtherReason);
                }

            // categorial attributes
            socketsNames = taskFunctions.GetCategorialAttributesSocketNames();
            if (socketsNames != null && socketsNames.Length > 0)
                foreach (string s in socketsNames)
                {
                    if (String.IsNullOrEmpty(s))
                        continue;

                    List<BitStringGeneratorPrx> prxs = GetCategorialAttributePrxs(boxModule, s, false, false);
                    if (prxs != null && prxs.Count > 0)
                        foreach (BitStringGeneratorPrx generatorPrx in prxs)
                        {
                            if (generatorPrx == null)
                                continue;

                            newer = generatorPrx.GetSourceDataTableId();
                            if (String.IsNullOrEmpty(last))
                                last = newer;
                            else if (last != newer)
                                throw Exceptions.BadValueError(null, boxModule.StringIceIdentity,
                                                               "Mining over only source data table is supported.",
                                                               new string[] {s},
                                                               restrictionTypeEnum.OtherReason);
                        }
                }
            return last;
        }

        public static BitStringGeneratorPrx GetBitStringGenerator(BoxModuleI boxModule, GuidStruct attributeId,
                                                                  ITask taskFunctions)
        {
            BitStringGeneratorPrx result;
            BooleanAttributeSettingFunctionsPrx prx;

            string[] socketsNames;

            // boolean attributes
            socketsNames = taskFunctions.GetBooleanAttributesSocketNames();
            if (socketsNames != null && socketsNames.Length > 0)
                foreach (string s in socketsNames)
                {
                    if (String.IsNullOrEmpty(s))
                        continue;

                    prx = GetBooleanAttributePrx(boxModule, s, false);

                    if (prx == null)
                        continue;

                    result = prx.GetBitStringGenerator(attributeId);
                    if (result != null)
                        return result;
                }

            // categorial attributes
            socketsNames = taskFunctions.GetCategorialAttributesSocketNames();
            if (socketsNames != null && socketsNames.Length > 0)
                foreach (string s in socketsNames)
                {
                    if (String.IsNullOrEmpty(s))
                        continue;

                    List<BitStringGeneratorPrx> prxs = GetCategorialAttributePrxs(boxModule, s, false, false);
                    if (prxs != null && prxs.Count > 0)
                        foreach (BitStringGeneratorPrx generatorPrx in prxs)
                        {
                            if (generatorPrx == null)
                                continue;

                            if (generatorPrx.GetAttributeId() == attributeId)
                                return generatorPrx;
                        }
                }
            return null;
        }

        public static GuidAttributeNamePair[] GetAttributeNames(BoxModuleI boxModule, ITask taskFunctions)
        {
            List<GuidAttributeNamePair> result = new List<GuidAttributeNamePair>();
            string[] socketsNames;

            // boolean attributes
            socketsNames = taskFunctions.GetBooleanAttributesSocketNames();
            foreach (string s in socketsNames)
            {
                if (String.IsNullOrEmpty(s))
                    continue;

                AttributeNameProviderPrx prx = SocketConnections.GetPrx<AttributeNameProviderPrx>(
                    boxModule,
                    s,
                    AttributeNameProviderPrxHelper.checkedCast,
                    false);
                if (prx != null)
                    result.AddRange(prx.GetAttributeNames());
            }
            // categorial attributes
            socketsNames = taskFunctions.GetCategorialAttributesSocketNames();
            if (socketsNames != null && socketsNames.Length > 0)
                foreach (string s in socketsNames)
                {
                    if (String.IsNullOrEmpty(s))
                        continue;

                    List<BitStringGeneratorPrx> prxs = GetCategorialAttributePrxs(boxModule, s, false, false);
                    if (prxs != null && prxs.Count > 0)
                        foreach (BitStringGeneratorPrx generatorPrx in prxs)
                        {
                            if (generatorPrx == null)
                                continue;

                            result.AddRange(generatorPrx.GetAttributeNames());
                        }
                }
            return result.ToArray();
        }

        #endregion

        public static MiningProcessorFunctionsPrx GetMiningProcessorFunctionsPrx(BoxModuleI boxModule)
        {
            //UNDONE Load Balancing
            return MiningProcessorFunctionsPrxHelper.checkedCast(
                boxModule.Manager.getManagersLocator().findAllObjectsWithType(
                    "::Ferda::Guha::MiningProcessor::MiningProcessorFunctions"
                    )[0]
                );
        }

        public static BitStringGeneratorProviderPrx GetBitStringGeneratorProviderPrx(BoxModuleI boxModule)
        {
            return BitStringGeneratorProviderPrxHelper.checkedCast(boxModule.getFunctions());
        }

        private static bool isSDTaskType(TaskTypeEnum taskType)
        {
            switch (taskType)
            {
                case TaskTypeEnum.CF:
                case TaskTypeEnum.FourFold:
                case TaskTypeEnum.KL:
                    return false;
                case TaskTypeEnum.SDCF:
                case TaskTypeEnum.SDFourFold:
                case TaskTypeEnum.SDKL:
                    return true;
                default:
                    throw new NotImplementedException();
            }
        }

        public static void RunTask(BoxModuleI boxModule, ITask taskFunctions, TaskTypeEnum taskType)
        {
            //validate
            //boxModule.Manager.getBoxModuleValidator().validate(boxModule.StringIceIdentity);
            
            MiningProcessorFunctionsPrx miningProcessor = GetMiningProcessorFunctionsPrx(boxModule);
            BitStringGeneratorProviderPrx bsProvider = GetBitStringGeneratorProviderPrx(boxModule);

            List<QuantifierBaseFunctionsPrx> quantifiers = GetQuantifierBaseFunctions(boxModule, true);
            if (quantifiers == null || quantifiers.Count == 0)
                throw Exceptions.BadValueError(null, boxModule.StringIceIdentity,
                                               "There must be connected at least one quantifier to task box module.",
                                               new string[] {SockQuantifiers}, restrictionTypeEnum.OtherReason);
            
            // UNDONE in this version is only operation mode difference of quatifier values supported for SD tasks            
            if (isSDTaskType(taskType))
            foreach (QuantifierBaseFunctionsPrx prx in quantifiers)
            {
                if (prx.GetQuantifierSetting().operationMode != OperationModeEnum.DifferenceOfQuantifierValues)
                    throw Exceptions.BadValueError(null, boxModule.StringIceIdentity,
                               "Only \"DifferenceOfQuantifierValues\" Operation Mode is supported in quantifiers for SD tasks.",
                               new string[] { SockQuantifiers }, restrictionTypeEnum.OtherReason);
            }
            
            
            WorkingWithSecondSetModeEnum secondSetWorking =
                isSDTaskType(taskType)
                    ?
                WorkingWithSecondSetMode(boxModule)
                    :
                WorkingWithSecondSetModeEnum.None;

            TaskRunParams taskRunParams = new TaskRunParams(
                taskType,
                ExecutionType(boxModule),
                MaxNumberOfHypotheses(boxModule),
                secondSetWorking
                );

            SetResult(boxModule, null);
            SetResultInfo(boxModule, null);

            string statistics;
            string result =
                miningProcessor.Run(
                    boxModule.MyProxy,
                    GetBooleanAttributes(boxModule, taskFunctions),
                    GetCategorialAttributes(boxModule, taskFunctions),
                    GetQuantifierBaseFunctions(boxModule, true).ToArray(),
                    taskRunParams,
                    bsProvider,
                    boxModule.Output,
                    out statistics
                    );
            SerializableResultInfo deserealized = SerializableResultInfo.Deserialize(statistics);
            boxModule.Output.writeMsg(MsgType.Info, "Peformance info", deserealized.OtherInfo);
            SetResult(boxModule, result);
            SetResultInfo(boxModule, statistics);
        }
    }
}