using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace ExoWebApi.Web.Helpers
{
    public static class ConfigHelper
    {
        public static string AccessKey => ConfigurationManager.AppSettings["AccessKey"];
        public static string SecretKey => ConfigurationManager.AppSettings["SecretKey"];
    }
}