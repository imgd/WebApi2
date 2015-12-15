using System.Runtime.Serialization;


namespace WebApi2.Demo.Common
{
    /// <summary>
    /// 结果输出模版
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [DataContract]
    public class Result<T> where T : new()
    {
        /// <summary>
        /// 返回的状态码
        /// </summary>
        [DataMember(Name = "code")]
        public int ResultDataCode { get; set; }
        /// <summary>
        /// 返回状态码的描述信息
        /// </summary>
        [DataMember(Name = "description")]
        public string ResultDataCodeDescription { get; set; }
        /// <summary>
        /// 返回的结果集
        /// </summary>
        [DataMember(Name = "data")]
        public T ResultData { get; set; }

        public Result() { }
        public Result(T data)
        {
            SetData(200, "返回成功", data);
        }
        public Result(int code, string description)
        {
            SetData(code, description, new T());
        }
        public Result(int code, string description, T data)
        {
            SetData(code, description, data);
        }

        public void SetProperty(int code, string description)
        {
            SetData(code, description, new T());
        }
        public void SetProperty(int code, string description,T data)
        {
            SetData(code, description, data);
        }

        private void SetData(int code, string description, T data)
        {
            this.ResultDataCode = code;
            this.ResultDataCodeDescription = description;
            this.ResultData = data;
        }

    }
}