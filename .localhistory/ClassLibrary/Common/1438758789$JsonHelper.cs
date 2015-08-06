using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.Common
{
    public static class JsonHelper
    {
        /// <summary>
        /// 转换对象为JSON格式数据
        /// </summary>
        /// <typeparam name="T">类</typeparam>
        /// <param name="obj">对象</param>
        /// <returns>字符格式的JSON数据</returns>
        public static string ToJson<T>(object obj)
        {
            string result = String.Empty;
            System.Runtime.Serialization.Json.DataContractJsonSerializer serializer =
            new System.Runtime.Serialization.Json.DataContractJsonSerializer(typeof(T));
            using (System.IO.MemoryStream memoryStream = new System.IO.MemoryStream())
            {
                serializer.WriteObject(memoryStream, obj);
                result = System.Text.Encoding.UTF8.GetString(memoryStream.ToArray());
            }
            return result;
        }
        /// <summary>
        /// 转换List<T>的数据为JSON格式
        /// </summary>
        /// <typeparam name="T">类</typeparam>
        /// <param name="vals">列表值</param>
        /// <returns>JSON格式数据</returns>
        public static string ToJson<T>(List<T> lstData)
        {
            System.Text.StringBuilder strBuilder = new System.Text.StringBuilder();
            System.Runtime.Serialization.Json.DataContractJsonSerializer serializer =
                new System.Runtime.Serialization.Json.DataContractJsonSerializer(typeof(T));

            foreach (T city in lstData)
            {
                using (System.IO.MemoryStream memoryStream = new System.IO.MemoryStream())
                {
                    serializer.WriteObject(memoryStream, city);
                    strBuilder.Append(System.Text.Encoding.UTF8.GetString(memoryStream.ToArray()));
                }
            }

            return strBuilder.ToString();
        }

        /// <summary>
        /// JSON格式字符转换为T类型的对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonStr"></param>
        /// <returns></returns>
        public static T FormJson<T>(string jsonStr)
        {
            T obj = Activator.CreateInstance<T>();
            using (System.IO.MemoryStream memoryStream = new System.IO.MemoryStream(System.Text.Encoding.UTF8.GetBytes(jsonStr)))
            {
                System.Runtime.Serialization.Json.DataContractJsonSerializer serializer =
                new System.Runtime.Serialization.Json.DataContractJsonSerializer(typeof(T));
                return (T)serializer.ReadObject(memoryStream);
            }
        }
    }
}
