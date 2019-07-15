using System;
using System.Web.Configuration;
using System.Web.Security;
using WechatAPI.Public;
using WechatAPI.Public.Cache;
using WechatAPI.Public.Common;
using WechatAPI.Public.Wechat.AccessToken;
using WechatAPI.Public.Wechat.Api;
using WechatAPI.Public.Wechat.JSAPI_Ticket;
using WeChatInterface.Public.Common;

namespace WeChatInterface.Public
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码、svc 和配置文件中的类名“AppAuthService”。
    // 注意: 为了启动 WCF 测试客户端以测试此服务，请在解决方案资源管理器中选择 AppAuthService.svc 或 AppAuthService.svc.cs，然后开始调试。
    public class AppAuthService : IAppAuthService
    {
        public AppAccessTokenResult GetAppAccessToken(AppConfidential appConfidential)
        {
            if (CheckApplication.AppHasAccess(appConfidential.AppId, appConfidential.AppSecret))
            {
                WXCONFIG.WX_GLOBAL_APPID = appConfidential.AppId;
                WXCONFIG.WX_GLOBAL_SECRET = appConfidential.AppSecret;
                var res = AccessTokenManager.GetAccessToken();

                AppAccessTokenResult ret = new AppAccessTokenResult();
                ret.Access_token = res.Token;
                ret.StartTime = res.StartTime;
                ret.ExpireTime = res.ExpireTime;
                return ret;
            }
            else
            {
                return new AppAccessTokenResult() { Access_token = string.Empty };
            }
        }


        public AppAccessTokenResult RefreshAppAccessToken(AppConfidential appConfidential)
        {
            if (CheckApplication.AppHasAccess(appConfidential.AppId, appConfidential.AppSecret))
            {
                WXCONFIG.WX_GLOBAL_APPID = appConfidential.AppId;
                WXCONFIG.WX_GLOBAL_SECRET = appConfidential.AppSecret;
                CacheHelper.RemoveCache(AccessTokenManager.GetAccessTokenCacheKey());
                var res = AccessTokenManager.GetAccessToken();

                AppAccessTokenResult ret = new AppAccessTokenResult();
                ret.Access_token = res.Token;
                ret.StartTime = res.StartTime;
                ret.ExpireTime = res.ExpireTime;
                return ret;
            }
            else
            {
                return new AppAccessTokenResult() { Access_token = string.Empty };
            }
        }


        public string GetOauth2AuthorizeUrl(AppConfidential appConfidential, string redirect_uri)
        {
            if (CheckApplication.AppHasAccess(appConfidential.AppId, appConfidential.AppSecret))
            {
                WXCONFIG.WX_GLOBAL_APPID = appConfidential.AppId;
                WXCONFIG.WX_GLOBAL_SECRET = appConfidential.AppSecret;
                Oauth2AuthorizeUrl appUrl;
                if (string.IsNullOrEmpty(redirect_uri))
                {
                    appUrl = new Oauth2AuthorizeUrl("STATE", "snsapi_userinfo", redirect_uri);
                }
                else
                {
                    appUrl = new Oauth2AuthorizeUrl("STATE", "snsapi_userinfo", redirect_uri);
                }

                return appUrl.AuthorizeUrl;
            }
            else
            {

                return "没有权限";
            }
        }
        public AppAccessTokenSnsResult GetAppAccessTokenSns(AppConfidential appConfidential, string code)
        {
            if (CheckApplication.AppHasAccess(appConfidential.AppId, appConfidential.AppSecret))
            {
                WXCONFIG.WX_GLOBAL_APPID = appConfidential.AppId;
                WXCONFIG.WX_GLOBAL_SECRET = appConfidential.AppSecret;
                var req = new GetTokenSnsRequest(code);
                var res = req.Request();

                AppAccessTokenSnsResult ret = new AppAccessTokenSnsResult();
                ret.Access_token = res.Access_Token;
                ret.Openid = res.Openid;
                return ret;
            }
            else
            {
                return new AppAccessTokenSnsResult() { Access_token = string.Empty };
            }
        }


        public AppUserInfoSnsResult GetAppUserSnsInfo(AppConfidential appConfidential, string tokenSns, string openid)
        {
            if (CheckApplication.AppHasAccess(appConfidential.AppId, appConfidential.AppSecret))
            {
                WXCONFIG.WX_GLOBAL_APPID = appConfidential.AppId;
                WXCONFIG.WX_GLOBAL_SECRET = appConfidential.AppSecret;
                var req = new GetUserDetailSnsRequest(tokenSns, openid);
                var res = req.Request();
                AppUserInfoSnsResult ret = new AppUserInfoSnsResult();
                ret.NickName = res.NickName;
                ret.Sex = res.Sex;
                ret.City = res.City;
                ret.Province = res.Province;
                ret.Country = res.Country;
                ret.HeadImgURL = res.HeadImgURL;
                ret.Unionid = res.Unionid;
                return ret;
            }
            else
            {
                return new AppUserInfoSnsResult();
            }
        }


        public AppUserInfoResult GetAppUserInfo(AppConfidential appConfidential, string openid)
        {
            if (CheckApplication.AppHasAccess(appConfidential.AppId, appConfidential.AppSecret))
            {
                WXCONFIG.WX_GLOBAL_APPID = appConfidential.AppId;
                WXCONFIG.WX_GLOBAL_SECRET = appConfidential.AppSecret;
                var req = new GetUserDetailRequest(openid);
                var res = req.Request();
                //如果报错，重新获取Accesstoken
                if (res.ErrCode == "40001")
                {
                    AccessTokenManager.GetAccessToken(true);
                    req = new GetUserDetailRequest(openid);
                    res = req.Request();
                }
                AppUserInfoResult ret = new AppUserInfoResult();
                ret.Subscribe = res.Subscribe;
                ret.NickName = res.NickName;
                ret.Sex = res.Sex;
                ret.Language = res.Language;
                ret.City = res.City;
                ret.Province = res.Province;
                ret.Country = res.Country;
                ret.HeadImgURL = res.HeadImgURL;
                ret.Subscribe_time = res.Subscribe_time;
                ret.Remark = res.Remark;
                ret.Groupid = res.Groupid;
                ret.Unionid = res.Unionid;
                return ret;
            }
            else
            {
                return new AppUserInfoResult();
            }
        }


        public JSAPITicketResult GetJSAPITicket(AppConfidential appConfidential)
        {
            if (CheckApplication.AppHasAccess(appConfidential.AppId, appConfidential.AppSecret))
            {
                WXCONFIG.WX_GLOBAL_APPID = appConfidential.AppId;
                WXCONFIG.WX_GLOBAL_SECRET = appConfidential.AppSecret;
                var res = JsapiTicketManager.GetJsapiTicket();

                JSAPITicketResult ret = new JSAPITicketResult();
                ret.Ticket = res.Ticket;
                ret.ExpireTime = res.ExpireTime;
                ret.StartTime = res.StartTime;
                return ret;
            }
            else
            {
                return new JSAPITicketResult() { Ticket = string.Empty };
            }
        }

        public WXConfigResult GetWXConfig(AppConfidential appConfidential, string directUrl)
        {
            if (CheckApplication.AppHasAccess(appConfidential.AppId, appConfidential.AppSecret))
            {
                WXCONFIG.WX_GLOBAL_APPID = appConfidential.AppId;
                WXCONFIG.WX_GLOBAL_SECRET = appConfidential.AppSecret;
                var res = JsapiTicketManager.GetJsapiTicket();
                
                string jsapi_ticket = res.Ticket;
                string timestamp = CommonMethod.ConvertDateTimeInt(DateTime.Now).ToString();//生成签名的时间戳
                string nonceStr = CommonMethod.GetRandCode(16);//生成签名的随机串

                string[] ArrayList = { "jsapi_ticket=" + jsapi_ticket, "timestamp=" + timestamp, "noncestr=" + nonceStr, "url=" + directUrl };
                Array.Sort(ArrayList);
                string signature = string.Join("&", ArrayList);
                signature = FormsAuthentication.HashPasswordForStoringInConfigFile(signature, "SHA1").ToLower();

                WXConfigResult wxconfig = new WXConfigResult();
                wxconfig.AppId = appConfidential.AppId;
                wxconfig.NonceStr = nonceStr;
                wxconfig.Signature = signature;
                wxconfig.TimeStamp = int.Parse(timestamp);

                return wxconfig;

            }
            else
            {
                return new WXConfigResult();
            }
        }
    }
}
