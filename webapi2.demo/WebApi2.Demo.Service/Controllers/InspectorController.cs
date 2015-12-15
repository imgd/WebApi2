using System.Web.Http;
using WebApi2.Demo.Inspector;

namespace WebApi2.Demo.Service.Controllers.inspector
{
    public class InspectorController : BaseApiController
    {       
        [HttpGet]
        //inspector/createtoken?clientkey=xwXXtotJVI9CeXxSWXN4GTY9bpw23DeFTjg7O2vztTZmuj74hdwdtN4gNsFIm7
        public string CreateToken(string clientKey)
        {
            return new WebApiInspector().GetToken(clientKey);
        }
    }
}