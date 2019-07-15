using WechatAPI.Public.Wechat.Http;

namespace WechatAPI.Public.Wechat.Api
{
    public class GetTokenRequest : WxGetRequest<GetTokenReponse>
    {
        public override string GetUrl()
        {
            return Format(ApiInfo.ADDR_GETTOKEN, WXCONFIG.WX_GLOBAL_APPID, WXCONFIG.WX_GLOBAL_SECRET);
        }
    }

    public class GetTokenReponse : WxBaseReponse
    {
        [Newtonsoft.Json.JsonProperty("access_token")]
        public string Access_Token { get; set; }
        [Newtonsoft.Json.JsonProperty("expires_in")]
        public int Expires_In { get; set; }

        [Newtonsoft.Json.JsonIgnore()]
        public override bool IsSuccess
        {
            get
            {
                if (string.IsNullOrEmpty(ErrCode) || ErrCode == "0")
                { return true; }
                else
                { return false; }
            }
        }

    }
}
