using System;
using log4net;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]
namespace WebApi2.Demo.Common
{
    /// <summary>
    /// 如发现版本问题 请在官网下载相应版本
    /// Version:.net fw 4.5
    /// http://logging.apache.org/log4net/download_log4net.cgi
    /// </summary>
    public static class Log4jHelper
    {
        private static readonly ILog logInfo = LogManager.GetLogger("loginfo");
        private static readonly ILog logError = LogManager.GetLogger("logerror");

        /// <summary>
        /// 写入信息日志记录
        /// </summary>
        /// <param name="info"></param>
        public static void LogInfo(this string info)
        {
            logInfo.Info(info);
        }

        /// <summary>
        /// 写入错误日志纪录
        /// </summary>
        /// <param name="error"></param>
        /// <param name="ex"></param>
        public static void LogError(this string error, Exception ex)
        {
            logError.Error(error, ex);
        }
        /// <summary>
        /// 写入错误日志纪录
        /// </summary>
        /// <param name="error"></param>        
        public static void LogError(this string error)
        {
            logError.Error(error);
        }


        /// <summary>
        /// 自定义类型错误
        /// </summary>
        /// <param name="error"></param>
        /// <param name="type"></param>
        public static void LogError(this string error, Type type)
        {
            ILog log = LogManager.GetLogger(type);
            log.Error(error);
        }
        /// <summary>
        /// 自定义类型错误
        /// </summary>
        /// <param name="error"></param>
        /// <param name="type"></param>
        /// <param name="ex"></param>
        public static void LogError(this string error, Type type, Exception ex)
        {
            ILog log = LogManager.GetLogger(type);
            log.Error(error, ex);
        }
    }

}