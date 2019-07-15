using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WechatAPI.Public.Wechat.Api
{
    public class Oauth2AuthorizeUrl
    {
        public string appid { get { return WXCONFIG.WX_GLOBAL_APPID; } }
        public string redirect_uri { get; set; }
        public string response_type { get { return "code"; } }
        public string scope { get; set; }
        public string state { get; set; }
        public string wechat_redirect { get { return "#wechat_redirect"; } }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="state">重定向后会带上state参数，企业可以填写a-zA-Z0-9的参数值，长度不可超过128个字节</param>
        /// <param name="redirect_uri">授权后重定向的回调链接地址，请使用urlencode对链接进行处理 </param>
        public Oauth2AuthorizeUrl(string state, string scope, string redirect_uri)
        {
            this.state = state;
            this.redirect_uri = redirect_uri;
            this.scope = scope;
        }

        public string AuthorizeUrl
        {
            get
            {
                return string.Format(ApiInfo.ADDR_OAUTH2_AUTHORIZE,
                    appid, redirect_uri, response_type, scope, state, wechat_redirect);
            }
        }
    }
}
