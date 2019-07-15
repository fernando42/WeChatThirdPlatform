using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WechatAPI.Public.Wechat.Http;

namespace WechatAPI.Public.Wechat.Api
{
    public class GetUserDetailRequest : WxPostWithTokenRequest<GetUserDetailReponse>
    {
        public string Openid { get; set; }
        public GetUserDetailRequest(string openid)
        {
            Openid = openid;
        }
        public override string GetUrl()
        {
            return Format(ApiInfo.ADDR_USER_GETUSERDETAIL, Token, Openid);
        }

        public override string GetParamters()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this);
        }
    }

    public class GetUserDetailReponse : WxBaseReponse
    {
        [Newtonsoft.Json.JsonProperty("subscribe")]
        public string Subscribe { get; set; }
        [Newtonsoft.Json.JsonProperty("nickname")]
        public string NickName { get; set; }
        [Newtonsoft.Json.JsonProperty("sex")]
        public string Sex { get; set; }
        [Newtonsoft.Json.JsonProperty("language")]
        public string Language { get; set; }
        [Newtonsoft.Json.JsonProperty("city")]
        public string City { get; set; }
        [Newtonsoft.Json.JsonProperty("province")]
        public string Province { get; set; }
        [Newtonsoft.Json.JsonProperty("country")]
        public string Country { get; set; }
        [Newtonsoft.Json.JsonProperty("headimgurl")]
        public string HeadImgURL { get; set; }
        [Newtonsoft.Json.JsonProperty("subscribe_time")]
        public string Subscribe_time { get; set; }
        [Newtonsoft.Json.JsonProperty("remark")]
        public string Remark { get; set; }
        [Newtonsoft.Json.JsonProperty("groupid")]
        public string Groupid { get; set; }
        [Newtonsoft.Json.JsonProperty("unionid")]
        public string Unionid { get; set; }

        [Newtonsoft.Json.JsonIgnore()]
        public override bool IsSuccess
        {
            get { return !string.IsNullOrEmpty(ReponseData); }
        }
    }
}
