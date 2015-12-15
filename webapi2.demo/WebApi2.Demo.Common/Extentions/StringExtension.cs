using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.IO;
using System.Net.Http;
using System.Web;
using System.ServiceModel.Channels;

namespace WebApi2.Demo.Common
{
    //---------------------------------------------------------------
    //   string 扩展方法类                                                   
    // —————————————————————————————————————————————————             
    // | varsion 1.0                                   |             
    // | creat by gd 2014.7.31                         |             
    // | 联系我:@大白2013 http://weibo.com/u/2239977692 |            
    // —————————————————————————————————————————————————             
    //                                                               
    // *使用说明：                                                    
    //    使用当前扩展类添加引用: using Extensions.StringExtension;                      
    //    使用所有扩展类添加引用: using Extensions;                         
    // --------------------------------------------------------------

    public static class StringExtension
    {
        #region 字符串处理

        #region 字符串转码
        /// <summary>
        /// 中文转unicode
        /// </summary>
        /// <returns></returns>
        public static string StrDecode(this string str)
        {
            string outStr = "";
            if (!string.IsNullOrEmpty(str))
            {
                for (int i = 0; i < str.Length; i++)
                {
                    outStr += "/u" + ((int)str[i]).ToString("x");
                }
            }
            return outStr;
        }
        /// <summary>
        /// unicode转中文
        /// </summary>
        /// <returns></returns>
        public static string StrEncode(this string str)
        {
            string outStr = "";
            if (!string.IsNullOrEmpty(str))
            {
                string[] strlist = str.Replace("/", "").Split('u');
                try
                {
                    for (int i = 1; i < strlist.Length; i++)
                    {
                        //将unicode字符转为10进制整数，然后转为char中文字符  
                        outStr += (char)int.Parse(strlist[i], System.Globalization.NumberStyles.HexNumber);
                    }
                }
                catch (FormatException ex)
                {
                    outStr = ex.Message;
                }
            }
            return outStr;

        }


        /// <summary>
        /// unicode转中文（符合js规则的）
        /// </summary>
        /// <returns></returns>
        public static string StrJsDecode(this string str)
        {
            string outStr = "";
            Regex reg = new Regex(@"(?i)\\u([0-9a-f]{4})");
            outStr = reg.Replace(str, delegate(Match m1)
            {
                return ((char)Convert.ToInt32(m1.Groups[1].Value, 16)).ToString();
            });
            return outStr;
        }
        /// <summary>
        /// 中文转unicode（符合js规则的）
        /// </summary>
        /// <returns></returns>
        public static string StrJsEncode(this string str)
        {
            string outStr = "";
            if (!string.IsNullOrEmpty(str))
            {
                for (int i = 0; i < str.Length; i++)
                {
                    if (Regex.IsMatch(str[i].ToString(), @"[\u4e00-\u9fa5]")) { outStr += "\\u" + ((int)str[i]).ToString("x"); }
                    else { outStr += str[i]; }
                }
            }
            return outStr;
        }
        #endregion

        #region 字符串截取

        /// <summary>
        /// 返回截取固定长度字符串
        /// </summary>
        /// <param name="queryString"></param>
        /// <param name="num"></param>
        /// <returns></returns>
        public static string SubString(this string queryString, int num)
        {
            if (queryString == null)
                return string.Empty;
            num = num > queryString.Length ? queryString.Length : num;

            return queryString.Substring(0, num);
        }
        /// <summary>
        /// 返回截取固定长度字符串
        /// </summary>
        /// <param name="queryString"></param>
        /// <param name="num"></param>
        /// <returns></returns>
        public static string SubString(this string queryString, int num, string defaultstr)
        {
            if (queryString == null)
                return string.Empty;
            num = num > queryString.Length ? queryString.Length : num;

            return queryString.Substring(0, num) + defaultstr;
        }


        /// <summary>
        /// md532加密码处理
        /// 规则：截前2位和后3位
        /// </summary>
        /// <param name="queryString">加密字符串</param>
        /// <returns></returns>
        public static string Md532SubString(this string queryString)
        {
            if (queryString == null)
                return string.Empty;

            int len = queryString.Length;

            if (len != 32)
                return queryString;

            queryString = queryString.Substring(2, 27);

            return queryString;
        }
        #endregion

        #region 字符串过滤

        /// <summary>
        /// 返回过滤不安全字符＜＞ htmlecode
        /// </summary>
        /// <param name="queryString">字符串</param>
        /// <returns></returns>
        public static string StrFilter(this string queryString)
        {
            if (queryString == null)
                return string.Empty;

            return queryString.Replace("<", "&lt;")
                              .Replace(">", "&gt;");
        }

        /// <summary>
        /// 返回过滤不安全字符＜＞转全角
        /// </summary>
        /// <param name="queryString">字符串</param>
        /// <returns></returns>
        public static string StrFilterFull(this string queryString)
        {
            if (queryString == null)
                return string.Empty;

            return queryString.Replace("<", "＜")
                              .Replace(">", "＞");
        }

        /// <summary>
        /// 返回过滤不安全字符去除＜＞包含里面的内容
        /// </summary>
        /// <param name="queryString">过滤的内容</param>
        /// <returns></returns>
        public static string StrFilter(this string queryString, bool replace)
        {
            if (queryString == null)
                return string.Empty;

            return Regex.Replace(queryString, "<.*?>", "");
        }

        /// <summary>
        /// lucene保留关键字过滤
        /// </summary>
        /// <param name="queryString">过滤关键字</param>
        /// <returns></returns>
        public static string LuceneStrStaticFilter(this string queryString)
        {
            if (queryString == null)
                return string.Empty;

            return queryString.Replace("+", "")
                              .Replace("-", "")
                              .Replace("&", "")
                              .Replace("|", "")
                              .Replace("!", "")
                              .Replace("(", "")
                              .Replace(")", "")
                              .Replace("{", "")
                              .Replace("}", "")
                              .Replace("[", "")
                              .Replace("]", "")
                              .Replace("^", "")
                              .Replace("~", "")
                              .Replace("*", "")
                              .Replace("?", "")
                              .Replace("'", "")
                              .Replace(":", "")
                              .Replace("\\", "")
                              .Replace("\"", "")
                              .Replace("AND", "")
                              .Replace("OR", "")
                              .Replace("NOT", "");

        }

        /// <summary>
        /// SQL防注入
        /// </summary>
        /// <param name="InText">要过滤的字符串 </param>
        /// <returns>如果参数存在不安全字符，则返回true</returns>
        public static bool IsSqlTextSafe(this string queryString, bool old)
        {
            string word = @"and|exec|select|update|delete|drop|chr|mid|master|truncate|char|declare|join|cmd";//这里加要过滤的SQL字符 
            if (Regex.IsMatch(queryString.ToLower(), word.ToLower(), RegexOptions.IgnoreCase))
                return true;
            return false;
        }
        /// <summary>
        /// SQl防止注入
        /// </summary>
        /// <param name="queryString"></param>
        /// <returns></returns>
        public static bool IsSqlTextSafe(this string queryString)
        {
            //过滤关键字
            string StrKeyWord = @"exec|select|insert|delete|from|count\(|drop table|update|truncate|asc\(|mid\(|char\(|xp_cmdshell|exec master|netlocalgroup administrators|:|net user|""|or|and";
            //过滤关键字符
            string StrRegex = @"[;|%|\@|*|!|']";
            if (Regex.IsMatch(queryString.ToLower(), StrKeyWord.ToLower(), RegexOptions.IgnoreCase) ||
                Regex.IsMatch(queryString.ToLower(), StrRegex.ToLower()))
                return false;
            return true;
        }
        /// <summary>
        /// SQL防注入
        /// </summary>
        /// <param name="InText">要过滤的字符串 </param>
        /// <returns>如果参数存在不安全字符，则返回true</returns>
        public static string SqlTextFilter(this string queryString)
        {
            if (queryString == null)
                return string.Empty;

            return queryString//.Replace("and", "")
                              .Replace("exec", "")
                //.Replace("select", "")
                              .Replace("chr", "")
                              .Replace("mid", "")
                              .Replace("master", "")
                              .Replace("or", "")
                              .Replace("truncate", "")
                              .Replace("char", "")
                              .Replace("declare", "")
                              .Replace("join", "")
                              .Replace("cmd", "")
                              .Replace(";", "")
                              .Replace("'", "")
                              .Replace("update", "")
                              .Replace("insert", "")
                              .Replace("drop", "")
                              .Replace("delete", "");
        }

        #endregion

        #region 字符串转换
        /// <summary>
        ///返回 List＜int＞数组转换成string字符串 中间 ',' 号隔开
        /// </summary>
        /// <param name="list">数组</param>
        /// <returns></returns>
        public static string ListToString(this List<int> list)
        {
            if (list == null || list.Count == 0)
                return string.Empty;

            StringBuilder retStr = new StringBuilder();
            foreach (int item in list)
            {
                retStr.AppendFormat(",{0}", item.ToString());
            }

            return retStr.Remove(0, 1).ToString();
        }
        /// <summary>
        ///返回 List＜int＞数组转换成string字符串 中间 ',' 号隔开
        /// </summary>
        /// <param name="list">数组</param>
        /// <returns></returns>
        public static string ListToString(this List<string> list)
        {
            if (list == null || list.Count == 0)
                return string.Empty;

            StringBuilder retStr = new StringBuilder();
            foreach (string item in list)
            {
                retStr.AppendFormat(",'{0}'", item.ToString());
            }

            return retStr.Remove(0, 1).ToString();
        }
        
        /// <summary>
        ///返回 List＜int＞数组转换成string字符串 中间 ',' 号隔开
        /// </summary>
        /// <param name="list">数组</param>
        /// <returns></returns>
        public static string ListToStrings(this List<string> list)
        {
            if (list == null || list.Count == 0)
                return string.Empty;

            StringBuilder retStr = new StringBuilder();
            foreach (string item in list)
            {
                retStr.AppendFormat(",{0}", item.ToString());
            }

            return retStr.Remove(0, 1).ToString();
        }


        /// <summary>
        ///返回 List＜int＞数组转换成string字符串  按照指定分隔符转化
        /// </summary>
        /// <param name="list">数组</param>
        /// <param name="split">分隔符</param>
        /// <returns></returns>
        public static string ListToString(this List<int> list, byte split)
        {
            if (list == null || split == null || list.Count == 0)
                return string.Empty;

            StringBuilder retStr = new StringBuilder();
            foreach (int item in list)
            {
                retStr.AppendFormat("{0}{1}", split, item.ToString());
            }

            return retStr.Remove(0, 1).ToString();
        }


        /// <summary>
        /// 时间撮转换成 datetime时间
        /// </summary>
        /// <param name="timeStamp">时间戳</param>
        /// <returns></returns>
        public static DateTime TimeStampConvertDateTime(this string timeStamp)
        {
            DateTime dateTimeStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long lTime = long.Parse(timeStamp + "0000000");
            TimeSpan toNow = new TimeSpan(lTime);
            return dateTimeStart.Add(toNow);
        }

        /// <summary>
        /// 时间转换成时间撮
        /// </summary>
        /// <param name="time">时间</param>
        /// <returns></returns>
        public static string DateTimeConvertTimeStamp(this DateTime time)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            return (time - startTime).TotalSeconds.ToString().Substring(0, 10);
        }
        #endregion

        #region 字符串加(解)密

        /// <summary>
        /// SHA1加密
        /// </summary>
        public static string SHA1Encrypt(this string encryptString)
        {
            byte[] StrRes = Encoding.Default.GetBytes(encryptString);
            HashAlgorithm iSHA = new SHA1CryptoServiceProvider();
            StrRes = iSHA.ComputeHash(StrRes);
            StringBuilder EnText = new StringBuilder();
            foreach (byte iByte in StrRes)
            {
                EnText.AppendFormat("{0:x2}", iByte);
            }
            return EnText.ToString();
        }

        /// <summary>
        /// 返回MD5加密后的字符串
        /// </summary>
        /// <param name="str">原始字符串</param>
        /// <returns>MD5加密字符串</returns>
        public static string StringToMd5(this string encryptString)
        {
            byte[] b = Encoding.Default.GetBytes(encryptString);
            b = new MD5CryptoServiceProvider().ComputeHash(b);

            string ret = "";
            for (int i = 0; i < b.Length; i++)
                ret += b[i].ToString("x").PadLeft(2, '0');

            return ret;
        }

        /// <summary>
        /// DES加密字符串
        /// </summary>
        /// <param name="encryptString">待加密的字符串</param>
        /// <param name="encryptKey">加密密钥,要求为8位</param>
        /// <returns>加密成功返回加密后的字符串，失败返回源串</returns>
        public static string DESEncrypt(this string encryptString, string encryptKey)
        {
            try
            {
                byte[] rgbKey = Encoding.UTF8.GetBytes(encryptKey.Substring(0, 8));
                byte[] rgbIV = Keys;
                byte[] inputByteArray = Encoding.UTF8.GetBytes(encryptString);
                DESCryptoServiceProvider dCSP = new DESCryptoServiceProvider();
                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, dCSP.CreateEncryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();
                return Convert.ToBase64String(mStream.ToArray());
            }
            catch
            {
                return string.Empty;
            }
        }
        /// <summary>
        /// DES加密字符串
        /// </summary>
        /// <param name="encryptString">待加密的字符串</param>        
        /// <returns>加密成功返回加密后的字符串，失败返回源串</returns>
        public static string DESEncrypt(this string encryptString)
        {
            try
            {
                string encryptKey = ConfigHelper.GetAppSettingsString("PublicKey");
                byte[] rgbKey = Encoding.UTF8.GetBytes(encryptKey.Substring(0, 8));
                byte[] rgbIV = Keys;
                byte[] inputByteArray = Encoding.UTF8.GetBytes(encryptString);
                DESCryptoServiceProvider dCSP = new DESCryptoServiceProvider();
                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, dCSP.CreateEncryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();
                return Convert.ToBase64String(mStream.ToArray());
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// DES解密字符串
        /// </summary>
        /// <param name="decryptString">待解密的字符串</param>
        /// <param name="decryptKey">解密密钥,要求为8位,和加密密钥相同
        /// <returns>解密成功返回解密后的字符串，失败返源串</returns>
        public static string DESDecrypt(this string decryptString, string decryptKey)
        {
            try
            {
                byte[] rgbKey = Encoding.UTF8.GetBytes(decryptKey.Substring(0, 8));
                byte[] rgbIV = Keys;
                byte[] inputByteArray = Convert.FromBase64String(decryptString);
                DESCryptoServiceProvider DCSP = new DESCryptoServiceProvider();
                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, DCSP.CreateDecryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();
                return Encoding.UTF8.GetString(mStream.ToArray());
            }
            catch
            {
                return string.Empty;
            }
        }
        /// <summary>
        /// DES解密字符串
        /// </summary>
        /// <param name="decryptString">待解密的字符串</param>        
        /// <returns>解密成功返回解密后的字符串，失败返源串</returns>
        public static string DESDecrypt(this string decryptString)
        {
            try
            {
                string decryptKey = ConfigHelper.GetAppSettingsString("PublicKey");

                byte[] rgbKey = Encoding.UTF8.GetBytes(decryptKey.Substring(0, 8));
                byte[] rgbIV = Keys;
                byte[] inputByteArray = Convert.FromBase64String(decryptString);
                DESCryptoServiceProvider DCSP = new DESCryptoServiceProvider();
                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, DCSP.CreateDecryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();
                return Encoding.UTF8.GetString(mStream.ToArray());
            }
            catch
            {
                return string.Empty;
            }
        }


        //默认密钥向量
        //private static byte[] Keys = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
        private static byte[] Keys = { 0x13, 0x35, 0x57, 0x79, 0x91, 0xAC, 0xDC, 0xFE };

        #endregion

        #region 字符串格式化
        public static string StrToString(this object str)
        {
            return str == null ? string.Empty : str.ToString();
        }
        /// <summary>
        /// datetime转换
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static string TimeToString(this DateTime time)
        {
            return time == null ? string.Empty : time.ToString("yyyy-MM-dd HH:ss:mm");
        }
        #endregion

        /// <summary>
        /// 获取IP
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static string GetClientIp(this HttpRequestMessage request)
        {
            if (request.Properties.ContainsKey("MS_HttpContext"))
            {
                return ((HttpContextWrapper)request.Properties["MS_HttpContext"]).Request.UserHostAddress;
            }

            if (request.Properties.ContainsKey(RemoteEndpointMessageProperty.Name))
            {
                RemoteEndpointMessageProperty prop;
                prop = (RemoteEndpointMessageProperty)request.Properties[RemoteEndpointMessageProperty.Name];
                return prop.Address;
            }

            return null;
        }

        #endregion
    }
}
