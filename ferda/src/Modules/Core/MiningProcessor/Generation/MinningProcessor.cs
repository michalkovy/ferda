using System;
using System.Collections.Generic;
using System.Text;
using Ferda.Guha.Data;
using Ferda.Guha.Math.Quantifiers;
using Ferda.Guha.MiningProcessor.BitStrings;
using Ferda.Guha.MiningProcessor.QuantifierEvaluator;
using Ferda.Modules;
using Ferda.Modules.Helpers.Common;

namespace Ferda.Guha.MiningProcessor.Generation
{
    public abstract class MiningProcessorBase
    {
        protected long _numberOfVerifications = 0;
        public long NumberOfVerifications
        {
            get { return _numberOfVerifications; }
        }

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

        public MiningProcessorBase(QuantifierBaseFunctionsPrx[] quantifiers, BitStringGeneratorProviderPrx taskFuncPrx)
        {
            _quantifiers = new Quantifiers(quantifiers, taskFuncPrx);
        }

        public static IEntitySetting GetBoolanAttributeBySemantic(MarkEnum semantic, BooleanAttribute[] booleanAttributes)
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

        public static BitStringGeneratorPrx[] GetCategorialAttributeBySemantic(MarkEnum semantic, CategorialAttribute[] categorialAttributes)
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

        public static IEntityEnumerator CreateBooleanAttributeTrace(MarkEnum semantic, BooleanAttribute[] booleanAttributes, bool allowsEmptyBitStrings)
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
                IMultipleOperandEntitySetting mos = (IMultipleOperandEntitySetting)setting;
                if (mos.minLength == 0 && !allowsEmptyBitStrings)
                    throw Exceptions.EmptyCedentIsNotAllowedError(semantic);
            }

            return Factory.Create(setting);
        }

        public static CategorialAttributeTrace[] CreateCategorialAttributeTrace(MarkEnum semantic, CategorialAttribute[] categorialAttributes, bool allowEmptyCategorialCedent)
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

        protected abstract void getCedents(out ICollection<IEntityEnumerator> booleanCedents, out ICollection<CategorialAttributeTrace[]> categorialCedents);

        protected static long totalCount(ICollection<IEntityEnumerator> booleanCedents, ICollection<CategorialAttributeTrace[]> categorialCedents)
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
        /// of IEmptyBitString.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="missings">The missings.</param>
        /// <param name="negation">The negation.</param>
        /// <param name="missingInformation">The missing information IBitString provider.</param>
        public static void GetNegationAndMissings(IBitString input, out IBitString missings, out IBitString negation, Set<Guid> usedAttributes, MissingInformation missingInformation)
        {
            missings = missingInformation[usedAttributes];
            negation = input.Not();
            return;
        }

        public abstract Result Trace();
    }

    public class FourFoldMiningProcessor : MiningProcessorBase
    {
        #region Fields
        private readonly IEntityEnumerator _antecedent;
        private readonly IEntityEnumerator _succedent;
        private readonly IEntityEnumerator _condition;
        #endregion

        public FourFoldMiningProcessor(
            BooleanAttribute[] booleanAttributes, 
            CategorialAttribute[] categorialAttributes, 
            QuantifierBaseFunctionsPrx[] quantifiers, 
            BitStringGeneratorProviderPrx taskFuncPrx
            )
            : base(quantifiers, taskFuncPrx)
        {
            // Validate quantifiers
            bool notOnlyFirstSetOperationMode;
            bool needsNumericValues; // ignore
            bool notOnlyDeletingMissingInformation; // ignore allways study ... for that purpose if user join new quantifier to task box after run to see its values in Result Browser
            CardinalityEnum maximalRequestedCardinality; // UNDONE ignored

            _quantifiers.ValidRequests(
                out notOnlyFirstSetOperationMode,
                out needsNumericValues,
                out notOnlyDeletingMissingInformation,
                out maximalRequestedCardinality
                );

            if (notOnlyFirstSetOperationMode)
                throw Ferda.Modules.Exceptions.BadParamsError(null, null, "Property \"Operation mode\" is not set to \"FirstSetOperationMode\" in some quantifier.", restrictionTypeEnum.OtherReason);

            // Create cedent traces
            _antecedent = CreateBooleanAttributeTrace(MarkEnum.Antecedent, booleanAttributes, true);
            _succedent = CreateBooleanAttributeTrace(MarkEnum.Succedent, booleanAttributes, false);
            _condition = CreateBooleanAttributeTrace(MarkEnum.Condition, booleanAttributes, true);
        }


        public override Result Trace()
        {
            Result result = new Result();
            result.TaskTypeEnum = TaskTypeEnum.FourFold;
            long allObjectsCount = Int64.MinValue;
            
            MissingInformation succedentMI = new MissingInformation();
            MissingInformation antecedentMI = new MissingInformation();
            MissingInformation conditionMI = new MissingInformation();
            IBitString sMis;
            IBitString sNeg;
            IBitString aMis;
            IBitString aNeg;
            IBitString cMis;
            IBitString cNeg;
            foreach (IBitString s in _succedent)
            {
                GetNegationAndMissings(s, out sNeg, out sMis, _succedent.UsedAttributes, succedentMI);
                if (allObjectsCount<0)
                    allObjectsCount = s.Length;
                foreach (IBitString a in _antecedent)
                {
                    GetNegationAndMissings(a, out aNeg, out aMis, _antecedent.UsedAttributes, antecedentMI);
                    foreach (IBitString c in _condition)
                    {
                        GetNegationAndMissings(c, out cNeg, out cMis, _condition.UsedAttributes, conditionMI);
                        _numberOfVerifications++;
                        FourFoldContingencyTable fft = new FourFoldContingencyTable();
                        //TODO
                        _quantifiers.Valid(
                            new ContingencyTableHelper(
                                fft.ContingencyTable, 
                                allObjectsCount
                                )
                            );
                    }
                }
            }
            result.AllObjectsCount = allObjectsCount;
            return result;
        }

        protected override void getCedents(out ICollection<IEntityEnumerator> booleanCedents, out ICollection<CategorialAttributeTrace[]> categorialCedents)
        {
            booleanCedents = new IEntityEnumerator[] { _antecedent, _succedent, _condition };
            categorialCedents = null;
        }
    }
}
