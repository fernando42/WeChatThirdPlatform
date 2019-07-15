using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using WechatAPI.Public.Wechat;
using WeChatInterface.Public.Common;

namespace WeChatInterface.Public
{
    /// <summary>
    /// TemplateMessageService 的摘要说明
    /// </summary>
    public class TemplateMessageService : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string json = context.Request.Form.ToString();
            var token = getAccessToken();
            var result = SendTempMess(json, token);
            context.Response.Write(result);
        }

        public string SendTempMess(string json, string token)
        {
            string url = "https://api.weixin.qq.com/cgi-bin/message/wxopen/template/send?access_token={0}";
            url = string.Format(ApiInfo.ADDR_SEND_TEMPLATE_MESSAGE, token);
            string result = HttpClientHelper.PostResponse(url, json);
            JObject output = JObject.Parse(result);
            return result;
        }

        public string getAccessToken()
        {
            string appid = ConfigurationManager.AppSettings["miniappid"];
            string secretid = ConfigurationManager.AppSettings["miniappsecret"];
            var url = string.Format(ApiInfo.ADDR_GETTOKEN, appid, secretid);
            string ble_result = HttpClientHelper.GetResponse(url);
            JObject output = JObject.Parse(ble_result);
            string token = output["access_token"].ToString();
            return token;
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