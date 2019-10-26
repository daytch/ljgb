using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ljgb.Infra.Options
{
    class LJGBSettings
    {
        static readonly NameValueCollection config = ConfigurationManager.GetSection("MarvelGroup/AxaSettings") as NameValueCollection;
        public static object GetValue(string key)
        {
            if (config.AllKeys.Contains(key))
            {
                return config[key];
            }
            return null;
        }

        public static string ApiServerUrl
        {
            get
            {
                return (string)GetValue("ApiServerUrl");
            }
        }

        public static string WebServerUrl
        {
            get
            {
                return (string)GetValue("WebServerUrl");
            }
        }


    }

    [Serializable]
    public static class MinioStorage
    {
        static readonly NameValueCollection config = ConfigurationManager.GetSection("MarvelGroup/MinioStorage") as NameValueCollection;
        public static object GetValue(string key)
        {
            if (config.AllKeys.Contains(key))
            {
                return config[key];
            }
            return null;
        }

        public static string Server
        {
            get
            {
                return (string)GetValue("Server");
            }
        }

        public static string Username
        {
            get
            {
                return (string)GetValue("Username");
            }
        }

        public static string Password
        {
            get
            {
                return (string)GetValue("Password");
            }
        }
    }

}
