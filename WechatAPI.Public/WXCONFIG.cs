namespace WechatAPI.Public
{
    /// <summary>
    /// 配置
    /// </summary>
    public static class WXCONFIG
    {
        //public static string WX_GLOBAL_APPID = GET("WX.GLOBAL.APPID"); //APPID

        //public static string WX_GLOBAL_SECRET = GET("WX.GLOBAL.SECRET"); //凭证密钥

        public static string WX_GLOBAL_APPID = ""; //APPID

        public static string WX_GLOBAL_SECRET = ""; //凭证密钥

        public static string WXDB_ACCESSTOKEN = "WX.ACCESSTOKEN"; //token

        public static string WXDB_JSAPITICKET = "JSAPITicket"; //JSAPITicket




        #region  METHOD
        public static int GETINT(string key)
        {
            string v = GET(key) ?? "0"; int vi = 0;
            int.TryParse(v, out vi);
            return vi;
        }
        public static string GET(string key)
        {
            return System.Configuration.ConfigurationManager.AppSettings[key];
        }
        #endregion

    }
}
