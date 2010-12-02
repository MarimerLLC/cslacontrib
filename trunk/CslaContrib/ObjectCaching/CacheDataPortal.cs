using System;
using System.Configuration;
using Csla.DataPortalClient;
using Csla.Server;

namespace CslaContrib.ObjectCaching
{
    /// <summary>
    /// CSLA Data Portal that directs fetch data access to cache as configured with fall 
    /// through to underlying data portal as necessary
    /// </summary>
    public class CacheDataPortal : IDataPortalProxy
    {
        /// <summary>
        /// Csla.ApplicationContext.ClientContext key value for caching group used to identify cached items
        /// shared by some group of users
        /// </summary>
        public const string CacheGroup = "_CACHE_GROUP_CONTEXT_KEY";

        #region IDataPortalProxy Members
        Csla.DataPortalClient.IDataPortalProxy proxy;

        public bool IsServerRemote
        {
            get 
            {
                if (proxy == null) proxy = GetDataPortalProxy();
                return proxy.IsServerRemote; 
            }
        }

        #endregion

        #region IDataPortalServer Members

        public Csla.Server.DataPortalResult Create(Type objectType, object criteria, Csla.Server.DataPortalContext context)
        {
            //evict cache items based on ObjectCacheEvictionAttribute
            RemoveCacheItems(objectType);

            proxy = GetDataPortalProxy();
            return proxy.Create(objectType, criteria, context);
        }

        public Csla.Server.DataPortalResult Update(object obj, Csla.Server.DataPortalContext context)
        {
            //evict cache items based on ObjectCacheEvictionAttribute
            RemoveCacheItems(obj.GetType());

            proxy = GetDataPortalProxy();
            return proxy.Update(obj, context);
        }

        public Csla.Server.DataPortalResult Delete(Type objectType, object criteria, Csla.Server.DataPortalContext context)
        {
            //evict cache items based on ObjectCacheEvictionAttribute
            RemoveCacheItems(objectType);

            proxy = GetDataPortalProxy();
            return proxy.Delete(objectType, criteria, context);
        }

        public DataPortalResult Fetch(Type objectType, object criteria, DataPortalContext context)
        {
            var cachingAttribute = ObjectCacheAttribute.GetObjectCacheAttribute(objectType);
            var cacheProvider = CacheManager.GetCacheProvider();

            if (cachingAttribute != null && cacheProvider != null)
            {
                //get scenario details
                var scope = cachingAttribute.Scope;
                var cacheByCriteria = cachingAttribute.CacheByCriteria;
                var expiration = cachingAttribute.Expiration;
                
                //get key
                var key = GetCacheItemKey(objectType, cachingAttribute);

                //include criteria hash if needed
                if (cachingAttribute.CacheByCriteria)
                    key = string.Format("{0}::{1}", key, criteria.GetHashCode());

                var data = cacheProvider.Get(key);
                if (data == null)
                {
                    //cache miss
                    proxy = GetDataPortalProxy();
                    var results = proxy.Fetch(objectType, criteria, context);
                    if (expiration > 0)
                        cacheProvider.Put(key, results, new TimeSpan(0, expiration, 0));
                    else
                        cacheProvider.Put(key, results);

                    return results;
                }
                else
                {
                    //cache hit
                    return (DataPortalResult)data;
                }
            }
            else
            {
                proxy = GetDataPortalProxy();
                return proxy.Fetch(objectType, criteria, context);
            }
        }

        private static string GetCacheItemKey(Type objectType, ObjectCacheAttribute cachingAttribute)
        {
            //determine entry key
            var key = string.Format("{0}.{1}", objectType.Namespace, objectType.Name);
            if (cachingAttribute.Scope == CacheScope.Group)
            {
                var group = Csla.ApplicationContext.ClientContext[CacheGroup];
                if (group == null) throw new ApplicationException("ClientContext group required for Group scope data caching");
                key = string.Format("{0}::{1}", key, group);
            }
            else if (cachingAttribute.Scope == CacheScope.User)
            {
                var group = Csla.ApplicationContext.ClientContext[CacheGroup];
                if (group == null) group = string.Empty; //allow user scope with or without a specified grouping
                var user = Csla.ApplicationContext.User;
                if (!user.Identity.IsAuthenticated) throw new ApplicationException("Authenticated user required for User scope data caching");
                key = string.Format("{0}::{1}::{2}", key, group, user.Identity.Name);
            }

            return key;
        }

        private static void RemoveCacheItems(Type objectType)
        {
            var cachingAttribute = ObjectCacheEvictionAttribute.GetObjectCacheEvictionAttribute(objectType);
            var cacheProvider = CacheManager.GetCacheProvider();

            if (cachingAttribute != null && cacheProvider != null)
            {
                //evict cache items for listed types
                foreach (var type in cachingAttribute.CachedTypes)
                {
                    var group = Csla.ApplicationContext.ClientContext[CacheGroup];
                    if (group == null) group = string.Empty; //allow group eviction with or without a specified grouping
                    var key = string.Format("{0}.{1}", type.Namespace, type.Name);
                    if (cachingAttribute.Scope == CacheScope.Group && !string.IsNullOrEmpty(group.ToString())) key = string.Format("{0}::{1}", key, group);
                    cacheProvider.RemoveAllByKeyPrefix(key);
                }
            }
        }

        #endregion

        #region DataPortal Proxy

        private static Type _proxyType;

        /* The cache data portal proxy can be inserted in front of the default data portal proxy using a chain of command pattern configued like this example where WCF
         * is the default proxy. If a specific data portal is not configured, the default data portal is used. The CslaDataPortalDefaultProxy entry is optional.
         * 
         *  <appSettings>
         *    <add key="CslaDataPortalProxy" value="CslaContrib.ObjectCaching.CacheDataPortal, CslaContrib" />
         *    <add key="CslaDataPortalDefaultProxy" value="Csla.DataPortalClient.WcfProxy, Csla" />
         *  </appSettings>
         */
        private static Csla.DataPortalClient.IDataPortalProxy GetDataPortalProxy()
        {
            if (_proxyType == null)
            {
                string proxyTypeName = ConfigurationManager.AppSettings["CslaDataPortalDefaultProxy"];
                if (string.IsNullOrEmpty(proxyTypeName))
                    proxyTypeName = "Local";
                if (proxyTypeName == "Local")
                    _proxyType = typeof(Csla.DataPortalClient.LocalProxy);
                else
                    _proxyType = Type.GetType(proxyTypeName, true, true);
            }
            return (Csla.DataPortalClient.IDataPortalProxy)Activator.CreateInstance(_proxyType);
        }

        public static void ResetProxyType()
        {
            _proxyType = null;
        }

        #endregion
    }
}
