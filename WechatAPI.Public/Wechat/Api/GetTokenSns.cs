using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WechatAPI.Public.Wechat.Http;

namespace WechatAPI.Public.Wechat.Api
{
    public class GetTokenSnsRequest : WxGetRequest<GetTokenSnsReponse>
    {
        public string Code { get; set; }
        public GetTokenSnsRequest(string code)
        {
            Code = code;
        }
        public override string GetUrl()
        {
            return Format(ApiInfo.ADDR_GETTOKENSNS, WXCONFIG.WX_GLOBAL_APPID, WXCONFIG.WX_GLOBAL_SECRET, Code);
        }
    }


    public class GetTokenSnsReponse : WxBaseReponse
    {
        [Newtonsoft.Json.JsonProperty("access_token")]
        public string Access_Token { get; set; }
        [Newtonsoft.Json.JsonProperty("openid")]
        public string Openid { get; set; }

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
