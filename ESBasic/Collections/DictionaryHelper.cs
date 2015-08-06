using System;
using System.Collections.Generic;
using System.Text;

namespace ESBasic.Collections
{
    public static class DictionaryHelper
    {
        #region ConvertToDictionary
        /// <summary>
        /// ConvertToDictionary 将集合中符合条件的对象添加到新的字典中。通过func获取object对应的Key
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
        /// ConvertToDictionary 将集合中符合条件的对象添加到新的字典中。通过func获取object对应的Key
        /// </summary>  
        public static Dictionary<TKey, TObject> ConvertToDictionary<TKey, TObject>(IEnumerable<TObject> source, Func<TObject, TKey> func)
        {
            return DictionaryHelper.ConvertToDictionary<TKey, TObject>(source, func, delegate(TObject obj) { return true; });
        }
        #endregion

        #region RemoveOneByValue
        /// <summary>
        /// RemoveOneByValue 从字典中删除第一个值与val相等的记录
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
        /// GetOneByValue 从字典中找出第一个值与val相等的记录的key
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
