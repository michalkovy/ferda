// Factory.cs - creation of proper enumerators
//
// Authors: Tomáš Kuchaø <tomas.kuchar@gmail.com>      
// Commented by: Martin Ralbovský <martin.ralbovsky@gmail.com>
//
// Copyright (c) 2006 Tomáš Kuchaø
//
// This program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA

using System;

namespace Ferda.Guha.MiningProcessor.Generation
{
    /// <summary>
    /// The factory class creates proper enumerators out of the entity setting and other
    /// parameters.
    /// </summary>
    public static class Factory
    {
        /// <summary>
        /// The method creates proper entity enumerators out of the entity setting
        /// and other parameters
        /// </summary>
        /// <param name="setting">The Entity setting</param>
        /// <param name="skipOptimalization">The skip step optimalization</param>
        /// <param name="cedentType">Type of the cedent</param>
        /// <returns>Proper entity enumerator</returns>
        public static IEntityEnumerator Create(IEntitySetting setting, ISkipOptimalization skipOptimalization, MarkEnum cedentType)
        {
            if (setting is CoefficientFixedSetSettingI)
                return new FixedSet((CoefficientFixedSetSettingI) setting, skipOptimalization, cedentType);
            else if (setting is CoefficientSettingI)
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
                    case CoefficientTypeEnum.SubsetsOneOne:
                        return new SubsetsOneOne(coefSetting, skipOptimalization, cedentType);
                }
            }
            else if (setting is NegationSettingI)
            {
                return new Negation((NegationSettingI)setting, skipOptimalization, cedentType);
            }
            else if (setting is BothSignsSettingI)
            {
                return new BothSigns((BothSignsSettingI)setting, skipOptimalization, cedentType);
            }
            else if (setting is DisjunctionSettingI)
            {
                return new Disjunction((DisjunctionSettingI)setting, skipOptimalization, cedentType);
            }
            else if (setting is ConjunctionSettingI)
            {
                return new Conjunction((ConjunctionSettingI)setting, skipOptimalization, cedentType);
            }
            throw new NotImplementedException();
        }
    }
}