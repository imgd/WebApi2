using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace WebApi2.Demo.Common
{
    public static class CacheContainer
    {
        private static Hashtable Data = new Hashtable();
        private static Mutex DataLock = new Mutex();
        private const int MaxCacheCount = 3000;

        public static bool SetData(string key, object value, int seconds)
        {
            bool successLock = DataLock.WaitOne(30000);
            if (!successLock)
            {
                return false;
            }


            if (Data[key] != null)
            {
                if (value == null)
                {
                    Data.Remove(key);
                }
                else
                {
                    Data[key] = new DataCacheUnit
                    {
                        CacheData = value,
                        ExpTime = DateTime.Now.AddSeconds(seconds)
                    };
                }
            }
            else
            {
                if (Data.Count >= MaxCacheCount)
                {
                    DataLock.ReleaseMutex();
                    return false;
                }

                Data.Add(key, new DataCacheUnit
                {
                    CacheData = value,
                    ExpTime = DateTime.Now.AddSeconds(seconds)
                });
            }
            DataLock.ReleaseMutex();
            return true;
        }

        public static T GetData<T>(string key)
        {
            if (Data[key] == null)
            {
                return default(T);
            }

            if (DateTime.Now > ((DataCacheUnit)Data[key]).ExpTime)
            {
                return default(T);
            }
            else
            {
                return (T)((DataCacheUnit)Data[key]).CacheData;
            }
        }

        public static bool ClearUp()
        {
            bool successLock = DataLock.WaitOne(30000);
            if (!successLock)
            {
                return false;
            }

            DateTime now = DateTime.Now;
            List<string> delKeyList = new List<string>();
            foreach (string key in Data.Keys)
            {
                if (now > ((DataCacheUnit)Data[key]).ExpTime)
                {
                    delKeyList.Add(key);
                }
            }

            foreach (string key in delKeyList)
            {
                Data.Remove(key);
            }

            DataLock.ReleaseMutex();
            return true;
        }

        private class DataCacheUnit
        {
            public object CacheData { get; set; }
            public DateTime ExpTime { get; set; }
        }
    }
}
