#define ProgressBarDebug

using Ferda.Guha.Math.Quantifiers;
using Ferda.Guha.MiningProcessor;
using Ferda.Guha.MiningProcessor.Results;
using Ice;

namespace Ferda.Modules.Boxes.GuhaMining.Tasks.SDTwoDimensional
{
    public class Functions : MiningTaskFunctionsDisp_, IFunctions, ITask
    {
        #region Invariant code ... same for all tasks

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

        #region ITask Members

        private SerializableResultInfo _cachedSerializableResultInfo = null;

        public SerializableResultInfo GetResultInfo()
        {
            if (_cachedSerializableResultInfo == null)
                _cachedSerializableResultInfo = Common.GetResultInfoDeserealized(_boxModule);
            return _cachedSerializableResultInfo;
        }

        #endregion

        public override BitStringGeneratorPrx GetBitStringGenerator(GuidStruct attributeId, Current current__)
        {
            return Common.GetBitStringGenerator(_boxModule, attributeId, this);
        }

        public override QuantifierBaseFunctionsPrx[] GetQuantifiers(Current current__)
        {
            return Common.GetQuantifierBaseFunctions(_boxModule, true).ToArray();
        }

        public override string GetResult(out string statistics, Current current__)
        {
            statistics = Common.GetResultInfo(_boxModule);
            return Common.GetResult(_boxModule);
        }

        public override GuidAttributeNamePair[] GetAttributeNames(Current current__)
        {
            return Common.GetAttributeNames(_boxModule, this);
        }

        public override string GetSourceDataTableId(Current current__)
        {
            return Common.GetSourceDataTableId(_boxModule, this);
        }

        #endregion

        #region ITask Members

        public string[] GetBooleanAttributesSocketNames()
        {
            return new string[]
                {
                    Common.SockCondition,
                    Common.SockSDCedent1,
                    Common.SockSDCedent2
                };
        }

        public string[] GetCategorialAttributesSocketNames()
        {
            return new string[]
                {
                    Common.SockRowAttribute,
                    Common.SockColumnAttribute
                };
        }

        public bool IsRequiredOneAtMinimumAttributeInSocket(string socketName)
        {
            if (socketName == Common.SockRowAttribute
                || socketName == Common.SockColumnAttribute
                || socketName == Common.SockSDCedent1
                || socketName == Common.SockSDCedent2)
                return true;
            return false;
        }

        #endregion

        public void Run()
        {
            // reset cache
            _cachedSerializableResultInfo = null;
            
            Common.RunTask(_boxModule, this, TaskTypeEnum.SDKL);
        }
    }
}