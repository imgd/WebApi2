using System;
using System.Linq;
using System.Text;
using WebApi2.Demo.Common;


namespace WebApi2.Demo.Service.Extend
{

    /// <summary>
    /// 调用操作的工具类
    /// </summary>
    public static class DataCacheActionInvoker
    {
        public static T Invoke<T>(object target, string actionName, Func<T> method, object @params = null, bool cancel = false)
        {
            if (cancel)
            {
                return method.Invoke();
            }

            var targetType = target.GetType();
            var targetInfo = targetType.GetMethod(actionName);

            if (targetInfo.IsDefined(typeof(DataCacheAttribute), true))
            {
                var attr = targetInfo
                    .GetCustomAttributes(typeof(DataCacheAttribute), true)
                    .OfType<DataCacheAttribute>()
                    .FirstOrDefault();

                StringBuilder sbKey = new StringBuilder();
                sbKey.Append(targetType.Name);
                sbKey.Append("-");
                sbKey.Append(actionName);

                if (@params != null)
                {
                    var pros = @params.GetType().GetProperties();
                    foreach (var pro in pros.OrderBy(item => item.Name))
                    {
                        var value = pro.GetValue(@params);
                        if (value != null)
                        {
                            sbKey.Append("-");
                            sbKey.Append(pro.Name);
                            sbKey.Append("-");
                            sbKey.Append(value.ToString());
                        }
                    }
                }

                string key = sbKey.ToString();

                T data = CacheContainer.GetData<T>(key);
                if (data != null)
                {
                    return (T)data;
                }
                else
                {
                    var rsl = method.Invoke();

                    CacheContainer.SetData(key, rsl, attr.CacheSeconds);
                    return rsl;
                }
            }
            else
            {
                return method.Invoke();
            }


        }
    }
}