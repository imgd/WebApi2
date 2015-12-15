# webapi2.demo
这是一个webapi demo项目，使用它可以实现分布式项目后端数据提供支持。<br/>
这是一个初始版本，后续会继续更新和完善，有问题 微博：[@大白2013](http://weibo.com/u/2239977692)  <br/>
如果对您有帮助，请不要忘记点个赞哦 <br/>

目前实现的功能有：<br/>
> 1. 支持api版本控制<br/>
> 1. 支持客户端请求身份验证<br/>
> 1. 支持基于命名空间路由<br/>
> 1. 支持消息压缩<br/>
> 1. 支持返回消息默认Json格式,修改DateTime类型序列化格式<br/>
> 1. 支持全局异常捕获和日志记录<br/>
> 1. 支持自定义的参数验证<br/>
> 1. 支持服务器端请求资源缓存<br/>
> 1. 支持Ajax Jsonp 跨域请求<br/>


<br/>


###api版本控制规范
服务端Controller命名<br/>
 - *VersionController （默认版本）*
 - *VersionV1_1_0_2Controller （v1.1.0.2版本）*
<br/><br/>客户端请求需要在头信息添加自定义头：X-SourceVersion

>```c
client.DefaultRequestHeaders.Add("X-SourceVersion", "1.1.0.2");
```

###客户端身份验证规范
客户端请求头添加自定义头：X-SourceToken<br/>

>```c
client.DefaultRequestHeaders.Add("X-SourceToken","eo6jrDsvm68qyS5znLDh1RCoF/sMZhn2k5OjxEz54AaApjfI=");
```


<br/>



###自定义参数验证 

示例<br />
>
```java
[Verify(true)]   必填
```
>
```java
[Verify(VerifyType.mobile)]   验证手机 必填
``` 
>
```java
[Verify(VerifyType.email,isRequired =false)]    验证手机 选填
```
>
```java
[Verify(type = VerifyType.regexpress,pattern = @"^\d[1-9]{1,2}$",errorMessage = "age必须是0 - 99的数字")]   自定义正则验证
```
<br/>

###Ajax Jsonp跨域请求 
>服务端配置：<br/>

>
```java
[JsonpFormatter]
public DateTime GetTime()
{
    return DateTime.Now;
}
```

>客户端配置：<br/>

>
```
function JsonpAjax(t, u, d, callback, e) {
            $.ajax({
                Type: t ? "GET" : t,
                url: u,
                dataType: "jsonp",
                jsonp: "callback",
                data: d,
                success: callback,
                error: e
            });
        }
```
<br/>


###其他说明
- 基于全局的功能配置，修改配置文件来部署
- 配置特性路由和自定义模版路由（这里访问可能有冲突）
- 自定义路由参数目前不会进行全局SQL参数安全验证（2015.12.15已添加）



