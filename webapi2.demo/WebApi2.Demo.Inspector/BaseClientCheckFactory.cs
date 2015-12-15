using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApi2.Demo.Inspector
{
    public  class BaseClientCheckFactory : IClientCheck
    {
        private void InitTokens()
        {
            _tokens = ClientIdentityKey.GetClientKeys();
            _tokenKeys = ClientIdentityKey.GetClientTokenKeys();
            ClientTokenTool = new ClientTokens(_tokens, _tokenKeys);
        }

        public BaseClientCheckFactory(string clientToken)
        {
            this.ClientToken = clientToken;
            InitTokens();
        }
        public BaseClientCheckFactory()
        {            
            InitTokens();
        }

        /// <summary>
        /// 验证token
        /// </summary>
        /// <returns></returns>
        public bool ClientIdentityCheck()
        {
            return ClientTokenTool.CheckClientToken(ClientToken);
        }
        public bool ClientIdentityCheck(string clientToken)
        {
            return ClientTokenTool.CheckClientToken(clientToken);
        }

        /// <summary>
        /// 生成token
        /// </summary>
        /// <param name="clientkey"></param>
        /// <returns></returns>
        public string GetClientToken(string clientkey)
        {
            return ClientTokenTool.KeyEnCode(clientkey);
        }


        private ClientTokens ClientTokenTool
        {
            get;
            set;
        }

        private string ClientToken
        {
            get;
            set;
        }

        private Dictionary<string, string> _tokens;
        Dictionary<string, string> IClientCheck.Tokens
        {
            get
            {
                return _tokens;
            }
            set
            {
                _tokens = value;
            }
        }

        private string[] _tokenKeys;
        public string[] TokenKeys
        {
            get
            {
                return _tokenKeys;
            }
            set
            {
                _tokenKeys = value;
            }
        }
    }
}
