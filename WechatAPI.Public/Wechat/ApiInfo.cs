namespace WechatAPI.Public.Wechat
{
    public class ApiInfo
    {
        public const string ADDR_GETTOKEN = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={0}&secret={1}";
        public const string ADDR_OAUTH2_AUTHORIZE = "https://open.weixin.qq.com/connect/oauth2/authorize?appid={0}&redirect_uri={1}&response_type={2}&scope={3}&state={4}{5}";
        public const string ADDR_GETTOKENSNS = "https://api.weixin.qq.com/sns/oauth2/access_token?appid={0}&secret={1}&code={2}&grant_type=authorization_code";
        public const string ADDR_USER_GETUSERDETAILSNS = "https://api.weixin.qq.com/sns/userinfo?access_token={0}&openid={1}&lang=zh_CN";
        public const string ADDR_USER_GETUSERDETAIL = "https://api.weixin.qq.com/cgi-bin/user/info?access_token={0}&openid={1}";
        public const string ADDR_GET_JSAPI_TICKET = "https://api.weixin.qq.com/cgi-bin/ticket/getticket?access_token={0}&type=jsapi";
        public const string ADDR_GET_APPCODE = "https://api.weixin.qq.com/sns/jscode2session?appid={0}&secret={1}&js_code={2}&grant_type=authorization_code";
        public const string ADDR_SEND_TEMPLATE_MESSAGE = "https://api.weixin.qq.com/cgi-bin/message/wxopen/template/send?access_token={0}";

    }
}
