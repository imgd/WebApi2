using System;
using System.Configuration;

namespace WebApi2.Demo.Common
{
    public static class ConfigHelper
    {
        /*ConnectionStrings节点*/
        public static string GetConfigConnnString(string configName)
        {
            try
            {
                return ConfigurationManager.ConnectionStrings[configName].ToString();
            }
            catch (Exception ex)
            {
                string.Format("{1}节点下{0}节点为空，请检查config!",configName, "connectionStrings")
                    .LogError(typeof(ConfigHelper), ex);
                return string.Empty;
            }
        }
        public static string GetConfigConnnString(string configName, string defaultName)
        {
            try
            {
                return ConfigurationManager.ConnectionStrings[configName].ToString();
            }
            catch (Exception ex)
            {
                string.Format("{1}节点下{0}节点为空，请检查config!", configName, "connectionStrings")
                    .LogError(typeof(ConfigHelper), ex);

                return defaultName;
            }
        }
        /*AppSettings节点*/
        public static string GetAppSettingsString(string configName)
        {
            try
            {
                return ConfigurationManager.AppSettings[configName].ToString();
            }
            catch (Exception ex)
            {
                string.Format("{1}节点下{0}节点为空，请检查config!", configName, "appSettings")
                    .LogError(typeof(ConfigHelper), ex);
                return string.Empty;
            }
        }
        public static string GetAppSettingsString(string configName, string defaultName)
        {
            try
            {                
                return ConfigurationManager.AppSettings[configName].ToString();
            }
            catch (Exception ex)
            {
                string.Format("{1}节点下{0}节点为空，请检查config!", configName, "appSettings")
                    .LogError(typeof(ConfigHelper), ex);

                return defaultName;
            }
        }
    }
}