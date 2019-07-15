using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;
using WechatAPI.Public.Wechat.Http;

namespace WechatAPI.Public.Wechat.Api
{

    public class GetAppCodeRequest : WxGetWithTokenRequest<GetAppCodeResponse>
    {
        public string code { get; set; }
        
        public override string GetUrl()
        {
            return Format(ApiInfo.ADDR_GET_APPCODE, WebConfigurationManager.AppSettings["miniappid"], WebConfigurationManager.AppSettings["miniappsecret"],code);
        }
    }

    public class GetAppCodeResponse : WxBaseReponse
    {
        public int errcode { get; set; }

        public string errmsg { get; set; }

        public string Openid { get; set; }

        public string Unionid { get; set; }

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
