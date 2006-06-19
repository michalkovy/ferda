using System.Collections.Generic;
using Ferda.Guha.MiningProcessor;
using Ice;

namespace Ferda.Modules.Boxes.GuhaMining.ClassOfEquivalence
{
    internal class Functions : EquivalenceClassFunctionsDisp_, IFunctions
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

        public const string SockBooleanAttributeSetting = "BooleanAttributeSetting";

        #endregion

        #region Methods

        public List<BooleanAttributeSettingFunctionsPrx> GetBooleanAttributeSettingFunctionsPrxs(bool fallOnError)
        {
            return SocketConnections.GetPrxs<BooleanAttributeSettingFunctionsPrx>(
                _boxModule,
                SockBooleanAttributeSetting,
                BooleanAttributeSettingFunctionsPrxHelper.checkedCast,
                false,
                fallOnError);
        }

        #endregion

        #region Ice Functions

        public override GuidStruct[] GetEquivalenceClass(Current current__)
        {
            List<BooleanAttributeSettingFunctionsPrx> booleanAttributes = GetBooleanAttributeSettingFunctionsPrxs(true);
            List<GuidStruct> result = new List<GuidStruct>();
            foreach (BooleanAttributeSettingFunctionsPrx prx in booleanAttributes)
            {
                result.Add(prx.GetEntitySetting().id);
            }
            return result.ToArray();
        }

        #endregion
    }
}