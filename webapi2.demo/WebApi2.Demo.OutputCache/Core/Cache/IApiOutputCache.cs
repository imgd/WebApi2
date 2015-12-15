using System;
using System.Collections.Generic;

namespace WebApi2.Demo.OutputCache
{
    public interface IApiOutputCache
    {
        void RemoveStartsWith(string key);

        T Get<T>(string key) where T : class;

        [Obsolete("Use Get<T> instead")]
        object Get(string key);

        void Remove(string key);

        bool Contains(string key);

        void Add(string key, object o, DateTimeOffset expiration, string dependsOnKey = null);

        IEnumerable<string> AllKeys { get; }
    }
}