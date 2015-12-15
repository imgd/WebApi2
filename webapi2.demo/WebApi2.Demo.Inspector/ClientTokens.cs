using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApi2.Demo.Common;

namespace WebApi2.Demo.Inspector
{
    public class ClientTokens
    {
        /// <summary>
        /// 加密公钥
        /// </summary>
        private readonly string EnPublicKey = ConfigHelper.GetAppSettingsString("PUBLICKEY_TOKEN");

        /// <summary>
        /// token过期时间 单位/秒
        /// </summary>
        private readonly int IdentityExpire = ClientIdentityKey.ClientIdentityExpire();

        /// <summary>
        /// client的token信息
        /// </summary>
        private Dictionary<string, string> tokens { get; set; }
        /// <summary>
        /// client token键信息
        /// </summary>
        private string[] tokenKeys { get; set; }

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="tokens"></param>
        public ClientTokens(Dictionary<string, string> tokens, string[] tokenKeys)
        {
            this.tokens = tokens;
            this.tokenKeys = tokenKeys;

        }
        /// <summary>
        /// 验证token是否 合法
        /// </summary>
        /// <param name="token">客户端的token</param>
        /// <returns></returns>
        public bool CheckClientToken(string token)
        {
            string[] tokenarr = token.IsNull() ?
                new string[] { } :
                token.DESDecrypt(EnPublicKey).Split(',');

            if (tokenarr.Length != 3 || !KeyIsInArray(tokenarr[1]))
            {
                return false;
            }

            return tokens.ContainsKey(tokenarr[1])
                && tokens.ContainsValue(tokenarr[2])
                && IsIdentityExpire(tokenarr[0]);
        }

        /// <summary>
        /// 返回当前时间节点token的加密信息
        /// </summary>
        /// <param name="key">client身份key</param>
        /// <param name="token">client身份token</param>
        /// <returns></returns>
        public string KeyEnCode(string clientkey)
        {
            //不存在的key
            if (!tokens.Values.Contains(clientkey))
            {                
                return string.Empty;
            }

            //如果失效那么根据clientkey重新生成一次
            var key = tokens.Where(o => o.Value == clientkey).
                FirstOrDefault().Key;

            return (DateTime.Now.AddSeconds(IdentityExpire).
                    DateTimeConvertTimeStamp() + ","
                    + key + ","
                    + clientkey)
                    .DESEncrypt(EnPublicKey);
        }

        /// <summary>
        /// 判断token是否过期
        /// </summary>
        /// <param name="timeStamp">token的时间戳</param>
        /// <returns></returns>
        private bool IsIdentityExpire(string timeStamp)
        {
            return timeStamp.TimeStampConvertDateTime() >= DateTime.Now;
        }

        /// <summary>
        /// 验证客户端的身份类型
        /// 例：web
        /// </summary>
        /// <param name="key">客户端的身份类型</param>
        /// <returns></returns>
        private bool KeyIsInArray(string key)
        {
            return key.IsInArray(tokenKeys);
        }

    }
}
