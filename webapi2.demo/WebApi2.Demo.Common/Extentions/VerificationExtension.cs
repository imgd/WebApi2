using System;
using System.Text.RegularExpressions;

namespace WebApi2.Demo.Common
{
    //---------------------------------------------------------------
    //   参数验证扩展方法类  
    //   此扩展适用于公开服务的几口参数验证                                            
    // —————————————————————————————————————————————————             
    // | varsion 1.0                                   |             
    // | creat by gd 2014.7.31                         |             
    // | 联系我:@大白2013 http://weibo.com/u/2239977692 |            
    // —————————————————————————————————————————————————             
    //                                                               
    // *使用说明：                                                    
    //    使用当前扩展类添加引用: using Extensions.VerificationExtension;                      
    //    使用所有扩展类添加引用: using Extensions;                         
    // --------------------------------------------------------------

    public static class VerificationExtension
    {
        #region 参数验证

        #region 参数字符长度验证

        /// <summary>
        /// 验证字符空
        /// </summary>
        /// <param name="str">字符</param>
        /// <returns></returns>
        public static bool StrLengthNullVerify(this string str)
        {
            if (str.Trim().Length > 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// 字节最小长度验证
        /// </summary>
        /// <param name="str">字符</param>
        /// <param name="min">最小长度</param>
        /// <returns></returns>
        public static bool ByteNullVerify(this string str, int min)
        {
            str = str ?? string.Empty;
            byte[] bytes = System.Text.Encoding.Unicode.GetBytes(str.Trim());

            return bytes.Length >= min;
        }

        /// <summary>
        /// 验证字符最小长度
        /// </summary>
        /// <param name="str">字符</param>
        /// <param name="min">最小长度</param>
        /// <returns></returns>
        public static bool StrLengthMinVerify(this string str, int minLength)
        {
            if (str == null)
            {
                return false;
            }
            if (str.Trim().Length >= minLength)
                return true;
            else
                return false;
        }

        /// <summary>
        /// 验证字符最大长度
        /// </summary>
        /// <param name="str">字符</param>
        /// <param name="max">最大长度</param>
        /// <returns></returns>
        public static bool StrLengthMaxVerify(this string str, int maxLength)
        {
            if (str == null)
            {
                return false;
            }
            if (str.Trim().Length <= maxLength)
                return true;
            else
                return false;
        }

        /// <summary>
        /// 验证字符长度
        /// </summary>
        /// <param name="str">字符</param>
        /// <param name="length">字符长度</param>        
        /// <returns></returns>
        public static bool StrLengthVerify(this string str, int length)
        {
            if (str == null)
            {
                return false;
            }
            if (str.Trim().Length == length)
                return true;
            else
                return false;
        }

        /// <summary>
        /// 验证字符长度范围
        /// </summary>
        /// <param name="str">字符</param>
        /// <param name="length">字符长度</param>        
        /// <returns></returns>
        public static bool StrLengthVerify(this string str, int minLength, int maxLength)
        {
            if (str == null)
            {
                return false;
            }
            if (str.Trim().Length >= minLength && str.Trim().Length <= maxLength)
                return true;
            else
                return false;
        }
        /// <summary>
        /// 验证时间范围
        /// </summary>
        /// <param name="Date">当前时间</param>
        /// <returns></returns>
        public static bool DateTimeRangeVerify(this DateTime Date)
        {

            DateTime dt_min = new DateTime(1753, 1, 1);
            DateTime dt_max = new DateTime(9999, 1, 1);
            if (Date < dt_min || Date > dt_max)
                return false;
            else
                return true;
        }

        #endregion

        #region 参数类型验证

        /// <summary>
        /// 验证验证码
        /// </summary>
        /// <param name="invicodeStr">验证码</param>
        /// <returns></returns>
        public static bool InviCodeVerify(this string invicodeStr)
        {
            return RegexVerify(invicodeStr, @"^\d{6}$");
        }

        /// <summary>
        /// 验证密码
        /// </summary>
        /// <param name="pwdStr"></param>
        /// <returns></returns>
        public static bool PassWordVerify(this string pwdStr)
        {
            return RegexVerify(pwdStr, @"^(\w){6,20}$");
        }

        /// <summary>
        /// guid验证
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public static bool GuidVerfy(this string guid)
        {
            return RegexVerify(guid, @"^\w{36}$");
        }
        /// <summary>
        /// 微信用户id合法性
        /// </summary>
        /// <param name="wxOpenId"></param>
        /// <returns></returns>
        public static bool WXOpenIdVerify(this string wxOpenId)
        {
            return StrLengthVerify(wxOpenId, 28);
        }

        /// <summary>
        /// 用户邀请码
        /// </summary>
        /// <param name="wxOpenId"></param>
        /// <returns></returns>
        public static bool InvitationCodeVerify(this string InvitationCode)
        {
            return StrLengthVerify(InvitationCode, 5);
        }

        /// <summary>
        /// 验证邮箱
        /// </summary>
        /// <param name="mail"></param>
        /// <returns></returns>
        public static bool EmailVerify(this string email)
        {
            if (email == null) 
                return false;
            if (email.Trim().Length == 0 || email.Trim().Length > 100 )
                return false;

            return RegexVerify(email.Trim(), "^\\w+((-\\w+)|(\\.\\w+))*\\@[A-Za-z0-9]+((\\.|-)[A-Za-z0-9]+)*\\.[A-Za-z0-9]+$");

        }

        /// <summary>
        /// 验证手机号码
        /// </summary>
        /// <param name="mobile">手机号码</param>
        /// <returns></returns>
        public static bool MobileVerify(this string mobile)
        {
            return RegexVerify(mobile, @"^0?(13|15|18|14|17)[0-9]{9}$");
        }

        /// <summary>
        /// 验证邮编号码
        /// </summary>
        /// <param name="zipCode">邮编号码</param>
        /// <returns></returns>
        public static bool ZipCodeVerify(this string zipCode)
        {
            return RegexVerify(zipCode, @"^\d{6}$");
        }
        /// <summary>
        /// 验证邀请码
        /// </summary>
        /// <param name="zipCode">邀请码</param>
        /// <returns></returns>
        public static bool InviationCodeVerify(this string inviationCode)
        {
            return RegexVerify(inviationCode, @"^\w{5}$");
        }

        /// <summary>
        /// 验证订单编号
        /// </summary>
        /// <param name="orderNumber">订单编号</param>
        /// <returns></returns>
        public static bool OrderNumberVerify(this string orderNumber)
        {
            return RegexVerify(orderNumber, @"^\d{12}$");
        }

        /// <summary>
        /// 验证优惠券编号
        /// </summary>
        /// <param name="ccCode">优惠券编号</param>
        /// <returns></returns>
        public static bool CouponsCodeVerify(this string ccCode)
        {
            return RegexVerify(ccCode, @"^\d{18}$");
        }

        /// <summary>
        /// 验证MD5字符串
        /// </summary>
        /// <param name="MD5Str">MD5字符串</param>
        /// <returns></returns>
        public static bool MD5Verify(this string md5Str)
        {
            return RegexVerify(md5Str, @"^\w{32}$");
        }
        /// <summary>
        /// 验证MD5处理字符串
        /// 掐头2去尾3
        /// </summary>
        /// <param name="MD5Str">MD5字符串</param>
        /// <returns></returns>
        public static bool MD5Verify(this string md5Str, bool isCheck)
        {
            return RegexVerify(md5Str, @"^\w{27}$");
        }


        /// <summary>
        /// 验证正整数
        /// </summary>
        /// <param name="number">正整数</param>
        /// <returns></returns>
        public static bool NumberZVerify(this int number)
        {
            return RegexVerify(number.ToString(), @"^\d+$");
        }

        /// <summary>
        ///验证数字
        /// </summary>
        /// <param name="number">数字</param>
        /// <returns></returns>
        public static bool NumberVerify(this int number)
        {
            return RegexVerify(number.ToString(), @"^[0-9]*$");
        }


        /// <summary>
        /// 验证价格
        /// </summary>
        /// <param name="price">价格</param>
        /// <returns></returns>
        public static bool PriceVerify(this double price)
        {
            return RegexVerify(price.ToString(), @"([0-9]|[1-9][0-9]+)(\.[0-9]{0,2})?");
        }
        /// <summary>
        /// 验证价格
        /// </summary>
        /// <param name="price">价格</param>
        /// <returns></returns>
        public static bool PriceVerify(this float price)
        {
            return RegexVerify(price.ToString(), @"([0-9]|[1-9][0-9]+)(\.[0-9]{0,2})?");
        }

        /// <summary>
        /// 验证ip
        /// </summary>
        /// <param name="price">价格</param>
        /// <returns></returns>
        public static bool IpVerify(this string ip)
        {
            return RegexVerify(ip, @"((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)");
        }


        #endregion

        #region 取值范围验证


        /// <summary>
        /// 字节最大长度验证
        /// </summary>
        /// <param name="str">字符</param>
        /// <param name="max">最大长度</param>
        /// <returns></returns>
        public static bool ByteMaxVerify(this string str, int max)
        {
            str = str ?? string.Empty;
            byte[] bytes = System.Text.Encoding.Unicode.GetBytes(str.Trim());

            return bytes.Length <= max;
        }

        /// <summary>
        /// 验证 byte 数字的取值范围
        /// </summary>
        /// <param name="number">数字</param>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        /// <returns></returns>
        public static bool ByteRangeVerify(this byte number, byte min, byte max)
        {
            if (number >= min && number <= max)
                return true;
            else
                return false;
        }

        /// <summary>
        /// 验证 int 数字的取值范围
        /// </summary>
        /// <param name="number">数字</param>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        /// <returns></returns>
        public static bool IntRangeVerify(this int number, int min, int max)
        {
            if (number >= min && number <= max)
                return true;
            else
                return false;
        }

        /// <summary>
        /// 验证 float 数字的取值范围
        /// </summary>
        /// <param name="number">数字</param>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        /// <returns></returns>
        public static bool FloatRangeVerify(this float number, float min, float max)
        {
            if (number >= min && number <= max)
                return true;
            else
                return false;
        }

        /// <summary>
        /// 验证 double 数字的取值范围
        /// </summary>
        /// <param name="number">数字</param>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        /// <returns></returns>
        public static bool DoubleRangeVerify(this double number, double min, double max)
        {
            if (number >= min && number <= max)
                return true;
            else
                return false;
        }

        #endregion

        #region 根字符串正则验证

        /// <summary>
        /// 返回正则验证
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="reg">表达式</param>
        /// <returns></returns>
        public static bool RegexVerify(this string str, string reg)
        {
            if (str == null || reg == null || reg.Length == 0 || str.Length == 0)
                return false;

            return Regex.IsMatch(str.Trim(), reg);
        }

        #endregion

        #endregion
    }
}
