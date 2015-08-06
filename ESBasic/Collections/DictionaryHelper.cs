using System;
using System.Collections.Generic;
using System.Text;

namespace ESBasic.Collections
{
    public static class DictionaryHelper
    {
        #region ConvertToDictionary
        /// <summary>
        /// ConvertToDictionary �������з��������Ķ�����ӵ��µ��ֵ��С�ͨ��func��ȡobject��Ӧ��Key
        /// </summary>       
        public static Dictionary<TKey, TObject> ConvertToDictionary<TKey, TObject>(IEnumerable<TObject> source, Func<TObject, TKey> func, Predicate<TObject> predicate)
        {
            Dictionary<TKey, TObject> dic = new Dictionary<TKey, TObject>();
            foreach (TObject obj in source)
            {
                if (predicate(obj))
                {
                    dic.Add(func(obj), obj);
                }
            }

            return dic;
        }

        /// <summary>
        /// ConvertToDictionary �������з��������Ķ�����ӵ��µ��ֵ��С�ͨ��func��ȡobject��Ӧ��Key
        /// </summary>  
        public static Dictionary<TKey, TObject> ConvertToDictionary<TKey, TObject>(IEnumerable<TObject> source, Func<TObject, TKey> func)
        {
            return DictionaryHelper.ConvertToDictionary<TKey, TObject>(source, func, delegate(TObject obj) { return true; });
        }
        #endregion

        #region RemoveOneByValue
        /// <summary>
        /// RemoveOneByValue ���ֵ���ɾ����һ��ֵ��val��ȵļ�¼
        /// </summary>      
        public static void RemoveOneByValue<TKey, TValue>(IDictionary<TKey, TValue> dic, TValue val)
            where TKey : class
            where TValue : IEquatable<TValue>
        {
            TKey dest = DictionaryHelper.GetOneByValue(dic, val);
            if (dest != null)
            {
                dic.Remove(dest);
            }
        }
        #endregion

        #region GetOneByValue
        /// <summary>
        /// GetOneByValue ���ֵ����ҳ���һ��ֵ��val��ȵļ�¼��key
        /// </summary>      
        public static TKey GetOneByValue<TKey, TValue>(IDictionary<TKey, TValue> dic, TValue val)
            where TKey : class
            where TValue : IEquatable<TValue>
        {

            return CollectionHelper.FindFirstSpecification<TKey>(dic.Keys, delegate(TKey cur) { return dic[cur].Equals(val); });
        }
        #endregion
    }
}
