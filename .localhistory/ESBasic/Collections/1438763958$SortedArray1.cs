using System;
using System.Collections.Generic;
using System.Text;

namespace ESBasic.Collections
{
    /// <summary>
    /// SortedArray 有序的数组，其中Key是不允许重复的。如果单个添加重复的key，则将覆盖旧数据。如果是批添加出现重复，则批添加将全部失败。
    /// 该实现是线程安全的。
    /// </summary>
    [Serializable]
    public class SortedArray<TKey, TVal> : SortedArray2<TKey, TVal>, IComparer<TKey> where TKey : IComparable
    {
        public SortedArray()
        {
            base.comparer4Key = this;
        }

        public SortedArray(IDictionary<TKey, TVal> dictionary)            
        {
            base.comparer4Key = this;
            base.Rebuild(dictionary);
        }       

        #region IComparer<TKey> 成员

        public int Compare(TKey x, TKey y)
        {
            return x.CompareTo(y);
        }

        #endregion
    }
}
