﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;

namespace WebApi2.Demo.OutputCache
{
    public class MemoryCacheDefault : IApiOutputCache
    {
        private static readonly MemoryCache Cache = MemoryCache.Default;

        public void RemoveStartsWith(string key)
        {
            lock (Cache)
            {
                Cache.Remove(key);
            }
        }

        public T Get<T>(string key) where T : class
        {
            var o = Cache.Get(key) as T;
            return o;
        }

        [Obsolete("Use Get<T> instead")]
        public object Get(string key)
        {
            return Cache.Get(key);
        }

        public void Remove(string key)
        {
            lock (Cache)
            {
                Cache.Remove(key);
            }
        }

        public bool Contains(string key)
        {
            return Cache.Contains(key);
        }

        public void Add(string key, object o, DateTimeOffset expiration, string dependsOnKey = null)
        {
            var cachePolicy = new CacheItemPolicy
            {
                AbsoluteExpiration = expiration
            };

            if (!string.IsNullOrWhiteSpace(dependsOnKey))
            {
                cachePolicy.ChangeMonitors.Add(
                    Cache.CreateCacheEntryChangeMonitor(new[] { dependsOnKey })
                );
            }
            lock (Cache)
            {
                Cache.Add(key, o, cachePolicy);
            }
        }

        public IEnumerable<string> AllKeys
        {
            get
            {
                return Cache.Select(x => x.Key);
            }
        }
    }
}