using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;

namespace ESBasic.Security
{
    public static class SecurityHelper
    {
        #region MD5Password
        /// <summary>
        /// MD5Password 对字符串进行MD5摘要计算。
        /// </summary>      
        public static string MD5String(string pwd)
        {
            return SecurityHelper.MD5String(pwd, Encoding.UTF8);
        }

        public static string MD5String(string pwd, Encoding encoding)
        {
            byte[] origin = encoding.GetBytes(pwd);
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] result = md5.ComputeHash(origin);

            StringBuilder strBuilder = new StringBuilder("");
            for (int i = 0; i < result.Length; i++)
            {
                strBuilder.Append(result[i].ToString("x")) ;
            }

            return strBuilder.ToString();                
        }

        /// <summary>
        /// MD5Password 对字符串进行MD5摘要计算。
        /// </summary>      
        public static string MD5String2(string pwd)
        {
            return SecurityHelper.MD5String2(pwd, Encoding.UTF8);
        }

        public static string MD5String2(string pwd, Encoding encoding)
        {
            byte[] origin = encoding.GetBytes(pwd);
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] result = md5.ComputeHash(origin);

            StringBuilder strBuilder = new StringBuilder("");
            for (int i = 0; i < result.Length; i++)
            {
                string ss = result[i].ToString("x");
                if (ss.Length == 1)
                {
                    ss = "0" + ss;
                }
                strBuilder.Append(ss);
            }

            return strBuilder.ToString();
        } 
        #endregion
    }
}
