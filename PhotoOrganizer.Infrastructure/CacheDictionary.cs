using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace PhotoOrganizer.Infrastructure
{
    public class CacheDictionary<TKey, TValue> : IDictionary<TKey, TValue>
    {
        #region Private Members

        private readonly IDictionary<TKey, TValue> _innerDictionary;
        private Queue<TKey> _cacheQueue;

        #endregion

        #region Properties

        private int _limit;
        public int Limit
        {
            get => _limit;
            set
            {
                _limit = value;
                if (value <= 0)
                    Clear();
                else
                {
                    int overLimitNumber = _innerDictionary.Count - value;
                    if (overLimitNumber > 0)
                    {
                        Enumerable
                            .Repeat(0, overLimitNumber)
                            .Select(_ => _cacheQueue.Dequeue())
                            .ForEach(key => _innerDictionary.Remove(key));
                    }
                }
            }
        }

        public TValue this[TKey key]
        {
            get => _innerDictionary[key];
            set
            {
                Remove(key);
                Add(key, value);
            }
        }

        public ICollection<TKey> Keys => _innerDictionary.Keys;
        public ICollection<TValue> Values => _innerDictionary.Values;
        public int Count => _innerDictionary.Count;
        public bool IsReadOnly => _innerDictionary.IsReadOnly;

        #endregion

        #region Constructors

        public CacheDictionary(int limit = 10)
        {
            _innerDictionary = new Dictionary<TKey, TValue>(limit);
            _cacheQueue = new Queue<TKey>(limit);
            Limit = limit;
        }

        #endregion

        #region Public Methods

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => _innerDictionary.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public void Add(KeyValuePair<TKey, TValue> item) => Add(item.Key, item.Value);
        public void Add(TKey key, TValue value)
        {
            if (_innerDictionary.Count == Limit)
            {
                TKey itemToRemove = _cacheQueue.Dequeue();
                _innerDictionary.Remove(itemToRemove);
            }

            _cacheQueue.Enqueue(key);
            _innerDictionary.Add(key, value);
        }

        public void Clear()
        {
            _innerDictionary.Clear();
            _cacheQueue.Clear();
        }

        public bool Contains(KeyValuePair<TKey, TValue> item) => _innerDictionary.Contains(item);
        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex) => _innerDictionary.CopyTo(array, arrayIndex);

        public bool Remove(KeyValuePair<TKey, TValue> item) => Remove(item.Key);
        public bool Remove(TKey key)
        {
            if (_innerDictionary.Remove(key))
            {
                List<TKey> queueAsList = _cacheQueue.ToList();
                queueAsList.Remove(key);
                _cacheQueue = new Queue<TKey>(queueAsList);
                return true;
            }

            return false;
        }

        public bool ContainsKey(TKey key) => _innerDictionary.ContainsKey(key);
        public bool TryGetValue(TKey key, out TValue value) => _innerDictionary.TryGetValue(key, out value);

        #endregion 
    }
}