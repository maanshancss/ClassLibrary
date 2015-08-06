using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.Common
{
    public static class SoftDog
    {
        private const string KEY_64 = "Whp1sfz2";// 8 位
        private const string IV_64 = "VavicAppWhpCardLanwhping";//24位

        #region   对数据进行加密
        /// <summary>
        /// 对数据进行加密  这里不能包含汉字
        /// </summary>
        /// <param name="encryptstring">需要加密的数据</param>
        /// <returns></returns>
        public static string Encode(string data)
        {
            byte[] byKey = System.Text.ASCIIEncoding.ASCII.GetBytes(KEY_64);
            byte[] byIV = System.Text.ASCIIEncoding.ASCII.GetBytes(IV_64);

            DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
            int i = cryptoProvider.KeySize;
            MemoryStream ms = new MemoryStream();
            CryptoStream cst = new CryptoStream(ms, cryptoProvider.CreateEncryptor(byKey, byIV), CryptoStreamMode.Write);

            StreamWriter sw = new StreamWriter(cst);
            sw.Write(data);
            sw.Flush();
            cst.FlushFinalBlock();
            sw.Flush();
            return Convert.ToBase64String(ms.GetBuffer(), 0, (int)ms.Length);

        }

        #endregion


        #region   对数据进行解密
        /// <summary>
        /// 对数据进行解密
        /// </summary>
        /// <param name="decryptstring">需要解密的数据</param>
        /// <returns></returns>
        public static string Decode(string data)
        {
            byte[] byKey = System.Text.ASCIIEncoding.ASCII.GetBytes(KEY_64);
            byte[] byIV = System.Text.ASCIIEncoding.ASCII.GetBytes(IV_64);

            byte[] byEnc;
            try
            {
                byEnc = Convert.FromBase64String(data);
            }
            catch
            {
                return null;
            }

            try
            {
                DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
                MemoryStream ms = new MemoryStream(byEnc);
                CryptoStream cst = new CryptoStream(ms, cryptoProvider.CreateDecryptor(byKey, byIV), CryptoStreamMode.Read);
                StreamReader sr = new StreamReader(cst);
                return sr.ReadToEnd();
            }
            catch
            {
                return null;
            }

        }

        #endregion
        //
    }
}
