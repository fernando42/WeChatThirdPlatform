using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
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
    /// MessageService 的摘要说明
    /// </summary>
    public class MessageService : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            //添加自定义token
            string sToken = "";
            //string sAppID = HttpContext.Current.Request.QueryString["appId"];
            //sAppID = sAppID.Replace("/","");
            string sAppID = WebConfigurationManager.AppSettings["MASTERAPPID"];
            //添加43位预设密钥
            string sEncodingAESKey = "";
            WXBizMsgCrypt wxcpt = new WXBizMsgCrypt(sToken, sEncodingAESKey, sAppID);
            //获取interfacetest页面的accesstoken
            var accesstoken = GetCache("token");

            string sReqData = GetPost();
            string respnseContent = "";
            string sResponse = "";  //加密之后的回复文本
            string sReqMsgSig = HttpContext.Current.Request.QueryString["msg_signature"];
            string sReqTimeStamp = HttpContext.Current.Request.QueryString["timestamp"];
            string sReqNonce = HttpContext.Current.Request.QueryString["nonce"];
            string openid = HttpContext.Current.Request.QueryString["openid"];

            string sMsg = "";  //解析之后的明文
            int ret = 0;
            ret = wxcpt.DecryptMsg(sReqMsgSig, sReqTimeStamp, sReqNonce, sReqData, ref sMsg);

            var xDoc = XDocument.Parse(sMsg);

            List<XElement> q = (from c in xDoc.Elements() select c).ToList();

            var model = new
            {
                ToUserName = q.Elements("ToUserName").First().Value,
                FromUserName = q.Elements("FromUserName").First().Value,
                CreateTime = q.Elements("CreateTime").First().Value,

                MsgType = q.Elements("MsgType").First().Value,
                Content = ("" + q.Elements("Content").First().Value).Trim(),
                MsgId = q.Elements("MsgId").First().Value
            };

            if (false == string.IsNullOrEmpty(model.Content))
            {
                var textTpl = "<xml>"
                  + "<ToUserName><![CDATA[{0}]]></ToUserName>"
                  + "<FromUserName><![CDATA[{1}]]></FromUserName>"
                  + "<CreateTime>{2}</CreateTime>"
                  + "<MsgType><![CDATA[{3}]]></MsgType>"
                  + "<Content><![CDATA[{4}]]></Content>"
                  + "</xml>";
                if(model.Content== "TESTCOMPONENT_MSG_TYPE_TEXT")
                {
                    //回复普通消息
                    respnseContent = "TESTCOMPONENT_MSG_TYPE_TEXT_callback";
                    int enRet = wxcpt.EncryptMsg(string.Format(textTpl, model.FromUserName, model.ToUserName, ConvertDateTimeInt(DateTime.Now), "text", respnseContent), sReqTimeStamp, sReqNonce, ref sResponse);
                    //sResponse = string.Format(textTpl, model.ToUserName, model.FromUserName, ConvertDateTimeInt(DateTime.Now), "text", respnseContent);
                    HttpContext.Current.Response.Clear();
                    HttpContext.Current.Response.Write(sResponse);
                    HttpContext.Current.Response.End();
                }
                else
                {
                    //回复API消息
                    HttpContext.Current.Response.Clear();
                    HttpContext.Current.Response.Write(string.Empty);//回复空字符串
                    var auth = model.Content;
                    auth = auth.Replace("QUERY_AUTH_CODE:queryauthcode@@@", "");
                    string data = "{\"component_appid\":\"" + WebConfigurationManager.AppSettings["MASTERAPPID"] + "\",\"authorization_code\":\"" + auth + "\"}";
                    var result = HttpClientHelper.PostResponse("https://api.weixin.qq.com/cgi-bin/component/api_query_auth?component_access_token=" + accesstoken, data);
                    JObject outputObj = JObject.Parse(result);
                    var token = outputObj["authorization_info"]["authorizer_access_token"].ToString();
                    respnseContent = model.Content.Replace("QUERY_AUTH_CODE:", "") + "_from_api";
                    var data2 = "{\"touser\":\"" + openid + "\",\"msgtype\":\"text\",\"text\":{\"content\":\"" + respnseContent + "\"}}";
                    var result2 = HttpClientHelper.PostResponse("https://api.weixin.qq.com/cgi-bin/message/custom/send?access_token=" + token, data2);
                    HttpContext.Current.Response.End();
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
        /// 获取数据缓存
        /// </summary>
        /// <param name="CacheKey">键</param>
        public static object GetCache(string CacheKey)
        {
            System.Web.Caching.Cache objCache = HttpRuntime.Cache;
            return objCache[CacheKey];
        }

        public static double ConvertDateTimeInt(System.DateTime time)
        {
            double intResult = 0;
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            intResult = (time - startTime).TotalSeconds;
            return intResult;
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