using System;
using System.Configuration;

namespace CslaContrib.ObjectCaching
{
    /// <summary>
    /// Cache provider factory class
    /// </summary>
    public class CacheManager
    {
        const string PROVIDER_CONFIG = "CachingProvider";
        static object syncProvider = new object();
        static ICacheProvider cacheProvider;

        /// <summary>
        /// Factory method to get configured cache provider, else null cache provider for support
        /// </summary>
        /// <returns>Configured ICacheProvider instance</returns>
        /// <example>
        /// <![CDATA[<appSettings>
        ///      <add key="CachingProvider" value="CslaContrib.ObjectCaching.InMemoryCacheProvider, CslaContrib" />
        ///  </appSettings>]]>
        /// </example>
        public static ICacheProvider GetCacheProvider()
        {
            if(cacheProvider != null) return cacheProvider;

#if !SILVERLIGHT
            lock (syncProvider)
            {
                var providerType = ConfigurationManager.AppSettings[PROVIDER_CONFIG];
                if (providerType == null) return null;

                cacheProvider = ActivateProvider(providerType);
                return cacheProvider;
            }
#endif
            throw new Exception("Missing CacheProvider configuration.");
        }

        /// <summary>
        /// Test cache system configuration and type declaration
        /// </summary>
        /// <param name="objectType">Target business object type</param>
        /// <returns>True if configuration is valid and type has ObjectCacheAttribute; else false</returns>
        public static bool IsCacheConfigured(Type objectType)
        {
            return (ObjectCacheAttribute.GetObjectCacheAttribute(objectType) != null && GetCacheProvider() != null) ;
        }

        /// <summary>
        /// Factory method to set pre-constructed provider, typically for test scenarios
        /// </summary>
        /// <param name="provider">Desired ICacheProvider instance</param>
        /// <returns></returns>
        public static ICacheProvider SetCacheProvider(string providerType)
        {
            lock (syncProvider)
            {
                cacheProvider = ActivateProvider(providerType);
                return cacheProvider;
            }
        }

        private static ICacheProvider ActivateProvider(string providerType)
        {
            if (string.IsNullOrEmpty(providerType)) return null;
 
            var type = Type.GetType(providerType);
            if (type == null) throw new Exception(string.Format("Unable to load configured cache provider, could not resolve {0}.", providerType));
            var provider = (ICacheProvider)Activator.CreateInstance(type);
            if (provider != null) provider.Initialize();
            return provider;
        }
    }
}
