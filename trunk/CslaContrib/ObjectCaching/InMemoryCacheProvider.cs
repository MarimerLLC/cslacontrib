using System;
using System.Collections.Generic;
using System.Linq;

namespace CslaContrib.ObjectCaching
{
    /// <summary>
    /// In memory cache provider to support simple cache configuration
    /// </summary>
    public class InMemoryCacheProvider : ICacheProvider
    {
        const string CacheEntriesKey = "_cache_entries";
        internal static Dictionary<string, object> cache = new Dictionary<string, object>();
        object cachelock = new object();

        #region ICacheProvider Members

        public void Put(string key, object value)
        {
            Put(key, value, new TimeSpan());
        }

        public void Put(string key, object value, string area)
        {
            Put(key, value);
        }

        public void Put(string key, object value, TimeSpan timeout)
        {
            if (cache.ContainsKey(key))
                lock (cachelock)
                {
                    cache[key] = value;
                }
            else
                lock (cachelock)
                {
                    cache.Add(key, value);
                }

            if (timeout.Ticks == 0) timeout = new TimeSpan(0, int.MaxValue, 0);
            AddEntry(key, DateTime.Now.Add(timeout));
        }

        public void Put(string key, object value, TimeSpan timeout, string area)
        {
            Put(key, value, timeout);
        }

        public object Get(string key)
        {
            object value = null;
            cache.TryGetValue(key, out value);
            if (value != null)
            {
                //test for expired entry
                if (Entries.ContainsKey(key) && Entries[key].CompareTo(DateTime.Now) <= 0) value = null;
            }
            return value;
        }

        public object Get(string key, string area)
        {
            return Get(key);
        }

        public object Get(string key, TimeSpan timeout)
        {
            var value = Get(key);
            //slide exipiration
            if (value != null) Put(key, value, timeout);
            return value;
        }

        public object Get(string key, TimeSpan timeout, string area)
        {
            return Get(key, timeout);
        }

        public void Remove(string key)
        {
            lock (cachelock)
            {
                cache.Remove(key);
            }
            RemoveEntry(key);
        }

        public void Remove(string key, string area)
        {
            Remove(key);
        }

        public void RemoveAllByKeyPrefix(string keyPrefix)
        {
            var items = new Dictionary<string, DateTime>();
            items = Entries.Where(e => e.Key.StartsWith(keyPrefix, StringComparison.InvariantCultureIgnoreCase)).ToDictionary(k => k.Key, v => v.Value);
            foreach (var item in items)
            {
                Remove(item.Key);
            }
        }

        public void CreateArea(string area)
        {
            throw new NotImplementedException();
        }

        public void RemoveArea(string area)
        {
            throw new NotImplementedException();
        }

        public void Initialize()
        {
            lock (cachelock)
            {
                cache.Clear();
            }
        }

        private Dictionary<string, DateTime> entries = new Dictionary<string, DateTime>();
        public Dictionary<string, DateTime> Entries
        {
            get { return entries; }
        }

        #endregion

        private void AddEntry(string key, DateTime expiration)
        {
            DateTime date;
            if (Entries.TryGetValue(key, out date))
            {
                //replace
                Entries[key] = expiration;
            }
            else
            {
                //add
                Entries.Add(key, expiration);
            }
        }

        private void RemoveEntry(string key)
        {
            DateTime date;
            if (Entries.TryGetValue(key, out date))
            {
                Entries.Remove(key);
            }
        }
    }
}