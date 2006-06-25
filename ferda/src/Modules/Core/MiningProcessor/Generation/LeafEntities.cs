#define Testing
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Ferda.Guha.Math;
using Ferda.Guha.MiningProcessor.BitStrings;
using Ferda.Guha.MiningProcessor.Formulas;

namespace Ferda.Guha.MiningProcessor.Generation
{
    public class FixedSet : EntityEnumerable
    {
        private readonly CoefficientFixedSetSetting _setting;

        public FixedSet(CoefficientFixedSetSetting setting)
            : base(new Guid(setting.id.value))
        {
            _setting = setting;
        }

        public override IEnumerator<IBitString> GetBitStringEnumerator()
        {
            yield return Helpers.GetBitString(
                _setting.generator,
                new Guid(_setting.generator.GetAttributeId().value),
                _setting.categoriesIds,
                BitwiseOperation.Or);
        }

        public override long TotalCount
        {
            get { return 1; }
        }

        public override string ToString()
        {
            string result = "";
#if Testing
            result += AttributeNameInLiteralsProvider.GetAttributeNameInLiterals(new Guid(_setting.id.value));
#else
            result += AttributeNameInLiteralsProvider.GetAttributeNameInLiterals(
                new Guid(_setting.generator.GetAttributeId().value)
                );
#endif
            result += "["
                + FormulaHelper.SequenceToString(_setting.categoriesIds, FormulaSeparator.AtomMembers, true)
                + "] (fixed set)";
            return result;
        }
    }

    public class LeftCuts : EntityEnumerableCoefficient
    {
        public LeftCuts(CoefficientSetting setting)
            : base(setting)
        {
            Debug.Assert(setting.coefficientType == CoefficientTypeEnum.LeftCuts);
            //TODO integritni omezeni (ordinal...)
        }

        public override IEnumerator<IBitString> GetBitStringEnumerator()
        {
            if (_effectiveMinLength <= _categoriesNames.Length)
                for (int i = 0; true; i++)
                {
                    prolongCoefficient(_categoriesNames[i]);
                    if (_actualLength < _effectiveMinLength)
                        continue;
                    yield return _currentBitString;
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
            result += AttributeNameInLiteralsProvider.GetAttributeNameInLiterals(new Guid(_setting.id.value));
#else
            result += AttributeNameInLiteralsProvider.GetAttributeNameInLiterals(
                new Guid(_setting.generator.GetAttributeId().value)
                );
#endif
            result += "Left Cuts [" + _effectiveMinLength + "-" + _effectiveMaxLength + "]";
            return result;
        }
    }

    public class RightCuts : EntityEnumerableCoefficient
    {
        public RightCuts(CoefficientSetting setting)
            : base(setting)
        {
            Debug.Assert(setting.coefficientType == CoefficientTypeEnum.RightCuts);
            //TODO integritni omezeni (ordinal...)
        }

        public override IEnumerator<IBitString> GetBitStringEnumerator()
        {
            if (_effectiveMinLength <= _categoriesNames.Length)
                for (int i = _categoriesNames.Length - 1; true; i--)
                {
                    prolongCoefficient(_categoriesNames[i]);
                    if (_actualLength < _effectiveMinLength)
                        continue;
                    yield return _currentBitString;
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
            result += AttributeNameInLiteralsProvider.GetAttributeNameInLiterals(new Guid(_setting.id.value));
#else
            result += AttributeNameInLiteralsProvider.GetAttributeNameInLiterals(
                new Guid(_setting.generator.GetAttributeId().value)
                );
#endif
            result += "Right Cuts [" + _effectiveMinLength + "-" + _effectiveMaxLength + "]";
            return result;
        }
    }

    public class Cuts : EntityEnumerableCoefficient
    {
        public Cuts(CoefficientSetting setting)
            : base(setting)
        {
            Debug.Assert(setting.coefficientType == CoefficientTypeEnum.Cuts);
            //TODO integritni omezeni (ordinal...)
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
                    yield return _currentBitString;
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
                    yield return _currentBitString;
                }

            resetCoefficient();
        }

        public override long TotalCount
        {
            get
            {
                if (_effectiveMaxLength == _categoriesNames.Length)
                    return ((_effectiveMaxLength - _effectiveMinLength + 1) * 2) - 1;
                else
                    return (_effectiveMaxLength - _effectiveMinLength + 1) * 2;
            }
        }

        public override string ToString()
        {
            string result = "";
#if Testing
            result += AttributeNameInLiteralsProvider.GetAttributeNameInLiterals(new Guid(_setting.id.value));
#else
            result += AttributeNameInLiteralsProvider.GetAttributeNameInLiterals(
                new Guid(_setting.generator.GetAttributeId().value)
                );
#endif
            result += "Cuts [" + _effectiveMinLength + "-" + _effectiveMaxLength + "]";
            return result;
        }
    }

    public class Intervals : EntityEnumerableCoefficient
    {
        public Intervals(CoefficientSetting setting)
            : base(setting)
        {
            Debug.Assert(setting.coefficientType == CoefficientTypeEnum.Intervals);
            //TODO integritni omezeni (ordinal...)
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
                    yield return _currentBitString;
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
            result += AttributeNameInLiteralsProvider.GetAttributeNameInLiterals(new Guid(_setting.id.value));
#else
            result += AttributeNameInLiteralsProvider.GetAttributeNameInLiterals(
                new Guid(_setting.generator.GetAttributeId().value)
                );
#endif
            result += "Intervals [" + _effectiveMinLength + "-" + _effectiveMaxLength + "]";
            return result;
        }
    }

    public class CyclicIntervals : EntityEnumerableCoefficient
    {
        public CyclicIntervals(CoefficientSetting setting)
            : base(setting)
        {
            Debug.Assert(setting.coefficientType == CoefficientTypeEnum.CyclicIntervals);
            //TODO integritni omezeni (ordinal...)
        }

        public override IEnumerator<IBitString> GetBitStringEnumerator()
        {
            int start = -1;
        restart:
            start++;
            if (start < _categoriesNames.Length)
                for (int i = start; true; i++)
                {
                    prolongCoefficient(_categoriesNames[i % _categoriesNames.Length]);
                    if (_actualLength < _effectiveMinLength)
                        continue;
                    if (_actualLength == _categoriesNames.Length && start > 0)
                    {
                        resetCoefficient();
                        goto restart;
                    }
                    yield return _currentBitString;
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
                    return (_effectiveMaxLength - _effectiveMinLength + 1) * _categoriesNames.Length;
                }
                else
                {
                    return (_effectiveMaxLength - _effectiveMinLength) * _categoriesNames.Length + 1;
                }
            }
        }

        public override string ToString()
        {
            string result = "";
#if Testing
            result += AttributeNameInLiteralsProvider.GetAttributeNameInLiterals(new Guid(_setting.id.value));
#else
            result += AttributeNameInLiteralsProvider.GetAttributeNameInLiterals(
                new Guid(_setting.generator.GetAttributeId().value)
                );
#endif
            result += "Cyclic Intervals [" + _effectiveMinLength + "-" + _effectiveMaxLength + "]";
            return result;
        }
    }

    public class Subsets : EntityEnumerableCoefficient, SubsetsInstance<IBitString, IBitString>
    {
        public Subsets(CoefficientSetting setting)
            : base(setting)
        {
            Debug.Assert(setting.coefficientType == CoefficientTypeEnum.Subsets);
            //TODO integritni omezeni (ordinal...)
        }
        /*
        Stack<IBitString> sB = new Stack<IBitString>();
        Stack<int> sI = new Stack<int>();
        private void sBPush(IBitString adding)
        {
            if (sB.Count > 0)
            {
                IBitString previous = sB.Peek();
                sB.Push(previous.Or(adding));
            }
            else
            {
                sB.Push(adding);
            }
        }
        private bool returnCurrent(out IBitString result)
        {
            Debug.Assert(sB.Count <= _effectiveMaxLength);
            if (sB.Count >= _effectiveMinLength)
            {
                result = sB.Peek();
                return true;
            }
            result = null;
            return false;
        }
        private void getEntity(int index)
        {
            IBitString bS = getBitString(_categoriesNames[index]);
            sBPush(bS);
            sI.Push(index);
        }
        private bool prolong(bool afterRemove)
        {
            if (sB.Count == _effectiveMaxLength) // not after remove
                return false;
            int newIndex;
            if (afterRemove)
                newIndex = sI.Pop() + 1;
            else
                newIndex = sI.Peek() + 1;
            if (newIndex >= _categoriesNames.Length)
                return false;
            getEntity(newIndex);
            return true;
        }
        private bool removeLastItem()
        {
            if (sB.Count > 0)
            {
                sB.Pop();
                return true;
            }
            return false;
        }
        public override IEnumerator<IBitString> GetBitStringEnumerator()
        {
            IBitString result;
            bool afterRemove;
            afterRemove = false;

            #region initialize

            sB.Clear();
            sI.Clear();
            getEntity(0);

            #endregion

        returnCurrent:
            if (returnCurrent(out result))
                yield return result;
        prolong:
            if (prolong(afterRemove))
            {
                afterRemove = false;
                goto returnCurrent;
            }
            if (removeLastItem())
            {
                afterRemove = true;
                goto prolong;
            }
        }
        */

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
            result += AttributeNameInLiteralsProvider.GetAttributeNameInLiterals(new Guid(_setting.id.value));
#else
            result += AttributeNameInLiteralsProvider.GetAttributeNameInLiterals(
                new Guid(_setting.generator.GetAttributeId().value)
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
