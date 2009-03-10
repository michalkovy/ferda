// LeafEntities.cs - Leaf entities enumerators
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

//#define Testing
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Ferda.Guha.Math;
using Ferda.Guha.MiningProcessor.BitStrings;
using Ferda.Guha.MiningProcessor.Formulas;
using Ferda.Modules.Helpers.Common;

namespace Ferda.Guha.MiningProcessor.Generation
{
    /// <summary>
    /// The fixed set entity enumerator.
    /// </summary>
    public class FixedSet : EntityEnumerable
    {
        /// <summary>
        /// The setting of the fixed set coeffiecient. It contains 
        /// names of the categories that form the fixed set .
        /// </summary>
        private readonly CoefficientFixedSetSetting _setting;

        /// <summary>
        /// Identification of the attribute (where the fixed set is constructed)
        /// </summary>
        private string _attributeGuid;

        /// <summary>
        /// Default constructor of the class
        /// </summary>
        /// <param name="setting">Fixed set coefficient setting</param>
        /// <param name="skipOptimalization">The skip steps optimalization</param>
        /// <param name="cedentType">Type of the cedent</param>
        public FixedSet(CoefficientFixedSetSetting setting, ISkipOptimalization skipOptimalization, MarkEnum cedentType)
            : base(setting.id, skipOptimalization, cedentType)
        {
            _setting = setting;
            _attributeGuid = setting.generator.GetAttributeId().value;
        }

        /// <summary>
        /// Retrieves the entity enumerator. For the fixed set enumerator,
        /// it retrieves only one bit string. This bit string is an OR result
        /// of the fixed set categories of the attribute. The base skip setting
        /// is also applied. 
        /// </summary>
        /// <returns>Entity enumerator</returns>
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

        #region IEntityEnumerator members

        /// <summary>
        /// Total number of bit strings in this enumerator - for fixed set it is
        /// always 1. 
        /// </summary>
        public override long TotalCount
        {
            get { return 1; }
        }

        /// <summary>
        /// Set of used attributes by enumerator. For the fixed set enumerator
        /// it is the set containing only one attribute.
        /// </summary>
        public override Set<string> UsedAttributes
        {
            get { return new Set<string>(_attributeGuid); }
        }

        #endregion

        /// <summary>
        /// Returns text representation of the fixed set enumerator, which is in
        /// form <c>ATTRIBUTE[c1, c2, c3]</c>
        /// </summary>
        /// <returns>Text representation of the fixed set</returns>
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
    }

    /// <summary>
    /// Left cuts coefficients entity enumerator
    /// </summary>
    public class LeftCuts : EntityEnumerableCoefficient
    {
        /// <summary>
        /// Default constructor of the class
        /// </summary>
        /// <param name="setting">Coefficient setting</param>
        /// <param name="skipOptimalization">The skip steps optimalization</param>
        /// <param name="cedentType">Type of the cedent</param>
        public LeftCuts(CoefficientSetting setting, ISkipOptimalization skipOptimalization, MarkEnum cedentType)
            : base(setting, skipOptimalization, cedentType)
        {
            Debug.Assert(setting.coefficientType == CoefficientTypeEnum.LeftCuts);
            //UNDONE integritni omezeni (ordinal...)
        }

        /// <summary>
        /// Retrieves the entity enumerator - the first (maxLength - minLength + 1)
        /// bit strings representing the left cuts
        /// </summary>
        /// <returns>Entity enumerator</returns>
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

        #region IEntityEnumerator members

        /// <summary>
        /// Total number of bit strings in this enumerator. For left cuts
        /// it is the <c>(maxLength - minLength + 1)</c>
        /// </summary>
        public override long TotalCount
        {
            get { return _effectiveMaxLength - _effectiveMinLength + 1; }
        }

        #endregion

        /// <summary>
        /// Returns text representation of the left cuts coefficient enumerator, 
        /// which is in form <c>ATTRIBUTE Left Cuts[minLenth - maxLength]</c>
        /// </summary>
        /// <returns>Text representation of the left cuts coefficient</returns>
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

    /// <summary>
    /// Right cuts coefficients entity enumerator
    /// </summary>
    public class RightCuts : EntityEnumerableCoefficient
    {
        /// <summary>
        /// Default constructor of the class
        /// </summary>
        /// <param name="setting">Coefficient setting</param>
        /// <param name="skipOptimalization">The skip steps optimalization</param>
        /// <param name="cedentType">Type of the cedent</param>
        public RightCuts(CoefficientSetting setting, ISkipOptimalization skipOptimalization, MarkEnum cedentType)
            : base(setting, skipOptimalization, cedentType)
        {
            Debug.Assert(setting.coefficientType == CoefficientTypeEnum.RightCuts);
            //UNDONE integritni omezeni (ordinal...)
        }

        /// <summary>
        /// Retrieves the entity enumerator - the last (maxLength - minLength + 1)
        /// bit strings representing the right cuts
        /// </summary>
        /// <returns>Entity enumerator</returns>
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

        #region IEntityEnumerator members

        /// <summary>
        /// Total number of bit strings in this enumerator. For right cuts
        /// it is the <c>(maxLength - minLength + 1)</c>
        /// </summary>
        public override long TotalCount
        {
            get { return _effectiveMaxLength - _effectiveMinLength + 1; }
        }

        #endregion

        /// <summary>
        /// Returns text representation of the right cuts coefficient enumerator, 
        /// which is in form <c>ATTRIBUTE Right Cuts[minLenth - maxLength]</c>
        /// </summary>
        /// <returns>Text representation of the right cuts coefficient</returns>
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

    /// <summary>
    /// Cuts coefficients entity enumerator
    /// </summary>
    public class Cuts : EntityEnumerableCoefficient
    {
        /// <summary>
        /// Default constructor of the class
        /// </summary>
        /// <param name="setting">Coefficient setting</param>
        /// <param name="skipOptimalization">The skip steps optimalization</param>
        /// <param name="cedentType">Type of the cedent</param>
        public Cuts(CoefficientSetting setting, ISkipOptimalization skipOptimalization, MarkEnum cedentType)
            : base(setting, skipOptimalization, cedentType)
        {
            Debug.Assert(setting.coefficientType == CoefficientTypeEnum.Cuts);
            //UNDONE integritni omezeni (ordinal...)
        }

        /// <summary>
        /// Retrieves the entity enumerator - the first and last (maxLength - minLength + 1)
        /// bit strings representing the cuts. The method first constructs the left cuts,
        /// then the right cuts.
        /// </summary>
        /// <returns>Entity enumerator</returns>
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

        #region IEntityEnumerator members

        /// <summary>
        /// Total number of bit strings in this enumerator. For cuts
        /// it is the <c>(maxLength - minLength)*2</c> for cases when
        /// <c>maxLength</c> is smaller then <c>categoriesCount</c>
        /// and <c>(maxLength - minLength)*2 - 1</c> otherwise.
        /// </summary>
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

        #endregion

        /// <summary>
        /// Returns text representation of the cuts coefficient enumerator, 
        /// which is in form <c>ATTRIBUTE Cuts[minLenth - maxLength]</c>
        /// </summary>
        /// <returns>Text representation of the cuts coefficient</returns>
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

    /// <summary>
    /// Interval coefficients entity enumerator
    /// </summary>
    public class Intervals : EntityEnumerableCoefficient
    {
        /// <summary>
        /// Default constructor of the class
        /// </summary>
        /// <param name="setting">Coefficient setting</param>
        /// <param name="skipOptimalization">The skip steps optimalization</param>
        /// <param name="cedentType">Type of the cedent</param>
        public Intervals(CoefficientSetting setting, ISkipOptimalization skipOptimalization, MarkEnum cedentType)
            : base(setting, skipOptimalization, cedentType)
        {
            Debug.Assert(setting.coefficientType == CoefficientTypeEnum.Intervals);
            //UNDONE integritni omezeni (ordinal...)
        }

        /// <summary>
        /// Retrieves the entity enumerator - the intervals of all the lengths from
        /// <c>minLenght</c> to <c>maxLength</c>. The enumerator starts with intervals
        /// with minimal length and then prolongs them. 
        /// </summary>
        /// <returns>Entity enumerator</returns>
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

        #region IEntityEnumerator members

        /// <summary>
        /// Total number of bit strings in this enumerator. For intervals
        /// it is the sum of intervals of lenght from <c>minLenght</c> to
        /// <c>maxLegth</c>
        /// </summary>
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

        #endregion

        /// <summary>
        /// Returns text representation of the intervals coefficient enumerator, 
        /// which is in form <c>ATTRIBUTE Intervals[minLenth - maxLength]</c>
        /// </summary>
        /// <returns>Text representation of the intervals coefficient</returns>
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

    /// <summary>
    /// Cyclic interval coefficients entity enumerator
    /// </summary>
    public class CyclicIntervals : EntityEnumerableCoefficient
    {
        /// <summary>
        /// Default constructor of the class
        /// </summary>
        /// <param name="setting">Coefficient setting</param>
        /// <param name="skipOptimalization">The skip steps optimalization</param>
        /// <param name="cedentType">Type of the cedent</param>
        public CyclicIntervals(CoefficientSetting setting, ISkipOptimalization skipOptimalization, MarkEnum cedentType)
            : base(setting, skipOptimalization, cedentType)
        {
            Debug.Assert(setting.coefficientType == CoefficientTypeEnum.CyclicIntervals);
            //UNDONE integritni omezeni (ordinal...)
        }

        /// <summary>
        /// Retrieves the entity enumerator - the cyclic intervals of all the lengths from
        /// <c>minLenght</c> to <c>maxLength</c>. The enumerator starts with cyclic intervals
        /// with minimal length and then prolongs them. 
        /// </summary>
        /// <returns>Entity enumerator</returns>
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

        #region IEntityEnumerator members

        /// <summary>
        /// Total number of bit strings in this enumerator. For cyclic intervals
        /// it is <c>(maxLength - minLength + 1)*categoriesCount</c> when the 
        /// <c>maxLength</c> is greater then <c>categoriesCount</c>, 
        /// <c>(maxLength - minLength)*categoriesCount + 1</c> otherwise. 
        /// </summary>
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

        #endregion

        /// <summary>
        /// Returns text representation of the cyclic intervals coefficient enumerator, 
        /// which is in form <c>ATTRIBUTE Cyclic Intervals[minLenth - maxLength]</c>
        /// </summary>
        /// <returns>Text representation of the cyclic intervals coefficient</returns>
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

    /// <summary>
    /// Subsets coefficients entity enumerator
    /// </summary>
    public class Subsets : EntityEnumerableCoefficient, SubsetsInstance<IBitString, IBitString>
    {
        /// <summary>
        /// Default constructor of the class
        /// </summary>
        /// <param name="setting">Coefficient setting</param>
        /// <param name="skipOptimalization">The skip steps optimalization</param>
        /// <param name="cedentType">Type of the cedent</param>
        public Subsets(CoefficientSetting setting, ISkipOptimalization skipOptimalization, MarkEnum cedentType)
            : base(setting, skipOptimalization, cedentType)
        {
            Debug.Assert(setting.coefficientType == CoefficientTypeEnum.Subsets);
            //UNDONE integritni omezeni (ordinal...)
        }

        /// <summary>
        /// Retrieves the entity enumerator - the subsets of all the lengths from
        /// <c>minLenght</c> to <c>maxLength</c>. The enumerator uses the
        /// <see cref="Ferda.Guha.MiningProcessor.Generation.Subsets&lt;T,M&gt;"/> type
        /// to compute the subsets.
        /// </summary>
        /// <returns>Entity enumerator</returns>
        public override IEnumerator<IBitString> GetBitStringEnumerator()
        {
            Subsets<IBitString, IBitString> enumerator =
                new Subsets<IBitString, IBitString>(_effectiveMinLength, _effectiveMaxLength, _categoriesNames.Length,
                                                    this);
            return enumerator.GetEnumerator();
        }

        #region IEntityEnumerator members

        /// <summary>
        /// Total number of bit strings in this enumerator. For subsets
        /// it is sum of <c>categoriesCount choose i</c>, where <c>i</c>
        /// goes from <c>minLength</c> to <c>maxLength</c>.
        /// </summary>
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

        #endregion

        /// <summary>
        /// Returns text representation of the subsets coefficient enumerator, 
        /// which is in form <c>ATTRIBUTE Subsets [minLenth - maxLength]</c>
        /// </summary>
        /// <returns>Text representation of the subsets coefficient</returns>
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

        /// <summary>
        /// The skip optimalization implementation
        /// </summary>
        /// <param name="current">The current bit string</param>
        /// <returns>If current bit string should be returned or not</returns>
        public bool skipOptimize(IBitString current)
        {
            SkipSetting parentSkipSetting = ParentSkipOptimalization.BaseSkipSetting(CedentType);
            if (parentSkipSetting != null)
            {
                return Common.Compare(parentSkipSetting.Relation,
                    current.Sum, parentSkipSetting.Treshold);
            }
            else
            {
                return true;
            }
        }

        #endregion
    }

    /// <summary>
    /// Subsets [1-1] coefficient enumerator. This special enumerator is
    /// used with virtual attributes (where only coefficient of type SS[1-1]
    /// makes sense). 
    /// </summary>
    public class SubsetsOneOne : EntityEnumerable
    {
        /// <summary>
        /// The setting of the fixed set coeffiecient.
        /// </summary>
        private readonly CoefficientSetting _setting;

        /// <summary>
        /// Count of the bit strings of virtual attribute. The number is equal to
        /// maximal number of bitstrings that a virtual attribute can generate
        /// (set by the user via property of the box).
        /// </summary>
        private long _totalCount = 2;

        /// <summary>
        /// Identification of the attribute (where the fixed set is constructed)
        /// </summary>
        private string _attributeGuid;

        /// <summary>
        /// How many bit strings are in buffer
        /// </summary>
        private int bufferedCount = 0;

        /// <summary>
        /// The instance of bit string buffer for multirelational GUHA procedures.
        /// </summary>
        BitStringBuffer _bufferInstance = null;

        /// <summary>
        /// Default constructor of the class. 
        /// </summary>
        /// <param name="setting">Coefficient setting</param>
        /// <param name="skipOptimalization">The skip steps optimalization</param>
        /// <param name="cedentType">Type of the cedent</param>
        public SubsetsOneOne(CoefficientSetting setting, ISkipOptimalization skipOptimalization, MarkEnum cedentType)
            : base(setting.id, skipOptimalization, cedentType)
        {
            _setting = setting;
            _attributeGuid = setting.generator.GetAttributeId().value;
            _totalCount = setting.generator.GetMaxBitStringCount();
            _bufferInstance = BitStringBuffer.GetInstance();

            if (!_bufferInstance.GuidPresent(_attributeGuid))
            {
                _bufferInstance.Reset();
                _bufferInstance.AddGuid(_attributeGuid);
            }
            else
            {
                bufferedCount = _bufferInstance.Count;
            }

        }

        /// <summary>
        /// Retrieves the entity enumerator. The method returns bit strings of the
        /// virtual hypotheses attribute. It takes first bit strings from the buffer
        /// and then it 
        /// </summary>
        /// <returns>Entity enumerator</returns>
        public override IEnumerator<IBitString> GetBitStringEnumerator()
        {
            int currentBitString = 0;
            while (true)
            {
                if (currentBitString < bufferedCount)
                {
                    BitStringIceWithCategoryId _bs =
                        _bufferInstance.GetBitString(currentBitString);
                    CrispBitStringIce crisp = _bs.bitString as CrispBitStringIce;
                    yield return new BitString(
                                new BitStringIdentifier(
                                _attributeGuid, _bs.categoryId),
                                crisp.length,
                                crisp.value);

                    currentBitString++;
                }
                else
                {
                    BitStringIceWithCategoryId tempString = null;
                    if (_setting.generator.GetNextBitString(bufferedCount, out tempString))
                    {
                        if (tempString != null)
                        {
                            CrispBitStringIce crisp = tempString.bitString as CrispBitStringIce;
                            IBitString result = new BitString(
                                new BitStringIdentifier(
                                _attributeGuid, tempString.categoryId),
                                crisp.length,
                                crisp.value);

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
                            if (_bufferInstance == null)
                            {
                                throw new ArgumentNullException("bitStringBuffer is null");
                            }

                            if (_bufferInstance.AddBitString(tempString))
                            {
                                bufferedCount++;
                            }
                            
                            _totalCount++;
                            currentBitString++;
                        }
                    }
                    else
                    {
                        yield break;
                    }
                }
            }

        }

        #region IEntityEnumerator members

        /// <summary>
        /// Count of the bit strings of virtual attribute. The number is equal to
        /// maximal number of bitstrings that a virtual attribute can generate
        /// (set by the user via property of the box).
        /// </summary>
        public override long TotalCount
        {
            get { return _totalCount; }
        }

        /// <summary>
        /// Set of used attributes by enumerator. For the fixed set enumerator
        /// it is the set containing only one virtual attribute
        /// </summary>
        public override Set<string> UsedAttributes
        {
            get { return new Set<string>(_attributeGuid); }
        }

        #endregion

        /// <summary>
        /// Returns text representation of the subsets coefficient enumerator, 
        /// which is in form <c>ATTRIBUTE Virtual attribute - subsets [1-1]</c>.
        /// </summary>
        /// <returns>Text representation of the subsets coefficient</returns>
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
            result += "Virtual attribute - subsets [1-1]";
            return result;
        }
    }
}