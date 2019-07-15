using System;
using WechatAPI.Public.Wechat.Api;

namespace WechatAPI.Public.Wechat.JSAPI_Ticket
{
    public class JsapiTicket
    {
        public string Ticket { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime ExpireTime { get; set; }
        public bool IsExpired { get { return DateTime.Now > ExpireTime; } }
        public JsapiTicket() { }
        public JsapiTicket(GetJsapiTicketResponse ticket)
        {
            Ticket = ticket.Ticket;
            StartTime = DateTime.Now;
            ExpireTime = DateTime.Now.AddSeconds(ticket.Expires_In);
        }
    }
}
