//#define Testing
using System.Collections.Generic;
using System.Diagnostics;
using Ferda.Guha.Math;
using Ferda.Guha.MiningProcessor.BitStrings;
using Ferda.Guha.MiningProcessor.Formulas;
using Ferda.Modules.Helpers.Common;

namespace Ferda.Guha.MiningProcessor.Generation
{
    public class FixedSet : EntityEnumerable
    {
        private readonly CoefficientFixedSetSetting _setting;

        private string _attributeGuid;

        public FixedSet(CoefficientFixedSetSetting setting, ISkipOptimalization skipOptimalization, MarkEnum cedentType)
            : base(setting.id, skipOptimalization, cedentType)
        {
            _setting = setting;
            _attributeGuid = setting.generator.GetAttributeId().value;
        }

        public override IEnumerator<IBitString> GetBitStringEnumerator()
        {
            IBitString result = Helpers.GetBitString(
                _setting.generator,
                _attributeGuid,
                _setting.categoriesIds,
                BitwiseOperation.Or);
            
            SkipSetting parentSkipSetting = ParentSkipOptimalization.BaseSkipSetting(CedentType);
            if (parentSkipSetting != null)
            {
                if (Common.Compare(parentSkipSetting.Relation, result.Sum, parentSkipSetting.Treshold))
                    yield return result;
            }
            else
            {
                yield return result;
            }
        }

        public override long TotalCount
        {
            get { return 1; }
        }

        public override string ToString()
        {
            string result = "";
#if Testing
            result += AttributeNameInLiteralsProvider.GetAttributeNameInLiterals(Guid);
#else
            result += AttributeNameInLiteralsProvider.GetAttributeNameInLiterals(
                _attributeGuid
                );
#endif
            result += "["
                      + FormulaHelper.SequenceToString(_setting.categoriesIds, FormulaSeparator.AtomMembers, true)
                      + "] (fixed set)";
            return result;
        }

        public override Set<string> UsedAttributes
        {
            get { return new Set<string>(_attributeGuid); }
        }

        public override Set<string> UsedEntities
        {
            get { return new Set<string>(Guid); }
        }
    }

    public class LeftCuts : EntityEnumerableCoefficient
    {
        public LeftCuts(CoefficientSetting setting, ISkipOptimalization skipOptimalization, MarkEnum cedentType)
            : base(setting, skipOptimalization, cedentType)
        {
            Debug.Assert(setting.coefficientType == CoefficientTypeEnum.LeftCuts);
            //UNDONE integritni omezeni (ordinal...)
        }

        public override IEnumerator<IBitString> GetBitStringEnumerator()
        {
            if (_effectiveMinLength <= _categoriesNames.Length)
                for (int i = 0; true; i++)
                {
                    prolongCoefficient(_categoriesNames[i]);
                    if (_actualLength < _effectiveMinLength)
                        continue;
                    
                    SkipSetting parentSkipSetting = ParentSkipOptimalization.BaseSkipSetting(CedentType);
                    if (parentSkipSetting != null)
                    {
                        if (Common.Compare(parentSkipSetting.Relation, _currentBitString.Sum, parentSkipSetting.Treshold))
                            yield return _currentBitString;
                    }
                    else
                    {
                        yield return _currentBitString;
                    }
                    
                    if (_actualLength + 1 > _effectiveMaxLength)
                        break;
                }
            resetCoefficient();
        }

        public override long TotalCount
        {
            get { return _effectiveMaxLength - _effectiveMinLength + 1; }
        }

        public override string ToString()
        {
            string result = "";
#if Testing
            result += AttributeNameInLiteralsProvider.GetAttributeNameInLiterals(_setting.id.value);
#else
            result += AttributeNameInLiteralsProvider.GetAttributeNameInLiterals(
                _setting.generator.GetAttributeId().value
                );
#endif
            result += "Left Cuts [" + _effectiveMinLength + "-" + _effectiveMaxLength + "]";
            return result;
        }
    }

    public class RightCuts : EntityEnumerableCoefficient
    {
        public RightCuts(CoefficientSetting setting, ISkipOptimalization skipOptimalization, MarkEnum cedentType)
            : base(setting, skipOptimalization, cedentType)
        {
            Debug.Assert(setting.coefficientType == CoefficientTypeEnum.RightCuts);
            //UNDONE integritni omezeni (ordinal...)
        }

        public override IEnumerator<IBitString> GetBitStringEnumerator()
        {
            if (_effectiveMinLength <= _categoriesNames.Length)
                for (int i = _categoriesNames.Length - 1; true; i--)
                {
                    prolongCoefficient(_categoriesNames[i]);
                    if (_actualLength < _effectiveMinLength)
                        continue;

                    SkipSetting parentSkipSetting = ParentSkipOptimalization.BaseSkipSetting(CedentType);
                    if (parentSkipSetting != null)
                    {
                        if (Common.Compare(parentSkipSetting.Relation, _currentBitString.Sum, parentSkipSetting.Treshold))
                            yield return _currentBitString;
                    }
                    else
                    {
                        yield return _currentBitString;
                    }
                    
                    if (_actualLength + 1 > _effectiveMaxLength)
                        break;
                }
            resetCoefficient();
        }

        public override long TotalCount
        {
            get { return _effectiveMaxLength - _effectiveMinLength + 1; }
        }

        public override string ToString()
        {
            string result = "";
#if Testing
            result += AttributeNameInLiteralsProvider.GetAttributeNameInLiterals(_setting.id.value);
#else
            result += AttributeNameInLiteralsProvider.GetAttributeNameInLiterals(
                _setting.generator.GetAttributeId().value
                );
#endif
            result += "Right Cuts [" + _effectiveMinLength + "-" + _effectiveMaxLength + "]";
            return result;
        }
    }

    public class Cuts : EntityEnumerableCoefficient
    {
        public Cuts(CoefficientSetting setting, ISkipOptimalization skipOptimalization, MarkEnum cedentType)
            : base(setting, skipOptimalization, cedentType)
        {
            Debug.Assert(setting.coefficientType == CoefficientTypeEnum.Cuts);
            //UNDONE integritni omezeni (ordinal...)
        }

        public override IEnumerator<IBitString> GetBitStringEnumerator()
        {
            // split to left cuts and right cuts

            // left cuts
            if (_effectiveMinLength <= _categoriesNames.Length)
                for (int i = 0; true; i++)
                {
                    prolongCoefficient(_categoriesNames[i]);
                    if (_actualLength < _effectiveMinLength)
                        continue;

                    SkipSetting parentSkipSetting = ParentSkipOptimalization.BaseSkipSetting(CedentType);
                    if (parentSkipSetting != null)
                    {
                        if (Common.Compare(parentSkipSetting.Relation, _currentBitString.Sum, parentSkipSetting.Treshold))
                            yield return _currentBitString;
                    }
                    else
                    {
                        yield return _currentBitString;
                    }
                    
                    if (_actualLength + 1 > _effectiveMaxLength)
                        break;
                }

            // reset 
            resetCoefficient();

            // do not repeat coefficient with all categories
            // right cuts
            //if (_effectiveMinLength <= _categoriesNames.Length)
            if (_categoriesNames.Length != _effectiveMaxLength || _categoriesNames.Length != 1)
                for (int i = _categoriesNames.Length - 1; true; i--)
                {
                    if (i < 0)
                        break;
                    prolongCoefficient(_categoriesNames[i]);
                    if (_actualLength > _effectiveMaxLength
                        || _actualLength == _categoriesNames.Length)
                        break;
                    if (_actualLength < _effectiveMinLength)
                        continue;

                    SkipSetting parentSkipSetting = ParentSkipOptimalization.BaseSkipSetting(CedentType);
                    if (parentSkipSetting != null)
                    {
                        if (Common.Compare(parentSkipSetting.Relation, _currentBitString.Sum, parentSkipSetting.Treshold))
                            yield return _currentBitString;
                    }
                    else
                    {
                        yield return _currentBitString;
                    }
                }

            resetCoefficient();
        }

        public override long TotalCount
        {
            get
            {
                if (_effectiveMaxLength == _categoriesNames.Length)
                    return ((_effectiveMaxLength - _effectiveMinLength + 1)*2) - 1;
                else
                    return (_effectiveMaxLength - _effectiveMinLength + 1)*2;
            }
        }

        public override string ToString()
        {
            string result = "";
#if Testing
            result += AttributeNameInLiteralsProvider.GetAttributeNameInLiterals(_setting.id.value);
#else
            result += AttributeNameInLiteralsProvider.GetAttributeNameInLiterals(
                _setting.generator.GetAttributeId().value
                );
#endif
            result += "Cuts [" + _effectiveMinLength + "-" + _effectiveMaxLength + "]";
            return result;
        }
    }

    public class Intervals : EntityEnumerableCoefficient
    {
        public Intervals(CoefficientSetting setting, ISkipOptimalization skipOptimalization, MarkEnum cedentType)
            : base(setting, skipOptimalization, cedentType)
        {
            Debug.Assert(setting.coefficientType == CoefficientTypeEnum.Intervals);
            //UNDONE integritni omezeni (ordinal...)
        }

        public override IEnumerator<IBitString> GetBitStringEnumerator()
        {
            int start = 0;
            restart:
            if (_effectiveMinLength <= _categoriesNames.Length - start
                && start < _categoriesNames.Length)
                for (int i = start; true; i++)
                {
                    prolongCoefficient(_categoriesNames[i]);
                    if (_actualLength < _effectiveMinLength)
                        continue;

                    SkipSetting parentSkipSetting = ParentSkipOptimalization.BaseSkipSetting(CedentType);
                    if (parentSkipSetting != null)
                    {
                        if (Common.Compare(parentSkipSetting.Relation, _currentBitString.Sum, parentSkipSetting.Treshold))
                            yield return _currentBitString;
                    }
                    else
                    {
                        yield return _currentBitString;
                    }
                    
                    if ((_actualLength + 1 > _effectiveMaxLength)
                        || (i + 1 >= _categoriesNames.Length))
                    {
                        //if (i == _categoriesNames.Length - 1)
                        //{
                        //    break;
                        //}
                        start++;
                        resetCoefficient();
                        goto restart;
                    }
                }
            resetCoefficient();
        }

        public override long TotalCount
        {
            get
            {
                return Combinatorics.SequenceSum(
                    _categoriesNames.Length - _effectiveMaxLength + 1,
                    _categoriesNames.Length - _effectiveMinLength + 1
                    );
            }
        }

        public override string ToString()
        {
            string result = "";
#if Testing
            result += AttributeNameInLiteralsProvider.GetAttributeNameInLiterals(_setting.id.value);
#else
            result += AttributeNameInLiteralsProvider.GetAttributeNameInLiterals(
                _setting.generator.GetAttributeId().value
                );
#endif
            result += "Intervals [" + _effectiveMinLength + "-" + _effectiveMaxLength + "]";
            return result;
        }
    }

    public class CyclicIntervals : EntityEnumerableCoefficient
    {
        public CyclicIntervals(CoefficientSetting setting, ISkipOptimalization skipOptimalization, MarkEnum cedentType)
            : base(setting, skipOptimalization, cedentType)
        {
            Debug.Assert(setting.coefficientType == CoefficientTypeEnum.CyclicIntervals);
            //UNDONE integritni omezeni (ordinal...)
        }

        public override IEnumerator<IBitString> GetBitStringEnumerator()
        {
            int start = -1;
            restart:
            start++;
            if (start < _categoriesNames.Length)
                for (int i = start; true; i++)
                {
                    prolongCoefficient(_categoriesNames[i%_categoriesNames.Length]);
                    if (_actualLength < _effectiveMinLength)
                        continue;
                    if (_actualLength == _categoriesNames.Length && start > 0)
                    {
                        resetCoefficient();
                        goto restart;
                    }
                    
                    SkipSetting parentSkipSetting = ParentSkipOptimalization.BaseSkipSetting(CedentType);
                    if (parentSkipSetting != null)
                    {
                        if (Common.Compare(parentSkipSetting.Relation, _currentBitString.Sum, parentSkipSetting.Treshold))
                            yield return _currentBitString;
                    }
                    else
                    {
                        yield return _currentBitString;
                    }
                    
                    if (_actualLength + 1 > _effectiveMaxLength)
                    {
                        resetCoefficient();
                        goto restart;
                    }
                }
            resetCoefficient();
        }

        public override long TotalCount
        {
            get
            {
                if (_effectiveMaxLength < _categoriesNames.Length)
                {
                    return (_effectiveMaxLength - _effectiveMinLength + 1)*_categoriesNames.Length;
                }
                else
                {
                    return (_effectiveMaxLength - _effectiveMinLength)*_categoriesNames.Length + 1;
                }
            }
        }

        public override string ToString()
        {
            string result = "";
#if Testing
            result += AttributeNameInLiteralsProvider.GetAttributeNameInLiterals(_setting.id.value);
#else
            result += AttributeNameInLiteralsProvider.GetAttributeNameInLiterals(
                _setting.generator.GetAttributeId().value
                );
#endif
            result += "Cyclic Intervals [" + _effectiveMinLength + "-" + _effectiveMaxLength + "]";
            return result;
        }
    }

    public class Subsets : EntityEnumerableCoefficient, SubsetsInstance<IBitString, IBitString>
    {
        public Subsets(CoefficientSetting setting, ISkipOptimalization skipOptimalization, MarkEnum cedentType)
            : base(setting, skipOptimalization, cedentType)
        {
            Debug.Assert(setting.coefficientType == CoefficientTypeEnum.Subsets);
            //UNDONE integritni omezeni (ordinal...)
        }

        public override IEnumerator<IBitString> GetBitStringEnumerator()
        {
            Subsets<IBitString, IBitString> enumerator =
                new Subsets<IBitString, IBitString>(_effectiveMinLength, _effectiveMaxLength, _categoriesNames.Length,
                                                    this);
            return enumerator.GetEnumerator();
        }

        public override long TotalCount
        {
            get
            {
                long result = 0;
                for (int j = _effectiveMinLength; j <= _effectiveMaxLength; j++)
                {
                    result += Combinatorics.BinomialCoefficient(_categoriesNames.Length, j);
                }
                return result;
            }
        }

        public override string ToString()
        {
            string result = "";
#if Testing
            result += AttributeNameInLiteralsProvider.GetAttributeNameInLiterals(_setting.id.value);
#else
            result += AttributeNameInLiteralsProvider.GetAttributeNameInLiterals(
                _setting.generator.GetAttributeId().value
                );
#endif
            result += "Subsets [" + _effectiveMinLength + "-" + _effectiveMaxLength + "]";
            return result;
        }

        #region SubsetsInstance<IBitString,IBitString> Members

        public IBitString operation(IBitString previous, IBitString current)
        {
            return previous.Or(current);
        }

        public IBitString operation(IBitString current)
        {
            return current;
        }

        public IBitString getItem(int index)
        {
            return getBitString(_categoriesNames[index]);
        }

        public IBitString getDefaultInit()
        {
            return null;
        }

        #endregion
    }
}