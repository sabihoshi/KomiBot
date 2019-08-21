using System;
using System.Collections.Generic;
using System.Text;

namespace KomiBot.Services.Core
{
    public class Cache<TKey, TValue>
    {
        private readonly Dictionary<TKey, TValue> _cachedItems = new Dictionary<TKey, TValue>();
        private readonly Func<TKey, TValue> _func;

        public Cache(Func<TKey, TValue> func)
        {
            _func = func;
        }

        public TValue this[TKey key]
        {
            get
            {
                if (_cachedItems.TryGetValue(key, out TValue value)) return value;
                TValue val = _func(key);
                _cachedItems[key] = val;
                return val;
            }
        }
    }
}
