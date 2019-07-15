namespace WechatAPI.Public.Wechat.JSAPI_Ticket
{
    public interface IJsapiTicketRepository
    {
        JsapiTicket Get();
        bool Update(JsapiTicket ticket);
    }

    public class DefaultJsapiTicketRepository : IJsapiTicketRepository
    {
        public JsapiTicket Get()
        {
            return null;
        }

        public bool Update(JsapiTicket ticket)
        {
            return true;
        }
    }
}
