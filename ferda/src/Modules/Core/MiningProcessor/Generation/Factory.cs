using System;

namespace Ferda.Guha.MiningProcessor.Generation
{
    public static class Factory
    {
        public static IEntityEnumerator Create(IEntitySetting setting)
        {
            Type settingType = setting.GetType();
            if (settingType.IsSubclassOf(typeof (ILeafEntitySetting)))
            {
                if (settingType == typeof (CoefficientFixedSetSetting))
                    return new FixedSet((CoefficientFixedSetSetting) setting);
                if (settingType == typeof (CoefficientSetting))
                {
                    switch (((CoefficientSetting) setting).coefficientType)
                    {
                        case CoefficientTypeEnum.Subsets:
                            return new Subsets((CoefficientSetting) setting);
                        case CoefficientTypeEnum.CyclicIntervals:
                            return new CyclicIntervals((CoefficientSetting) setting);
                        case CoefficientTypeEnum.Intervals:
                            return new Intervals((CoefficientSetting) setting);
                        case CoefficientTypeEnum.Cuts:
                            return new Cuts((CoefficientSetting) setting);
                        case CoefficientTypeEnum.LeftCuts:
                            return new LeftCuts((CoefficientSetting) setting);
                        case CoefficientTypeEnum.RightCuts:
                            return new RightCuts((CoefficientSetting) setting);
                    }
                }
            }
            else if (settingType.IsSubclassOf(typeof (ISingleOperandEntitySetting)))
            {
                if (settingType == typeof (NegationSetting))
                    return new Negation((NegationSetting)setting);
                else if (settingType == typeof (BothSignsSetting))
                    return new BothSigns((BothSignsSetting)setting);
            }
            else if (settingType.IsSubclassOf(typeof (IMultipleOperandEntitySetting)))
            {
                if (settingType == typeof (DisjunctionSetting))
                    return new Disjunction((DisjunctionSetting) setting);
                else if (settingType == typeof (ConjunctionSetting))
                    return new Conjunction((IMultipleOperandEntitySetting) setting);
            }
            throw new NotImplementedException();
        }
    }
}