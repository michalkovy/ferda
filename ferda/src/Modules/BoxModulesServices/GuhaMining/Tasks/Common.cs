using System;
using System.Collections.Generic;
using System.Text;
using Ferda.Guha.Math.Quantifiers;
using Ferda.Guha.MiningProcessor;

namespace Ferda.Modules.Boxes.GuhaMining.Tasks
{
    public interface ITask
    {
        SerializableResultInfo GetResultInfo();
    }

    public static class Common
    {
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

        public static BitStringGeneratorPrx GetBitStringGenerator(BoxModuleI boxModule, GuidStruct attributeId, string[] socketsNames)
        {
            BitStringGeneratorPrx result;
            BooleanAttributeSettingFunctionsPrx prx;

            foreach (string s in socketsNames)
            {
                if (String.IsNullOrEmpty(s))
                    continue;

                prx = GetBooleanAttributeSettingFunctionsPrx(boxModule, s, false);

                if (prx == null)
                    continue;

                result = prx.GetBitStringGenerator(attributeId);
                if (result != null)
                    return result;
            }
            return null;
        }


        public const string PropMaxNumberOfHypotheses = "MaxNumberOfHypotheses";
        public const string PropExecutionType = "ExecutionType";

        public static long MaxNumberOfHypotheses(BoxModuleI boxModule)
        {
            return boxModule.GetPropertyLong(PropMaxNumberOfHypotheses);
        }
        
        public static TaskEvaluationTypeEnum ExecutionType(BoxModuleI boxModule)
        {
            return (TaskEvaluationTypeEnum)Enum.Parse(
                        typeof(TaskEvaluationTypeEnum),
                        boxModule.GetPropertyString(PropExecutionType)
                    );
        }

        #region Boolean and Categorial attributes

        public const string SockAntecedent = "Antecedent";
        public const string SockSuccedent = "Succedent";
        public const string SockCondition = "Condition";
        public const string SockRowAttribute = "RowAttribute";
        public const string SockColumnAttribute = "ColumnAttribute";
        public const string SockAttribute = "Attribute";
        public const string SockFirstSet = "FirstSet";
        public const string SockSecondSet = "SecondSet";

        public static BooleanAttributeSettingFunctionsPrx GetBooleanAttributeSettingFunctionsPrx(BoxModuleI boxModule, string sockName, bool fallOnError)
        {
            return SocketConnections.GetPrx<BooleanAttributeSettingFunctionsPrx>(
                boxModule,
                sockName,
                BooleanAttributeSettingFunctionsPrxHelper.checkedCast,
                fallOnError);
        }

        public static BooleanAttribute GetBooleanAttribute(BoxModuleI boxModule, MarkEnum semantic, string sockName, bool fallOnError)
        {
            BooleanAttributeSettingFunctionsPrx prx = GetBooleanAttributeSettingFunctionsPrx(boxModule, sockName, fallOnError);
            if (prx != null)
            {
                return new BooleanAttribute(semantic, prx.GetEntitySetting());
            }
            return null;
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
                return SerializableResultInfo.DeSerialize(serializedRI);
        }

        public static string GetResultInfo(BoxModuleI boxModule)
        {
            return boxModule.GetPropertyString(PropResultInfo);
        }

        public static string GetResult(BoxModuleI boxModule)
        {
            return boxModule.GetPropertyString(PropResult);
        }

        #endregion
    }
}
