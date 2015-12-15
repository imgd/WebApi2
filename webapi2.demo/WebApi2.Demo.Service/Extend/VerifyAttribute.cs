using System;
using System.ComponentModel.DataAnnotations;
using WebApi2.Demo.Common;

namespace WebApi2.Demo.Service
{
    /// <summary>
    /// 自定义实体属性格式验证类
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class VerifyAttribute : ValidationAttribute
    {
        #region 属性

        public string errorMessage { get; set; }
        /// <summary>
        /// 验证类型
        /// </summary>
        public VerifyType type { get; set; }

        /// <summary>
        /// 正则验证格式
        /// </summary>
        public String pattern { get; set; }
        /// <summary>
        /// 字符验证最大长度
        /// </summary>
        public int maxLength { get; set; }
        /// <summary>
        /// 字符验证最小长度
        /// </summary>
        public int minLength { get; set; }

        /// <summary>
        /// 范围验证最小
        /// </summary>
        public long minRange { get; set; }
        /// <summary>
        /// 范围验证最大
        /// </summary>
        public long maxRange { get; set; }

        /// <summary>
        /// 是否必填
        /// </summary>
        public bool isRequired { get; set; }

        #endregion

        #region 构造
        public VerifyAttribute()
        {
            this.isRequired = false;
            this.type = VerifyType.other;
        }
        public VerifyAttribute(VerifyType type)
        {
            this.isRequired = true;
            this.type = type;
        }
        public VerifyAttribute(VerifyType type, bool isRequired)
        {
            this.isRequired = isRequired;
            this.type = type;
        }
        public VerifyAttribute(bool isrequired)
        {
            this.isRequired = isrequired;
            this.type = VerifyType.other;
        }
        public VerifyAttribute(VerifyType type, int min, int max)
        {
            this.type = type;
            this.minLength = min;
            this.maxLength = max;
            this.isRequired = true;
        }
        public VerifyAttribute(VerifyType type, int min, int max, bool isRequired)
        {
            this.type = type;
            this.minLength = min;
            this.maxLength = max;
            this.isRequired = isRequired;
        }

        public VerifyAttribute(string pattern, string errorMeaage)
        {
            this.type = VerifyType.regexpress;
            this.type = type;
            this.pattern = pattern;
            this.isRequired = true;
        }
        public VerifyAttribute(string pattern, string errorMeaage, bool isRequired)
        {
            this.type = VerifyType.regexpress;
            this.type = type;
            this.pattern = pattern;
            this.isRequired = isRequired;
        }
        #endregion



        /// <summary>
        /// 验证值是否符合规范
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public override bool IsValid(object value)
        {
            //是否验证通过标识
            bool isPass = true;

            ///选填 值为空
            if (!isRequired && value.IsNullOrEnpty())
            {
                return true;
            }
            ///必填 值为空
            else if (isRequired && value.IsNullOrEnpty())
            {
                base.ErrorMessage = "参数格式：必填项";
                return false;
            }
            ///选/必填 值不为空
            else
            {
                switch (type)
                {
                    #region verify
                    case VerifyType.mobile:
                        base.ErrorMessage = "参数格式：手机";
                        isPass = value.ToString().MobileVerify();
                        break;
                    case VerifyType.email:
                        base.ErrorMessage = "参数格式：邮箱";
                        isPass = value.ToString().EmailVerify();
                        break;
                    case VerifyType.invicode:
                        base.ErrorMessage = "参数格式：6位数字验证码";
                        isPass = value.ToString().InviCodeVerify();
                        break;
                    case VerifyType.password:
                        base.ErrorMessage = "参数格式：6-20位下划线数字字符格式密码";
                        isPass = value.ToString().PassWordVerify();
                        break;
                    case VerifyType.guid:
                        base.ErrorMessage = "参数格式：36位下划线数字字符guid";
                        isPass = value.ToString().GuidVerfy();
                        break;
                    case VerifyType.zipcode:
                        base.ErrorMessage = "参数格式：6位数字zip";
                        isPass = value.ToString().ZipCodeVerify();
                        break;
                    case VerifyType.couponscode:
                        base.ErrorMessage = "参数格式：18位数字优惠卷码";
                        isPass = value.ToString().CouponsCodeVerify();
                        break;
                    case VerifyType.ip:
                        base.ErrorMessage = "参数格式：IP";
                        isPass = value.ToString().IpVerify();
                        break;
                    case VerifyType.ordernumber:
                        base.ErrorMessage = "参数格式：12数字订单编号";
                        isPass = value.ToString().OrderNumberVerify();
                        break;
                    case VerifyType.length:
                        base.ErrorMessage = string.Format("参数格式：字符长度 {0}-{1}", minLength, maxLength);
                        isPass = value.ToString().StrLengthVerify(minLength, maxLength);
                        break;
                    case VerifyType.intRange:
                        base.ErrorMessage = string.Format("参数格式：数字范围 {0}-{1}", minRange, maxRange);
                        isPass = value.ParseInt().IntRangeVerify((int)minRange, (int)maxRange);
                        break;
                    case VerifyType.regexpress:
                        base.ErrorMessage = string.Format("参数格式：{0}", errorMessage == null ? "自定义错误" : errorMessage);
                        isPass = value.ToString().RegexVerify(pattern);
                        break;
                        #endregion
                }
            }            
            return isPass;
        }


    }


    /// <summary>
    /// 验证类型
    /// </summary>
    public enum VerifyType
    {
        /// <summary>
        /// 其他
        /// </summary>
        other,
        /// <summary>
        /// 手机
        /// </summary>
        mobile,
        /// <summary>
        /// 邮箱
        /// </summary>
        email,
        /// <summary>
        /// 注册验证码
        /// </summary>
        invicode,
        /// <summary>
        /// 密码
        /// </summary>
        password,
        /// <summary>
        /// guid
        /// </summary>
        guid,
        /// <summary>
        /// 邮箱
        /// </summary>
        zipcode,
        /// <summary>
        /// 优惠卷
        /// </summary>
        couponscode,
        /// <summary>
        /// ip
        /// </summary>
        ip,
        /// <summary>
        /// 数字
        /// </summary>
        ordernumber,
        /// <summary>
        /// 长度
        /// </summary>
        length,
        /// <summary>
        /// int范围验证
        /// </summary>
        intRange,
        /// <summary>
        /// 自定义正则
        /// </summary>
        regexpress

    }
}