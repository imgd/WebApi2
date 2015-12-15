using System;
using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;
using WebApi2.Demo.Common;


namespace WebApi2.Demo.Service
{
    [AttributeUsage(AttributeTargets.Assembly)]
    public class GlobalExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public bool errorSwitch { get; set; }
        public bool loggerSwitch { get; set; }

        public GlobalExceptionFilterAttribute() {
            this.errorSwitch = true;
            this.loggerSwitch = true;
        }
        public GlobalExceptionFilterAttribute(bool errorSwitch,bool loggerSwitch) {            
            this.errorSwitch = errorSwitch;
            this.loggerSwitch = loggerSwitch;
        }

        public override void OnException(HttpActionExecutedContext context)
        {
            Exception ex = context.Exception;
            string method = ex.TargetSite.Name;
            string message = ex.Message.ToString();

            if (loggerSwitch)
            {    
                //记录异常到日志
                string.Format("客户端[IP:{0}]请求错误,来源:{1}\r\n描述：{2}\r\n详细：",
                    context.Request.GetClientIp(),
                    context.Request.RequestUri.ToString(),
                    message).LogError(ex);
            }

            if (errorSwitch)
            {
                //自定义返回错误消息到client
                var response = new HttpResponseMessage();
                response.Content = new StringContent(string.Format("REQUEST ERROR!! ((‵□′)) \r\n描述: {0}\r\n来源: {1}",
                     message, context.Request.RequestUri.ToString()));
                response.StatusCode = HttpStatusCode.BadRequest;
                context.Response = response;
            }                        
        }
    }
}