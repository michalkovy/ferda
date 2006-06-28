using System;
using System.Collections.Generic;
using System.Text;
using Ferda.Guha.MiningProcessor;
using Ferda.Guha.Math.Quantifiers;

namespace Ferda.Modules.Boxes.GuhaMining.Tasks.FourFold
{
    public class Functions : MiningTaskFunctionsDisp_, IFunctions
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

        public const string SockAntecedent = "Antecedent";
        public const string SockSuccedent = "Succedent";
        public const string SockCondition = "Condition";
        public const string SockQuantifiers = "Quantifiers";

        public const string PropDone = "Done";
        public const string PropNumberOfHypotheses = "NumberOfHypotheses";

        public DateTimeTI Done
        {
            get
            {
                //TODO
                return new DateTimeTI(DateTime.MinValue);
            }
        }

        public IntTI NumberOfHypotheses
        {
            get
            {
                //TODO
                return 0;
            }
        }

        #endregion


        public BooleanAttributeSettingFunctionsPrx GetBooleanAttributeSettingFunctionsPrx(string sockName, bool fallOnError)
        {
            return SocketConnections.GetPrx<BooleanAttributeSettingFunctionsPrx>(
                _boxModule,
                sockName,
                BooleanAttributeSettingFunctionsPrxHelper.checkedCast,
                fallOnError);
        }
        public List<QuantifierBaseFunctionsPrx> GetQuantifierBaseFunctions(bool fallOnError)
        {
            return SocketConnections.GetPrxs<QuantifierBaseFunctionsPrx>(
                _boxModule,
                SockQuantifiers,
                QuantifierBaseFunctionsPrxHelper.checkedCast,
                true,
                fallOnError);
        }

        public static BitStringGeneratorPrx getBitStringGenerator(GuidStruct attributeId, BooleanAttributeSettingFunctionsPrx prx)
        {
            if (prx != null)
                return prx.GetBitStringGenerator(attributeId);
            return (null);
        }
        
        public override BitStringGeneratorPrx GetBitStringGenerator(GuidStruct attributeId, Ice.Current current__)
        {
            BitStringGeneratorPrx result;
            BooleanAttributeSettingFunctionsPrx prx;
            
            prx = GetBooleanAttributeSettingFunctionsPrx(SockSuccedent, true);
            result = getBitStringGenerator(attributeId, prx);
            if (result != null)
                return result;
            
            prx = GetBooleanAttributeSettingFunctionsPrx(SockAntecedent, false);
            result = getBitStringGenerator(attributeId, prx);
            if (result != null)
                return result;
            
            prx = GetBooleanAttributeSettingFunctionsPrx(SockCondition, false);
            result = getBitStringGenerator(attributeId, prx);
            if (result != null)
                return result;
            
            return null;
        }


        public void Run()
        {
            List<BooleanAttribute> cedents = new List<BooleanAttribute>();

            BooleanAttributeSettingFunctionsPrx prx;

            prx = GetBooleanAttributeSettingFunctionsPrx(SockSuccedent, true);
            cedents.Add(new BooleanAttribute(MarkEnum.Succedent, prx.GetEntitySetting()));

            prx = GetBooleanAttributeSettingFunctionsPrx(SockAntecedent, false);
            if (prx != null)
                cedents.Add(new BooleanAttribute(MarkEnum.Antecedent, prx.GetEntitySetting()));

            prx = GetBooleanAttributeSettingFunctionsPrx(SockCondition, false);
            if (prx != null)
                cedents.Add(new BooleanAttribute(MarkEnum.Condition, prx.GetEntitySetting()));

            //UNDONE Load Balancing
            MiningProcessorFunctionsPrx miningProcessor = 
                MiningProcessorFunctionsPrxHelper.checkedCast(
                    _boxModule.Manager.getManagersLocator().findAllObjectsWithType(
                        "::Ferda::Guha::MiningProcessor::MiningProcessorFunctions"
                        )[0]
                    );

            BitStringGeneratorProviderPrx bsProvider = BitStringGeneratorProviderPrxHelper.checkedCast(_boxModule.getFunctions());

            string statistics;
            string result =
                miningProcessor.Run(
                        cedents.ToArray(),
                        new CategorialAttribute[0],
                        GetQuantifierBaseFunctions(true).ToArray(),
                        TaskTypeEnum.FourFold,
                        bsProvider,
                        _boxModule.Output,
                        out statistics
                    );
        }

        public override QuantifierBaseFunctionsPrx[] GetQuantifiers(Ice.Current current__)
        {
            return GetQuantifierBaseFunctions(true).ToArray();
        }

        public override string GetResult(out string statistics, Ice.Current current__)
        {
            //TODO
            throw new Exception("The method or operation is not implemented.");
        }
    }
}
