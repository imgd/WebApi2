using System;
using System.Web.Http;

namespace WebApi2.Demo.Service.Controllers.web
{
    /// <summary>
    /// version的初始版本
    /// </summary>
    public class VersionController : BaseApiController
    {
        public string Get()
        {
            return "version=v1.0" + "=" + DateTime.Now.ToString();
        }
        public string Getv1()
        {
            return "version=v1";
        }
    }
}