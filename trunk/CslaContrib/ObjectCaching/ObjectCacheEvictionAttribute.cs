using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CslaContrib.ObjectCaching
{
    /// <summary>
    /// Class attribute for business objects that alter cached data.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class ObjectCacheEvictionAttribute : Attribute
    {
        /// <summary>
        /// Get the cache eviction attribute from a given type if present.
        /// </summary>
        /// <param name="objectType">Target object type.</param>
        /// <returns>ObjectCacheEvictionAttribute instance if present else null.</returns>
        public static ObjectCacheEvictionAttribute GetObjectCacheEvictionAttribute(Type objectType)
        {
            var result = objectType.GetCustomAttributes(typeof(ObjectCacheEvictionAttribute), true);
            if (result != null && result.Length > 0)
                return result[0] as ObjectCacheEvictionAttribute;
            else
                return null;
        }

        /// <summary>
        /// Default cache eviction attribute CacheScope is Global, use Named Parameters to set
        /// scope and cached types as needed.
        /// </summary>
        /// <example>
        /// [ObjectCacheEviction(Scope = CacheScope.Group, CachedTypes = new Type[]{ typeof() })]
        /// </example>
        public ObjectCacheEvictionAttribute()
        {
            //defaults
            Scope = CacheScope.Global;
        }

        /// <summary>
        /// Declares the CacheScope for the business object data when cache items are evicted. Default is CacheScope.Global.
        /// </summary>
        public CacheScope Scope { get; set; }

        /// <summary>
        /// Array of Type objects for all affected cached objects to be evicted when data changes are made.
        /// </summary>
        public Type[] CachedTypes { get; set; }
    }
}
