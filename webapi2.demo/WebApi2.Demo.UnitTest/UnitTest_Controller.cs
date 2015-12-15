using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WebApi2.Demo.UnitTest
{
    [TestClass]
    public class UnitTest_Controller
    {
        public HttpClient client;
        public UnitTest_Controller()
        {
            this.client = new HttpClient();
        }

        public async Task GetName()
        {
            var response = client.GetAsync("http://localhost:54726/test/getname");
            var msg = await response.Result.Content.ReadAsStringAsync();
            Console.WriteLine(string.Format("method:GetName返回值：{0}", msg));
            Assert.AreEqual(msg, "\"name\"");
        }

        public async Task GetContentsCompress()
        {
            var response = client.GetAsync("http://localhost:54726/test/getage");
            var responsemsg = await response.Result.Content.ReadAsStringAsync();


            Console.WriteLine(string.Format("压缩前字节：2401002,压缩后字节：{0}",
                response.Result.Content.Headers.ContentLength));
            Console.WriteLine(response.Result.Content.Headers.ContentEncoding);
            Console.WriteLine(response.Result.Content.Headers.ContentLength);
            //如果测试失败 则是IE浏览器的问题 其他浏览器正常 可以F12查看response头信息
            Assert.AreEqual(response.Result.Content.Headers.ContentLength, 7054);
            Assert.IsTrue(response.Result.Content.Headers.ContentEncoding.Contains("gzip"));
        }


        public async Task ClientIdentity()
        {
            client.DefaultRequestHeaders.Add("token", "5cF6JJK");
            Console.WriteLine(client.DefaultRequestHeaders.Contains("token"));
            var response = client.GetAsync("http://localhost:54726/test/getname/1");
            var responsemsg = await response.Result.Content.ReadAsStringAsync();
            Assert.IsTrue(response.Result.StatusCode != HttpStatusCode.Unauthorized);
            Assert.AreEqual(response.Result.StatusCode, HttpStatusCode.OK);
            Assert.AreEqual(responsemsg, "\"我是牛牛1\"");
        }

        [TestMethod]
        public async Task TestPost()
        {
            //var p = await client.PostAsync("http://127.0.0.1:81/test/test/getage",
            //      (HttpContent)new FormUrlEncodedContent(
            //          new Dictionary<string, string>() {
            //              { "age", "18" },
            //              { "name", "gd" },
            //              { "mobile", "13683461619" }, }
            //          ));

            var p = await client.PostAsync("http://127.0.0.1:81/test/test/People",
                  (HttpContent)new FormUrlEncodedContent(
                      new Dictionary<string, string>() {
                          { "age", "28" },
                          { "name", "exec" },
                          { "mobile", "13683461619" } }
                      ));
            Console.WriteLine(p.Content.ReadAsStringAsync().Result);


            //var paras = "{ \"age\" : \"28\",\"name\" : \"delete\",\"mobile\" : \"13683461619\"}";
            //byte[] postData = Encoding.UTF8.GetBytes(paras);
            //WebClient webClient = new WebClient();
            //webClient.Headers.Add("Content-Type", "application/Json");
            //byte[] responseData = webClient.UploadData("http://127.0.0.1:81/test/test/People", "POST", postData);
            //string srcString = Encoding.UTF8.GetString(responseData);
            //srcString = Encoding.UTF8.GetString(responseData);
            //Console.WriteLine(srcString);

        }

        /// <summary>
        /// 测试获取token
        /// </summary>
        /// <returns></returns>
        public async Task TestToken()
        {
            var response = client.GetAsync("http://127.0.0.1:81/inspector/createtoken?clientkey=xwXXtotJVI9CeXxSWXN4GTY9bpw23DeFTjg7O2vztTZmuj74hdwdtN4gNsFIm7");
            var responsemsg = await response.Result.Content.ReadAsStringAsync();

            Console.WriteLine(responsemsg);
        }

        /// <summary>
        /// 测试请求是否通过验证
        /// </summary>
        /// <returns></returns>


        public async Task TestGetName()
        {

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            var response = client.GetAsync("http://127.0.0.1:81/inspector/inspector/createtoken?clientkey=xwXXtotJVI9CeXxSWXN4GTY9bpw23DeFTjg7O2vztTZmuj74hdwdtN4gNsFIm7");
            var responsemsg = await response.Result.Content.ReadAsStringAsync();
            Console.WriteLine(responsemsg);
            //Z7rjtkeohh9Nyq6/TpygGIJUsy+KQEB3jKVgdV0VclPcbFk7E+y1pnJb+Zxeo6jrDsvm68qyS5znLDh1RCoF/sMZhn2k5OjxEz54AaApjfI=
            client.DefaultRequestHeaders.Add("X-SourceToken", responsemsg.Replace('"', ' ').Trim());
            var response2 = client.GetAsync("http://127.0.0.1:81/test/test/getname");
            var responsemsg2 = await response2.Result.Content.ReadAsStringAsync();
            Console.WriteLine("返回的值：" + responsemsg2);

            //Assert.AreEqual(responsemsg2,"\"name\"");
        }

        /// <summary>
        /// 测试获取token
        /// </summary>
        /// <returns></returns>       
        
        public async Task TestVersion()
        {
            //基于namespace版本控制
            client.DefaultRequestHeaders.Add("X-SourceVersion", "1.1.0.2");
            var response = client.GetAsync("http://127.0.0.1:81/web/version/get");
            var responsemsg = await response.Result.Content.ReadAsStringAsync();

            //返回标准v1.1.0.2版本
            Console.WriteLine(responsemsg);
        }

    }
}
