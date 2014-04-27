using System.Collections.Generic;

namespace Amib.Threading.Internal
{
    internal class SynchronizedDictionary<TKey, TValue>
    {
        private readonly Dictionary<TKey, TValue> _dictionary;
        private readonly object _lock;

        public SynchronizedDictionary()
        {
            _lock = new object();
            _dictionary = new Dictionary<TKey, TValue>();
        }

        public int Count
        {
            get { return _dictionary.Count; }
        }

        public Dictionary<TKey, TValue>.KeyCollection Keys
        {
            get
            {
                lock (_lock)
                {
                    return _dictionary.Keys;
                }
            }
        }

        public object SyncRoot
        {
            get { return _lock; }
        }

        public Dictionary<TKey, TValue>.ValueCollection Values
        {
            get
            {
                lock (_lock)
                {
                    return _dictionary.Values;
                }
            }
        }

        public TValue this[TKey key]
        {
            get
            {
                lock (_lock)
                {
                    return _dictionary[key];
                }
            }
            set
            {
                lock (_lock)
                {
                    _dictionary[key] = value;
                }
            }
        }

        public void Clear()
        {
            lock (_lock)
            {
                _dictionary.Clear();
            }
        }

        public bool Contains(TKey key)
        {
            lock (_lock)
            {
                return _dictionary.ContainsKey(key);
            }
        }

        public void Remove(TKey key)
        {
            lock (_lock)
            {
                _dictionary.Remove(key);
            }
        }
    }
}