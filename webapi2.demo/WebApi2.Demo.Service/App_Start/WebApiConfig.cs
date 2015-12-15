using System.Web.Http;
using WebApi2.Demo.Compress;
using WebApi2.Demo.Compress.Compressors;
using WebApi2.Demo.Common;
using System.Web.Http.Dispatcher;
using System.Net.Http.Formatting;

namespace WebApi2.Demo.Service
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            //允许特性注入路由
            //此项设置如果配置必须放在MapHttpRoute前面
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "{namespaces}/{controller}/{action}",
                defaults: new { namespaces = string.Format("WebApi2.Demo.Service.Controllers.{0}", RouteParameter.Optional) }
            );

            

            //全局功能处理配置开关
            //需要注意的是 此配置是注入到服务器启动事件，如果修改配置则需要重启服务器

            var GLOBAL_CUSTOMERROR_SWITCH = ConfigHelper.GetAppSettingsString("GLOBAL_CUSTOMERROR_SWITCH").ToString().ToLower() == "on" ? true : false;
            var GLOBAL_LOGGER_SWITCH = ConfigHelper.GetAppSettingsString("GLOBAL_LOGGER_SWITCH").ToString().ToLower() == "on" ? true : false;
            var GLOBAL_AUTHENTICATION_SWITCH = ConfigHelper.GetAppSettingsString("GLOBAL_AUTHENTICATION_SWITCH").ToString().ToLower() == "on" ? true : false;
            var GLOBAL_SQLINJECTION_SWITCH = ConfigHelper.GetAppSettingsString("GLOBAL_SQLINJECTION_SWITCH").ToString().ToLower() == "on" ? true : false;
            

            //消息JSON返回
            config.Services.Replace(typeof(IContentNegotiator), new JsonContentNegotiator());

            //Version版本支持
            config.Services.Replace(typeof(IHttpControllerSelector), new NamespaceHttpControllerSelector(config));

            //全局异常捕获
            config.Filters.Add(new GlobalExceptionFilterAttribute(GLOBAL_LOGGER_SWITCH,GLOBAL_LOGGER_SWITCH));

            //全局身份验证
            config.Filters.Add(new GlobalActionFilterAttribute(GLOBAL_AUTHENTICATION_SWITCH,GLOBAL_SQLINJECTION_SWITCH));
                       
            //全局消息压缩
            //压缩阀值 返回主体内容1000字节以上采用压缩
            config.MessageHandlers.Insert(0, new ServerCompressionHandler(1000, new GZipCompressor(), new DeflateCompressor()));
                        
        }
    }
}
