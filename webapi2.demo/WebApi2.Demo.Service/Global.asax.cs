using System.Web;
using System.Web.Http;

namespace WebApi2.Demo.Service
{
    // 注意: 有关启用 IIS6 或 IIS7 经典模式的说明，
    // 请访问 http://go.microsoft.com/?LinkId=9394801

    public class WebApiApplication : HttpApplication
    {
        protected void Application_Start()
        {
            // WebApiConfig.Register(GlobalConfiguration.Configuration);
            GlobalConfiguration.Configure(WebApiConfig.Register);            
        }

        protected void Application_BeginRequest()
        {            
           //这里可以实现每次方法请求的操作           
        }

        protected void Application_Error()
        {
            //这里可以实现每次服务器异常的处理操作
        }

    }
}