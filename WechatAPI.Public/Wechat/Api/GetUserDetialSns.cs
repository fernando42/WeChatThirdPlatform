using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WechatAPI.Public.Wechat.Http;

namespace WechatAPI.Public.Wechat.Api
{
    public class GetUserDetailSnsRequest : WxGetRequest<GetUserDetailSnsReponse>
    {
        public string Openid { get; set; }
        public string TokenSns { get; set; }
        public GetUserDetailSnsRequest(string tokenSns,string openid)
        {
            Openid = openid;
            TokenSns = tokenSns;
        }
        public override string GetUrl()
        {
            return Format(ApiInfo.ADDR_USER_GETUSERDETAILSNS, TokenSns, Openid);
        }

        public override string GetParamters()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this);
        }
    }

    public class GetUserDetailSnsReponse : WxBaseReponse
    {
        [Newtonsoft.Json.JsonProperty("nickname")]
        public string NickName { get; set; }
        [Newtonsoft.Json.JsonProperty("sex")]
        public string Sex { get; set; }
        [Newtonsoft.Json.JsonProperty("city")]
        public string City { get; set; }
        [Newtonsoft.Json.JsonProperty("province")]
        public string Province { get; set; }
        [Newtonsoft.Json.JsonProperty("country")]
        public string Country { get; set; }
        [Newtonsoft.Json.JsonProperty("headimgurl")]
        public string HeadImgURL { get; set; }
        [Newtonsoft.Json.JsonProperty("unionid")]
        public string Unionid { get; set; }

        [Newtonsoft.Json.JsonIgnore()]
        public override bool IsSuccess
        {
            get { return !string.IsNullOrEmpty(ReponseData); }
        }
    }
}
