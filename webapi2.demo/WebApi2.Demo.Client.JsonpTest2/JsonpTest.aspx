<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="JsonpTest.aspx.cs" Inherits="WebApi2.Demo.Client.JsonpTest.JsonpTest" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>    
    <script src="http://apps.bdimg.com/libs/jquery/1.6.4/jquery.min.js"></script>
    <script>
        //client jsonp 请求测试
        $(function () {
            JsonpAjax("GET", "http://127.0.0.1:81/a/b/d", "", function (d) {
                $("#result").html("返回结果：" +
                    d.age + "=" +
                    d.name + "=" +
                    d.mobile + "=" +
                    d.birthday);
            }, function (error) {
                console.log(error);
            }
            )

            JsonpAjax("GET", "http://127.0.0.1:81/a/b/c", "", function (d) {
                $("#result").append("<br />返回时间：" + d);
            }, function (error) {
                console.log(error);
            }
            )
        })

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
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div id="result">
        </div>
    </form>
</body>
</html>
