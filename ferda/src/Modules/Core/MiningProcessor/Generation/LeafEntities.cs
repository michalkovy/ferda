using System;
using System.Collections.Generic;
using System.Diagnostics;
using Ferda.Guha.Math;
using Ferda.Guha.MiningProcessor.BitStrings;

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
                    if (_actualLenght < _effectiveMinLength)
                        continue;
                    yield return _currentBitString;
                    if (_actualLenght + 1 > _effectiveMaxLength)
                        break;
                }
        }

        public override long TotalCount
        {
            get { return _effectiveMaxLength - _effectiveMinLength + 1; }
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
                    if (_actualLenght < _effectiveMinLength)
                        continue;
                    yield return _currentBitString;
                    if (_actualLenght + 1 > _effectiveMaxLength)
                        break;
                }
        }

        public override long TotalCount
        {
            get { return _effectiveMaxLength - _effectiveMinLength + 1; }
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
                    if (_actualLenght < _effectiveMinLength)
                        continue;
                    yield return _currentBitString;
                    if (_actualLenght + 1 > _effectiveMaxLength)
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
                    if (_actualLenght > _effectiveMaxLength
                        || _actualLenght == _categoriesNames.Length)
                        break;
                    if (_actualLenght < _effectiveMinLength)
                        continue;
                    yield return _currentBitString;
                }
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
                    if (_actualLenght < _effectiveMinLength)
                        continue;
                    yield return _currentBitString;
                    if ((_actualLenght + 1 > _effectiveMaxLength)
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
                    if (_actualLenght < _effectiveMinLength)
                        continue;
                    if (_actualLenght == _categoriesNames.Length && start > 0)
                    {
                        resetCoefficient();
                        goto restart;
                    }
                    yield return _currentBitString;
                    if (_actualLenght + 1 > _effectiveMaxLength)
                    {
                        resetCoefficient();
                        goto restart;
                    }
                }
        }

        //public override IEnumerator<IBitString> GetBitStringEnumerator()
        //{
        //    int start = 0;
        //restart:
        //    if (_effectiveMinLength <= _categoriesNames.Length - start
        //        && start < _categoriesNames.Length)
        //        for (int i = start; true; i++)
        //        {
        //            prolongCoefficient(_categoriesNames[i]);
        //            if (_actualLenght < _effectiveMinLength)
        //                continue;
        //            yield return _currentBitString;
        //            if ((_actualLenght + 1 > _effectiveMaxLength)
        //                || (i + 1 >= _categoriesNames.Length))
        //            {
        //                //if (i == _categoriesNames.Length - 1)
        //                //{
        //                //    break;
        //                //}
        //                start++;
        //                resetCoefficient();
        //                goto restart;
        //            }
        //        }
        //}

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
    }

    public class Subsets : EntityEnumerableCoefficient
    {
        public Subsets(CoefficientSetting setting)
            : base(setting)
        {
            Debug.Assert(setting.coefficientType == CoefficientTypeEnum.Subsets);
            //TODO integritni omezeni (ordinal...)
        }

        public override IEnumerator<IBitString> GetBitStringEnumerator()
        {
            //TODO
            throw new Exception("The method or operation is not implemented.");
        }

        public override long TotalCount
        {
            get
            {
                long result = 0;
                for (int j = _effectiveMinLength; j <= _categoriesNames.Length; j++)
                {
                    result += Combinatorics.BinomialCoefficient(_categoriesNames.Length, j);
                }
                return result;
            }
        }
    }
}