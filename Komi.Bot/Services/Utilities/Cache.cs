using System;
using System.Collections.Generic;

namespace Komi.Bot.Services.Utilities
{
    public class Cache<TKey, TValue>
    {
        private readonly Dictionary<TKey, TValue> _cachedItems = new Dictionary<TKey, TValue>();
        private readonly Func<TKey, TValue> _func;

        public Cache(Func<TKey, TValue> func) => _func = func;

        public TValue this[TKey key]
        {
            get
            {
                if (_cachedItems.TryGetValue(key, out var value))
                    return value;

                var val = _func(key);
                _cachedItems[key] = val;
                return val;
            }
        }
    }
}