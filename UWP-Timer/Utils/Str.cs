using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace UWP_Timer.Utils
{
    public static class Str
    {

        public static string Studly(string val)
        {
            var data = val.Split('-', '_', ' ');
            var res = new StringBuilder();
            foreach (var item in data)
            {
                res.Append(item.Substring(0, 1).ToUpper());
                res.Append(item.Substring(1).ToLower());
            }
            return res.ToString();
        }

        public static string UnStudly(string val)
        {
            var res = new StringBuilder();
            for (int i = 0; i < val.Length; i++)
            {
                var code = val[i];
                if (code < 65 || code > 90)
                {
                    res.Append(code);
                    continue;
                }
                if (i > 0)
                {
                    res.Append('_');
                }
                res.Append((char)(code + 32));

            }
            return res.ToString();
        }

        public static string Base64Encode(string val)
        {
            var bytes = Encoding.UTF8.GetBytes(val);
            return Convert.ToBase64String(bytes);
        }

        public static string Base64Decode(string val)
        {
            var bytes = Convert.FromBase64String(val);
            return Encoding.UTF8.GetString(bytes);
        }

        public static string MD5Encode(string source)
        {
            var sor = Encoding.UTF8.GetBytes(source);
            var md5 = MD5.Create();
            var result = md5.ComputeHash(sor);
            md5.Dispose();
            var strbul = new StringBuilder(40);
            for (int i = 0; i < result.Length; i++)
            {
                strbul.Append(result[i].ToString("x2"));//加密结果"x2"结果为32位,"x3"结果为48位,"x4"结果为64位

            }
            return strbul.ToString();
        }
    }
}
