using System;
using System.Collections.Generic;
using System.Text;

namespace ESBasic.Arithmetic
{
    /// <summary>
    /// BinarySearcher 折半查找
    /// CQ，2008.12.13
    /// </summary>
    /// <typeparam name="TVal"></typeparam>
    public static class BinarySearcher<T> where T : System.IComparable
    {
        /// <summary>
        /// Search 返回的是目标值所在的索引，如果不存在则返回-1
        /// </summary>        
        public static int Search(IList<T> list, T value)
        {
            int left = 0;
            int right = list.Count;
            int middle;
            while (right >= left)
            {
                middle = (left + right) / 2;
                if (list[middle].CompareTo(value) == 0)
                {
                    return middle;
                }
                else if (list[middle].CompareTo(value) > 0)
                {
                    right = middle-1;
                }
                else
                {
                    left = middle+1;
                }
            }
            return -1;
        }
    }
}
