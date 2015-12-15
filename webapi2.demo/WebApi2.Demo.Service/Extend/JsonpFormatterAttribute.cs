﻿using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http.Filters;

namespace WebApi2.Demo.Service
{
    /// <summary>
    /// Service Jsonp extend
    /// </summary>
    public class JsonpFormatterAttribute : ActionFilterAttribute
    {
        //jsonp parakey value
        private const string CallbackQueryParameter = "callback";
        public override void OnActionExecuted(HttpActionExecutedContext context)
        {
            var callback = string.Empty;

            if (IsJsonp(out callback))
            {
                var jsonBuilder = new StringBuilder(callback);

                jsonBuilder.AppendFormat("({0})", context.Response.Content.ReadAsStringAsync().Result);

                context.Response.Content = new StringContent(jsonBuilder.ToString());
            }

            base.OnActionExecuted(context);
        }
        private bool IsJsonp(out string callback)
        {
            callback = HttpContext.Current.Request.QueryString[CallbackQueryParameter];

            return !string.IsNullOrEmpty(callback);
        }
    }
}