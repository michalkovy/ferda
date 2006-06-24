#define Testing
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Ferda.Guha.Math;
using Ferda.Guha.MiningProcessor.BitStrings;

namespace Ferda.Guha.MiningProcessor.Generation
{
    #region Subsets
    internal class LongMultiplicationArraySubsetsInstance : SubsetsInstance<long, long>
    {
        private long [] _items;
        public LongMultiplicationArraySubsetsInstance(long[] items)
        {
            _items = items;
        }

        #region SubsetsInstance<int,int[]> Members

        public long operation(long previous, long current)
        {
            return previous*current;
        }

        public long operation(long current)
        {
            return current;
        }

        public long getItem(int index)
        {
            return _items[index];
        }

        public long getDefaultInit()
        {
            return 0;
        }

        #endregion
    }

    public interface SubsetsInstance<T, M>
    {
        M operation(M previous, T current);
        M operation(T current);
        T getItem(int index);
        M getDefaultInit();
    }

    public class Subsets<T, M> : IEnumerable<M>
    {
        private int _effectiveMinLength;
        private int _effectiveMaxLength;
        private int _itemsCount;
        private SubsetsInstance<T, M> _instance;
        public Subsets(int effectiveMinLength, int effectiveMaxLength, int itemsCount, SubsetsInstance<T, M> instance)
        {
            _effectiveMinLength = effectiveMinLength;
            _effectiveMaxLength = effectiveMaxLength;
            _itemsCount = itemsCount;
            _instance = instance;
        }

        Stack<M> sB = new Stack<M>();
        Stack<int> sI = new Stack<int>();
        private void sBPush(T adding)
        {
            if (sB.Count > 0)
            {
                M previous = sB.Peek();
                sB.Push(_instance.operation(previous, adding));
            }
            else
            {
                sB.Push(_instance.operation(adding));
            }
        }
        private bool returnCurrent(out M result)
        {
            Debug.Assert(sB.Count <= _effectiveMaxLength);
            if (sB.Count >= _effectiveMinLength)
            {
                result = sB.Peek();
                return true;
            }
            result = _instance.getDefaultInit();
            return false;
        }
        private void getEntity(int index)
        {
            sBPush(_instance.getItem(index));
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
            if (newIndex >= _itemsCount)
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

        #region IEnumerable<M> Members

        public IEnumerator<M> GetEnumerator()
        //public override IEnumerator<IBitString> GetBitStringEnumerator()
        {
            M result;
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

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    } 
    #endregion
    
    public abstract class EntityEnumerable : IEntityEnumerator
    {
        /// <summary>
        /// Unique identifier of entity.
        /// </summary>
        private Guid _id;

        public Guid Id
        {
            get { return _id; }
        }

        private EntityEnumerable()
        {
        }

        public EntityEnumerable(Guid id)
        {
            _id = id;
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

        public abstract long TotalCount { get;}
    }

    public abstract class EntityEnumerableCoefficient : EntityEnumerable
    {
        protected readonly CoefficientSetting _setting;

        public EntityEnumerableCoefficient(CoefficientSetting setting)
            : base(new Guid(setting.id.value))
        {
            _setting = setting;

            _cache = BitStringCache.GetInstance(setting.generator);
#if Testing
            _attributeId = new Guid(setting.id.value);
#else
            _attributeId = setting.generator.GetAttributeId();
#endif
            _categoriesNames = _cache.GetCategoriesIds(_attributeId);

            // TODO integrini omezeni
            _effectiveMaxLength = System.Math.Min(System.Math.Max(1, _setting.maxLength), _categoriesNames.Length);

            // TODO integrini omezeni
            _effectiveMinLength = System.Math.Min(System.Math.Max(1, _setting.minLength), _effectiveMaxLength);
        }

        // >= 1
        protected int _effectiveMinLength;

        // <= categories.Count, >= 1
        protected int _effectiveMaxLength;

        protected Guid _attributeId;

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
            return _cache[_attributeId, categoryName];
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
    }

    public abstract class SingleOperandEntity : EntityEnumerable
    {
        protected readonly ISingleOperandEntitySetting _setting;

        protected readonly IEntityEnumerator _entity;

        public SingleOperandEntity(ISingleOperandEntitySetting setting)
            : base(new Guid(setting.id.value))
        {
            _setting = setting;
            _entity = Factory.Create(_setting.operand);
        }
    }

    public abstract class MutliOperandEntity : EntityEnumerable
    {
        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="setting">The setting.</param>
        public MutliOperandEntity(IMultipleOperandEntitySetting setting)
            : base(new Guid(setting.id.value))
        {
            if (setting == null)
                throw new ArgumentNullException("setting", "The reference to setting cannot be null.");

            _setting = setting;

            // sort source entities by importance
            List<IEntityEnumerator> forcedEnts = new List<IEntityEnumerator>();
            List<IEntityEnumerator> basicEnts = new List<IEntityEnumerator>();
            List<IEntityEnumerator> auxiliaryEnts = new List<IEntityEnumerator>();
            foreach (IEntitySetting operand in _setting.operands)
            {
                IEntityEnumerator tmpEnt = Factory.Create(operand);
                // TODO constructors in factory or something like that
                switch (operand.importance)
                {
                    case ImportanceEnum.Forced:
                        forcedEnts.Add(tmpEnt);
                        break;
                    case ImportanceEnum.Basic:
                        basicEnts.Add(tmpEnt);
                        break;
                    case ImportanceEnum.Auxiliary:
                        auxiliaryEnts.Add(tmpEnt);
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }
            _sourceEntities.AddRange(forcedEnts);
            _sourceEntities.AddRange(basicEnts);
            _sourceEntities.AddRange(auxiliaryEnts);

            // max(1, max(number of forced operands, min length param))
            _effectiveMinLength = System.Math.Max(1, System.Math.Max(forcedEnts.Count, _setting.minLength));

            // min(number of operands, max length param)
            _effectiveMaxLength = System.Math.Min(_sourceEntities.Count, _setting.maxLength);

            if (_effectiveMaxLength < _effectiveMinLength)
                throw new ArgumentException("Effective MinLength is greather than MaxLength");

            //TODO
            //_setting.ClassOfEquivalence
        }

        #endregion

        protected abstract IBitString operation(IBitString operand1, IBitString operand2);

        #region Fields

        protected List<IEntityEnumerator> _sourceEntities = new List<IEntityEnumerator>();

        // max(1, number of forced operands, min length param))
        private int _effectiveMinLength;

        // min(number of operands, max length param)
        //////// _effectiveMaxLength can be less then _effectiveMinLength, but only in the case that (_effectiveMaxLength == 0) and thus this enumerator i.e. MoveNext always returns false
        private int _effectiveMaxLength;

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
                        new LongMultiplicationArraySubsetsInstance(totalCounts.ToArray())
                    );
                foreach (long l in subsets)
                {
                    _totalCount += l;
                }

                return _totalCount;
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

        Stack<IEnumerator<IBitString>> sE = new Stack<IEnumerator<IBitString>>();
        Stack<IBitString> sB = new Stack<IBitString>();
        Stack<int> sI = new Stack<int>();
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
            Debug.Assert(enumerator.MoveNext());
            sE.Push(enumerator);
            sBPush(enumerator.Current);
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
            if (newIndex >= _sourceEntities.Count)
                return false;
            getEntity(newIndex);
            return true;
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
            ;
        }
    }
}