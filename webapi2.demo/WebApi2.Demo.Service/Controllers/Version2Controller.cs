using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using WebApi2.Demo.OutputCache;

namespace WebApi2.Demo.Service.Controllers.web
{
    /// <summary>
    /// version 1.1.0.2版本
    /// </summary>    
    public class VersionV1_1_0_2Controller : BaseApiController
    {
        [Obsolete()]
        [CacheOutput(ServerTimeSpan = 100, ClientTimeSpan = 100)]
        public string Get()
        {
            return "version=v1.1.0.2" + "=" + DateTime.Now.ToString();
        }
    }
}