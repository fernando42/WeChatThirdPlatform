using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using WechatAPI.Public.Wechat;
using WeChatInterface.Public.Common;

namespace WeChatInterface.Public
{
    /// <summary>
    /// WeChatProgramService 的摘要说明
    /// </summary>
    public class WeChatProgramService : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var code = context.Request.QueryString["code"];
            var result = getcode(code);
            context.Response.Write(result);
        }

        public string getcode(string code)
        {
            string openid = null;
            string unionid = null;
            string json = null;
            string url = string.Format(ApiInfo.ADDR_GET_APPCODE, WebConfigurationManager.AppSettings["miniappid"], WebConfigurationManager.AppSettings["miniappsecret"], code);
            var result = HttpClientHelper.GetResponse(url);
            JObject outputObj = JObject.Parse(result);
            try
            {
                openid = outputObj["openid"].ToString();
                unionid = outputObj["unionid"].ToString();
                json = "{\"openid\":\"" + openid + "\",\"unionid\":\""+unionid+"\"}";
            }
            catch
            {
                result = outputObj["errmsg"].ToString();
                return result;
            }

            return json;
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