using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Xml.Linq;
using Tencent;
using WeChatInterface.Public.Common;

namespace WeChatInterface.Public
{
    /// <summary>
    /// interfaceTest 的摘要说明
    /// </summary>
    public class interfaceTest : IHttpHandler
    {

        public void ProcessRequest(HttpContext param_context)
        {
            string postString = string.Empty;
            if (HttpContext.Current.Request.HttpMethod.ToUpper() == "POST")
            {
                ResponseRequest(param_context);
                //using (Stream stream = HttpContext.Current.Request.InputStream)
                //{
                //    Byte[] postBytes = new Byte[stream.Length];
                //    stream.Read(postBytes, 0, (Int32)stream.Length);
                //    postString = Encoding.UTF8.GetString(postBytes);
                //    Handle(postString);
                //}
            }
            if (HttpContext.Current.Request.HttpMethod.ToUpper() == "GET")
            {
                //InterfaceTest();
                ResponseRequest(param_context);
            }
        }

        /// <summary>
        /// 处理信息并应答
        /// </summary>
        private void Handle(string postStr)
        {
            messageHelp help = new messageHelp();
            string responseContent = help.ReturnMessage(postStr);

            HttpContext.Current.Response.ContentEncoding = Encoding.UTF8;
            HttpContext.Current.Response.Write(responseContent);
        }

        //成为开发者url测试，返回echoStr
        public void InterfaceTest()
        {
            string token = "Token";
            if (string.IsNullOrEmpty(token))
            {
                return;
            }

            string echoString = HttpContext.Current.Request.QueryString["echoStr"];
            string signature = HttpContext.Current.Request.QueryString["signature"];
            string timestamp = HttpContext.Current.Request.QueryString["timestamp"];
            string nonce = HttpContext.Current.Request.QueryString["nonce"];

            if (!string.IsNullOrEmpty(echoString))
            {
                HttpContext.Current.Response.Write(echoString);
                HttpContext.Current.Response.End();
            }
        }

        private void ResponseRequest(HttpContext context)
        {
            string sToken = "Token";
            string sAppID = WebConfigurationManager.AppSettings["MASTERAPPID"];
            string sEncodingAESKey = "ybusoyOcAwO9VlO16VY8eeE5pZWsfcrTr8vtOJeqRvj";

            //WXBizMsgCrypt 这个类是腾讯提供的，下载地址是http://mp.weixin.qq.com/wiki/static/assets/a5a22f38cb60228cb32ab61d9e4c414b.zip
            //这里的构造函数我自己改写了，腾讯提供的构造函数需要提供三个参数的，具体请看微信提供的示例代码
            WXBizMsgCrypt wxcpt = new WXBizMsgCrypt(sToken, sEncodingAESKey, sAppID);

            string sReqMsgSig = HttpContext.Current.Request.QueryString["msg_signature"];
            string sReqTimeStamp = HttpContext.Current.Request.QueryString["timestamp"];
            string sReqNonce = HttpContext.Current.Request.QueryString["nonce"];
            string sReqData = GetPost();

            //sReqMsgSig = "8e62a1c86769320cf82dc9f01e947b904db4c6ac";
            //sReqTimeStamp = "1562813744";
            //sReqNonce = "129684983";
            //sReqData = "<xml>< AppId >< ![CDATA[wx77e59ec1c4a582e7]] ></ AppId >< Encrypt >< ![CDATA[CBI8yD8tbfgO8aFytLuyU / 1x + SK87QNwPFVGnebG + ZuZLynxwHlGbl49JxpiYkg5cK883EPfQkpEYChOhSLMP1DMI4T6F5NB9mmNtHtsyjeL93unOSBg26YlfHKjJI9juiB4WCQlcpYZdLNoAyaXYY + oczriNbdPKcYqBCmngzdWNOf2modI + MrGoNZqaLsJuP / A3GeoyHMEiBeJ4rB / 7bxEM + idXzjjGXn5ss4LAlCuzSR / SFBveu0jsKwiWHW / urFM8Afa6NMP6lG4h1M + B9DA3L9f7hD + BpAvL5s61unTiO27GqQONM8zfgH85YJqU44Dl1kIs4i8pdQOK7TK0g ==]] ></ Encrypt ></ xml > ";

            string sMsg = "";  //解析之后的明文
            int ret = 0;
            ret = wxcpt.DecryptMsg(sReqMsgSig, sReqTimeStamp, sReqNonce, sReqData, ref sMsg);
            if (ret != 0)
            {
            }
            else
            {

                var xDoc = XDocument.Parse(sMsg);

                List<XElement> q = (from c in xDoc.Elements() select c).ToList();

                var infoType = q.Elements("InfoType").First().Value;

                switch (infoType)
                {
                    case "component_verify_ticket":
                        var ticket = q.Elements("ComponentVerifyTicket").First().Value;
                        HttpContext.Current.Response.Write("success");
                        ticket = ticket.Replace("ticket@@@", "");
                        //这里就是component_verify_ticket的值，保存起来就可以了,处理完成后在页面上输出success,通知微信服务器已经接收到ticket
                        string data = "{\"component_appid\":\""+WebConfigurationManager.AppSettings["MASTERAPPID"]+"\",\"component_appsecret\":\""+WebConfigurationManager.AppSettings["MASTERSECRET"]+ "\",\"component_verify_ticket\":\"" + ticket+ "\"}";
                        var result = HttpClientHelper.PostResponse("https://api.weixin.qq.com/cgi-bin/component/api_component_token", data);
                        JObject outputObj = JObject.Parse(result);
                        var accesstoken = outputObj["component_access_token"].ToString();
                        SetCache("token", accesstoken, DateTime.Now.AddHours(2), System.Web.Caching.Cache.NoSlidingExpiration);
                        //data = "{\"component_appid\":\"" + WebConfigurationManager.AppSettings["MASTERAPPID"] + "\"}";
                        //获取预授权码
                        //result = HttpClientHelper.PostResponse("https://api.weixin.qq.com/cgi-bin/component/api_create_preauthcode?component_access_token=" + accesstoken,data);
                        //JObject authcode = JObject.Parse(result);
                        //var code = authcode["pre_auth_code"].ToString();
                        //code = code.Replace("preauthcode@@@", "");
                        //data = "{\"component_appid\":\"" + WebConfigurationManager.AppSettings["MASTERAPPID"] + "\",\"authorization_code\":\"" + code + "\"}";
                        //获取接口调用凭据和授权信息
                        //result = HttpClientHelper.PostResponse("https://api.weixin.qq.com/cgi-bin/component/api_query_auth?component_access_token=" + accesstoken, data);
                        HttpContext.Current.Response.End();
                        break;
                    case "unauthorized":
                        //当用户取消授权的时候，微信服务器也会向这个页面发送信息，在这里做一下记录
                        HttpContext.Current.Response.End();
                        break;
                    default:
                        break;
                }
            }
        }

        public string GetPost()
        {
            try
            {
                System.IO.Stream s = HttpContext.Current.Request.InputStream;
                int count = 0;
                byte[] buffer = new byte[s.Length];
                StringBuilder builder = new StringBuilder();
                while ((count = s.Read(buffer, 0, buffer.Length)) > 0)
                {
                    builder.Append(HttpContext.Current.Request.ContentEncoding.GetString(buffer, 0, count));
                }
                s.Flush();
                s.Close();
                s.Dispose();
                return builder.ToString();
            }
            catch (Exception ex)
            { throw ex; }
        }

        /// <summary>
        /// 设置数据缓存
        /// </summary>
        public static void SetCache(string CacheKey, object objObject, DateTime absoluteExpiration, TimeSpan slidingExpiration)
        {
            System.Web.Caching.Cache objCache = HttpRuntime.Cache;
            objCache.Insert(CacheKey, objObject, null, absoluteExpiration, slidingExpiration);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}