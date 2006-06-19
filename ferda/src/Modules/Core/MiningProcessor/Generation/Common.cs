#define Testing
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Ferda.Guha.MiningProcessor.BitStrings;

namespace Ferda.Guha.MiningProcessor.Generation
{
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
    }

    public abstract class EntityEnumerableCoefficient : EntityEnumerable
    {
        private readonly CoefficientSetting _setting;

        public EntityEnumerableCoefficient(CoefficientSetting setting)
            : base(new Guid(setting.id.value))
        {
            _setting = setting;

            _bitStringCache = BitStringCache.GetInstance(setting.generator);
#if Testing
            _attributeId = new Guid(setting.id.value);
#else
            _attributeId = setting.generator.GetAttributeId();
#endif
            _categoriesNames = _bitStringCache.GetCategoriesIds(_attributeId);

            // TODO integrini omezeni
            _effectiveMaxLength = System.Math.Min(System.Math.Max(1, _setting.maxLenght), _categoriesNames.Length);

            // TODO integrini omezeni
            _effectiveMinLength = System.Math.Min(System.Math.Max(1, _setting.minLenght), _effectiveMaxLength);
        }

        // >= 1
        protected int _effectiveMinLength;

        // <= categories.Count, >= 1
        protected int _effectiveMaxLength;

        protected Guid _attributeId;

        protected string[] _categoriesNames;

        protected IBitString _currentBitString = null;

        protected IBitStringCache _bitStringCache;

        protected int _actualLenght = 0;

        protected void resetCoefficient()
        {
            _currentBitString = null;
            _actualLenght = 0;
        }

        protected void prolongCoefficient(string categoryName)
        {
            IBitString newBitString =
                _bitStringCache.GetBitString(new BitStringIdentifier(_attributeId, categoryName));

            _actualLenght++;

            if (_currentBitString == null)
            {
                Debug.Assert(_actualLenght == 1);
                _currentBitString = newBitString;
            }
            else
                _currentBitString = _currentBitString.Or(newBitString);
        }

        public abstract long TotalCount { get;}
    }

    public abstract class EntityEnumerator : EntityEnumerable, IEnumerator<IBitString>
    {
        protected IBitString _currentBitString;

        public EntityEnumerator(Guid id)
            : base(id)
        {
        }

        #region IEnumerator<IBitString> Members

        /// <summary>
        /// Gets the element in the collection at the current 
        /// position of the enumerator.
        /// </summary>
        /// <value></value>
        /// <returns>
        /// The element in the collection at the current 
        /// position of the enumerator.
        /// </returns>
        public IBitString Current
        {
            get
            {
                if (_currentBitString == null)
                    throw new InvalidOperationException("Invalid position of the enumerator.");

                return _currentBitString;
            }
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
        }

        #endregion

        #region IEnumerator Members

        /// <summary>
        /// Gets the element in the collection at the current 
        /// position of the enumerator.
        /// </summary>
        /// <value></value>
        /// <returns>
        /// The element in the collection at the current 
        /// position of the enumerator.
        /// </returns>
        object IEnumerator.Current
        {
            get
            {
                if (_currentBitString == null)
                    throw new InvalidOperationException("Invalid position of the enumerator.");

                return _currentBitString;
            }
        }

        public abstract bool MoveNext();

        public void Reset()
        {
            _currentBitString = null;
        }

        #endregion

        public override IEnumerator<IBitString> GetBitStringEnumerator()
        {
            return this;
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

    /// <summary>
    /// Contains a trace of bit string enumerator entity.
    /// </summary>
    public abstract class MutliOperandEntity : EntityEnumerator
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
            _effectiveMinLength = System.Math.Max(1, System.Math.Max(forcedEnts.Count, _setting.minLenght));

            // min(number of operands, max length param)
            _effectiveMaxLength = System.Math.Min(_sourceEntities.Count, _setting.maxLenght);

            if (_effectiveMaxLength < _effectiveMinLength)
                throw new ArgumentException("Effective MinLenght is greather than MaxLenght");

            //TODO
            //_setting.ClassOfEquivalence
        }

        #endregion

        protected abstract IBitString operation(IBitString operand1, IBitString operand2);

        #region Fields

        private List<IEntityEnumerator> _sourceEntities = new List<IEntityEnumerator>();

        // max(1, number of forced operands, min length param))
        private int _effectiveMinLength;

        // min(number of operands, max length param)
        //////// _effectiveMaxLength can be less then _effectiveMinLength, but only in the case that (_effectiveMaxLength == 0) and thus this enumerator i.e. MoveNext always returns false
        private int _effectiveMaxLength;

        private readonly IMultipleOperandEntitySetting _setting;

        #endregion

        #region IDisposable Members

        /// <summary>
        /// Performs application-defined tasks associated 
        /// with freeing, releasing, or resetting unmanaged 
        /// resources.
        /// </summary>
        public new void Dispose()
        {
            base.Dispose();
        }

        #endregion

        #region IEnumerator Members

        #region IEnumerator state

        //private enumeratorState _state = enumeratorState.StartState;
        private int _state = 0;
        private Stack<int> _sourceEntityIndexStack;
        private Stack<IEnumerator<IBitString>> _sourceEntityStack;
        private Stack<IBitString> _bitStringStack;

        private enum enumeratorState
        {
            StartState = 0,
            EndState = 1,
            ProlongSubset = 2, // make the cedent longer
            MakeNextEntry = 3, // make next literal
            SwitchLastSourceEntity = 4, // drop the last literal and create one with higher index
            ShortenSubset = 5, // shorten current cedent
        }

        #endregion
        /*

        private bool makeLongerSet(int sourceEntityIndex)
        {
            // is it possible to create longer cedent next time?
            // (we are still not at maximum cedent length and we have unused literals => yes)
            if ((_sourceEntityStack.Count < _effectiveMaxLength) && (sourceEntityIndex + 1 < _sourceEntities.Count))
            {
                // BTW: this condition is always true due <ShortenSubset, sourceEntityTrace.MoveNext()>

                // yes, store the bit string to stack
                _bitStringStack.Push(_currentBitString);

                // next time create longer cedent again
                _state = enumeratorState.ProlongSubset;

                // is this a result
                if (_sourceEntityStack.Count >= _effectiveMinLength)
                {
                    // this is a result
                    return false;
                }
                else
                {
                    // make the cedent longer
                    //_state = enumeratorState.ProlongSubset;
                    return true;
                }
            }
            else
            {
                // it is not possible to create longer cedent;
                // next state is "make next literal"
                _state = enumeratorState.MakeNextEntry;

                // this is a result
                Debug.Assert(_sourceEntityStack.Count >= _effectiveMinLength);
                return false;
            }
        }

        private void prepareBitString(bool prolongPeaked, IBitString bitStringForAdd)
        {
            if (_sourceEntityStack.Count == 1)
            {
                Debug.Assert((_bitStringStack == null) || (_bitStringStack.Count == 0));
                _currentBitString = bitStringForAdd;
            }
            else
            {
                if (prolongPeaked)
                {
                    Debug.Assert((_bitStringStack != null) && (_bitStringStack.Count > 0));
                    _currentBitString = _bitStringStack.Peek();
                }
                else
                    Debug.Assert((_currentBitString != null) || (_bitStringStack.Count != 0));
                _currentBitString = operation(_currentBitString, bitStringForAdd);
            }
        }
       
         
        /// <summary>
        /// Advances the enumerator to the next element 
        /// of the collection.
        /// </summary>
        /// <returns>
        /// true if the enumerator was successfully advanced 
        /// to the next element; false if the enumerator has 
        /// passed the end of the collection.
        /// </returns>
        /// <exception cref="T:System.InvalidOperationException">
        /// The collection was modified after the enumerator 
        /// was created. 
        /// </exception>
        public override bool MoveNext()
        {
            int sourceEntityIndex = Int32.MinValue;
            IEnumerator<IBitString> sourceEntityTrace;

        again:
            Debug.Assert((_state != enumeratorState.StartState && _sourceEntityStack != null && _sourceEntityIndexStack != null)
                        || (_state == enumeratorState.StartState && _sourceEntityStack == null && _sourceEntityIndexStack == null));
            Debug.Assert(_state == enumeratorState.StartState 
                         || (_sourceEntityStack.Count == _sourceEntityIndexStack.Count));
            switch (_state)
            {
                case enumeratorState.StartState:
                    // start state
                    if (_effectiveMaxLength == 0)
                    {
                        _state = enumeratorState.EndState;
                        return false;
                    }
                    Debug.Assert(_effectiveMinLength <= _effectiveMaxLength);

                    // initialize status stacks
                    _sourceEntityIndexStack = new Stack<int>(_effectiveMaxLength);
                    _sourceEntityStack = new Stack<IEnumerator<IBitString>>(_effectiveMaxLength);
                    if (_effectiveMaxLength > 1)
                        _bitStringStack = new Stack<IBitString>(_effectiveMaxLength - 1);

                    // push first literal to stack

                    sourceEntityIndex = -1;
                    goto case enumeratorState.ProlongSubset;

                // -----------------------------------------------------------------------------
                case enumeratorState.EndState:
                    // finish state
                    return false;

                // -----------------------------------------------------------------------------
                case enumeratorState.ProlongSubset:
                    // make the cedent longer
                    Debug.Assert(_sourceEntityStack.Count == _sourceEntityIndexStack.Count);
                    Debug.Assert(_sourceEntityStack.Count < _effectiveMaxLength);

                    if (sourceEntityIndex != -1)
                        sourceEntityIndex = _sourceEntityIndexStack.Peek();

                    Debug.Assert(sourceEntityIndex + 1 < _sourceEntities.Count);
                    Debug.Assert(sourceEntityIndex + (_effectiveMinLength - _sourceEntityStack.Count) <
                                 _sourceEntities.Count);
                    sourceEntityIndex++;
                    _sourceEntityIndexStack.Push(sourceEntityIndex);
                    sourceEntityTrace = _sourceEntities[sourceEntityIndex].GetEnumerator();
                    _sourceEntityStack.Push(sourceEntityTrace);

                    if (sourceEntityTrace.MoveNext())
                    {
                        // prepare bit string
                        prepareBitString(false, sourceEntityTrace.Current);
                    }
                    else
                    {
                        Debug.Assert(false);
                    }

                    // is it necessary to make longer cedent (min length)?
                    if (makeLongerSet(sourceEntityIndex))
                        goto again;
                    else
                        return true;

                // -----------------------------------------------------------------------------
                case enumeratorState.MakeNextEntry:
                    // make next literal
                    Debug.Assert(_sourceEntityStack.Count == _sourceEntityIndexStack.Count);
                    Debug.Assert(_sourceEntityStack.Count <= _effectiveMaxLength);

                    sourceEntityTrace = _sourceEntityStack.Peek();
                    if (sourceEntityTrace.MoveNext())
                    {
                        // we have next literal, prepare bit string
                        prepareBitString(true, sourceEntityTrace.Current);

                        // state stays the same 
                        // and this is the result
                        return true;
                    }
                    else
                    {
                        // go to another state
                        goto case enumeratorState.SwitchLastSourceEntity;
                    }

                // -----------------------------------------------------------------------------
                case enumeratorState.ShortenSubset:
                    // shorten current cedent
                    Debug.Assert(_sourceEntityStack.Count == _sourceEntityIndexStack.Count);
                    Debug.Assert(((_sourceEntityStack.Count <= 1) &&
                                  ((_bitStringStack == null) || (_bitStringStack.Count == 0))) ||
                                 ((_sourceEntityStack.Count > 1) && (_bitStringStack != null) &&
                                  (_bitStringStack.Count + 1 == _sourceEntityStack.Count)));
                    Debug.Assert(_sourceEntityStack.Count < _effectiveMaxLength);

                    // test the end of enumerator
                    if (_sourceEntityStack.Count == 0)
                    {
                        _state = enumeratorState.EndState;
                        _sourceEntityIndexStack = null;
                        _sourceEntityStack = null;
                        _bitStringStack = null;
                        _currentBitString = null;
                        return false;
                    }

                    // make next literal
                    sourceEntityTrace = _sourceEntityStack.Peek();
                    if (sourceEntityTrace.MoveNext())
                    {
                        // we have next literal, prepare bit string
                        prepareBitString(true, sourceEntityTrace.Current);

                        // next time it is surely possible to make longer cedent
                        // is it necessary to make longer cedent (min length)?
                        if (makeLongerSet(sourceEntityIndex))
                            goto again;
                        else
                            if (_state == enumeratorState.MakeNextEntry)
                                // subset was not prolonged
                            return true;
                    }
                    else
                    {
                        // go to another state
                        goto case enumeratorState.SwitchLastSourceEntity;
                    }

                // -----------------------------------------------------------------------------
                case enumeratorState.SwitchLastSourceEntity:
                    // drop the last literal and create one with higher index
                    sourceEntityIndex = _sourceEntityIndexStack.Pop();
                    _sourceEntityStack.Pop();

                    // be careful with minimum required cedent length, jump to case ShortenSubset if necessary
                    if ((sourceEntityIndex + 1 < _sourceEntities.Count) &&
                        (sourceEntityIndex + (_effectiveMinLength - _sourceEntityStack.Count) < _sourceEntities.Count))
                    {
                        // well, it is possible, proceed...
                        sourceEntityIndex++;
                        _sourceEntityIndexStack.Push(sourceEntityIndex);
                        sourceEntityTrace = _sourceEntities[sourceEntityIndex].GetEnumerator();
                        _sourceEntityStack.Push(sourceEntityTrace);
                        Debug.Assert(sourceEntityTrace.MoveNext());

                        // mozna nebude vzdy platit
                        Debug.Assert(((_sourceEntityStack.Count <= 1) &&
                                      ((_bitStringStack == null) || (_bitStringStack.Count == 0))) ||
                                     ((_sourceEntityStack.Count > 1) && (_bitStringStack != null) &&
                                      (_bitStringStack.Count + 1 == _sourceEntityStack.Count)));

                        // we have next literal, prepare bit string
                        prepareBitString(true, sourceEntityTrace.Current);

                        // is it necessary to make longer cedent (min length)?
                        if (makeLongerSet(sourceEntityIndex))
                            goto again;
                        else
                            return true;
                    }
                    else
                    {
                        // no more available literals,
                        // we must shorten current cedent

                        // remove one bit string from the stack
                        if ((_bitStringStack != null) && (_bitStringStack.Count > 0))
                            _bitStringStack.Pop();

                        // go to "shorten cedent" state
                        goto case enumeratorState.ShortenSubset;
                    }
                default:
                    throw new InvalidOperationException();
            }
        }
        */

        /// <summary>
        /// Prepares next partial cedent.
        /// </summary>
        /// <returns><b>true</b> if there is another partial cedent available, <b>false</b> otherwise.</returns>
        public override bool MoveNext()
        {
            int literalIndex;
            IEnumerator<IBitString> literalTrace;
            //IBitString bitString;

            switch (_state)
            {
                case 0:
                    // start state
                    if (_effectiveMaxLength == 0)
                    {
                        _state = 1;
                        return false;
                    }
                    Debug.Assert(_effectiveMinLength <= _effectiveMaxLength);

                    // initialize status stacks
                    _sourceEntityIndexStack = new Stack<int>(_effectiveMaxLength);
                    _sourceEntityStack = new Stack<IEnumerator<IBitString>>(_effectiveMaxLength);
                    if (_effectiveMaxLength > 1)
                        _bitStringStack = new Stack<IBitString>(_effectiveMaxLength - 1);

                    // push first literal to stack
                    _sourceEntityIndexStack.Push(0);
                    literalTrace = _sourceEntities[0].GetEnumerator();
                    _sourceEntityStack.Push(literalTrace);
                    Debug.Assert(literalTrace.MoveNext());
                    _currentBitString = literalTrace.Current;

                    // is it possible to create longer cedent?
                    if (_effectiveMaxLength > 1)
                    {
                        // yes, store the bit string to stack
                        _bitStringStack.Push(_currentBitString);

                        // next state is "make longer cedent"
                        _state = 2;

                        if (_effectiveMinLength <= 1)
                        {
                            // this is a result
                            return true;
                        }
                        else
                        {
                            // make the cedent longer
                            goto case 2;
                        }
                    }
                    else
                    {
                        // no, next state is "make next literal"
                        _state = 3;

                        // this is a result
                        return true;
                    }


                // -----------------------------------------------------------------------------
                case 1:
                    // finish state
                    return false;


                // -----------------------------------------------------------------------------
                case 2:
                    // make the cedent longer
                    Debug.Assert(_sourceEntityStack.Count == _sourceEntityIndexStack.Count);
                    Debug.Assert(_sourceEntityStack.Count < _effectiveMaxLength);

                    literalIndex = _sourceEntityIndexStack.Peek();
                    Debug.Assert(literalIndex + 1 < _sourceEntities.Count);
                    Debug.Assert(literalIndex + (_effectiveMinLength - _sourceEntityStack.Count) < _sourceEntities.Count);
                    literalIndex++;
                    _sourceEntityIndexStack.Push(literalIndex);
                    literalTrace = _sourceEntities[0].GetEnumerator();
                    _sourceEntityStack.Push(literalTrace);
                    Debug.Assert(literalTrace.MoveNext());
                    _currentBitString = operation(_currentBitString, literalTrace.Current);

                    // is it possible to create longer cedent next time?
                    // (we are still not at maximum cedent length and we have unused literals => yes)
                    if ((_sourceEntityStack.Count < _effectiveMaxLength) && (literalIndex + 1 < _sourceEntities.Count))
                    {
                        // yes, store the bit string to stack
                        _bitStringStack.Push(_currentBitString);

                        // next time create longer cedent again
                        _state = 2;

                        // is this a result
                        if (_sourceEntityStack.Count >= _effectiveMinLength)
                        {
                            // this is a result
                            return true;
                        }
                        else
                        {
                            // make the cedent longer
                            goto case 2;
                        }
                    }
                    else
                    {
                        // it is not possible to create longer cedent;
                        // next state is "make next literal"
                        _state = 3;

                        // this is a result
                        Debug.Assert(_sourceEntityStack.Count >= _effectiveMinLength);
                        return true;
                    }


                // -----------------------------------------------------------------------------
                case 3:
                    // make next literal
                    Debug.Assert(_sourceEntityStack.Count == _sourceEntityIndexStack.Count);
                    Debug.Assert(_sourceEntityStack.Count <= _effectiveMaxLength);

                    literalTrace = _sourceEntityStack.Peek();
                    if (literalTrace.MoveNext())
                    {
                        // we have next literal, prepare bit string
                        if (_sourceEntityStack.Count == 1)
                        {
                            Debug.Assert((_bitStringStack == null) || (_bitStringStack.Count == 0));
                            _currentBitString = literalTrace.Current;
                        }
                        else
                        {
                            _currentBitString = _bitStringStack.Peek();
                            _currentBitString = operation(_currentBitString, literalTrace.Current);
                        }

                        // state stays the same 
                        // and this is the result
                        return true;
                    }
                    else
                    {
                        // go to another state
                        goto case 4;
                    }


                // -----------------------------------------------------------------------------
                case 4:
                    // drop the last literal and create one with higher index
                    literalIndex = _sourceEntityIndexStack.Pop();
                    _sourceEntityStack.Pop();
                    if (literalIndex + 1 < _sourceEntities.Count)
                    {
                        // well, it is possible, proceed...
                        literalIndex++;
                        _sourceEntityIndexStack.Push(literalIndex);
                        literalTrace = _sourceEntities[literalIndex].GetEnumerator();
                        _sourceEntityStack.Push(literalTrace);
                        Debug.Assert(literalTrace.MoveNext());
                        _currentBitString = _bitStringStack.Peek();
                        _currentBitString = operation(_currentBitString, literalTrace.Current);

                        Debug.Assert(_sourceEntityStack.Count == _sourceEntityIndexStack.Count);
                        Debug.Assert(((_sourceEntityStack.Count <= 1) && ((_bitStringStack == null) || (_bitStringStack.Count == 0))) ||
                            ((_sourceEntityStack.Count > 1) && (_bitStringStack != null) && (_bitStringStack.Count + 1 == _sourceEntityStack.Count)));

                        return true;
                    }
                    else
                    {
                        // no more available literals,
                        // we must shorten current cedent

                        // remove one bit string from the stack
                        if ((_bitStringStack != null) && (_bitStringStack.Count > 0))
                            _bitStringStack.Pop();

                        // go to "shorten cedent" state
                        goto case 5;
                    }


                // -----------------------------------------------------------------------------
                case 5:
                    // shorten current cedent
                    Debug.Assert(_sourceEntityStack.Count == _sourceEntityIndexStack.Count);
                    Debug.Assert(((_sourceEntityStack.Count <= 1) && ((_bitStringStack == null) || (_bitStringStack.Count == 0))) ||
                        ((_sourceEntityStack.Count > 1) && (_bitStringStack != null) && (_bitStringStack.Count + 1 == _sourceEntityStack.Count)));
                    Debug.Assert(_sourceEntityStack.Count < _effectiveMaxLength);

                    // test the end of enumerator
                    if (_sourceEntityStack.Count == 0)
                    {
                        _state = 1;
                        _sourceEntityIndexStack = null;
                        _sourceEntityStack = null;
                        _bitStringStack = null;
                        _currentBitString = null;
                        return false;
                    }

                    // make next literal
                    literalTrace = _sourceEntityStack.Peek();
                    if (literalTrace.MoveNext())
                    {
                        // we have next literal, prepare bit string
                        if (_sourceEntityStack.Count == 1)
                        {
                            Debug.Assert((_bitStringStack == null) || (_bitStringStack.Count == 0));
                            _currentBitString = literalTrace.Current;
                        }
                        else
                        {
                            Debug.Assert((_bitStringStack != null) && (_bitStringStack.Count > 0));
                            _currentBitString = _bitStringStack.Peek();
                            _currentBitString = operation(_currentBitString, literalTrace.Current);
                        }

                        // next time it is surely possible to make longer cedent
                        _state = 2;

                        // store the bit string to stack
                        _bitStringStack.Push(_currentBitString);

                        // is this cedent long enough to be a result?
                        if (_sourceEntityStack.Count >= _effectiveMinLength)
                        {
                            // yes
                            return true;
                        }
                        else
                        {
                            // no, proceed directly to the "make longer cedent" state
                            goto case 2;
                        }
                    }
                    else
                    {
                        // go to another state
                        goto case 6;
                    }


                // -----------------------------------------------------------------------------
                case 6:
                    // drop the last literal and create one with higher index
                    literalIndex = _sourceEntityIndexStack.Pop();
                    _sourceEntityStack.Pop();

                    // be careful with minimum required cedent length, jump to case 5 if necessary
                    if ((literalIndex + 1 < _sourceEntities.Count) && (literalIndex + (_effectiveMinLength - _sourceEntityStack.Count) < _sourceEntities.Count))
                    {
                        // well, it is possible, proceed...
                        literalIndex++;
                        _sourceEntityIndexStack.Push(literalIndex);
                        literalTrace = _sourceEntities[literalIndex].GetEnumerator();
                        _sourceEntityStack.Push(literalTrace);
                        Debug.Assert(literalTrace.MoveNext());

                        if (_sourceEntityStack.Count == 1)
                        {
                            Debug.Assert((_bitStringStack == null) || (_bitStringStack.Count == 0));
                            _currentBitString = literalTrace.Current;
                        }
                        else
                        {
                            Debug.Assert((_bitStringStack != null) && (_bitStringStack.Count > 0));
                            _currentBitString = _bitStringStack.Peek();
                            _currentBitString = operation(_currentBitString, literalTrace.Current);
                        }

                        // is it possible to make longer cedent next time?
                        if ((_sourceEntityStack.Count < _effectiveMaxLength) && (literalIndex + 1 < _sourceEntities.Count))
                        {
                            // make it longer then
                            _state = 2;

                            // store the bit string to stack
                            _bitStringStack.Push(_currentBitString);

                            // is this cedent long enough to be a result?
                            if (_sourceEntityStack.Count >= _effectiveMinLength)
                            {
                                // yes
                                return true;
                            }
                            else
                            {
                                // no, proceed directly to the "make longer cedent" state
                                goto case 2;
                            }
                        }
                        else
                        {
                            // it is not possible to create longer cedent;
                            // next state is "make next literal"
                            _state = 3;

                            // this is a result
                            Debug.Assert(_sourceEntityStack.Count >= _effectiveMinLength);
                            return true;
                        }
                    }
                    else
                    {
                        // no more available literals,
                        // we must shorten current cedent

                        // remove one bit string from the stack
                        if ((_bitStringStack != null) && (_bitStringStack.Count > 0))
                            _bitStringStack.Pop();

                        // go to "shorten cedent" state
                        goto case 5;
                    }
            }


            Debug.Assert(false, "This code should not be reached.");
            return false;
        }
        
        /// <summary>
        /// Sets the enumerator to its initial position, which 
        /// is before the first element in the collection.
        /// </summary>
        /// <exception cref="T:System.InvalidOperationException">
        /// The collection was modified after the enumerator was created.
        /// </exception>
        public new void Reset()
        {
            base.Reset();
            _state = 0;
            //_state = enumeratorState.StartState;
            _sourceEntityIndexStack = null;
            _sourceEntityStack = null;
            _bitStringStack = null;
        }

        #endregion
    }
}