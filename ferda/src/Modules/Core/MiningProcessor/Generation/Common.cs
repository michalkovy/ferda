// Common.cs - common abstract classes for entity enumerators
//
// Authors: Tomáš Kuchař <tomas.kuchar@gmail.com>      
// Commented by: Martin Ralbovský <martin.ralbovsky@gmail.com>
//
// Copyright (c) 2006 Tomáš Kuchař
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
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Ferda.Guha.MiningProcessor.BitStrings;
using Ferda.Modules;
using Ferda.Modules.Helpers.Caching;
using Ferda.Modules.Helpers.Common;

namespace Ferda.Guha.MiningProcessor.Generation
{
    /// <summary>
    /// Basic abstract class for entity enumerables. 
    /// </summary>
    public abstract class EntityEnumerable : IEntityEnumerator
    {
        #region Fields

        /// <summary>
        /// Unique identifier of entity enumerator. The enumerator gets
        /// it from the entity (setting) that uses the enumerator.
        /// </summary>
        private readonly string _guid;

        /// <summary>
        /// Cedent type of this entity enumerator
        /// </summary>
        private readonly MarkEnum _cedentType;

        /// <summary>
        /// The skip steps optimalization
        /// </summary>
        private readonly ISkipOptimalization _parentSkipOptimalization;

        #endregion 

        #region Properties

        /// <summary>
        /// Unique identifier of entity enumerator. The enumerator gets
        /// it from the entity (setting) that uses the enumerator.
        /// </summary>
        public string Guid
        {
            get { return _guid; }
        }

        /// <summary>
        /// Cedent type of this entity enumerator
        /// </summary>
        public MarkEnum CedentType
        {
            get { return _cedentType; }
        }

        /// <summary>
        /// The skip steps optimalization
        /// </summary>
        public ISkipOptimalization ParentSkipOptimalization
        {
            get { return _parentSkipOptimalization; }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Protected constructor for the derived classes
        /// </summary>
        /// <param name="id">Identifier of the entity enumerator</param>
        /// <param name="skipOptimalization">Skip steps optimalization</param>
        /// <param name="cedentType">Type of cedent of the enumerator</param>
        protected EntityEnumerable(GuidStruct id, ISkipOptimalization skipOptimalization, MarkEnum cedentType)
        {
            _guid = id.value;
            _cedentType = cedentType;
            _parentSkipOptimalization = skipOptimalization;
        }

        #endregion 

        /// <summary>
        /// Retrieves the entity enumerator
        /// </summary>
        /// <returns>Entity enumerator</returns>
        public abstract IAsyncEnumerator<IBitString> GetBitStringEnumerator();

        #region IAsyncEnumerable<IBitString> Members

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        public IAsyncEnumerator<IBitString> GetAsyncEnumerator(CancellationToken cancellationToken = default)
        {
            return GetBitStringEnumerator();
        }

        #endregion

        #region IEntityEnumerator members

        /// <summary>
        /// Total number of bit strings in this enumerator
        /// </summary>
        public abstract long TotalCount { get; }

        #endregion
    }

    /// <summary>
    /// Abstract class for coefficient enumerators
    /// </summary>
    public abstract class EntityEnumerableCoefficient : EntityEnumerable
    {
        #region Protected fields

        /// <summary>
        /// The coefficient setting
        /// </summary>
        protected readonly CoefficientSetting _setting;

        /// <summary>
        /// The effective minimal length of the coefficient. It has to be greater
        /// then 1. 
        /// </summary>
        protected int _effectiveMinLength;

        /// <summary>
        /// The effecitive maximal length, has to be greater than one and less then
        /// total count of categories of the attribute
        /// </summary>
        protected int _effectiveMaxLength;

        /// <summary>
        /// Identification of the attribute
        /// </summary>
        protected string _attributeGuid;

        /// <summary>
        /// Names of the categories of the attribute
        /// </summary>
        protected string[] _categoriesNames;

        /// <summary>
        /// Current processed bit string
        /// </summary>
        protected IBitString _currentBitString = null;

        /// <summary>
        /// The bit string cache
        /// </summary>
        protected IBitStringCache _cache;

        /// <summary>
        /// Actual length of the coefficient
        /// </summary>
        protected int _actualLength = 0;

        /// <summary>
        /// Resets the coefficient
        /// </summary>
        protected void resetCoefficient()
        {
            _currentBitString = null;
            _actualLength = 0;
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Protected constructor for the derived classes. It performs
        /// initializations and checks the length of the coefficient.
        /// </summary>
        /// <param name="setting">Coefficient setting</param>
        /// <param name="skipOptimalization">Skip step optimalization</param>
        /// <param name="cedentType">The type of the cedent</param>
        protected EntityEnumerableCoefficient(CoefficientSetting setting, ISkipOptimalization skipOptimalization, MarkEnum cedentType)
            : base(setting.id, skipOptimalization, cedentType)
        {
            _setting = setting;

            _cache = BitStringCache.GetInstance(setting.generator);
#if Testing
            _attributeGuid = Guid;
#else
            _attributeGuid = setting.generator.GetAttributeId().value;
#endif
            _categoriesNames = _cache.GetCategoriesIds(_attributeGuid);

            _effectiveMaxLength = System.Math.Min(System.Math.Max(1, _setting.maxLength), _categoriesNames.Length);
            _effectiveMinLength = System.Math.Min(System.Math.Max(1, _setting.minLength), _effectiveMaxLength);
            if (_effectiveMaxLength < _effectiveMinLength)
                throw Exceptions.MaxLengthIsLessThanMinLengthError();
        }

        #endregion

        /// <summary>
        /// Gets bit string of a speciffied category (attribute is
        /// contained in the coefficient setting)
        /// </summary>
        /// <param name="categoryName">Name of the desired category</param>
        /// <returns>Bit string representing the category</returns>
        protected Task<IBitString> getBitStringAsync(string categoryName)
        {
            return _cache.GetValueAsync(_attributeGuid, categoryName);
        }

        /// <summary>
        /// Prolongs the coefficient. If the coefficient length is zero,
        /// it creates a new bit string, else it performs an OR operation
        /// on the existing bit string and new category bit string
        /// </summary>
        /// <param name="categoryName">The new category</param>
        protected async Task prolongCoefficient(string categoryName)
        {
            IBitString newBitString = await getBitStringAsync(categoryName).ConfigureAwait(false);

            if (_actualLength == 0)
            {
                Debug.Assert(_currentBitString == null);
                _currentBitString = newBitString;
            }
            //else if (_actualLength == 1)
            //{
            //    //_currentBitString = _currentBitString.OrCloned(newBitString);
            //    _currentBitString = _currentBitString.Or(newBitString);
            //}
            else
            {
                _currentBitString = _currentBitString.Or(newBitString);
            }
            _actualLength++;
        }
    }

    /// <summary>
    /// Abstract class for single operand entities enumerators(Sign)
    /// </summary>
    public abstract class SingleOperandEntity : EntityEnumerable, INotLeafEntityEnumerator
    {
        #region Protected fields

        /// <summary>
        /// The single operand entity setting
        /// </summary>
        protected readonly ISingleOperandEntitySetting _setting;

        /// <summary>
        /// The entity enumerator of the operand of this entity (enumerator)
        /// </summary>
        protected readonly IEntityEnumerator _entity;

        #endregion

        #region Constructor

        /// <summary>
        /// Protected constructor for the derived classes. 
        /// </summary>
        /// <param name="setting">Setting of this enumerator</param>
        /// <param name="skipOptimalization">Skip step optimalization</param>
        /// <param name="cedentType">Type of the cedent</param>
        protected SingleOperandEntity(ISingleOperandEntitySetting setting, ISkipOptimalization skipOptimalization, MarkEnum cedentType)
            : base(setting.id, skipOptimalization, cedentType)
        {
            _setting = setting;
            _entity = Factory.Create(_setting.operand, this, cedentType);
        }

        #endregion

        #region ISkipOptimalization members

        /// <summary>
        /// The base skip otrimalization settng
        /// </summary>
        /// <param name="cedentType">Type of the cedent</param>
        /// <returns>The base skip optimalization setting for the cedent</returns>
        public abstract SkipSetting BaseSkipSetting(MarkEnum cedentType);

        #endregion
    }

    /// <summary>
    /// Abstract class for multiple operand setting entities enumerators
    /// (Conjunction, Disjunction)
    /// </summary>
    public abstract class MutliOperandEntity : EntityEnumerable, INotLeafEntityEnumerator
    {   
        #region Constructor

        /// <summary>
        /// Protected constructor for the derived classes. 
        /// </summary>
        /// <param name="setting">The setting of this enumerator</param>
        /// <param name="skipOptimalization">The skip optimalization.</param>
        /// <param name="cedentType">Type of the cedent.</param>
        protected MutliOperandEntity(IMultipleOperandEntitySetting setting, 
            ISkipOptimalization skipOptimalization, MarkEnum cedentType)
            : base(setting.id, skipOptimalization, cedentType)
        {
            if (setting == null)
                throw new ArgumentNullException("setting", "The reference to setting cannot be null.");

            _setting = setting;

            // sort source entities by importance
            SortedList<double, IEntityEnumerator> forcedEnts = new SortedList<double, IEntityEnumerator>();
            SortedList<double, IEntityEnumerator> basicEnts = new SortedList<double, IEntityEnumerator>();
            SortedList<double, IEntityEnumerator> auxiliaryEnts = new SortedList<double, IEntityEnumerator>();

            //Here individual lists of importance operand entities are sorted.
            //The operands with lowest number of bit string come first, 
            //operantds with highest number come last
            foreach (IEntitySetting operand in _setting.operands)
            {
                IEntityEnumerator operandEnt = Factory.Create(operand, this, cedentType);
                double operandEntCount = operandEnt.TotalCount;
                switch (operand.importance)
                {
                    case ImportanceEnum.Forced:
                        while (forcedEnts.ContainsKey(operandEntCount))
                            operandEntCount *= 1.1d; //UNDONE
                        forcedEnts.Add(operandEntCount, operandEnt);
                        break;
                    case ImportanceEnum.Basic:
                        while (basicEnts.ContainsKey(operandEntCount))
                            operandEntCount *= 1.1d;
                        basicEnts.Add(operandEntCount, operandEnt);
                        break;
                    case ImportanceEnum.Auxiliary:
                        while (auxiliaryEnts.ContainsKey(operandEntCount))
                            operandEntCount *= 1.1d; //UNDONE
                        auxiliaryEnts.Add(operandEntCount, operandEnt);
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }

            //adding the sorted importance lists to the source entities list
            //TODO repair the adding mechanism - podle mne to nefunguje
            _forcedCount = forcedEnts.Count;
            if (_forcedCount > 0)
            	_sourceEntities.AddRange(forcedEnts.Values);
            _basicCount = basicEnts.Count;
			if (_basicCount > 0)
            	_sourceEntities.AddRange(basicEnts.Values);
			if (auxiliaryEnts.Values.Count > 0)
            	_sourceEntities.AddRange(auxiliaryEnts.Values);

            // max(0, max(number of forced operands, min length param))
            _effectiveMinLength = System.Math.Max(0, System.Math.Max(forcedEnts.Count, _setting.minLength));
            // min(number of operands, max length param)
            _effectiveMaxLength = System.Math.Min(_sourceEntities.Count, _setting.maxLength);
            if (_effectiveMaxLength < _effectiveMinLength)
                throw Exceptions.MaxLengthIsLessThanMinLengthError();

            _memoizedSourceAsyncEnumerables.AddRange(_sourceEntities.Select(e => e.Memoize()));
        }

        #endregion

        #region Private fields

        /// <summary>
        /// The multiple operand entity setting
        /// </summary>
        private readonly IMultipleOperandEntitySetting _setting;

        /// <summary>
        /// The effective minimal length. 
        /// max(0, number of forced operands, min length param))
        /// </summary>
        private readonly int _effectiveMinLength;

        /// <summary>
        /// The effective maximal length.
        /// min(number of operands, max length param) 
        /// </summary>
        private readonly int _effectiveMaxLength;

        /// <summary>
        /// Total number of bit strings in this enumerator
        /// </summary>
        private long _totalCount = -1;

        #endregion

        #region Protected fields

        /// <summary>
        /// List of entity enumerators of all operands
        /// (<c>Forced</c> and <c>Basic</c> and <c>Auxiliary</c>)
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")] 
        protected List<IEntityEnumerator> _sourceEntities = 
            new List<IEntityEnumerator>();

        protected List<IAsyncEnumerable<IBitString>> _memoizedSourceAsyncEnumerables = new List<IAsyncEnumerable<IBitString>>();

        /// <summary>
        /// Number of forced operands
        /// </summary>
        protected int _forcedCount = 0;

        /// <summary>
        /// Number of basic operands
        /// </summary>
        protected int _basicCount = 0;

        #endregion

        #region Properties

        /// <summary>
        /// The effective minimal length. 
        /// max(0, number of forced operands, min length param))
        /// </summary>
        public int EffectiveMinLength
        {
            get { return _effectiveMinLength; }
        }

        /// <summary>
        /// The effective maximal length.
        /// min(number of operands, max length param) 
        /// </summary>
        public int EffectiveMaxLength
        {
            get { return _effectiveMaxLength; }
        }

        #endregion

        #region IEntityEnumberable members

        /// <summary>
        /// Total number of bit strings in this enumerator
        /// </summary>        
        public override long TotalCount
        {
            get
            {
                lock (this)
                {
                    if (_totalCount >= 0)
                        return _totalCount;

                    _totalCount = 0;

                    // initialize
                    int count = _sourceEntities.Count;
                    List<long> totalCounts = new List<long>(_sourceEntities.Count);
                    foreach (IEntityEnumerator entity in _sourceEntities)
                    {
                        totalCounts.Add(entity.TotalCount);
                    }

                    Subsets<long, long> subsets =
                        new Subsets<long, long>(_effectiveMinLength,
                        _effectiveMaxLength, count,
                        new LongMultiplicationArraySubsetsInstance(totalCounts.ToArray())
                        );
                    foreach (long l in subsets)
                    {
                        _totalCount += l;
                    }
                }

                return _totalCount;
            }
        }

        #endregion

        #region ISkipOptimalization members

        /// <summary>
        /// The base skip otrimalization settng
        /// </summary>
        /// <param name="cedentType">Type of the cedent</param>
        /// <returns>The base skip optimalization setting for the cedent</returns>
        public abstract SkipSetting BaseSkipSetting(MarkEnum cedentType);

        #endregion

        #region IDisposable Members

        /// <summary>
        /// Performs application-defined tasks associated
        /// with freeing, releasing, or resetting unmanaged
        /// resources.
        /// </summary>
        public void Dispose()
        {
            // Do nothing
        }

        #endregion

        #region Private methods
        private async IAsyncEnumerable<IBitString> ConstructNextLevel(IBitString soFar, IBitString b, SkipSetting parentSkipSetting, IEnumerable<IAsyncEnumerable<IBitString>> enumerables, int depth, int i)
        {
            var newString = soFar == null ? b : operation(soFar, b, parentSkipSetting != null); //await Task.Run<IBitString>(() => soFar == null ? b : operation(soFar, b, parentSkipSetting != null));

            if (parentSkipSetting == null || Ferda.Guha.Math.Common.Compare(parentSkipSetting.Relation, newString.Sum, parentSkipSetting.Treshold))
            {
                if (EffectiveMinLength <= depth)
                    yield return newString;
                if (depth < EffectiveMaxLength)
                {
                    await foreach (var item in ReadParallel(enumerables.Skip(i + 1), depth + 1, newString))
                    {
                        yield return item;
                    }
                }
            }
        }

        private IAsyncEnumerable<IBitString> ReadParallel(IEnumerable<IAsyncEnumerable<IBitString>> enumerables, int depth, IBitString soFar)
        {
            SkipSetting parentSkipSetting = ParentSkipOptimalization.BaseSkipSetting(CedentType);
            var asyncEnums = enumerables
                .Select((e, i) =>
                    e.SelectMany(b => ConstructNextLevel(soFar, b, parentSkipSetting, enumerables, depth, i)));
            if (EffectiveMaxLength - depth < 3)
                return System.Linq.AsyncEnumerableEx.Merge(asyncEnums.ToArray()); // array merge is concurrent, IEnumerable isn't
            else
                return asyncEnums.Merge();
        }
        #endregion

        /// <summary>
        /// Retrieves the entity enumerator. The entity enumerator works on
        /// the basis of rather complicated automaton. The design of the
        /// automaton is described in 
        /// <c>ferda/docsrc/draft/multiOperandGenerationAutomaton.svg</c>.
        /// </summary>
        /// <returns>Entity enumerator</returns>
        public override IAsyncEnumerator<IBitString> GetBitStringEnumerator()
        {
            var enumerable = ReadParallel(_memoizedSourceAsyncEnumerables, 1, null);
            return enumerable.GetAsyncEnumerator();
        }

        /// <summary>
        /// The operation that is performed between the bit strings of operands
        /// </summary>
        /// <param name="operand1">First operand</param>
        /// <param name="operand2">Second operand</param>
        /// <param name="precomputeSum">should sum be precomputed during operation</param>
        /// <returns>Resulting bit string</returns>
        protected abstract IBitString operation(IBitString operand1, IBitString operand2, bool precomputeSum);
    }
}
