using System;
using WechatAPI.Public.Wechat.Api;

namespace WechatAPI.Public.Wechat.AccessToken
{
    public class AccessToken
    {
        public string Token { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime ExpireTime { get; set; }
        public bool IsExpired { get { return DateTime.Now > ExpireTime; } }
        public AccessToken() { }
        public AccessToken(GetTokenReponse token)
        {
            Token = token.Access_Token;
            StartTime = DateTime.Now;
            ExpireTime = DateTime.Now.AddSeconds(token.Expires_In);
        }
    }
}
