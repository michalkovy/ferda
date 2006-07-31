using System;

namespace Ferda.Guha.MiningProcessor.Generation
{
    public static class Factory
    {
        public static IEntityEnumerator Create(IEntitySetting setting, ISkipOptimalization skipOptimalization, MarkEnum cedentType)
        {
            if (setting.ice_isA("::Ferda::Guha::MiningProcessor::CoefficientFixedSetSetting"))
                return new FixedSet((CoefficientFixedSetSettingI) setting, skipOptimalization, cedentType);
            else if (setting.ice_isA("::Ferda::Guha::MiningProcessor::CoefficientSetting"))
            {
                CoefficientSettingI coefSetting = (CoefficientSettingI) setting;
                switch (coefSetting.coefficientType)
                {
                    case CoefficientTypeEnum.Subsets:
                        return new Subsets(coefSetting, skipOptimalization, cedentType);
                    case CoefficientTypeEnum.CyclicIntervals:
                        return new CyclicIntervals(coefSetting, skipOptimalization, cedentType);
                    case CoefficientTypeEnum.Intervals:
                        return new Intervals(coefSetting, skipOptimalization, cedentType);
                    case CoefficientTypeEnum.Cuts:
                        return new Cuts(coefSetting, skipOptimalization, cedentType);
                    case CoefficientTypeEnum.LeftCuts:
                        return new LeftCuts(coefSetting, skipOptimalization, cedentType);
                    case CoefficientTypeEnum.RightCuts:
                        return new RightCuts(coefSetting, skipOptimalization, cedentType);
                }
            }
            else if (setting.ice_isA("::Ferda::Guha::MiningProcessor::NegationSetting"))
            {
                return new Negation((NegationSettingI)setting, skipOptimalization, cedentType);
            }
            else if (setting.ice_isA("::Ferda::Guha::MiningProcessor::BothSignsSetting"))
            {
                return new BothSigns((BothSignsSettingI)setting, skipOptimalization, cedentType);
            }
            else if (setting.ice_isA("::Ferda::Guha::MiningProcessor::DisjunctionSetting"))
            {
                return new Disjunction((DisjunctionSettingI)setting, skipOptimalization, cedentType);
            }
            else if (setting.ice_isA("::Ferda::Guha::MiningProcessor::ConjunctionSetting"))
            {
                return new Conjunction((ConjunctionSettingI)setting, skipOptimalization, cedentType);
            }
            throw new NotImplementedException();
        }
    }
}