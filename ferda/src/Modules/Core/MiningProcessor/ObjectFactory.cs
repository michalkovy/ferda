using System;
using System.Collections.Generic;
using System.Text;
using Ferda.Modules;

namespace Ferda.Guha.MiningProcessor
{
    #region Ice object implementations
    class BitStringI : BitString { }

    class IEntitySettingI : IEntitySetting { }

    class ILeafEntitySettingI : ILeafEntitySetting { }

    class CoefficientFixedSetSettingI : CoefficientFixedSetSetting { }

    class CoefficientSettingI : CoefficientSetting { }

    class ISingleOperandEntitySettingI : ISingleOperandEntitySetting { }

    class NegationSettingI : NegationSetting { }

    class BothSignsSettingI : BothSignsSetting { }

    class IMultipleOperandEntitySettingI : IMultipleOperandEntitySetting { }

    class ConjunctionSettingI : ConjunctionSetting { }

    class DisjunctionSettingI : DisjunctionSetting { }
    #endregion
    
    public class ObjectFactoryForPropertyTypes : Ice.LocalObjectImpl, Ice.ObjectFactory
    {
        #region ObjectFactoryOperationsNC_ Members

        public Ice.Object create(string type)
        {
            switch (type)
            {
                case "::Ferda::Guha::MiningProcessor::BitString":
                    return new BitStringI();
                case "::Ferda::Guha::MiningProcessor::IEntitySetting":
                    return new IEntitySettingI();
                case "::Ferda::Guha::MiningProcessor::ILeafEntitySetting":
                    return new ILeafEntitySettingI();
                case "::Ferda::Guha::MiningProcessor::CoefficientFixedSetSetting":
                    return new CoefficientFixedSetSettingI();
                case "::Ferda::Guha::MiningProcessor::CoefficientSetting":
                    return new CoefficientSettingI();
                case "::Ferda::Guha::MiningProcessor::ISingleOperandEntitySetting":
                    return new ISingleOperandEntitySettingI();
                case "::Ferda::Guha::MiningProcessor::NegationSetting":
                    return new NegationSettingI();
                case "::Ferda::Guha::MiningProcessor::BothSignsSetting":
                    return new BothSignsSettingI();
                case "::Ferda::Guha::MiningProcessor::IMultipleOperandEntitySetting":
                    return new IMultipleOperandEntitySettingI();
                case "::Ferda::Guha::MiningProcessor::ConjunctionSetting":
                    return new ConjunctionSettingI();
                case "::Ferda::Guha::MiningProcessor::DisjunctionSetting":
                    return new DisjunctionSettingI();
            }
            System.Diagnostics.Debug.Assert(false);
            return null;
        }

        public void destroy()
        {
            return;
        }

        #endregion

        public static void addFactoryToCommunicator(Ice.Communicator communicator,
        ObjectFactoryForPropertyTypes factory)
        {
            lock (communicator)
            {
                string[] types = new string[]
                    {
                        "::Ferda::Guha::MiningProcessor::BitString",
                        "::Ferda::Guha::MiningProcessor::IEntitySetting",
                        "::Ferda::Guha::MiningProcessor::ILeafEntitySetting",
                        "::Ferda::Guha::MiningProcessor::CoefficientFixedSetSetting",
                        "::Ferda::Guha::MiningProcessor::CoefficientSetting",
                        "::Ferda::Guha::MiningProcessor::ISingleOperandEntitySetting",
                        "::Ferda::Guha::MiningProcessor::NegationSetting",
                        "::Ferda::Guha::MiningProcessor::BothSignsSetting",
                        "::Ferda::Guha::MiningProcessor::IMultipleOperandEntitySetting",
                        "::Ferda::Guha::MiningProcessor::ConjunctionSetting",
                        "::Ferda::Guha::MiningProcessor::DisjunctionSetting"
                    };
                foreach (string s in types)
                {
                    if (communicator.findObjectFactory(s) == null)
                        communicator.addObjectFactory(factory, s);                    
                }
            }
        }
    }
}
