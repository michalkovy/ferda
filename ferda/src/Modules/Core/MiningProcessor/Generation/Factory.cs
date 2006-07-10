using System;

namespace Ferda.Guha.MiningProcessor.Generation
{
    public static class Factory
    {
        public static IEntityEnumerator Create(IEntitySetting setting)
        {
            if (setting.ice_isA("::Ferda::Guha::MiningProcessor::CoefficientFixedSetSetting"))
                return new FixedSet((CoefficientFixedSetSettingI) setting);
            else if (setting.ice_isA("::Ferda::Guha::MiningProcessor::CoefficientSetting"))
            {
                CoefficientSettingI coefSetting = (CoefficientSettingI) setting;
                switch (coefSetting.coefficientType)
                {
                    case CoefficientTypeEnum.Subsets:
                        return new Subsets(coefSetting);
                    case CoefficientTypeEnum.CyclicIntervals:
                        return new CyclicIntervals(coefSetting);
                    case CoefficientTypeEnum.Intervals:
                        return new Intervals(coefSetting);
                    case CoefficientTypeEnum.Cuts:
                        return new Cuts(coefSetting);
                    case CoefficientTypeEnum.LeftCuts:
                        return new LeftCuts(coefSetting);
                    case CoefficientTypeEnum.RightCuts:
                        return new RightCuts(coefSetting);
                }
            }
            else if (setting.ice_isA("::Ferda::Guha::MiningProcessor::NegationSetting"))
            {
                return new Negation((NegationSettingI) setting);
            }
            else if (setting.ice_isA("::Ferda::Guha::MiningProcessor::BothSignsSetting"))
            {
                return new BothSigns((BothSignsSettingI) setting);
            }
            else if (setting.ice_isA("::Ferda::Guha::MiningProcessor::DisjunctionSetting"))
            {
                return new Disjunction((DisjunctionSettingI) setting);
            }
            else if (setting.ice_isA("::Ferda::Guha::MiningProcessor::ConjunctionSetting"))
            {
                return new Conjunction((ConjunctionSettingI) setting);
            }
            throw new NotImplementedException();
        }
    }
}