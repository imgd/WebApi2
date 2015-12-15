using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace WebApi2.Demo.Inspector
{
    public class WebApiInspector
    {
        /// <summary>
        /// 身份请求头 key
        /// </summary>
        private const string InspectorHeaderName = "X-SourceToken";

        public BaseClientCheckFactory _inspectorFactory { get; private set; }
        public HttpRequestMessage _request { get; set; }
        public WebApiInspector(HttpRequestMessage request)
        {
            this._request = request;
            this._inspectorFactory = new BaseClientCheckFactory();
        }
        public WebApiInspector()
        {
            this._inspectorFactory = new BaseClientCheckFactory();
        }
        public bool IsPass()
        {
            var headers = _request.Headers;
            if (headers.Contains(InspectorHeaderName))
            {
                var token = headers.GetValues(InspectorHeaderName).FirstOrDefault();
                if (token == null)
                    return false;
                else
                    return _inspectorFactory.ClientIdentityCheck(token);
            }
            else
                return false;

        }
        public string GetToken(string key)
        {
            return _inspectorFactory.GetClientToken(key);
        }
    }
}
