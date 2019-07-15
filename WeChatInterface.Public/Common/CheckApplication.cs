using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace WeChatInterface.Public.Common
{
    public class CheckApplication
    {
        public static bool AppHasAccess(string appId, string appSecret)
        {
            if (appId != "" && appSecret != "")
                return true;
            return false;
        }
    }
}