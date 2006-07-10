//#define Testing
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Ferda.Guha.MiningProcessor.BitStrings;
using Ferda.Modules;
using Ferda.Modules.Helpers.Common;

namespace Ferda.Guha.MiningProcessor.Generation
{
    public abstract class EntityEnumerable : IEntityEnumerator
    {
        /// <summary>
        /// Unique identifier of entity.
        /// </summary>
        private string _guid;

        public string Guid
        {
            get { return _guid; }
        }

        private EntityEnumerable()
        {
        }

        protected EntityEnumerable(GuidStruct id)
        {
            _guid = id.value;
        }

        public abstract IEnumerator<IBitString> GetBitStringEnumerator();

        #region IEnumerable<IBitString> Members

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"></see> 
        /// that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<IBitString> GetEnumerator()
        {
            return GetBitStringEnumerator();
        }

        #endregion

        #region IEnumerable Members

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"></see> object 
        /// that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetBitStringEnumerator();
        }

        #endregion

        public abstract long TotalCount { get; }

        public abstract Set<string> UsedAttributes { get; }

        public abstract Set<string> UsedEntities { get; }
    }

    public abstract class EntityEnumerableCoefficient : EntityEnumerable
    {
        protected readonly CoefficientSetting _setting;

        protected EntityEnumerableCoefficient(CoefficientSetting setting)
            : base(setting.id)
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

        // >= 1
        protected int _effectiveMinLength;

        // <= categories.Count, >= 1
        protected int _effectiveMaxLength;

        protected string _attributeGuid;

        protected string[] _categoriesNames;

        protected IBitString _currentBitString = null;

        protected IBitStringCache _cache;

        protected int _actualLength = 0;

        protected void resetCoefficient()
        {
            _currentBitString = null;
            _actualLength = 0;
        }

        protected IBitString getBitString(string categoryName)
        {
            return _cache[_attributeGuid, categoryName];
        }

        protected void prolongCoefficient(string categoryName)
        {
            IBitString newBitString = getBitString(categoryName);

            _actualLength++;

            if (_currentBitString == null)
            {
                Debug.Assert(_actualLength == 1);
                _currentBitString = newBitString;
            }
            else
                _currentBitString = _currentBitString.Or(newBitString);
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

    public abstract class SingleOperandEntity : EntityEnumerable
    {
        protected readonly ISingleOperandEntitySetting _setting;

        protected readonly IEntityEnumerator _entity;

        protected SingleOperandEntity(ISingleOperandEntitySetting setting)
            : base(setting.id)
        {
            _setting = setting;
            _entity = Factory.Create(_setting.operand);
        }

        public override Set<string> UsedAttributes
        {
            get { return _entity.UsedAttributes; }
        }

        public override Set<string> UsedEntities
        {
            get { return _entity.UsedEntities; }
        }
    }

    public abstract class MutliOperandEntity : EntityEnumerable
    {
        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="setting">The setting.</param>
        protected MutliOperandEntity(IMultipleOperandEntitySetting setting)
            : base(setting.id)
        {
            if (setting == null)
                throw new ArgumentNullException("setting", "The reference to setting cannot be null.");

            _setting = setting;

            // sort source entities by importance
            SortedList<double, IEntityEnumerator> forcedEnts = new SortedList<double, IEntityEnumerator>();
            SortedList<double, IEntityEnumerator> basicEnts = new SortedList<double, IEntityEnumerator>();
            SortedList<double, IEntityEnumerator> auxiliaryEnts = new SortedList<double, IEntityEnumerator>();
            foreach (IEntitySetting operand in _setting.operands)
            {
                IEntityEnumerator tmpEnt = Factory.Create(operand);
                double tmpEntCount = tmpEnt.TotalCount;
                switch (operand.importance)
                {
                    case ImportanceEnum.Forced:
                        while (forcedEnts.ContainsKey(tmpEntCount))
                            tmpEntCount *= 1.0000001d;
                        forcedEnts.Add(tmpEntCount, tmpEnt);
                        break;
                    case ImportanceEnum.Basic:
                        while (basicEnts.ContainsKey(tmpEntCount))
                            tmpEntCount *= 1.0000001d;
                        basicEnts.Add(tmpEntCount, tmpEnt);
                        break;
                    case ImportanceEnum.Auxiliary:
                        while (auxiliaryEnts.ContainsKey(tmpEntCount))
                            tmpEntCount *= 1.0000001d;
                        auxiliaryEnts.Add(tmpEntCount, tmpEnt);
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }
            _forcedCount = forcedEnts.Count;
            _sourceEntities.AddRange(forcedEnts.Values);
            _basicCount = basicEnts.Count;
            _sourceEntities.AddRange(basicEnts.Values);
            _sourceEntities.AddRange(auxiliaryEnts.Values);

            // max(0, max(number of forced operands, min length param))
            _effectiveMinLength = System.Math.Max(0, System.Math.Max(forcedEnts.Count, _setting.minLength));
            // min(number of operands, max length param)
            _effectiveMaxLength = System.Math.Min(_sourceEntities.Count, _setting.maxLength);
            if (_effectiveMaxLength < _effectiveMinLength)
                throw Exceptions.MaxLengthIsLessThanMinLengthError();

            // prepare classes of equivalence
            if (_setting.classesOfEquivalence != null
                && _setting.classesOfEquivalence.Length > 0)
            {
                Set<Set<string>> cof = new Set<Set<string>>();
                foreach (GuidStruct[] structs in _setting.classesOfEquivalence)
                {
                    if (structs != null && structs.Length > 0)
                    {
                        Set<string> set = new Set<string>();
                        foreach (GuidStruct guidStruct in structs)
                        {
                            if (guidStruct != null)
                                set.Add(guidStruct.value);
                        }
                        if (cof.Count > 0)
                        {
                            // if set is {A, B} and some previous set was {B, C}, only union will be added ie.e. {A, B, C}
                            // test disjunctivity with other classes of equivalence
                            List<Set<string>> inCollision = new List<Set<string>>();

                            foreach (Set<string> previousSet in cof)
                            {
                                foreach (string guid in set)
                                {
                                    if (previousSet.Contains(guid))
                                    {
                                        inCollision.Add(previousSet);
                                        break;
                                    }
                                }
                            }
                            foreach (Set<string> col in inCollision)
                            {
                                set.AddRange(col);
                                cof.Remove(col);
                            }
                        }
                        cof.Add(set);
                    }
                }
                _classesOfEquivalence = cof;
            }
        }

        private Set<Set<string>> _classesOfEquivalence;

        #endregion

        protected abstract IBitString operation(IBitString operand1, IBitString operand2);

        #region Fields

        protected int _forcedCount = 0;
        protected int _basicCount = 0;

        [SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")] protected List<IEntityEnumerator>
            _sourceEntities = new List<IEntityEnumerator>();

        // max(0, number of forced operands, min length param))
        private readonly int _effectiveMinLength;

        public int EffectiveMinLength
        {
            get { return _effectiveMinLength; }
        }

        // min(number of operands, max length param)
        private readonly int _effectiveMaxLength;

        public int EffectiveMaxLength
        {
            get { return _effectiveMaxLength; }
        }

        private readonly IMultipleOperandEntitySetting _setting;

        #endregion

        private long _totalCount = -1;

        public override long TotalCount
        {
            get
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

                Subsets<long, long> subsets = new Subsets<long, long>(_effectiveMinLength, _effectiveMaxLength, count,
                                                                      new LongMultiplicationArraySubsetsInstance(
                                                                          totalCounts.ToArray())
                    );
                foreach (long l in subsets)
                {
                    _totalCount += l;
                }

                return _totalCount;
            }
        }

        public override Set<string> UsedAttributes
        {
            get
            {
                if (sE.Count == 0)
                    return new Set<string>();
                else
                {
                    Set<string> result = null;
                    foreach (IEnumerator<IBitString> enumerator in sE)
                    {
                        if (result == null)
                            result = ((IEntityEnumerator) enumerator).UsedAttributes;
                        else
                            result.AddRange(((IEntityEnumerator) enumerator).UsedAttributes);
                    }
                    return result;
                }
            }
        }

        public override Set<string> UsedEntities
        {
            get
            {
                if (sE.Count == 0)
                    return new Set<string>(Guid);
                else
                {
                    Set<string> result = null;
                    foreach (IEnumerator<IBitString> enumerator in sE)
                    {
                        if (result == null)
                            result = ((IEntityEnumerator) enumerator).UsedAttributes;
                        else
                            result.AddRange(((IEntityEnumerator) enumerator).UsedAttributes);
                    }
                    result.Add(Guid);
                    return result;
                }
            }
        }

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

        private Stack<IEnumerator<IBitString>> sE = new Stack<IEnumerator<IBitString>>();
        private Stack<IBitString> sB = new Stack<IBitString>();
        private Stack<int> sI = new Stack<int>();

        private void sBPush(IBitString adding)
        {
            if (sB.Count > 0)
            {
                IBitString previous = sB.Peek();
                sB.Push(operation(previous, adding));
            }
            else
            {
                sB.Push(adding);
            }
        }

        private bool moveNextInTopEntity()
        {
            IEnumerator<IBitString> enumerator = sE.Peek();
            if (enumerator.MoveNext())
            {
                if (sB.Count > 0)
                    sB.Pop();
                sBPush(enumerator.Current);
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool returnCurrent(out IBitString result)
        {
            // TODO classes of equivalence ... rozmyslet a dodelat

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
            IEnumerator<IBitString> enumerator = _sourceEntities[index].GetEnumerator();

            //enumerator.Reset();
            // UNDONE ... myslim, ze by to melo byt odkomentovane, ale 
            // kdyz je to odkomentovane tak to zde pada na NotSupprotedOperation

            Debug.Assert(enumerator.MoveNext());
            sE.Push(enumerator);
            sBPush(enumerator.Current);
            sI.Push(index);
        }

        private bool prolong(bool afterRemove)
        {
            restart:
            if (sB.Count == _effectiveMaxLength) // not after remove
                return false;
            int newIndex;
            if (afterRemove)
            {
                if (sI.Count == 1)
                {
                    // switching first member of conjunction
                    if (_forcedCount > 0)
                    {
                        // forced entities are defined 
                        // ! but forced entity can not be removed
                        // => end iteration
                        Debug.Assert(sB.Count == 0); //because after remove && sI.Count == 1
                        return false;
                    }
                    if (sI.Peek() >= _forcedCount + _basicCount - 1)
                    {
                        // index of next entity is index of auxiliary entity
                        // i.e. all following entities are auxiliary
                        // ! but output can not be created only from auxiliary entities
                        // => end iteration
                        Debug.Assert(sB.Count == 0); //because after remove && sI.Count == 1
                        return false;
                    }
                }
                newIndex = sI.Pop() + 1;
            }
            else
                newIndex = sI.Peek() + 1;
            if (newIndex >= _sourceEntities.Count)
                return false;
            // test if the classes of equivalece
            if (classOfEquivalenceCollision(newIndex))
                goto restart;
            getEntity(newIndex);
            return true;
        }

        private Set<string> getClassOfEquivalence(Set<string> set)
        {
            if (_classesOfEquivalence == null || _classesOfEquivalence.Count == 0)
                return null;
            if (set == null || set.Count == 0)
                return null;
            foreach (Set<string> set1 in _classesOfEquivalence)
            {
                foreach (string guid in set)
                {
                    if (set1.Contains(guid))
                        return set1;
                }
            }
            return null;
        }

        private bool classOfEquivalenceCollision(int newIndex)
        {
            if (_classesOfEquivalence == null || _classesOfEquivalence.Count == 0)
                return false;

            Set<string> usedClassOfEquiv = getClassOfEquivalence(UsedEntities);
            if (usedClassOfEquiv == null)
                return false;

            Set<string> newItemClassOfEquiv =
                getClassOfEquivalence((_sourceEntities[newIndex]).UsedEntities);
            if (newItemClassOfEquiv == null)
                return false;

            if (usedClassOfEquiv == newItemClassOfEquiv)
                return true;
            return false;
        }

        private bool removeLastItem()
        {
            if (sE.Count > 0)
            {
                sE.Pop();
                sB.Pop();
                return true;
            }
            return false;
        }

        //private void debugTest()
        //{
        //    Debug.Assert(sE.Count == sB.Count);
        //}
        public override IEnumerator<IBitString> GetBitStringEnumerator()
        {
            if (_effectiveMinLength == 0)
                yield return EmptyBitString.GetInstance();

            IBitString result;
            bool afterRemove;
            afterRemove = false;

            #region initialize

            sE.Clear();
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
            else if (afterRemove)
            {
                if (sB.Count == 0)
                    goto finish;
                afterRemove = false;
                if (moveNextInTopEntity())
                    goto returnCurrent;
            }
            while (moveNextInTopEntity())
            {
                if (returnCurrent(out result))
                    yield return result;
            }
            if (removeLastItem())
            {
                afterRemove = true;
                goto prolong;
            }
            finish:
            sE.Clear();
            sB.Clear();
            sI.Clear();
            ;
        }
    }
}