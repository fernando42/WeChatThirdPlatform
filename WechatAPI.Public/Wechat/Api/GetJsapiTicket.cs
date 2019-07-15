using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WechatAPI.Public.Wechat.Http;

namespace WechatAPI.Public.Wechat.Api
{
    public class GetJsapiTicketRequest : WxGetWithTokenRequest<GetJsapiTicketResponse>
    {
        public override string GetUrl()
        {
            return Format(ApiInfo.ADDR_GET_JSAPI_TICKET, Token);
        }
    }

    public class GetJsapiTicketResponse : WxBaseReponse
    {
        [Newtonsoft.Json.JsonProperty("ticket")]
        public string Ticket { get; set; }
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

        public GetJsapiTicketResponse()
        {

        }
    }
}
