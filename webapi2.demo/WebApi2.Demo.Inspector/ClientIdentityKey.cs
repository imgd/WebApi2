using System;
using System.Collections.Generic;
using System.Xml;
using WebApi2.Demo.Common;

namespace WebApi2.Demo.Inspector
{
    public static class ClientIdentityKey
    {
        private static string ConfigPath = AppDomain.CurrentDomain.BaseDirectory
                                             + @"Config/Inspector.config";
        private static string ConfigCacheKey_Expire = "IDENTITY_CACHE_KEY_Expire";
        private static string ConfigCacheKey_Keys = "IDENTITY_CACHE_KEY_Keys";
        private static string ConfigCacheKey_TokenKeys = "IDENTITY_CACHE_KEY_TokenKeys";
        private static string ConfigCacheFile = "IDENTITY_CACHE_File";

        /// <summary>
        /// 获取client身份key
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, string> GetClientKeys()
        {
            var keys = new Dictionary<string, string>();
            var data = ConfigCacheKey_Keys.FindCache_M();

            if (data.IsNull())
            {
                XmlHelper config = LoadConfig();
                if (config.LoadXmlSuccess())
                {
                    XmlNode clientNode = config.doc.DocumentElement.SelectSingleNode("client");

                    if (clientNode.IsNull())
                        throw new Exception("找不到Identity client节点配置");

                    var clientChildNodes = clientNode.ChildNodes;
                    foreach (XmlNode nd in clientChildNodes)
                    {
                        keys.Add(nd.Name, nd.InnerText);
                    }
                    keys.SetCache_M(ConfigCacheKey_Keys, ConfigPath);
                }
            }
            else
            {
                keys = data as Dictionary<string, string>;
            }

            return keys;
        }

        /// <summary>
        /// 获取Token keys 
        /// </summary>
        /// <returns></returns>
        public static string[] GetClientTokenKeys()
        {
            var data = ConfigCacheKey_TokenKeys.FindCache_M();

            if (data.IsNull())
            {
                XmlHelper config = LoadConfig();
                if (config.LoadXmlSuccess())
                {
                    XmlNode clientNode = config.doc.DocumentElement.SelectSingleNode("client");

                    if (clientNode.IsNull())
                        throw new Exception("找不到Identity client节点配置");


                    var clientChildNodes = clientNode.ChildNodes;
                    var keys = new string[clientChildNodes.Count];
                    for (int i = 0; i < clientChildNodes.Count; i++)
                    {
                        keys[i] = clientChildNodes[i].Name;
                    }

                    keys.SetCache_M(ConfigCacheKey_TokenKeys, ConfigPath);

                    return keys;
                }
                else
                {
                    return new string[] { };
                }
            }
            else
            {
                return data as string[];
            }
        }

        /// <summary>
        /// 获取身份验证超时时间
        /// </summary>
        /// <returns></returns>
        public static int ClientIdentityExpire()
        {
            int result = 0;
            var data = ConfigCacheKey_Expire.FindCache_M();
            if (data.IsNull())
            {
                XmlHelper config = LoadConfig();
                if (config.LoadXmlSuccess())
                {
                    XmlNode expireNode = config.doc.DocumentElement.SelectSingleNode("timeexpire");
                    if (expireNode.IsNull())
                    {
                        throw new Exception("找不到timeexpire节点配置");
                    }
                    result = Convert.ToInt32(expireNode.InnerText);
                    result.SetCache_M(ConfigCacheKey_Expire, ConfigPath);
                }
            }
            else
            {
                result = (int)data;
            }

            return result;
        }

        /// <summary>
        /// 加载配置文件
        /// </summary>
        /// <returns></returns>
        private static XmlHelper LoadConfig()
        {
            //初始化 配置文件
            XmlHelper xm = null;
            var data = ConfigCacheFile.FindCache_M();
            if (data.IsNull())
            {
                xm = new XmlHelper(ConfigPath);
                xm.SetCache_M(ConfigCacheFile, ConfigPath);
            }
            else
            {
                xm = data as XmlHelper;
            }
            return xm;
        }
    }
}
