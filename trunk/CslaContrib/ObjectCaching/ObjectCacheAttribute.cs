using System;

namespace CslaContrib.ObjectCaching
{
    /// <summary>
    /// Class attribute for business objects that utilize caching.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class ObjectCacheAttribute : Attribute
    {
        /// <summary>
        /// Get the cache attribute from a given type if present.
        /// </summary>
        /// <param name="objectType">Target object type.</param>
        /// <returns>ObjectCacheAttribute instance if present else null.</returns>
        public static ObjectCacheAttribute GetObjectCacheAttribute(Type objectType)
        {
            var result = objectType.GetCustomAttributes(typeof(ObjectCacheAttribute), true);
            if (result != null && result.Length > 0)
                return result[0] as ObjectCacheAttribute;
            else
                return null;
        }

        /// <summary>
        /// Default cache attribute CacheScope is Global, use Named Parameters to set
        /// scope and/or criteria handling if desired.
        /// </summary>
        /// <example>
        /// [Caching(Scope = CacheScope.Group, CacheByCriteria = true)]
        /// </example>
        public ObjectCacheAttribute()
        {
            //defaults
            Scope = CacheScope.Global;
            CacheByCriteria = false;
            Expiration = 0;
        }

        /// <summary>
        /// Declares the CacheScope for the business object data when cached. Default is CacheScope.Global.
        /// </summary>
        public CacheScope Scope { get; set; }

        /// <summary>
        /// Declares criteria should be included for the business object data when cached . Default is false.
        /// </summary>
        /// <remarks>
        /// Since the hash code for the criteria will included as part fo the cache entry key, it is necessary to 
        /// override GetHashCode in these cases to guarantee two instances of the same criteria with equal criteria
        /// values will result in the same hash code.
        /// </remarks>
        /// <example>
        /// <![CDATA[class MyCriteria : Csla.SingleCriteria<MyCriteria, int>
        /// {
        ///     public override int GetHashCode()
        ///     {
        ///         return base.Value.GetHashCode();
        ///     }
        /// }]]>
        /// </example>
        public bool CacheByCriteria { get; set; }

        /// <summary>
        /// Declares specific time span in minutes for expiration. Default is 0.
        /// </summary>
        public int Expiration { get; set; }
    }

    /// <summary>
    /// Range of scope for cached data.
    /// </summary>
    public enum CacheScope
    {
        Global,
        Group,
        User
    }
}
