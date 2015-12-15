using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using WebApi2.Demo.Common;
using WebApi2.Demo.Inspector;

namespace WebApi2.Demo.Service
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class GlobalActionFilterAttribute : ActionFilterAttribute
    {
        public bool authenticationSwitch { get; set; }
        public bool sqlinjectionSwitch { get; set; }
        public GlobalActionFilterAttribute()
        {
            this.authenticationSwitch = true;
            this.sqlinjectionSwitch = true;
        }
        public GlobalActionFilterAttribute(bool authenticationSwitch, bool sqlinjectionSwitch)
        {
            this.authenticationSwitch = authenticationSwitch;
            this.sqlinjectionSwitch = sqlinjectionSwitch;
        }

        public override void OnActionExecuting(HttpActionContext actionContext)
        {

            //全局身份验证
            if (authenticationSwitch)
            {
                //未授权访问
                if (!EdentityPass(actionContext))
                {
                    //logger
                    string.Format("客户端[IP:{0}] 请求身份验证失败。请求来源：{1}",
                        actionContext.Request.GetClientIp(),
                        actionContext.Request.RequestUri.ToString())
                        .LogError();

                    actionContext.Response =
                        ResponseWrite("ERROR:401 身份验证失败，服务器已拒绝。", HttpStatusCode.Unauthorized);

                }

            }
            //请求参数含有非法字符
            else if (sqlinjectionSwitch)
            {
                //非法请求
                if (!IsParametersSafe(actionContext))
                {
                    //logger
                    string.Format("客户端[IP:{0}] 请求参数含有SQL注入字符。请求来源：{1}",
                    actionContext.Request.GetClientIp(),
                    actionContext.Request.RequestUri.ToString())
                    .LogError();

                    actionContext.Response =
                        ResponseWrite("ERROR:400 请求参数含有不合法字符，请检查后重试。", HttpStatusCode.BadRequest);

                }
            }

            //抛出参数验证
            if (!actionContext.ModelState.IsValid)
            {
                actionContext.Response = actionContext.Request.CreateErrorResponse(
                    HttpStatusCode.BadRequest,
                    actionContext.ModelState);
            }

            base.OnActionExecuting(actionContext);
        }
        /// <summary>
        /// 这里可以扩展 操作发生之后的行为
        /// </summary>
        /// <param name="actionExecutedContext"></param>
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            base.OnActionExecuted(actionExecutedContext);
        }


        /// <summary>
        /// 身份验证
        /// </summary>
        /// <param name="actionContext"></param>
        private bool EdentityPass(HttpActionContext actionContext)
        {

            if (actionContext.Request.RequestUri.LocalPath.IsInArray<string>(new string[] { "/inspector/inspector/createtoken" }))
                return true;

            WebApiInspector inspector = new WebApiInspector(actionContext.Request);
            return inspector.IsPass();
        }

        /// <summary>
        /// 请求参数验证
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        private bool IsParametersSafe(HttpActionContext context)
        {
            //获取传统context
            HttpContextBase CON = (HttpContextBase)context.Request.Properties["MS_HttpContext"];
            if (CON.IsNull())
            {
                return true;
            }
            HttpRequestBase request = CON.Request;
            var method = request.HttpMethod.ToString().ToUpper();

            NameValueCollection paras = method == "GET" ? request.QueryString : request.Form;
            
            foreach (string item in paras)
            {
                if (!paras[item].ToString().IsSqlTextSafe())
                    return false;
                else
                    continue;
            }
            return true;
        }
        private HttpResponseMessage ResponseWrite(string contentmsg, HttpStatusCode statuscode)
        {
            return new HttpResponseMessage()
            {
                Content = new StringContent(contentmsg),
                StatusCode = statuscode
            };
        }

        #region 测试方法，暂不调用
        /// <summary>
        /// 该方法是否是jsonp响应
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Obsolete("目前还在测试中", true)]
        private bool IsJsonpRequest(HttpRequestMessage request)
        {
            HttpConfiguration configuration = request.GetConfiguration();
            HttpControllerDescriptor controllerDescriptor =
                configuration.Services.GetHttpControllerSelector().SelectController(request);

            HttpControllerContext controllerContext =
                new HttpControllerContext(request.GetConfiguration(), request.GetRouteData(), request)
                {
                    ControllerDescriptor = controllerDescriptor
                };

            HttpActionDescriptor actionDescriptor =
                configuration.Services.GetActionSelector().SelectAction(controllerContext);

            JsonpFormatterAttribute jsonpattribute =
                actionDescriptor.GetCustomAttributes<JsonpFormatterAttribute>().FirstOrDefault() ??
                controllerDescriptor.GetCustomAttributes<JsonpFormatterAttribute>().FirstOrDefault();

            if (jsonpattribute.IsNull())
                return false;

            else
                return true;

        }

        [Obsolete("目前还在测试中", true)]
        private bool IsJsonp(out string callback)
        {
            callback = HttpContext.Current.Request.QueryString["callback"];
            return !string.IsNullOrEmpty(callback);
        }

        [Obsolete("目前还在测试中", true)]
        private HttpResponseMessage JsonpResponse(string backdata, HttpStatusCode statuscode, string callback)
        {
            var response = new HttpResponseMessage();
            var jsonBuilder = new StringBuilder(callback);
            jsonBuilder.AppendFormat("({0})", backdata);
            response.Content = new StringContent(jsonBuilder.ToString());
            response.StatusCode = statuscode;
            return response;
        }
        [Obsolete("目前还在测试中", true)]
        private HttpResponseMessage ResponseWrite(HttpRequestMessage request, string contentmsg, HttpStatusCode statuscode)
        {
            var cakkback = string.Empty;

            if (IsJsonp(out cakkback))
            {
                return JsonpResponse(string.Format("\"{0}\"", contentmsg), statuscode, cakkback);
            }
            else
            {
                return new HttpResponseMessage()
                {
                    Content = new StringContent(contentmsg),
                    StatusCode = statuscode
                };
            }
        }

        #endregion

    }
}