using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using Microsoft.ApplicationServer.Caching;

namespace CslaContrib.ObjectCaching.AppFabric
{
    public class CacheProvider : ICacheProvider
    {
        const string CacheEntriesKey = "_cache_entries";
        DataCacheFactory cacheFactory;
        DataCache defaultCache;

        #region ICacheProvider Members

        public void Initialize()
        {
            var hostName = ConfigurationManager.AppSettings["CacheHost"] ?? "localhost";
            var cachePort = string.IsNullOrEmpty(ConfigurationManager.AppSettings["CacheName"]) ? 22233 : int.Parse(ConfigurationManager.AppSettings["CacheName"]);
            var cacheName = ConfigurationManager.AppSettings["CacheName"] ?? "default";

            List<DataCacheServerEndpoint> servers = new List<DataCacheServerEndpoint>(1);
            servers.Add(new DataCacheServerEndpoint(hostName, cachePort));
            DataCacheFactoryConfiguration configuration = new DataCacheFactoryConfiguration();
            configuration.TransportProperties.MaxBufferSize = Int32.MaxValue;
            configuration.TransportProperties.MaxBufferPoolSize = Int32.MaxValue;
            configuration.Servers = servers;
            configuration.LocalCacheProperties = new DataCacheLocalCacheProperties(10000, new TimeSpan(0, 5, 0), DataCacheLocalCacheInvalidationPolicy.TimeoutBased);
            DataCacheClientLogManager.ChangeLogLevel(System.Diagnostics.TraceLevel.Off);
            cacheFactory = new DataCacheFactory(configuration);
            defaultCache = cacheFactory.GetCache(cacheName);
        }

        public Dictionary<string, DateTime> Entries
        {
            get 
            {
                var entries = Get(CacheEntriesKey);
                if (entries == null)
                {
                    entries = new Dictionary<string, DateTime>();
                }    
                return (Dictionary<string, DateTime>)entries;
            }
        }

        public void Put(string key, object value)
        {
            using (var catalog = new Catalog(defaultCache))
            {
                defaultCache.Put(key, value);
                catalog.AddEntry(key);
            }
        }

        public void Put(string key, object value, string area)
        {
            //ignore region for now
            Put(key, value);
        }

        public void Put(string key, object value, TimeSpan timeout)
        {
            using (var catalog = new Catalog(defaultCache))
            {
                defaultCache.Put(key, value, timeout);
                catalog.AddEntry(key);
            }
        }

        public void Put(string key, object value, TimeSpan timeout, string area)
        {
            //ignore region for now
            Put(key, value, timeout);
        }

        public object Get(string key)
        {
            return defaultCache.Get(key);
        }

        public object Get(string key, string area)
        {
            //ignore region for now
            return Get(key);
        }

        public object Get(string key, TimeSpan timeout)
        {
            //slide expiration
            var value = Get(key);
            Put(key, value, timeout);
            return value;
        }

        public object Get(string key, TimeSpan timeout, string area)
        {
            //ignore region for now
            return Get(key, timeout);
        }

        public void Remove(string key)
        {
            using (var catalog = new Catalog(defaultCache))
            {
                defaultCache.Remove(key);
                catalog.RemoveEntry(key);
            }
        }

        public void Remove(string key, string area)
        {
            //ignore region for now
            Remove(key);
        }

        public void RemoveAllByKeyPrefix(string keyPrefix)
        {
            foreach (var item in Entries.Where(e => e.Key.StartsWith(keyPrefix, StringComparison.InvariantCultureIgnoreCase)))
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

        #endregion

        /// <summary>
        /// handle concurrency for updates to catalog entry collection within the cache
        /// </summary>
        private class Catalog : IDisposable
        {
            DataCache _cache;
            DataCacheLockHandle _lockHandle;
            Dictionary<string, DateTime> _entries;
            public Catalog(DataCache cache)
            {
                _cache = cache;
                var entries = _cache.Get(CacheProvider.CacheEntriesKey);
                if (entries == null)
                {
                    entries = new Dictionary<string, DateTime>();
                    _cache.Put(CacheProvider.CacheEntriesKey, entries);
                }

                _entries = (Dictionary<string, DateTime>)_cache.GetAndLock(CacheProvider.CacheEntriesKey, new TimeSpan(0, 0, 60), out _lockHandle);
            }

            public void AddEntry(string key)
            {
                DateTime date;
                if (_entries.TryGetValue(key, out date))
                {
                    //replace
                    _entries[key] = DateTime.Now;
                }
                else
                {
                    //add
                    _entries.Add(key, DateTime.Now);
                }
            }

            public void RemoveEntry(string key)
            {
                DateTime date;
                if (_entries.TryGetValue(key, out date))
                {
                    _entries.Remove(key);
                }
            }

            #region IDisposable Members

            public void Dispose()
            {
                _cache.PutAndUnlock(CacheProvider.CacheEntriesKey, _entries, _lockHandle);
            }

            #endregion
        }
    }
}
