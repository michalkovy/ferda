// ObjectFactory.cs - Defines exceptions for the MiningProcessor
//
// Author: Tomáš Kuchaø <tomas.kuchar@gmail.com>
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

using System.Diagnostics;
using Ferda.Modules;
using Ice;

namespace Ferda.Guha.MiningProcessor
{
    ///These classes implement structures from the ICE design as defined
    ///in <c>slice.Modules.Guha.MiningProcessor</c>.
    ///Some of them contain also default constructor.
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
                                   int minLength, int maxLength)
            : base(id, importance, operands, minLength, maxLength)
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
                                   int minLength, int maxLength)
            : base(id, importance, operands, minLength, maxLength)
        {
        }

        public DisjunctionSettingI()
            : base()
        {
        }
    }

    #endregion

    /// <summary>
    /// The mining processor object factory. This factory is used for
    /// creation of mining processor objects.
    /// </summary>
    public class ObjectFactory : LocalObjectImpl, Ice.ObjectFactory
    {
        #region ObjectFactoryOperationsNC_ Members

        /// <summary>
        /// Creates an object according to its type defined by 
        /// string in the parameter.
        /// </summary>
        /// <param name="type">Type of the object</param>
        /// <returns>Object of a given type</returns>
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

        /// <summary>
        /// Adds this object factory to the communicator. For further details,
        /// see the ICE documentation. 
        /// </summary>
        /// <param name="communicator">Communicator</param>
        /// <param name="factory">Factory to be added to a communicator</param>
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