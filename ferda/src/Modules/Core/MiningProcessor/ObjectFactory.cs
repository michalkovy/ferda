using System.Diagnostics;
using Ferda.Modules;
using Ice;

namespace Ferda.Guha.MiningProcessor
{

    #region Ice object implementations

    public class BitStringIceI : BitStringIce
    {
    }

    public class IEntitySettingI : IEntitySetting
    {
    }

    public class ILeafEntitySettingI : ILeafEntitySetting
    {
    }

    public class CoefficientFixedSetSettingI : CoefficientFixedSetSetting
    {
        public CoefficientFixedSetSettingI(GuidStruct id, ImportanceEnum importance, BitStringGeneratorPrx generator,
                                           string[] categoriesIds)
            : base(id, importance, generator, categoriesIds)
        {
        }

        public CoefficientFixedSetSettingI()
            : base()
        {
        }
    }

    public class CoefficientSettingI : CoefficientSetting
    {
        public CoefficientSettingI(GuidStruct id, ImportanceEnum importance, BitStringGeneratorPrx generator,
                                   int minLength, int maxLength, CoefficientTypeEnum coefficientType)
            : base(id, importance, generator, minLength, maxLength, coefficientType)
        {
        }

        public CoefficientSettingI()
            : base()
        {
        }
    }

    public class ISingleOperandEntitySettingI : ISingleOperandEntitySetting
    {
    }

    public class NegationSettingI : NegationSetting
    {
        public NegationSettingI(GuidStruct id, ImportanceEnum importance, IEntitySetting operand)
            : base(id, importance, operand)
        {
        }

        public NegationSettingI()
            : base()
        {
        }
    }

    public class BothSignsSettingI : BothSignsSetting
    {
        public BothSignsSettingI(GuidStruct id, ImportanceEnum importance, IEntitySetting operand)
            : base(id, importance, operand)
        {
        }

        public BothSignsSettingI()
            : base()
        {
        }
    }

    public class IMultipleOperandEntitySettingI : IMultipleOperandEntitySetting
    {
    }

    public class ConjunctionSettingI : ConjunctionSetting
    {
        public ConjunctionSettingI(GuidStruct id, ImportanceEnum importance, IEntitySetting[] operands,
                                   GuidStruct[][] classesOfEquivalence, int minLength, int maxLength)
            : base(id, importance, operands, classesOfEquivalence, minLength, maxLength)
        {
        }

        public ConjunctionSettingI()
            : base()
        {
        }
    }


    public class DisjunctionSettingI : DisjunctionSetting
    {
        public DisjunctionSettingI(GuidStruct id, ImportanceEnum importance, IEntitySetting[] operands,
                                   GuidStruct[][] classesOfEquivalence, int minLength, int maxLength)
            : base(id, importance, operands, classesOfEquivalence, minLength, maxLength)
        {
        }

        public DisjunctionSettingI()
            : base()
        {
        }
    }

    #endregion

    public class ObjectFactory : LocalObjectImpl, Ice.ObjectFactory
    {
        #region ObjectFactoryOperationsNC_ Members

        public Object create(string type)
        {
            switch (type)
            {
                case "::Ferda::Guha::MiningProcessor::BitStringIce":
                    return new BitStringIceI();
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
            Debug.Assert(false);
            return null;
        }

        public void destroy()
        {
            return;
        }

        #endregion

        public static void addFactoryToCommunicator(Communicator communicator,
                                                    ObjectFactory factory)
        {
            lock (communicator)
            {
                string[] types = new string[]
                    {
                        "::Ferda::Guha::MiningProcessor::BitStringIce",
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