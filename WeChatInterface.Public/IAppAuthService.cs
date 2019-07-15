using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace WeChatInterface.Public
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码和配置文件中的接口名“IAppAuthService”。
    [ServiceContract]
    public interface IAppAuthService
    {
        //获取access token
        [OperationContract]
        AppAccessTokenResult GetAppAccessToken(AppConfidential appConfidential);

        //刷新access token
        [OperationContract]
        AppAccessTokenResult RefreshAppAccessToken(AppConfidential appConfidential);

        //获取oauth URL
        [OperationContract]
        string GetOauth2AuthorizeUrl(AppConfidential appConfidential, string directUrl);

        //获取access token
        [OperationContract]
        AppAccessTokenSnsResult GetAppAccessTokenSns(AppConfidential appConfidential, string code);

        //通过特殊token获取用户信息（openid)
        [OperationContract]
        AppUserInfoSnsResult GetAppUserSnsInfo(AppConfidential appConfidential, string tokenSns, string openid);

        //获取用户信息（openid)
        [OperationContract]
        AppUserInfoResult GetAppUserInfo(AppConfidential appConfidential, string openid);

        //获取jsapi_ticket
        [OperationContract]
        JSAPITicketResult GetJSAPITicket(AppConfidential appConfidential);

        //获取wx.config配置参数
        [OperationContract]
        WXConfigResult GetWXConfig(AppConfidential appConfidential, string directUrl);
    }


    [DataContract]
    public class AppConfidential
    {
        [DataMember]
        public string AppId { get; set; }

        [DataMember]
        public string AppSecret { get; set; }

    }

    [DataContract]
    public class AppAccessTokenResult
    {
        [DataMember]
        public string Access_token { get; set; }

        [DataMember]
        public DateTime StartTime { get; set; }

        [DataMember]
        public DateTime ExpireTime { get; set; }
    }

    [DataContract]
    public class AppAccessTokenSnsResult
    {
        [DataMember]
        public string Access_token { get; set; }

        [DataMember]
        public string Openid { get; set; }
    }

    [DataContract]
    public class AppUserInfoSnsResult
    {
        [DataMember]
        public string NickName { get; set; }

        [DataMember]
        public string Sex { get; set; }

        [DataMember]
        public string City { get; set; }

        [DataMember]
        public string Province { get; set; }

        [DataMember]
        public string Country { get; set; }

        [DataMember]
        public string HeadImgURL { get; set; }

        [DataMember]
        public string Unionid { get; set; }
    }

    [DataContract]
    public class AppUserInfoResult
    {
        [DataMember]
        public string Subscribe { get; set; }

        [DataMember]
        public string NickName { get; set; }

        [DataMember]
        public string Sex { get; set; }

        [DataMember]
        public string Language { get; set; }

        [DataMember]
        public string City { get; set; }

        [DataMember]
        public string Province { get; set; }

        [DataMember]
        public string Country { get; set; }

        [DataMember]
        public string HeadImgURL { get; set; }

        [DataMember]
        public string Subscribe_time { get; set; }

        [DataMember]
        public string Remark { get; set; }

        [DataMember]
        public string Groupid { get; set; }

        [DataMember]
        public string Unionid { get; set; }
    }

    [DataContract]
    public class JSAPITicketResult
    {
        [DataMember]
        public string Ticket { get; set; }

        [DataMember]
        public DateTime StartTime { get; set; }

        [DataMember]
        public DateTime ExpireTime { get; set; }
    }

    [DataContract]
    public class WXConfigResult
    {
        [DataMember]
        public string AppId { get; set; }

        [DataMember]
        public string NonceStr { get; set; }

        [DataMember]
        public int TimeStamp { get; set; }

        [DataMember]
        public string Signature { get; set; }
    }
}
