using System;
using System.Collections.Generic;
using System.Diagnostics;
using Ferda.Guha.Data;
using Ferda.Guha.Math.Quantifiers;
using Ferda.Guha.MiningProcessor.BitStrings;
using Ferda.Guha.MiningProcessor.Generation;
using Ferda.Guha.MiningProcessor.QuantifierEvaluator;
using Ferda.Modules;
using Ferda.Modules.Helpers.Common;

namespace Ferda.Guha.MiningProcessor.Miners
{
    public abstract class MiningProcessorBase
    {
        private long _totalCount = Int64.MinValue;

        public long TotalCount
        {
            get
            {
                if (_totalCount == Int64.MinValue)
                {
                    ICollection<IEntityEnumerator> booleanCedents;
                    ICollection<CategorialAttributeTrace[]> categorialCedents;
                    getCedents(out booleanCedents, out categorialCedents);
                    _totalCount = totalCount(booleanCedents, categorialCedents);
                }
                return _totalCount;
            }
        }

        protected readonly Quantifiers _quantifiers;

        private readonly TaskRunParams _taskParams;

        public TaskRunParams TaskParams
        {
            get { return _taskParams; }
        }


        protected MiningProcessorBase(
            QuantifierBaseFunctionsPrx[] quantifiers,
            BitStringGeneratorProviderPrx taskFuncPrx,
            TaskRunParams taskParams
            )
        {
            _taskParams = taskParams;
            _quantifiers = new Quantifiers(quantifiers, taskFuncPrx);
        }

        public static IEntitySetting GetBoolanAttributeBySemantic(MarkEnum semantic,
                                                                  BooleanAttribute[] booleanAttributes)
        {
            if (booleanAttributes == null)
                return null;
            foreach (BooleanAttribute booleanAttribute in booleanAttributes)
            {
                if (booleanAttribute.mark == semantic)
                    return booleanAttribute.setting;
            }
            return null;
        }

        public static BitStringGeneratorPrx[] GetCategorialAttributeBySemantic(MarkEnum semantic,
                                                                               CategorialAttribute[]
                                                                                   categorialAttributes)
        {
            if (categorialAttributes == null)
                return null;
            List<BitStringGeneratorPrx> result = new List<BitStringGeneratorPrx>();
            foreach (CategorialAttribute categorialAttribute in categorialAttributes)
            {
                if (categorialAttribute.mark == semantic)
                    result.Add(categorialAttribute.setting);
            }
            if (result.Count == 0)
                return null;
            else
                return result.ToArray();
        }

        public static IEntityEnumerator CreateBooleanAttributeTrace(MarkEnum semantic,
                                                                    BooleanAttribute[] booleanAttributes,
                                                                    bool allowsEmptyBitStrings)
        {
            IEntitySetting setting = GetBoolanAttributeBySemantic(semantic, booleanAttributes);
            if (setting == null)
            {
                if (!allowsEmptyBitStrings)
                    throw Exceptions.EmptyCedentIsNotAllowedError(semantic);
                else
                    return new EmptyTrace();
            }

            if (setting is IMultipleOperandEntitySetting)
            {
                IMultipleOperandEntitySetting mos = (IMultipleOperandEntitySetting) setting;
                if (mos.minLength == 0 && !allowsEmptyBitStrings)
                    throw Exceptions.EmptyCedentIsNotAllowedError(semantic);
            }

            return Factory.Create(setting);
        }

        public static CategorialAttributeTrace[] CreateCategorialAttributeTrace(MarkEnum semantic,
                                                                                CategorialAttribute[]
                                                                                    categorialAttributes,
                                                                                bool allowEmptyCategorialCedent)
        {
            BitStringGeneratorPrx[] setting = GetCategorialAttributeBySemantic(semantic, categorialAttributes);
            if (setting == null)
            {
                if (!allowEmptyCategorialCedent)
                    throw Exceptions.EmptyCedentIsNotAllowedError(semantic);
                else
                    return null;
            }
            List<CategorialAttributeTrace> result = new List<CategorialAttributeTrace>();
            foreach (BitStringGeneratorPrx prx in setting)
            {
                result.Add(new CategorialAttributeTrace(prx));
            }
            if (result.Count == 0)
            {
                if (!allowEmptyCategorialCedent)
                    throw Exceptions.EmptyCedentIsNotAllowedError(semantic);
                else
                    return null;
            }
            else
                return result.ToArray();
        }

        protected abstract void getCedents(out ICollection<IEntityEnumerator> booleanCedents,
                                           out ICollection<CategorialAttributeTrace[]> categorialCedents);

        protected static long totalCount(ICollection<IEntityEnumerator> booleanCedents,
                                         ICollection<CategorialAttributeTrace[]> categorialCedents)
        {
            unchecked
            {
                long result = 1;
                if (booleanCedents != null)
                    foreach (IEntityEnumerator cedent in booleanCedents)
                    {
                        if (cedent != null)
                            result *= cedent.TotalCount;
                    }
                if (categorialCedents != null)
                    foreach (CategorialAttributeTrace[] cedent in categorialCedents)
                    {
                        if (cedent == null || cedent.Length == 0)
                            continue;
                        else
                            result *= cedent.Length;
                    }
                if (result == 1)
                    return 0;
                return result;
            }
        }


        /// <summary>
        /// Gets the negation and missings bit strings. Please note
        /// that both negation and missings can be instances
        /// of IEmptyBitString or IFalseBitString.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="missings">The missings.</param>
        /// <param name="negation">The negation.</param>
        /// <param name="usedAttributes">The used attributes.</param>
        /// <param name="missingInformation">The missing information IBitString provider.</param>
        public static void GetNegationAndMissings(IBitString input, out IBitString missings, out IBitString negation,
                                                  Set<string> usedAttributes, MissingInformation missingInformation)
        {
            if (input is EmptyBitString)
            {
                missings = FalseBitString.GetInstance();
                negation = FalseBitString.GetInstance();
                return;
            }
            else
            {
                missings = missingInformation[usedAttributes];
                Debug.Assert(!(missings is EmptyBitString));
                if (missings is EmptyBitString)
                    throw new ArgumentException();
                if (missings is FalseBitString)
                {
                    negation = input.Not();
                }
                else
                {
                    negation = input.Or(missings).Not();    
                }
                return;
            }
        }

        /// <summary>
        /// Gets the missings bit strings. Please note
        /// that missings can be instances
        /// of IEmptyBitString or IFalseBitString.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="missings">The missings.</param>
        /// <param name="usedAttributes">The used attributes.</param>
        /// <param name="missingInformation">The missing information IBitString provider.</param>
        public static void GetMissings(IBitString input, out IBitString missings,
                                                  Set<string> usedAttributes, MissingInformation missingInformation)
        {
            if (input is EmptyBitString)
            {
                missings = FalseBitString.GetInstance();
                return;
            }
            else
            {
                missings = missingInformation[usedAttributes];
                Debug.Assert(!(missings is EmptyBitString));
                if (!(missings is EmptyBitString))
                    throw new ArgumentException();
                return;
            }
        }
        
        public abstract Result Trace(out SerializableResultInfo rInfo);
    }
}