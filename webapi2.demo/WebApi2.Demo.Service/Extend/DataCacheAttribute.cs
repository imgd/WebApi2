using System;
using System.Linq;
using System.Net;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using WebApi2.Demo.Common;

namespace WebApi2.Demo.Service
{
    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class DataCacheAttribute : Attribute
    {

        public int CacheSeconds { get; set; }

        public DataCacheAttribute()
        {
            CacheSeconds = 60;
        }

        public DataCacheAttribute(int CacheSeconds)
        {
            this.CacheSeconds = CacheSeconds;
        }
    }
}