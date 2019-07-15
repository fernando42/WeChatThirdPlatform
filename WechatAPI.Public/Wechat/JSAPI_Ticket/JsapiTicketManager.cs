using WechatAPI.Public.Cache;
using WechatAPI.Public.Wechat.Api;

namespace WechatAPI.Public.Wechat.JSAPI_Ticket
{
    public class JsapiTicketManager
    {
        private static IJsapiTicketRepository iJsapiTicketRepository = null;

        static object lockObj = new object();

        public static JsapiTicket GetJsapiTicket(bool isQueryAgin = false)
        {
            string cacheKey = GetJsapiTicketCacheKey();
            JsapiTicket ticket = CacheEx.Instance.Get<JsapiTicket>(cacheKey);
            if (!isQueryAgin && ticket != null && !ticket.IsExpired)
            {
                return ticket;
            }
            ticket = GetJsapiTicketByWx(cacheKey, isQueryAgin);
            return ticket;
        }


        private static JsapiTicket GetJsapiTicketByWx(string cacheKey, bool isQueryAgin = false)
        {
            lock (lockObj)
            {

                JsapiTicket ticket = CacheEx.Instance.Get<JsapiTicket>(cacheKey);
                if (!isQueryAgin && ticket != null && !ticket.IsExpired)
                {
                    return ticket;
                }

                var jsapiTicketRepository = GetJsapiTicketRepository();
                ticket = jsapiTicketRepository.Get();

                if (ticket == null || (ticket != null && ticket.IsExpired))
                {
                    var res = new GetJsapiTicketRequest().Request();
                    //如果报错，重新获取Accesstoken
                    if (res.ErrCode=="40001")
                    {
                        WechatAPI.Public.Wechat.AccessToken.AccessTokenManager.GetAccessToken(true);
                        res = new GetJsapiTicketRequest().Request();
                    }
                    if (res.IsSuccess)
                    {
                        ticket = new JsapiTicket(res);
                        CacheEx.Instance.Add(cacheKey, ticket, ticket.ExpireTime);
                        jsapiTicketRepository.Update(ticket);
                    }
                }
                else
                {
                    CacheEx.Instance.Add(cacheKey, ticket, ticket.ExpireTime);
                }

                return ticket;
            }
        }

        private static string GetJsapiTicketCacheKey()
        {
            return string.Format("CACHEKEY>>{0}>>{1}", WXCONFIG.WX_GLOBAL_APPID, WXCONFIG.WXDB_JSAPITICKET);
        }


        private static IJsapiTicketRepository GetJsapiTicketRepository()
        {
            return iJsapiTicketRepository ?? new DefaultJsapiTicketRepository();
        }
        public static void SetAccessTokenRepository(IJsapiTicketRepository iJsapiTicketRepository)
        {
            JsapiTicketManager.iJsapiTicketRepository = iJsapiTicketRepository;
        }
    }
}
