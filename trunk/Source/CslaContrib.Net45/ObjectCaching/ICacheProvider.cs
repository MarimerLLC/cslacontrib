using System;
using System.Collections.Generic;

namespace CslaContrib.ObjectCaching
{
    /// <summary>
    /// Interface for any cache provider
    /// </summary>
    public interface ICacheProvider
    {
        /// <summary>
        /// Provider initization method
        /// </summary>
        void Initialize();

        /// <summary>
        /// Catalog of entries used for cache invalidation management
        /// </summary>
        Dictionary<string, DateTime> Entries { get; }

        /// <summary>
        /// Add OR Replace cache item by key
        /// </summary>
        /// <param name="key">unique item id</param>
        /// <param name="value">cache data</param>
        void Put(string key, object value);
        /// <summary>
        /// Add OR Replace cache item by key
        /// </summary>
        /// <param name="key">unique item id</param>
        /// <param name="value">cache data</param>
        /// <param name="area">named cache area</param>
        void Put(string key, object value, string area);

        /// <summary>
        /// Add OR Replace cache item by key
        /// </summary>
        /// <param name="key">unique item id</param>
        /// <param name="value">cache data</param>
        /// <param name="timeout">expiration time span</param>
        void Put(string key, object value, TimeSpan timeout);
        /// <summary>
        /// Add OR Replace cache item by key
        /// </summary>
        /// <param name="key">unique item id</param>
        /// <param name="value">cache data</param>
        /// <param name="timeout">expiration time span</param>
        /// <param name="area">named cache area</param>
        void Put(string key, object value, TimeSpan timeout, string area);

        /// <summary>
        /// Get cache item by key
        /// </summary>
        /// <param name="key">unique item id</param>
        /// <returns>cached data</returns>
        object Get(string key);
        /// <summary>
        /// Get cache item by key
        /// </summary>
        /// <param name="key">unique item id</param>
        /// <param name="area">named cache area</param>
        /// <returns>cached data</returns>
        object Get(string key, string area);
        /// <summary>
        /// Get cache item by key and slide expiration
        /// </summary>
        /// <param name="key">unique item id</param>
        /// <param name="timeout">expiration time span</param>
        /// <returns>cached data</returns>
        object Get(string key, TimeSpan timeout);
        /// <summary>
        /// Get cache item by key and slide expiration
        /// </summary>
        /// <param name="key">unique item id</param>
        /// <param name="timeout">expiration time span</param>
        /// <param name="area">named cache area</param>
        /// <returns>cached data</returns>
        object Get(string key, TimeSpan timeout, string area);

        /// <summary>
        /// Remove cache item by key
        /// </summary>
        /// <param name="key">unique item id</param>
        void Remove(string key);
        /// <summary>
        /// Remove cache item by key
        /// </summary>
        /// <param name="key">unique item id</param>
        /// <param name="area">named cache area</param>
        void Remove(string key, string area);

        /// <summary>
        /// Remove all cache items where key begins with the specified prefix
        /// </summary>
        /// <param name="keyPrefix">item prefix</param>
        void RemoveAllByKeyPrefix(string keyPrefix);

        /// <summary>
        /// Create cache area for segmentation
        /// </summary>
        /// <param name="area">named cache area</param>
        void CreateArea(string area);
        /// <summary>
        /// Remove cache area
        /// </summary>
        /// <param name="area">named cache area</param>
        void RemoveArea(string area);
    }
}
