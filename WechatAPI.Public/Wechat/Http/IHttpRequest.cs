namespace WechatAPI.Public.Wechat.Http
{
    public interface IHttpRequest
    {
        string GetUrl();
        string GetParamters();
        string Method { get; }
    }

    public interface IHttpResponse
    {
        string ReponseData { get; set; }
    }
}
