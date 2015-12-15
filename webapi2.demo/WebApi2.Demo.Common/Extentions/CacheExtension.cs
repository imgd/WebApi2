using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Caching;
using System.Runtime.Caching;
using System.IO;
using System.Data.SqlClient;


namespace WebApi2.Demo.Common
{
    //---------------------------------------------------------------
    //   缓存 扩展方法类                                                   
    // —————————————————————————————————————————————————             
    // | varsion 1.0                                   |             
    // | creat by gd 2014.7.31                         |
    // | edit2014.12.24 添加 memory cache               |
    // | 联系我:@大白2013 http://weibo.com/u/2239977692  |            
    // —————————————————————————————————————————————————             
    //                                                               
    // *使用说明：                                                    
    //    使用当前扩展类添加引用: using Extensions.CacheExtension;                      
    //    使用所有扩展类添加引用: using Extensions;                         
    // -------------------------------------------------------------- 
    public static class CacheExtension
    {
        #region System.Web.Caching

        #region Set
        /// <summary>
        /// 添加到缓存
        /// </summary>
        /// <param name="value">缓存值</param>
        /// <param name="cacheKey">缓存键</param>
        public static void SetCache(this object value, string cacheKey)
        {
            if (FindCache(cacheKey) == null)
            {
                HttpRuntime.Cache.Insert(cacheKey, value);
            }
        }
        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <param name="value">缓存值</param>
        /// <param name="cacheKey">缓存键</param>
        /// <param name="isReplace">缓存存在是否替换</param>
        public static void SetCache(this object value, string cacheKey, bool isReplace)
        {
            RemoveCache(cacheKey);
            SetCache(value, cacheKey);
        }

        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <param name="value">缓存值</param>
        /// <param name="cacheKey">缓存键</param>
        /// <param name="dependency">缓存依赖项(sql表依赖 请使用 SqlCacheDependency )</param>
        public static void SetCache(this object value, string cacheKey, CacheDependency dependency)
        {
            if (FindCache(cacheKey) == null)
            {
                HttpRuntime.Cache.Insert(cacheKey, value);
            }
        }
        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <param name="value">缓存值</param>
        /// <param name="cacheKey">缓存键</param>
        /// <param name="dependency">缓存依赖项(sql表依赖 请使用 SqlCacheDependency )</param>
        /// <param name="expriationTime">缓存滑动(连续访问时间)过期时间</param>
        public static void SetCache(this object value, string cacheKey, CacheDependency dependency, TimeSpan expriationTime)
        {
            if (FindCache(cacheKey) == null)
            {
                HttpRuntime.Cache.Insert(cacheKey, value, dependency, System.Web.Caching.Cache.NoAbsoluteExpiration, expriationTime);
            }
        }
        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <param name="value">缓存值</param>
        /// <param name="cacheKey">缓存键</param>
        /// <param name="dependency">缓存依赖项(sql表依赖 请使用 SqlCacheDependency )</param>
        /// <param name="expriationTime">缓存绝对过期时间</param>
        public static void SetCache(this object value, string cacheKey, CacheDependency dependency, DateTime expriationTime)
        {
            if (FindCache(cacheKey) == null)
            {
                HttpRuntime.Cache.Insert(cacheKey, value, dependency, expriationTime, TimeSpan.Zero);
            }
        }

        #endregion

        #region Remove
        /// <summary>
        /// 移除缓存
        /// </summary>
        /// <param name="cacheKey">缓存键</param>
        public static void RemoveCache(this string cacheKey)
        {
            if (FindCache(cacheKey) != null)
            {
                HttpRuntime.Cache.Remove(cacheKey);
            }
        }
        #endregion

        #region Update

        /// <summary>
        /// 更新缓存值
        /// </summary>
        /// <param name="value">缓存值</param>
        /// <param name="cacheKey">缓存键值</param>
        public static void UpdateSertCache(this object value, string cacheKey)
        {
            if (FindCache(cacheKey) == null)
            {
                SetCache(value, cacheKey);
            }
            else
            {
                HttpRuntime.Cache[cacheKey] = value;
            }
        }

        #endregion

        #region Find
        /// <summary>
        /// 获取缓存值
        /// </summary>
        /// <param name="cacheKey">缓存键</param>
        /// <returns></returns>
        public static object FindCache(this string cacheKey)
        {
            return HttpRuntime.Cache[cacheKey];
        }

        #endregion

        #endregion

        #region System.Runtime.Caching [.net 4.0]

        #region Set

        /// <summary>
        /// 添加到缓存
        /// </summary>
        /// <param name="value">被缓存的数据</param>
        /// <param name="cacheKey">缓存key</param>
        public static void SetCache_M(this object value, string cacheKey)
        {
            ObjectCache cache = MemoryCache.Default;
            cache.Set(cacheKey, value, new CacheItemPolicy());
        }

        /// <summary>
        /// 添加到缓存
        /// </summary>
        /// <param name="value">被缓存的数据</param>
        /// <param name="cacheKey">缓存key</param>
        /// <param name="expriationTime">缓存绝对过期时间[指定某个时间点]</param>
        public static void SetCache_M(this object value, string cacheKey, DateTime expriationTime)
        {
            ObjectCache cache = MemoryCache.Default;
            cache.Set(cacheKey, value, expriationTime);
        }

        /// <summary>
        /// 添加到缓存
        /// </summary>
        /// <param name="value">被缓存的数据</param>
        /// <param name="cacheKey">缓存key</param>
        /// <param name="expriationTime">缓存相对过期时间[当前某个时间段后]</param>
        public static void SetCache_M(this object value, string cacheKey, DateTimeOffset expriationTime)
        {
            ObjectCache cache = MemoryCache.Default;
            CacheItemPolicy policy = new CacheItemPolicy();
            //设置时间点过期
            policy.AbsoluteExpiration = expriationTime;
            cache.Set(cacheKey, value, policy);
        }
        /// <summary>
        /// 添加到缓存
        /// </summary>
        /// <param name="value">被缓存的数据</param>
        /// <param name="cacheKey">缓存key</param>
        /// <param name="expriationTime">缓存相对过期时间[指定持续时间未被访问]</param>
        public static void SetCache_M(this object value, string cacheKey, TimeSpan expriationTime)
        {
            ObjectCache cache = MemoryCache.Default;
            CacheItemPolicy policy = new CacheItemPolicy();
            //设置时间点过期
            policy.SlidingExpiration = expriationTime;
            cache.Set(cacheKey, value, policy);
        }

        /// <summary>
        /// 添加到缓存
        /// </summary>
        /// <param name="value">被缓存的数据</param>
        /// <param name="cacheKey">缓存key</param>
        /// <param name="filePath">依赖缓存文件的绝对路径</param>
        public static void SetCache_M(this object value, string cacheKey, string dependency_filePath)
        {
            if (!File.Exists(dependency_filePath))
            {
                throw new FileNotFoundException("缓存依赖文件不存在");
            }

            ObjectCache cache = MemoryCache.Default;
            CacheItemPolicy policy = new CacheItemPolicy();

            //缓存优先级别
            policy.Priority = System.Runtime.Caching.CacheItemPriority.Default;

            cache.Set(cacheKey, value, policy);

            //设置监视对象
            HostFileChangeMonitor monitor = new HostFileChangeMonitor(new List<string>() { dependency_filePath });
            //设置监视对象的回调操作
            //依赖文件发生变化 即删除缓存
            monitor.NotifyOnChanged(new OnChangedCallback(o =>
            {
                RemoveCache_M(cacheKey);
            }));
            //添加到监视器
            policy.ChangeMonitors.Add(monitor);


        }

        /// <summary>
        /// 添加到缓存
        /// </summary>
        /// <param name="value">被缓存的数据</param>
        /// <param name="cacheKey">缓存key</param>
        /// <param name="depency">SQL依赖缓存项</param>
        public static void SetCache_M(this object value, string cacheKey, SqlDependency dependency_sql)
        {

            ObjectCache cache = MemoryCache.Default;
            CacheItemPolicy policy = new CacheItemPolicy();

            //缓存优先级别
            policy.Priority = System.Runtime.Caching.CacheItemPriority.Default;

            cache.Set(cacheKey, value, policy);

            //设置监视对象            
            SqlChangeMonitor monitor = new SqlChangeMonitor(dependency_sql);

            //设置监视对象的回调操作
            //依赖文件发生变化 即删除缓存
            monitor.NotifyOnChanged(new OnChangedCallback(o =>
            {
                RemoveCache_M(cacheKey);
            }));
            //添加到监视器
            policy.ChangeMonitors.Add(monitor);


        }

        #endregion

        #region Remove

        /// <summary>
        /// 移除缓存
        /// </summary>
        /// <param name="cacheKey">缓存键</param>
        public static void RemoveCache_M(this string cacheKey)
        {
            ObjectCache cache = MemoryCache.Default;
            if (cache.Contains(cacheKey))
            {
                cache.Remove(cacheKey);
            }
        }

        #endregion

        #region Update

        /// <summary>
        /// 插入/更新缓存值
        /// </summary>
        /// <param name="value">缓存值</param>
        /// <param name="cacheKey">缓存键值</param>
        public static void UpdateSertCache_M(this object value, string cacheKey)
        {
            ObjectCache cache = MemoryCache.Default;
            if (!cache.Contains(cacheKey))
            {
                RemoveCache_M(cacheKey);
            }

            SetCache_M(value, cacheKey);
        }

        #endregion

        #region Find


        /// <summary>
        /// 获取缓存值
        /// </summary>
        /// <param name="cacheKey">缓存键</param>
        /// <returns></returns>
        public static object FindCache_M(this string cacheKey)
        {
            ObjectCache cache = MemoryCache.Default;
            return cache.Get(cacheKey);
        }

        /// <summary>
        /// 缓存数据并且返回---------待修改
        /// **key命名考虑不能冲突**
        /// </summary>
        /// <param name="cacheValue">缓存数据</param>
        /// <param name="cacheKey">缓存key</param>
        /// <returns></returns>
        public static object ObjectSetGetCache(this string cacheKey, object objectValue)
        {
            object value = cacheKey.FindCache_M();
            if (value == null)
            {
                objectValue.SetCache_M(cacheKey);
                return objectValue;
            }
            else
            {
                return value;
            }
        }
        /// <summary>
        /// 缓存数据并且返回---------待修改
        /// **key命名考虑不能冲突**
        /// </summary>
        /// <param name="cacheValue">缓存数据</param>
        /// <param name="cacheKey">缓存key</param>
        /// <param name="expiredTime">设置缓存绝对过期时间</param>
        /// <returns></returns>
        public static object ObjectSetGetCache(this string cacheKey, object objectValue ,DateTime expiredTime)
        {
            object value = cacheKey.FindCache_M();
            if (value == null)
            {
                objectValue.SetCache_M(cacheKey, expiredTime);
                return objectValue;
            }
            else
            {
                return value;
            }
        }

        #endregion

        #endregion
    }
}
